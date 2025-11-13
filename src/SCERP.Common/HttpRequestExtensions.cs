using System.Web;

namespace SCERP.Common
{
    public static class HttpRequestExtensions
    {
        public static string GetCompanyId(this HttpRequest request)
        {
            string value = request.Path.Split('/')[2];

            if (value == "undefined")
            {
                value = null;
            }

            return value;
        }
    }
}
