using System;
using System.Web.Mvc;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.Common;
using SCERP.Web.Areas.Merchandising.Models.ViewModel;
using SCERP.Web.Controllers;

namespace SCERP.Web.Areas.Merchandising.Controllers
{
    public class CompConsumptionDetailController : BaseController
    {

        private readonly ICompConsumptionDetailManager _compConsumptionDetailManager;
        private readonly ICompConsumptionManager _compConsumptionManager;
        private readonly IConsumptionTypeManager _consumptionTypeManager;
        private readonly IConsumptionDetailManager _consumptionDetailManager;

        public CompConsumptionDetailController(ICompConsumptionDetailManager compConsumptionDetailManager,
            ICompConsumptionManager compConsumptionManager, 
            IConsumptionTypeManager consumptionTypeManager,
            IConsumptionDetailManager consumptionDetailManager)
        {
            this._compConsumptionDetailManager = compConsumptionDetailManager;
            this._compConsumptionManager = compConsumptionManager;
            this._consumptionTypeManager = consumptionTypeManager;
            this._consumptionDetailManager = consumptionDetailManager;

        }
        public ActionResult Index(CompConsumptionDetailViewModel model)
        {
          ModelState.Clear();
          var consComponet=  _compConsumptionManager.GetCompConsumptionById(model.CompConsumptionId);
            model.CompConsumption = _compConsumptionManager.GetVCompConsumptionById(model.CompConsumptionId);
          model.ConsumptionTypes = _consumptionTypeManager.GetConsumptionTypes();
          model.GColors = _consumptionDetailManager.GetGColorList(consComponet.OrderStyleRefId);
          model.GSizes = _consumptionDetailManager.GetGSizeList(consComponet.OrderStyleRefId);
          model.CompConsumptionDetails = _compConsumptionDetailManager.GetVCompConsumptionDetails(consComponet.ComponentSlNo, consComponet.ConsRefId, consComponet.OrderStyleRefId);
          model.ConsRefId = consComponet.ConsRefId;
          model.CompomentSlNo = consComponet.ComponentSlNo;
          model.OrderStyleRefId = consComponet.OrderStyleRefId;
          return PartialView("~/Areas/Merchandising/Views/CompConsumptionDetail/Index.cshtml", model);
        }

        public ActionResult UpdateCompConsDetail(CompConsumptionDetailViewModel model)
        {
            try
            {
                var updateInde = _compConsumptionDetailManager.UpdateCompConsDetail(model,model.UpdateKey);
                return updateInde > 0 ? ReloadAction(model) : ErrorResult("Update Fail !!");
            }
            catch (Exception exception)
            {
                
                Errorlog.WriteLog(exception);
                return ErrorResult("Stystem Error :"+exception.Message);
            }
        }

        public ActionResult UpdateFabricSize(CompConsumptionDetailViewModel model)
        {
            try
            {
                var updateInde = _compConsumptionDetailManager.UpdateFabricSize(model);
                return updateInde > 0 ? ReloadAction(model) : ErrorResult("Update Fail !!");
            }
            catch (Exception exception)
            {

                Errorlog.WriteLog(exception);
                return ErrorResult("Stystem Error :" + exception.Message);
            }


        }
        public ActionResult UpdateGrWidh(CompConsumptionDetailViewModel model)
        {
            try
            {
                var updateInde = _compConsumptionDetailManager.UpdateGrWidh(model);

                return updateInde > 0 ? ReloadAction(model) : ErrorResult("Update Fail !!");

            }
            catch (Exception exception)
            {

                Errorlog.WriteLog(exception);
                return ErrorResult("Stystem Error :" + exception.Message);
            }


        }
        public ActionResult UpdateGrColor(CompConsumptionDetailViewModel model)
        {
            try
            {
                var updateInde = _compConsumptionDetailManager.UpdateGrColor(model);
                return updateInde > 0 ? ReloadAction(model) : ErrorResult("Update Fail !!");

            }
            catch (Exception exception)
            {

                Errorlog.WriteLog(exception);
                return ErrorResult("Stystem Error :" + exception.Message);
            }


        }
        public ActionResult ReloadAction(CompConsumptionDetailViewModel model)
        {
            model.CompConsumptionDetails = _compConsumptionDetailManager.GetVCompConsumptionDetails(model.CompomentSlNo, model.ConsRefId, model.OrderStyleRefId);
            return PartialView("~/Areas/Merchandising/Views/CompConsumptionDetail/_CompConsDetailList.cshtml", model);
        }

        public ActionResult UpdateFabricQty([Bind(Include = "ConsRefId")]CompConsumptionDetailViewModel model)
        {
            int updateIndex = _compConsumptionDetailManager.UpdateFabricConsQty(model.ConsRefId);
            return ErrorResult(updateIndex>0 ? "Update Success Fully" : "Update Failed");
        }
        public ActionResult UpdatePColor(CompConsumptionDetailViewModel model)
        {
            try
            {
                var updateInde = _compConsumptionDetailManager.UpdateProductColor(model);
                return updateInde > 0 ? ReloadAction(model) : ErrorResult("Update Fail !!");

            }
            catch (Exception exception)
            {

                Errorlog.WriteLog(exception);
                return ErrorResult("Stystem Error :" + exception.Message);
            }
        }
	}
}