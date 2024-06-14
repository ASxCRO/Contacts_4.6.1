using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Threading;
using System.Web;
using System.Web.Http.Filters;
using System.Net.Http;
using System.Web.Http;

namespace api.Utils
{

    public class JwtAuthenticationFilter : IAuthenticationFilter
    {
        private readonly TokenValidationParameters _validationParameters;

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
                // No token provided
                context.ErrorResult = new AuthenticationFailureResult("Missing token", context.Request);
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
                // Token validation failed
                context.ErrorResult = new AuthenticationFailureResult("Invalid token", context.Request);
            }

            return Task.CompletedTask;
        }

        public Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }

    public class AuthenticationFailureResult : IHttpActionResult
    {
        private string _reason;
        private HttpRequestMessage _request;

        public AuthenticationFailureResult(string reason, HttpRequestMessage request)
        {
            _reason = reason;
            _request = request;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            var response = new HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized)
            {
                RequestMessage = _request,
                ReasonPhrase = _reason
            };
            return Task.FromResult(response);
        }
    }
}