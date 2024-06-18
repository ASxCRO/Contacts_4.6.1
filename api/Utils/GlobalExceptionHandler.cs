using System.Net;
using System.Net.Http;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Results;
using NLog;

namespace api.Utils
{
    public class GlobalExceptionHandler : ExceptionHandler
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public override void Handle(ExceptionHandlerContext context)
        {
            logger.Error(context.Exception, "Error in axinas.contacts.webapi");

            var response = context.Request.CreateResponse(HttpStatusCode.InternalServerError,
                                                          new { Message = "Greška na serveru, pokušajte kasnije" });

            response.Headers.Add("X-Error", context.Exception.Message);

            context.Result = new ResponseMessageResult(response);
        }

        public override bool ShouldHandle(ExceptionHandlerContext context)
        {
            return true;
        }
    }
}

