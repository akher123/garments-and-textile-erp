using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
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
    public class CashVoucherEntryController : BaseAccountingController
    {
        [AjaxAuthorize(Roles = "cashvoucher-2,cashvoucher-3")]
        public ActionResult Edit(CashVoucherEntryViewModel model)
        {
            var cash = CashVoucherEntryManager.GetAccVoucherMasterById(model.Id);

            ModelState.Clear();
            model.VoucherDate = DateTime.Now;
            model.FinancialPeriodId = Convert.ToInt32(CashVoucherEntryManager.GetPeriodName());    
        
            ViewBag.CostCentreId = new SelectList(new List<Acc_CostCentre>(), "Id", "CostCentreId");
            ViewBag.VoucherTypeId = new SelectList(new[] {new {Id = "CP", Value = "Cash Payment"}, new {Id = "CR", Value = "Cash Receipt"}}, "Id", "Value");
            ViewBag.SectorId = new SelectList(CashVoucherEntryManager.GetAllCompanySector().AsEnumerable(), "Id", "SectorName");
              
            if (model.Id > 0)
            {
                model.FinancialPeriodId = Convert.ToInt32(CashVoucherEntryManager.GetFinancialPeriodById(cash.FinancialPeriodId.Value));                    
                model.Particulars = cash.Particulars;
                model.VoucherDate = cash.VoucherDate;

                ViewBag.VoucherTypeId =
                    new SelectList(
                        new[] {new {Id = "CP", Value = "Cash Payment"}, new {Id = "CR", Value = "Cash Receipt"}}, "Id",
                        "Value", cash.VoucherType);

                ViewBag.CostCentreId = new SelectList(CashVoucherEntryManager.GetAllCostCentres(cash.SectorId.Value).ToList(), "Id", "CostCentreName", cash.CostCentreId);
                ViewBag.SectorId = new SelectList(CashVoucherEntryManager.GetAllCompanySector().AsEnumerable(), "Id", "SectorName", cash.SectorId);                   
            }

            if (Request.IsAjaxRequest())
            {
                return PartialView(model);
            }
            return View(model);
        }

        public ActionResult GetCashInHandBalance(Acc_ClosingBalanceViewModel Obj)
        {
            Obj.FpId = CashVoucherEntryManager.GetPeriodId();
            Obj.GlId = CashVoucherEntryManager.GetCashInHandGLId();
            string closingBalance = CashVoucherEntryManager.GetClosingBalance(Obj);
            return Json(new {Success = true, Data = closingBalance});
        }

        [AjaxAuthorize(Roles = "cashvoucher-2,cashvoucher-3")]
        public ActionResult SaveMaster(Acc_VoucherMaster voucherMaster)
        {
            var voucher = CashVoucherEntryManager.GetAccVoucherMasterById(voucherMaster.Id) ?? new Acc_VoucherMaster();

            voucher.VoucherType = voucherMaster.VoucherType;
            voucher.VoucherNo = voucherMaster.VoucherNo;
            voucher.VoucherDate = voucherMaster.VoucherDate;
            voucher.Particulars = voucherMaster.Particulars;
            voucher.SectorId = voucherMaster.SectorId;
            voucher.CostCentreId = voucherMaster.CostCentreId;
            voucher.FinancialPeriodId = voucherMaster.FinancialPeriodId;
            voucher.FinancialPeriodId = CashVoucherEntryManager.GetPeriodId();
            voucher.IsActive = true;

            if (voucherMaster.Id == 0)
            {
                voucher.VoucherNo = CashVoucherEntryManager.GetVoucherNo(CashVoucherEntryManager.GetPeriodId(),
                    voucherMaster.VoucherType);
            }
            CashVoucherEntryManager.SaveMaster(voucher);

            return Json(new {Success = false, Errors = "Delete successfully !"});
        }

        [AjaxAuthorize(Roles = "cashvoucher-2,cashvoucher-3")]
        public ActionResult SaveDetail(Acc_VoucherDetail voucherDetail)
        {
            CashVoucherEntryManager.SaveDetail(voucherDetail);
            return Json(new {Success = false, Errors = "Delete successfully !"});
        }

        [AjaxAuthorize(Roles = "cashvoucher-2,cashvoucher-3")]
        public ActionResult TagSearch(string term)
        {
            List<string> tags = CashVoucherEntryManager.GetAccountName();

            return this.Json(tags.Where(t => t.Substring(11, t.Length - 11).StartsWith(term.ToLower())),
                JsonRequestBehavior.AllowGet);
        }

        [AjaxAuthorize(Roles = "cashvoucher-2,cashvoucher-3")]
        public ActionResult CashAccountNames(string term)
        {
            List<string> tags = CashVoucherEntryManager.GetCashAccountNames();

            return this.Json(tags.Where(t => t.Substring(11, t.Length - 11).StartsWith(term.ToLower())),
                JsonRequestBehavior.AllowGet);
        }

        [AjaxAuthorize(Roles = "cashvoucher-2,cashvoucher-3")]
        public ActionResult GetCashInHand()
        {
            var cash = CashVoucherEntryManager.GetCashInHand();
            return Json(new {data = cash, Success = true});
        }

        [AjaxAuthorize(Roles = "cashvoucher-2,cashvoucher-3")]
        public JsonResult GetCostCentreBySector(int sectorId)
        {
            var costCentre = CashVoucherEntryManager.GetAllCostCentres(sectorId);
            return Json(costCentre, JsonRequestBehavior.AllowGet);
        }

        [AjaxAuthorize(Roles = "cashvoucher-2,cashvoucher-3")]
        public JsonResult GetCashVoucherDetail(long Id)
        {
            var count = 1;
            decimal totalDebit = 0;
            decimal totalCredit = 0;
            decimal totalbalance = 0;
            var temp = CashVoucherEntryManager.GetCashVoucherDetail(Id);

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
            return Json(new {data = lt, tDr = totalDebit, tCr = totalCredit, tbc = totalbalance, Success = true},
                JsonRequestBehavior.AllowGet);
        }

        [AjaxAuthorize(Roles = "cashvoucher-2,cashvoucher-3")]
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

        [AjaxAuthorize(Roles = "cashvoucher-2,cashvoucher-3")]
        public ActionResult CheckGLHead(string glHead)
        {
            var value = CashVoucherEntryManager.CheckGLHeadValidation(glHead);
            return Json(new {data = value, Success = true});
        }

        [AjaxAuthorize(Roles = "cashvoucher-2,cashvoucher-3")]
        public ActionResult GetAccountId(decimal accountCode)
        {
            string value = CashVoucherEntryManager.GetAccountId(accountCode).ToString();
            return Json(new {data = value, Success = true});
        }
    }
}