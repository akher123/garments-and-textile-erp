using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.BLL.IManager.IMisManager;
using SCERP.BLL.IManager.IPlanningManager;
using SCERP.BLL.IManager.IProductionManager;
using SCERP.Common;
using SCERP.Web.Areas.Inventory.Models.ViewModels;
using SCERP.Web.Areas.MIS.Models.ViewModel;
using SCERP.Web.Controllers;
using SCERP.Web.Models;

namespace SCERP.Web.Areas.MIS.Controllers
{
    public class MisDashboardController : BaseController
    {
        private readonly IMisDashboardManager _misDashboardManager;
        private readonly ISewingOutPutProcessManager _sewingOutPutProcessManager;
        private readonly IOmBuyerManager _buyerManager;
        private readonly IOmBuyOrdStyleManager _omBuyOrdStyleManager;
        private readonly IBuyOrdShipManager _buyOrdShipManager;
        private readonly ITimeAndActionManager _timeAndActionManager;
        public MisDashboardController(IMisDashboardManager misDashboardManager, ISewingOutPutProcessManager sewingOutPutProcessManager, IOmBuyerManager buyerManager, IOmBuyOrdStyleManager omBuyOrdStyleManager, IBuyOrdShipManager buyOrdShipManager, ITimeAndActionManager timeAndActionManager)
        {
            _misDashboardManager = misDashboardManager;
            _sewingOutPutProcessManager = sewingOutPutProcessManager;
            _buyerManager = buyerManager;
            this._omBuyOrdStyleManager = omBuyOrdStyleManager;
            _buyOrdShipManager = buyOrdShipManager;
            _timeAndActionManager = timeAndActionManager;
        }
        public ActionResult Index(MisDashboardViewModel model)
        {
            //SpYearlyBuyerWiseOrderStatusSummary
            model.BuyerWiseOrderStyleDtable = _misDashboardManager.GetBuyerWiseOrderStyleDtable();
            model.MerchadiserWiseOrderStyleDtable = _misDashboardManager.GetMerchadiserWiseOrderStyleDtable();

            model.BuyerOrderMasterDataTable = _misDashboardManager.GetMontlyBuyerWiseProductionPlan();
            //  model.SummaryDataTable = _misDashboardManager.GetOrderStatusSummaryDtable();
            model.SummaryDataTable = _timeAndActionManager.GetBuyerWiseTnaAlert(PortalContext.CurrentUser.CompId);
            return View(model);
        }

        public ActionResult StyleDashBoard(MisDashboardViewModel model)
        {
            model.BuyerWiseOrderStyleDtable = _misDashboardManager.GetSpMISReprotDashBoard();
            return View(model);
        }

        public ActionResult MonthlyShipment(InventoryReportViewModel model)
        {
            ModelState.Clear();
            model.FromDate = DateTime.Now;
            model.ToDate = DateTime.Now;
            return View(model);
        }
        public ActionResult ProductionForecast(MisDashboardViewModel model)
        {
            ModelState.Clear();
            model.FilterDate = DateTime.Now;
            model.ProductionForecastCurrentMonth = _sewingOutPutProcessManager.ProductionForecast(model.FilterDate, PortalContext.CurrentUser.CompId);
            model.ProductionForecastPreviousMonth = _sewingOutPutProcessManager.ProductionForecastLastMonth(model.FilterDate.AddMonths(-1), PortalContext.CurrentUser.CompId);
            return View(model);
        }

        public ActionResult StyleDashBoardWithValue(MisDashboardViewModel model)
        {
            model.BuyerWiseOrderStyleDtable = _misDashboardManager.GetOrderStatusWithValue();
            return View(model);
        }

        public ActionResult RunningOrderSummary(string month, string buyer)
        {
            MisDashboardViewModel model = new MisDashboardViewModel();
            model.BuyerWiseOrderStyleDtable = _misDashboardManager.RunningOrderSummary(month, buyer, PortalContext.CurrentUser.CompId);
            return View(model);
        }

        public ActionResult SewingPlanDetail(string month, string buyer)
        {
            MisDashboardViewModel model = new MisDashboardViewModel();
            model.BuyerWiseOrderStyleDtable = _misDashboardManager.MontlySewingPlanDetail(month, buyer, PortalContext.CurrentUser.CompId);
            return View(model);
        }
        public ActionResult MonthlyOrderStatus(MisDashboardViewModel model)
        {
            model.BuyerWiseOrderStyleDtable = _misDashboardManager.GetMonthlyOrderStatus(PortalContext.CurrentUser.CompId);
            return View(model);
        }


