using System;
using System.Web.Mvc;
using SCERP.Common;
using SCERP.Model;
using SCERP.Web.Areas.Inventory.Models.ViewModels;

namespace SCERP.Web.Areas.Inventory.Controllers
{
    public class RequisitionTypeController : BaseInventoryController
    {
        [AjaxAuthorize(Roles = "requisitiontype-1,requisitiontype-2,requisitiontype-3")]
        public ActionResult Index(RequisitiontypeViewModel model)
        {
            ModelState.Clear();
            var totalRecords = 0;
            try
            {
                model.Title = model.RequsitionTypeTitle;
                model.InventoryRequsitionTypes = RequisitiontypeManager.GetRquisitiontypesByPaging(model, out totalRecords);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            model.TotalRecords = totalRecords;
            return View(model);
        }

        [AjaxAuthorize(Roles = "requisitiontype-2,requisitiontype-3")]
        public ActionResult Edit(RequisitiontypeViewModel model)
        {
            ModelState.Clear();
            try
            {
                if (model.RequisitionTypeId > 0)
                {
                    Inventory_RequsitionType requsitionType = RequisitiontypeManager.GetRquisitiontypeById(model.RequisitionTypeId);
                    model.Title = requsitionType.Title;
                    model.Description = requsitionType.Description;
                }
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return View(model);
        }

        [AjaxAuthorize(Roles = "requisitiontype-2,requisitiontype-3")]
        public ActionResult Save(Inventory_RequsitionType model)
        {

            try
            {
                ResponsModel = model.RequisitionTypeId > 0 ? RequisitiontypeManager.EditRquisitiontype(model) : RequisitiontypeManager.SaveRquisitiontype(model);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return (ResponsModel.Status ) ? Reload() : ErrorResult(String.Format(ResponsModel.Message));
        }

        [AjaxAuthorize(Roles = "requisitiontype-3")]
        public ActionResult Delete(Inventory_RequsitionType requsitionType)
        {
            var deleteIndex = 0;
            try
            {
                deleteIndex = RequisitiontypeManager.DeleteRequsitionType(requsitionType.RequisitionTypeId);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return (deleteIndex > 0) ? Reload() : ErrorResult("Failed to delete Brand");

        }

        public JsonResult CheckRequsitionType(Inventory_RequsitionType model)
        {
            bool isExist = !RequisitiontypeManager.IsExistRquisitiontype(model);
            return Json(isExist, JsonRequestBehavior.AllowGet);
        }
    }
}