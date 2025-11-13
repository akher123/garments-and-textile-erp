using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web.Hosting;
using System.Web.Mvc;
using Humanizer.Localisation.NumberToWords;
using Microsoft.Reporting.WebForms;
using SCERP.BLL.IManager.ICommonManager;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.BLL.IManager.IProductionManager;
using SCERP.Common;
using SCERP.Common.Mail;
using SCERP.Model.Custom;
using SCERP.Web.Areas.Merchandising.Models.ViewModel;
using SCERP.Web.Areas.MIS.Models.ViewModel;
using SCERP.Web.Controllers;
using SCERP.Web.Helpers;
using SCERP.Web.Models;
using Spell;

namespace SCERP.Web.Areas.Merchandising.Controllers
{
    public class ReportController : BaseController
    {
        private readonly IBuyerOrderManager buyerOrderManager;
        private IMrcReportManager mrcReportManager;
        private IMarchandisingReportManager marchandisingReportManager;
        private IOmBuyerManager omBuyerManager;
        private IDailyFabricReceiveManager _dailyFabricReceiveManager;
        private IMerchandiserManager _merchandiserManager;
        private readonly IEmailSendManager _emailSendManager;
        public ReportController(IMerchandiserManager merchandiserManager,IMrcReportManager mrcReportManager,IDailyFabricReceiveManager dailyFabricReceiveManager, IBuyerOrderManager buyerOrderManager, IMarchandisingReportManager marchandisingReport, IOmBuyerManager omBuyerManager, IEmailSendManager emailSendManager)
        {
            _dailyFabricReceiveManager = dailyFabricReceiveManager;
            this.buyerOrderManager = buyerOrderManager;
            this.mrcReportManager = mrcReportManager;
            this.marchandisingReportManager = marchandisingReport;
            this.omBuyerManager = omBuyerManager;
            _emailSendManager = emailSendManager;
            _merchandiserManager = merchandiserManager;
        }
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult SpecSheetDetail(int? specificationSheetId)
        {
            List<SpecSheetModel> specSheet = new List<SpecSheetModel>();

            if (specificationSheetId != null)
                specSheet = mrcReportManager.GetSpecSheetDetail(specificationSheetId.Value);

            LocalReport lr = new LocalReport();

            string path = Path.Combine(Server.MapPath("~/Areas/Merchandising/Report"), "SpecSheetDetail.rdlc");
            if (System.IO.File.Exists(path))
                lr.ReportPath = path;
            else
                return View("Index");

            ReportDataSource rd = new ReportDataSource("SpecSheetDS", specSheet);
            lr.DataSources.Add(rd);

            string reportType = "pdf";
            string mimeType;
            string encoding;
            string fileNameExtension;

            string deviceInfo =
                "<DeviceInfo>" +
                "  <OutputFormat>" + 2 + "</OutputFormat>" +
                "  <PageWidth>11.69in</PageWidth>" +
                "  <PageHeight>8.27in</PageHeight>" +
                "  <MarginTop>0..2in</MarginTop>" +
                "  <MarginLeft>.4in</MarginLeft>" +
                "  <MarginRight>.2in</MarginRight>" +
                "  <MarginBottom>0.2in</MarginBottom>" +
                "</DeviceInfo>";

            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;

            renderedBytes = lr.Render(
                reportType,
                deviceInfo,
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);

            return File(renderedBytes, mimeType);
        }

        public ActionResult SpecSheetList(int? buyerId, string styleNo, string jobNo, DateTime? fromDate, DateTime? toDate)
        {
            List<SpecSheetModel> specSheet = new List<SpecSheetModel>();
            specSheet = mrcReportManager.GetSpecSheetList(buyerId, styleNo, jobNo, new DateTime(2015, 1, 1), new DateTime(2016, 1, 1));
            LocalReport lr = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/Areas/Merchandising/Report"), "SpecSheetList.rdlc");
            if (System.IO.File.Exists(path))
                lr.ReportPath = path;
            else
                return View("Index");

            ReportDataSource rd = new ReportDataSource("SpecSheetListDS", specSheet);
            lr.DataSources.Add(rd);

            string reportType = "pdf";
            string mimeType;
            string encoding;
            string fileNameExtension;

            string deviceInfo =
                "<DeviceInfo>" +
                "  <OutputFormat>" + 2 + "</OutputFormat>" +
                "  <PageWidth>11.69in</PageWidth>" +
                "  <PageHeight>8.27in</PageHeight>" +
                "  <MarginTop>0..2in</MarginTop>" +
                "  <MarginLeft>.4in</MarginLeft>" +
                "  <MarginRight>.2in</MarginRight>" +
                "  <MarginBottom>0.2in</MarginBottom>" +
                "</DeviceInfo>";

            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;

            renderedBytes = lr.Render(
                reportType,
                deviceInfo,
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);

            return File(renderedBytes, mimeType);
        }