        public ActionResult BuyerOrderMaster(MisDashboardViewModel model)
        {
            ModelState.Clear();
            model.BuyerRefId = model.BuyerRefId ?? "";
            model.OrderNo = model.OrderNo ?? "";
            model.OrderStyleRefId = model.OrderStyleRefId ?? "";
            model.BuyerOrderMasterDataTable =
                _misDashboardManager.GetSpBuyerOrderMasterDashBoard(model.BuyerRefId, model.OrderNo, model.OrderStyleRefId);
            model.Buyers = _buyerManager.GetAllBuyers();

            model.OrderList = _omBuyOrdStyleManager.GetOrderByBuyer(model.BuyerRefId);
            model.BuyerOrderStyles = _omBuyOrdStyleManager.GetBuyerOrderStyleByOrderNo(model.OrderNo);
            return View(model);
        }


        public ActionResult BuyerOrderMasterAjaxPopup(string refid, string columnName)
        {
            MisDashboardViewModel model = new MisDashboardViewModel();
            model.BuyerOrderMasterDataTable = _buyOrdShipManager.UpdateTempAssort(refid);
            model.BuyerRefId = refid;
            model.ColoumnName = columnName;

            return View(model);
        }

        public ActionResult TnaDetail( string orderStyleRefId)
        {
            dynamic tna = _timeAndActionManager.GetStyleWiseTna(orderStyleRefId ,PortalContext.CurrentUser.CompId);
            return View(tna);
        }
        public ActionResult LineWiseSewingProduction()
        {
            MisDashboardViewModel model = new MisDashboardViewModel();
            model.CurrentYear = DateTime.Now.Year;
            model.BuyerOrderMasterDataTable = _misDashboardManager.GetLineWiseSewingProduction(model.CurrentYear, PortalContext.CurrentUser.CompId);
            return View(model);
        }

        public ActionResult StyleSewingProduction(int currentYear, string currentMonth, int lineId, string viewType)
        {
            MisDashboardViewModel model = new MisDashboardViewModel();
            if (viewType == "S")
            {
                model.BuyerOrderMasterDataTable = _misDashboardManager.StyleSewingProduction(currentYear, currentMonth,
                    lineId, PortalContext.CurrentUser.CompId);
            }
            else if (viewType == "D")
            {
                model.BuyerOrderMasterDataTable = _misDashboardManager.StyleSewingDetailProduction(currentYear, currentMonth,
                    lineId, PortalContext.CurrentUser.CompId);
            }
            else
            {
                model.BuyerOrderMasterDataTable = _misDashboardManager.GetLineWiseDailySewingProduction(currentYear, currentMonth,
              PortalContext.CurrentUser.CompId);
            }
            model.ViewType = viewType;
            return View(model);
        }

        public ActionResult ExportImportDashBoard(MisDashboardViewModel model)
        {
            model.SummaryDataTable = _misDashboardManager.CommarcialExportImport();
            return View(model);
        }

        public ActionResult SewingProductionBoard()
        {
            DateTime currentDate = DateTime.Now;
            if (AppConfig.IsSetCustomProductionDate)
            {
                currentDate = DateTime.Parse(ConfigurationManager.AppSettings["ProductionDate"]);
            }

            //    SpMisSewingProductionBoard sewingProductionBoard =_misDashboardManager.GetSewingProductionBoard(currentDate, PortalContext.CurrentUser.CompId);
            List<dynamic> tvA = _misDashboardManager.GetDailyTargetAchivemnet(currentDate, PortalContext.CurrentUser.CompId);
            List<dynamic> htA = _misDashboardManager.GetHourlyTargetAchivemnet(currentDate, PortalContext.CurrentUser.CompId);
            DashBoardViewModel model = new DashBoardViewModel();
            model.LastEntryDateTime = _sewingOutPutProcessManager.GetLastSwingOutputDateTime(PortalContext.CurrentUser.CompId);
            model.SpMisSewingProductionBoard.TargetQty = tvA.Sum(x => x.TQty ?? 0);
            model.SpMisSewingProductionBoard.ActualQty = tvA.Sum(x => x.AQty ?? 0);
            model.SpMisSewingProductionBoard.Variance = model.SpMisSewingProductionBoard.TargetQty -
                                                        model.SpMisSewingProductionBoard.ActualQty;
            if (model.SpMisSewingProductionBoard.TargetQty > 0)
            {
                model.SpMisSewingProductionBoard.Efficiency = (model.SpMisSewingProductionBoard.ActualQty * 100) /
                                                              model.SpMisSewingProductionBoard.TargetQty;
            }
            model.Charts.Add(GetLineChart(htA));
            model.Charts.Add(GetBartChart(tvA));
            if (tvA.Any() && htA.Any())
            {
             
                model.Charts.Add(GetPiChart(tvA));
            }



            return View("~/Areas/MIS/Views/MisDashboard/SewingProductionBoard.cshtml", model);
        }

