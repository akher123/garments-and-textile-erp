using System.Collections;
using System.Globalization;
using System.IO;
using Microsoft.Reporting.WebForms;
using SCERP.BLL.IManager.ICommercialManager;
using SCERP.Common;
using SCERP.Model.CommercialModel;
using SCERP.Model.Custom;
using SCERP.Web.Areas.Commercial.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCERP.Web.Controllers;
using SCERP.Web.Helpers;
using SCERP.Web.Models;
using SCERP.BLL.IManager.ICommonManager;
using System.Data;
using System.Reflection;
using SCERP.BLL.IManager.IInventoryManager;

namespace SCERP.Web.Areas.Commercial.Controllers
{
    public class ReportController : BaseController
    {
        private readonly IReportManager _reportManager;
        private readonly ISalesContactManager _salesContactManager;
        private readonly ILcManager _lcManager;
        private readonly IExportManager _exportManager;
        private readonly IBbLcManager _bbLcManager;
        private readonly IInventoryGroupManager _inventoryManager;

        public ReportController(IReportManager reportManager, ISalesContactManager salesContactManager, ILcManager lcManager, IExportManager exportManager, IInventoryGroupManager inventoryManager)
        {
            this._reportManager = reportManager;
            _salesContactManager = salesContactManager;
            _lcManager = lcManager;
            _exportManager = exportManager;
            _inventoryManager = inventoryManager;
        }

