using System;
using System.Web.Mvc;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.Common;
using SCERP.Model;
using SCERP.Web.Areas.Merchandising.Models.ViewModel;
using SCERP.Web.Controllers;

namespace SCERP.Web.Areas.Merchandising.Controllers
{
    public class CostDefinationController : BaseController
    {
        private readonly ICostDefinationManager _costDefinationManager;

        public CostDefinationController(ICostDefinationManager costDefinationManager)
        {
            this._costDefinationManager = costDefinationManager;
        }

        [AjaxAuthorize(Roles = "costdefinition-1,costdefinition-2,costdefinition-3")]
        public ActionResult Index(CostDefinationViewModel model)
        {
            ModelState.Clear();
            var totalRecords = 0;
            model.CostDefinations = _costDefinationManager.GetCostDefinationPaging(model, out totalRecords);
            model.TotalRecords = totalRecords;
            return View(model);
        }
        [AjaxAuthorize(Roles = "costdefinition-2,costdefinition-3")]
        public ActionResult Edit(CostDefinationViewModel model)
        {
            ModelState.Clear();
            if (model.CostDefinationId > 0)
            {
                var costDefination = _costDefinationManager.GetCostDefinationById(model.CostDefinationId);
                model.CostDefinationId = costDefination.CostDefinationId;
                model.CostName = costDefination.CostName;
                model.CostRefId = costDefination.CostRefId;
                model.CostGroup = costDefination.CostGroup;
            }
            else
            {
                model.CostRefId = _costDefinationManager.GetNewCostRefId();
            }
            return View(model);
        }
       [HttpPost]
       [AjaxAuthorize(Roles = "costdefinition-2,costdefinition-3")]
        public ActionResult Save(OM_CostDefination model)
        {
            var index = 0;
            var errorMessage = "";
            try
            {
              index = model.CostDefinationId > 0 ? _costDefinationManager.EditCostDefination(model) : _costDefinationManager.SaveCostDefination(model);
            }
            catch (Exception exception)
            {
                errorMessage = exception.Message;
                Errorlog.WriteLog(exception);
            }
            return index > 0 ? Reload() : ErrorResult("Style save fail " + errorMessage);

        }

        [AjaxAuthorize(Roles = "costdefinition-3")]
        public ActionResult Delete(string costRefId)
        {
            var saveIndex = _costDefinationManager.DeleteCostDefination(costRefId);
            if (saveIndex == -1)
            {
                return ErrorResult("Could not possible to delete Style because of it's all ready used in Style Cost");
            }
            return saveIndex > 0 ? Reload() : ErrorResult("Delate Fail");
        }
        [HttpPost]
        public JsonResult CheckExistingCostDefination(OM_CostDefination model)
        {
            var isExist = !_costDefinationManager.CheckExistingCostDefination(model);
            return Json(isExist, JsonRequestBehavior.AllowGet);
        }
	}
}