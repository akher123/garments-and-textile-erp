using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCERP.Model.Planning;
using SCERP.Web.Areas.Planning.Models.ViewModels;
using SCERP.Web.Controllers;
using SCERP.Common;
using SCERP.BLL.IManager.IPlanningManager;

namespace SCERP.Web.Areas.Planning.Controllers
{
    public class TNATemplateController : BaseController
    {
        private readonly int _pageSize = 50;

        private readonly ITNAManager TnaManager;
        public TNATemplateController(ITNAManager TnaManager)
        {
            this.TnaManager = TnaManager;
        }

        public ActionResult Index(TNATemplateViewModel model)
        {
            ModelState.Clear();

            PLAN_Activity activity = model;

            var startPage = 0;
            if (model.page.HasValue && model.page.Value > 0)
            {
                startPage = model.page.Value - 1;
            }

            var totalRecords = 0;
            model.Activities = TnaManager.GetAllTnaTemplateByPaging(startPage, _pageSize, out totalRecords, activity) ?? new List<PLAN_Activity>();
            model.TotalRecords = totalRecords;
            return View(model);
        }

        [HttpPost]
        public JsonResult Save(List<string> values)
        {
            ModelState.Clear();
            var result = TnaManager.SaveTNATemplate(values);
            return Json(new {Success = true, Result = result});
        }
    }
}