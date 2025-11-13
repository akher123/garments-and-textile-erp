using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace SCERP.API
{
    public class GlobalException : IHttpActionResult
    {
        public HttpRequestMessage Request { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public string Message { get; set; }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            ErrorMessage message = new ErrorMessage { Message = Message };
            HttpResponseMessage response = Request.CreateResponse(StatusCode, message);

            return Task.FromResult(response);
        }
    }
}