        public ActionResult MerchaiserWiseOrderSummary(MerchandisingReportViewModel model)
        {
            if (!model.IsShowReport)
            {
                model.IsShowReport = true;
                return View(model);
            }
            else
            {
                model.DataTable = buyerOrderManager.GetMerchaiserWiseOrderDataTable(model.FromDate, model.ToDate);

                ReportParameter fromDate = new ReportParameter("FromDate", model.FromDate.ToString());
                ReportParameter toDate = new ReportParameter("ToDate", model.ToDate.ToString());
                var reportDataSource = new ReportDataSourceModel()
                {
                    DataSource = model.DataTable,
                    Path = "~/Areas/Merchandising/Report/RDLC/MerchandiserWiseOrderSummary.rdlc",
                    DataSetName = "MerchandiserWiseOrderSummaryDataSet",
                    ReportParameters = new[] { fromDate, toDate }
                };
                return PartialView("~/Views/Shared/SSRSReportViwerAPX.aspx", reportDataSource);
            }

        }
        public ActionResult BuyerWiseOrderSummary(MerchandisingReportViewModel model)
        {
            model.Buyers = omBuyerManager.GetAllBuyers();
            if (!model.IsShowReport)
            {
                model.IsShowReport = true;
                return View(model);
            }
            else
            {
                model.DataTable = marchandisingReportManager.GetBuyerWiseOrderSummaryDataTable(model.FromDate,
                    model.ToDate, model.BuyerRefId);
                ReportParameter fromDateParameter = new ReportParameter("FromDate", model.FromDate.ToString());
                ReportParameter toDateParameter = new ReportParameter("ToDate", model.ToDate.ToString());
                var reportDataSource = new ReportDataSourceModel()
                {
                    DataSource = model.DataTable,
                    Path = "~/Areas/Merchandising/Report/RDLC/BuyerWiseOrderSummary.rdlc",
                    DataSetName = "BuyerWiseOrderSummaryDataSet",
                    ReportParameters = new[] { fromDateParameter, toDateParameter }
                };
                return PartialView("~/Views/Shared/ReportViwerAPX.aspx", reportDataSource);
            }

        }
        public ActionResult ConfirmedOrderStatus(MerchandisingReportViewModel model)
        {

            if (!model.IsShowReport)
            {
                model.IsShowReport = true;
                return View(model);
            }
            else
            {
                model.DataTable = marchandisingReportManager.GetConfirmedOrderStatus();

                var reportDataSource = new ReportDataSourceModel()
                {
                    DataSource = model.DataTable,
                    Path = "~/Areas/Merchandising/Report/RDLC/ConfirmedOrderStatus.rdlc",
                    DataSetName = "ConfirmedOrderStatusDataSet",

                };
                return PartialView("~/Views/Shared/ReportViwerAPX.aspx", reportDataSource);
            }

        }
        public ActionResult RunningBuyerOrderStatus(MerchandisingReportViewModel model)
        {
                model.DataTable = marchandisingReportManager.GetRunningOrderOrderStatus(PortalContext.CurrentUser.CompId);
                var reportDataSource = new ReportDataSourceModel()
                {
                    DataSource = model.DataTable,
                    Path = "~/Areas/Merchandising/Report/RDLC/RunningBuyerOrderStatus.rdlc",
                    DataSetName = "RunBuyerOrderStatusDSet",

                };
                return PartialView("~/Views/Shared/ReportViwerAPX.aspx", reportDataSource);

        }
        public ActionResult DetailOrderStatus(MerchandisingReportViewModel model)
        {

            if (!model.IsShowReport)
            {
                model.IsShowReport = true;
                return View(model);
            }
            else
            {
                model.DataTable = marchandisingReportManager.GetDetailOrderStatus();

                var reportDataSource = new ReportDataSourceModel()
                {
                    DataSource = model.DataTable,
                    Path = "~/Areas/Merchandising/Report/RDLC/DetaliOrderStatus.rdlc",
                    DataSetName = "DetailOrderStatusDataSet",

                };
                return PartialView("~/Views/Shared/ReportViwerAPX.aspx", reportDataSource);
            }

        }

