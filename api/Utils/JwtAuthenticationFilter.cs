using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Threading;
using System.Web.Http.Filters;
using System.Net.Http;
using System.Web.Http;
using NLog;

namespace api.Utils
{

    public class JwtAuthenticationFilter : IAuthenticationFilter
    {
        private readonly TokenValidationParameters _validationParameters;
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();


        public JwtAuthenticationFilter(TokenValidationParameters validationParameters)
        {
            _validationParameters = validationParameters ?? throw new ArgumentNullException(nameof(validationParameters));
        }

        public bool AllowMultiple => false;

        public Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            var descriptor = context.ActionContext.ActionDescriptor;
            bool allowAnonymous = descriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any() ||
                                  descriptor.ControllerDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any();

            if (allowAnonymous)
            {
                return Task.CompletedTask;
            }


            var token = context.Request.Headers.Authorization?.Parameter;

            if (token == null)
            {
                context.ErrorResult = new AuthenticationFailureResult("Missing token", context.Request);
                logger.Error($"authentication error missing token");
                return Task.CompletedTask;
            }

            try
            {
                var handler = new JwtSecurityTokenHandler();
                SecurityToken validatedToken;
                ClaimsPrincipal claimsPrincipal = handler.ValidateToken(token, _validationParameters, out validatedToken);

                context.Principal = claimsPrincipal;
            }
            catch (Exception ex)
            {
                context.ErrorResult = new AuthenticationFailureResult("Invalid token", context.Request);
                logger.Error(ex,"authentication error");
            }

            return Task.CompletedTask;
        }

        public Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }

}