using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using SCERP.Common;

namespace SCERP.Web.Controllers
{
    public class ScriptController : BaseController
    {
        //
        // GET: /Script/
        public JavaScriptResult Index()
        {
            var sb = new StringBuilder();
            string format = "var {0} = \"{1}\";\n";
            sb.AppendFormat(format, "UploadSizeLimit", ((Int32)AppConfig.MaxUploadFileSizeMB * 1048576));
            sb.AppendFormat(format, "ASPSESSID", Session.SessionID);
            sb.AppendFormat(format, "AUTHID", Request.Cookies[FormsAuthentication.FormsCookieName].Value);

            return JavaScript(sb.ToString());
        }
	}
}