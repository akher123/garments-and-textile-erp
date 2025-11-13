using System;
using System.Collections;
using System.Data;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Hosting;
using System.Web.Mvc;
using Microsoft.Reporting.WebForms;
using SCERP.BLL.IManager.ICommonManager;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.BLL.IManager.IProductionManager;
using SCERP.Common;
using SCERP.Common.Mail;
using SCERP.Model.Production;
using SCERP.Web.Areas.Production.Models;
using SCERP.Web.Controllers;
using SCERP.Web.Helpers;
using SCERP.Web.Models;

namespace SCERP.Web.Areas.Production.Controllers
{
    public class ProductionReportController : BaseController
    {
        private readonly ISewingInputProcessManager _sewingInputProcessManager;
        private readonly ISewingOutPutProcessManager _sewingOutPutProcessManager;
        private readonly IProductionReportManager _productionReportManager;
        private readonly IOmBuyOrdStyleManager _buyOrdStyleManager;
        private readonly IOmBuyerManager _buyerManager;
        private readonly ICuttingSequenceManager _cuttingSequenceManager;
        private readonly IRollCuttingManager _rollCuttingManager;
        private readonly IGraddingManager _graddingManager;
        private readonly ICuttingBatchManager _cuttingBatchManager;
        private readonly IProcessDeliveryManager _processDeliveryManager;
        private readonly ICuttingTagManager _cuttingTagManager;
        private readonly IBatchManager _batchManager;
        private readonly IKnittingRollManager _knittingRollManager;
        private readonly IEmailSendManager _emailSendManager;
        private readonly IProcessReceiveManager _processReceive;
        public ProductionReportController(IBatchManager batchManager, ISewingInputProcessManager sewingInputProcessManager, ISewingOutPutProcessManager sewingOutPutProcessManager, ICuttingTagManager cuttingTagManager, IProcessReceiveManager processReceive, IProcessDeliveryManager processDeliveryManager, ICuttingBatchManager cuttingBatchManager, IGraddingManager graddingManager, IRollCuttingManager rollCuttingManager, ICuttingSequenceManager cuttingSequenceManager, IOmBuyerManager buyerManager, IOmBuyOrdStyleManager buyOrdStyleManager, IProductionReportManager productionReportManager, IBundleCuttingManager bundleCuttingManager, IKnittingRollManager knittingRollManager, IEmailSendManager emailSendManager)
        {
            _batchManager = batchManager;
            _sewingInputProcessManager = sewingInputProcessManager;
            _sewingOutPutProcessManager = sewingOutPutProcessManager;
            _productionReportManager = productionReportManager;
            _buyOrdStyleManager = buyOrdStyleManager;
            _buyerManager = buyerManager;
            _cuttingSequenceManager = cuttingSequenceManager;
            _rollCuttingManager = rollCuttingManager;
            _graddingManager = graddingManager;
            _cuttingBatchManager = cuttingBatchManager;
            _processDeliveryManager = processDeliveryManager;
            _processReceive = processReceive;
            _cuttingTagManager = cuttingTagManager;
            _knittingRollManager = knittingRollManager;
            _emailSendManager = emailSendManager;
        }

        public ActionResult DailyKnittingRollSummary(KnittingRollViewModel model, int reportTypeId)
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
         
