using iTextSharp.text.pdf.qrcode;
using SCERP.BLL.IManager.IAccountingManager;
using SCERP.Common;
using SCERP.Model;
using SCERP.Web.Areas.Accounting.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SCERP.Web.Areas.Accounting.Controllers
{
    public class GlHeadGroupChangeController : BaseAccountingController
    {
        private readonly Guid? _employeeGuidId = PortalContext.CurrentUser.UserId;
        private readonly IVoucherMasterManager voucherMasterManager;
        public GlHeadGroupChangeController(IVoucherMasterManager voucherMasterManager)
        {
            this.voucherMasterManager = voucherMasterManager;
        }
        public ActionResult Index(int? page, string sort, GeneralLedgerViewModel model, int? costCentreId)
        {
            var companyId = voucherMasterManager.GetCostCentreByEmployeeId(_employeeGuidId);

            if (companyId != null) model.sectorId = (int) companyId;

            ViewBag.sectorId = new SelectList(BankVoucherEntryManager.GetAllCompanySector().AsEnumerable(), "Id", "SectorName", model.sectorId);

            var costCentre = CostCentreManager.GetCostCentreById(costCentreId);
            var costCentrelist = new List<Acc_CostCentre>();
            if (costCentre != null)
                costCentrelist.Add(costCentre);
            ViewBag.CostCentreId = costCentre != null ? new SelectList(costCentrelist, "Id", "CostCentreName", costCentre.Id) : new SelectList(costCentrelist, "Id", "CostCentreId");
           
            List<GeneralLedgerViewModel> ItemList = new List<GeneralLedgerViewModel>();

            if (model.GLHeadName != "0" && model.GLHeadName != "" && string.IsNullOrEmpty(model.GLHeadName) == false)
            {
                string glCode = model.GLHeadName.Substring(model.GLHeadName.Length - 10, 10);
                var iObj = new Acc_ReportViewModel();
                iObj.SectorCode = model.sectorId.ToString();
                iObj.CostCentreID = costCentreId.ToString();
                iObj.StartDate = "01/01/2000";
                iObj.EndDate = "01/01/2100";
                iObj.GLId = glCode;
                ItemList = (List<GeneralLedgerViewModel>) GetGeneralLedgerData(iObj);
            }

            model.GeneralLedger = ItemList;
            return View(model);
        }

        public ActionResult TransferGlAccount(GeneralLedgerViewModel model)
        {
            if (string.IsNullOrEmpty(model.GLHeadName) || string.IsNullOrEmpty(model.GLHeadNameNew))
                return ErrorResult("Please enter valid Account Head !");

            string glCode = model.GLHeadName.Substring(model.GLHeadName.Length - 10, 10);
            string glCodeNew = model.GLHeadNameNew.Substring(model.GLHeadNameNew.Length - 10, 10);

            var companyId = voucherMasterManager.GetCostCentreByEmployeeId(_employeeGuidId);
            if (companyId != null) model.sectorId = (int) companyId;

            var result = voucherMasterManager.ChangeGlHeadGroup(model.sectorId.ToString(), glCode, glCodeNew);

            model.GLHeadName = model.GLHeadNameNew;

            if (result > 0)
                return ErrorResult(" Saved Successfully !");

            return ErrorResult(" Error Created !");
        }

        private object GetGeneralLedgerData(Acc_ReportViewModel model)
        {
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
            iObj.OpStartDate = "01/01/2013";
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
            foreach (DataRow dr in dt.Rows)
            {
                dlClosingBalance = dlClosingBalance + Convert.ToDecimal(dr["Debit"].ToString()) - Convert.ToDecimal(dr["Credit"].ToString());

                ItemList.Add(new GeneralLedgerViewModel()
                {
                    TotalDebit = Convert.ToDecimal(dr["Debit"].ToString()),
                    TotalCredit = Convert.ToDecimal(dr["Credit"].ToString()),
                    Particulars = dr["Particulars"].ToString(),
                    VoucherDateShow = dr["VoucherDate"].ToString(),
                    VoucherNoShow = dr["VoucherNo"].ToString(),
                    Balance = dlClosingBalance
                });
                dlTotalDebit += Convert.ToDecimal(dr["Debit"].ToString());
                dlTotalCredit += Convert.ToDecimal(dr["Credit"].ToString());
            }
            ItemList.Add(new GeneralLedgerViewModel()
            {
                Particulars = "Total :",
                TotalDebit = dlTotalDebit,
                TotalCredit = dlTotalCredit,
                Balance = 0,
            });
            //ItemList.Add(new GeneralLedgerViewModel()
            //{
            //    Particulars = "Closing Balance",
            //    Balance = dlClosingBalance,
            //    TotalDebit = 0,
            //    TotalCredit = 0
            //});
            return ItemList;
        }
    }
}