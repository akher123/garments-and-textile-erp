using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.Common;
using SCERP.Model;
using SCERP.Web.Areas.Merchandising.Models.ViewModel;
using SCERP.Web.Controllers;

namespace SCERP.Web.Areas.Merchandising.Controllers
{
    public class OrderTypeController : BaseController
    {
        private IOrderTypeManager orderTypeManager;
        public OrderTypeController(IOrderTypeManager orderTypeManager)
        {
            this.orderTypeManager = orderTypeManager;
        }
        [AjaxAuthorize(Roles = "ordertype-1,ordertype-2,ordertype-3")]
        public ActionResult Index(OrderTypeViewModel model)
        {
            ModelState.Clear();
            var totalRecords = 0;
            model.OrderTypes = orderTypeManager.GetOrderTypesByPaging(model, out totalRecords);
            model.TotalRecords = totalRecords;
            return View(model);

        }
        [AjaxAuthorize(Roles = "ordertype-2,ordertype-3")]
        [HttpGet]
        public ActionResult Edit(OrderTypeViewModel model)
        {
            ModelState.Clear();
            if (model.OrderTypeId > 0)
            {
                var oType = orderTypeManager.GetOrderTypeById(model.OrderTypeId);
                model.OTypeName = oType.OTypeName;
                model.Prefix = oType.Prefix;
                model.OTypeRefId = oType.OTypeRefId;
                model.OrderTypeId = oType.OrderTypeId;
            }
            else
            {
                model.OTypeRefId = orderTypeManager.GetNewOTypeRefId();
            }
            return View(model);
        }
        [HttpPost]
        [AjaxAuthorize(Roles = "ordertype-2,ordertype-3")]
        public ActionResult Save(OM_OrderType model)
        {

            var index = 0;
            var errorMessage = "";
            try
            {
                index = model.OrderTypeId > 0 ? orderTypeManager.EditOrderType(model) : orderTypeManager.SaveOrderType(model);
            }
            catch (Exception exception)
            {
                errorMessage = exception.Message;
                Errorlog.WriteLog(exception);
            }
            return index > 0 ? Reload() : ErrorResult("Season save fail " + errorMessage);
        }
        [AjaxAuthorize(Roles = "ordertype-3")]
        public ActionResult Delete(OM_OrderType model)
        {
            var saveIndex = orderTypeManager.DeleteOType(model.OTypeRefId);
            if (saveIndex == -1)
            {
                return ErrorResult("Could not possible to delete OrderType because of it's all ready used in buyer Order");
            }
            return saveIndex > 0 ? Reload() : ErrorResult("Delate Fail");
        }

        [HttpPost]
        public JsonResult CheckExistingOrderType(OM_OrderType model)
        {
            var isExist = !orderTypeManager.CheckExistingOrderType(model);
            return Json(isExist, JsonRequestBehavior.AllowGet);
        }
    }
}