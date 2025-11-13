using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace SCERP.Web.App_Start
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
        
    }
 
}