            DataTable knittingRollDataTable = _knittingRollManager.GetDailyKnittingRollSummaryByDate(model.FromDate ?? DateTime.Now.Date, PortalContext.CurrentUser.CompId);
            string path = Path.Combine(Server.MapPath("~/Areas/Production/Report"), "PartyWiseDailyKnittingRoll.rdlc");
            ReportParameter filterDate = new ReportParameter("FilterDate", model.FromDate.GetValueOrDefault().ToString("dd/MM/yyyy"));
            ReportParameter[] reportParameters = new[] { filterDate };
            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("DataSet1", knittingRollDataTable) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 8.27 , PageHeight = 11.69, MarginTop = .25, MarginLeft = .25, MarginRight = .25, MarginBottom = .25 };
            return ReportExtension.ToFile(reportType, path, reportDataSources, deviceInformation, reportParameters.ToList());
        }
        public ActionResult DailyKnittingRollDetail(KnittingRollViewModel model, int reportTypeId)
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
            List<VwKnittingRoll> knittingRolls = _knittingRollManager.GetDailyKnittingRollByDate(model.FromDate ?? DateTime.Now.Date, PortalContext.CurrentUser.CompId);
            string path = Path.Combine(Server.MapPath("~/Areas/Production/Report"), "DailyKnittingRollDetail.rdlc");
            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("DataSet1", knittingRolls) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 11.69, PageHeight = 8.27, MarginTop = .2, MarginLeft = 0.1, MarginRight = 0.1, MarginBottom = .2 };
            return ReportExtension.ToFile(reportType, path, reportDataSources, deviceInformation);
        }
    
        public ActionResult PrintEmbroideryBalance(PrintEmbroideryBalanceViewModel model, int reportTypeId)
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
            List<SpPrintEmbroiderySummary> printEmbroideryBalanceList = _cuttingTagManager.GetPrintEmbroideryBalance(model.CuttingBatch.CuttingBatchRefId, model.CuttingBatch.BuyerRefId, model.CuttingBatch.OrderNo, model.CuttingBatch.OrderStyleRefId, model.CuttingBatch.ColorRefId);
            string path = Path.Combine(Server.MapPath("~/Areas/Production/Report"), "PrintEmbroideryBalance.rdlc");
            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("DataSet1", printEmbroideryBalanceList) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 11.69, PageHeight = 8.27, MarginTop = .2, MarginLeft = 0.2, MarginRight = 0.2, MarginBottom = .2 };
            return ReportExtension.ToFile(reportType, path, reportDataSources, deviceInformation);
        }
        public ActionResult StyleAndColorWiseInput(SewingInputProcessViewModel model, int reportTypeId)
        {
            ModelState.Clear();
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
            List<VwSewingInputProcess> VwSewingInputProcesses = _sewingInputProcessManager.GetSewingInputProcessByStyleColor(model.SewingInputProcess.OrderStyleRefId, model.SewingInputProcess.ColorRefId, model.SewingInputProcess.OrderShipRefId);
            string path = Path.Combine(Server.MapPath("~/Areas/Production/Report"), "StyleAndColorWiseInput.rdlc");
            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("DataSet1", VwSewingInputProcesses.OrderBy(x => x.MachineName)) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 11, PageHeight = 8.50, MarginTop = .3, MarginLeft = 0.2, MarginRight = 0.2, MarginBottom = .2 };
            return ReportExtension.ToFile(reportType, path, reportDataSources, deviceInformation);
        }
        public ActionResult SewingWIP(SewingOutputProcessViewModel model, int reportTypeId)
        {
            ModelState.Clear();
            if (model.SewingOutPutProcess.HourId < 0)
            {
                return ErrorResult("Select Hour from dropdown list.");
            }
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
            DataTable sewingwipDataTable = _sewingOutPutProcessManager.GetSewingWIP(model.SewingOutPutProcess.OutputDate, model.SewingOutPutProcess.HourId, PortalContext.CurrentUser.CompId) ?? new DataTable();
            string path = Path.Combine(Server.MapPath("~/Areas/Production/Report"), "SewingWIP.rdlc");
            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("DataSet1", sewingwipDataTable) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 11, PageHeight = 8.50, MarginTop = .3, MarginLeft = 0.2, MarginRight = 0.2, MarginBottom = .2 };
            return ReportExtension.ToFile(reportType, path, reportDataSources, deviceInformation);

        }
        public ActionResult SewingWIPShot(SewingOutputProcessViewModel model, int reportTypeId)
        {
            ModelState.Clear();
            if (model.SewingOutPutProcess.HourId < 0)
            {
                return ErrorResult("Select Hour from dropdown list.");
            }
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
            DataTable sewingwipDataTable = _sewingOutPutProcessManager.GetSewingWIP(model.SewingOutPutProcess.OutputDate, model.SewingOutPutProcess.HourId, PortalContext.CurrentUser.CompId) ?? new DataTable();
            string path = Path.Combine(Server.MapPath("~/Areas/Production/Report"), "SewingWIPShot.rdlc");
            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("DataSet1", sewingwipDataTable) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 8.27, PageHeight = 11.69, MarginTop = .5, MarginLeft = 0.5, MarginRight = 0.4, MarginBottom = .3 };
            return ReportExtension.ToFile(reportType, path, reportDataSources, deviceInformation);

        }
        public ActionResult SewingWIPDetail(SewingOutputProcessViewModel model, int reportTypeId)
        {
            ModelState.Clear();
            model.SewingOutPutProcess.OutputDate = Convert.ToDateTime(model.SewingOutPutProcess.OutputDate);
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
            DataTable sewingwipDetailDataTable = _sewingOutPutProcessManager.GetSewingWIPDetail(model.SewingOutPutProcess.OutputDate, PortalContext.CurrentUser.CompId) ?? new DataTable();
            string path = Path.Combine(Server.MapPath("~/Areas/Production/Report"), "SewingWIPDetail.rdlc");
            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("DataSet1", sewingwipDetailDataTable) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 8.27, PageHeight = 11.69, MarginTop = .2, MarginLeft = 0.2, MarginRight = 0.2, MarginBottom = .2 };
            return ReportExtension.ToFile(reportType, path, reportDataSources, deviceInformation);
        }
        public ActionResult DailySewingOutput(SewingOutputProcessViewModel model, int reportTypeId)
        {
            ModelState.Clear();
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
            List<VwSewingOutputProcess> sewingOutputs = _sewingOutPutProcessManager.GetDailySewingOutForReport(model.SewingOutPutProcess.OutputDate, model.SewingOutPutProcess.LineId);
            string path = Path.Combine(Server.MapPath("~/Areas/Production/Report"), "DailySewingOutput.rdlc");
            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("DataSet1", sewingOutputs) }; ;
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 8.27, PageHeight = 11.69, MarginTop = .4, MarginLeft = 0.2, MarginRight = 0.2, MarginBottom = .2 };
            return ReportExtension.ToFile(reportType, path, reportDataSources, deviceInformation);

        }
        public ActionResult DailyUnitWiseHourlyProductionReprot(DateTime outputDate)
        {

            string compId = PortalContext.CurrentUser.CompId;
            DataTable dataTable = _productionReportManager.GetUnitWiseHourlyProduction(compId, outputDate);
            DataTable pforecastDataTable = _productionReportManager.GetSewingUnitProductionForecasting(compId, outputDate);
            string path = Path.Combine(Server.MapPath("~/Areas/Production/Report"), "UnitWiseHourlySewingProduction.rdlc");
            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("UnitWiseHourlySewingProductionDSet", dataTable), new ReportDataSource("ProductonForecustDSet", pforecastDataTable) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 11.69, PageHeight = 8.27, MarginTop =.5, MarginLeft = .2, MarginRight = .2, MarginBottom =.1 };
            return ReportExtension.ToFile(ReportType.PDF, path, reportDataSources, deviceInformation);
        }

        public ActionResult DalilyLineWiseTargetVsProduction(DateTime outputDate)
        {

            string compId = PortalContext.CurrentUser.CompId;

            DataTable dataTable = _productionReportManager.GetDalilyLineWiseTargetVsProduction(compId, outputDate);
            string path = Path.Combine(Server.MapPath("~/Areas/Production/Report"), "DailyLineWiseTargetVAchivedReport.rdlc");
            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("LineWiseTvADSet", dataTable)};
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 11.69, PageHeight = 8.27, MarginTop = .1, MarginLeft = .1, MarginRight = .1, MarginBottom = 1 };
            return ReportExtension.ToFile(ReportType.PDF, path, reportDataSources, deviceInformation);
        }

        public ActionResult DailyJobCard(CuttingBatchViewModel model, int reportTypeId)
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
            List<VwCuttingBatch> cuttingBatchList = _cuttingBatchManager.GetAllCuttingBatchForReport(model.CuttingBatch.CuttingDate, model.SearchString,model.CuttingBatch.MachineId);
            string path = Path.Combine(Server.MapPath("~/Areas/Production/Report"), "DailyJob.rdlc");
            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("DataSet1", cuttingBatchList) }; ;
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 8.27, PageHeight = 11.69, MarginTop = .2, MarginLeft = 0.3, MarginRight = 0.2, MarginBottom = .2 };

            //ReportExtension.ToFile(reportType, "D:\\QA_TempFile\\Jobcard.pdf", reportDataSources, deviceInformation);

            //EmailSender emailSender = new EmailSender("smtpout.secureserver.net", 80, "info@soft-code.net", "info123");

            //emailSender.From = "info@soft-code.net";
            //emailSender.To = "kallol39@gmail.com";
            //emailSender.Subject = "Test Mail - 1";
            //emailSender.Body = "mail with attachment";
            //emailSender.AddAttachment("D:\\QA_TempFile\\Jobcard.pdf");
            //emailSender.Send();

            return ReportExtension.ToFile(reportType, path, reportDataSources, deviceInformation);
        }
        public ActionResult BundleSlip(string cuttingBatchRefId)
        {

            string compId = PortalContext.CurrentUser.CompId;
            List<VwProdBundleSlip> bundleSlips = _productionReportManager.GetBundleSlip(cuttingBatchRefId, compId);
             var jobCut= bundleSlips.FirstOrDefault();
             var reportName = "BundleSlipReportCountry.rdlc";
             if (jobCut!=null)
             {
                 if (jobCut.ComponentRefId!="001")
                 {
                     // reportName = "BundleSlipReport_V1.rdlc";
                     reportName = "BundleSlipReportCountry.rdlc";
                 }
             }
             string path = Path.Combine(Server.MapPath("~/Areas/Production/Report"), reportName);
            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("EVENDS", bundleSlips.Where(x => x.BundleNo % 2 == 0).OrderBy(x => x.BundleNo)), new ReportDataSource("ODDDS", bundleSlips.Where(x => x.BundleNo % 2 == 1).OrderBy(x => x.BundleNo)) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 8.27, PageHeight = 11.69, MarginTop = .2, MarginLeft = 0.3, MarginRight = 0.2, MarginBottom = .2 };
            return ReportExtension.ToFile(ReportType.PDF, path, reportDataSources, deviceInformation);
        }
        public ActionResult BundleChartReport(string cuttingBatchRefId)
        {
            string compId = PortalContext.CurrentUser.CompId;
            DataTable bundelChartDataTable = _productionReportManager.GetSpBundleChar(cuttingBatchRefId, compId);
            DataTable bundelDataTable = _productionReportManager.GetSpBundle(cuttingBatchRefId, compId);
            var rollCuttingList = _rollCuttingManager.GetRollCuttingByCuttingBatchRefId(cuttingBatchRefId);
            string path = Path.Combine(Server.MapPath("~/Areas/Production/Report"), "BundleChartReport.rdlc");
            var graddingList = _graddingManager.GetGradingListByCuttingBatch(rollCuttingList.First().CuttingBatchId);
            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("BundleChartDSet", bundelChartDataTable), new ReportDataSource("BundleDSet", bundelDataTable), new ReportDataSource("RollDSet", rollCuttingList), new ReportDataSource("Gradding", graddingList) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 8.27, PageHeight = 11.69, MarginTop = .2, MarginLeft = 0.3, MarginRight = 0.2, MarginBottom = .2 };
            return ReportExtension.ToFile(ReportType.PDF, path, reportDataSources, deviceInformation);
        }
        public ActionResult CuttingProductionSummary(ProductionReportViewModel model)
        {
            ModelState.Clear();
            if (!model.IsSearch)
            {
                model.Buyers = _buyerManager.GetCuttingProcessStyleActiveBuyers() as IEnumerable;
                model.IsSearch = true;
                model.CuttingBatch.CuttingDate = DateTime.Now;
                return View(model);
            }
            else
            {
                ReportParameter cuttDate = new ReportParameter("CuttDate", model.CuttingBatch.CuttingDate.GetValueOrDefault().ToString("dd/MM/yyyy"));
                ReportParameter[] reportParameters = new[] { cuttDate };

                model.CuttingBatch.ComponentRefId = model.CuttingBatch.ComponentRefId ?? "001";
                model.DataTable = _productionReportManager.GetSpProdCuttiongReportSummary(model.CuttingBatch.BuyerRefId, model.CuttingBatch.OrderNo, model.CuttingBatch.OrderStyleRefId, model.CuttingBatch.ComponentRefId, model.CuttingBatch.CuttingDate);
                var reportDataSource = new ReportDataSourceModel()

                {
                    DataSource = model.DataTable,
                    Path = "~/Areas/Production/Report/CuttingProductionSummaryReport.rdlc",
                    DataSetName = "DataSet1",
                    ReportParameters = reportParameters
                };
                return PartialView("~/Views/Shared/ReportViwerAPX.aspx", reportDataSource);
            }
        }
        public ActionResult DailyMonthWiseCuttingReport(ProductionReportViewModel model)
        {
           
             model.DataTable = _cuttingBatchManager.GetDailyMonthWiseCutting(model.YearId, PortalContext.CurrentUser.CompId);
             string path = Path.Combine(Server.MapPath("~/Areas/Production/Report"), "DayAndMonthWiseCuttingReport.rdlc");
             ReportParameter yearPram = new ReportParameter("YearId", model.YearId.ToString());
             ReportParameter[] reportParameters = new[] { yearPram };
            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("DayMonthCuttDSet", model.DataTable) }; 
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 15, PageHeight = 8.5, MarginTop = .1, MarginLeft = 0.1, MarginRight = 0.1, MarginBottom = .1 };
            return ReportExtension.ToFile(model.RptType, path, reportDataSources, deviceInformation, reportParameters.ToList());
        }
        public ActionResult CuttSummary(ProductionReportViewModel model)
        {
            ModelState.Clear();
            model.Buyers = _buyerManager.GetAllBuyers();
            model.IsSearch = true;
            model.CuttingBatch.CuttingDate = model.CuttingBatch.CuttingDate ?? DateTime.Now;
            model.DataTable = _productionReportManager.GetDailyProductionStatusSummary(model.CuttingBatch.CuttingDate.GetValueOrDefault());
            return View(model);

        }
        public ActionResult ProcessDeliverySummary(long partyId, string serachString)
        {
            string processRetId = ProcessCode.PRINTING;
            List<VwProcessDelivery> deliveries = _processDeliveryManager.ProcessDeliverySummaryReport(processRetId, serachString, partyId);
            string path = Path.Combine(Server.MapPath("~/Areas/Production/Report"), "ProcessDeliverySummary.rdlc");
            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("DataSet1", deliveries) }; ;
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 8.27, PageHeight = 11.69, MarginTop = .2, MarginLeft = 0.3, MarginRight = 0.2, MarginBottom = .2 };
            return ReportExtension.ToFile(ReportType.PDF, path, reportDataSources, deviceInformation);
        }
        public ActionResult DeliveryChalan(long processDeliveryId, string processRefId, int deliveryType)
        {
            var parameters = new List<ReportParameter>();
            string reportTitle = "";
            if (processRefId == ProcessCode.EMBROIDARY)
            {
                switch (deliveryType)
                {
                    case 1:
                        reportTitle = "EMBROIDERY DELIVERY CHALLAN (RETURNABLE)";
                        break;
                    case 2:
                        reportTitle = "EMBROIDERY DELIVERY GATE PASS ";
                        break;
                }
                parameters.Add(new ReportParameter("reportTitle", reportTitle));
            }
            else if (processRefId == ProcessCode.PRINTING)
            {
                switch (deliveryType)
                {
                    case 1:
                        reportTitle = "PRINT DELIVERY CHALLAN (RETURNABLE)";
                        break;
                    case 2:
                        reportTitle = "PRINT DELIVERY GATE PASS ";
                        break;
                }
                parameters.Add(new ReportParameter("reportTitle", reportTitle));
            }
            DataTable cuttingJobsummary = _productionReportManager.GetProcessDelivery(processDeliveryId);
            string path = Path.Combine(Server.MapPath("~/Areas/Production/Report"), "ProcessDeliveryReport.rdlc");
            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("ProcessDeliveryDSet", cuttingJobsummary) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 8.27, PageHeight = 11.69, MarginTop = .2, MarginLeft = .2, MarginRight = 0, MarginBottom = .2 };
            return ReportExtension.ToFile(ReportType.PDF, path, reportDataSources, deviceInformation, parameters);

        }
        public ActionResult JobWiseCuttingReport(CuttingBatchViewModel model)
        {
            ModelState.Clear();
            DataTable cuttingJobsummary = _productionReportManager.GetJobCuttingSummary(model.CuttingBatch.OrderStyleRefId, model.PartCutting.ComponentRefId, model.RollCutting.ColorRefId);
            DataTable cuttingJobDetail = _productionReportManager.GetJobCuttingDetail(model.CuttingBatch.OrderStyleRefId, model.PartCutting.ComponentRefId, model.RollCutting.ColorRefId);
            string path = Path.Combine(Server.MapPath("~/Areas/Production/Report"), "JobWiseCuttingReport.rdlc");
            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("JobCuttSummaryDSet", cuttingJobsummary), new ReportDataSource("JobCuttDetailDSet", cuttingJobDetail) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 8.27, PageHeight = 11.69, MarginTop = .2, MarginLeft = .5, MarginRight = 0.2, MarginBottom = .2 };
            return ReportExtension.ToFile(ReportType.PDF, path, reportDataSources, deviceInformation);


        }


        public ActionResult PartDesignSummary(ProductionReportViewModel model)
        {
            ModelState.Clear();
            if (!model.IsSearch)
            {
                model.Buyers = _buyerManager.GetAllBuyers();
                model.IsSearch = true;
                return View(model);
            }
            else
            {
                model.DataTable = _productionReportManager.GetPartDesignSummary(model.CuttingBatch.BuyerRefId, model.CuttingBatch.OrderNo, model.CuttingBatch.OrderStyleRefId, model.CuttingBatch.ComponentRefId);
                var reportDataSource = new ReportDataSourceModel()
                {
                    DataSource = model.DataTable,
                    Path = "~/Areas/Production/Report/PartDesignSummary.rdlc",
                    DataSetName = "DataSet1"
                };
                return PartialView("~/Views/Shared/ReportViwerAPX.aspx", reportDataSource);
            }
        }
        public ActionResult StyleWiseReport(ProductionReportViewModel model)
        {
            ModelState.Clear();
            if (!model.IsSearch)
            {
                model.Buyers = _buyerManager.GetAllBuyers();
                model.IsSearch = true;
                return View(model);
            }
            else
            {
                model.DataTable = _productionReportManager.GetSpProdStyleWiseTagCuttingReport(model.CuttingBatch.BuyerRefId, model.CuttingBatch.OrderNo, model.CuttingBatch.OrderStyleRefId, model.CuttingBatch.ComponentRefId);
                var reportDataSource = new ReportDataSourceModel()
                {
                    DataSource = model.DataTable,
                    Path = "~/Areas/Production/Report/StyleWiseCuttingReport.rdlc",
                    DataSetName = "DataSet1"
                };
                return PartialView("~/Views/Shared/ReportViwerAPX.aspx", reportDataSource);
            }
        }


        public ActionResult CuttingProductionDetail(ProductionReportViewModel model)
        {
            ModelState.Clear();
            if (!model.IsSearch)
            {
                model.Buyers = _buyerManager.GetAllBuyers();
                model.IsSearch = true;
                model.CuttingBatch.CuttingDate = DateTime.Now;
                return null;// return View(model);
            }
            else
            {

                ReportParameter cuttDate = new ReportParameter("CuttDate", model.CuttingBatch.CuttingDate.GetValueOrDefault().ToString("dd/MM/yyyy"));
                ReportParameter[] reportParameters = new[] { cuttDate };
                model.DataTable = _productionReportManager.GetSpProdCuttiongReportDetail(model.CuttingBatch.BuyerRefId, model.CuttingBatch.OrderNo, model.CuttingBatch.OrderStyleRefId, model.CuttingBatch.ComponentRefId, model.CuttingBatch.CuttingDate);

                var reportDataSource = new ReportDataSourceModel()
                {
                    DataSource = model.DataTable,
                    Path = "~/Areas/Production/Report/CuttingProductionDetailReport.rdlc",
                    DataSetName = "CuttingProductionDetailDset",
                    ReportParameters = reportParameters
                };
                return PartialView("~/Views/Shared/ReportViwerAPX.aspx", reportDataSource);
            }
        }
        public ActionResult ProcessReceiveReport(string ProcessReceiveRefId)
        {

            string compId = PortalContext.CurrentUser.CompId;
            DataTable processReceiveDetaliDt = _productionReportManager.GetProcessReceiveDetail(ProcessReceiveRefId, compId);
            string path = Path.Combine(Server.MapPath("~/Areas/Production/Report"), "ProcessReceiveReport.rdlc");
            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("ProcessReceiveDSet", processReceiveDetaliDt) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 8.27, PageHeight = 11.69, MarginTop = .2, MarginLeft = 0.3, MarginRight = 0.2, MarginBottom = .2 };
            return ReportExtension.ToFile(ReportType.PDF, path, reportDataSources, deviceInformation);
        }

        public ActionResult ReceiveBalanceStatusReport(string processRefId, int reportTypeId, long partyId, string searchString)
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
            var processReceive = _processReceive.GetProcessReceiveBalance(processRefId, partyId, searchString);
            string path = Path.Combine(Server.MapPath("~/Areas/Production/Report"), "ReceiveBalanceStatusReport.rdlc");
            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("ReeiveBalanceStatusDset", processReceive) }; ;
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 11.69, PageHeight = 8.27, MarginTop = .2, MarginLeft = 0.2, MarginRight = 0.2, MarginBottom = .2 };
            return ReportExtension.ToFile(reportType, path, reportDataSources, deviceInformation);

        }
        public ActionResult PrintBalanceStatusReport(ProductionReportViewModel model)
        {
            var reportType = new ReportType();

            switch (Convert.ToInt32(model.ReportType))
            {
                case 1:
                    reportType = ReportType.PDF;
                    break;
                case 3:
                    reportType = ReportType.Excel;
                    break;
            }
            string compId = PortalContext.CurrentUser.CompId;
            DataTable processDeliveryDtable = _productionReportManager.GetProcessDeliveryDatable(model.CuttingBatch.OrderStyleRefId, model.CuttingBatch.ColorRefId, model.CuttingTagId, compId, model.ProcessRefId);
            DataTable processReceiveDtable = _productionReportManager.GetProcessReceiveDataTable(model.CuttingBatch.OrderStyleRefId, model.CuttingBatch.ColorRefId, model.CuttingTagId, compId, model.ProcessRefId);
            DataTable factoryDTable = _productionReportManager.GetfactoryDataTable(model.CuttingBatch.OrderStyleRefId, model.CuttingBatch.ColorRefId, model.CuttingTagId, compId, model.ProcessRefId);
            string path = Path.Combine(Server.MapPath("~/Areas/Production/Report"), "ProcessBalanceStatus.rdlc");
            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("ProcessDeliveryDSet", processDeliveryDtable), new ReportDataSource("ProcessReceiveDSet", processReceiveDtable), new ReportDataSource("FactoryDSet", factoryDTable) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 8.27, PageHeight = 11.69, MarginTop = .2, MarginLeft = 0.3, MarginRight = 0.2, MarginBottom = .2 };
            return ReportExtension.ToFile(reportType, path, reportDataSources, deviceInformation);
        }

        public ActionResult FactoryStyleWiseBalanceReport(ProductionReportViewModel model)
        {
            string compId = PortalContext.CurrentUser.CompId;
            var reportType = new ReportType();

            switch (Convert.ToInt32(model.ReportType))
            {
                case 1:
                    reportType = ReportType.PDF;
                    break;
                case 3:
                    reportType = ReportType.Excel;
                    break;
            }
            DataTable dataTable = _productionReportManager.GetFactoryStyleWiseBalanceReport(compId, model.PartyId, model.CuttingBatch.OrderStyleRefId, model.ProcessRefId);
            DataTable minimumTable = _productionReportManager.GetMinimumSendReceive(compId, model.PartyId, model.CuttingBatch.OrderStyleRefId, model.ProcessRefId);

            string path = Path.Combine(Server.MapPath("~/Areas/Production/Report"), "FactoryStyleWiseReport.rdlc");
            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("FactoryStyleDSet", dataTable), new ReportDataSource("MinmumSendRecvDSet", minimumTable) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 14, PageHeight = 8.5, MarginTop = 0, MarginLeft = 0.3, MarginRight = 0.2, MarginBottom = 0.1 };

            return ReportExtension.ToFile(reportType, path, reportDataSources, deviceInformation);
        }

        public ActionResult PrintEmbroideryBalanceSummary(ProductionReportViewModel model)
        {
            string compId = PortalContext.CurrentUser.CompId;
            var reportType = new ReportType();

            switch (Convert.ToInt32(model.ReportType))
            {
                case 1:
                    reportType = ReportType.PDF;
                    break;
                case 3:
                    reportType = ReportType.Excel;
                    break;
            }
            DataTable dataTable = _productionReportManager.GetPrintEmbroideryBalanceSummaryt(compId, model.PartyId, model.CuttingBatch.OrderStyleRefId);

            string path = Path.Combine(Server.MapPath("~/Areas/Production/Report"), "PrintEmbBalanceSummaryReport.rdlc");
            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("ProcessDeliveryReceiveDSet", dataTable) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 8.27, PageHeight = 11.69, MarginTop = .2, MarginLeft = 0.1, MarginRight = 0.1, MarginBottom = 0.2 };
            return ReportExtension.ToFile(reportType, path, reportDataSources, deviceInformation);
        }
        public ActionResult ProcessDeliveryDetailReport(string orderStyleRefId)
        {
            string compId = PortalContext.CurrentUser.CompId;

            DataTable dataTable = _productionReportManager.GetProcessDeliveryDetailReportData(compId, orderStyleRefId, ProcessCode.PRINTING);
            string path = Path.Combine(Server.MapPath("~/Areas/Production/Report"), "ProcessDeliveryDetail.rdlc");
            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("ProcessDeliveryDetailDSet", dataTable) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 8.27, PageHeight = 11.69, MarginTop = .2, MarginLeft = 1, MarginRight = 1, MarginBottom = 0.2 };
            return ReportExtension.ToFile(ReportType.PDF, path, reportDataSources, deviceInformation);
        }
        public ActionResult CuttBankReport(string orderStyleRefId)
        {
            string compId = PortalContext.CurrentUser.CompId;
            DataTable dataTable = _productionReportManager.GetCuttBankData(compId, orderStyleRefId);
            string path = Path.Combine(Server.MapPath("~/Areas/Production/Report"), "CuttBankReport.rdlc");
            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("CutBankDSet", dataTable) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 11.69, PageHeight = 8.27, MarginTop = .2, MarginLeft = .2, MarginRight = .2, MarginBottom = 0.2 };
            return ReportExtension.ToFile(ReportType.PDF, path, reportDataSources, deviceInformation);
        }
        public ActionResult ProcessReceiveDetailReport(string orderStyleRefId)
        {
            string compId = PortalContext.CurrentUser.CompId;

            DataTable dataTable = _productionReportManager.GetReceiveDetailReportData(compId, orderStyleRefId, ProcessCode.PRINTING);
            string path = Path.Combine(Server.MapPath("~/Areas/Production/Report"), "ProcessReceiveDetailReport.rdlc");
            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("ProcessReceiveDetailDataSet", dataTable) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 8.27, PageHeight = 11.69, MarginTop = .2, MarginLeft = .2, MarginRight = .2, MarginBottom = 0.2 };
            return ReportExtension.ToFile(ReportType.PDF, path, reportDataSources, deviceInformation);
        }
        public ActionResult SweingInputReport(long sewingInputProcessId)
        {
            string compId = PortalContext.CurrentUser.CompId;

            DataTable dataTable = _productionReportManager.GetSweingInputReport(compId, sewingInputProcessId);
            string path = Path.Combine(Server.MapPath("~/Areas/Production/Report"), "SewingInputIssueNoteReport.rdlc");
            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("SIINODSET", dataTable) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 8.27, PageHeight = 11.69, MarginTop = 1, MarginLeft = .5, MarginRight = .5, MarginBottom = 1 };
            return ReportExtension.ToFile(ReportType.PDF, path, reportDataSources, deviceInformation);
        }
        public ActionResult HourlyProductionReprot(DateTime outputDate)
        {
            string compId = PortalContext.CurrentUser.CompId;
            DataTable dataTable = _productionReportManager.GetHourlyProductionDataTable(compId, outputDate);
            string path = Path.Combine(Server.MapPath("~/Areas/Production/Report"), "HourlyProductionReport.rdlc");
            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("HourlyProdDSet", dataTable) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 14, PageHeight = 8.5, MarginTop = .2, MarginLeft = .1, MarginRight = .1, MarginBottom = .2 };
            return ReportExtension.ToFile(ReportType.PDF, path, reportDataSources, deviceInformation);
        }

        public ActionResult SizeLineWiseSewingOutputReport(SewingOutputProcessViewModel model)
        {
            string compId = PortalContext.CurrentUser.CompId;
            DataTable dataTableSummary = _productionReportManager.GetSizeLineWiseSewingOutputSummary(compId, model.SewingOutPutProcess.OrderStyleRefId, model.SewingOutPutProcess.ColorRefId);
            DataTable dataTableDetail = _productionReportManager.GetSizeLineWiseSewingOutputDetail(compId, model.SewingOutPutProcess.OrderStyleRefId, model.SewingOutPutProcess.ColorRefId);
            string path = Path.Combine(Server.MapPath("~/Areas/Production/Report"), "SizeLineWiseSewingOutputReport.rdlc");
            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("SizeLineSwOutpurtDset", dataTableSummary), new ReportDataSource("SizeLineDetailDset", dataTableDetail) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 8.27, PageHeight = 11.69, MarginTop = .2, MarginLeft = .2, MarginRight = .1, MarginBottom = .2 };
            return ReportExtension.ToFile(ReportType.PDF, path, reportDataSources, deviceInformation);
        }

        public ActionResult LineSizeWiseSewingInputReport(SewingInputProcessViewModel model)
        {
            string compId = PortalContext.CurrentUser.CompId;
            DataTable dataTableSummary = _productionReportManager.GetSizeLineWiseSewingOutputSummary(compId, model.SewingInputProcess.OrderStyleRefId, model.SewingInputProcess.ColorRefId);
            DataTable dataTableDetail = _productionReportManager.GetSizeLineWiseSewingInputDetail(compId, model.SewingInputProcess.OrderStyleRefId, model.SewingInputProcess.ColorRefId);
            string path = Path.Combine(Server.MapPath("~/Areas/Production/Report"), "LineAndSizeWiseSewingInputReport.rdlc");
            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("LineSizeInputDset", dataTableSummary), new ReportDataSource("LineSizeInputDetailDSet", dataTableDetail) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 8.27, PageHeight = 11.69, MarginTop = .2, MarginLeft = .2, MarginRight = .1, MarginBottom = .2 };
            return ReportExtension.ToFile(ReportType.PDF, path, reportDataSources, deviceInformation);
        }


        public ActionResult ManMachineUtiliztiont(DateTime outputDate)
        {
            string compId = PortalContext.CurrentUser.CompId;
            DataTable dataTableLineStatus = _productionReportManager.GetLineStatus(compId, outputDate);
            DataTable dataTableManMachineUtiliztiont = _productionReportManager.GetManMachineUtiliztiont(outputDate);
            string path = Path.Combine(Server.MapPath("~/Areas/Production/Report"), "ManMachineUtilizationReport.rdlc");
           int totalProductionHr = _sewingOutPutProcessManager.GetTotalProductionHours(outputDate, compId);
            var pareParameters = new List<ReportParameter>() { new ReportParameter("TotalProHr", Convert.ToString(totalProductionHr)) };
            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("LineStatusDSet", dataTableLineStatus), new ReportDataSource("ManMachineUtilizationDSet", dataTableManMachineUtiliztiont) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 8.27, PageHeight = 11.69, MarginTop = .2, MarginLeft = .7, MarginRight = .7, MarginBottom = .2 };
            return ReportExtension.ToFile(ReportType.PDF, path, reportDataSources, deviceInformation, pareParameters);
        }
        public ActionResult GetOrderByBuyer(string buyerRefId)
        {
            IEnumerable orderList = _buyOrdStyleManager.GetOrderByBuyer(buyerRefId);
            return Json(orderList, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetStyleByOrderNo(string orderNo)
        {
            IEnumerable styleList = _buyOrdStyleManager.GetStyleByOrderNo(orderNo);
            return Json(styleList, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetCuttingSequenceOrderStyle(string orderStyleRefId, string orderNo)
        {
            IEnumerable sequenceList = _cuttingSequenceManager.GetCuttingSequenceOrderStyle(orderStyleRefId, orderNo);
            var model = new { sequenceList };
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DailyProductionStatus(ProductionReportViewModel model)
        {
            ModelState.Clear();
            model.FilterDate = model.FilterDate ?? DateTime.Now;
            return View(model);
        }

        public ActionResult DailyProductionStatusReport(ProductionReportViewModel model)
        {

            var reportType = new ReportType();

            switch (Convert.ToInt32(model.ReportType))
            {
                case 1:
                    reportType = ReportType.PDF;
                    break;
                case 3:
                    reportType = ReportType.Excel;
                    break;
            }
            DataTable dataTable = _productionReportManager.GetDailyProductionStatus(model.FilterDate.GetValueOrDefault());

            string path = Path.Combine(Server.MapPath("~/Areas/Production/Report"), "DailyProductionStatusReport.rdlc");
            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("DailyProductionDSet", dataTable) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 14, PageHeight = 8.5, MarginTop = .25, MarginLeft = 0.2, MarginRight = 0.2, MarginBottom = 0.25 };
            return ReportExtension.ToFile(reportType, path, reportDataSources, deviceInformation);
        }



        public ActionResult DailyProductionStatusSummaryReport(ProductionReportViewModel model)
        {

            var reportType = new ReportType();

            switch (Convert.ToInt32(model.ReportType))
            {
                case 1:
                    reportType = ReportType.PDF;
                    break;
                case 3:
                    reportType = ReportType.Excel;
                    break;
            }

            DataTable dataTable = _productionReportManager.GetDailyProductionStatusSummary(model.CuttingBatch.CuttingDate.GetValueOrDefault());

            string path = Path.Combine(Server.MapPath("~/Areas/Production/Report"), "DailyProductionStatusSummary.rdlc");
            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("DailyProductionStatusSummaryDset", dataTable) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 14, PageHeight = 8.5, MarginTop = .2, MarginLeft = 0.2, MarginRight = 0.2, MarginBottom = 0.2 };
            return ReportExtension.ToFile(reportType, path, reportDataSources, deviceInformation);
        }

        public ActionResult MonthlyProductionStatus(ProductionReportViewModel model)
        {
            if (!model.IsSearch)
            {
                model.IsSearch = true;
                model.MonthId = (MonthEnum)DateTime.Now.Month;
                return View(model);
            }
            else
            {
                DataTable dataTable = _productionReportManager.GetMonthlyProductionStatus(model.YearId, (int)model.MonthId);

                string path = Path.Combine(Server.MapPath("~/Areas/Production/Report"), "MonthlyCuttingReport.rdlc");
                if (!System.IO.File.Exists(path))
                {
                    return PartialView("~/Views/Shared/Error.cshtml");
                }
                var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("MonthlyCuttDSet", dataTable) };
                var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 11.69, PageHeight = 8.27, MarginTop = .2, MarginLeft = 0.2, MarginRight = 0.2, MarginBottom = 0.2 };
                return ReportExtension.ToFile(ReportType.PDF, path, reportDataSources, deviceInformation);
            }


        }
        public ActionResult MonthlySewingProductionStatus(ProductionReportViewModel model)
        {
            DataTable dataTable = _productionReportManager.GetMonthlySewingProductionStatus(model.YearId, (int)model.MonthId);
            var pareParameters = new List<ReportParameter>() { new ReportParameter("Title", Convert.ToString(model.MonthId) + " " + model.YearId) };
            string path = Path.Combine(Server.MapPath("~/Areas/Production/Report"), "MonthlyLineWiseProduction.rdlc");
            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("MonthlyLineWiseProductionDset", dataTable) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 11.69, PageHeight = 8.27, MarginTop = .2, MarginLeft = 0.2, MarginRight = 0.2, MarginBottom = 0.2 };
            return ReportExtension.ToFile(ReportType.PDF, path, reportDataSources, deviceInformation, pareParameters);
        }
        public ActionResult MonthlyDayWiseSewingProductionStatus(ProductionReportViewModel model)
        {
            DataTable dataTable = _productionReportManager.GetMonthlyDayWiseSewingProductionStatus(model.YearId, (int)model.MonthId);
            var pareParameters = new List<ReportParameter>() { new ReportParameter("Title", Convert.ToString(model.MonthId) + " " + model.YearId) };
            string path = Path.Combine(Server.MapPath("~/Areas/Production/Report"), "MonthlyDayWiseSewingProductionReport.rdlc");
            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("MonthlySewingProductionDSet", dataTable) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 11.69, PageHeight = 8.27, MarginTop = .2, MarginLeft = 0.2, MarginRight = 0.2, MarginBottom = 0.2 };
            return ReportExtension.ToFile(ReportType.PDF, path, reportDataSources, deviceInformation, pareParameters);
        }

        public ActionResult MontyPlanningVsProduction(ProductionReportViewModel model)
        {
            DataTable dataTable = _productionReportManager.MontyPlanningVsProduction(model.YearId, (int)model.MonthId);
            var pareParameters = new List<ReportParameter>() { new ReportParameter("Title", Convert.ToString(model.MonthId) + " " + model.YearId) };
            string path = Path.Combine(Server.MapPath("~/Areas/Production/Report"), "TartgetAchivementReport.rdlc");
            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("TargetAchivementDSet", dataTable) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 11.69, PageHeight = 8.27, MarginTop = .1, MarginLeft = 0.1, MarginRight = 0.1, MarginBottom = 0.1 };
            return ReportExtension.ToFile(ReportType.PDF, path, reportDataSources, deviceInformation, pareParameters);
        }
        public ActionResult DailyPrintEmbStatus(ProductionReportViewModel model)
        {
            if (!model.IsSearch)
            {
                model.IsSearch = true;
                model.FilterDate = DateTime.Now;
                return View(model);
            }
            else
            {
                DataTable dataTable = _productionReportManager.GetDailyPrintEmbStatus(model.FilterDate);
                string path = Path.Combine(Server.MapPath("~/Areas/Production/Report"), "DailyPrintEmbSendRcvStatusReport.rdlc");
                if (!System.IO.File.Exists(path))
                {
                    return PartialView("~/Views/Shared/Error.cshtml");
                }
                var reportDataSources = new List<ReportDataSource>()
                {
                    new ReportDataSource("DailyPrintEmbSRStatus", dataTable)
                };
                var deviceInformation = new DeviceInformation()
                {
                    OutputFormat = 2,
                    PageWidth = 11.69,
                    PageHeight = 8.27,
                    MarginTop = .2,
                    MarginLeft = 0.3,
                    MarginRight = 0.2,
                    MarginBottom = 0.2
                };
                return ReportExtension.ToFile(ReportType.PDF, path, reportDataSources, deviceInformation);
            }
        }

        public ActionResult MonthlyCuttngStatus(CuttingBatchViewModel model)
        {
            var reportType = ReportType.PDF;

            switch (Convert.ToInt32(model.ReportType))
            {
                case 1:
                    reportType = ReportType.PDF;
                    break;
                case 3:
                    reportType = ReportType.Excel;
                    break;
            }
            var fromDate = new ReportParameter("FromDate", model.FromDate.GetValueOrDefault().ToString("dd/MM/yyyy"));
            var toDate = new ReportParameter("ToDate", model.ToDate.GetValueOrDefault().ToString("dd/MM/yyyy"));
            var reportParameters = new List<ReportParameter>() { fromDate, toDate };
            DataTable dataTable = _productionReportManager.GetMonthlyCuttingStatus(model.FromDate.GetValueOrDefault(), model.ToDate.GetValueOrDefault(), PortalContext.CurrentUser.CompId);
            string path = Path.Combine(Server.MapPath("~/Areas/Production/Report"), "MonthlyCuttingStatus.rdlc");
            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("DataSet1", dataTable) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 11.69, PageHeight = 8.27, MarginTop = .2, MarginLeft = 0.2, MarginRight = 0.2, MarginBottom = 0.2 };
            return ReportExtension.ToFile(reportType, path, reportDataSources, deviceInformation, reportParameters);
        }

        public ActionResult PrintEmbProcessStatus(string orderNo,string orderStyleRefId, string reportType)
        {
            var reporttp = ReportType.PDF;

            switch (Convert.ToInt32(reportType))
            {
                case 1:
                    reporttp = ReportType.PDF;
                    break;
                case 3:
                    reporttp = ReportType.Excel;
                    break;
            }

            DataTable dataTable = _productionReportManager.GetPrintEmprocessStatus(orderNo,orderStyleRefId, PortalContext.CurrentUser.CompId);
            string path = Path.Combine(Server.MapPath("~/Areas/Production/Report"), "PrintEmbProcessStatus.rdlc");
            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("PrintEmbProcessDSet", dataTable) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 14.5, PageHeight = 8.27, MarginTop = .1, MarginLeft = 0.1, MarginRight = 0.1, MarginBottom = 0.1 };
            return ReportExtension.ToFile(reporttp, path, reportDataSources, deviceInformation);
        }

        public ActionResult KnittingProductionDetail(string orderStyleRefId, string reportType)
        {
            var reporttp = ReportType.PDF;

            switch (Convert.ToInt32(reportType))
            {
                case 1:
                    reporttp = ReportType.PDF;
                    break;
                case 3:
                    reporttp = ReportType.Excel;
                    break;
            }

            DataTable dataTable = _productionReportManager.GetKnittingProductionDetailStatus(orderStyleRefId, PortalContext.CurrentUser.CompId);
            string path = Path.Combine(Server.MapPath("~/Areas/Production/Report"), "KnittingProductionReport.rdlc");
            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("KntProdDSet", dataTable) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 18, PageHeight = 8.27, MarginTop = .2, MarginLeft = 0.2, MarginRight = 0.2, MarginBottom = 0.2 };
            return ReportExtension.ToFile(reporttp, path, reportDataSources, deviceInformation);
        }


        #region Dyeing Production

        public ActionResult DayeingBatchStatus(BatchViewModel model)
        {
            var reportType = ReportType.PDF;

            switch (Convert.ToInt32(model.ReportType))
            {
                case 1:
                    reportType = ReportType.PDF;
                    break;
                case 3:
                    reportType = ReportType.Excel;
                    break;
            }
            List<VProBatch> batch = _batchManager.GetBachStatus(model.FromDate, model.ToDate, model.PartyId,model.MachineId);
            var fromDate = new ReportParameter("FromDate", model.FromDate.GetValueOrDefault().ToString("dd/MM/yyyy"));
            var toDate = new ReportParameter("ToDate", model.ToDate.GetValueOrDefault().ToString("dd/MM/yyyy"));
            var reportParameters = new List<ReportParameter>() { fromDate, toDate };
            string path = Path.Combine(Server.MapPath("~/Areas/Production/Report"), "DyeingBatchStatus.rdlc");
            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("DyeingBatchDSet", batch) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 11.69, PageHeight = 8.27, MarginTop = .2, MarginLeft = 0.1, MarginRight = 0.1, MarginBottom = 0.2 };
            return ReportExtension.ToFile(reportType, path, reportDataSources, deviceInformation, reportParameters);
        }

        public ActionResult GetBatchDetail(long batchId)
        {
            ModelState.Clear();
            DataTable batchDetailDataTable = _productionReportManager.GetBatchDetail(batchId, PortalContext.CurrentUser.CompId);
            string path = Path.Combine(Server.MapPath("~/Areas/Production/Report"), "BatchDetailReport.rdlc");
            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("BatchCardDset", batchDetailDataTable) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 8.27, PageHeight = 11.69, MarginTop = 1, MarginLeft = 0.1, MarginRight = 0.1, MarginBottom = 0.1};
            return ReportExtension.ToFile(ReportType.PDF, path, reportDataSources, deviceInformation);
        }

        public ActionResult GetDyeingSpChallan(long dyeingSpChallanId,string reportName)
        {
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 8.27, PageHeight = 11.69, MarginTop = .5, MarginLeft = 0.1, MarginRight = 0.1, MarginBottom = .2 };
            switch (reportName)
            {
                case "EX":
                    reportName = "DyeingSubprocessChallan.rdlc";
                    break;
                case "IN":
                     reportName = "DyeingSubprocessChallanDetail.rdlc";
                     deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 11.69, PageHeight = 8.27, MarginTop = .5, MarginLeft = 0.1, MarginRight = 0.1, MarginBottom = .2 };
                    break;
            }
            DataTable processReceiveDetaliDt = _productionReportManager.GetDyeingSpChallan(dyeingSpChallanId);
            string path = Path.Combine(Server.MapPath("~/Areas/Production/Report"), reportName);
            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("DyeingSpDSet", processReceiveDetaliDt) };
           
            return ReportExtension.ToFile(ReportType.PDF, path, reportDataSources, deviceInformation);
        }

        public ActionResult KnittingRollDeliveryChallan(int knittingRollIssueId,int challanType=1)
        {
            string reportName = "KnittingRollDeliveryChallan.rdlc";
            if (challanType==2)
            {
                 reportName = "KnittingCollarCuffDeliveryChallan.rdlc";
            }
            DataTable dataTable = _productionReportManager.GetKnittingRollDeliveryChallan(knittingRollIssueId);
            DataTable inProgramDataTable = _productionReportManager.GetInKnitProgramDataTable(knittingRollIssueId);
            string path = Path.Combine(Server.MapPath("~/Areas/Production/Report"), reportName);
            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("KnittingRollDeliveryChallanDSet", dataTable), new ReportDataSource("InPorgDset", inProgramDataTable) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 8.27, PageHeight = 11.69, MarginTop = .1, MarginLeft = 0.1, MarginRight = 0.1, MarginBottom = 0.1 };
            return ReportExtension.ToFile(ReportType.PDF, path, reportDataSources, deviceInformation);
        }
        public ActionResult KnittingPartyChallan(int knittingRollIssueId)
        {
            DataTable dataTable = _productionReportManager.GetKnittingRollDeliveryPartyChallan(knittingRollIssueId);
            DataTable inProgramDataTable = _productionReportManager.GetInKnitProgramDataTable(knittingRollIssueId);
            string path = Path.Combine(Server.MapPath("~/Areas/Production/Report"), "KnittingPartyChallan.rdlc");
            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("KBDSET", dataTable), new ReportDataSource("InProgramDSet", inProgramDataTable) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 8.27, PageHeight = 11.69, MarginTop = .1, MarginLeft = 0.1, MarginRight = 0.1, MarginBottom = 0.1 };
            return ReportExtension.ToFile(ReportType.PDF, path, reportDataSources, deviceInformation);
        }
        public ActionResult KnittingPartyBill(int knittingRollIssueId)
        {
            DataTable dataTable = _productionReportManager.GetKnittingRollDeliveryPartyChallan(knittingRollIssueId);
            DataTable inProgramDataTable = _productionReportManager.GetInKnitProgramDataTable(knittingRollIssueId);

            string path = Path.Combine(Server.MapPath("~/Areas/Production/Report"), "KnittingBill.rdlc");
            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
           // var reportParameters = new List<ReportParameter>() { new ReportParameter("InWord", inWord), new ReportParameter("ReportTitle", reportTitle) };
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("KBDSET", dataTable), new ReportDataSource("InProgramDSet", inProgramDataTable) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 8.27, PageHeight = 11.69, MarginTop = .1, MarginLeft = 0.1, MarginRight = 0.1, MarginBottom = 0.1 };
            return ReportExtension.ToFile(ReportType.PDF, path, reportDataSources, deviceInformation);
        }
        public ActionResult KnittingRollDeliveryChallanSummary(int knittingRollIssueId)
        {
            DataTable dataTable = _productionReportManager.GetKnittingRollDeliveryChallanSummary(knittingRollIssueId);
            DataTable inProgramDataTable = _productionReportManager.GetInKnitProgramDataTable(knittingRollIssueId);
            string path = Path.Combine(Server.MapPath("~/Areas/Production/Report"), "RollDeliveryChallanSummary.rdlc");
            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("KnittingRollDeliveryChallanDSet", dataTable), new ReportDataSource("InPorgDSet", inProgramDataTable) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 8.27, PageHeight = 11.69, MarginTop = .5, MarginLeft = 0.005, MarginRight = 0.005, MarginBottom = 0.2 };
            return ReportExtension.ToFile(ReportType.PDF, path, reportDataSources, deviceInformation);
        }

        public ActionResult MontyLossTimeSummaryReport(ProductionReportViewModel model)
        {
            DataTable dataTable = _productionReportManager.GetMontyLossTimeSummaryReport(model.YearId, (int)model.MonthId);
            var pareParameters = new List<ReportParameter>() { new ReportParameter("Title", Convert.ToString(model.MonthId) + " " + model.YearId) };
            string path = Path.Combine(Server.MapPath("~/Areas/Production/Report"), "MonthlyLossTimeSummary.rdlc");
            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("MonthlyLossTimeSummaryDSet", dataTable) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 11.69, PageHeight = 8.27, MarginTop = .1, MarginLeft = 0.1, MarginRight = 0.1, MarginBottom = 0.1 };
            return ReportExtension.ToFile(ReportType.PDF, path, reportDataSources, deviceInformation, pareParameters);
        }

        public ActionResult MonthlyStyleWiseProductionReport(ProductionReportViewModel model)
        {
            DataTable dataTable = _productionReportManager.GetMontyStyleWiseProduction(model.YearId, (int)model.MonthId);
            var pareParameters = new List<ReportParameter>() { new ReportParameter("Title", Convert.ToString(model.MonthId) + " " + model.YearId) };
            string path = Path.Combine(Server.MapPath("~/Areas/Production/Report"), "MonthlyStyleWiseProduction.rdlc");
            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("CMSMVDS", dataTable) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 11.69, PageHeight = 8.27, MarginTop = .2, MarginLeft = 0.2, MarginRight = 0.2, MarginBottom = 0.2 };
            return ReportExtension.ToFile(ReportType.Excel, path, reportDataSources, deviceInformation, pareParameters);
        }
        public ActionResult TargetVProductionSpLine(ProductionReportViewModel model)
        {
            DataTable dataTable = _productionReportManager.GetTargetVProduction(model.YearId, (int)model.MonthId);
            var pareParameters = new List<ReportParameter>() { new ReportParameter("Title", Convert.ToString(model.MonthId) + " " + model.YearId) };
            string path = Path.Combine(Server.MapPath("~/Areas/Production/Report"), "SPLINE.rdlc");
            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("SPLINEDSET", dataTable) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 11.69, PageHeight = 8.27, MarginTop = .2, MarginLeft = 0.2, MarginRight = 0.2, MarginBottom = 0.2 };
            return ReportExtension.ToFile(ReportType.PDF, path, reportDataSources, deviceInformation, pareParameters);
        }
        [HttpGet]
        public ActionResult MailDailyProductionStatus()
        {

            DataTable productionStatus = _productionReportManager.DailyProductionStatus(DateTime.Now.AddDays(-1));
            var reportDate = new ReportParameter("ReportDate", DateTime.Now.AddDays(-1).ToString("dd/MM/yyyy"));
            var reportParameters = new List<ReportParameter>() { reportDate };
            string path = Path.Combine(Server.MapPath("~/Areas/Production/Report"), "DailyProductionStatus.rdlc");
            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("ProductionStatusDS", productionStatus) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 11.69, PageHeight = 8.27, MarginTop = 1, MarginLeft =1.2, MarginRight = 1.2, MarginBottom =1 };
           var reportExport=  ReportExtension.ToWhiteFile(ReportType.PDF, path, reportDataSources, deviceInformation, reportParameters);

           DbEmailModel dbEmail = _emailSendManager.GetDbEmailByTemplateId(EmailTemplateRefId.FABRICSTORE, PortalContext.CurrentUser.CompId);
           dbEmail.Subject = "Production Status Report";
           dbEmail.Body = "Daily Knitting ,Dyeing,Cutting ,Sewing and Finishing Production Status as on date :" + DateTime.Now.AddDays(-1).ToString("dd/MM/yyyy");
           dbEmail.FileAttachments = HostingEnvironment.MapPath(AppConfig.ExportReportFillPath + "." + ReportType.PDF);
           bool send = _emailSendManager.SendDbEmail(dbEmail);
            return reportExport;
        }


        public ActionResult MonthlyOtherCuttingProductionReport(ProductionReportViewModel model)
        {
            DataTable dataTable = _productionReportManager.GetMonthlyOtherCuttingProduction(model.YearId, (int)model.MonthId);
            var pareParameters = new List<ReportParameter>() { new ReportParameter("Title", Convert.ToString(model.MonthId) + " " + model.YearId) };
            string path = Path.Combine(Server.MapPath("~/Areas/Production/Report"), "MonthlyOrtherCuttingReport.rdlc");
            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("MCRDSET", dataTable) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 11, PageHeight = 8.5, MarginTop = .1, MarginLeft = 0.1, MarginRight = 0.1, MarginBottom = 0.1 };
            return ReportExtension.ToFile(ReportType.Excel, path, reportDataSources, deviceInformation, pareParameters);
        }


        public ActionResult CuttingBodyReplaceReport(string orderStyleRefId)
        {
            string reportName = "CuttingBodyReplaceReport";
            var reportParams = new List<ReportParameter> { new ReportParameter("OrderStyleRefId",orderStyleRefId),
                 new ReportParameter("CompId", PortalContext.CurrentUser.CompId) };
            return ReportExtension.ToSsrsFile(ReportType.PDF, reportName, reportParams);
        }
        #endregion

    }
}