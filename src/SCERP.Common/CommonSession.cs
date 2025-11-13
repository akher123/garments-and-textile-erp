using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.SessionState;

namespace SCERP.Common
{
    public class CommonSession
    {
        private static HttpSessionState Session
        {
            get { return HttpHelper.Session; }
        }

        public static object GetValue(string key)
        {
            return Session[key];

        }
        public static object SetValue(string key, object value)
        {
            return Session[key] = value;

        }
    }
}