        public ActionResult ProductionStatus(MerchandisingReportViewModel model)
        {
            ModelState.Clear();
     
            if (!model.IsShowReport)
            {
                model.Buyers = omBuyerManager.GetAllBuyers();
                model.IsShowReport = true;
                
            }
            else
            {
                model.DataTable = marchandisingReportManager.GetProductionStatus(model.FromDate,model.ToDate);
                ReportParameter fromDateParameter = new ReportParameter("FromDate", model.FromDate.ToString());
                ReportParameter toDateParameter = new ReportParameter("ToDate", model.ToDate.ToString());
                ReportParameter[] reportParameters = new[] { fromDateParameter, toDateParameter };
                var reportDataSource = new ReportDataSourceModel()
                {
                    DataSource = model.DataTable,
                    Path = "~/Areas/Merchandising/Report/RDLC/ProductionStatusReport.rdlc",
                    DataSetName = "ProductionStatusDataSet",
                    ReportParameters = reportParameters
                   
                };
                return PartialView("~/Views/Shared/ReportViwerAPX.aspx", reportDataSource);

            }
            return View(model);
        }


        public ActionResult StyleWiseProductionStatus(MerchandisingReportViewModel model)
        {
            ModelState.Clear();
            if (!model.IsShowReport)
            {
              
                model.IsShowReport = true;

            }
            else
            {
                model.DataTable = marchandisingReportManager.GetProductionStatus(model.FromDate,model.ToDate);
                ReportParameter fromDateParameter = new ReportParameter("FromDate", model.FromDate.ToString());
                ReportParameter toDateParameter = new ReportParameter("ToDate", model.ToDate.ToString());
                ReportParameter[] reportParameters = new[] { fromDateParameter, toDateParameter };
                var reportDataSource = new ReportDataSourceModel()
                {
                    DataSource = model.DataTable,
                    Path = "~/Areas/Merchandising/Report/RDLC/StyleWiseProdStatus.rdlc",
                    DataSetName = "StyleWiseProdStatusDataSource",
                    ReportParameters = reportParameters
                };
                return PartialView("~/Views/Shared/ReportViwerAPX.aspx", reportDataSource);

            }
            return View(model);
        }
        public ActionResult StyleProductionDetail(MerchandisingReportViewModel model)
        {
            ModelState.Clear();
            if (!model.IsShowReport)
            {
                model.Buyers = omBuyerManager.GetAllBuyers();
                model.IsShowReport = true;

            }
            else
            {
                model.DataTable = marchandisingReportManager.GetStyleWiseProduction(model.OrderStyleRefId,
                    PortalContext.CurrentUser.CompId);
                var reportDataSource = new ReportDataSourceModel()
                {
                    DataSource = model.DataTable,
                    Path = "~/Areas/Merchandising/Report/RDLC/StyleProductionDetailReport.rdlc",
                    DataSetName = "StyleWiseProdStatusDSet",
                 
                };
                return PartialView("~/Views/Shared/ReportViwerAPX.aspx", reportDataSource);

            }
            return View(model);
        }

        public ActionResult FabricCosnumption(MerchandisingReportViewModel model)
        {
            ModelState.Clear();
            model.Buyers = omBuyerManager.GetAllBuyers();
            if (!model.IsShowReport)
            {
                model.IsShowReport = true;
                model.RecevedDate = DateTime.Now;
            }
            else
            {
                var dailyFabricReceiveReport = _dailyFabricReceiveManager.GetVwReceivedFabricProductionSummary(model.SearchString, model.BuyerRefId, model.RecevedDate);
                var receivedDate = new ReportParameter("ReceivedDate", model.RecevedDate.ToString());
                var reportDataSource = new ReportDataSourceModel()
                {
                    DataSource = dailyFabricReceiveReport,
                    Path = "~/Areas/Merchandising/Report/RDLC/DailyFabricReceiveReport.rdlc",
                    DataSetName = "FabricConsumptionDS",
                    ReportParameters = new[] { receivedDate }
                };
                return PartialView("~/Views/Shared/ReportViwerAPX.aspx", reportDataSource);

            }
            return View(model);
        }

   

