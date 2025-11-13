using System;
using System.Web.Mvc;
using SCERP.Common;
using SCERP.Model;
using SCERP.Web.Areas.Inventory.Models.ViewModels;

namespace SCERP.Web.Areas.Inventory.Controllers
{
    public class InventoryApprovalStatusController : BaseInventoryController
    {
        [AjaxAuthorize(Roles = "inventoryapprovalstatus-1,inventoryapprovalstatus-2,inventoryapprovalstatus-3")]
        public ActionResult Index(InventoryApprovalStatusViewModel model)
        {
            ModelState.Clear();
            var totalRecords = 0;
            try
            {
                model.StatusName = model.StatusName;
                model.InventoryApprovalStatuses = InventoryApprovalStatusManager.GetInventoryApprovalStatusByPaging(model, out totalRecords);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            model.TotalRecords = totalRecords;
            return View(model);
        }
        [AjaxAuthorize(Roles = "inventoryapprovalstatus-2,inventoryapprovalstatus-3")]
        public ActionResult Edit(InventoryApprovalStatusViewModel model)
        {
            ModelState.Clear();
            try
            {
                if (model.ApprovalStatusId > 0)
                {
                    Inventory_ApprovalStatus approvalStatus = InventoryApprovalStatusManager.GetInventoryApprovalStatusById(model.ApprovalStatusId);
                    model.StatusName = approvalStatus.StatusName;
                    model.Description = approvalStatus.Description;
                }
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return View(model);
        }

        [AjaxAuthorize(Roles = "inventoryapprovalstatus-2,inventoryapprovalstatus-3")]
        public ActionResult Save(Inventory_ApprovalStatus model)
        {
            var saveIndex = 0;
            try
            {
                saveIndex = model.ApprovalStatusId > 0 ? InventoryApprovalStatusManager.EditInventoryApprovalStatus(model) : InventoryApprovalStatusManager.SaveInventoryApprovalStatus(model);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return (saveIndex > 0) ? Reload() : ErrorResult(String.Format("Failed to save {0}", model.StatusName));
        }

        [AjaxAuthorize(Roles = "inventoryapprovalstatus-3")]
        public ActionResult Delete(Inventory_ApprovalStatus approvalStatus)
        {
            var deleteIndex = 0;
            try
            {
                deleteIndex = InventoryApprovalStatusManager.DeleteInventoryApprovalStatus(approvalStatus.ApprovalStatusId);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return (deleteIndex > 0) ? Reload() : ErrorResult("Failed to delete Brand");

        }


        public JsonResult CheckApprovalStatus(Inventory_ApprovalStatus model)
        {
            bool isExist = !InventoryApprovalStatusManager.IsExistApprovalStatus(model);
            return Json(isExist, JsonRequestBehavior.AllowGet);
        }
    }
}