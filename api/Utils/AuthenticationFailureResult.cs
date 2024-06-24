using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;
using System.Web.Http;

namespace api.Utils
{
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