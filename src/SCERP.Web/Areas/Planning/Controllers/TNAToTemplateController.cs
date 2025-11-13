using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCERP.Web.Areas.Planning.Models.ViewModels;
using SCERP.Web.Controllers;
using SCERP.Common;
using SCERP.BLL.IManager.IPlanningManager;

namespace SCERP.Web.Areas.Planning.Controllers
{
    public class TNAToTemplateController : BaseController
    {
        private readonly ITNAManager TnaManager;

        public TNAToTemplateController(ITNAManager TnaManager)
        {
            this.TnaManager = TnaManager;
        }

        public ActionResult Index(TNATemplateViewModel model)
        {
            ModelState.Clear();
            try
            {
                ViewBag.TemplateId = new SelectList(new[] {new {Id = 1, Name = "Template 1"}}, "Id", "Name");
                model.Styles = TnaManager.GetAllStyleNames();
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return View(model);
        }

        public ActionResult Save(TNATemplateViewModel model)
        {
            try
            {
                string stylerefId = "";
                int templateId = model.TemplateId;
               
                if (model.OrderStyleRefId != null) stylerefId = TnaManager.GetOrderStyleRefIdByStyleName(model.OrderStyleRefId);

                TnaManager.ProcessTNAToTemplate(templateId, stylerefId);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return View();
        }
    }
}