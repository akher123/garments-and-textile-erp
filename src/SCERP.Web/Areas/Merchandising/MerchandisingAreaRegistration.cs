using System.Web.Mvc;

namespace SCERP.Web.Areas.Merchandising
{
    public class MerchandisingAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Merchandising";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Merchandising_default",
                "Merchandising/{controller}/{action}/{id}",
                new {controller="Buyer", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}