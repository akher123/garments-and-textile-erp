using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Routing;

namespace SCERP.Web.App_Start
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapMvcAttributeRoutes();

            routes.MapRoute(
                name: "Script",
                url: "Scripts/scerp.constant.js",
                defaults: new { controller = "Script", action = "Index" },
               namespaces: new[] { "SCERP.Web.Controllers" }
            );


            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
               namespaces: new[] { "SCERP.Web.Controllers" }
            );

        }
    }
}
