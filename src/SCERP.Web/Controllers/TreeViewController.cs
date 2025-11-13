using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCERP.BLL.IManager.IUserManagementManager;
using SCERP.BLL.Manager.UserManagementManager;
using SCERP.Common;
using SCERP.Model;
using SCERP.Web.Areas.UserManagement.Controllers;
using SCERP.Web.Helpers;

namespace SCERP.Web.Controllers
{
    public class TreeViewController : Controller
    {
       private readonly IModuleFeatureManager moduleFeatureManager;
        public TreeViewController(IModuleFeatureManager moduleFeatureManager)
        {
            this.moduleFeatureManager = moduleFeatureManager;
        }
        //
        // GET: /Treeview/
        public ActionResult Index()
        {
            return View();
        }
        [ChildActionOnly]
        public PartialViewResult Simple()
        {
            var all = moduleFeatureManager.GetModuleFeaturesByUser(PortalContext.CurrentUser.Name);           
            return PartialView("_Menu", all);         
        }
	}
}