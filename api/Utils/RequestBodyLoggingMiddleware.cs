using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace api.Utils
{
    public class RequestBodyLoggingMiddleware : DelegatingHandler
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
        {
            if (request.Content != null)
            {
                var requestBody = await request.Content.ReadAsStringAsync();
                if (IsLoginOrRegisterRequest(request.RequestUri.ToString()))
                {
                    requestBody = MaskPassword(requestBody);
                }

                if(!string.IsNullOrWhiteSpace(requestBody))
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

        private string MaskPassword(string requestBody)
        {
            var maskedRequestBody = requestBody;
            var passwordKey = "\"password\"";
            var passwordIndex = requestBody.IndexOf(passwordKey, StringComparison.OrdinalIgnoreCase);

            if (passwordIndex >= 0)
            {
                var startIndex = passwordIndex + passwordKey.Length + 1;
                var endIndex = requestBody.IndexOf('"', startIndex + 1);

                if (endIndex > startIndex)
                {
                    var password = requestBody.Substring(startIndex, endIndex - startIndex);
                    maskedRequestBody = requestBody.Replace(password, "****");
                }
            }

            return maskedRequestBody;
        }
    }
}