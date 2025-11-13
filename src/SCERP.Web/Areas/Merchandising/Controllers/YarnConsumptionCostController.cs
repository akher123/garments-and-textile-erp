using System;
using System.Linq;
using System.Web.Mvc;
using SCERP.BLL.IManager.ICommercialManager;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.Common;
using SCERP.Web.Areas.Merchandising.Models.ViewModel;
using SCERP.Web.Controllers;

namespace SCERP.Web.Areas.Merchandising.Controllers
{
    public class YarnConsumptionCostController : BaseController
    {
   
        private readonly IYarnConsumptionManager _yarnConsumptionManager;
        private readonly IFabricOrderManager _fabricOrderManager;
        public readonly IProFormaInvoiceManager proFormaInvoice;
        public YarnConsumptionCostController(IProFormaInvoiceManager proFormaInvoice,IFabricOrderManager fabricOrderManager, IYarnConsumptionManager yarnConsumptionManager)
        {
            this._yarnConsumptionManager = yarnConsumptionManager;
            _fabricOrderManager = fabricOrderManager;
            this.proFormaInvoice = proFormaInvoice;
        }
        public ActionResult Index(YarnConsumptionCostViewModel model)
        {
            ModelState.Clear();
            int totalRecords;
            model.OmBuyOrdStyles = _fabricOrderManager.GetVwFabricOrders(model.PageIndex,model.SearchString, out totalRecords);
            model.TotalRecords = totalRecords;
            return View(model);
        }

        public ActionResult YarnConsumptionList(YarnConsumptionCostViewModel model)
        {
            model.SupplierCompanies = SupplierCompanyManager.GetAllSupplierCompany();
            model.YarnConsumptions = _yarnConsumptionManager.GetYarnConsSummaryByOrderSyleRefId(model.OrderStyleRefId);
            int[] supplierIds = model.YarnConsumptions.Select(x => x.SupplierId??0).ToArray();
            model.PiInvoices= proFormaInvoice.GetProFormaInvoiceBySupplierIds(supplierIds);
            return View("~/Areas/Merchandising/Views/YarnConsumptionCost/YarnConsumptionList.cshtml", model);
        }

        public ActionResult Update(YarnConsumptionCostViewModel model)
        {

            try
            {
                var yarnConsumptions = model.YarnConsumptions.Where(x => x.SupplierId > 0 && x.PiRefId != null).ToList();         
                if (yarnConsumptions.Any())
                {
                    var updateIndex = _yarnConsumptionManager.UpdateYarnConsRate(yarnConsumptions);
                    if (updateIndex > 0)
                    {
                        return ErrorResult("Update Successfully !");
                    }
                    return ErrorResult("Fail To Update Yarn Rate and Supplier !! ");
                }
                else
                {
                    return ErrorResult("Assign Rate and  Supplier !! ");
                }
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult("System Error :" + exception.Message);

            }

        }

    }
}