        public ActionResult PandingConsumptionReport(MerchandisingReportViewModel model)
        {
            ModelState.Clear();
            model.Merchandisers = _merchandiserManager.GetMerchandisers();
            if (!model.IsShowReport)
            {
                model.IsShowReport = true;
                model.RecevedDate = DateTime.Now;
            }
            else
            {
                var pandingConsumptionDataTable = marchandisingReportManager.GetPandingConsumptionDataTable(model.MerchandiserId);
                var reportDataSource = new ReportDataSourceModel()
                {
                    DataSource =pandingConsumptionDataTable,
                    Path = "~/Areas/Merchandising/Report/RDLC/PandingConsumptionReport.rdlc",
                    DataSetName = "MrcDataSet",
                };
                return PartialView("~/Views/Shared/ReportViwerAPX.aspx", reportDataSource);

            }
            return View(model);
        }
        public ActionResult GetPoSheet(long purchaseOrderId, int reportTypeId)
        {
            var reportType = new ReportType();
            switch (reportTypeId)
            {
                case 1:
                    reportType = ReportType.PDF;
                    break;
                case 3:
                    reportType = ReportType.Excel;
                    break;
            }
            DataTable dataTable = marchandisingReportManager.GetPoSheet(purchaseOrderId);
          //  string amount = dataTable.Compute("sum(Quantity*xRate)",string.Empty).ToString();
            decimal sumTotal = dataTable.AsEnumerable()
             .Sum(r => r.Field<decimal>("Quantity") * r.Field<decimal>("xRate"));
            string inWord = SpellAmount.InWrods(sumTotal);
            string path = Path.Combine(Server.MapPath("~/Areas/Merchandising/Report/RDLC"), "AccessoriesOrderReport.rdlc");
            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("AccesssoriesOrderDset", dataTable) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 11.69, PageHeight = 8.27, MarginTop = .2, MarginLeft = 0.2, MarginRight = 0.2, MarginBottom = .2 };
            return ReportExtension.ToFile(reportType, path, reportDataSources, deviceInformation);
        }
        public ActionResult FabricWorkOrderSheet(string orderStyleRefId)
        {
            DataTable fabricWorkOrderSheetdaTable = marchandisingReportManager.GetFabricWorkOrderSheet(orderStyleRefId);
            string path = Path.Combine(Server.MapPath("~/Areas/Merchandising/Report/RDLC"), "FabricWorkOrder.rdlc");
            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("FabricWorkOrderDSet", fabricWorkOrderSheetdaTable) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 8.27, PageHeight = 11.69, MarginTop = .2, MarginLeft = 0.3, MarginRight = 0.2, MarginBottom = 0.2 };
            return ReportExtension.ToFile(ReportType.PDF, path, reportDataSources, deviceInformation);

        }

        public ActionResult CollarCuffBulkFabricOrderSheet(int fabricOrderId, int reportTypeId)
        {
            var reportType = new ReportType();
            switch (reportTypeId)
            {
                case 1:
                    reportType = ReportType.PDF;
                    break;
                case 3:
                    reportType = ReportType.Excel;
                    break;
            }
            DataTable bulkfabricOrderSheetdaTable = marchandisingReportManager.GetCollarCuffBulkFabricOrderSheet(fabricOrderId);
         
            string path = Path.Combine(Server.MapPath("~/Areas/Merchandising/Report/RDLC"), "CollarCuffBulkOrder.rdlc");
            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("CollarCuffBulkOrderDset", bulkfabricOrderSheetdaTable) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 14, PageHeight = 8.5, MarginTop = .2, MarginLeft = 0.2, MarginRight = 0.1, MarginBottom = 0.2 };
            return ReportExtension.ToFile(reportType, path, reportDataSources, deviceInformation);
        }

        public ActionResult BulkFabricOrderSheet(int fabricOrderId, int reportTypeId)
        {
            var reportType = new ReportType();
            switch (reportTypeId)
            {
                case 1:
                    reportType = ReportType.PDF;
                    break;
                case 3:
                    reportType = ReportType.Excel;
                    break;
            }
            DataTable bulkfabricOrderSheetdaTable = marchandisingReportManager.GetBulkFabricOrderSheet(fabricOrderId);
            string path = Path.Combine(Server.MapPath("~/Areas/Merchandising/Report/RDLC"), "FabricBulkOrderReport.rdlc");
            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("FabBulkOrderDset", bulkfabricOrderSheetdaTable) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 14, PageHeight = 8.5, MarginTop = .2, MarginLeft = 0.2, MarginRight = 0.2, MarginBottom = 0.2 };
            return ReportExtension.ToFile(reportType, path, reportDataSources, deviceInformation);
        }
        public ActionResult FabricWorkOrderDetailSheet(string orderStyleRefId)
        {

            DataTable fabricWorkOrderSheetdaTable = marchandisingReportManager.GetFabricWorkOrderDetailSheet(orderStyleRefId);
            string path = Path.Combine(Server.MapPath("~/Areas/Merchandising/Report/RDLC"), "FabricWorkOrderDetailSheet.rdlc");
            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("FabricSheetDSet", fabricWorkOrderSheetdaTable) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 11.69, PageHeight = 8.27, MarginTop = .2, MarginLeft = 0.2, MarginRight = 0.2, MarginBottom = 0.2 };
            return ReportExtension.ToFile(ReportType.PDF, path, reportDataSources, deviceInformation);

        }
        public ActionResult YarnWorkOrderSheet(long purchaseOrderId, int reportTypeId)
        {
            var reportType = new ReportType();
            switch (reportTypeId)
            {
                case 1:
                    reportType = ReportType.PDF;
                    break;
                case 3:
                    reportType = ReportType.Excel;
                    break;
            }
            DataTable yarnWorkOrderSheetdaTable = marchandisingReportManager.GetYarnWorkOrderSheet(purchaseOrderId);
            string path = Path.Combine(Server.MapPath("~/Areas/Merchandising/Report/RDLC"), "YarnWorkOrderReport.rdlc");
            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("YarnWorkOrderDset", yarnWorkOrderSheetdaTable) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 8.27, PageHeight = 11.69, MarginTop = 1, MarginLeft = 0.2, MarginRight = 0.1, MarginBottom = 0 };
            return ReportExtension.ToFile(reportType, path, reportDataSources, deviceInformation);

        }

        public ActionResult BulkYarnBooking(long bulkBookingId)
        {
            DataTable bulkYarnBkDta = marchandisingReportManager.GetBulkYarnBooking(bulkBookingId);
            var path = Path.Combine(Server.MapPath("~/Areas/Merchandising/Report/RDLC"), "BulkYarnBookingReport.rdlc");
            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("BulkYarnBkDSet", bulkYarnBkDta) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 14, PageHeight = 8.5, MarginTop = 1, MarginLeft = 0.1, MarginRight = 0.1, MarginBottom =.1 };
            return ReportExtension.ToFile(ReportType.PDF, path, reportDataSources, deviceInformation);

        }
        public ActionResult RunningOrderStatusSummary()
        {
            DataTable runningOrderStatusDt = marchandisingReportManager.GetRunningOrderStatus();
            string path = Path.Combine(Server.MapPath("~/Areas/Merchandising/Report/RDLC"), "RunningOrderSummaryReport.rdlc");
            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("RunningOrderStatusSummaryDSet", runningOrderStatusDt) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 8.27, PageHeight = 11.69, MarginTop = .1, MarginLeft = 2, MarginRight =1.5, MarginBottom = 0.1 };
            return ReportExtension.ToFile(ReportType.PDF, path, reportDataSources, deviceInformation);

        }

        public ActionResult SeasonWiseOrderSummary(MerchandisingReportViewModel model)
        {
            if (!model.IsShowReport)
            {
                model.IsShowReport = true;
                model.RecevedDate = DateTime.Now;
                return View(model);
            }
            else
            {
                DataTable seasonWiseOrderSummaryDataTable = marchandisingReportManager.GetSeasonWiseOrderSummary(model.FromDate,model.ToDate);
                string path = Path.Combine(Server.MapPath("~/Areas/Merchandising/Report/RDLC"), "OrderStatusReport.rdlc");
                if (!System.IO.File.Exists(path))
                {
                    return PartialView("~/Views/Shared/Error.cshtml");
                }
                var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("OrderStatusDataSet", seasonWiseOrderSummaryDataTable) };
                var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 11.69, PageHeight = 8.27, MarginTop = .2, MarginLeft = .3, MarginRight = .2, MarginBottom = 0.1 };
                return ReportExtension.ToFile(ReportType.PDF, path, reportDataSources, deviceInformation);
            }

        }

        public ActionResult OrderShipmentSummary(MerchandisingReportViewModel model)
        {
            if (!model.IsShowReport)
            {
                model.IsShowReport = true;
                model.FromDate = DateTime.Now;
                model.ToDate = DateTime.Now.AddMonths(1);
                return View(model);
            }
            else
            {
                var fromDateParameter = new ReportParameter("FromDate", model.FromDate.ToString());
                var toDateParameter = new ReportParameter("ToDate", model.ToDate.ToString());
                List<ReportParameter> reportParameters = new List<ReportParameter>() { fromDateParameter, toDateParameter };
                Guid? userId= PortalContext.CurrentUser.UserId;
                DataTable seasonWiseOrderSummaryDataTable = marchandisingReportManager.GetOrderShipmentSummary(model.FromDate, model.ToDate, userId);
                string path = Path.Combine(Server.MapPath("~/Areas/Merchandising/Report/RDLC"), "BuyerOrderShipSummary.rdlc");
                if (!System.IO.File.Exists(path))
                {
                    return PartialView("~/Views/Shared/Error.cshtml");
                }
                var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("BuyerShipSummaryDSet", seasonWiseOrderSummaryDataTable) };
                var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 11.69, PageHeight = 8.27, MarginTop = .2, MarginLeft = .2, MarginRight = .2, MarginBottom = 0.2 };
                return ReportExtension.ToFile(ReportType.PDF, path, reportDataSources, deviceInformation,reportParameters);
            }
        }

        public ActionResult CostSheet(string orderStyleRefId,bool isMail)
        {

            DataTable dataTable = marchandisingReportManager.GetPreCostSheet(orderStyleRefId,PortalContext.CurrentUser.CompId);
            string path = Path.Combine(Server.MapPath("~/Areas/Merchandising/Report/RDLC"), "CostSheetReport.rdlc");
            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }

             var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("CostSheetDSet", dataTable) };
             var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 8.27, PageHeight = 11.69, MarginTop = .2, MarginLeft = 0.1, MarginRight = 0.1, MarginBottom = .2 };
             var report= ReportExtension.ToWhiteFile(ReportType.PDF, path, reportDataSources, deviceInformation);
            if (isMail)
            {
                DbEmailModel dbEmail = _emailSendManager.GetDbEmailByTemplateId(EmailTemplateRefId.PREE_COST_SHEET, PortalContext.CurrentUser.CompId);
                dbEmail.Subject = "PRE COST SHEET FROM " + PortalContext.CurrentUser.Name;
                dbEmail.Body = "PRE COST SHEET IS CREATED BY :" + PortalContext.CurrentUser.Name;
                dbEmail.FileAttachments = HostingEnvironment.MapPath(AppConfig.ExportReportFillPath + "." + ReportType.PDF);
                bool send = _emailSendManager.SendDbEmail(dbEmail);
            }
        
            return report;
        }


        public ActionResult ShipmentAlert(MisDashboardViewModel model)
        {

            DataTable dataTable = marchandisingReportManager.GetShipmentAlert(model.BuyerRefId, model.OrderNo, model.OrderStyleRefId, PortalContext.CurrentUser.CompId);
            string path = Path.Combine(Server.MapPath("~/Areas/Merchandising/Report/RDLC"), "ShipmentAlretReport.rdlc");
            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("ShipAlretDSet", dataTable) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth =11, PageHeight = 8.5, MarginTop = .2, MarginLeft = 0.2, MarginRight = 0.2, MarginBottom = .2 };
            return ReportExtension.ToFile(ReportType.PDF, path, reportDataSources, deviceInformation);
        }
    }
}