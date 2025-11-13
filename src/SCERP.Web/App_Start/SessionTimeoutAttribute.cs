using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.Mvc;

namespace SCERP.Web.App_Start
{
    public class SessionTimeoutAttribute :ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpContext ctx = HttpContext.Current;
            if (HttpContext.Current.Session["PortalUser"]== null)
            {
                filterContext.Result = new RedirectResult("~/Home/LogOff");
                return;
            }
            base.OnActionExecuting(filterContext);
        }
    }
}