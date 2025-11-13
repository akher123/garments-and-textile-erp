using System;
using System.Web.Mvc;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.Common;
using SCERP.Web.Areas.Merchandising.Models.ViewModel;
using SCERP.Web.Controllers;

namespace SCERP.Web.Areas.Merchandising.Controllers
{
    public class ConsumptionDetailController : BaseController
    {

        private readonly IConsumptionManager _consumptionManager;
        private readonly IConsumptionDetailManager _consumptionDetailManager;
        private readonly IConsumptionTypeManager _consumptionTypeManager;
     

        public ConsumptionDetailController(IConsumptionManager consumptionManager, IConsumptionDetailManager consumptionDetailManager, IConsumptionTypeManager consumptionTypeManager)
        {
            this._consumptionManager = consumptionManager;
            this._consumptionDetailManager = consumptionDetailManager;
            this._consumptionTypeManager = consumptionTypeManager;
        }

        [AjaxAuthorize(Roles = "consumptionanalysis-1,consumptionanalysis-2,consumptionanalysis-3")]
        public ActionResult Index(ConsumptionDetailViewModel model)
        {
            ModelState.Clear();
            if (model.ConsumptionId>0)
            {
                var consumption = _consumptionManager.GetVConsumptionById(model.ConsumptionId);
                model.ConsRefId = consumption.ConsRefId;
                model.OrderStyleRefId = consumption.OrderStyleRefId;
                model.ConsumptionDetails = _consumptionDetailManager.GetVConsumptionDetails(model.ConsRefId);
                model.ConsumptionTypes = _consumptionTypeManager.GetConsumptionTypes();
                model.GColors = _consumptionDetailManager.GetGColorList(model.OrderStyleRefId);
                model.GSizes = _consumptionDetailManager.GetGSizeList(model.OrderStyleRefId);
                model.ComponentName = consumption.ItemName;
           
            }
            else
            {
                return ErrorResult("Select any Consumption !");
            }
        

            return View(model);
        }
           [AjaxAuthorize(Roles = "consumptionanalysis-2,consumptionanalysis-3")]
        public ActionResult Update(ConsumptionDetailViewModel model)
        {
            ModelState.Clear();
            try
            {
                var updateIndex = _consumptionDetailManager.UpdateConsDetail(model);
                model.ConsumptionDetails = _consumptionDetailManager.GetVConsumptionDetails(model.ConsRefId);
            }
            catch (Exception exception)
            {

                return ErrorResult(exception.Message);
            }
            return PartialView("~/Areas/Merchandising/Views/ConsumptionDetail/_ConsumtionDetailList.cshtml", model); 
        }
            [AjaxAuthorize(Roles = "consumptionanalysis-2,consumptionanalysis-3")]
        public ActionResult UpdateProductionColor(ConsumptionDetailViewModel model)
        {
            ModelState.Clear();
            try
            {
                var updateIndex = _consumptionDetailManager.UpdateConsDetailByPcolor(model);
                model.ConsumptionDetails = _consumptionDetailManager.GetVConsumptionDetails(model.ConsRefId);
            }
            catch (Exception exception)
            {

                return ErrorResult(exception.Message);
            }
        
            return PartialView("~/Areas/Merchandising/Views/ConsumptionDetail/_ConsumtionDetailList.cshtml", model);
        }
            [AjaxAuthorize(Roles = "consumptionanalysis-2,consumptionanalysis-3")]
        public ActionResult UpdateProductionSize(ConsumptionDetailViewModel model)
        {
            ModelState.Clear();
            try
            {
                var updateIndex = _consumptionDetailManager.UpdateConsDetailByPSize(model);
                model.ConsumptionDetails = _consumptionDetailManager.GetVConsumptionDetails(model.ConsRefId);
            }
            catch (Exception exception)
            {

                return ErrorResult(exception.Message);
            }

            return PartialView("~/Areas/Merchandising/Views/ConsumptionDetail/_ConsumtionDetailList.cshtml", model);
        }
            public ActionResult UpdateRemarks(ConsumptionDetailViewModel model)
            {
                ModelState.Clear();
                try
                {
                    var updateIndex = _consumptionDetailManager.UpdateRemarks(model);
                    model.ConsumptionDetails = _consumptionDetailManager.GetVConsumptionDetails(model.ConsRefId);
                }
                catch (Exception exception)
                {

                    return ErrorResult(exception.Message);
                }

                return PartialView("~/Areas/Merchandising/Views/ConsumptionDetail/_ConsumtionDetailList.cshtml", model);
            }

            public ActionResult UpdateProductQty(ConsumptionDetailViewModel model)
            {
                ModelState.Clear();
                try
                {
                    var updateIndex = _consumptionDetailManager.UpdateProductQty(model);
                    model.ConsumptionDetails = _consumptionDetailManager.GetVConsumptionDetails(model.ConsRefId);
                }
                catch (Exception exception)
                {

                    return ErrorResult(exception.Message);
                }

                return PartialView("~/Areas/Merchandising/Views/ConsumptionDetail/_ConsumtionDetailList.cshtml", model);
            }
        
	}
}