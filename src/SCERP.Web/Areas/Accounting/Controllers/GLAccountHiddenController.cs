using System;
using System.Web.Mvc;
using SCERP.Common;
using SCERP.Model;
using SCERP.Web.Areas.Accounting.Models.ViewModels;
using SCERP.Model.AccountingModel;

namespace SCERP.Web.Areas.Accounting.Controllers
{
    public class GLAccountHiddenController : BaseAccountingController
    {

        private readonly int _pageSize = AppConfig.PageSize;
        private Guid _employeeGuidId = new Guid();

        public ActionResult Index(int? page, string sort, GLAccountHiddenViewModel model)
        {
            var startPage = 0;
            var totalRecordsHidden = 0;
            var totalRecordsVisible = 0;


            if (model.page.HasValue && model.page.Value > 0)
            {
                startPage = model.page.Value - 1;
            }

            model.GLAccountHidden = GLAccountHiddenManager.GetAllGLAccountHiddens(startPage, _pageSize, out totalRecordsHidden);
            model.GLAccountVisible = GLAccountHiddenManager.GetAllGLAccountVisibles(startPage, _pageSize, out totalRecordsVisible);

            model.totalRecordsHidden = totalRecordsHidden;
            model.totalRecordsVisible = totalRecordsVisible;
            
            return View(model);
        }

        public ActionResult Edit(Acc_GLAccounts_Hidden model)
        {
            ModelState.Clear();

            if (Request.IsAjaxRequest())
            {
                return PartialView(model);
            }
            return View(model);
        }

        public ActionResult Save(GLAccountHiddenViewModel model)
        {
            if (Session["EmployeeGuid"] != null)
            {
                _employeeGuidId = (Guid)Session["EmployeeGuid"];
            }

            var glHidden = new Acc_GLAccounts_Hidden();

            glHidden.AccountName = model.AccountName;

            string message = GLAccountHiddenManager.SaveGLAccountHidden(glHidden);

            return CreateJsonResult(new { Success = false, Reload = true, Message = message });
        }

        public ActionResult SaveStatus(bool status)
        {
            string message = "";
            message = GLAccountHiddenManager.SaveStatus(status);
            return CreateJsonResult(new { Success = false, Reload = true, Message = message });
        }

        public JsonResult GetStatus()
        {
            int result = 2;
            result = GLAccountHiddenManager.GetStatus();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Delete(int id)
        {
            var glAccount = GLAccountHiddenManager.GetGLAccountHiddenById(id) ?? new Acc_GLAccounts_Hidden();
            GLAccountHiddenManager.DeleteGLAccountHidden(glAccount);
            return Reload();
        }

        public ActionResult MakeHidden(int id)
        {
            var glAccount = GLAccountHiddenManager.GetGLAccountHiddenById(id) ?? new Acc_GLAccounts_Hidden();
            GLAccountHiddenManager.MakeGLAccountHidden(glAccount);
            return Reload();
        }

        public ActionResult MakeVisible(int id)
        {
            var glAccount = GLAccountHiddenManager.GetGLAccountHiddenById(id) ?? new Acc_GLAccounts_Hidden();
            GLAccountHiddenManager.MakeGLAccountVisible(glAccount);
            return Reload();
        }    
    }
}