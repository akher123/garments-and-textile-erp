using System;
using System.Web.Mvc;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.Common;
using SCERP.Model;
using SCERP.Model.MerchandisingModel;
using SCERP.Web.Areas.Merchandising.Models.ViewModel;
using SCERP.Web.Controllers;

namespace SCERP.Web.Areas.Merchandising.Controllers
{
    public class FabConsumptionController : BaseController
    {

        private readonly IConsumptionManager _consumptionManager;
        private readonly ICompConsumptionManager _compConsumptionManager;
        private readonly IComponentManager _componentManager;
        private readonly IFabricTypeManager _fabricTypeManager;
        private readonly IFabricOrderManager _fabricOrderManager;
        private IOmBuyOrdStyleManager _omBuyOrdStyleManager;
        public FabConsumptionController(IConsumptionManager consumptionManager,
            ICompConsumptionManager compConsumptionManager,
            IOmBuyOrdStyleManager omBuyOrdStyleManager,
            IComponentManager componentManager,
            IFabricTypeManager fabricTypeManager,
            IFabricOrderManager fabricOrderManager)
        {
            this._consumptionManager = consumptionManager;
            this._compConsumptionManager = compConsumptionManager;
            this._componentManager = componentManager;
            this._fabricTypeManager = fabricTypeManager;
            _omBuyOrdStyleManager = omBuyOrdStyleManager;
            _fabricOrderManager = fabricOrderManager;
        }
        [AjaxAuthorize(Roles = "fabricconsumption-1,fabricconsumption-2,fabricconsumption-3")]
        public ActionResult Index(FabConsumptionViewModel model)
        {
            ModelState.Clear();
             model.BuyOrdStyle= _omBuyOrdStyleManager.GetVBuyOrdStyleByRefId(model.OrderStyleRefId);
            model.Consumptions = _consumptionManager.GetConsumptionsFabric(model.OrderStyleRefId);
            model.Components = _componentManager.GetComponents();
            model.FabricTypes = _fabricTypeManager.GetFabricTypes();
            model.CompConsumptions = _compConsumptionManager.GetCompConsumption(model.OrderStyleRefId);
            model.ComponentSlNo = _compConsumptionManager.GetNewCompConsuotionSlNo(model.OrderStyleRefId);
            model.EnDate = DateTime.Now;
            return PartialView("~/Areas/Merchandising/Views/FabConsumption/Index.cshtml", model);
        }

        public ActionResult FabConsumptionProcess(FabConsumptionViewModel model)
        {
            ModelState.Clear();
            return View(model);
        }
        public ActionResult OrderStyleList(FabConsumptionViewModel model)
        {
            ModelState.Clear();
            var totalRecords = 0;
            var omBuyOrdStyle = new OM_BuyOrdStyle { page = model.page, sort = model.sort, sortdir = model.sortdir, FromDate = model.FromDate, ToDate = model.ToDate, SearchString = model.SearchString };
            model.OmBuyOrdStyles = _compConsumptionManager.GetVwCompConsumptionOrderStyle(omBuyOrdStyle, out totalRecords);
            model.TotalRecords = totalRecords;
            return View(model);
        }
        [AjaxAuthorize(Roles = "fabricconsumption-2,fabricconsumption-3")]
        public ActionResult Save(OM_CompConsumption model)
        {

            try
            {
                var saveIndex = 0;
                if (!_fabricOrderManager.IsFabricBookingLock(model.OrderStyleRefId,PortalContext.CurrentUser.CompId))
                {
                    saveIndex = model.CompConsumptionId > 0 ? _compConsumptionManager.EditCompConsumption(model) : _compConsumptionManager.SaveCompConsumption(model);
                }
                else
                {
                    return ErrorResult("This is not allow to  change !Because this is already loked by Purchase departments");
                }
                if (saveIndex > 0)
                {
                    return RedirectToAction("Index", new { model.OrderStyleRefId, model.OrderNo });
                }
                else
                {
                    return ErrorResult("Save failed !!");
                }
            }
            catch (Exception exception)
            {

                return ErrorResult(exception.Message);
            }


        }
        [AjaxAuthorize(Roles = "fabricconsumption-2,fabricconsumption-3")]
        public ActionResult Edit(FabConsumptionViewModel model)
        {
            ModelState.Clear();
            if (model.CompConsumptionId > 0)
            {
                var compConsumption = _compConsumptionManager.GetCompConsumptionById(model.CompConsumptionId);
                model.CompConsumptionId = compConsumption.CompConsumptionId;
                model.ComponentSlNo = compConsumption.ComponentSlNo;
                model.ConsRefId = compConsumption.ConsRefId;
                model.ComponentRefId = compConsumption.ComponentRefId;
                model.NParts = compConsumption.NParts;
                model.FabricType = compConsumption.FabricType;
                model.EnDate = compConsumption.EnDate;
                model.OrderStyleRefId = compConsumption.OrderStyleRefId;
                model.Description = compConsumption.Description;
            }
            else
            {
                model.ComponentSlNo = _compConsumptionManager.GetNewCompConsuotionSlNo(model.OrderStyleRefId);
                model.EnDate = DateTime.Now;
            }

            model.Consumptions = _consumptionManager.GetConsumptionsFabric(model.OrderStyleRefId);
            model.Components = _componentManager.GetComponents();
            model.FabricTypes = _fabricTypeManager.GetFabricTypes();
            return PartialView("~/Areas/Merchandising/Views/FabConsumption/_Edit.cshtml", model);
        }
        [AjaxAuthorize(Roles = "fabricconsumption-3")]
        public ActionResult Delete(FabConsumptionViewModel model)
        {
            try
            {
                var deleteIndex = 0;
                if (!_fabricOrderManager.IsFabricBookingLock(model.OrderStyleRefId, PortalContext.CurrentUser.CompId))
                {
                     deleteIndex = _compConsumptionManager.DeleteCompConsumption(model.CompConsumptionId);
                }
                else
                {
                    return ErrorResult("This is not allow to  change !Because this is already loked by Purchase departments");
                }
          
                if (deleteIndex > 0)
                {
                    return RedirectToAction("Index", new
                    {
                        OrderStyleRefId = model.OrderStyleRefId,
                        OrderNo=model.OrderNo
                    });
                }
                else
                {
                    //return deleteIndex>0 ? Json(new {Status=true}, JsonRequestBehavior.AllowGet) : ErrorResult("Delete Fail");
                    return ErrorResult("Delete Failed");
                }

            }
            catch (Exception exception)
            {

                Errorlog.WriteLog(exception);
                return ErrorResult(exception.Message);
            }
        }
    }
}