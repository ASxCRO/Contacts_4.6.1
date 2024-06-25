using NLog;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Configuration;

namespace api.Utils
{
    public class RequestBodyLoggingMiddleware : DelegatingHandler
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly string _maskingPattern;

        public RequestBodyLoggingMiddleware()
        {
            _maskingPattern = ConfigurationManager.AppSettings["masking_pattern"];
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
        {
            if (request.Content != null)
            {
                var requestBody = await request.Content.ReadAsStringAsync();
                if (IsLoginOrRegisterRequest(request.RequestUri.ToString()))
                {
                    requestBody = MaskSensitiveData(requestBody);
                }

                if (!string.IsNullOrWhiteSpace(requestBody))
                {
                    logger.Info($"{requestBody}");
                }
            }

            return await base.SendAsync(request, cancellationToken);
        }

        private bool IsLoginOrRegisterRequest(string url)
        {
            return url.Contains("login") || url.Contains("register");
        }

        private string MaskSensitiveData(string requestBody)
        {
            if (string.IsNullOrEmpty(_maskingPattern))
            {
                return requestBody;
            }

            return Regex.Replace(requestBody, _maskingPattern, "\"password\":\"******\"", RegexOptions.IgnoreCase);
        }
    }
}
