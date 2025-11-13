using System.Net;

namespace SCERP.API
{
    public class GlobalExceptionHandler : ExceptionHandler
    {
        public override void HandleCore(System.Web.Http.ExceptionHandling.ExceptionHandlerContext context)
        {
            HttpStatusCode statusCode = HttpStatusCode.BadRequest;
            string message = "Internal exception has occured. Please contact with administrator.";

            try
            {
                if (context.Exception.HResult > 1)
                {
                    statusCode = (HttpStatusCode)context.Exception.HResult;
                }
            }
            catch
            {
                statusCode = HttpStatusCode.BadRequest;
            }

            if (!string.IsNullOrWhiteSpace(context.Exception.Message))
            {
                message = context.Exception.Message;
            }

            context.Result = new GlobalException()
            {
                StatusCode = statusCode,
                Message = message,
                Request = context.Request
            };
        }
    }
}