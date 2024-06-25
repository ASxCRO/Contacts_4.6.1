using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Net.Http.Headers;
using System.Web.Http.ExceptionHandling;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using api.Utils;
using System.Configuration;

namespace api
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Services.Replace(typeof(IExceptionHandler), new Utils.GlobalExceptionHandler());

            config.Formatters.JsonFormatter.SerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(), 
                DateTimeZoneHandling = DateTimeZoneHandling.Utc
            };

            config.Formatters.Remove(config.Formatters.XmlFormatter);

            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/octet-stream"));

            var cors = new EnableCorsAttribute(
                ConfigurationManager.AppSettings["cors_origins"],
                ConfigurationManager.AppSettings["cors_headers"],
                ConfigurationManager.AppSettings["cors_methods"]);

            config.EnableCors(cors);

            config.MessageHandlers.Add(new RequestBodyLoggingMiddleware());

            ConfigureOAuthTokenGeneration(config);

            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }

            );
        }

        private static void ConfigureOAuthTokenGeneration(HttpConfiguration config)
        {
            var issuer = ConfigurationManager.AppSettings["jwt_issuer"];
            var audience = ConfigurationManager.AppSettings["jwt_audience"];
            var key = Encoding.ASCII.GetBytes(ConfigurationManager.AppSettings["jwt_key"]);

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateIssuerSigningKey = true,
                ValidIssuer = issuer,
                ValidAudience = audience,
                IssuerSigningKey = new SymmetricSecurityKey(key)
            };

            config.Filters.Add(new Utils.JwtAuthenticationFilter(tokenValidationParameters));
        }
    }
}