        public ActionResult TnaSweingPandingStatus()
        {
            MisDashboardViewModel model = new MisDashboardViewModel();
            model.SummaryDataTable = _misDashboardManager.GetTnaSweingPandingStatus();
            return View(model);
        }
        public ActionResult TnaSweingUpcomingStatus()
        {
            MisDashboardViewModel model = new MisDashboardViewModel();
            model.SummaryDataTable = _misDashboardManager.GetTnaSweingUpcomingStatus();
            return View(model);
        }

        private Chart GetLineChart(List<dynamic> htA)
        {
            Chart _chart = new Chart();
            _chart.labels = htA.Select(x => x.HourName).ToArray();
            _chart.datasets = new List<Datasets>();
            List<Datasets> _dataSet = new List<Datasets>();
            _dataSet.Add(new Datasets()
            {
                label = "Target",
                data = htA.Select(x => x.QTQty).ToArray(),
                backgroundColor = "rgb(88, 80, 141)",
                borderColor = "rgb(88, 80, 141)",
            });
            _dataSet.Add(new Datasets()
            {
                label = "Achievement",
                data = htA.Select(x => x.QAQty).ToArray(),
                backgroundColor = "rgb(255, 99, 97)",
                borderColor = "rgb(255, 99, 97)",

            });
            _chart.datasets = _dataSet;
            return _chart;

        }


        private Chart GetBartChart(List<dynamic> tvA)
        {
            Chart _chart = new Chart();
            _chart.labels = tvA.Select(x => x.Name).ToArray();
            _chart.datasets = new List<Datasets>();
            List<Datasets> _dataSet = new List<Datasets>();
            _dataSet.Add(new Datasets()
            {
                label = "Target Qty",
                data = tvA.Select(x => x.TQty ?? 0).ToArray(),
                borderColor = "rgb(88, 80, 141)",
                backgroundColor = "rgb(88, 80, 141)",
                borderWidth = "1"
            });
            _dataSet.Add(new Datasets()
            {
                label = "Achieved Qty",
                data = tvA.Select(x => x.AQty ?? 0).ToArray(),

                borderColor = "rgb(255, 99, 97)",
                backgroundColor = "rgb(255, 99, 97)",
                borderWidth = "1"
            });
            _chart.datasets = _dataSet;
            return _chart;
        }

        private Chart GetPiChart(List<dynamic> tvA)
        {
           
            Chart _chart = new Chart();
            _chart.labels = new[] { "Remaining", "Achieved" };
            _chart.datasets = new List<Datasets>();
            List<Datasets> _dataSet = new List<Datasets>();
          
            _dataSet.Add(new Datasets()
            {
                label = "Target VS Achieved",


                data = new object[] { tvA.Sum(x =>( x.TQty != null || x.AQty==null)? (x.TQty - x.AQty) : 0), tvA.Sum(x => x.AQty ?? 0) },
                backgroundColor = new string[] { "rgb(88, 80, 141)", "rgb(255, 99, 97)" },

                borderWidth = "1"
            });
           
            _chart.datasets = _dataSet;
            return _chart;
        }


        public ActionResult TnaActionAlert(string buyerRefId,string alertType)
        {
            ModelState.Clear();
            DataTable dataTable= _timeAndActionManager.GetBuyerWiseActive(PortalContext.CurrentUser.CompId,buyerRefId,alertType);
            return View(dataTable);
        }

        public ActionResult TnaActivityLog(long id,string keyName)
        {
            ModelState.Clear();
            DataTable dataTable = _timeAndActionManager.GetTnaActivityLog( id,keyName);
            return View(dataTable);
        }


    }
}