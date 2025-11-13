using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.Services.Description;
using SCERP.BLL.IManager.IAccountingManager;
using SCERP.BLL.Manager.AccountingManager;
using SCERP.Common;
using SCERP.Model;
using SCERP.Web.Areas.Accounting.Models.ViewModels;
using System.IO;
using SCERP.Web.Controllers;


namespace SCERP.Web.Areas.Accounting.Controllers
{
    public class BankVoucherEntryController : BaseAccountingController
    {

        [AjaxAuthorize(Roles = "bankvoucher-2,bankvoucher-3")]
        public ActionResult Edit(BankVoucherEntryViewModel model)
        {
            ModelState.Clear();

            model.FinancialPeriodId = Convert.ToInt32(BankVoucherEntryManager.GetPeriodName());
            ViewBag.CostCentreId = new SelectList(new List<Acc_CostCentre>(), "Id", "CostCentreId");

            var bank = BankVoucherEntryManager.GetAccVoucherMasterById(model.Id);
            model.VoucherDate = DateTime.Now;

            ViewBag.VoucherTypeId =
                new SelectList(
                    new[] {new {Id = "BP", Value = "Bank Payment"}, new {Id = "BR", Value = "Bank Receipt"}}, "Id",
                    "Value");

            ViewBag.SectorId = new SelectList(BankVoucherEntryManager.GetAllCompanySector().AsEnumerable(), "Id",
                "SectorName");

            if (model.Id > 0)
            {
                model.FinancialPeriodId =
                    Convert.ToInt32(BankVoucherEntryManager.GetFinancialPeriodById(bank.FinancialPeriodId.Value));
                model.Particulars = bank.Particulars;
                model.VoucherDate = bank.VoucherDate;
                model.CheckDate = bank.CheckDate;
                model.CheckNo = bank.CheckNo;

                ViewBag.VoucherTypeId =
                    new SelectList(
                        new[] {new {Id = "BP", Value = "Bank Payment"}, new {Id = "BR", Value = "Bank Receipt"}}, "Id",
                        "Value", bank.VoucherType);

                ViewBag.CostCentreId =
                    new SelectList(BankVoucherEntryManager.GetAllCostCentres(bank.SectorId.Value).ToList(), "Id",
                        "CostCentreName", bank.CostCentreId);

                ViewBag.SectorId = new SelectList(BankVoucherEntryManager.GetAllCompanySector().AsEnumerable(), "Id",
                    "SectorName", bank.SectorId);
            }
            if (Request.IsAjaxRequest())
            {
                return PartialView(model);
            }
            return View(model);
        }

         [AjaxAuthorize(Roles = "bankvoucher-2,bankvoucher-3")]
        public JsonResult GetBankVoucherDetail(long Id)
        {
            var count = 1;
            decimal totalDebit = 0;
            decimal totalCredit = 0;
            decimal totalbalance = 0;
            var temp = BankVoucherEntryManager.GetBankVoucherDetail(Id);

            List<object> lt = new List<object>();

            foreach (var t in temp)
            {
                lt.Add(
                    new
                    {
                        Id = count,
                        GlName = t.Acc_GLAccounts.AccountCode + "-" + t.Acc_GLAccounts.AccountName,
                        Particulars = t.Particulars == null ? "" : t.Particulars,
                        Debit = t.Debit,
                        Credit = t.Credit
                    });
                totalDebit += t.Debit.Value;
                totalCredit += t.Credit.Value;
                count++;
            }
            totalbalance = totalDebit - totalCredit;
            return Json(new { data = lt, tDr = totalDebit, tCr = totalCredit, tbc = totalbalance, Success = true },
                JsonRequestBehavior.AllowGet);
        }

