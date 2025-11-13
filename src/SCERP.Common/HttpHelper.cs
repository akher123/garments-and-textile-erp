using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.SessionState;

namespace SCERP.Common
{
    public static class HttpHelper
    {
        public static HttpContext Context
        {
            get { return HttpContext.Current; }
        }

        public static HttpServerUtility Server
        {
            get { return Context.Server; }
        }

        public static HttpSessionState Session
        {
            get { return Context.Session; }
        }

        public static HttpRequest Request
        {
            get { return Context.Request; }
        }

        public static Exception LastError
        {
            get { return Server.GetLastError(); }
        }

        public static string MapPath(this string virtualPath)
        {
            return Server.MapPath(virtualPath);
        }

        public static string HtmlEncode(this string value)
        {
            return HttpUtility.HtmlEncode(value);
        }

        public static string HtmlDecode(this string value)
        {
            return HttpUtility.HtmlDecode(value);
        }
    }
}
