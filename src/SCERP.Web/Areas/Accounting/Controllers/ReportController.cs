using System.Globalization;
using System.Web.UI;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
//using iTextSharp.text.pdf;
using iTextSharp.text.pdf.qrcode;
using SCERP.BLL.Manager.AccountingManager;
using SCERP.Common;
using SCERP.Model.AccountingModel;
using SCERP.Web.Areas.Accounting.Models.ViewModels;
using SCERP.Model;
using System.Data;
using System.Collections;
using System.IO;
using System.Web.Services.Description;
using Microsoft.Reporting.WebForms;
using SCERP.Model.Custom;
using SCERP.Web.Helpers;
using SCERP.Web.Models;
using SCERP.BLL.IManager.IAccountingManager;

namespace SCERP.Web.Areas.Accounting.Controllers
{
    public class ReportController : BaseAccountingController
    {

        private readonly IVoucherMasterManager voucherMasterManager;
        private readonly Guid? _employeeGuidId = PortalContext.CurrentUser.UserId;
        public ReportController(IVoucherMasterManager voucherMasterManager)
        {
            this.voucherMasterManager = voucherMasterManager;

        }
        public ActionResult Index()
        {
            return View();
        }    
        public ActionResult ProxReport(string proxUrl)
        {
            ViewBag.ProxUrl = proxUrl;
            return View();
        }

    
        public ActionResult Voucher(long Id)
        {
            List<VoucherModel> voucher = new List<VoucherModel>();
            voucher = ReportAccountManger.GetVoucherInfo(Id);

            LocalReport lr = new LocalReport();

            string path = Path.Combine(Server.MapPath("~/Areas/Accounting/Reports"), "Voucher.rdlc");
            if (System.IO.File.Exists(path))
                lr.ReportPath = path;
            else
                return View("Index");

            ReportDataSource rd = new ReportDataSource("VoucherDS", voucher);

            lr.DataSources.Add(rd);

            string reportType = "pdf";
            string mimeType;
            string encoding;
            string fileNameExtension;

            string deviceInfo =
                "<DeviceInfo>" +
                "  <OutputFormat>" + 2 + "</OutputFormat>" +
                "  <PageWidth>8.3in</PageWidth>" +
                "  <PageHeight>5.84in</PageHeight>" +
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

        public ActionResult VoucherFromBalanceSheet(int? VoucherNoShow)
        {
            List<VoucherModel> voucher = new List<VoucherModel>();

            long id = ReportAccountManger.GetVoucherIdbyVoucherNo(VoucherNoShow);

            voucher = ReportAccountManger.GetVoucherInfo(id);

            LocalReport lr = new LocalReport();

            string path = Path.Combine(Server.MapPath("~/Areas/Accounting/Reports"), "Voucher.rdlc");
            if (System.IO.File.Exists(path))
                lr.ReportPath = path;
            else
                return View("Index");

            ReportDataSource rd = new ReportDataSource("VoucherDS", voucher);

            lr.DataSources.Add(rd);

            string reportType = "pdf";
            string mimeType;
            string encoding;
            string fileNameExtension;

            string deviceInfo =
                "<DeviceInfo>" +
                "  <OutputFormat>" + 2 + "</OutputFormat>" +
                "  <PageWidth>8.3in</PageWidth>" +
                "  <PageHeight>5.84in</PageHeight>" +
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

        public ActionResult VoucherMultiCurrency(long Id)
        {
            List<VoucherModel> voucher = new List<VoucherModel>();
            voucher = ReportAccountManger.GetVoucherMultiCurrencyInfo(Id);

            LocalReport lr = new LocalReport();

            string path = Path.Combine(Server.MapPath("~/Areas/Accounting/Reports"), "VoucherMultiCurrency.rdlc");
            if (System.IO.File.Exists(path))
                lr.ReportPath = path;
            else
                return View("Index");


            int? CurrencyId = ReportAccountManger.GetActiveCurrencyByVoucherMasterId(Id);

            ReportParameter[] parameters = new ReportParameter[1];

            if (CurrencyId == 2)
                parameters[0] = new ReportParameter("param_CurrencySymbol", "EUR");
            else if (CurrencyId == 3)
                parameters[0] = new ReportParameter("param_CurrencySymbol", "USD");
            else
                parameters[0] = new ReportParameter("param_CurrencySymbol", "BDT");


            ReportDataSource rd = new ReportDataSource("VoucherDS", voucher);
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
                "  <PageHeight>5.84in</PageHeight>" +
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

        public ActionResult VoucherStatement(VoucherEntryViewModel model)
        {
            ModelState.Clear();

            if (!model.IsSearch)
            {
                var printFormat = from PrintFormatType formatType in Enum.GetValues(typeof(PrintFormatType))
                                  select new { Id = (int)formatType, Name = formatType.ToString() };

                ViewBag.PrintFormatId = new SelectList(printFormat.AsEnumerable(), "Id", "Name", 1);

                model.VoucherList.FromDate = DateTime.Now.Date;
                model.VoucherList.ToDate = DateTime.Now.Date;
            }

            return View(model);
        }

        public ActionResult VoucherStatementPrint(DateTime? fromDate, DateTime? toDate, string voucherType, string ExportType)
        {
            List<VoucherModel> statement = new List<VoucherModel>();
            statement = ReportAccountManger.GetVoucherStatement(voucherType, fromDate, toDate);

            LocalReport lr = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/Areas/Accounting/Reports"), "VoucherStatement.rdlc");
            if (System.IO.File.Exists(path))
                lr.ReportPath = path;
            else
                return View("Index");

            ReportDataSource rd = new ReportDataSource("VoucherStatementDS", statement);
            lr.DataSources.Add(rd);
            string reportType = ExportType;
            string mimeType;
            string encoding;
            string fileNameExtension;

            string deviceInfo =
                "<DeviceInfo>" +
                "  <OutputFormat>" + ExportType + "</OutputFormat>" +
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


        public ActionResult VoucherSummary(VoucherEntryViewModel model)
        {
            ModelState.Clear();

            if (!model.IsSearch)
            {
                var printFormat = from PrintFormatType formatType in Enum.GetValues(typeof(PrintFormatType))
                                  select new { Id = (int)formatType, Name = formatType.ToString() };

                ViewBag.PrintFormatId = new SelectList(printFormat.AsEnumerable(), "Id", "Name", 1);

                model.VoucherList.FromDate = DateTime.Now.Date;
                model.VoucherList.ToDate = DateTime.Now.Date;
            }

            return View(model);
        }

        public ActionResult VoucherSummaryPrint(DateTime? fromDate, DateTime? toDate, string voucherType, string ExportType)
        {
            List<VoucherModel> statement = new List<VoucherModel>();
            statement = ReportAccountManger.GetVoucherSummary(voucherType, fromDate, toDate);

            LocalReport lr = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/Areas/Accounting/Reports"), "VoucherSummary.rdlc");
            if (System.IO.File.Exists(path))
                lr.ReportPath = path;
            else
                return View("Index");

            ReportDataSource rd = new ReportDataSource("VoucherSummaryDS", statement);
            lr.DataSources.Add(rd);
            string reportType = ExportType;
            string mimeType;
            string encoding;
            string fileNameExtension;

            string deviceInfo =
                "<DeviceInfo>" +
                "  <OutputFormat>" + ExportType + "</OutputFormat>" +
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

             
        public ActionResult ReceivePayment(VoucherEntryViewModel model)
        {
            ModelState.Clear();

            var companyId = voucherMasterManager.GetCostCentreByEmployeeId(_employeeGuidId);

            if (!model.IsSearch)
            {
                var printFormat = from PrintFormatType formatType in Enum.GetValues(typeof(PrintFormatType))
                                  select new { Id = (int)formatType, Name = formatType.ToString() };

                ViewBag.PrintFormatId = new SelectList(printFormat.AsEnumerable(), "Id", "Name", companyId);

                ViewBag.SectorId = new SelectList(BankVoucherEntryManager.GetAllCompanySector().AsEnumerable(), "Id", "SectorName", companyId);
                model.VoucherList.FromDate = DateTime.Now.Date;
                model.VoucherList.ToDate = DateTime.Now.Date;
            }

            return View(model);
        }

        public ActionResult ReceivePaymentPrint(string fromDate, string toDate, int sectorId, string ExportType)
        {
            var iObj = new Acc_ReportViewModel();
            var receivePayment = new List<ReceivePaymentReportModel>();
            var model = new ReceivePaymentReportModel();

            List<VoucherModel> receivedata = ReportAccountManger.GetReceivePaymentData(sectorId, "10206", "10207", fromDate, toDate);

            iObj.OpStartDate = "01/01/2013";
            iObj.OpEndDate = fromDate;
            iObj.StartDate = fromDate;
            iObj.EndDate = toDate;
            iObj.SectorCode = sectorId.ToString();
            iObj.TrialBalanceLebel = 5;

            DataTable dt = ReportAccountManger.GetTrialBalance(iObj);

            foreach (DataRow dr in dt.Rows)
            {
                if (dr["SubGrpControlCode"].ToString() == "10206" || dr["SubGrpControlCode"].ToString() == "10207")
                {
                    model = new ReceivePaymentReportModel();
                    model.OrderId = 1;
                    model.BalanceType = "Opening Balance";
                    model.ControlHead = "    " + dr["SubGrpControlName"].ToString();
                    model.AccountHead = "         " + dr["AccountName"].ToString();
                    model.Balance = Convert.ToDecimal(dr["OpeningBalance"].ToString());
                    receivePayment.Add(model);
                }

                if (dr["SubGrpControlCode"].ToString() == "10206" || dr["SubGrpControlCode"].ToString() == "10207")
                {
                    model = new ReceivePaymentReportModel();
                    model.OrderId = 4;
                    model.BalanceType = "Closing Balance";
                    model.ControlHead = "    " + dr["SubGrpControlName"].ToString();
                    model.AccountHead = "         " + dr["AccountName"].ToString();
                    model.Balance = Convert.ToDecimal(dr["ClosingBalance"].ToString());
                    receivePayment.Add(model);
                }
            }

            foreach (var item in receivedata)
            {
                if (item.Debit - item.Credit < 0)
                {
                    model = new ReceivePaymentReportModel();
                    model.OrderId = 2;
                    model.BalanceType = "Received";
                    model.ControlHead = "    " + item.ControlName;
                    model.AccountHead = "         " + item.AccountName;
                    model.Balance = Convert.ToDecimal(item.Credit);
                    receivePayment.Add(model);
                }

                else if (item.Debit - item.Credit > 0)
                {
                    model = new ReceivePaymentReportModel();
                    model.OrderId = 3;
                    model.BalanceType = "Payment";
                    model.ControlHead = "    " + item.ControlName;
                    model.AccountHead = "         " + item.AccountName;
                    model.Balance = Convert.ToDecimal(item.Debit);
                    receivePayment.Add(model);
                }
            }

            receivePayment = receivePayment.OrderBy(p => p.OrderId).ToList();

            LocalReport lr = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/Areas/Accounting/Reports"), "ReceivePayment.rdlc");
            if (System.IO.File.Exists(path))
                lr.ReportPath = path;
            else
                return View("Index");

            ReportDataSource rd = new ReportDataSource("ReceivePaymentDS", receivePayment);
            lr.DataSources.Add(rd);
            string reportType = ExportType;
            string mimeType;
            string encoding;
            string fileNameExtension;

            string deviceInfo =
                "<DeviceInfo>" +
                "  <OutputFormat>" + ExportType + "</OutputFormat>" +
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
        

        public ActionResult ControlSummary(int? page, string sort, GeneralLedgerViewModel model, int? fpId, int? sectorId, string GlId, string fromDate, string toDate, int? CostCentreId)
        {
            var companyId = voucherMasterManager.GetCostCentreByEmployeeId(_employeeGuidId);
            sectorId = companyId;

            ViewBag.FpId = new SelectList(OpeningBalaceManager.GetFinancialPeriod().AsEnumerable(), "Id", "PeriodName");
            ViewBag.SectorId = new SelectList(BankVoucherEntryManager.GetAllCompanySector().AsEnumerable(), "Id", "SectorName", sectorId);

            var costCentre = CostCentreManager.GetCostCentreById(CostCentreId);
            var costCentrelist = new List<Acc_CostCentre>();

            if (costCentre != null)
            {
                costCentrelist.Add(costCentre);
            }

            ViewBag.CostCentreId = costCentre != null ? new SelectList(costCentrelist, "Id", "CostCentreName", costCentre.Id) : new SelectList(costCentrelist, "Id", "CostCentreId");

            var printFormat = from PrintFormatType formatType in Enum.GetValues(typeof(PrintFormatType))
                              select new { Id = (int)formatType, Name = formatType.ToString() };

            ViewBag.PrintFormatId = new SelectList(printFormat.AsEnumerable(), "Id", "Name", companyId);

            var itemList = new List<GeneralLedgerViewModel>();

            if (GlId != "0" && GlId != "" && string.IsNullOrEmpty(GlId) == false)
            {
                string[] glCode = GlId.Split('-');
                var iObj = new Acc_ReportViewModel();
                iObj.SectorCode = sectorId.ToString();
                iObj.GLId = glCode[0];
                iObj.StartDate = fromDate;
                iObj.EndDate = toDate;
                itemList = (List<GeneralLedgerViewModel>)GetControlLedgerData(iObj);
            }
            model.GeneralLedger = itemList;
            return View(model);
        }

        public ActionResult ControlSummaryPrint(int? page, string sort, GeneralLedgerViewModel model, int? fpId, int? sectorId, string GlId, string fromDate, string toDate, int? CostCentreId, string ExportType)
        {

            var CompanySector = CompanySectorManager.GetCompanySectorById(sectorId).SectorName;

            List<TrialBalanceViewModel> ItemList = new List<TrialBalanceViewModel>();

            if (fpId != null)
            {
                var iObj = new Acc_ReportViewModel();
                int controlId = 0;
                iObj.SectorCode = sectorId.ToString();
                iObj.StartDate = fromDate;
                iObj.EndDate = toDate;

                if (int.TryParse(GlId.Substring(GlId.Length - 7, 7), out controlId))
                {
                    iObj.TrialBalanceLebel = 5;
                }

                else if (int.TryParse(GlId.Substring(GlId.Length - 5, 5), out controlId))
                {
                    iObj.TrialBalanceLebel = 4;
                }

                else if (int.TryParse(GlId.Substring(GlId.Length - 3, 3), out controlId))
                {
                    iObj.TrialBalanceLebel = 3;
                }


                ItemList = (List<TrialBalanceViewModel>)GetControlSummaryData(iObj, 1, controlId.ToString(CultureInfo.InvariantCulture), CompanySector);
            }

            LocalReport lr = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/Areas/Accounting/Reports"), "ControlSummary.rdlc");
            if (System.IO.File.Exists(path))
                lr.ReportPath = path;
            else
                return View("Index");

            ReportDataSource rd = new ReportDataSource("ControlSummary", ItemList);
            lr.DataSources.Add(rd);
            string reportType = ExportType;
            string mimeType;
            string encoding;
            string fileNameExtension;

            string deviceInfo =
                "<DeviceInfo>" +
                "  <OutputFormat>" + ExportType + "</OutputFormat>" +
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

        private object GetControlSummaryData(Acc_ReportViewModel iObj, int? openingOption, string ControlId, string CompanySector)
        {
            iObj.OpStartDate = "01/01/2013";
            iObj.OpEndDate = iObj.StartDate;
            List<TrialBalanceViewModel> ItemList = new List<TrialBalanceViewModel>();
            DataTable dt = ReportAccountManger.GetTrialBalance(iObj);

            decimal dlTotalDebit = 0;
            decimal dlTotalCredit = 0;

            decimal dlTempDebit = 0;
            decimal dlTempCredit = 0;
            foreach (DataRow dr in dt.Rows)
            {
                dlTempDebit = 0;
                dlTempCredit = 0;
                if (openingOption == 1)
                {
                    dlTempDebit = Convert.ToDecimal(dr["Debit"].ToString());
                    dlTempCredit = Convert.ToDecimal(dr["Credit"].ToString());
                }
                else
                {
                    if (dr["ClsControlCode"].ToString().Substring(0, 1) == "1" ||
                        dr["ClsControlCode"].ToString().Substring(0, 1) == "4")
                    {
                        if (Convert.ToDecimal(dr["ClosingBalance"].ToString()) < 0)
                            dlTempCredit = (Convert.ToDecimal(dr["ClosingBalance"].ToString()) * -1);
                        else
                            dlTempDebit = Convert.ToDecimal(dr["ClosingBalance"].ToString());
                    }
                    else
                    {
                        if (Convert.ToDecimal(dr["ClosingBalance"].ToString()) < 0)
                            dlTempDebit = (Convert.ToDecimal(dr["ClosingBalance"].ToString()) * -1);
                        else
                            dlTempCredit = Convert.ToDecimal(dr["ClosingBalance"].ToString());
                    }
                }

                if (iObj.TrialBalanceLebel == 3 && dr["GrpControlCode"].ToString() == ControlId)
                {
                    ItemList.Add(new TrialBalanceViewModel()
                    {
                        ClasslCode = dr["ClsControlCode"].ToString(),
                        ClasslHead = dr["ClsControlName"].ToString(),
                        GrouplHead = dr["GrpControlName"].ToString(),
                        SubGrouplHead = dr["SubGrpControlName"].ToString(),
                        ControlHead = dr["GrpControlName"].ToString(),
                        GlHead = dr["SubGrpControlName"].ToString(),

                        OpeningBalance = Convert.ToDecimal(dr["OpeningBalance"].ToString()),
                        TotalDebit = dlTempDebit,
                        TotalCredit = dlTempCredit,
                        ClosingBalance = Convert.ToDecimal(dr["ClosingBalance"].ToString()),
                        DateBetween = iObj.StartDate + " To " + iObj.EndDate,
                        CompanyName = CompanySector,
                    });
                }

                else if (iObj.TrialBalanceLebel == 4 && dr["SubGrpControlCode"].ToString() == ControlId)
                {
                    ItemList.Add(new TrialBalanceViewModel()
                    {
                        ClasslCode = dr["ClsControlCode"].ToString(),
                        ClasslHead = dr["ClsControlName"].ToString(),
                        GrouplHead = dr["GrpControlName"].ToString(),
                        SubGrouplHead = dr["SubGrpControlName"].ToString(),
                        ControlHead = dr["SubGrpControlName"].ToString(),
                        GlHead = dr["ControlCodeName"].ToString(),

                        OpeningBalance = Convert.ToDecimal(dr["OpeningBalance"].ToString()),
                        TotalDebit = dlTempDebit,
                        TotalCredit = dlTempCredit,
                        ClosingBalance = Convert.ToDecimal(dr["ClosingBalance"].ToString()),
                        DateBetween = iObj.StartDate + " To " + iObj.EndDate,
                        CompanyName = CompanySector,
                    });
                }

                else if (iObj.TrialBalanceLebel == 5 && dr["ControlContolCode"].ToString() == ControlId)
                {
                    ItemList.Add(new TrialBalanceViewModel()
                    {
                        ClasslCode = dr["ClsControlCode"].ToString(),
                        ClasslHead = dr["ClsControlName"].ToString(),
                        GrouplHead = dr["GrpControlName"].ToString(),
                        SubGrouplHead = dr["SubGrpControlName"].ToString(),
                        ControlHead = dr["ControlCodeName"].ToString(),
                        GlHead = dr["AccountName"].ToString(),

                        OpeningBalance = Convert.ToDecimal(dr["OpeningBalance"].ToString()),
                        TotalDebit = dlTempDebit,
                        TotalCredit = dlTempCredit,
                        ClosingBalance = Convert.ToDecimal(dr["ClosingBalance"].ToString()),
                        DateBetween = iObj.StartDate + " To " + iObj.EndDate,
                        CompanyName = CompanySector,
                    });
                }

                dlTotalDebit += dlTempDebit;
                dlTotalCredit += dlTempCredit;
            }

            return ItemList;
        }
       

        public ActionResult GeneralLedgerDetail(int? page, string sort, GeneralLedgerViewModel model, int? fpId, int? sectorId, string GlId, string fromDate, string toDate, int? CostCentreId)
        {
            ModelState.Clear();

            var companyId = voucherMasterManager.GetCostCentreByEmployeeId(_employeeGuidId);
            sectorId = companyId;

            var activeFinancialPeriod = ReportAccountManger.GetActiveFinancialPeriod();

            ViewBag.FpId = new SelectList(OpeningBalaceManager.GetFinancialPeriod().AsEnumerable(), "Id", "PeriodName", activeFinancialPeriod.Id);
            ViewBag.SectorId = new SelectList(BankVoucherEntryManager.GetAllCompanySector().AsEnumerable(), "Id", "SectorName", sectorId);

            ViewBag.FromDate = activeFinancialPeriod.PeriodStartDate.GetValueOrDefault().ToString("dd/MM/yyy");
            ViewBag.ToDate = activeFinancialPeriod.PeriodEndDate.GetValueOrDefault().ToString("dd/MM/yyy");

            var costCentre = CostCentreManager.GetCostCentreById(CostCentreId);
            var costCentrelist = new List<Acc_CostCentre>();

            if (costCentre != null)
            {
                costCentrelist.Add(costCentre);
            }

            var currencyList = from CurrencyType currency in Enum.GetValues(typeof(CurrencyType))
                               select new { Id = (int)currency, Name = currency.ToString() };

            ViewBag.CostCentreId = costCentre != null ? new SelectList(costCentrelist, "Id", "CostCentreName", costCentre.Id) : new SelectList(costCentrelist, "Id", "CostCentreId");
            ViewBag.currencyId = new SelectList(currencyList, "Id", "Name", (int)CurrencyType.BDT);

            var printFormat = from PrintFormatType formatType in Enum.GetValues(typeof(PrintFormatType))
                              select new { Id = (int)formatType, Name = formatType.ToString() };

            ViewBag.PrintFormatId = new SelectList(printFormat.AsEnumerable(), "Id", "Name", companyId);


            List<GeneralLedgerViewModel> ItemList = new List<GeneralLedgerViewModel>();

            if (GlId != "0" && GlId != "" && string.IsNullOrEmpty(GlId) == false)
            {
                string[] glCode = GlId.Split('-');
                var iObj = new Acc_ReportViewModel();
                iObj.SectorCode = sectorId.ToString();
                iObj.GLId = glCode[0];
                iObj.StartDate = fromDate;
                iObj.EndDate = toDate;
                ItemList = (List<GeneralLedgerViewModel>)GetControlLedgerData(iObj);
            }
            model.GeneralLedger = ItemList;
            return View(model);
        }

        public ActionResult GeneralLedgerDetailPrint(int? page, string sort, GeneralLedgerViewModel model, int? fpId, int? sectorId, string GlId, string fromDate, string toDate, int? CostCentreId, int CurrencyId, string ExportType)
        {
            ModelState.Clear();

            var itemList = new List<GeneralLedgerDetailModel>();

            if (fpId != null)
            {
                DateTime FromDate = DateTime.ParseExact(fromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                DateTime ToDate = DateTime.ParseExact(toDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                itemList = ReportAccountManger.GetGeneralLedgerDetail(GlId.Substring(GlId.Length - 10, 10), sectorId, CostCentreId, CurrencyId, FromDate, ToDate);
            }

            LocalReport lr = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/Areas/Accounting/Reports"), "GeneralLedgerDetail.rdlc");
            if (System.IO.File.Exists(path))
                lr.ReportPath = path;
            else
                return View("Index");

            ReportParameter[] parameters = new ReportParameter[1];

            if (CurrencyId == 2)
                parameters[0] = new ReportParameter("param_CurrencySymbol", "EUR");
            else if (CurrencyId == 3)
                parameters[0] = new ReportParameter("param_CurrencySymbol", "USD");
            else
                parameters[0] = new ReportParameter("param_CurrencySymbol", "BDT");

            ReportDataSource rd = new ReportDataSource("glDetail", itemList);
            lr.SetParameters(parameters);
            lr.DataSources.Add(rd);
            string reportType = ExportType;
            string mimeType;
            string encoding;
            string fileNameExtension;

            string deviceInfo =
                "<DeviceInfo>" +
                "  <OutputFormat>" + ExportType + "</OutputFormat>" +
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

       

        //***********************************************************************************************************************************************

        #region ChartOfAccounts

        public ActionResult ChartOfAccountsPrint()
        {
            ModelState.Clear();

            List<ChartOfAccountsReportModel> itemList = ReportAccountManger.GetChartOfAccountsReportData();

            LocalReport lr = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/Areas/Accounting/Reports"), "ChartOfAccounts.rdlc");
            if (System.IO.File.Exists(path))
                lr.ReportPath = path;
            else
                return View("Index");

            ReportDataSource rd = new ReportDataSource("ChartDS", itemList);

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

        public ActionResult ChartOfAccountsPrintExcel()
        {
            ModelState.Clear();

            List<ChartOfAccountsReportModel> itemList = ReportAccountManger.GetChartOfAccountsReportData();

            LocalReport lr = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/Areas/Accounting/Reports"), "ChartOfAccounts.rdlc");
            if (System.IO.File.Exists(path))
                lr.ReportPath = path;
            else
                return View("Index");

            ReportDataSource rd = new ReportDataSource("ChartDS", itemList);

            lr.DataSources.Add(rd);
            string reportType = "excel";
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

        #endregion

        //***********************************************************************************************************************************************

        #region GeneralLedger

        public ActionResult GeneralLedger(int? page, string sort, GeneralLedgerViewModel model, int? fpId, int? sectorId, int? costcentreId, string GlId, string fromDate, string toDate, int? CostCentreId)
        {
            var companyId = voucherMasterManager.GetCostCentreByEmployeeId(_employeeGuidId);
            sectorId = companyId;

            ViewBag.FpId = new SelectList(OpeningBalaceManager.GetFinancialPeriod().AsEnumerable(), "Id", "PeriodName");
            ViewBag.SectorId = new SelectList(BankVoucherEntryManager.GetAllCompanySector().AsEnumerable(), "Id", "SectorName", sectorId);

            var costCentre = CostCentreManager.GetCostCentreById(CostCentreId);
            var costCentrelist = new List<Acc_CostCentre>();

            if (costCentre != null)
            {
                costCentrelist.Add(costCentre);
            }

            ViewBag.CostCentreId = costCentre != null ? new SelectList(costCentrelist, "Id", "CostCentreName", costCentre.Id) : new SelectList(costCentrelist, "Id", "CostCentreId");

            var currencyList = from CurrencyType currency in Enum.GetValues(typeof(CurrencyType))
                               select new { Id = (int)currency, Name = currency.ToString() };

            ViewBag.currencyId = new SelectList(currencyList, "Id", "Name", (int)CurrencyType.BDT);

            var printFormat = from PrintFormatType formatType in Enum.GetValues(typeof(PrintFormatType))
                              select new { Id = (int)formatType, Name = formatType.ToString() };

            ViewBag.PrintFormatId = new SelectList(printFormat.AsEnumerable(), "Id", "Name", companyId);

            List<GeneralLedgerViewModel> ItemList = new List<GeneralLedgerViewModel>();

            if (GlId != "0" && GlId != "" && string.IsNullOrEmpty(GlId) == false)
            {
                string glCode = GlId.Substring(GlId.Length - 10, 10);

                var reportObject = new Acc_ReportViewModel();

                if (glCode[0] == '1' || glCode[0] == '2')
                {
                    reportObject.OpStartDate = "01/01/2013";
                }
                else
                {
                    var periodStartDate = ReportAccountManger.GetFinancialPeriod(fpId.ToString()).PeriodStartDate;
                    if (periodStartDate != null) reportObject.OpStartDate = periodStartDate.Value.ToString("dd/MM/yyyy");
                }

                reportObject.SectorCode = sectorId.ToString();
                reportObject.CostCentreID = costcentreId.ToString();
                reportObject.StartDate = fromDate;
                reportObject.EndDate = toDate;
                reportObject.GLId = glCode;
                ItemList = (List<GeneralLedgerViewModel>)GetGeneralLedgerData(reportObject);
            }

            model.GeneralLedger = ItemList;
            return View(model);
        }

        public ActionResult PrintGeneralLedger(string FpId, string SectorId, int currencyId, string SectorText, string fromDate, string toDate, string GlId = "", string ExportType = "")
        {
            string generalLedgerCode = GlId.Substring(GlId.Length - 10, 10);

            var reportObject = new Acc_ReportViewModel();

            if (generalLedgerCode[0] == '1' || generalLedgerCode[0] == '2')
            {
                reportObject.OpStartDate = "01/01/2013";
            }
            else
            {
                var periodStartDate = ReportAccountManger.GetFinancialPeriod(FpId).PeriodStartDate;
                if (periodStartDate != null) reportObject.OpStartDate = periodStartDate.Value.ToString("dd/MM/yyyy");
            }

            reportObject.SectorCode = SectorId;
            reportObject.GLId = generalLedgerCode;
            reportObject.StartDate = fromDate;
            reportObject.EndDate = toDate;
            reportObject.currencyId = currencyId;

            List<GeneralLedgerViewModel> item = (List<GeneralLedgerViewModel>)GetGeneralLedgerData(reportObject);

            ReportClass rptH = new ReportClass();
            ArrayList iArrayRpt = new ArrayList();
            rptH.FileName = Server.MapPath("/Reports/Accounting/GeneralLedger.rpt");
            rptH.Load();
            GeneralLedgerViewModel obj;
            Company company = CompanyManager.GetCompanyInfo();
            if (SectorText == "- Select -")
                SectorText = "Consulate";

            foreach (GeneralLedgerViewModel dr in item)
            {
                obj = new GeneralLedgerViewModel();
                obj.CompanyName = ReportAccountManger.GetVoucherNoByRefNo(Convert.ToInt64(dr.VoucherNoShow));
                obj.SectorName = SectorText;
                obj.GLHeadName = dr.GLHeadName ?? generalLedgerCode;
                obj.DateBetween = fromDate + " to " + toDate;
                obj.Particulars = dr.Particulars;
                obj.VoucherDateShow = dr.VoucherDateShow;
                obj.VoucherNoShow = dr.VoucherNoShow;
                obj.TotalDebit = dr.TotalDebit;
                obj.TotalCredit = dr.TotalCredit;
                obj.Balance = dr.Balance;
                obj.GlId = currencyId;
                iArrayRpt.Add(obj);
            }

            rptH.SetDataSource(iArrayRpt);

            if (ExportType == "Excel")
            {
                Stream stream = rptH.ExportToStream(ExportFormatType.Excel);
                rptH.Dispose();
                return File(stream, "application/vnd.ms-excel", "GeneralLedger.xls");
            }
            else
            {
                Stream stream = rptH.ExportToStream(ExportFormatType.PortableDocFormat);
                rptH.Dispose();
                return File(stream, "application/pdf");
            }
        }

        private object GetGeneralLedgerData(Acc_ReportViewModel model)
        {
            decimal currencyValue = 0;

            List<GeneralLedgerViewModel> ItemList = new List<GeneralLedgerViewModel>();
            DataTable dt = ReportAccountManger.GetGeneralLedger(model);
            decimal dlOPeningBalance = 0;
            decimal dlClosingBalance = 0;
            decimal dlTotalDebit = 0;
            decimal dlTotalCredit = 0;
            dlClosingBalance = dlOPeningBalance;

            var iObj = new Acc_ReportViewModel();
            iObj.SectorCode = model.SectorCode;
            iObj.TrialBalanceLebel = 5;
            iObj.StartDate = model.StartDate;
            iObj.EndDate = model.EndDate;
            iObj.OpStartDate = model.OpStartDate;
            iObj.OpEndDate = iObj.StartDate;
            iObj.AccountCode = model.GLId;
            DataTable dtOp = ReportAccountManger.GetTrialBalance(iObj);

            if (dtOp.Rows.Count > 0)
            {
                dlOPeningBalance = Convert.ToDecimal(dtOp.Rows[0]["OpeningBalance"].ToString());
                dlClosingBalance = dlOPeningBalance;
            }
            if (dt.Rows.Count > 0)
            {

                ItemList.Add(new GeneralLedgerViewModel()
                {
                    Particulars = "Opening Balance",
                    Balance = dlOPeningBalance,
                    TotalDebit = 0,
                    TotalCredit = 0,
                    CompanyName = "ABC",
                    GLHeadName = dt.Rows[0]["AccountName"].ToString()
                });
            }

            else
            {
                DateTime fromDate = DateTime.Parse(model.OpStartDate, CultureInfo.GetCultureInfo("en-gb"));
                DateTime todate = DateTime.Parse(model.StartDate, CultureInfo.GetCultureInfo("en-gb"));

                GeneralLedgerDetailModel ledger = ReportAccountManger.GetOpeningBalanceByGlId(model.SectorCode, fromDate, todate, model.GLId);

                if (ledger.Balance != null)
                    ItemList.Add(new GeneralLedgerViewModel()
                    {
                        Particulars = "Opening Balance",
                        Balance = ledger.Balance.Value,
                        TotalDebit = 0,
                        TotalCredit = 0,
                        CompanyName = "ABC",
                        GLHeadName = ledger.GLName
                    });
            }

            foreach (DataRow dr in dt.Rows)
            {
                if (model.currencyId == 1)
                    currencyValue = Convert.ToDecimal(dr["FirstCurValue"].ToString());
                else if (model.currencyId == 2)
                    currencyValue = Convert.ToDecimal(dr["SecendCurValue"].ToString());
                else if (model.currencyId == 3)
                    currencyValue = Convert.ToDecimal(dr["ThirdCurValue"].ToString());
                else
                    currencyValue = 1;

                dlClosingBalance = dlClosingBalance + Convert.ToDecimal(dr["Debit"].ToString()) - Convert.ToDecimal(dr["Credit"].ToString());

                ItemList.Add(new GeneralLedgerViewModel()
                {
                    TotalDebit = Convert.ToDecimal(dr["Debit"].ToString()) / currencyValue,
                    TotalCredit = Convert.ToDecimal(dr["Credit"].ToString()) / currencyValue,
                    Particulars = dr["Particulars"].ToString(),
                    VoucherDateShow = dr["VoucherDate"].ToString(),
                    VoucherNoShow = dr["VoucherNo"].ToString(),
                    VoucherNoMasterId = dr["VoucherMasterId"].ToString(),
                    Balance = dlClosingBalance / currencyValue,
                    GLHeadName = dr["AccountName"].ToString(),
                    VoucherRefNo = dr["VoucherRefNo"].ToString()
                });
                dlTotalDebit += Convert.ToDecimal(dr["Debit"].ToString()) / currencyValue;
                dlTotalCredit += Convert.ToDecimal(dr["Credit"].ToString()) / currencyValue;
            }

            if (ItemList.Count > 1)
            {
                ItemList[0].Balance = ItemList[1].Balance + ItemList[1].TotalCredit - ItemList[1].TotalDebit;
            }

            ItemList.Add(new GeneralLedgerViewModel()
            {
                Particulars = "Total :",
                TotalDebit = dlTotalDebit,
                TotalCredit = dlTotalCredit,
                Balance = 0,
            });

            return ItemList;
        }

        #endregion

        //***********************************************************************************************************************************************

        #region ControlLedger

        public ActionResult ControlLedger(int? page, string sort, GeneralLedgerViewModel model, int? fpId, int? sectorId, string GlId, string fromDate, string toDate, int? CostCentreId)
        {
            ViewBag.FpId = new SelectList(OpeningBalaceManager.GetFinancialPeriod().AsEnumerable(), "Id", "PeriodName");
            ViewBag.SectorId = new SelectList(BankVoucherEntryManager.GetAllCompanySector().AsEnumerable(), "Id", "SectorName");

            var costCentre = CostCentreManager.GetCostCentreById(CostCentreId);
            var costCentrelist = new List<Acc_CostCentre>();

            if (costCentre != null)
            {
                costCentrelist.Add(costCentre);
            }
            ViewBag.CostCentreId = costCentre != null ? new SelectList(costCentrelist, "Id", "CostCentreName", costCentre.Id) : new SelectList(costCentrelist, "Id", "CostCentreId");

            List<GeneralLedgerViewModel> ItemList = new List<GeneralLedgerViewModel>();

            if (GlId != "0" && GlId != "" && string.IsNullOrEmpty(GlId) == false)
            {
                string[] glCode = GlId.Split('-');
                var iObj = new Acc_ReportViewModel();
                iObj.SectorCode = sectorId.ToString();
                iObj.GLId = glCode[0];
                iObj.StartDate = fromDate;
                iObj.EndDate = toDate;
                ItemList = (List<GeneralLedgerViewModel>)GetControlLedgerData(iObj);
            }
            model.GeneralLedger = ItemList;
            return View(model);
        }

        public ActionResult PrintControlLedger(string FpId = "", string SectorId = "", string SectorText = "", string GlId = "", string fromDate = "", string toDate = "", string ExportType = "")
        {
            var iObj = new Acc_ReportViewModel();
            iObj.SectorCode = SectorId;
            iObj.TrialBalanceLebel = 5;
            iObj.StartDate = fromDate;
            iObj.EndDate = toDate;
            iObj.FpId = FpId;

            string controlCode = GlId.Substring(GlId.Length - 7, 7);
            string controlName = GlId.Substring(0, GlId.Length - 8);

            List<TrialBalanceViewModel> TrItemList = new List<TrialBalanceViewModel>();
            TrItemList = (List<TrialBalanceViewModel>)GetTrialBalanceControlData(iObj, 2, controlCode);

            ReportClass rptH = new ReportClass();
            ArrayList iArrayRpt = new ArrayList();
            rptH.FileName = Server.MapPath("/Reports/Accounting/ControlLedger.rpt");
            rptH.Load();

            GeneralLedgerViewModel obj;
            Company company = CompanyManager.GetCompanyInfo();
            string FpName = "";
            if (!string.IsNullOrEmpty(FpId))
                FpName = JournalVoucherEntryManager.GetFinancialPeriodById(Convert.ToInt32(FpId));

            foreach (TrialBalanceViewModel dr in TrItemList)
            {
                obj = new GeneralLedgerViewModel();
                obj.CompanyName = company.Name;
                obj.SectorName = SectorText;
                obj.GLHeadName = dr.GLHeadName ?? controlName;
                obj.DateBetween = fromDate + " to " + toDate;
                obj.Particulars = dr.GlHead;
                obj.TotalDebit = dr.TotalDebit;
                obj.TotalCredit = dr.TotalCredit;
                obj.Balance = dr.OpeningBalance + dr.TotalDebit - dr.TotalCredit;
                obj.OpeningBalance = dr.OpeningBalance;
                iArrayRpt.Add(obj);
            }

            rptH.SetDataSource(iArrayRpt);

            if (ExportType == "XL")
            {
                Stream stream = rptH.ExportToStream(ExportFormatType.Excel);
                rptH.Dispose();
                return File(stream, "application/vnd.ms-excel", "GeneralLedger.xls");
            }

            else
            {
                Stream stream = rptH.ExportToStream(ExportFormatType.PortableDocFormat);
                rptH.Dispose();
                return File(stream, "application/pdf");
            }
        }

        private object GetControlLedgerData(Acc_ReportViewModel model)
        {
            List<GeneralLedgerViewModel> ItemList = new List<GeneralLedgerViewModel>();
            DataTable dt = ReportAccountManger.GetGeneralLedger(model);

            //decimal dlClosingBalance = 0;
            //decimal dlTotalDebit = 0;
            //decimal dlTotalCredit = 0;

            var iObj = new Acc_ReportViewModel();
            iObj.SectorCode = model.SectorCode;
            iObj.TrialBalanceLebel = 4;
            iObj.StartDate = model.StartDate;
            iObj.EndDate = model.EndDate;

            List<TrialBalanceViewModel> TrItemList = new List<TrialBalanceViewModel>();
            TrItemList = (List<TrialBalanceViewModel>)GetTrialBalanceData(iObj, 2);

            foreach (TrialBalanceViewModel dr in TrItemList)
            {
                ItemList.Add(new GeneralLedgerViewModel()
                {
                    TotalDebit = Convert.ToDecimal(dr.TotalDebit),
                    TotalCredit = Convert.ToDecimal(dr.TotalCredit),
                    Particulars = dr.SubGrouplHead,
                    Balance = dr.ClosingBalance,
                    OpeningBalance = dr.OpeningBalance
                });
            }

            return ItemList;
        }

        #endregion

        //***********************************************************************************************************************************************

        #region TrialBalance

        public ActionResult GroupTrialBalance(int? page, string sort, TrialBalanceViewModel model, int? fpId, int? sectorId, int? openingOption, string fromDate, string toDate)
        {
            var companyId = voucherMasterManager.GetCostCentreByEmployeeId(_employeeGuidId);
            sectorId = companyId;

            ViewBag.FpId = new SelectList(OpeningBalaceManager.GetFinancialPeriod().AsEnumerable(), "Id", "PeriodName");
            ViewBag.SectorId = new SelectList(BankVoucherEntryManager.GetAllCompanySector().AsEnumerable(), "Id", "SectorName", sectorId);

            ViewBag.CostCentreId = new SelectList(new List<Acc_CostCentre>(), "Id", "CostCentreId");
            ViewBag.OpeningOption =
                new SelectList(
                    new[] { new { Id = "1", Value = "Show Opening" }, new { Id = "2", Value = "Hide Opening" } }, "Id",
                    "Value");

            var printFormat = from PrintFormatType formatType in Enum.GetValues(typeof(PrintFormatType))
                              select new { Id = (int)formatType, Name = formatType.ToString() };

            ViewBag.PrintFormatId = new SelectList(printFormat.AsEnumerable(), "Id", "Name", companyId);

            var itemList = new List<TrialBalanceViewModel>();

            if (fpId != null)
            {
                var iObj = new Acc_ReportViewModel();
                iObj.SectorCode = sectorId.ToString();
                iObj.TrialBalanceLebel = 2;
                iObj.StartDate = fromDate;
                iObj.EndDate = toDate;
                itemList = (List<TrialBalanceViewModel>)GetTrialBalanceData(iObj, openingOption);
            }
            model.TrialBalance = itemList;
            return View(model);
        }

        public ActionResult SubGroupTrialBalance(int? page, string sort, TrialBalanceViewModel model, int? fpId, int? sectorId, int? openingOption, string fromDate, string toDate)
        {
            var companyId = voucherMasterManager.GetCostCentreByEmployeeId(_employeeGuidId);
            sectorId = companyId;

            ViewBag.FpId = new SelectList(OpeningBalaceManager.GetFinancialPeriod().AsEnumerable(), "Id", "PeriodName");
            ViewBag.SectorId = new SelectList(BankVoucherEntryManager.GetAllCompanySector().AsEnumerable(), "Id", "SectorName", sectorId);
            ViewBag.CostCentreId = new SelectList(new List<Acc_CostCentre>(), "Id", "CostCentreId");

            ViewBag.OpeningOption =
                new SelectList(
                    new[] { new { Id = "1", Value = "Show Opening" }, new { Id = "2", Value = "Hide Opening" } }, "Id",
                    "Value");

            var printFormat = from PrintFormatType formatType in Enum.GetValues(typeof(PrintFormatType))
                              select new { Id = (int)formatType, Name = formatType.ToString() };

            ViewBag.PrintFormatId = new SelectList(printFormat.AsEnumerable(), "Id", "Name", companyId);

            var itemList = new List<TrialBalanceViewModel>();

            if (fpId != null)
            {
                var iObj = new Acc_ReportViewModel();
                iObj.SectorCode = sectorId.ToString();
                iObj.TrialBalanceLebel = 3;
                iObj.StartDate = fromDate;
                iObj.EndDate = toDate;
                itemList = (List<TrialBalanceViewModel>)GetTrialBalanceData(iObj, openingOption);
            }
            model.TrialBalance = itemList;
            return View(model);
        }

        public ActionResult ControlTrialBalance(int? page, string sort, TrialBalanceViewModel model, int? fpId, int? sectorId, int? openingOption, string fromDate, string toDate)
        {
            var companyId = voucherMasterManager.GetCostCentreByEmployeeId(_employeeGuidId);
            sectorId = companyId;

            ViewBag.FpId = new SelectList(OpeningBalaceManager.GetFinancialPeriod().AsEnumerable(), "Id", "PeriodName");
            ViewBag.SectorId = new SelectList(BankVoucherEntryManager.GetAllCompanySector().AsEnumerable(), "Id", "SectorName", sectorId);
            ViewBag.CostCentreId = new SelectList(new List<Acc_CostCentre>(), "Id", "CostCentreId");

            ViewBag.OpeningOption =
                new SelectList(
                    new[] { new { Id = "1", Value = "Show Opening" }, new { Id = "2", Value = "Hide Opening" } }, "Id",
                    "Value", openingOption);

            var printFormat = from PrintFormatType formatType in Enum.GetValues(typeof(PrintFormatType))
                              select new { Id = (int)formatType, Name = formatType.ToString() };

            ViewBag.PrintFormatId = new SelectList(printFormat.AsEnumerable(), "Id", "Name", companyId);

            var itemList = new List<TrialBalanceViewModel>();

            if (fpId != null)
            {
                var iObj = new Acc_ReportViewModel();
                iObj.SectorCode = sectorId.ToString();
                iObj.TrialBalanceLebel = 4;
                iObj.StartDate = fromDate;
                iObj.EndDate = toDate;
                itemList = (List<TrialBalanceViewModel>)GetTrialBalanceData(iObj, openingOption);
            }
            model.TrialBalance = itemList;
            return View(model);
        }

        public ActionResult GLTrialBalance(int? page, string sort, TrialBalanceViewModel model, int? fpId, int? sectorId, int? openingOption, string fromDate, string toDate)
        {
            var companyId = voucherMasterManager.GetCostCentreByEmployeeId(_employeeGuidId);
            sectorId = companyId;

            ViewBag.FpId = new SelectList(OpeningBalaceManager.GetFinancialPeriod().AsEnumerable(), "Id", "PeriodName");
            ViewBag.SectorId = new SelectList(BankVoucherEntryManager.GetAllCompanySector().AsEnumerable(), "Id", "SectorName", sectorId);
            ViewBag.CostCentreId = new SelectList(new List<Acc_CostCentre>(), "Id", "CostCentreId");

            ViewBag.OpeningOption =
                new SelectList(
                    new[] { new { Id = "1", Value = "Show Opening" }, new { Id = "2", Value = "Hide Opening" } }, "Id",
                    "Value");

            var printFormat = from PrintFormatType formatType in Enum.GetValues(typeof(PrintFormatType))
                              select new { Id = (int)formatType, Name = formatType.ToString() };

            ViewBag.PrintFormatId = new SelectList(printFormat.AsEnumerable(), "Id", "Name", companyId);

            var itemList = new List<TrialBalanceViewModel>();

            if (fpId != null)
            {
                var iObj = new Acc_ReportViewModel();
                iObj.SectorCode = sectorId.ToString();
                iObj.TrialBalanceLebel = 5;
                iObj.StartDate = fromDate;
                iObj.EndDate = toDate;
                itemList = (List<TrialBalanceViewModel>)GetTrialBalanceData(iObj, openingOption);
            }
            model.TrialBalance = itemList;
            return View(model);
        }

        private object GetBalanceSheetNew(Acc_ReportViewModel iObj, int? openingOption)
        {
            List<TrialBalanceViewModel> ItemList = new List<TrialBalanceViewModel>();
            DataTable dt = ReportAccountManger.GetTrialbalanceNew(iObj);

            decimal closingBalance = 0;
            decimal OpeningBalance = 0;
            decimal totalOpeningAsset = 0;
            decimal totalOpeningLiability = 0;
            decimal totalClosingAsset = 0;
            decimal totalClosingLiability = 0;

            foreach (DataRow dr in dt.Rows)
            {
                if (dr["ClsControlCode"].ToString().Substring(0, 1) == "2")
                {
                    closingBalance = (Convert.ToDecimal(dr["ClosingBalance"].ToString()) * -1);
                    OpeningBalance = (Convert.ToDecimal(dr["OpeningBalance"].ToString()) * -1);
                    totalOpeningLiability += OpeningBalance;
                    totalClosingLiability += closingBalance;
                }
                else
                {
                    closingBalance = (Convert.ToDecimal(dr["ClosingBalance"].ToString()));
                    OpeningBalance = (Convert.ToDecimal(dr["OpeningBalance"].ToString()));
                    totalOpeningAsset += OpeningBalance;
                    totalClosingAsset += closingBalance;
                }

                ItemList.Add(new TrialBalanceViewModel()
                {
                    ClasslCode = dr["ClsControlCode"].ToString(),
                    ClasslHead = dr["ClsControlName"].ToString(),
                    GrouplHead = dr["GrpControlName"].ToString(),
                    SubGrouplHead = dr["SubGrpControlName"].ToString(),
                    OpeningBalance = OpeningBalance,
                    ClosingBalance = closingBalance,
                    DateBetween = iObj.StartDate + " to " + iObj.EndDate
                });
            }

            foreach (var t in ItemList)
            {
                if (t.SubGrouplHead == "Reserves and Surplus")
                {
                    t.OpeningBalance = t.OpeningBalance + totalOpeningAsset - totalOpeningLiability;
                    t.ClosingBalance = t.ClosingBalance + totalClosingAsset - totalClosingLiability;
                }
            }

            return ItemList.Where(p => p.ClosingBalance != 0 && p.OpeningBalance != 0).ToList();
        }

        private object GetBalanceSheetPreview(Acc_ReportViewModel iObj, int? openingOption)
        {
            List<TrialBalanceViewModel> ItemList = new List<TrialBalanceViewModel>();
            DataTable dt = ReportAccountManger.GetTrialbalanceNew(iObj);

            decimal closingBalance = 0;
            decimal OpeningBalance = 0;

            decimal totalOpeningAsset = 0;
            decimal totalOpeningLiability = 0;

            decimal totalClosingAsset = 0;
            decimal totalClosingLiability = 0;

            decimal openingNonCurrentAsset = 0;
            decimal closingNonCurrentAsset = 0;

            decimal openingCurrentAsset = 0;
            decimal closingCurrentAsset = 0;

            decimal openingShareHolder = 0;
            decimal closingShareHolder = 0;

            decimal openingNonCurrentLiability = 0;
            decimal closingNonCurrentLiability = 0;

            decimal openingCurrentLiability = 0;
            decimal closingCurrentLiability = 0;

            decimal shareholderOpening = 0;
            decimal shareholderClosing = 0;

            var temp1 = "0";
            var temp2 = "0";
            var tempG1 = "0";
            var tempG2 = "0";

            foreach (DataRow dr in dt.Rows)
            {
                if (dr["ClsControlCode"].ToString().Substring(0, 1) == "2")
                {
                    OpeningBalance = (Convert.ToDecimal(dr["OpeningBalance"].ToString()) * -1);
                    closingBalance = (Convert.ToDecimal(dr["ClosingBalance"].ToString()) * -1);
                    totalOpeningLiability += OpeningBalance;
                    totalClosingLiability += closingBalance;
                }
                else
                {
                    OpeningBalance = (Convert.ToDecimal(dr["OpeningBalance"].ToString()));
                    closingBalance = (Convert.ToDecimal(dr["ClosingBalance"].ToString()));
                    totalOpeningAsset += OpeningBalance;
                    totalClosingAsset += closingBalance;
                }

                if (dr["GrpControlCode"].ToString() == "101")
                {
                    openingNonCurrentAsset += (Convert.ToDecimal(dr["OpeningBalance"].ToString()));
                    closingNonCurrentAsset += (Convert.ToDecimal(dr["ClosingBalance"].ToString()));
                }

                if (dr["GrpControlCode"].ToString() == "102")
                {
                    openingCurrentAsset += (Convert.ToDecimal(dr["OpeningBalance"].ToString()));
                    closingCurrentAsset += (Convert.ToDecimal(dr["ClosingBalance"].ToString()));
                }

                if (dr["GrpControlCode"].ToString() == "201")
                {
                    openingShareHolder += (Convert.ToDecimal(dr["OpeningBalance"].ToString()));
                    closingShareHolder += (Convert.ToDecimal(dr["ClosingBalance"].ToString()));
                }

                if (dr["GrpControlCode"].ToString() == "202")
                {
                    openingNonCurrentLiability += (Convert.ToDecimal(dr["OpeningBalance"].ToString()));
                    closingNonCurrentLiability += (Convert.ToDecimal(dr["ClosingBalance"].ToString()));
                }

                if (dr["GrpControlCode"].ToString() == "203")
                {
                    openingCurrentLiability += (Convert.ToDecimal(dr["OpeningBalance"].ToString()));
                    closingCurrentLiability += (Convert.ToDecimal(dr["ClosingBalance"].ToString()));
                }

                temp2 = dr["ClsControlCode"].ToString();

                if (temp1 == temp2)
                {

                }
                else
                {
                    ItemList.Add(new TrialBalanceViewModel()
                    {
                        ClasslCode = dr["ClsControlCode"].ToString(),
                        ClasslHead = dr["ClsControlName"].ToString(),
                        GrouplHead = "",
                        SubGrouplHead = "",
                        OpeningBalance = OpeningBalance,
                        ClosingBalance = closingBalance,
                        DateBetween = iObj.StartDate + " to " + iObj.EndDate
                    });
                    temp1 = temp2;
                }

                tempG2 = dr["GrpControlCode"].ToString();

                if (tempG1 == tempG2)
                {

                }
                else
                {
                    ItemList.Add(new TrialBalanceViewModel()
                    {
                        ClasslCode = "",
                        ClasslHead = "",
                        GrouplHead = dr["GrpControlName"].ToString(),
                        SubGrouplHead = "",
                        OpeningBalance = OpeningBalance,
                        ClosingBalance = closingBalance,
                        DateBetween = iObj.StartDate + " to " + iObj.EndDate
                    });
                    tempG1 = tempG2;
                }

                ItemList.Add(new TrialBalanceViewModel()
                {
                    ClasslCode = "",
                    ClasslHead = "",
                    GrouplHead = "",
                    SubGroupCode = dr["SubGrpControlCode"].ToString(),
                    SubGrouplHead = dr["SubGrpControlName"].ToString(),
                    OpeningBalance = OpeningBalance,
                    ClosingBalance = closingBalance,
                    DateBetween = iObj.StartDate + " to " + iObj.EndDate
                });
            }

            foreach (var t in ItemList)
            {
                if (t.SubGrouplHead == "Reserves and Surplus")
                {
                    t.OpeningBalance = t.OpeningBalance + totalOpeningAsset - totalOpeningLiability;
                    t.ClosingBalance = t.ClosingBalance + totalClosingAsset - totalClosingLiability;
                }

                if (t.ClasslHead == "Assets")
                {
                    t.OpeningBalance = totalOpeningAsset;
                    t.ClosingBalance = totalClosingAsset;
                }

                if (t.ClasslHead == "Liabilities & Equity")
                {
                    t.OpeningBalance = totalOpeningAsset;
                    t.ClosingBalance = totalClosingAsset;
                }

                if (t.GrouplHead == "Non Current Assets")
                {
                    t.OpeningBalance = openingNonCurrentAsset;
                    t.ClosingBalance = closingNonCurrentAsset;
                }

                if (t.GrouplHead == "Current Assets")
                {
                    t.OpeningBalance = openingCurrentAsset;
                    t.ClosingBalance = closingCurrentAsset;
                }


                //if (t.GrouplHead == "Share Holders Equity")
                //{
                //    t.OpeningBalance = openingShareHolder + totalOpeningAsset - totalOpeningLiability;
                //    t.ClosingBalance = closingShareHolder + totalClosingAsset - totalClosingLiability;
                //}

                if (t.GrouplHead == "Non Current Liabilities")
                {
                    t.OpeningBalance = openingNonCurrentLiability * -1;
                    t.ClosingBalance = closingNonCurrentLiability * -1;
                }

                if (t.GrouplHead == "Current Liabilities")
                {
                    t.OpeningBalance = openingCurrentLiability * -1;
                    t.ClosingBalance = closingCurrentLiability * -1;
                }

                if (t.SubGroupCode == "20101" || t.SubGroupCode == "20102")
                {
                    shareholderOpening = shareholderOpening + t.OpeningBalance;
                    shareholderClosing = shareholderClosing + t.ClosingBalance;
                }
            }

            foreach (var t in ItemList)
            {
                if (t.GrouplHead == "Share Holders Equity")
                {
                    t.OpeningBalance = shareholderOpening;
                    t.ClosingBalance = shareholderClosing;
                }
            }

            return ItemList;
        }

        private object GetBalanceSheetCostCentre(Acc_ReportViewModel iObj, int? openingOption)
        {
            List<TrialBalanceViewModel> ItemList = new List<TrialBalanceViewModel>();
            DataTable dt = ReportAccountManger.GetTrialbalanceCostCentre(iObj);

            decimal closingBalance = 0;
            decimal OpeningBalance = 0;

            decimal totalOpeningAsset = 0;
            decimal totalOpeningLiability = 0;

            decimal totalClosingAsset = 0;
            decimal totalClosingLiability = 0;

            decimal openingNonCurrentAsset = 0;
            decimal closingNonCurrentAsset = 0;

            decimal openingCurrentAsset = 0;
            decimal closingCurrentAsset = 0;

            decimal openingShareHolder = 0;
            decimal closingShareHolder = 0;

            decimal openingNonCurrentLiability = 0;
            decimal closingNonCurrentLiability = 0;

            decimal openingCurrentLiability = 0;
            decimal closingCurrentLiability = 0;

            var temp1 = "0";
            var temp2 = "0";
            var tempG1 = "0";
            var tempG2 = "0";

            foreach (DataRow dr in dt.Rows)
            {
                if (Convert.ToDecimal(dr["ClosingBalance"].ToString()) != 0)
                {

                    if (dr["ClsControlCode"].ToString().Substring(0, 1) == "2")
                    {
                        OpeningBalance = (Convert.ToDecimal(dr["OpeningBalance"].ToString()) * -1);
                        closingBalance = (Convert.ToDecimal(dr["ClosingBalance"].ToString()) * -1);
                        totalOpeningLiability += OpeningBalance;
                        totalClosingLiability += closingBalance;
                    }
                    else
                    {
                        OpeningBalance = (Convert.ToDecimal(dr["OpeningBalance"].ToString()));
                        closingBalance = (Convert.ToDecimal(dr["ClosingBalance"].ToString()));
                        totalOpeningAsset += OpeningBalance;
                        totalClosingAsset += closingBalance;
                    }

                    if (dr["GrpControlCode"].ToString() == "101")
                    {
                        openingNonCurrentAsset += (Convert.ToDecimal(dr["OpeningBalance"].ToString()));
                        closingNonCurrentAsset += (Convert.ToDecimal(dr["ClosingBalance"].ToString()));
                    }

                    if (dr["GrpControlCode"].ToString() == "102")
                    {
                        openingCurrentAsset += (Convert.ToDecimal(dr["OpeningBalance"].ToString()));
                        closingCurrentAsset += (Convert.ToDecimal(dr["ClosingBalance"].ToString()));
                    }

                    if (dr["GrpControlCode"].ToString() == "201")
                    {
                        openingShareHolder += (Convert.ToDecimal(dr["OpeningBalance"].ToString()));
                        closingShareHolder += (Convert.ToDecimal(dr["ClosingBalance"].ToString()));
                    }

                    if (dr["GrpControlCode"].ToString() == "202")
                    {
                        openingNonCurrentLiability += (Convert.ToDecimal(dr["OpeningBalance"].ToString()));
                        closingNonCurrentLiability += (Convert.ToDecimal(dr["ClosingBalance"].ToString()));
                    }

                    if (dr["GrpControlCode"].ToString() == "203")
                    {
                        openingCurrentLiability += (Convert.ToDecimal(dr["OpeningBalance"].ToString()));
                        closingCurrentLiability += (Convert.ToDecimal(dr["ClosingBalance"].ToString()));
                    }

                    temp2 = dr["ClsControlCode"].ToString();

                    if (temp1 == temp2)
                    {

                    }
                    else
                    {
                        ItemList.Add(new TrialBalanceViewModel()
                        {
                            ClasslCode = dr["ClsControlCode"].ToString(),
                            ClasslHead = dr["ClsControlName"].ToString(),
                            GrouplHead = "",
                            SubGrouplHead = "",
                            OpeningBalance = OpeningBalance,
                            ClosingBalance = closingBalance,
                            DateBetween = iObj.StartDate + " to " + iObj.EndDate
                        });
                        temp1 = temp2;
                    }

                    tempG2 = dr["GrpControlCode"].ToString();

                    if (tempG1 == tempG2)
                    {

                    }
                    else
                    {
                        ItemList.Add(new TrialBalanceViewModel()
                        {
                            ClasslCode = "",
                            ClasslHead = "",
                            GrouplHead = dr["GrpControlName"].ToString(),
                            SubGrouplHead = "",
                            OpeningBalance = OpeningBalance,
                            ClosingBalance = closingBalance,
                            DateBetween = iObj.StartDate + " to " + iObj.EndDate
                        });
                        tempG1 = tempG2;
                    }

                    ItemList.Add(new TrialBalanceViewModel()
                    {
                        ClasslCode = "",
                        ClasslHead = "",
                        GrouplHead = "",
                        SubGroupCode = dr["SubGrpControlCode"].ToString(),
                        SubGrouplHead = dr["SubGrpControlName"].ToString(),
                        OpeningBalance = OpeningBalance,
                        ClosingBalance = closingBalance,
                        DateBetween = iObj.StartDate + " to " + iObj.EndDate
                    });
                }
            }
            foreach (var t in ItemList)
            {
                if (t.SubGrouplHead == "Reserves & Surplus")
                {
                    t.OpeningBalance = totalOpeningAsset - totalOpeningLiability;
                    t.ClosingBalance = totalClosingAsset - totalClosingLiability;
                }

                if (t.ClasslHead == "Assets")
                {
                    t.OpeningBalance = totalOpeningAsset;
                    t.ClosingBalance = totalClosingAsset;
                }

                if (t.ClasslHead == "Liabilities & Equity")
                {
                    t.OpeningBalance = totalOpeningAsset;
                    t.ClosingBalance = totalClosingAsset;
                }

                if (t.GrouplHead == "Non Current Assets")
                {
                    t.OpeningBalance = openingNonCurrentAsset;
                    t.ClosingBalance = closingNonCurrentAsset;
                }

                if (t.GrouplHead == "Current Assets")
                {
                    t.OpeningBalance = openingCurrentAsset;
                    t.ClosingBalance = closingCurrentAsset;
                }

                if (t.GrouplHead == "Share Holders Equity")
                {
                    t.OpeningBalance = openingShareHolder + totalOpeningAsset - totalOpeningLiability;
                    t.ClosingBalance = closingShareHolder + totalClosingAsset - totalClosingLiability;
                }

                if (t.GrouplHead == "Non Current Liabilities")
                {
                    t.OpeningBalance = openingNonCurrentLiability * -1;
                    t.ClosingBalance = closingNonCurrentLiability * -1;
                }

                if (t.GrouplHead == "Current Liabilities")
                {
                    t.OpeningBalance = openingCurrentLiability * -1;
                    t.ClosingBalance = closingCurrentLiability * -1;
                }
            }

            return ItemList;
        }

        private object GetTrialBalanceData(Acc_ReportViewModel iObj, int? openingOption)
        {
            iObj.OpStartDate = "01/01/2013";
            iObj.OpEndDate = iObj.StartDate;
            List<TrialBalanceViewModel> ItemList = new List<TrialBalanceViewModel>();
            DataTable dt = ReportAccountManger.GetTrialBalance(iObj);

            decimal dlTotalDebit = 0;
            decimal dlTotalCredit = 0;

            decimal dlTempDebit = 0;
            decimal dlTempCredit = 0;
            foreach (DataRow dr in dt.Rows)
            {
                dlTempDebit = 0;
                dlTempCredit = 0;
                if (openingOption == 1)
                {
                    dlTempDebit = Convert.ToDecimal(dr["Debit"].ToString());
                    dlTempCredit = Convert.ToDecimal(dr["Credit"].ToString());
                }
                else
                {
                    if (dr["ClsControlCode"].ToString().Substring(0, 1) == "1" ||
                        dr["ClsControlCode"].ToString().Substring(0, 1) == "4")
                    {
                        if (Convert.ToDecimal(dr["ClosingBalance"].ToString()) < 0)
                            dlTempCredit = (Convert.ToDecimal(dr["ClosingBalance"].ToString()) * -1);
                        else
                            dlTempDebit = Convert.ToDecimal(dr["ClosingBalance"].ToString());
                    }
                    else
                    {
                        if (Convert.ToDecimal(dr["ClosingBalance"].ToString()) < 0)
                            dlTempDebit = (Convert.ToDecimal(dr["ClosingBalance"].ToString()) * -1);
                        else
                            dlTempCredit = Convert.ToDecimal(dr["ClosingBalance"].ToString());
                    }
                }
                if (iObj.TrialBalanceLebel == 1)
                {
                    ItemList.Add(new TrialBalanceViewModel()
                    {
                        ClasslCode = dr["ClsControlCode"].ToString(),
                        ClasslHead = dr["ClsControlName"].ToString(),
                        OpeningBalance = Convert.ToDecimal(dr["OpeningBalance"].ToString()),
                        TotalDebit = dlTempDebit,
                        TotalCredit = dlTempCredit,
                        ClosingBalance = Convert.ToDecimal(dr["ClosingBalance"].ToString()),
                    });
                }
                else if (iObj.TrialBalanceLebel == 2)
                {
                    ItemList.Add(new TrialBalanceViewModel()
                    {
                        ClasslCode = dr["ClsControlCode"].ToString(),
                        ClasslHead = dr["ClsControlName"].ToString(),
                        GrouplHead = dr["GrpControlName"].ToString(),
                        OpeningBalance = Convert.ToDecimal(dr["OpeningBalance"].ToString()),
                        TotalDebit = dlTempDebit,
                        TotalCredit = dlTempCredit,
                        ClosingBalance = Convert.ToDecimal(dr["ClosingBalance"].ToString()),
                    });
                }
                else if (iObj.TrialBalanceLebel == 3)
                {
                    ItemList.Add(new TrialBalanceViewModel()
                    {
                        ClasslCode = dr["ClsControlCode"].ToString(),
                        ClasslHead = dr["ClsControlName"].ToString(),
                        GrouplHead = dr["GrpControlName"].ToString(),
                        SubGrouplHead = dr["SubGrpControlName"].ToString(),

                        OpeningBalance = Convert.ToDecimal(dr["OpeningBalance"].ToString()),
                        TotalDebit = dlTempDebit,
                        TotalCredit = dlTempCredit,
                        ClosingBalance = Convert.ToDecimal(dr["ClosingBalance"].ToString()),
                    });
                }
                else if (iObj.TrialBalanceLebel == 4)
                {
                    ItemList.Add(new TrialBalanceViewModel()
                    {
                        ClasslCode = dr["ClsControlCode"].ToString(),
                        ClasslHead = dr["ClsControlName"].ToString(),
                        GrouplHead = dr["GrpControlName"].ToString(),
                        SubGrouplHead = dr["SubGrpControlName"].ToString(),
                        ControlHead = dr["ControlCodeName"].ToString(),

                        OpeningBalance = Convert.ToDecimal(dr["OpeningBalance"].ToString()),
                        TotalDebit = dlTempDebit,
                        TotalCredit = dlTempCredit,
                        ClosingBalance = Convert.ToDecimal(dr["ClosingBalance"].ToString()),
                    });
                }
                else if (iObj.TrialBalanceLebel == 5)
                {
                    ItemList.Add(new TrialBalanceViewModel()
                    {
                        ClasslCode = dr["ClsControlCode"].ToString(),
                        ClasslHead = dr["ClsControlName"].ToString(),
                        GrouplHead = dr["GrpControlName"].ToString(),
                        SubGrouplHead = dr["SubGrpControlName"].ToString(),
                        ControlHead = dr["ControlCodeName"].ToString(),
                        GlHead = dr["AccountName"].ToString(),

                        OpeningBalance = Convert.ToDecimal(dr["OpeningBalance"].ToString()),
                        TotalDebit = dlTempDebit,
                        TotalCredit = dlTempCredit,
                        ClosingBalance = Convert.ToDecimal(dr["ClosingBalance"].ToString()),
                    });
                }
                else if (iObj.TrialBalanceLebel == 6)
                {
                    ItemList.Add(new TrialBalanceViewModel()
                    {
                        ClasslCode = dr["ClsControlCode"].ToString(),
                        ClasslHead = dr["ClsControlName"].ToString(),
                        GrouplHead = dr["GrpControlName"].ToString(),

                        ControlHead = dr["ControlCodeName"].ToString(),


                        OpeningBalance = Convert.ToDecimal(dr["OpeningBalance"].ToString()),
                        TotalDebit = dlTempDebit,
                        TotalCredit = dlTempCredit,
                        ClosingBalance = Convert.ToDecimal(dr["ClosingBalance"].ToString()),
                    });
                }
                dlTotalDebit += dlTempDebit;
                dlTotalCredit += dlTempCredit;
            }
            ItemList.Add(new TrialBalanceViewModel()
            {
                Particulars = "Total :",
                TotalDebit = dlTotalDebit,
                TotalCredit = dlTotalCredit,
            });

            return ItemList;
        }

        private object GetTrialBalanceIncomeStatementWithSubGroupCode(Acc_ReportViewModel iObj, int? openingOption)
        {
            iObj.OpStartDate = "01/01/2013";
            iObj.OpEndDate = iObj.StartDate;
            List<TrialBalanceViewModel> ItemList = new List<TrialBalanceViewModel>();
            DataTable dt = ReportAccountManger.GetTrialBalance(iObj);

            decimal dlTotalDebit = 0;
            decimal dlTotalCredit = 0;

            decimal dlTempDebit = 0;
            decimal dlTempCredit = 0;
            foreach (DataRow dr in dt.Rows)
            {
                dlTempDebit = 0;
                dlTempCredit = 0;
                if (openingOption == 1)
                {
                    dlTempDebit = Convert.ToDecimal(dr["Debit"].ToString());
                    dlTempCredit = Convert.ToDecimal(dr["Credit"].ToString());
                }
                else
                {

                    if (dr["ClsControlCode"].ToString().Substring(0, 1) == "3")
                    {


                        dlTempCredit = Convert.ToDecimal(dr["Credit"].ToString()) - Convert.ToDecimal(dr["Debit"].ToString());

                    }
                    else if (dr["ClsControlCode"].ToString().Substring(0, 1) == "4")
                    {

                        dlTempCredit = Convert.ToDecimal(dr["Debit"].ToString()) - Convert.ToDecimal(dr["Credit"].ToString());
                    }
                }
                if (iObj.TrialBalanceLebel == 1)
                {
                    ItemList.Add(new TrialBalanceViewModel()
                    {
                        ClasslCode = dr["ClsControlCode"].ToString(),
                        ClasslHead = dr["ClsControlName"].ToString(),
                        OpeningBalance = Convert.ToDecimal(dr["OpeningBalance"].ToString()),
                        TotalDebit = dlTempDebit,
                        TotalCredit = dlTempCredit,
                        ClosingBalance = Convert.ToDecimal(dr["ClosingBalance"].ToString()),
                    });
                }
                else if (iObj.TrialBalanceLebel == 2)
                {
                    ItemList.Add(new TrialBalanceViewModel()
                    {
                        ClasslCode = dr["ClsControlCode"].ToString(),
                        ClasslHead = dr["ClsControlName"].ToString(),
                        GrouplHead = dr["GrpControlName"].ToString(),
                        OpeningBalance = Convert.ToDecimal(dr["OpeningBalance"].ToString()),
                        TotalDebit = dlTempDebit,
                        TotalCredit = dlTempCredit,
                        ClosingBalance = Convert.ToDecimal(dr["ClosingBalance"].ToString()),
                    });
                }
                else if (iObj.TrialBalanceLebel == 3)
                {
                    ItemList.Add(new TrialBalanceViewModel()
                    {
                        ClasslCode = dr["ClsControlCode"].ToString(),
                        ClasslHead = dr["ClsControlName"].ToString(),
                        GrouplHead = dr["GrpControlName"].ToString(),
                        SubGrouplHead = dr["SubGrpControlName"].ToString(),

                        OpeningBalance = Convert.ToDecimal(dr["OpeningBalance"].ToString()),
                        TotalDebit = dlTempDebit,
                        TotalCredit = dlTempCredit,
                        ClosingBalance = Convert.ToDecimal(dr["ClosingBalance"].ToString()),
                    });
                }
                else if (iObj.TrialBalanceLebel == 4)
                {
                    ItemList.Add(new TrialBalanceViewModel()
                    {
                        ClasslCode = dr["ClsControlCode"].ToString(),
                        ClasslHead = dr["ClsControlName"].ToString(),
                        GrouplHead = dr["GrpControlName"].ToString(),
                        SubGrouplHead = dr["SubGrpControlName"].ToString(),
                        ControlHead = dr["ControlCodeName"].ToString(),

                        OpeningBalance = Convert.ToDecimal(dr["OpeningBalance"].ToString()),
                        TotalDebit = dlTempDebit,
                        TotalCredit = dlTempCredit,
                        ClosingBalance = Convert.ToDecimal(dr["ClosingBalance"].ToString()),
                    });
                }
                else if (iObj.TrialBalanceLebel == 5)
                {
                    ItemList.Add(new TrialBalanceViewModel()
                    {
                        ClasslCode = dr["ClsControlCode"].ToString(),
                        ClasslHead = dr["ClsControlName"].ToString(),
                        GrouplHead = dr["GrpControlName"].ToString(),
                        SubGrouplHead = dr["SubGrpControlName"].ToString(),
                        SubGroupCode = dr["SubGrpControlCode"].ToString(),
                        ControlHead = dr["ControlCodeName"].ToString(),
                        GlHead = dr["AccountName"].ToString(),

                        OpeningBalance = Convert.ToDecimal(dr["OpeningBalance"].ToString()),
                        TotalDebit = dlTempDebit,
                        TotalCredit = dlTempCredit,
                        ClosingBalance = Convert.ToDecimal(dr["ClosingBalance"].ToString()),
                    });
                }
                else if (iObj.TrialBalanceLebel == 6)
                {
                    ItemList.Add(new TrialBalanceViewModel()
                    {
                        ClasslCode = dr["ClsControlCode"].ToString(),
                        ClasslHead = dr["ClsControlName"].ToString(),
                        GrouplHead = dr["GrpControlName"].ToString(),

                        ControlHead = dr["ControlCodeName"].ToString(),


                        OpeningBalance = Convert.ToDecimal(dr["OpeningBalance"].ToString()),
                        TotalDebit = dlTempDebit,
                        TotalCredit = dlTempCredit,
                        ClosingBalance = Convert.ToDecimal(dr["ClosingBalance"].ToString()),
                    });
                }
                dlTotalDebit += dlTempDebit;
                dlTotalCredit += dlTempCredit;
            }
            ItemList.Add(new TrialBalanceViewModel()
            {
                Particulars = "Total :",
                TotalDebit = dlTotalDebit,
                TotalCredit = dlTotalCredit,
            });

            return ItemList;
        }

        private object GetTrialBalanceDataIncomeStatement(Acc_ReportViewModel iObj, int? openingOption)
        {
            iObj.OpStartDate = iObj.StartDate;
            iObj.OpEndDate = iObj.EndDate;
            List<TrialBalanceViewModel> ItemList = new List<TrialBalanceViewModel>();
            DataTable dt = ReportAccountManger.GetTrialBalance(iObj);

            decimal TotalDebit = 0;
            decimal TotalCredit = 0;
            decimal TempDebit = 0;
            decimal TempCredit = 0;
            decimal ClosingBalance = 0;

            foreach (DataRow dr in dt.Rows)
            {
                if (openingOption == 1)
                {
                    TempDebit = Convert.ToDecimal(dr["Debit"].ToString());
                    TempCredit = Convert.ToDecimal(dr["Credit"].ToString());
                }
                else
                {
                    if (dr["ClsControlCode"].ToString().Substring(0, 1) == "3")
                    {
                        ClosingBalance = Convert.ToDecimal(dr["Credit"].ToString()) - Convert.ToDecimal(dr["Debit"].ToString());
                    }
                    else if (dr["ClsControlCode"].ToString().Substring(0, 1) == "4")
                    {
                        ClosingBalance = Convert.ToDecimal(dr["Debit"].ToString()) - Convert.ToDecimal(dr["Credit"].ToString());
                    }
                }

                ItemList.Add(new TrialBalanceViewModel()
                {
                    ClasslCode = dr["ClsControlCode"].ToString(),
                    ClasslHead = dr["ClsControlName"].ToString(),
                    GrouplHead = dr["GrpControlName"].ToString(),
                    //ControlHead = dr["ControlCodeName"].ToString(),
                    ControlHead = dr["AccountName"].ToString(),

                    OpeningBalance = 0,
                    TotalDebit = TempDebit,
                    TotalCredit = TempCredit,
                    ClosingBalance = Convert.ToDecimal(ClosingBalance)
                });

                TotalDebit += TempDebit;
                TotalCredit += TempCredit;
            }
            ItemList.Add(new TrialBalanceViewModel()
            {
                Particulars = "Total :",
                TotalDebit = TotalDebit,
                TotalCredit = TotalCredit,
            });

            return ItemList;
        }

        private object GetTrialBalanceControlData(Acc_ReportViewModel iObj, int? openingOption, string ControlCode)
        {
            iObj.OpStartDate = "01/01/2013";
            iObj.OpEndDate = iObj.StartDate;
            List<TrialBalanceViewModel> ItemList = new List<TrialBalanceViewModel>();
            DataTable dt = ReportAccountManger.GetTrialBalance(iObj);

            decimal dlTotalDebit = 0;
            decimal dlTotalCredit = 0;

            decimal dlTempDebit = 0;
            decimal dlTempCredit = 0;
            foreach (DataRow dr in dt.Rows)
            {
                dlTempDebit = 0;
                dlTempCredit = 0;
                if (openingOption == 1)
                {
                    dlTempDebit = Convert.ToDecimal(dr["Debit"].ToString());
                    dlTempCredit = Convert.ToDecimal(dr["Credit"].ToString());
                }
                else
                {
                    if (dr["ClsControlCode"].ToString().Substring(0, 1) == "1" ||
                        dr["ClsControlCode"].ToString().Substring(0, 1) == "4")
                    {
                        if (Convert.ToDecimal(dr["ClosingBalance"].ToString()) < 0)
                            dlTempCredit = (Convert.ToDecimal(dr["ClosingBalance"].ToString()) * -1);
                        else
                            dlTempDebit = Convert.ToDecimal(dr["ClosingBalance"].ToString());
                    }
                    else
                    {
                        if (Convert.ToDecimal(dr["ClosingBalance"].ToString()) < 0)
                            dlTempDebit = (Convert.ToDecimal(dr["ClosingBalance"].ToString()) * -1);
                        else
                            dlTempCredit = Convert.ToDecimal(dr["ClosingBalance"].ToString());
                    }
                }
                if (iObj.TrialBalanceLebel == 1)
                {
                    ItemList.Add(new TrialBalanceViewModel()
                    {
                        ClasslCode = dr["ClsControlCode"].ToString(),
                        ClasslHead = dr["ClsControlName"].ToString(),
                        OpeningBalance = Convert.ToDecimal(dr["OpeningBalance"].ToString()),
                        TotalDebit = dlTempDebit,
                        TotalCredit = dlTempCredit,
                        ClosingBalance = Convert.ToDecimal(dr["ClosingBalance"].ToString()),
                    });
                }
                else if (iObj.TrialBalanceLebel == 2)
                {
                    ItemList.Add(new TrialBalanceViewModel()
                    {
                        ClasslCode = dr["ClsControlCode"].ToString(),
                        ClasslHead = dr["ClsControlName"].ToString(),
                        GrouplHead = dr["GrpControlName"].ToString(),
                        OpeningBalance = Convert.ToDecimal(dr["OpeningBalance"].ToString()),
                        TotalDebit = dlTempDebit,
                        TotalCredit = dlTempCredit,
                        ClosingBalance = Convert.ToDecimal(dr["ClosingBalance"].ToString()),
                    });
                }
                else if (iObj.TrialBalanceLebel == 3)
                {
                    ItemList.Add(new TrialBalanceViewModel()
                    {
                        ClasslCode = dr["ClsControlCode"].ToString(),
                        ClasslHead = dr["ClsControlName"].ToString(),
                        GrouplHead = dr["GrpControlName"].ToString(),
                        SubGrouplHead = dr["SubGrpControlName"].ToString(),

                        OpeningBalance = Convert.ToDecimal(dr["OpeningBalance"].ToString()),
                        TotalDebit = dlTempDebit,
                        TotalCredit = dlTempCredit,
                        ClosingBalance = Convert.ToDecimal(dr["ClosingBalance"].ToString()),
                    });
                }
                else if (iObj.TrialBalanceLebel == 4)
                {
                    ItemList.Add(new TrialBalanceViewModel()
                    {
                        ClasslCode = dr["ClsControlCode"].ToString(),
                        ClasslHead = dr["ClsControlName"].ToString(),
                        GrouplHead = dr["GrpControlName"].ToString(),
                        SubGrouplHead = dr["SubGrpControlName"].ToString(),
                        ControlHead = dr["ControlCodeName"].ToString(),

                        OpeningBalance = Convert.ToDecimal(dr["OpeningBalance"].ToString()),
                        TotalDebit = dlTempDebit,
                        TotalCredit = dlTempCredit,
                        ClosingBalance = Convert.ToDecimal(dr["ClosingBalance"].ToString()),
                    });
                }
                else if (iObj.TrialBalanceLebel == 5 && dr["ControlContolCode"].ToString() == ControlCode)
                {
                    ItemList.Add(new TrialBalanceViewModel()
                    {
                        ClasslCode = dr["ClsControlCode"].ToString(),
                        ClasslHead = dr["ClsControlName"].ToString(),
                        GrouplHead = dr["GrpControlName"].ToString(),
                        SubGrouplHead = dr["SubGrpControlName"].ToString(),
                        ControlHead = dr["ControlCodeName"].ToString(),
                        GlHead = dr["AccountName"].ToString(),

                        OpeningBalance = Convert.ToDecimal(dr["OpeningBalance"].ToString()),
                        TotalDebit = Convert.ToDecimal(dr["Debit"].ToString()),
                        TotalCredit = Convert.ToDecimal(dr["Credit"].ToString()),
                        ClosingBalance = Convert.ToDecimal(dr["ClosingBalance"].ToString()),
                    });
                }
                else if (iObj.TrialBalanceLebel == 6)
                {
                    ItemList.Add(new TrialBalanceViewModel()
                    {
                        ClasslCode = dr["ClsControlCode"].ToString(),
                        ClasslHead = dr["ClsControlName"].ToString(),
                        GrouplHead = dr["GrpControlName"].ToString(),

                        ControlHead = dr["ControlCodeName"].ToString(),


                        OpeningBalance = Convert.ToDecimal(dr["OpeningBalance"].ToString()),
                        TotalDebit = dlTempDebit,
                        TotalCredit = dlTempCredit,
                        ClosingBalance = Convert.ToDecimal(dr["ClosingBalance"].ToString()),
                    });
                }
                dlTotalDebit += dlTempDebit;
                dlTotalCredit += dlTempCredit;
            }
            //ItemList.Add(new TrialBalanceViewModel()
            //{
            //    Particulars = "Total :",
            //    TotalDebit = dlTotalDebit,
            //    TotalCredit = dlTotalCredit,
            //});

            return ItemList;
        }

        public ActionResult PrintTrialBalance(string FpId, string SectorId, string SectorText, string fromDate, string toDate, int openingOption = 1, int TrialBalanceLebel = 1, string ExportType = "")
        {
            var iObj = new Acc_ReportViewModel();
            iObj.SectorCode = SectorId;
            iObj.TrialBalanceLebel = TrialBalanceLebel;
            iObj.StartDate = fromDate;
            iObj.EndDate = toDate;
            List<TrialBalanceViewModel> ItemList = (List<TrialBalanceViewModel>)GetTrialBalanceData(iObj, openingOption);

            ReportClass rptH = new ReportClass();
            ArrayList iArrayRpt = new ArrayList();
            var reportTitle = "";
            if (TrialBalanceLebel == 2)
            {
                reportTitle = "Group Trial Balance";
                rptH.FileName = Server.MapPath("/Reports/Accounting/TrialBalance_Group.rpt");
            }
            if (TrialBalanceLebel == 3)
            {
                reportTitle = "Sub Group Trial Balance";
                rptH.FileName = Server.MapPath("/Reports/Accounting/TrialBalance_SubGroup.rpt");
            }
            if (TrialBalanceLebel == 4)
            {
                rptH.FileName = Server.MapPath("/Reports/Accounting/TrialBalance_Control.rpt");
                reportTitle = "Control Trial Balance";
            }
            if (TrialBalanceLebel == 5)
            {
                rptH.FileName = Server.MapPath("/Reports/Accounting/TrialBalance_GL.rpt");
                reportTitle = "GL Trial Balance";
            }
            rptH.Load();
            TrialBalanceViewModel obj;
            int iCount = 1;

            Company company = CompanyManager.GetCompanyInfo();
            if (SectorText == "- Select -")
                SectorText = "Consulate";
            foreach (TrialBalanceViewModel dr in ItemList)
            {
                if (ItemList.Count == iCount) break;
                iCount += 1;
                obj = new TrialBalanceViewModel();
                obj.CompanyName = company.Name;
                obj.SectorName = SectorText; // "Consulate";
                obj.ReportTitle = reportTitle;
                obj.DateBetween = fromDate + " to " + toDate;
                obj.OpeningOption = openingOption;
                obj.ClasslHead = dr.ClasslHead;
                if (TrialBalanceLebel >= 2)
                    obj.GrouplHead = dr.GrouplHead;
                if (TrialBalanceLebel >= 3)
                    obj.SubGrouplHead = dr.SubGrouplHead;
                if (TrialBalanceLebel >= 4)
                    obj.ControlHead = dr.ControlHead;
                if (TrialBalanceLebel >= 5)
                    obj.GlHead = dr.GlHead;

                obj.TotalDebit = dr.TotalDebit;
                obj.TotalCredit = dr.TotalCredit;
                obj.OpeningBalance = dr.OpeningBalance;
                obj.ClosingBalance = dr.ClosingBalance;
                iArrayRpt.Add(obj);
            }

            if (iArrayRpt.Count == 0)
            {
                obj = new TrialBalanceViewModel();
                obj.CompanyName = company.Name;
                obj.SectorName = SectorText;

                if (TrialBalanceLebel == 2)
                    obj.ReportTitle = "Group Trial Balance";
                else if (TrialBalanceLebel == 3)
                    obj.ReportTitle = "Sub Group Trial Balance";
                else if (TrialBalanceLebel == 4)
                    obj.ReportTitle = "Control Trial Balance";
                else if (TrialBalanceLebel == 5)
                    obj.ReportTitle = "GL Trial Balance";

                obj.DateBetween = fromDate + " to " + toDate;
                iArrayRpt.Add(obj);
            }

            rptH.SetDataSource(iArrayRpt);

            if (ExportType == "Excel")
            {
                Stream stream = rptH.ExportToStream(ExportFormatType.Excel);
                rptH.Dispose();
                return File(stream, "application/vnd.ms-excel", "GeneralLedger.xls");
            }

            else
            {
                Stream stream = rptH.ExportToStream(ExportFormatType.PortableDocFormat);
                rptH.Dispose();
                return File(stream, "application/pdf");
            }
        }

        #endregion

        //***********************************************************************************************************************************************

        #region BalanceSheet

        public ActionResult BalanceSheet(int? page, string sort, TrialBalanceViewModel model, int? fpId, int? sectorId, string fromDate, string toDate)
        {
            var companyId = voucherMasterManager.GetCostCentreByEmployeeId(_employeeGuidId);
            model.SectorId = (int)companyId;

            var activeFinancialPeriod = ReportAccountManger.GetActiveFinancialPeriod(); 

            ViewBag.FpId = new SelectList(OpeningBalaceManager.GetFinancialPeriod().AsEnumerable(), "Id", "PeriodName", activeFinancialPeriod.Id);                
            ViewBag.SectorId = new SelectList(BankVoucherEntryManager.GetAllCompanySector().AsEnumerable(), "Id", "SectorName", companyId);
            ViewBag.CostCentreId = new SelectList(new List<Acc_CostCentre>(), "Id", "CostCentreId");

            ViewBag.FromDate = activeFinancialPeriod.PeriodStartDate.GetValueOrDefault().AddDays(-1).ToString("dd/MM/yyy"); 
            ViewBag.ToDate = activeFinancialPeriod.PeriodEndDate.GetValueOrDefault().ToString("dd/MM/yyy"); 

            var printFormat = from PrintFormatType formatType in Enum.GetValues(typeof(PrintFormatType))
                              select new { Id = (int)formatType, Name = formatType.ToString() };

            ViewBag.PrintFormatId = new SelectList(printFormat.AsEnumerable(), "Id", "Name", companyId);

            var itemListBalanceSheet = new List<TrialBalanceViewModel>();

            if (fpId == null)
            {
                model.TrialBalance = itemListBalanceSheet;
                return View(model);
            }

            var ItemList = new List<TrialBalanceViewModel>();
            var iObj = new Acc_ReportViewModel();
            iObj.SectorCode = model.SectorId.ToString();
            iObj.TrialBalanceLebel = 5;
            iObj.StartDate = model.FromDate;
            iObj.EndDate = model.ToDate;
            iObj.GLId = "1,2";
            ItemList = (List<TrialBalanceViewModel>)GetBalanceSheetPreview(iObj, 5);
            ItemList.RemoveAll(x => x.SubGrouplHead == "Retained Earnings");
            model.TrialBalance = ItemList;

            return View(model);
        }

        public ActionResult PrintBalanceSheet(int? page, string sort, GeneralLedgerViewModel model, int? fpId, int? sectorId, string GlId, string fromDate, string toDate, int? CostCentreId, string ExportType)
        {
            List<TrialBalanceViewModel> ItemList = new List<TrialBalanceViewModel>();
            var iObj = new Acc_ReportViewModel();
            iObj.SectorCode = sectorId.ToString();
            iObj.TrialBalanceLebel = 3;
            iObj.StartDate = fromDate;
            iObj.EndDate = toDate;
            iObj.GLId = "1,2";
            ItemList = (List<TrialBalanceViewModel>)GetBalanceSheetNew(iObj, 5);

            LocalReport lr = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/Areas/Accounting/Reports"), "BalanceSheet.rdlc");
            if (System.IO.File.Exists(path))
                lr.ReportPath = path;
            else
                return View("Index");

            string companyName = CompanySectorManager.GetCompanySectorById(sectorId).SectorName;
            string companyAddress = CompanySectorManager.GetCompanySectorById(sectorId).SectorAddress;

            ReportParameter[] parameters = new ReportParameter[2];
            parameters[0] = new ReportParameter("param_companyName", companyName);
            parameters[1] = new ReportParameter("param_companyAddress", companyAddress);

            ReportDataSource rd = new ReportDataSource("BalanceDS", ItemList);

            lr.SetParameters(parameters);
            lr.DataSources.Add(rd);

            string reportType = ExportType;
            string mimeType;
            string encoding;
            string fileNameExtension;

            string deviceInfo =
                "<DeviceInfo>" +
                "  <OutputFormat>" + ExportType + "</OutputFormat>" +
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

        public ActionResult BalanceSheetDetail(string subGroupCode, int? sectorId, string fromDate, string toDate)
        {
            List<TrialBalanceViewModel> ItemList;
            var model = new TrialBalanceViewModel();

            var iObj = new Acc_ReportViewModel();
            iObj.SectorCode = sectorId.ToString();
            iObj.TrialBalanceLebel = 5;
            iObj.StartDate = fromDate;
            iObj.EndDate = toDate;
            ItemList = (List<TrialBalanceViewModel>)GetBalanceSheetDetailData(iObj, 2, subGroupCode);

            model.TrialBalance = ItemList;
            return View(model);
        }

        public ActionResult BalanceSheetToGeneralLedger(string glId, string fromDate, string toDate, string costCentreId)
        {
            GeneralLedgerViewModel model = new GeneralLedgerViewModel();

            var iObj = new Acc_ReportViewModel();
            iObj.SectorCode = costCentreId;
            iObj.GLId = glId;
            iObj.StartDate = fromDate;
            iObj.EndDate = toDate;
            iObj.currencyId = 1;
            List<GeneralLedgerViewModel> item = (List<GeneralLedgerViewModel>)GetGeneralLedgerData(iObj);
            model.GeneralLedger = item;
            return View(model);
        }

        private object GetBalanceSheetDetailData(Acc_ReportViewModel iObj, int? openingOption, string subGroupCode)
        {
            iObj.OpStartDate = "01/01/2013";
            iObj.OpEndDate = iObj.StartDate;
            var itemList = new List<TrialBalanceViewModel>();
            DataTable dt = ReportAccountManger.GetTrialBalance(iObj);

            decimal dlTotalDebit = 0;
            decimal dlTotalCredit = 0;

            decimal dlTempDebit = 0;
            decimal dlTempCredit = 0;


            var temp1 = "0";
            var temp2 = "0";

            var tempG1 = "0";
            var tempG2 = "0";


            foreach (DataRow dr in dt.Rows)
            {
                dlTempDebit = 0;
                dlTempCredit = 0;
                if (openingOption == 1)
                {
                    dlTempDebit = Convert.ToDecimal(dr["Debit"].ToString());
                    dlTempCredit = Convert.ToDecimal(dr["Credit"].ToString());
                }
                else
                {
                    if (dr["ClsControlCode"].ToString().Substring(0, 1) == "1" || dr["ClsControlCode"].ToString().Substring(0, 1) == "4")
                    {
                        if (Convert.ToDecimal(dr["ClosingBalance"].ToString()) < 0)
                            dlTempCredit = (Convert.ToDecimal(dr["ClosingBalance"].ToString()) * -1);
                        else
                            dlTempDebit = Convert.ToDecimal(dr["ClosingBalance"].ToString());
                    }
                    else
                    {
                        if (Convert.ToDecimal(dr["ClosingBalance"].ToString()) < 0)
                            dlTempDebit = (Convert.ToDecimal(dr["ClosingBalance"].ToString()) * -1);
                        else
                            dlTempCredit = Convert.ToDecimal(dr["ClosingBalance"].ToString());
                    }
                }

                if (iObj.TrialBalanceLebel == 5 && dr["SubGrpControlCode"].ToString() == subGroupCode)
                {

                    temp2 = dr["SubGrpControlCode"].ToString();

                    //if (temp1 == temp2)
                    //{

                    //}
                    //else
                    //{
                    //    itemList.Add(new TrialBalanceViewModel()
                    //    {
                    //        ClasslCode = "",
                    //        ClasslHead = "",
                    //        GrouplHead = "",
                    //        SubGrouplHead = dr["SubGrpControlName"].ToString(),
                    //        ControlHead = "",
                    //        GlHead = "",

                    //        OpeningBalance = Convert.ToDecimal(dr["OpeningBalance"].ToString()),
                    //        TotalDebit = dlTempDebit,
                    //        TotalCredit = dlTempCredit,
                    //        ClosingBalance = 0,
                    //    });
                    //    temp1 = temp2;
                    //}

                    tempG2 = dr["ControlContolCode"].ToString();

                    if (tempG1 == tempG2)
                    {

                    }
                    else
                    {
                        itemList.Add(new TrialBalanceViewModel()
                        {
                            ClasslCode = "",
                            ClasslHead = "",
                            GrouplHead = "",
                            SubGrouplHead = "",
                            ControlHead = dr["ControlCodeName"].ToString(),
                            GlHead = "",

                            OpeningBalance = Convert.ToDecimal(dr["OpeningBalance"].ToString()),
                            TotalDebit = dlTempDebit,
                            TotalCredit = dlTempCredit,
                            ClosingBalance = 0,
                        });
                        tempG1 = tempG2;
                    }

                    itemList.Add(new TrialBalanceViewModel()
                    {
                        ClasslCode = "",
                        ClasslHead = "",
                        GrouplHead = "",
                        SubGrouplHead = "",
                        ControlHead = "",
                        GlHead = dr["AccountName"].ToString(),
                        AccountCode = dr["AccountCode"].ToString(),
                        OpeningBalance = Convert.ToDecimal(dr["OpeningBalance"].ToString()),
                        TotalDebit = dlTempDebit,
                        TotalCredit = dlTempCredit,
                        SectorId = Convert.ToInt32(iObj.SectorCode),
                        FromDate = iObj.StartDate,
                        ToDate = iObj.EndDate,
                        ClosingBalance = Convert.ToDecimal(dr["ClosingBalance"].ToString())
                    });
                }

                dlTotalDebit += dlTempDebit;
                dlTotalCredit += dlTempCredit;

            }

            itemList.Add(new TrialBalanceViewModel()
            {
                Particulars = "Total :",
                TotalDebit = dlTotalDebit,
                TotalCredit = dlTotalCredit,
            });

            return itemList;
        }



        #endregion

        //***********************************************************************************************************************************************

        #region CostCentre

        public ActionResult CostCentre(int? page, string sort, TrialBalanceViewModel model, int? fpId, int? sectorId, string costId, string fromDate, string toDate)
        {
            var companyId = voucherMasterManager.GetCostCentreByEmployeeId(_employeeGuidId);
            model.SectorId = (int)companyId;

            var activeFinancialPeriod = ReportAccountManger.GetActiveFinancialPeriod();

            ViewBag.FpId = new SelectList(OpeningBalaceManager.GetFinancialPeriod().AsEnumerable(), "Id", "PeriodName", activeFinancialPeriod.Id);
            ViewBag.SectorId = new SelectList(BankVoucherEntryManager.GetAllCompanySector().AsEnumerable(), "Id", "SectorName", companyId);
            ViewBag.CostCentreId = new SelectList(new List<Acc_CostCentre>(), "Id", "CostCentreId");
            ViewBag.CostCentresMultilayers = new SelectList(JournalVoucherEntryManager.GetAllCostCentres().AsEnumerable(), "Id", "ItemName", companyId);
            ViewBag.FromDate = activeFinancialPeriod.PeriodStartDate.GetValueOrDefault().ToString("dd/MM/yyy");
            ViewBag.ToDate = activeFinancialPeriod.PeriodEndDate.GetValueOrDefault().ToString("dd/MM/yyy");

            var itemListBalanceSheet = new List<TrialBalanceViewModel>();

            if (fpId == null)
            {
                model.TrialBalance = itemListBalanceSheet;
                return View(model);
            }

            var ItemList = new List<TrialBalanceViewModel>();
            var iObj = new Acc_ReportViewModel();
            iObj.SectorCode = model.SectorId.ToString();
            iObj.TrialBalanceLebel = 5;
            iObj.StartDate = model.FromDate;
            iObj.EndDate = model.ToDate;
            iObj.GLId = "1,2";
            iObj.CostCentreID = model.CostCentresMultilayers.ToString();
            ItemList = (List<TrialBalanceViewModel>)GetBalanceSheetCostCentre(iObj, 5);

            model.TrialBalance = ItemList;

            return View(model);
        }

        public ActionResult PrintCostCentre(int? page, string sort, GeneralLedgerViewModel model, int? fpId, int? sectorId, string GlId, string fromDate, string toDate, int? CostCentreId)
        {
            List<TrialBalanceViewModel> ItemList = new List<TrialBalanceViewModel>();
            var iObj = new Acc_ReportViewModel();
            iObj.SectorCode = sectorId.ToString();
            iObj.TrialBalanceLebel = 3;
            iObj.StartDate = fromDate;
            iObj.EndDate = toDate;
            iObj.GLId = "1,2";
            ItemList = (List<TrialBalanceViewModel>)GetBalanceSheetNew(iObj, 5);

            LocalReport lr = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/Areas/Accounting/Reports"), "BalanceSheet.rdlc");
            if (System.IO.File.Exists(path))
                lr.ReportPath = path;
            else
                return View("Index");

            string companyName = CompanySectorManager.GetCompanySectorById(sectorId).SectorName;

            ReportParameter[] parameters = new ReportParameter[1];
            parameters[0] = new ReportParameter("param_companyName", companyName);


            ReportDataSource rd = new ReportDataSource("BalanceDS", ItemList);

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

        public ActionResult CostCentreDetail(string subGroupCode, int? sectorId, string fromDate, string toDate, string costCentreId)
        {
            List<TrialBalanceViewModel> ItemList;
            var model = new TrialBalanceViewModel();

            var iObj = new Acc_ReportViewModel();
            iObj.SectorCode = sectorId.ToString();
            iObj.CostCentreID = costCentreId;
            iObj.TrialBalanceLebel = 5;
            iObj.StartDate = fromDate;
            iObj.EndDate = toDate;
            ItemList = (List<TrialBalanceViewModel>)GetCostCentreDetailData(iObj, 2, subGroupCode);

            model.TrialBalance = ItemList;
            return View(model);
        }

        public ActionResult CostCentreToGeneralLedger(string glId, string fromDate, string toDate, string costCentreId)
        {
            GeneralLedgerViewModel model = new GeneralLedgerViewModel();

            var iObj = new Acc_ReportViewModel();
            iObj.SectorCode = costCentreId;
            iObj.GLId = glId;
            iObj.StartDate = fromDate;
            iObj.EndDate = toDate;
            iObj.currencyId = 1;
            List<GeneralLedgerViewModel> item = (List<GeneralLedgerViewModel>)GetGeneralLedgerData(iObj);
            model.GeneralLedger = item;
            return View(model);
        }

        private object GetCostCentreDetailData(Acc_ReportViewModel iObj, int? openingOption, string subGroupCode)
        {
            iObj.OpStartDate = "01/01/2013";
            iObj.OpEndDate = iObj.StartDate;
            var itemList = new List<TrialBalanceViewModel>();
            DataTable dt = ReportAccountManger.GetTrialBalanceCostCentre(iObj);

            decimal dlTotalDebit = 0;
            decimal dlTotalCredit = 0;

            decimal dlTempDebit = 0;
            decimal dlTempCredit = 0;


            var temp1 = "0";
            var temp2 = "0";

            var tempG1 = "0";
            var tempG2 = "0";


            foreach (DataRow dr in dt.Rows)
            {
                dlTempDebit = 0;
                dlTempCredit = 0;
                if (openingOption == 1)
                {
                    dlTempDebit = Convert.ToDecimal(dr["Debit"].ToString());
                    dlTempCredit = Convert.ToDecimal(dr["Credit"].ToString());
                }
                else
                {
                    if (dr["ClsControlCode"].ToString().Substring(0, 1) == "1" || dr["ClsControlCode"].ToString().Substring(0, 1) == "4")
                    {
                        if (Convert.ToDecimal(dr["ClosingBalance"].ToString()) < 0)
                            dlTempCredit = (Convert.ToDecimal(dr["ClosingBalance"].ToString()) * -1);
                        else
                            dlTempDebit = Convert.ToDecimal(dr["ClosingBalance"].ToString());
                    }
                    else
                    {
                        if (Convert.ToDecimal(dr["ClosingBalance"].ToString()) < 0)
                            dlTempDebit = (Convert.ToDecimal(dr["ClosingBalance"].ToString()) * -1);
                        else
                            dlTempCredit = Convert.ToDecimal(dr["ClosingBalance"].ToString());
                    }
                }

                if (iObj.TrialBalanceLebel == 5 && dr["SubGrpControlCode"].ToString() == subGroupCode)
                {

                    temp2 = dr["SubGrpControlCode"].ToString();

                    if (temp1 == temp2)
                    {

                    }
                    else
                    {
                        itemList.Add(new TrialBalanceViewModel()
                        {
                            ClasslCode = "",
                            ClasslHead = "",
                            GrouplHead = "",
                            SubGrouplHead = dr["SubGrpControlName"].ToString(),
                            ControlHead = "",
                            GlHead = "",

                            OpeningBalance = Convert.ToDecimal(dr["OpeningBalance"].ToString()),
                            TotalDebit = dlTempDebit,
                            TotalCredit = dlTempCredit,
                            ClosingBalance = 0,
                        });
                        temp1 = temp2;
                    }

                    tempG2 = dr["ControlContolCode"].ToString();

                    if (tempG1 == tempG2)
                    {

                    }
                    else
                    {
                        itemList.Add(new TrialBalanceViewModel()
                        {
                            ClasslCode = "",
                            ClasslHead = "",
                            GrouplHead = "",
                            SubGrouplHead = "",
                            ControlHead = dr["ControlCodeName"].ToString(),
                            GlHead = "",

                            OpeningBalance = Convert.ToDecimal(dr["OpeningBalance"].ToString()),
                            TotalDebit = dlTempDebit,
                            TotalCredit = dlTempCredit,
                            ClosingBalance = 0,
                        });
                        tempG1 = tempG2;
                    }

                    itemList.Add(new TrialBalanceViewModel()
                    {
                        ClasslCode = "",
                        ClasslHead = "",
                        GrouplHead = "",
                        SubGrouplHead = "",
                        ControlHead = "",
                        GlHead = dr["AccountName"].ToString(),
                        AccountCode = dr["AccountCode"].ToString(),
                        OpeningBalance = Convert.ToDecimal(dr["OpeningBalance"].ToString()),
                        TotalDebit = dlTempDebit,
                        TotalCredit = dlTempCredit,
                        SectorId = Convert.ToInt32(iObj.SectorCode),
                        FromDate = iObj.StartDate,
                        ToDate = iObj.EndDate,
                        ClosingBalance = Convert.ToDecimal(dr["Debit"].ToString()) - Convert.ToDecimal(dr["Credit"].ToString())
                    });
                }

                dlTotalDebit += dlTempDebit;
                dlTotalCredit += dlTempCredit;

            }

            itemList.Add(new TrialBalanceViewModel()
            {
                Particulars = "Total :",
                TotalDebit = dlTotalDebit,
                TotalCredit = dlTotalCredit,
            });

            return itemList;
        }


        #endregion

        //***********************************************************************************************************************************************

        #region IncomeStatement

        public ActionResult IncomeStatementMfg(VoucherEntryViewModel model)
        {
            ModelState.Clear();

            var companyId = voucherMasterManager.GetCostCentreByEmployeeId(_employeeGuidId);
            model.SectorId = companyId;

            if (!model.IsSearch)
            {
                var printFormat = from PrintFormatType formatType in Enum.GetValues(typeof(PrintFormatType))
                                  select new { Id = (int)formatType, Name = formatType.ToString() };

                ViewBag.PrintFormatId = new SelectList(printFormat.AsEnumerable(), "Id", "Name", companyId);

                ViewBag.SectorId = new SelectList(BankVoucherEntryManager.GetAllCompanySector().AsEnumerable(), "Id", "SectorName", model.SectorId);
                model.VoucherList.FromDate = DateTime.Now.Date;
                model.VoucherList.ToDate = DateTime.Now.Date;
            }

            return View(model);
        }

        public ActionResult IncomeStatementMfgPrint(DateTime? fromDate, DateTime? toDate, int sectorId, string ExportType)
        {
            List<Acc_IncomeStatementMfgModel> statement = new List<Acc_IncomeStatementMfgModel>();

            statement = ReportAccountManger.GetIncomeStatementMfg(sectorId, fromDate, toDate);

            LocalReport lr = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/Areas/Accounting/Reports"), "IncomeStatementMfg.rdlc");
            if (System.IO.File.Exists(path))
                lr.ReportPath = path;
            else
                return View("Index");

            ReportDataSource rd = new ReportDataSource("IncomeMfgDS", statement);
            lr.DataSources.Add(rd);
            string reportType = ExportType;
            string mimeType;
            string encoding;
            string fileNameExtension;

            string deviceInfo =
                "<DeviceInfo>" +
                "  <OutputFormat>" + ExportType + "</OutputFormat>" +
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



        public ActionResult IncomeStatement(int? page, string sort, TrialBalanceViewModel model, int? fpId, int? sectorId, int? openingOption, string fromDate, string toDate)
        {
            var companyId = voucherMasterManager.GetCostCentreByEmployeeId(_employeeGuidId);
            sectorId = companyId;

            var activeFinancialPeriod = ReportAccountManger.GetActiveFinancialPeriod();

            ViewBag.FpId = new SelectList(OpeningBalaceManager.GetFinancialPeriod().AsEnumerable(), "Id", "PeriodName", activeFinancialPeriod.Id);
            ViewBag.SectorId = new SelectList(BankVoucherEntryManager.GetAllCompanySector().AsEnumerable(), "Id", "SectorName", sectorId);
            ViewBag.FromDate = activeFinancialPeriod.PeriodStartDate.GetValueOrDefault().ToString("dd/MM/yyy");
            ViewBag.ToDate = activeFinancialPeriod.PeriodEndDate.GetValueOrDefault().ToString("dd/MM/yyy");
            ViewBag.CostCentreId = new SelectList(new List<Acc_CostCentre>(), "Id", "CostCentreId");

            ViewBag.OpeningOption =
                new SelectList(
                    new[] { new { Id = "1", Value = "Show Previous Year" }, new { Id = "2", Value = "Hide Previous Year" } },
                    "Id",
                    "Value");

            var printFormat = from PrintFormatType formatType in Enum.GetValues(typeof(PrintFormatType))
                              select new { Id = (int)formatType, Name = formatType.ToString() };

            ViewBag.PrintFormatId = new SelectList(printFormat.AsEnumerable(), "Id", "Name", companyId);

            if (fpId == null)
            {
                List<TrialBalanceViewModel> ItemListIncomeStatement = new List<TrialBalanceViewModel>();
                model.TrialBalance = ItemListIncomeStatement;
                return View(model);
            }
            List<TrialBalanceViewModel> ItemList = new List<TrialBalanceViewModel>();
            var iObj = new Acc_ReportViewModel();
            iObj.SectorCode = sectorId.ToString();
            iObj.TrialBalanceLebel = 5;
            var periodStartDate = ReportAccountManger.GetFinancialPeriod(fpId.ToString()).PeriodStartDate;
            if (periodStartDate != null) iObj.OpStartDate = periodStartDate.Value.ToString("dd/MM/yyyy");
            iObj.StartDate = fromDate;
            iObj.EndDate = toDate;
            iObj.GLId = "3,4";
            model.TrialBalance = (List<TrialBalanceViewModel>)GetNetProfitOrLossData(iObj, 1);
            model.SectorId = (int)sectorId;
            return View(model);
        }

        public ActionResult CashBook(AccountingReportViewModel model)
        {
            ModelState.Clear();
            model.FromDate = DateTime.Now;
            model.ToDate = DateTime.Now;
            return View(model);
        }

        public ActionResult PrintCashBook(AccountingReportViewModel model)
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
            DataTable dataTable = ReportAccountManger.GetGetCashBook(model.FromDate.GetValueOrDefault(), model.ToDate.GetValueOrDefault(), model.GlId);
            var fromDate = new ReportParameter("FromDate", model.FromDate.GetValueOrDefault().ToString("dd/MM/yyyy"));
            var toDate = new ReportParameter("ToDate", model.ToDate.GetValueOrDefault().ToString("dd/MM/yyyy"));
            var reportParameters = new List<ReportParameter>() { fromDate, toDate };

            string path = Path.Combine(Server.MapPath("~/Areas/Accounting/Reports"), "CashBookReport.rdlc");
            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("CashBookDSet", dataTable) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 8.27, PageHeight = 11.69, MarginTop = .5, MarginLeft = 0.1, MarginRight = 0.1, MarginBottom = 0.1 };
            return ReportExtension.ToFile(reportType, path, reportDataSources, deviceInformation, reportParameters);
        }




        private object GetNetProfitOrLossData(Acc_ReportViewModel iObj, int ReturnData)
        {
            List<TrialBalanceViewModel> ItemList = new List<TrialBalanceViewModel>();
            ItemList = (List<TrialBalanceViewModel>)GetTrialBalanceIncomeStatementWithSubGroupCode(iObj, 2);
            List<TrialBalanceViewModel> ItemListIncomeStatement = new List<TrialBalanceViewModel>();
            decimal dlTotalIncome = 0;
            decimal dlTotalExp = 0;
            decimal dlNetProfitOrLoss = 0;
            decimal totalIncomeTemp = 0;
            decimal totalExpTemp = 0;
            var temp1 = "0";
            var temp2 = "0";

            var tempG1 = "0";
            var tempG2 = "0";

            var tempC1 = "0";
            var tempC2 = "0";

            foreach (TrialBalanceViewModel item in ItemList)
            {
                if (item.ClasslCode == "3")
                {
                    totalIncomeTemp += Convert.ToDecimal(item.TotalCredit);
                }
                else if (item.ClasslCode == "4")
                {
                    totalExpTemp += Convert.ToDecimal(item.TotalCredit);
                }
            }


            foreach (TrialBalanceViewModel dr in ItemList)
            {
                if (dr.ClasslCode == "3")
                {
                    temp2 = dr.ClasslCode;
                    if (temp1 == temp2)
                    {

                    }
                    else
                    {
                        ItemListIncomeStatement.Add(new TrialBalanceViewModel()
                        {
                            ClasslCode = dr.ClasslCode,
                            SubGroupCode = dr.SubGroupCode,
                            ClasslHead = dr.ClasslHead,                     
                            GrouplHead = "",
                            Particulars = "",
                            CurrentYear = dr.ClosingBalance,
                            TotalCredit = totalIncomeTemp,
                            PrevYear = 0
                        });
                        temp1 = temp2;
                    }

                    tempG2 = dr.GrouplHead;

                    if (tempG2 == tempG1)
                    {

                    }
                    else
                    {
                        ItemListIncomeStatement.Add(new TrialBalanceViewModel()
                        {
                            ClasslCode = dr.ClasslCode,
                            SubGroupCode = dr.SubGroupCode,                          
                            ClasslHead = "",
                            GrouplHead = dr.GrouplHead,
                            Particulars = "",
                            CurrentYear = dr.ClosingBalance,
                            //TotalCredit = totalIncomeTemp,
                            PrevYear = 0
                        });
                        tempG1 = tempG2;
                    }

                    //--- 
                    tempC2 = dr.SubGrouplHead;

                    if (tempC2 == tempC1)
                    {

                    }
                    else
                    {
                        ItemListIncomeStatement.Add(new TrialBalanceViewModel()
                        {
                            ClasslCode = dr.ClasslCode,
                            SubGroupCode = "",
                            SubGrouplHead = dr.SubGrouplHead,
                            ClasslHead = "",
                            GrouplHead = "",
                            Particulars = "",
                            CurrentYear = dr.ClosingBalance,            
                            PrevYear = 0
                        });
                        tempC1 = tempC2;
                    }
                    //---
                    ItemListIncomeStatement.Add(new TrialBalanceViewModel()
                    {
                        ClasslCode = dr.ClasslCode,
                        SubGroupCode = dr.SubGroupCode,                        
                        ClasslHead = "",
                        GrouplHead = "",
                        Particulars = dr.ControlHead,
                        CurrentYear = dr.ClosingBalance,
                        TotalCredit = dr.TotalCredit,
                        PrevYear = 0
                    });

                    dlTotalIncome += Convert.ToDecimal(dr.TotalCredit);

                }
                else if (dr.ClasslCode == "4")
                {

                    temp2 = dr.ClasslCode;
                    if (temp1 == temp2)
                    {

                    }
                    else
                    {
                        ItemListIncomeStatement.Add(new TrialBalanceViewModel()
                        {
                            ClasslCode = dr.ClasslCode,
                            SubGroupCode = dr.SubGroupCode,
                            ClasslHead = dr.ClasslHead,                         
                            GrouplHead = "",
                            Particulars = "",
                            CurrentYear = dr.ClosingBalance,
                            TotalCredit = totalExpTemp,
                            PrevYear = 0
                        });
                        temp1 = temp2;
                    }

                    tempG2 = dr.GrouplHead;

                    if (tempG2 == tempG1)
                    {

                    }
                    else
                    {
                        ItemListIncomeStatement.Add(new TrialBalanceViewModel()
                        {
                            ClasslCode = dr.ClasslCode,
                            SubGroupCode = dr.SubGroupCode,                      
                            ClasslHead = "",
                            GrouplHead = dr.GrouplHead,
                            Particulars = "",
                            CurrentYear = dr.ClosingBalance,
                            //TotalCredit = totalIncomeTemp,
                            PrevYear = 0
                        });
                        tempG1 = tempG2;
                    }

                     //---
                    tempC2 = dr.SubGrouplHead;

                    if (tempC2 == tempC1)
                    {

                    }
                    else
                    {
                        ItemListIncomeStatement.Add(new TrialBalanceViewModel()
                        {
                            ClasslCode = dr.ClasslCode,
                            SubGroupCode = "",
                            SubGrouplHead = dr.SubGrouplHead,
                            ClasslHead = "",
                            GrouplHead = "",
                            Particulars = "",
                            CurrentYear = dr.ClosingBalance,
                            PrevYear = 0
                        });
                        tempC1 = tempC2;
                    }

                    //---

                    ItemListIncomeStatement.Add(new TrialBalanceViewModel()
                    {
                        ClasslCode = dr.ClasslCode,
                        SubGroupCode = dr.SubGroupCode,                   
                        ClasslHead = "",
                        GrouplHead = "",
                        Particulars = dr.ControlHead,
                        CurrentYear = dr.ClosingBalance,
                        TotalCredit = dr.TotalCredit,
                        PrevYear = 0
                    });
                    dlTotalExp += Convert.ToDecimal(dr.TotalCredit);
                }
            }
            //dlNetProfitOrLoss = dlTotalIncome - dlTotalExp;
            dlNetProfitOrLoss = totalIncomeTemp - totalExpTemp;
            ItemListIncomeStatement.Add(new TrialBalanceViewModel()
            {
                ClasslCode = "",
                ClasslHead = "",
                GrouplHead = "",
                Particulars = "Net Income",
                CurrentYear = dlNetProfitOrLoss,
                TotalCredit = dlNetProfitOrLoss,
            
                PrevYear = 0
            });
            if (ReturnData == 1) return ItemListIncomeStatement;
            else
                return dlNetProfitOrLoss;
        }

        public ActionResult PrintIncomeStatement(string FpId, string SectorId, string SectorText, int? openingOption, string fromDate, string toDate, string ExportType = "")
        {
            List<TrialBalanceViewModel> ItemList = new List<TrialBalanceViewModel>();
            var iObj = new Acc_ReportViewModel();
            iObj.SectorCode = SectorId;
            iObj.TrialBalanceLebel = 5;
            iObj.StartDate = fromDate;
            iObj.EndDate = toDate;
            iObj.GLId = "3,4";
            ItemList = (List<TrialBalanceViewModel>)GetTrialBalanceDataIncomeStatement(iObj, 2);

            ReportClass rptH = new ReportClass();
            ArrayList iArrayRpt = new ArrayList();
            rptH.FileName = Server.MapPath("/Reports/Accounting/IncomeStatement.rpt");

            rptH.Load();
            TrialBalanceViewModel obj;
            int iCount = 1;
            decimal dlTotalIncome = 0;
            decimal dlTotalExp = 0;
            decimal dlNetProfitOrLoss = 0;

            foreach (TrialBalanceViewModel dr in ItemList)
            {
                if (ItemList.Count == iCount) break;
                iCount += 1;
                if (dr.ClasslCode == "3")
                    dlTotalIncome += Convert.ToDecimal(dr.ClosingBalance);
                else if (dr.ClasslCode == "4")
                    dlTotalExp += Convert.ToDecimal(dr.ClosingBalance);
            }

            dlNetProfitOrLoss = dlTotalIncome - dlTotalExp;

            Company company = CompanyManager.GetCompanyInfo();
            if (SectorText == "- Select -")
                SectorText = "Consulate";
            iCount = 1;

            foreach (TrialBalanceViewModel dr in ItemList)
            {
                if (ItemList.Count == iCount) break;
                iCount += 1;

                obj = new TrialBalanceViewModel();
                obj.CompanyName = company.Name;
                obj.SectorName = SectorText;
                obj.ReportTitle = "Income Statement";
                obj.DateBetween = fromDate + " to " + toDate;
                obj.OpeningOption = openingOption.Value;

                obj.ClasslHead = dr.ClasslHead;
                obj.GrouplHead = dr.GrouplHead;
                obj.Particulars = dr.ControlHead;
                obj.CurrentYear = dr.ClosingBalance;
                obj.PrevYear = 0;
                obj.Others1 = "Net Income";
                obj.Others2 = "";
                obj.Others1_CurrentYear = dlNetProfitOrLoss;
                obj.Others1_PrevYear = 0;
                iArrayRpt.Add(obj);
            }
            if (iArrayRpt.Count == 0)
            {
                obj = new TrialBalanceViewModel();
                obj.CompanyName = company.Name;
                obj.SectorName = SectorText;
                obj.ReportTitle = "Income Statement";
                obj.DateBetween = fromDate + " to " + toDate;
                iArrayRpt.Add(obj);
            }

            rptH.SetDataSource(iArrayRpt);

            if (ExportType == "Excel")
            {
                Stream stream = rptH.ExportToStream(ExportFormatType.Excel);
                rptH.Dispose();
                return File(stream, "application/vnd.ms-excel", "GeneralLedger.xls");
            }
            else
            {
                Stream stream = rptH.ExportToStream(ExportFormatType.PortableDocFormat);
                rptH.Dispose();
                return File(stream, "application/pdf");
            }
        }

        public ActionResult IncomeStatementDetail(string subGroupCode, int? sectorId, string fromDate, string toDate)
        {
            List<TrialBalanceViewModel> ItemList;
            var model = new TrialBalanceViewModel();

            var iObj = new Acc_ReportViewModel();
            iObj.SectorCode = sectorId.ToString();
            iObj.TrialBalanceLebel = 5;
            iObj.StartDate = fromDate;
            iObj.EndDate = toDate;
            ItemList = (List<TrialBalanceViewModel>)GetIncomeStatementDetailData(iObj, 2, subGroupCode);

            model.TrialBalance = ItemList;
            return View(model);
        }


        private object GetIncomeStatementDetailData(Acc_ReportViewModel iObj, int? openingOption, string subGroupCode)
        {
            iObj.OpStartDate = "01/01/2013";
            iObj.OpEndDate = iObj.StartDate;
            var itemList = new List<TrialBalanceViewModel>();
            DataTable dt = ReportAccountManger.GetTrialBalance(iObj);

            decimal dlTotalDebit = 0;
            decimal dlTotalCredit = 0;

            decimal dlTempDebit = 0;
            decimal dlTempCredit = 0;


            var temp1 = "0";
            var temp2 = "0";

            var tempG1 = "0";
            var tempG2 = "0";



            foreach (DataRow dr in dt.Rows)
            {
                dlTempDebit = 0;
                dlTempCredit = 0;
                if (openingOption == 1)
                {
                    dlTempDebit = Convert.ToDecimal(dr["Debit"].ToString());
                    dlTempCredit = Convert.ToDecimal(dr["Credit"].ToString());
                }
                else
                {
                    if (dr["ClsControlCode"].ToString().Substring(0, 1) == "3")
                    {

                        //dlTempCredit = (Convert.ToDecimal(dr["ClosingBalance"].ToString()) * -1);
                        dlTempCredit = Convert.ToDecimal(dr["Credit"].ToString()) - Convert.ToDecimal(dr["Debit"].ToString());

                    }
                    else if (dr["ClsControlCode"].ToString().Substring(0, 1) == "4")
                    {

                        dlTempCredit = Convert.ToDecimal(dr["Debit"].ToString()) - Convert.ToDecimal(dr["Credit"].ToString());
                    }
                }



                if (iObj.TrialBalanceLebel == 5 && dr["SubGrpControlCode"].ToString() == subGroupCode)
                {

                    temp2 = dr["SubGrpControlCode"].ToString();

                    if (temp1 == temp2)
                    {

                    }
                    else
                    {
                        itemList.Add(new TrialBalanceViewModel()
                        {
                            ClasslCode = "",
                            ClasslHead = "",
                            GrouplHead = "",
                            SubGrouplHead = dr["SubGrpControlName"].ToString(),
                            ControlHead = "",
                            GlHead = "",

                            OpeningBalance = Convert.ToDecimal(dr["OpeningBalance"].ToString()),
                            TotalDebit = dlTempDebit,
                            TotalCredit = dlTempCredit,
                            ClosingBalance = 0,
                        });
                        temp1 = temp2;
                    }

                    tempG2 = dr["ControlContolCode"].ToString();

                    if (tempG1 == tempG2)
                    {

                    }
                    else
                    {
                        itemList.Add(new TrialBalanceViewModel()
                        {
                            ClasslCode = "",
                            ClasslHead = "",
                            GrouplHead = "",
                            SubGrouplHead = "",
                            ControlHead = dr["ControlCodeName"].ToString(),
                            GlHead = "",

                            OpeningBalance = Convert.ToDecimal(dr["OpeningBalance"].ToString()),
                            TotalDebit = dlTempDebit,
                            TotalCredit = dlTempCredit,
                            ClosingBalance = 0,
                        });
                        tempG1 = tempG2;
                    }

                    itemList.Add(new TrialBalanceViewModel()
                    {
                        ClasslCode = "",
                        ClasslHead = "",
                        GrouplHead = "",
                        SubGrouplHead = "",
                        ControlHead = "",
                        GlHead = dr["AccountName"].ToString(),
                        AccountCode = dr["AccountCode"].ToString(),
                        OpeningBalance = Convert.ToDecimal(dr["OpeningBalance"].ToString()),
                        TotalDebit = dlTempDebit,
                        TotalCredit = dlTempCredit,
                        SectorId = Convert.ToInt32(iObj.SectorCode),
                        FromDate = iObj.StartDate,
                        ToDate = iObj.EndDate,
                        ClosingBalance = Convert.ToDecimal(dr["ClosingBalance"].ToString())
                    });
                }

                dlTotalDebit += dlTempDebit;
                dlTotalCredit += dlTempCredit;

            }

            itemList.Add(new TrialBalanceViewModel()
            {
                Particulars = "Total :",
                TotalDebit = dlTotalDebit,
                TotalCredit = dlTotalCredit,
            });

            return itemList;
        }

        public ActionResult IncomeStatementToGeneralLedger(string glId, string fromDate, string toDate, string costCentreId)
        {
            GeneralLedgerViewModel model = new GeneralLedgerViewModel();


            DateTime fromdate = DateTime.ParseExact(fromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            DateTime todate = DateTime.ParseExact(toDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            var iObj = new Acc_ReportViewModel();
            iObj.SectorCode = costCentreId;
            iObj.GLId = glId;
            iObj.StartDate = fromDate;
            iObj.EndDate = toDate;
            iObj.currencyId = 1;
            var fpid = ReportAccountManger.GetFinancialPeriodId(fromdate, todate);
            var periodStartDate = ReportAccountManger.GetFinancialPeriod(fpid.ToString()).PeriodStartDate;
            if (periodStartDate != null) iObj.OpStartDate = periodStartDate.Value.ToString("dd/MM/yyyy");

            List<GeneralLedgerViewModel> item = (List<GeneralLedgerViewModel>)GetGeneralLedgerData(iObj);
            model.GeneralLedger = item;
            return View(model);
        }



        #endregion

        //***********************************************************************************************************************************************

        #region CashFlow

        public ActionResult CashFlow(int? page, string sort, TrialBalanceViewModel model, int? fpId, int? sectorId, string fromDate, string toDate)
        {
            var companyId = voucherMasterManager.GetCostCentreByEmployeeId(_employeeGuidId);
            model.SectorId = (int)companyId;

            var activeFinancialPeriod = ReportAccountManger.GetActiveFinancialPeriod();

            ViewBag.FpId = new SelectList(OpeningBalaceManager.GetFinancialPeriod().AsEnumerable(), "Id", "PeriodName", activeFinancialPeriod.Id);
            ViewBag.SectorId = new SelectList(BankVoucherEntryManager.GetAllCompanySector().AsEnumerable(), "Id", "SectorName", companyId);
            ViewBag.CostCentreId = new SelectList(new List<Acc_CostCentre>(), "Id", "CostCentreId");
            ViewBag.FromDate = activeFinancialPeriod.PeriodStartDate.GetValueOrDefault().ToString("dd/MM/yyy");
            ViewBag.ToDate = activeFinancialPeriod.PeriodEndDate.GetValueOrDefault().ToString("dd/MM/yyy");

            var printFormat = from PrintFormatType formatType in Enum.GetValues(typeof(PrintFormatType))
                              select new { Id = (int)formatType, Name = formatType.ToString() };

            ViewBag.PrintFormatId = new SelectList(printFormat.AsEnumerable(), "Id", "Name", companyId);

            if (fpId == null)
            {
                var itemListIncomeStatement = new List<TrialBalanceViewModel>();
                model.TrialBalance = itemListIncomeStatement;
                return View(model);
            }

            var ItemList = new List<TrialBalanceViewModel>();
            var iObj = new Acc_ReportViewModel();
            iObj.SectorCode = sectorId.ToString();
            iObj.StartDate = fromDate;
            iObj.EndDate = toDate;
            ItemList = (List<TrialBalanceViewModel>)GetCashFlowData(iObj);
            model.TrialBalance = ItemList;
            return View(model);
        }

        private object GetCashFlowData(Acc_ReportViewModel model)
        {
            List<TrialBalanceViewModel> ItemList = new List<TrialBalanceViewModel>();
            DataTable dt = ReportAccountManger.GetCashFlowData(model);

            List<TrialBalanceViewModel> ItemListCashFlow = new List<TrialBalanceViewModel>();

            decimal dlCurrentYear = 0;
            decimal dlInflow = 0, dlOutFlow = 0;
            foreach (DataRow dr in dt.Rows)
            {
                if (dr["CashFlow"].ToString() == "Inflow")
                {
                    dlCurrentYear = Convert.ToDecimal(dr["Credit"].ToString()) -
                                    Convert.ToDecimal(dr["Debit"].ToString());

                    dlInflow += dlCurrentYear;
                }
                else
                {
                    dlCurrentYear = Convert.ToDecimal(dr["Debit"].ToString()) -
                                    Convert.ToDecimal(dr["Credit"].ToString());
                    dlOutFlow += dlCurrentYear;
                }
                ItemListCashFlow.Add(new TrialBalanceViewModel()
                {
                    GrouplHead = dr["CashFlow"].ToString(),
                    ControlHead = dr["ControlCodeName"].ToString(),
                    Particulars = dr["AccountName"].ToString(),
                    CurrentYear = dlCurrentYear,
                    PrevYear = 0
                });
            }
            decimal dlNetIncrsDecrs = dlInflow - dlOutFlow;
            ItemListCashFlow.Add(new TrialBalanceViewModel()
            {
                Particulars = "Net Increase or (decrease) in cash and cash equivalents",
                CurrentYear = dlNetIncrsDecrs,
                PrevYear = 0
            });
            decimal dlOpening = 0;
            ItemListCashFlow.Add(new TrialBalanceViewModel()
            {
                Particulars = "Cash and cash equivalents at the beginning of the period",
                CurrentYear = dlOpening,
                PrevYear = 0
            });
            decimal dlcosing = dlOpening + dlNetIncrsDecrs;
            ItemListCashFlow.Add(new TrialBalanceViewModel()
            {
                Particulars = "Cash and cash equivalents at the end of the period",
                CurrentYear = dlcosing,
                PrevYear = 0
            });


            return ItemListCashFlow;
        }

        public ActionResult PrintCashFlow(string FpId, string SectorId, string SectorText, string fromDate, string toDate, string ExportType)
        {
            List<TrialBalanceViewModel> ItemList = new List<TrialBalanceViewModel>();
            var iObj = new Acc_ReportViewModel();
            iObj.SectorCode = SectorId;
            iObj.TrialBalanceLebel = 4;
            iObj.StartDate = fromDate;
            iObj.EndDate = toDate;
            ItemList = (List<TrialBalanceViewModel>)GetCashFlowData(iObj);

            ReportClass rptH = new ReportClass();
            ArrayList iArrayRpt = new ArrayList();
            rptH.FileName = Server.MapPath("/Reports/Accounting/CashFlow.rpt");

            rptH.Load();
            TrialBalanceViewModel obj;
            int iCount = 1;

            iCount = 1;
            Company company = CompanyManager.GetCompanyInfo();
            if (SectorText == "- Select -")
                SectorText = "Consulate";

            foreach (TrialBalanceViewModel dr in ItemList)
            {
                //if (ItemList.Count == iCount) break;
                //iCount += 1;

                obj = new TrialBalanceViewModel();
                obj.CompanyName = company.Name;
                obj.SectorName = SectorText;
                obj.ReportTitle = "Cash Flow";
                obj.DateBetween = fromDate + " to " + toDate;

                obj.GrouplHead = dr.GrouplHead;
                obj.Particulars = dr.Particulars;
                obj.CurrentYear = dr.CurrentYear;
                obj.PrevYear = dr.PrevYear;

                iArrayRpt.Add(obj);
            }

            rptH.SetDataSource(iArrayRpt);

            if (ExportType == "Excel")
            {
                Stream stream = rptH.ExportToStream(ExportFormatType.Excel);
                rptH.Dispose();
                return File(stream, "application/vnd.ms-excel", "GeneralLedger.xls");
            }
            else
            {
                Stream stream = rptH.ExportToStream(ExportFormatType.PortableDocFormat);
                rptH.Dispose();
                return File(stream, "application/pdf");
            }
        }

        #endregion
       
  
        public ActionResult ScheduleOfFixedAsset(int? page, string sort, TrialBalanceViewModel model, int? fpId, int? sectorId, string fromDate, string toDate)
        {
            ViewBag.FpId = new SelectList(OpeningBalaceManager.GetFinancialPeriod().AsEnumerable(), "Id", "PeriodName");
            ViewBag.SectorId = new SelectList(BankVoucherEntryManager.GetAllCompanySector().AsEnumerable(), "Id",
                "SectorName");
            ViewBag.CostCentreId = new SelectList(new List<Acc_CostCentre>(), "Id", "CostCentreId");
            ViewBag.OpeningOption =
                new SelectList(
                    new[] { new { Id = "1", Value = "Show Opening" }, new { Id = "2", Value = "Hide Opening" } }, "Id",
                    "Value", 2);

            List<TrialBalanceViewModel> ItemList = new List<TrialBalanceViewModel>();
            if (fpId != null)
            {
                var iObj = new Acc_ReportViewModel();
                iObj.SectorCode = sectorId.ToString();
                iObj.TrialBalanceLebel = 4;
                iObj.StartDate = fromDate;
                iObj.EndDate = toDate;
                ItemList = (List<TrialBalanceViewModel>)GetScheduleOfFixedAssetData(iObj, 2);
            }
            model.TrialBalance = ItemList;
            return View(model);
        }

        private object GetScheduleOfFixedAssetData(Acc_ReportViewModel iObj, int? openingOption)
        {
            iObj.OpStartDate = "01/01/2013";
            iObj.OpEndDate = iObj.StartDate;
            List<TrialBalanceViewModel> ItemList = new List<TrialBalanceViewModel>();
            DataTable dt = ReportAccountManger.GetTrialBalance(iObj);

            decimal dlTotalBalance = 0;
            decimal dlTotalDep = 0;

            decimal dlTempDebit = 0;
            decimal dlTempCredit = 0;

            decimal dlDepreciation = 0;
            decimal dlRateOfDep = 10;
            foreach (DataRow dr in dt.Rows)
            {
                if (dr["GrpControlName"].ToString() == "Fixed Assets")
                {
                    dlTempDebit = 0;
                    dlTempCredit = 0;
                    if (openingOption == 1)
                    {
                        dlTempDebit = Convert.ToDecimal(dr["Debit"].ToString());
                        dlTempCredit = Convert.ToDecimal(dr["Credit"].ToString());
                    }
                    else
                    {
                        if (dr["ClsControlCode"].ToString().Substring(0, 1) == "1" ||
                            dr["ClsControlCode"].ToString().Substring(0, 1) == "4")
                        {
                            if (Convert.ToDecimal(dr["ClosingBalance"].ToString()) < 0)
                                dlTempCredit = (Convert.ToDecimal(dr["ClosingBalance"].ToString()) * -1);
                            else
                                dlTempDebit = Convert.ToDecimal(dr["ClosingBalance"].ToString());
                        }
                        else
                        {
                            if (Convert.ToDecimal(dr["ClosingBalance"].ToString()) < 0)
                                dlTempDebit = (Convert.ToDecimal(dr["ClosingBalance"].ToString()) * -1);
                            else
                                dlTempCredit = Convert.ToDecimal(dr["ClosingBalance"].ToString());
                        }
                    }

                    dlDepreciation = Convert.ToDecimal(dr["ClosingBalance"].ToString()) * dlRateOfDep;
                    dlDepreciation = dlDepreciation / 100;
                    ItemList.Add(new TrialBalanceViewModel()
                    {
                        ClasslCode = dr["ClsControlCode"].ToString(),
                        ClasslHead = dr["ClsControlName"].ToString(),
                        GrouplHead = dr["GrpControlName"].ToString(),
                        SubGrouplHead = dr["SubGrpControlName"].ToString(),
                        ControlHead = dr["ControlCodeName"].ToString(),

                        OpeningBalance = Convert.ToDecimal(dr["OpeningBalance"].ToString()),
                        TotalDebit = dlTempDebit,
                        TotalCredit = dlTempCredit,
                        ClosingBalance = Convert.ToDecimal(dr["ClosingBalance"].ToString()),
                        RateOfDep = dlRateOfDep,
                        Depreciation = dlDepreciation
                    });
                    dlTotalBalance += Convert.ToDecimal(dr["ClosingBalance"].ToString());
                    dlTotalDep += dlDepreciation;
                }
            }

            ItemList.Add(new TrialBalanceViewModel()
            {

                ClasslHead = "Total",

                ClosingBalance = dlTotalBalance,

                Depreciation = dlTotalDep
            });

            return ItemList;
        }


        #region BalanceSummary

        public ActionResult BalanceSummary(int? page, string sort, GeneralLedgerViewModel model, int? fpId, int? sectorId, int? costcentreId, string GlId, string fromDate, string toDate, int? CostCentreId)
        {
            var companyId = voucherMasterManager.GetCostCentreByEmployeeId(_employeeGuidId);
            sectorId = companyId;

            ViewBag.FpId = new SelectList(OpeningBalaceManager.GetFinancialPeriod().AsEnumerable(), "Id", "PeriodName");
            ViewBag.SectorId = new SelectList(BankVoucherEntryManager.GetAllCompanySector().AsEnumerable(), "Id", "SectorName", sectorId);

            var costCentre = CostCentreManager.GetCostCentreById(CostCentreId);
            var costCentrelist = new List<Acc_CostCentre>();

            if (costCentre != null)
            {
                costCentrelist.Add(costCentre);
            }

            ViewBag.CostCentreId = costCentre != null ? new SelectList(costCentrelist, "Id", "CostCentreName", costCentre.Id) : new SelectList(costCentrelist, "Id", "CostCentreId");

            var currencyList = from CurrencyType currency in Enum.GetValues(typeof(CurrencyType))
                               select new { Id = (int)currency, Name = currency.ToString() };

            ViewBag.currencyId = new SelectList(currencyList, "Id", "Name", (int)CurrencyType.BDT);

            var printFormat = from PrintFormatType formatType in Enum.GetValues(typeof(PrintFormatType))
                              select new { Id = (int)formatType, Name = formatType.ToString() };

            ViewBag.PrintFormatId = new SelectList(printFormat.AsEnumerable(), "Id", "Name", companyId);

            List<GeneralLedgerViewModel> ItemList = new List<GeneralLedgerViewModel>();

            if (GlId != "0" && GlId != "" && string.IsNullOrEmpty(GlId) == false)
            {
                string glCode = GlId.Substring(GlId.Length - 5, 5);

                var reportObject = new Acc_ReportViewModel();

                if (glCode[0] == '1' || glCode[0] == '2')
                {
                    reportObject.OpStartDate = "01/01/2013";
                }
                else
                {
                    var periodStartDate = ReportAccountManger.GetFinancialPeriod(fpId.ToString()).PeriodStartDate;
                    if (periodStartDate != null) reportObject.OpStartDate = periodStartDate.Value.ToString("dd/MM/yyyy");
                }

                reportObject.SectorCode = sectorId.ToString();
                reportObject.CostCentreID = costcentreId.ToString();
                reportObject.StartDate = fromDate;
                reportObject.EndDate = toDate;
                reportObject.GLId = glCode;
                ItemList = (List<GeneralLedgerViewModel>)GetGeneralLedgerData(reportObject);
            }

            model.GeneralLedger = ItemList;
            return View(model);
        }

        public ActionResult ControlSummaryForthLayer(int? page, string sort, GeneralLedgerViewModel model, int? fpId, int? sectorId, string GlId, string fromDate, string toDate, int? CostCentreId)
        {
            var companyId = voucherMasterManager.GetCostCentreByEmployeeId(_employeeGuidId);
            sectorId = companyId;

            var activeFinancialPeriod = ReportAccountManger.GetActiveFinancialPeriod();

            ViewBag.FpId = new SelectList(OpeningBalaceManager.GetFinancialPeriod().AsEnumerable(), "Id", "PeriodName", activeFinancialPeriod.Id);
            ViewBag.SectorId = new SelectList(BankVoucherEntryManager.GetAllCompanySector().AsEnumerable(), "Id", "SectorName", sectorId);
            ViewBag.FromDate = activeFinancialPeriod.PeriodStartDate.GetValueOrDefault().ToString("dd/MM/yyy");
            ViewBag.ToDate = activeFinancialPeriod.PeriodEndDate.GetValueOrDefault().ToString("dd/MM/yyy");

            var costCentre = CostCentreManager.GetCostCentreById(CostCentreId);
            var costCentrelist = new List<Acc_CostCentre>();

            if (costCentre != null)
            {
                costCentrelist.Add(costCentre);
            }

            ViewBag.CostCentreId = costCentre != null ? new SelectList(costCentrelist, "Id", "CostCentreName", costCentre.Id) : new SelectList(costCentrelist, "Id", "CostCentreId");

            var printFormat = from PrintFormatType formatType in Enum.GetValues(typeof(PrintFormatType))
                              select new { Id = (int)formatType, Name = formatType.ToString() };

            ViewBag.PrintFormatId = new SelectList(printFormat.AsEnumerable(), "Id", "Name", companyId);

            var itemList = new List<GeneralLedgerViewModel>();

            if (GlId != "0" && GlId != "" && string.IsNullOrEmpty(GlId) == false)
            {
                string[] glCode = GlId.Split('-');
                var iObj = new Acc_ReportViewModel();
                iObj.SectorCode = sectorId.ToString();
                iObj.GLId = glCode[0];
                iObj.StartDate = fromDate;
                iObj.EndDate = toDate;
                itemList = (List<GeneralLedgerViewModel>)GetControlLedgerData(iObj);
            }
            model.GeneralLedger = itemList;
            return View(model);
        }
        #endregion

        #region cheque

        public ActionResult Cheque(int id)
        {
            DataTable voucherDataTable = voucherMasterManager.GetChequeReport(id, PortalContext.CurrentUser.CompId);
            string path = Path.Combine(Server.MapPath("~/Areas/Accounting/Reports"), "Cheque.rdlc");
            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("ChequeDataSource", voucherDataTable) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 7.5, PageHeight = 3.5, MarginTop = 0, MarginLeft = 0, MarginRight = 0, MarginBottom = 0 };
            return ReportExtension.ToFile(ReportType.PDF, path, reportDataSources, deviceInformation);
        }
        #endregion


        public ActionResult ControlTrialBalanceNewIndex(int? page, string sort, TrialBalanceViewModel model, int? fpId, int? sectorId, int? openingOption, string fromDate, string toDate)
        {
            var companyId = voucherMasterManager.GetCostCentreByEmployeeId(_employeeGuidId);
            sectorId = companyId;

            var activeFinancialPeriod = ReportAccountManger.GetActiveFinancialPeriod();

            ViewBag.FpId = new SelectList(OpeningBalaceManager.GetFinancialPeriod().AsEnumerable(), "Id", "PeriodName", activeFinancialPeriod.Id);
            ViewBag.SectorId = new SelectList(BankVoucherEntryManager.GetAllCompanySector().AsEnumerable(), "Id", "SectorName", sectorId);
            ViewBag.FromDate = activeFinancialPeriod.PeriodStartDate.GetValueOrDefault().ToString("dd/MM/yyy");
            ViewBag.ToDate = activeFinancialPeriod.PeriodEndDate.GetValueOrDefault().ToString("dd/MM/yyy");
            ViewBag.CostCentreId = new SelectList(new List<Acc_CostCentre>(), "Id", "CostCentreId");

            ViewBag.OpeningOption =
                new SelectList(
                    new[] { new { Id = "1", Value = "Show Previous Year" }, new { Id = "2", Value = "Hide Previous Year" } },
                    "Id",
                    "Value");

            var printFormat = from PrintFormatType formatType in Enum.GetValues(typeof(PrintFormatType))
                              select new { Id = (int)formatType, Name = formatType.ToString() };

            ViewBag.PrintFormatId = new SelectList(printFormat.AsEnumerable(), "Id", "Name", companyId);

            if (fpId == null)
            {
                List<TrialBalanceViewModel> ItemListIncomeStatement = new List<TrialBalanceViewModel>();
                model.TrialBalance = ItemListIncomeStatement;
                return View(model);
            }
            List<TrialBalanceViewModel> ItemList = new List<TrialBalanceViewModel>();
            var iObj = new Acc_ReportViewModel();
            iObj.SectorCode = sectorId.ToString();
            iObj.TrialBalanceLebel = 5;
            var periodStartDate = ReportAccountManger.GetFinancialPeriod(fpId.ToString()).PeriodStartDate;
            if (periodStartDate != null) iObj.OpStartDate = periodStartDate.Value.ToString("dd/MM/yyyy");
            iObj.StartDate = fromDate;
            iObj.EndDate = toDate;
            iObj.GLId = "3,4";
            model.TrialBalance = (List<TrialBalanceViewModel>)GetNetProfitOrLossData(iObj, 1);
            model.SectorId = (int)sectorId;
            return View(model);
        }

        public ActionResult ControlTrialBalanceNew(int? sectorId, string fromDate, string toDate, int FpId, string ExportType)
        {

            DateTime startDate = DateTime.Parse(fromDate, CultureInfo.GetCultureInfo("en-gb"));
            DateTime endDate = DateTime.Parse(toDate, CultureInfo.GetCultureInfo("en-gb"));

            DataTable data = ReportAccountManger.GetControlTrialBalance(sectorId.Value, startDate, endDate, FpId);

            LocalReport lr = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/Areas/Accounting/Reports"), "ControlTrialBalanceNew.rdlc");
            if (System.IO.File.Exists(path))
                lr.ReportPath = path;
            else
                return View("Index");

            ReportDataSource rd = new ReportDataSource("DataSet1", data);

            lr.DataSources.Add(rd);
            string reportType = ExportType;
            string mimeType;
            string encoding;
            string fileNameExtension;

            string deviceInfo =
                "<DeviceInfo>" +
                "  <OutputFormat>" + ExportType + "</OutputFormat>" +
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


        public ActionResult GLTrialBalanceNewIndex(int? page, string sort, TrialBalanceViewModel model, int? fpId, int? sectorId, int? openingOption, string fromDate, string toDate)
        {
            var companyId = voucherMasterManager.GetCostCentreByEmployeeId(_employeeGuidId);
            sectorId = companyId;

            var activeFinancialPeriod = ReportAccountManger.GetActiveFinancialPeriod();

            ViewBag.FpId = new SelectList(OpeningBalaceManager.GetFinancialPeriod().AsEnumerable(), "Id", "PeriodName", activeFinancialPeriod.Id);
            ViewBag.SectorId = new SelectList(BankVoucherEntryManager.GetAllCompanySector().AsEnumerable(), "Id", "SectorName", sectorId);
            ViewBag.FromDate = activeFinancialPeriod.PeriodStartDate.GetValueOrDefault().ToString("dd/MM/yyy");
            ViewBag.ToDate = activeFinancialPeriod.PeriodEndDate.GetValueOrDefault().ToString("dd/MM/yyy");
            ViewBag.CostCentreId = new SelectList(new List<Acc_CostCentre>(), "Id", "CostCentreId");

            ViewBag.OpeningOption =
                new SelectList(
                    new[] { new { Id = "1", Value = "Show Previous Year" }, new { Id = "2", Value = "Hide Previous Year" } },
                    "Id",
                    "Value");

            var printFormat = from PrintFormatType formatType in Enum.GetValues(typeof(PrintFormatType))
                              select new { Id = (int)formatType, Name = formatType.ToString() };

            ViewBag.PrintFormatId = new SelectList(printFormat.AsEnumerable(), "Id", "Name", companyId);

            if (fpId == null)
            {
                List<TrialBalanceViewModel> ItemListIncomeStatement = new List<TrialBalanceViewModel>();
                model.TrialBalance = ItemListIncomeStatement;
                return View(model);
            }
            List<TrialBalanceViewModel> ItemList = new List<TrialBalanceViewModel>();
            var iObj = new Acc_ReportViewModel();
            iObj.SectorCode = sectorId.ToString();
            iObj.TrialBalanceLebel = 5;
            var periodStartDate = ReportAccountManger.GetFinancialPeriod(fpId.ToString()).PeriodStartDate;
            if (periodStartDate != null) iObj.OpStartDate = periodStartDate.Value.ToString("dd/MM/yyyy");
            iObj.StartDate = fromDate;
            iObj.EndDate = toDate;
            iObj.GLId = "3,4";
            model.TrialBalance = (List<TrialBalanceViewModel>)GetNetProfitOrLossData(iObj, 1);
            model.SectorId = (int)sectorId;
            return View(model);
        }

        public ActionResult GLTrialBalanceNew(int? sectorId, string fromDate, string toDate, int FpId, string ExportType)
        {

            DateTime startDate = DateTime.Parse(fromDate, CultureInfo.GetCultureInfo("en-gb"));
            DateTime endDate = DateTime.Parse(toDate, CultureInfo.GetCultureInfo("en-gb"));

            DataTable data = ReportAccountManger.GetGLTrialBalance(sectorId.Value, startDate, endDate, FpId);

            LocalReport lr = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/Areas/Accounting/Reports"), "GLTrialBalanceNew.rdlc");
            if (System.IO.File.Exists(path))
                lr.ReportPath = path;
            else
                return View("Index");

            ReportDataSource rd = new ReportDataSource("DataSet1", data);

            lr.DataSources.Add(rd);
            string reportType = ExportType;
            string mimeType;
            string encoding;
            string fileNameExtension;

            string deviceInfo =
                "<DeviceInfo>" +
                "  <OutputFormat>" + ExportType + "</OutputFormat>" +
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


        public ActionResult SubGroupTrialBalanceNewIndex(int? page, string sort, TrialBalanceViewModel model, int? fpId, int? sectorId, int? openingOption, string fromDate, string toDate)
        {
            var companyId = voucherMasterManager.GetCostCentreByEmployeeId(_employeeGuidId);
            sectorId = companyId;

            var activeFinancialPeriod = ReportAccountManger.GetActiveFinancialPeriod();

            ViewBag.FpId = new SelectList(OpeningBalaceManager.GetFinancialPeriod().AsEnumerable(), "Id", "PeriodName", activeFinancialPeriod.Id);
            ViewBag.SectorId = new SelectList(BankVoucherEntryManager.GetAllCompanySector().AsEnumerable(), "Id", "SectorName", sectorId);
            ViewBag.FromDate = activeFinancialPeriod.PeriodStartDate.GetValueOrDefault().ToString("dd/MM/yyy");
            ViewBag.ToDate = activeFinancialPeriod.PeriodEndDate.GetValueOrDefault().ToString("dd/MM/yyy");
            ViewBag.CostCentreId = new SelectList(new List<Acc_CostCentre>(), "Id", "CostCentreId");

            ViewBag.OpeningOption =
                new SelectList(
                    new[] { new { Id = "1", Value = "Show Previous Year" }, new { Id = "2", Value = "Hide Previous Year" } },
                    "Id",
                    "Value");

            var printFormat = from PrintFormatType formatType in Enum.GetValues(typeof(PrintFormatType))
                              select new { Id = (int)formatType, Name = formatType.ToString() };

            ViewBag.PrintFormatId = new SelectList(printFormat.AsEnumerable(), "Id", "Name", companyId);

            if (fpId == null)
            {
                List<TrialBalanceViewModel> ItemListIncomeStatement = new List<TrialBalanceViewModel>();
                model.TrialBalance = ItemListIncomeStatement;
                return View(model);
            }
            List<TrialBalanceViewModel> ItemList = new List<TrialBalanceViewModel>();
            var iObj = new Acc_ReportViewModel();
            iObj.SectorCode = sectorId.ToString();
            iObj.TrialBalanceLebel = 5;
            var periodStartDate = ReportAccountManger.GetFinancialPeriod(fpId.ToString()).PeriodStartDate;
            if (periodStartDate != null) iObj.OpStartDate = periodStartDate.Value.ToString("dd/MM/yyyy");
            iObj.StartDate = fromDate;
            iObj.EndDate = toDate;
            iObj.GLId = "3,4";
            model.TrialBalance = (List<TrialBalanceViewModel>)GetNetProfitOrLossData(iObj, 1);
            model.SectorId = (int)sectorId;
            return View(model);
        }

        public ActionResult SubGroupTrialBalanceNew(int? sectorId, string fromDate, string toDate, int FpId, string ExportType)
        {

            DateTime startDate = DateTime.Parse(fromDate, CultureInfo.GetCultureInfo("en-gb"));
            DateTime endDate = DateTime.Parse(toDate, CultureInfo.GetCultureInfo("en-gb"));

            DataTable data = ReportAccountManger.GetSubGroupTrialBalance(sectorId.Value, startDate, endDate, FpId);

            LocalReport lr = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/Areas/Accounting/Reports"), "SubGroupTrialBalanceNew.rdlc");
            if (System.IO.File.Exists(path))
                lr.ReportPath = path;
            else
                return View("Index");

            ReportDataSource rd = new ReportDataSource("DataSet1", data);

            lr.DataSources.Add(rd);
            string reportType = ExportType;
            string mimeType;
            string encoding;
            string fileNameExtension;

            string deviceInfo =
                "<DeviceInfo>" +
                "  <OutputFormat>" + ExportType + "</OutputFormat>" +
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


        public ActionResult GroupTrialBalanceNewIndex(int? page, string sort, TrialBalanceViewModel model, int? fpId, int? sectorId, int? openingOption, string fromDate, string toDate)
        {
            var companyId = voucherMasterManager.GetCostCentreByEmployeeId(_employeeGuidId);
            sectorId = companyId;

            var activeFinancialPeriod = ReportAccountManger.GetActiveFinancialPeriod();

            ViewBag.FpId = new SelectList(OpeningBalaceManager.GetFinancialPeriod().AsEnumerable(), "Id", "PeriodName", activeFinancialPeriod.Id);
            ViewBag.SectorId = new SelectList(BankVoucherEntryManager.GetAllCompanySector().AsEnumerable(), "Id", "SectorName", sectorId);
            ViewBag.FromDate = activeFinancialPeriod.PeriodStartDate.GetValueOrDefault().ToString("dd/MM/yyy");
            ViewBag.ToDate = activeFinancialPeriod.PeriodEndDate.GetValueOrDefault().ToString("dd/MM/yyy");
            ViewBag.CostCentreId = new SelectList(new List<Acc_CostCentre>(), "Id", "CostCentreId");

            ViewBag.OpeningOption =
                new SelectList(
                    new[] { new { Id = "1", Value = "Show Previous Year" }, new { Id = "2", Value = "Hide Previous Year" } },
                    "Id",
                    "Value");

            var printFormat = from PrintFormatType formatType in Enum.GetValues(typeof(PrintFormatType))
                              select new { Id = (int)formatType, Name = formatType.ToString() };

            ViewBag.PrintFormatId = new SelectList(printFormat.AsEnumerable(), "Id", "Name", companyId);

            if (fpId == null)
            {
                List<TrialBalanceViewModel> ItemListIncomeStatement = new List<TrialBalanceViewModel>();
                model.TrialBalance = ItemListIncomeStatement;
                return View(model);
            }
            List<TrialBalanceViewModel> ItemList = new List<TrialBalanceViewModel>();
            var iObj = new Acc_ReportViewModel();
            iObj.SectorCode = sectorId.ToString();
            iObj.TrialBalanceLebel = 5;
            var periodStartDate = ReportAccountManger.GetFinancialPeriod(fpId.ToString()).PeriodStartDate;
            if (periodStartDate != null) iObj.OpStartDate = periodStartDate.Value.ToString("dd/MM/yyyy");
            iObj.StartDate = fromDate;
            iObj.EndDate = toDate;
            iObj.GLId = "3,4";
            model.TrialBalance = (List<TrialBalanceViewModel>)GetNetProfitOrLossData(iObj, 1);
            model.SectorId = (int)sectorId;
            return View(model);
        }

        public ActionResult GroupTrialBalanceNew(int? sectorId, string fromDate, string toDate, int FpId, string ExportType)
        {

            DateTime startDate = DateTime.Parse(fromDate, CultureInfo.GetCultureInfo("en-gb"));
            DateTime endDate = DateTime.Parse(toDate, CultureInfo.GetCultureInfo("en-gb"));

            DataTable data = ReportAccountManger.GetGroupTrialBalance(sectorId.Value, startDate, endDate, FpId);

            LocalReport lr = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/Areas/Accounting/Reports"), "GroupTrialBalanceNew.rdlc");
            if (System.IO.File.Exists(path))
                lr.ReportPath = path;
            else
                return View("Index");

            ReportDataSource rd = new ReportDataSource("DataSet1", data);

            lr.DataSources.Add(rd);
            string reportType = ExportType;
            string mimeType;
            string encoding;
            string fileNameExtension;

            string deviceInfo =
                "<DeviceInfo>" +
                "  <OutputFormat>" + ExportType + "</OutputFormat>" +
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



        public ActionResult ControlLedgerNewIndex(int? page, string sort, TrialBalanceViewModel model, int? fpId, int? sectorId, int? openingOption, string fromDate, string toDate)
        {
            var companyId = voucherMasterManager.GetCostCentreByEmployeeId(_employeeGuidId);
            sectorId = companyId;

            var activeFinancialPeriod = ReportAccountManger.GetActiveFinancialPeriod();

            ViewBag.FpId = new SelectList(OpeningBalaceManager.GetFinancialPeriod().AsEnumerable(), "Id", "PeriodName", activeFinancialPeriod.Id);
            ViewBag.SectorId = new SelectList(BankVoucherEntryManager.GetAllCompanySector().AsEnumerable(), "Id", "SectorName", sectorId);

            ViewBag.FromDate = activeFinancialPeriod.PeriodStartDate.GetValueOrDefault().ToString("dd/MM/yyy");
            ViewBag.ToDate = activeFinancialPeriod.PeriodEndDate.GetValueOrDefault().ToString("dd/MM/yyy");

            ViewBag.CostCentreId = new SelectList(new List<Acc_CostCentre>(), "Id", "CostCentreId");

            ViewBag.OpeningOption =
                new SelectList(
                    new[] { new { Id = "1", Value = "Show Previous Year" }, new { Id = "2", Value = "Hide Previous Year" } },
                    "Id",
                    "Value");

            var printFormat = from PrintFormatType formatType in Enum.GetValues(typeof(PrintFormatType))
                              select new { Id = (int)formatType, Name = formatType.ToString() };

            ViewBag.PrintFormatId = new SelectList(printFormat.AsEnumerable(), "Id", "Name", companyId);

            if (fpId == null)
            {
                List<TrialBalanceViewModel> ItemListIncomeStatement = new List<TrialBalanceViewModel>();
                model.TrialBalance = ItemListIncomeStatement;
                return View(model);
            }
            List<TrialBalanceViewModel> ItemList = new List<TrialBalanceViewModel>();
            var iObj = new Acc_ReportViewModel();
            iObj.SectorCode = sectorId.ToString();
            iObj.TrialBalanceLebel = 5;
            var periodStartDate = ReportAccountManger.GetFinancialPeriod(fpId.ToString()).PeriodStartDate;
            if (periodStartDate != null) iObj.OpStartDate = periodStartDate.Value.ToString("dd/MM/yyyy");
            iObj.StartDate = fromDate;
            iObj.EndDate = toDate;
            iObj.GLId = "3,4";
            model.TrialBalance = (List<TrialBalanceViewModel>)GetNetProfitOrLossData(iObj, 1);
            model.SectorId = (int)sectorId;
            return View(model);
        }

        public ActionResult ControlLedgerNew(int? sectorId, string fromDate, string toDate, int FpId, string ControlHead, string ExportType)
        {

            DateTime startDate = DateTime.Parse(fromDate, CultureInfo.GetCultureInfo("en-gb"));
            DateTime endDate = DateTime.Parse(toDate, CultureInfo.GetCultureInfo("en-gb"));

            int count = ControlHead.Split('-').Count();

            int controlCode = Convert.ToInt32(ControlHead.Split('-').ElementAt(count - 1));

            DataTable data = ReportAccountManger.GetControlLedger(sectorId.Value, startDate, endDate, FpId, controlCode);

            LocalReport lr = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/Areas/Accounting/Reports"), "ControlLedgerNew.rdlc");
            if (System.IO.File.Exists(path))
                lr.ReportPath = path;
            else
                return View("Index");

            ReportDataSource rd = new ReportDataSource("DataSet1", data);

            lr.DataSources.Add(rd);
            string reportType = ExportType;
            string mimeType;
            string encoding;
            string fileNameExtension;

            string deviceInfo =
                "<DeviceInfo>" +
                "  <OutputFormat>" + ExportType + "</OutputFormat>" +
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


        public ActionResult BalanceSheetNoteIndex(int? page, string sort, TrialBalanceViewModel model, int? fpId, int? sectorId, int? openingOption, string fromDate, string toDate)
        {
            var companyId = voucherMasterManager.GetCostCentreByEmployeeId(_employeeGuidId);
            sectorId = companyId;

            var activeFinancialPeriod = ReportAccountManger.GetActiveFinancialPeriod();

            ViewBag.FpId = new SelectList(OpeningBalaceManager.GetFinancialPeriod().AsEnumerable(), "Id", "PeriodName", activeFinancialPeriod.Id);
            ViewBag.SectorId = new SelectList(BankVoucherEntryManager.GetAllCompanySector().AsEnumerable(), "Id", "SectorName", sectorId);

            ViewBag.FromDate = activeFinancialPeriod.PeriodStartDate.GetValueOrDefault().ToString("dd/MM/yyy");
            ViewBag.ToDate = activeFinancialPeriod.PeriodEndDate.GetValueOrDefault().ToString("dd/MM/yyy");

            ViewBag.CostCentreId = new SelectList(new List<Acc_CostCentre>(), "Id", "CostCentreId");

            ViewBag.OpeningOption =
                new SelectList(
                    new[] { new { Id = "1", Value = "Show Previous Year" }, new { Id = "2", Value = "Hide Previous Year" } },
                    "Id",
                    "Value");

            var printFormat = from PrintFormatType formatType in Enum.GetValues(typeof(PrintFormatType))
                              select new { Id = (int)formatType, Name = formatType.ToString() };

            ViewBag.PrintFormatId = new SelectList(printFormat.AsEnumerable(), "Id", "Name", companyId);

            if (fpId == null)
            {
                List<TrialBalanceViewModel> ItemListIncomeStatement = new List<TrialBalanceViewModel>();
                model.TrialBalance = ItemListIncomeStatement;
                return View(model);
            }
            List<TrialBalanceViewModel> ItemList = new List<TrialBalanceViewModel>();
            var iObj = new Acc_ReportViewModel();
            iObj.SectorCode = sectorId.ToString();
            iObj.TrialBalanceLebel = 5;
            var periodStartDate = ReportAccountManger.GetFinancialPeriod(fpId.ToString()).PeriodStartDate;
            if (periodStartDate != null) iObj.OpStartDate = periodStartDate.Value.ToString("dd/MM/yyyy");
            iObj.StartDate = fromDate;
            iObj.EndDate = toDate;
            iObj.GLId = "3,4";
            model.TrialBalance = (List<TrialBalanceViewModel>)GetNetProfitOrLossData(iObj, 1);
            model.SectorId = (int)sectorId;
            return View(model);
        }

        public ActionResult BalanceSheetNote(int? sectorId, string fromDate, string toDate, int FpId, string ControlHead, string Note, string ExportType)
        {

            DateTime startDate = DateTime.Parse(fromDate, CultureInfo.GetCultureInfo("en-gb"));
            DateTime endDate = DateTime.Parse(toDate, CultureInfo.GetCultureInfo("en-gb"));

            int count = ControlHead.Split('-').Count();

            int controlCode = Convert.ToInt32(ControlHead.Split('-').ElementAt(count - 1));

            DataTable data = ReportAccountManger.GetBalanceSheetNote(sectorId.Value, startDate, endDate, FpId, controlCode);

            LocalReport lr = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/Areas/Accounting/Reports"), "BalanceSheetNote.rdlc");
            if (System.IO.File.Exists(path))
                lr.ReportPath = path;
            else
                return View("Index");

            ReportParameter[] parameters = new ReportParameter[1];
            parameters[0] = new ReportParameter("param_Note", Note);

            ReportDataSource rd = new ReportDataSource("DataSet1", data);
            lr.SetParameters(parameters);
            lr.DataSources.Add(rd);
            string reportType = ExportType;
            string mimeType;
            string encoding;
            string fileNameExtension;

            string deviceInfo =
                "<DeviceInfo>" +
                "  <OutputFormat>" + ExportType + "</OutputFormat>" +
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

        public ActionResult AgingIndex(int? page, string sort, TrialBalanceViewModel model, int? fpId, int? sectorId, int? openingOption, string fromDate, string toDate)
        {
            var companyId = voucherMasterManager.GetCostCentreByEmployeeId(_employeeGuidId);
            sectorId = companyId;

            ViewBag.FpId = new SelectList(OpeningBalaceManager.GetFinancialPeriod().AsEnumerable(), "Id", "PeriodName");
            ViewBag.SectorId = new SelectList(BankVoucherEntryManager.GetAllCompanySector().AsEnumerable(), "Id", "SectorName", sectorId);

            ViewBag.CostCentreId = new SelectList(new List<Acc_CostCentre>(), "Id", "CostCentreId");

            ViewBag.OpeningOption =
                new SelectList(
                    new[] { new { Id = "1", Value = "Show Previous Year" }, new { Id = "2", Value = "Hide Previous Year" } },
                    "Id",
                    "Value");

            var printFormat = from PrintFormatType formatType in Enum.GetValues(typeof(PrintFormatType))
                              select new { Id = (int)formatType, Name = formatType.ToString() };

            ViewBag.PrintFormatId = new SelectList(printFormat.AsEnumerable(), "Id", "Name", companyId);

            if (fpId == null)
            {
                List<TrialBalanceViewModel> ItemListIncomeStatement = new List<TrialBalanceViewModel>();
                model.TrialBalance = ItemListIncomeStatement;
                return View(model);
            }
            List<TrialBalanceViewModel> ItemList = new List<TrialBalanceViewModel>();
            var iObj = new Acc_ReportViewModel();
            iObj.SectorCode = sectorId.ToString();
            iObj.TrialBalanceLebel = 5;
            var periodStartDate = ReportAccountManger.GetFinancialPeriod(fpId.ToString()).PeriodStartDate;
            if (periodStartDate != null) iObj.OpStartDate = periodStartDate.Value.ToString("dd/MM/yyyy");
            iObj.StartDate = fromDate;
            iObj.EndDate = toDate;
            iObj.GLId = "3,4";
            model.TrialBalance = (List<TrialBalanceViewModel>)GetNetProfitOrLossData(iObj, 1);
            model.SectorId = (int)sectorId;
            return View(model);
        }

        public ActionResult Aging(int? sectorId, string fromDate, string ExportType)
        {
            DateTime startDate = DateTime.Parse(fromDate, CultureInfo.GetCultureInfo("en-gb"));

            DataTable data = ReportAccountManger.GetAgingData(sectorId.Value, startDate);

            LocalReport lr = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/Areas/Accounting/Reports"), "AgingReport.rdlc");
            if (System.IO.File.Exists(path))
                lr.ReportPath = path;
            else
                return View("Index");

            ReportDataSource rd = new ReportDataSource("DataSet1", data);

            lr.DataSources.Add(rd);
            string reportType = ExportType;
            string mimeType;
            string encoding;
            string fileNameExtension;

            string deviceInfo =
                "<DeviceInfo>" +
                "  <OutputFormat>" + ExportType + "</OutputFormat>" +
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

        public ActionResult TagSearch(string term)
        {
            List<string> tags = JournalVoucherEntryManager.GetAccountName();
            return this.Json(tags.Where(t => t.Substring(0, t.Length - 11).ToLower().Contains(term.ToLower())).Skip(0).Take(15), JsonRequestBehavior.AllowGet);
        }

        public ActionResult TagSearchThirdLayer(string term)
        {
            List<string> tags = JournalVoucherEntryManager.GetAccountNameThirdLayer();
            return this.Json(tags.Where(t => t.Substring(0, t.Length - 8).ToLower().Contains(term.ToLower())).Skip(0).Take(15), JsonRequestBehavior.AllowGet);
        }

        public ActionResult ControlSearch(string term)
        {
            List<string> tags = JournalVoucherEntryManager.GetControlNames();
            return this.Json(tags.Where(t => t.Substring(0, t.Length - 8).ToLower().Contains(term.ToLower())).Skip(0).Take(15), JsonRequestBehavior.AllowGet);
        }
        public ActionResult SubGroupAndControlSearch(string term)
        {
            List<string> tags = JournalVoucherEntryManager.GetSubGroupAndControlNames();
            return this.Json(tags.Where(t => t.Substring(0, t.Length - 8).ToLower().Contains(term.ToLower())).Skip(0).Take(15), JsonRequestBehavior.AllowGet);
        }
        public ActionResult ControlSummarySearch(string term)
        {
            List<string> tags = JournalVoucherEntryManager.GetControlSummaryNames();
            return this.Json(tags.Where(t => t.Substring(0, t.Length - 8).ToLower().Contains(term.ToLower())).Skip(0).Take(15), JsonRequestBehavior.AllowGet);
        }
    }
}