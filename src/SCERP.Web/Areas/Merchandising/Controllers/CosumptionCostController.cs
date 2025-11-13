using System.Linq;
using System.Web.Mvc;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.Common;
using SCERP.Model;
using SCERP.Model.MerchandisingModel;
using SCERP.Web.Areas.Merchandising.Models.ViewModel;
using SCERP.Web.Controllers;

namespace SCERP.Web.Areas.Merchandising.Controllers
{
    public class CosumptionCostController : BaseController
    {
        private readonly IOmBuyOrdStyleManager _omBuyOrdStyleManager;
        private readonly IConsumptionManager _consumptionManager;
        private readonly IConsumptionSupplierManager _consumptionSupplierManager;
        public CosumptionCostController(IOmBuyOrdStyleManager omBuyOrdStyleManager, IConsumptionManager consumptionManager, IConsumptionSupplierManager consumptionSupplierManager)
        {
            this._omBuyOrdStyleManager = omBuyOrdStyleManager;
            this._consumptionManager = consumptionManager;
            _consumptionSupplierManager = consumptionSupplierManager;
        }
        public ActionResult Index(CosumptionCostViewModel model)
        {
            ModelState.Clear();
           
            var totalRecords = 0;
            model.OmBuyOrdStyles = _omBuyOrdStyleManager.GetBuyerOrderStyles(model, out totalRecords);
            model.TotalRecords = totalRecords;
            return View(model);
        }

        public ActionResult CosumptionLsit(CosumptionCostViewModel model)
        {
           
            const string consGroup = "B";
            model.SupplierCompanies = SupplierCompanyManager.GetAllSupplierCompany();
            model.Consumptions = _consumptionManager.GetConsumptions(model.OrderStyleRefId, consGroup);
            return View("~/Areas/Merchandising/Views/CosumptionCost/CosumptionLsit.cshtml", model);
        }
        public ActionResult Update(CosumptionCostViewModel model)
        {
            var consumptions = model.Consumptions.Where(x => x.Rate > 0).Select(x => new OM_Consumption()
            {
                ConsumptionId = x.ConsumptionId,
                ConsRefId = x.ConsRefId,
                OrderStyleRefId = x.OrderStyleRefId,
                Rate = x.Rate,
                SupplierId = x.SupplierId
            }).ToList();
            var updateIndex = 0;
            if (consumptions.Any())
            {

                updateIndex = _consumptionManager.UpdateConstumptionCost(consumptions);

            }
            if (updateIndex > 0)
            {
                return RedirectToAction("CosumptionLsit", new { model.OrderStyleRefId });

            }
            return ErrorResult("Update Failed !");

        }

        public ActionResult ConsSupplier(CosumptionCostViewModel model)
        {

            if (model.ConsRate <= 0)
            {
                return ErrorResult("Please Assign Rate First ");
            }
            if (model.ConsQty <= 0)
            {
                return ErrorResult("Accessories Consumption First");
            }

            model.AssignedQty = _consumptionSupplierManager.GetAssignedQtyByConsumptionId(model.ConsumptionId);
            model.ConsumptionSupplier.ConsumptionId = model.ConsumptionId;
            model.BalanceQty = model.ConsQty - model.AssignedQty;
            model.ConsumptionSupplier.Quantity = model.BalanceQty;
            model.ConsumptionSupplier.Rate = model.ConsRate;
            model.SupplierCompanies = SupplierCompanyManager.GetAllSupplierCompany();
            model.ConsumptionSuppliers = _consumptionSupplierManager.GetConsSupplierList(PortalContext.CurrentUser.CompId, model.ConsumptionId);
            return View(model);
        }

        public ActionResult SaveConsSupplier(CosumptionCostViewModel model)
        {
            var consSupllier = _consumptionSupplierManager.GetConsumtionSupplierByConsumtionSupplierId(model.ConsumptionSupplier.ConsumptionId, model.ConsumptionSupplier.SupplierId) ?? new OM_ConsumptionSupplier();
            consSupllier.Quantity = model.ConsumptionSupplier.Quantity;
            consSupllier.ConsumptionId = model.ConsumptionSupplier.ConsumptionId;
            consSupllier.Rate = model.ConsumptionSupplier.Rate;
            consSupllier.SupplierId = model.ConsumptionSupplier.SupplierId;
            consSupllier.Remarks = model.ConsumptionSupplier.Remarks;
            consSupllier.Percentage = model.ConsumptionSupplier.Quantity / (model.BalanceQty + model.ConsumptionSupplier.Quantity);
            consSupllier.CompId = PortalContext.CurrentUser.CompId;
            int saved = _consumptionSupplierManager.SaveConsSupplier(consSupllier);
            return Json(saved > 0, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Delete(int consumptionSupplierId)
        {
            int deleted = _consumptionSupplierManager.DeleteConsumptionSupplier(consumptionSupplierId);
            return Json(deleted > 0, JsonRequestBehavior.AllowGet);
        }
    }
}