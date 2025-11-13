using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCERP.Common;
using SCERP.Model;
using SCERP.Web.Areas.Inventory.Models.ViewModels;

namespace SCERP.Web.Areas.Inventory.Controllers
{
    public class PurchaseTypeController : BaseInventoryController
    {
        [AjaxAuthorize(Roles = "purchasetype-1,purchasetype-2,purchasetype-3")]
        public ActionResult Index(PurchaseTypeViewModel model)
        {
            ModelState.Clear();
            var totalRecords = 0;
            try
            {
                model.Title = model.PurchaseTypeTitle;
                model.InventoryPurchaseTypes = PurchaseTypeManager.GetPurchaseTypesByPaging(model, out totalRecords);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            model.TotalRecords = totalRecords;
            return View(model);
        }

        [AjaxAuthorize(Roles = "purchasetype-2,purchasetype-3")]
        public ActionResult Edit(PurchaseTypeViewModel model)
        {
            ModelState.Clear();
            try
            {
                if (model.PurchaseTypeId > 0)
                {
                    var purchaseType = PurchaseTypeManager.GetPurchaseTypeById(model.PurchaseTypeId);
                    model.Title = purchaseType.Title;
                    model.Description = purchaseType.Description;
                }
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return View(model);
        }

        [AjaxAuthorize(Roles = "purchasetype-2,purchasetype-3")]
        public ActionResult Save(Inventory_PurchaseType model)
        {
            var saveIndex = 0;
            try
            {
                saveIndex = model.PurchaseTypeId > 0 ? PurchaseTypeManager.EditPurchaseType(model) : PurchaseTypeManager.SavePurchaseType(model);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return (saveIndex > 0) ? Reload() : ErrorResult(String.Format("Failed to save {0}", model.Title));
        }

        [AjaxAuthorize(Roles = "purchasetype-3")]
        public ActionResult Delete(Inventory_PurchaseType purchaseType)
        {
            var deleteIndex = 0;
            try
            {
                deleteIndex = PurchaseTypeManager.DeletePurchaseType(purchaseType.PurchaseTypeId);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return (deleteIndex > 0) ? Reload() : ErrorResult("Failed to delete Brand");

        }

        public JsonResult CheckPurchaseType(Inventory_PurchaseType model)
        {
            bool isExist = !PurchaseTypeManager.IsExistPurchaseType(model);
            return Json(isExist, JsonRequestBehavior.AllowGet);
        }
    }
}