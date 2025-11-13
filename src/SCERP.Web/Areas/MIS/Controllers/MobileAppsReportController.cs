using SCERP.BLL.IManager.IInventoryManager;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.BLL.IManager.IMisManager;
using SCERP.BLL.IManager.IProductionManager;
using SCERP.Common;
using SCERP.Web.Areas.Inventory.Models.ViewModels;
using SCERP.Web.Areas.MIS.Models.ViewModel;
using SCERP.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SCERP.Web.Areas.MIS.Controllers
{
    public class MobileAppsReportController : BaseController
    {

        private readonly IMisDashboardManager _misDashboardManager;
        private readonly ISewingOutPutProcessManager _sewingOutPutProcessManager;
        private readonly IOmBuyerManager _buyerManager;
        private readonly IOmBuyOrdStyleManager _omBuyOrdStyleManager;
        private readonly IBuyOrdShipManager _buyOrdShipManager;
        private readonly IStyleShipmentManager _shipmentManager;
        private readonly IMobileAppsManager _mobileAppsManager;
        private readonly IProductionReportManager _reportManager;

        public MobileAppsReportController(IProductionReportManager reportManager,IMisDashboardManager misDashboardManager, ISewingOutPutProcessManager sewingOutPutProcessManager, IOmBuyerManager buyerManager, IOmBuyOrdStyleManager omBuyOrdStyleManager, IBuyOrdShipManager buyOrdShipManager, IStyleShipmentManager shipmentManager, IMobileAppsManager mobileAppsManager)
        {
            _misDashboardManager = misDashboardManager;
            _shipmentManager = shipmentManager;
            _sewingOutPutProcessManager = sewingOutPutProcessManager;
            _buyerManager = buyerManager;
            this._omBuyOrdStyleManager = omBuyOrdStyleManager;
            _buyOrdShipManager = buyOrdShipManager;
            _mobileAppsManager = mobileAppsManager;
            _reportManager = reportManager;
        }

        // GET: MIS/MobileAppsReport
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult HrReport(MisDashboardViewModel model)
        {
            ModelState.Clear();
            model.FilterDate = DateTime.Now;
            model.ProductionForecastCurrentMonth = _sewingOutPutProcessManager.ProductionForecast(model.FilterDate, PortalContext.CurrentUser.CompId);
            model.ProductionForecastPreviousMonth = _sewingOutPutProcessManager.ProductionForecastLastMonth(model.FilterDate.AddMonths(-1), PortalContext.CurrentUser.CompId);
            return View(model);
        }

        public ActionResult SwingProduction(MisDashboardViewModel model)
        {
            ModelState.Clear();
            model.FilterDate = DateTime.Now;
            model.ProductionForecastCurrentMonth = _sewingOutPutProcessManager.ProductionForecast(model.FilterDate, PortalContext.CurrentUser.CompId);
            model.ProductionForecastPreviousMonth = _sewingOutPutProcessManager.ProductionForecastLastMonth(model.FilterDate.AddMonths(-1), PortalContext.CurrentUser.CompId);
            return View(model);
        }

        public ActionResult ShipmentStatusIndex(MisDashboardViewModel model)
        {
            ModelState.Clear();
            return View(model);
        }

        public ActionResult ShipmentStatus(MisDashboardViewModel model)
        {

            //model.BuyerWiseOrderStyleDtable = _shipmentManager.GetMonthlyShipmentSummary(model.FromDate, model.ToDate);
            model.BuyerWiseOrderStyleDtable = _mobileAppsManager.GetMonthlyShipmentSummary(model.FromDate, model.ToDate);
            ModelState.Clear();
            return View(model);
        }

        public ActionResult DyeingProfitabilyAnalysis()
        {
            MisDashboardViewModel model = new MisDashboardViewModel();
            model.SummaryDataTable = _reportManager.DyeingProfitabilyAnalysis(2018);
            return View(model);
        }

        public ActionResult TnaSweingPandingStatus()
        {
            MisDashboardViewModel model = new MisDashboardViewModel();
            model.SummaryDataTable = _misDashboardManager.GetTnaSweingPandingStatus();
            return View(model);
        }

        public ActionResult StyleDashBoard(MisDashboardViewModel model)
        {
            model.BuyerWiseOrderStyleDtable = _mobileAppsManager.GetSpMISReprotDashBoard();
            return View(model);
        }


    }
}