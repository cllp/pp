using PP.Core;
using PP.Core.Interfaces;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;

namespace PP.Api.Filters
{
    public class ApiExceptionFilter : ExceptionFilterAttribute
    {
        private ILog log = CoreFactory.Log;

        public override void OnException(HttpActionExecutedContext context)
        {
            var ex = context.Exception;
            log.Error(string.Format("Exception ({0})", ex.GetType().Name), context.Exception);
            var statusCode = GetStatusCode(ex);
            context.Response = context.Request.CreateResponse(GetStatusCode(ex), GetMessage(ex));
        }

        private HttpStatusCode GetStatusCode(Exception ex)
        {
            switch (ex.GetType().Name)
            {
                case "NotImplementedException":
                        return HttpStatusCode.NotImplemented;
                case "UnauthorizedAccessException":
                        return HttpStatusCode.Unauthorized;
                default:
                    return HttpStatusCode.InternalServerError;
            }
        }

        private string GetMessage(Exception ex)
        {
            return ex.Message;
        }
    }
}