         [AjaxAuthorize(Roles = "bankvoucher-2,bankvoucher-3")]
        public ActionResult SaveMaster(Acc_VoucherMaster voucherMaster)
        {
            var voucher = BankVoucherEntryManager.GetAccVoucherMasterById(voucherMaster.Id) ?? new Acc_VoucherMaster();

            voucher.VoucherType = voucherMaster.VoucherType;
            voucher.VoucherNo = voucherMaster.VoucherNo;
            voucher.VoucherDate = voucherMaster.VoucherDate;
            voucher.Particulars = voucherMaster.Particulars;
            voucher.SectorId = voucherMaster.SectorId;
            voucher.CostCentreId = voucherMaster.CostCentreId;
            voucher.CheckDate = voucherMaster.CheckDate;
            voucher.CheckNo = voucherMaster.CheckNo;
            voucher.FinancialPeriodId = voucherMaster.FinancialPeriodId;
            voucher.FinancialPeriodId = BankVoucherEntryManager.GetPeriodId();
            voucher.IsActive = true;

            if (voucherMaster.Id == 0)
            {
                voucher.VoucherNo = BankVoucherEntryManager.GetVoucherNo(BankVoucherEntryManager.GetPeriodId(),
                    voucherMaster.VoucherType);
            }
            BankVoucherEntryManager.SaveMaster(voucher);

            return Json(new {Success = false, Errors = "Delete successfully !"});
        }

         [AjaxAuthorize(Roles = "bankvoucher-2,bankvoucher-3")]
        public ActionResult SaveDetail(Acc_VoucherDetail voucherDetail)
        {
            BankVoucherEntryManager.SaveDetail(voucherDetail);
            return Json(new {Success = false, Errors = "Delete successfully !"});
        }

         [AjaxAuthorize(Roles = "bankvoucher-2,bankvoucher-3")]
        public ActionResult TagSearch(string term)
        {
            List<string> tags = BankVoucherEntryManager.GetAccountName();

            return this.Json(tags.Where(t => t.Substring(11, t.Length - 11).StartsWith(term.ToLower())),
                JsonRequestBehavior.AllowGet);
        }

         [AjaxAuthorize(Roles = "bankvoucher-2,bankvoucher-3")]
        public ActionResult BankAccountNames(string term)
        {
            List<string> tags = BankVoucherEntryManager.GetBankAccountNames();

            return this.Json(tags.Where(t => t.Substring(11, t.Length - 11).StartsWith(term.ToLower())),
                JsonRequestBehavior.AllowGet);
        }

         [AjaxAuthorize(Roles = "bankvoucher-2,bankvoucher-3")]
        public ActionResult GetBankInHand()
        {
            var Bank = BankVoucherEntryManager.GetBankInHand();
            return Json(new {data = Bank, Success = true});
        }

         [AjaxAuthorize(Roles = "bankvoucher-2,bankvoucher-3")]
        public JsonResult GetCostCentreBySector(int sectorId)
        {
            var costCentre = BankVoucherEntryManager.GetAllCostCentres(sectorId);
            return Json(costCentre, JsonRequestBehavior.AllowGet);
        }

         [AjaxAuthorize(Roles = "bankvoucher-2,bankvoucher-3")]
        public ActionResult GetInWords(string amount)
        {
            var inWords = "";
            try
            {
                if (Convert.ToDecimal(amount) < 0)
                    inWords = "Negative Number";
                else
                    inWords = Spell.SpellAmount.InWrods(Convert.ToDecimal(amount));

                return Json(new {data = inWords, Success = true});
            }
            catch (Exception ex)
            {
                return Json(new {data = inWords, Success = true});
            }
        }

         [AjaxAuthorize(Roles = "bankvoucher-2,bankvoucher-3")]
        public ActionResult CheckGLHead(string glHead)
        {
            var value = BankVoucherEntryManager.CheckGLHeadValidation(glHead);
            return Json(new {data = value, Success = true});
        }

         [AjaxAuthorize(Roles = "bankvoucher-2,bankvoucher-3")]
        public ActionResult GetAccountId(decimal accountCode)
        {
            string value = BankVoucherEntryManager.GetAccountId(accountCode).ToString();
            return Json(new {data = value, Success = true});
        }       
    }
}