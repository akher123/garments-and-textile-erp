using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCERP.Web.Areas.Common.Models.ViewModel;
using SCERP.Web.Controllers;

namespace SCERP.Web.Areas.Common.Controllers
{
    public class UserReportController : BaseController
    {
     
    
        public ActionResult Index(CustomSqlQuaryViewModel model)
        {

        //    model.VUserReports = CustomSqlQuaryManager.GetUserReportList();
            return View();
        }
	}
}