        public ActionResult LcIndividual(int lcId)
        {
            List<COMMLcInfo> reportdata = _reportManager.GetLcIndividual(lcId);

            LocalReport lr = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/Areas/Commercial/Reports"), "LcIndividual.rdlc");
            if (System.IO.File.Exists(path))
                lr.ReportPath = path;
            else
                return View("Error");

            string companyId = PortalContext.CurrentUser.CompId;
            string companyName = _reportManager.GetCompanyNameByCompanyId(companyId);
            string companyAddress = _reportManager.GetCompanyAddressByCompanyId(companyId);

            ReportParameter[] parameters = new ReportParameter[2];
            parameters[0] = new ReportParameter("param_CompanySector", companyName);
            parameters[1] = new ReportParameter("param_CompanyAddress", companyAddress);

            ReportDataSource rd = new ReportDataSource("DataSet1", reportdata);
            lr.SetParameters(parameters);
            lr.DataSources.Add(rd);
            string reportType = "pdf";
            string mimeType;
            string encoding;
            string fileNameExtension;

            string deviceInfo =
                "<DeviceInfo>" +
                "  <OutputFormat>" + 2 + "</OutputFormat>" +
                "  <PageWidth>8.3in</PageWidth>" +
                "  <PageHeight>11.7in</PageHeight>" +
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
        public  ActionResult LcIndividualReport(int lcId, ReportType reportTyle)
        {
            ModelState.Clear();
            DataTable dataTable = _reportManager.GetLcInfoDetail(lcId);
            string path = Path.Combine(Server.MapPath("~/Areas/Commercial/Reports"), "IndividualLcReport.rdlc");
            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("LcDataSet", dataTable) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 11, PageHeight = 8.5, MarginTop = 0, MarginLeft = 0, MarginRight = 0.25, MarginBottom = .25 };
            return ReportExtension.ToFile(reportTyle, path, reportDataSources, deviceInformation);
        }
        public ActionResult LcIndividualDetailReport(int lcId, ReportType reportTyle) 
        {
         
            DataTable summary = _reportManager.GetLcInfoDetail(lcId);
            DataTable details = _reportManager.GetSalesContactExpByLcId(lcId);
            string path = Path.Combine(Server.MapPath("~/Areas/Commercial/Reports"), "LcReportDetail.rdlc");
            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("LcSummaryDS", summary), new ReportDataSource("LcDetailDS", details) };

            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 11.69, PageHeight = 8.27 , MarginTop = .2, MarginLeft = .1, MarginRight = .1, MarginBottom = .2 };
            var reportExport = ReportExtension.ToFile(reportTyle, path, reportDataSources, deviceInformation);
            return reportExport;
        }

        public ActionResult BbLcIndividual(int bbLcId)
        {
            List<BbLcIndividualReport> reportdata = _reportManager.GetBbLcIndividual(bbLcId);

            LocalReport lr = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/Areas/Commercial/Reports"), "BbLcIndividual.rdlc");
            if (System.IO.File.Exists(path))
                lr.ReportPath = path;
            else
                return View("Error");

            string companyId = PortalContext.CurrentUser.CompId;
            string companyName = _reportManager.GetCompanyNameByCompanyId(companyId);
            string companyAddress = _reportManager.GetCompanyAddressByCompanyId(companyId);

            ReportParameter[] parameters = new ReportParameter[2];
            parameters[0] = new ReportParameter("param_CompanySector", companyName);
            parameters[1] = new ReportParameter("param_CompanyAddress", companyAddress);

            ReportDataSource rd = new ReportDataSource("DataSet1", reportdata);
            lr.SetParameters(parameters);
            lr.DataSources.Add(rd);
            string reportType = "pdf";
            string mimeType;
            string encoding;
            string fileNameExtension;

            string deviceInfo =
                "<DeviceInfo>" +
                "  <OutputFormat>" + 2 + "</OutputFormat>" +
                "  <PageWidth>8.3in</PageWidth>" +
                "  <PageHeight>11.7in</PageHeight>" +
                "  <MarginTop>0..2in</MarginTop>" +
                "  <MarginLeft>.3in</MarginLeft>" +
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

        public ActionResult LcListReport(long? buyerId, int? lcType, string fromDate, string toDate, string lcNo)
        {
            DateTime? fromD = new DateTime(2001, 1, 1);
            DateTime? toD = new DateTime(2020, 1, 1);

            if (!string.IsNullOrEmpty(fromDate))
                fromD = DateTime.Parse(fromDate, CultureInfo.GetCultureInfo("en-gb"));
            if (!string.IsNullOrEmpty(toDate))
                toD = DateTime.Parse(toDate, CultureInfo.GetCultureInfo("en-gb"));

            COMMLcInfo lcInfo = new COMMLcInfo();
            lcInfo.BuyerId = buyerId;
            lcInfo.LcType = lcType;
            lcInfo.FromDate = fromD;
            lcInfo.ToDate = toD;
            lcInfo.LcNo = lcNo.Trim();

            List<COMMLcInfo> reportdata = _reportManager.GetLcInfos(lcInfo);

            LocalReport lr = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/Areas/Commercial/Reports"), "LcList.rdlc");
            if (System.IO.File.Exists(path))
                lr.ReportPath = path;
            else
                return View("Error");

            string companyId = PortalContext.CurrentUser.CompId;
            string companyName = _reportManager.GetCompanyNameByCompanyId(companyId);
            string companyAddress = _reportManager.GetCompanyAddressByCompanyId(companyId);

            ReportParameter[] parameters = new ReportParameter[4];
            parameters[0] = new ReportParameter("param_CompanySector", companyName);
            parameters[1] = new ReportParameter("param_FromDate", fromDate == "" ? " " : fromDate);
            parameters[2] = new ReportParameter("param_ToDate", toDate == "" ? " " : toDate);
            parameters[3] = new ReportParameter("param_CompanyAddress", companyAddress);


            ReportDataSource rd = new ReportDataSource("DataSet1", reportdata);
            lr.SetParameters(parameters);
            lr.DataSources.Add(rd);
            string reportType = "pdf";
            string mimeType;
            string encoding;
            string fileNameExtension;

            string deviceInfo =
                "<DeviceInfo>" +
                "  <OutputFormat>" + 2 + "</OutputFormat>" +
                "  <PageWidth>8.3in</PageWidth>" +
                "  <PageHeight>11.7in</PageHeight>" +
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

        public ActionResult BbLcListReport(int? supplierId, int? bbLcType, int? lcRefId, string fromDate, string toDate, string maturityDateFrom, string maturityDateTo, string expiryDateFrom, string expiryDateTo, string bbLcNo, int? issuingBankId, int printFormatId)
        {

            DateTime? fromD = new DateTime(1900, 1, 1);
            DateTime? toD = new DateTime(2050, 1, 1);

            DateTime? maturityFrom = new DateTime(2001, 1, 1);
            DateTime? maturityTo = new DateTime(2001, 1, 1);
            DateTime? expiryFrom = new DateTime(2001, 1, 1);
            DateTime? expiryTo = new DateTime(2001, 1, 1);

            if (!string.IsNullOrEmpty(fromDate))
                fromD = DateTime.Parse(fromDate, CultureInfo.GetCultureInfo("en-gb"));
            if (!string.IsNullOrEmpty(toDate))
                toD = DateTime.Parse(toDate, CultureInfo.GetCultureInfo("en-gb"));

            if (!string.IsNullOrEmpty(maturityDateFrom))
                maturityFrom = DateTime.Parse(maturityDateFrom, CultureInfo.GetCultureInfo("en-gb"));

            if (!string.IsNullOrEmpty(maturityDateTo))
                maturityTo = DateTime.Parse(maturityDateTo, CultureInfo.GetCultureInfo("en-gb"));

            if (!string.IsNullOrEmpty(expiryDateFrom))
                expiryFrom = DateTime.Parse(expiryDateFrom, CultureInfo.GetCultureInfo("en-gb"));

            if (!string.IsNullOrEmpty(expiryDateTo))
                expiryTo = DateTime.Parse(expiryDateTo, CultureInfo.GetCultureInfo("en-gb"));

            CommBbLcInfo bbLcInfo = new CommBbLcInfo();
            bbLcInfo.SupplierCompanyRefId = supplierId;
            bbLcInfo.BbLcType = bbLcType;
            bbLcInfo.LcRefId = lcRefId ?? 0;
            bbLcInfo.FromDate = fromD;
            bbLcInfo.ToDate = toD;
            bbLcInfo.MaturityDateFrom = maturityFrom;
            bbLcInfo.MaturityDateTo = maturityTo;
            bbLcInfo.ExpiryDateFrom = expiryFrom;
            bbLcInfo.ExpiryDateTo = expiryTo;
            bbLcInfo.BbLcNo = bbLcNo;
            bbLcInfo.IssuingBankId = issuingBankId;

            List<CommBbLcReport> reportdata = _reportManager.GetBbLcInfos(bbLcInfo).OrderByDescending(x => x.BbLcId).ToList();

            LocalReport lr = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/Areas/Commercial/Reports"), "BbLcInfo.rdlc");
            if (System.IO.File.Exists(path))
                lr.ReportPath = path;
            else
                return View("Error");

            string companyId = PortalContext.CurrentUser.CompId;
            string companyName = _reportManager.GetCompanyNameByCompanyId(companyId);
            string companyAddress = _reportManager.GetCompanyAddressByCompanyId(companyId);

            ReportParameter[] parameters = new ReportParameter[4];
            parameters[0] = new ReportParameter("param_CompanySector", companyName);
            parameters[1] = new ReportParameter("param_FromDate", fromDate == "" ? " " : fromDate);
            parameters[2] = new ReportParameter("param_ToDate", toDate == "" ? " " : toDate);
            parameters[3] = new ReportParameter("param_CompanyAddress", companyAddress);

            string reportType = "";

            switch (printFormatId)
            {
                case 1:
                    reportType = "PDF";
                    break;
                case 2:
                    reportType = "Excel";
                    break;
            }

            ReportDataSource rd = new ReportDataSource("DataSet1", reportdata);
            lr.SetParameters(parameters);
            lr.DataSources.Add(rd);
            string mimeType;
            string encoding;
            string fileNameExtension;

            string deviceInfo =
                "<DeviceInfo>" +
                "  <OutputFormat>" + 2 + "</OutputFormat>" +
                "  <PageWidth>8.3in</PageWidth>" +
                "  <PageHeight>11.7in</PageHeight>" +
                "  <MarginTop>0..2in</MarginTop>" +
                "  <MarginLeft>.2in</MarginLeft>" +
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

        public ActionResult ImportExportPerformance(long? buyerId, int reportTypeId)
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
            List<CommImportExportPerformanceReport> reportdata = _reportManager.GetCommImportExportPerformance(buyerId);
            string path = Path.Combine(Server.MapPath("~/Areas/Commercial/Reports"), "ImportExportPerformance.rdlc");
            string companyName = _reportManager.GetCompanyNameByCompanyId(PortalContext.CurrentUser.CompId);
            string companyAddress = _reportManager.GetCompanyAddressByCompanyId(PortalContext.CurrentUser.CompId);

            ReportParameter[] parameters =
            {
                new ReportParameter("param_CompanySector", companyName),
                new ReportParameter("param_CompanyAddress", companyAddress)
            };

            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("DataSet1", reportdata) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 11.69, PageHeight = 8.27, MarginTop = .25, MarginLeft = .2, MarginRight = .2, MarginBottom = .25 };
            return ReportExtension.ToFile(reportType, path, reportDataSources, deviceInformation, parameters.ToList());
        }

        public ActionResult ExportListReport(string fromDate, string toDate, string searchKey)
        {
            DateTime? fromD = new DateTime(2000, 1, 1);
            DateTime? toD = new DateTime(2000, 1, 1);

            if (!string.IsNullOrEmpty(fromDate))
                fromD = DateTime.Parse(fromDate, CultureInfo.GetCultureInfo("en-gb"));
            if (!string.IsNullOrEmpty(toDate))
                toD = DateTime.Parse(toDate, CultureInfo.GetCultureInfo("en-gb"));
            if (!string.IsNullOrEmpty(searchKey))
                searchKey = "";

            CommExportListReport export = new CommExportListReport();
            export.FromDate = fromD;
            export.ToDate = toD;
            export.SearchString = searchKey;
            List<CommExportListReport> reportdata = _reportManager.GetExportListReport(export);

            LocalReport lr = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/Areas/Commercial/Reports"), "ExportList.rdlc");
            if (System.IO.File.Exists(path))
                lr.ReportPath = path;
            else
                return View("Error");

            string companyId = PortalContext.CurrentUser.CompId;
            string companyName = _reportManager.GetCompanyNameByCompanyId(companyId);
            string companyAddress = _reportManager.GetCompanyAddressByCompanyId(companyId);

            ReportParameter[] parameters = new ReportParameter[2];
            parameters[0] = new ReportParameter("param_CompanySector", companyName);
            parameters[1] = new ReportParameter("param_CompanyAddress", companyAddress);


            ReportDataSource rd = new ReportDataSource("DataSet1", reportdata);
            lr.SetParameters(parameters);
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

        public ActionResult LcOrderReport(LcViewModel model)
        {
            ModelState.Clear();
            IEnumerable searchType = from LcOrderSearch search in Enum.GetValues(typeof(LcOrderSearch)) select new { Id = (int)search, Name = search.ToString() };
            model.FromDate = DateTime.Now;
            model.ToDate = DateTime.Now;
            model.SearchType = searchType;
            return View(model);
        }

        public ActionResult LcOrderReportPrint(string fromDate, string toDate, string searchKey)
        {
            DateTime? fromD = new DateTime(2000, 1, 1);
            DateTime? toD = new DateTime(2000, 1, 1);

            if (!string.IsNullOrEmpty(fromDate))
                fromD = DateTime.Parse(fromDate, CultureInfo.GetCultureInfo("en-gb"));
            if (!string.IsNullOrEmpty(toDate))
                toD = DateTime.Parse(toDate, CultureInfo.GetCultureInfo("en-gb"));
            if (string.IsNullOrEmpty(searchKey))
                searchKey = "";

            CommLcToOrderReport export = new CommLcToOrderReport();
            export.FromDate = fromD;
            export.ToDate = toD;
            export.SearchString = searchKey;
            List<CommLcToOrderReport> reportdata = _reportManager.GetLcOrderReport(export);

            LocalReport lr = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/Areas/Commercial/Reports"), "LcOrder.rdlc");
            if (System.IO.File.Exists(path))
                lr.ReportPath = path;
            else
                return View("Error");

            string companyId = PortalContext.CurrentUser.CompId;
            string companyName = _reportManager.GetCompanyNameByCompanyId(companyId);

            ReportParameter[] parameters = new ReportParameter[1];
            parameters[0] = new ReportParameter("param_CompanySector", companyName);

            ReportDataSource rd = new ReportDataSource("DataSet1", reportdata);
            lr.SetParameters(parameters);
            lr.DataSources.Add(rd);
            string reportType = "pdf";
            string mimeType;
            string encoding;
            string fileNameExtension;

            string deviceInfo =
                "<DeviceInfo>" +
                "  <OutputFormat>" + 2 + "</OutputFormat>" +
                "  <PageWidth>8.27in</PageWidth>" +
                "  <PageHeight>11.69in</PageHeight>" +
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

        public ActionResult BankAdvicePrint(string exportId)
        {
            CommBankAdviceReport bankAdvice = new CommBankAdviceReport();
            if (!string.IsNullOrEmpty(exportId))
                bankAdvice.ExportId = Convert.ToInt64(exportId);

            List<CommBankAdviceReport> reportdata = _reportManager.GetBankAdviceReport(bankAdvice);

            LocalReport lr = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/Areas/Commercial/Reports"), "BankAdvice.rdlc");
            if (System.IO.File.Exists(path))
                lr.ReportPath = path;
            else
                return View("Error");

            string companyId = PortalContext.CurrentUser.CompId;
            string companyName = _reportManager.GetCompanyNameByCompanyId(companyId);

            ReportParameter[] parameters = new ReportParameter[1];
            parameters[0] = new ReportParameter("param_CompanySector", companyName);

            ReportDataSource rd = new ReportDataSource("DataSet1", reportdata);
            lr.SetParameters(parameters);
            lr.DataSources.Add(rd);
            string reportType = "pdf";
            string mimeType;
            string encoding;
            string fileNameExtension;

            string deviceInfo =
                "<DeviceInfo>" +
                "  <OutputFormat>" + 2 + "</OutputFormat>" +
                "  <PageWidth>8.27in</PageWidth>" +
                "  <PageHeight>11.69in</PageHeight>" +
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

        public ActionResult CommLcWithOrderSummaryReport(int reportTypeId)
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
            List<CommLcWithOrderSummaryReport> reportdata = _reportManager.GetCommLcWithOrderSummaryReport();
            string path = Path.Combine(Server.MapPath("~/Areas/Commercial/Reports"), "LcWithOrderSummaryReport.rdlc");
            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            string companyName = _reportManager.GetCompanyNameByCompanyId(PortalContext.CurrentUser.CompId);
            string companyAddress = _reportManager.GetCompanyAddressByCompanyId(PortalContext.CurrentUser.CompId);

            ReportParameter[] parameters =
            {
                new ReportParameter("param_CompanySector", companyName),
                new ReportParameter("param_CompanyAddress", companyAddress)
            };
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("DataSet1", reportdata) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 8.27, PageHeight = 11.69, MarginTop = 0.25, MarginLeft = 0.2, MarginRight = 0.1, MarginBottom = 0.25 };
            return ReportExtension.ToFile(reportType, path, reportDataSources, deviceInformation, parameters.ToList());
        }

        public ActionResult CommLcWithOrderDetailReport(int reportTypeId)
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
            List<CommLcWithOrderDetailReport> reportdata = _reportManager.GetCommLcWithOrderDetailReport();
            string path = Path.Combine(Server.MapPath("~/Areas/Commercial/Reports"), "LcWithOrderDetailReport.rdlc");
            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            string companyName = _reportManager.GetCompanyNameByCompanyId(PortalContext.CurrentUser.CompId);
            string companyAddress = _reportManager.GetCompanyAddressByCompanyId(PortalContext.CurrentUser.CompId);
            ReportParameter[] parameters =
            {
                new ReportParameter("param_CompanySector", companyName),
                new ReportParameter("param_CompanyAddress", companyAddress)
            };
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("DataSet1", reportdata) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 8.27, PageHeight = 11.69, MarginTop = 0.25, MarginLeft = 0.2, MarginRight = 0.1, MarginBottom = 0.25 };
            return ReportExtension.ToFile(reportType, path, reportDataSources, deviceInformation, parameters.ToList());
        }

        public ActionResult CommLcWithoutOrderReport(int reportTypeId)
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
            List<CommGetLcWithoutOrderReport> reportdata = _reportManager.CommGetLcWithoutOrderReport();
            string path = Path.Combine(Server.MapPath("~/Areas/Commercial/Reports"), "LcWithoutOrder.rdlc");
            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            string companyName = _reportManager.GetCompanyNameByCompanyId(PortalContext.CurrentUser.CompId);
            string companyAddress = _reportManager.GetCompanyAddressByCompanyId(PortalContext.CurrentUser.CompId);

            ReportParameter[] parameters =
            {
                new ReportParameter("param_CompanySector", companyName),
                 new ReportParameter("param_CompanyAddress", companyAddress)
            };
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("DataSet1", reportdata) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 8.27, PageHeight = 11.69, MarginTop = 0.25, MarginLeft = 0.25, MarginRight = 0.2, MarginBottom = 0.25 };
            return ReportExtension.ToFile(reportType, path, reportDataSources, deviceInformation, parameters.ToList());
        }

        public ActionResult CommOrderWithoutLcReport(int reportTypeId)
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
            List<CommGetOrderWithoutLcReport> reportdata = _reportManager.CommGetOrderWithoutLcReport();
            string path = Path.Combine(Server.MapPath("~/Areas/Commercial/Reports"), "OrderWithoutLc.rdlc");
            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            string companyName = _reportManager.GetCompanyNameByCompanyId(PortalContext.CurrentUser.CompId);
            string companyAddress = _reportManager.GetCompanyAddressByCompanyId(PortalContext.CurrentUser.CompId);
            ReportParameter[] parameters =
            {
                new ReportParameter("param_CompanySector", companyName),
                new ReportParameter("param_CompanyAddress", companyAddress)
            };
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("DataSet1", reportdata) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 8.27, PageHeight = 11.69, MarginTop = 0.2, MarginLeft = 0.1, MarginRight = 0.1, MarginBottom = 0.2 };
            return ReportExtension.ToFile(reportType, path, reportDataSources, deviceInformation, parameters.ToList());
        }

        public ActionResult CommLcDetailReport(int reportTypeId)
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

            List<CommLcWithOrderDetailReport> reportdata = _reportManager.GetCommLcDetailReport();
            string path = Path.Combine(Server.MapPath("~/Areas/Commercial/Reports"), "LcDetail.rdlc");

            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            string companyName = _reportManager.GetCompanyNameByCompanyId(PortalContext.CurrentUser.CompId);
            string companyAddress = _reportManager.GetCompanyAddressByCompanyId(PortalContext.CurrentUser.CompId);
            ReportParameter[] parameters =
            {
                new ReportParameter("param_CompanySector", companyName) ,
                new ReportParameter("param_CompanyAddress", companyAddress)
            };
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("DataSet1", reportdata) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 8.27, PageHeight = 11.69, MarginTop = .25, MarginLeft = .2, MarginRight = .2, MarginBottom = .25 };
            return ReportExtension.ToFile(reportType, path, reportDataSources, deviceInformation, parameters.ToList());
        }

        public ActionResult CommercialInvoice(int reportTypeId)
        {
            decimal? totalAmount = 0;
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
            List<CommCommercialInvoiceReport> reportdata = _reportManager.GetCommercialInvoiceReport();

            foreach (var t in reportdata)
            {
                totalAmount += t.Rate * t.ItemQuantity;
            }
            foreach (var t in reportdata)
            {
                t.AmountInWords = Spell.SpellAmount.InWrods(Convert.ToDecimal(totalAmount)).Replace("Taka", "").ToUpper();
            }
            string path = Path.Combine(Server.MapPath("~/Areas/Commercial/Reports"), "CommercialInvoice.rdlc");
            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            string companyName = _reportManager.GetCompanyNameByCompanyId(PortalContext.CurrentUser.CompId);

            ReportParameter[] parameters = { new ReportParameter("param_CompanySector", companyName) };
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("DataSet1", reportdata) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 8.27, PageHeight = 11.69, MarginTop = .25, MarginLeft = .2, MarginRight = .2, MarginBottom = .25 };
            return ReportExtension.ToFile(reportType, path, reportDataSources, deviceInformation, parameters.ToList());
        }

        public ActionResult CommPackingList(int reportTypeId)
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
            List<CommPackingListReport> reportdata = _reportManager.GetPackingListReport(10);

            string path = Path.Combine(Server.MapPath("~/Areas/Commercial/Reports"), "PackingList.rdlc");
            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }

            string companyName = _reportManager.GetCompanyNameByCompanyId(PortalContext.CurrentUser.CompId);

            ReportParameter[] parameters = { new ReportParameter("param_CompanySector", companyName) };

            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("DataSet1", reportdata) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 8.27, PageHeight = 11.69, MarginTop = .25, MarginLeft = .2, MarginRight = .2, MarginBottom = .25 };
            return ReportExtension.ToFile(reportType, path, reportDataSources, deviceInformation, parameters.ToList());
        }

        public ActionResult CommLcOrderSummary(int reportTypeId)
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
            List<CommLcOrderSummary> reportdata = _reportManager.GetLcOrderSummary();

            string path = Path.Combine(Server.MapPath("~/Areas/Commercial/Reports"), "LcOrderSummary.rdlc");
            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            string companyName = _reportManager.GetCompanyNameByCompanyId(PortalContext.CurrentUser.CompId);
            string companyAddress = _reportManager.GetCompanyAddressByCompanyId(PortalContext.CurrentUser.CompId);

            ReportParameter[] parameters =
            {
              new ReportParameter("param_CompanySector", companyName),
              new ReportParameter("param_CompanyAddress", companyAddress)
            };
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("DataSet1", reportdata) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 8.27, PageHeight = 11.69, MarginTop = .25, MarginLeft = .2, MarginRight = .2, MarginBottom = .25 };
            return ReportExtension.ToFile(reportType, path, reportDataSources, deviceInformation, parameters.ToList());
        }

        public ActionResult LcStatusReport(ReportType reportTyle,string rStatus = "O", int receivingBankId = 0)
        {

            List<VwCommLcInfo> lcInfos = _reportManager.GetLcStatus(rStatus, receivingBankId);
            string path = Path.Combine(Server.MapPath("~/Areas/Commercial/Reports"), "LcStatusReport.rdlc");
            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("LcStatusDSet", lcInfos) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 14, PageHeight = 8.5, MarginTop = .2, MarginLeft = .1, MarginRight = .1, MarginBottom = .2 };
            var reportExport = ReportExtension.ToFile(reportTyle, path, reportDataSources, deviceInformation);
            return reportExport;
        }

        public ActionResult SalesContactReport(int LcId)
        {

            List<COMMLcInfo> lcInfos = _lcManager.GetLcInfoByLcId(LcId);
            List<CommSalseContact> SalesContacts = _salesContactManager.GetSalesContacts(LcId);
            List<CommExport> Exports = _exportManager.GetExportByLcId(LcId);


            string path = Path.Combine(Server.MapPath("~/Areas/Commercial/Reports"), "SalesContact.rdlc");
            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("DataSet1", Exports), new ReportDataSource("DataSet2", lcInfos), new ReportDataSource("DataSet3", SalesContacts) };

            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 14, PageHeight = 8.5, MarginTop = .2, MarginLeft = .1, MarginRight = .1, MarginBottom = .2 };
            var reportExport = ReportExtension.ToFile(ReportType.PDF, path, reportDataSources, deviceInformation);
            return reportExport;
        }

        public ActionResult SalesContactReportNew(int LcId)
        {


            DataTable SalesContacInfo = _reportManager.GetSalesContactByLcId(LcId);
            DataTable SalesContacExpInfo = _reportManager.GetSalesContactExpByLcId(LcId);
            string path = Path.Combine(Server.MapPath("~/Areas/Commercial/Reports"), "SalesContactReportCopy.rdlc");
            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("SalesContactDataSet", SalesContacInfo), new ReportDataSource("SalesContactExpDataSet", SalesContacExpInfo) };

            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 7.27, PageHeight = 11.69, MarginTop = .2, MarginLeft = .1, MarginRight = .1, MarginBottom = .2 };
            var reportExport = ReportExtension.ToFile(ReportType.PDF, path, reportDataSources, deviceInformation);
            return reportExport;
        }
        public ActionResult LcDetailsReport(int LcId)
        {


            DataTable SalesContacInfo = _reportManager.GetSalesContactByLcId(LcId);
            DataTable SalesContacExpInfo = _reportManager.GetSalesContactExpByLcId(LcId);
            string path = Path.Combine(Server.MapPath("~/Areas/Commercial/Reports"), "SalesContactDetailsReportCopy.rdlc");
            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("SalesContactDataSet", SalesContacInfo), new ReportDataSource("SalesContactExpDataSet", SalesContacExpInfo) };

            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 8.27, PageHeight = 11.69, MarginTop = .2, MarginLeft = .1, MarginRight = .1, MarginBottom = .2 };
            var reportExport = ReportExtension.ToFile(ReportType.PDF, path, reportDataSources, deviceInformation);
            return reportExport;
        }

        public ActionResult SalesContactDetailsReport(int LcId)
        {


            DataTable SalesContacInfo = _reportManager.GetSalesContactByLcId(LcId);
            DataTable SalesContacExpInfo = _reportManager.GetSalesContactExpByLcId(LcId);
            string path = Path.Combine(Server.MapPath("~/Areas/Commercial/Reports"), "SalesContactDetailsReportCopy.rdlc");
            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("SalesContactDataSet", SalesContacInfo), new ReportDataSource("SalesContactExpDataSet", SalesContacExpInfo) };

            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 8.27, PageHeight = 11.69, MarginTop = .2, MarginLeft = .1, MarginRight = .1, MarginBottom = .2 };
            var reportExport = ReportExtension.ToFile(ReportType.PDF, path, reportDataSources, deviceInformation);
            return reportExport;
        }

        public ActionResult BblcReportByItemType(BbLcViewModel model)
        {
            try
            {
                ModelState.Clear();

                IEnumerable lcTypeList = from LcType lcType in Enum.GetValues(typeof(LcType))
                                         select new { Id = (int)lcType, Name = lcType.ToString() };

                model.LcTypes = lcTypeList;
                model.Lcs = _lcManager.GetAllLcInfos();
                model.InventoryGroupNames = _inventoryManager.GetGroups();
                model.Banks = _lcManager.GetBankInfo("Receiving");
                CommBbLcInfo bbLcInfo = model;
                bbLcInfo.CompId = model.CompId;
                bbLcInfo.LcRefId = model.LcRefId;
                bbLcInfo.BbLcType = model.BbLcType;
                bbLcInfo.SupplierCompanyRefId = model.SupplierCompanyRefId;
                bbLcInfo.FromDate = model.FromDate;
                bbLcInfo.ToDate = model.ToDate;

                int totalRecords = 0;

                model.TotalRecords = totalRecords;
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return View(model);
        }

        public ActionResult BblcReportByItemTypeReport(BbLcViewModel model)
        {

            CommBbLcInfo bbLcInfo = model;
            bbLcInfo.CompId = model.CompId;
            bbLcInfo.ItemType = model.ItemType;
            bbLcInfo.IssuingBankId = model.IssuingBankId;
            bbLcInfo.FromDate = model.FromDate;
            bbLcInfo.ToDate = model.ToDate;
            DataTable bblcDtaTable = _reportManager.GetBblcByItemTypeAndBank(bbLcInfo);

            string path = Path.Combine(Server.MapPath("~/Areas/Commercial/Reports"), "BblcByItemTypeReport.rdlc");
            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("DataSet1", bblcDtaTable) };

            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 8.27, PageHeight = 11.69, MarginTop = .2, MarginLeft = .1, MarginRight = .1, MarginBottom = .2 };
            var reportExport = ReportExtension.ToFile(ReportType.PDF, path, reportDataSources, deviceInformation);
            return reportExport;
        }

        public ActionResult CashIncentiveReport(int LcId)
        {



            List<COMMLcInfo> reportdata = _reportManager.GetLcIndividual(LcId);
            string path = Path.Combine(Server.MapPath("~/Areas/Commercial/Reports"), "CashIncentiveReport.rdlc");
            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("InvoiceDataSet", reportdata) };

            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 14.69, PageHeight = 11.69, MarginTop = .2, MarginLeft = .1, MarginRight = .1, MarginBottom = .2 };
            var reportExport = ReportExtension.ToFile(ReportType.PDF, path, reportDataSources, deviceInformation);
            return reportExport;
        }

        public ActionResult CashIncentiveByDateRangeReport(int LcId)
        {



            List<COMMLcInfo> reportdata = _reportManager.GetLcIndividual(LcId);
            string path = Path.Combine(Server.MapPath("~/Areas/Commercial/Reports"), "CashIncentiveReport.rdlc");
            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("InvoiceDataSet", reportdata) };

            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 11.27, PageHeight = 11.69, MarginTop = .2, MarginLeft = .1, MarginRight = .1, MarginBottom = .2 };
            var reportExport = ReportExtension.ToFile(ReportType.PDF, path, reportDataSources, deviceInformation);
            return reportExport;
        }

        public ActionResult CashLcReport(string supplierName, string cashLcNo, string lcFromDate, string lcToDate, string shipmentMode, string portOfDelivery, string itemName, int printFormatId)
        {

            DateTime? fromD = new DateTime(2001, 1, 1);
            DateTime? toD = new DateTime(2020, 1, 1);

            if (!string.IsNullOrEmpty(lcFromDate))
                fromD = DateTime.Parse(lcFromDate, CultureInfo.GetCultureInfo("en-gb"));
            if (!string.IsNullOrEmpty(lcToDate))
                toD = DateTime.Parse(lcToDate, CultureInfo.GetCultureInfo("en-gb"));


            CommCashLc cashLc = new CommCashLc();
            cashLc.SupplierName = supplierName;
            cashLc.CashLcNo = cashLcNo;
            cashLc.FromDate = fromD;
            cashLc.ToDate = toD;
            cashLc.WayOfEntry = shipmentMode;
            cashLc.PortOfDelivery = portOfDelivery;
            cashLc.Item = itemName;


            List<CommCashLc> reportdata = _reportManager.GetCashLcInfo(cashLc).OrderByDescending(x => x.CashLcId).ToList();

            LocalReport lr = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/Areas/Commercial/Reports"), "CashLcReport.rdlc");
            if (System.IO.File.Exists(path))
                lr.ReportPath = path;
            else
                return View("Error");

            string companyId = PortalContext.CurrentUser.CompId;
            string companyName = _reportManager.GetCompanyNameByCompanyId(companyId);
            string companyAddress = _reportManager.GetCompanyAddressByCompanyId(companyId);

            ReportParameter[] parameters = new ReportParameter[2];
            parameters[0] = new ReportParameter("param_CompanySector", companyName);
            parameters[1] = new ReportParameter("param_CompanyAddress", companyAddress);

            string reportType = "";

            switch (printFormatId)
            {
                case 1:
                    reportType = "PDF";
                    break;
                case 2:
                    reportType = "Excel";
                    break;
            }

            ReportDataSource rd = new ReportDataSource("DataSet1", reportdata);
            lr.SetParameters(parameters);
            lr.DataSources.Add(rd);
            string mimeType;
            string encoding;
            string fileNameExtension;

            string deviceInfo =
                "<DeviceInfo>" +
                "  <OutputFormat>" + 2 + "</OutputFormat>" +
                "  <PageWidth>8.3in</PageWidth>" +
                "  <PageHeight>11.7in</PageHeight>" +
                "  <MarginTop>0..2in</MarginTop>" +
                "  <MarginLeft>.2in</MarginLeft>" +
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

        public ActionResult LcIndividualDetailReportV2(int LcId, ReportType reportType)
        {
            string reportName = "LcReportDetail"; 
            var reportParams = new List<ReportParameter> { 
                new ReportParameter("LcId", LcId.ToString()),
                new ReportParameter("CompId",PortalContext.CurrentUser.CompId),
                new ReportParameter("HostingServerAddress",AppConfig.HostingServerAddress)
            };
            return ReportExtension.ToSsrsFile(reportType, reportName, reportParams);
        }
    }
}