using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Net.Http.Headers;
using System.Web.Http.ExceptionHandling;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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

            var cors = new EnableCorsAttribute("*", "*", "*");
            config.EnableCors(cors);

            ConfigureOAuthTokenGeneration(config);

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }

            );

        }

        private static void ConfigureOAuthTokenGeneration(HttpConfiguration config)
        {
            var issuer = "your_issuer";
            var audience = "your_audience";
            var key = Encoding.ASCII.GetBytes("<f?i^vfa1@H?ysc8(D0u6uCz]?3x5*e");

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
