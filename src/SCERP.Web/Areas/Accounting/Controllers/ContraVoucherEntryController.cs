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
using Spell;


namespace SCERP.Web.Areas.Accounting.Controllers
{
    public class ContraVoucherEntryController : BaseAccountingController
    {

        [AjaxAuthorize(Roles = "contravoucher-2,contravoucher-3")]
        public ActionResult Edit(ContraVoucherEntryViewModel model)
        {
            ModelState.Clear();

            model.FinancialPeriodId = Convert.ToInt32(ContraVoucherEntryManager.GetPeriodName());
            ViewBag.CostCentreId = new SelectList(new List<Acc_CostCentre>(), "Id", "CostCentreId");

            var contra = ContraVoucherEntryManager.GetAccVoucherMasterById(model.Id);
            model.VoucherDate = DateTime.Now;

            ViewBag.VoucherTypeId =
                new SelectList(
                    new[]
                    {
                        new {Id = "BC", Value = "Bank To Cash"},
                        new {Id = "CB", Value = "Cash To Bank"},
                        new {Id = "BB", Value = "Bank To Bank"}
                    }, "Id", "Value");

            ViewBag.SectorId = new SelectList(ContraVoucherEntryManager.GetAllCompanySector().AsEnumerable(), "Id", "SectorName");
                
            if (model.Id > 0)
            {
                model.FinancialPeriodId = Convert.ToInt32(ContraVoucherEntryManager.GetFinancialPeriodById(contra.FinancialPeriodId.Value));                
                model.Particulars = contra.Particulars;
                model.VoucherDate = contra.VoucherDate;
                model.CheckDate = contra.CheckDate;
                model.CheckNo = contra.CheckNo;

                ViewBag.VoucherTypeId =
                    new SelectList(
                        new[]
                        {
                            new {Id = "BC", Value = "Bank To Cash"},
                            new {Id = "CB", Value = "Cash To Bank"},
                            new {Id = "BB", Value = "Bank To Bank"}
                        }, "Id", "Value", contra.VoucherType);

                ViewBag.CostCentreId = new SelectList(ContraVoucherEntryManager.GetAllCostCentres(contra.SectorId.Value).ToList(), "Id", "CostCentreName", contra.CostCentreId);                                     
                ViewBag.SectorId = new SelectList(ContraVoucherEntryManager.GetAllCompanySector().AsEnumerable(), "Id", "SectorName", contra.SectorId);
            }
          
            if (Request.IsAjaxRequest())
            {
                return PartialView(model);
            }
            return View(model);
        }

        private string GetBalance(Acc_ClosingBalanceViewModel Obj)
        {
            string ClosingBalance = ContraVoucherEntryManager.GetClosingBalance(Obj);
            return ClosingBalance;
        }

        [AjaxAuthorize(Roles = "contravoucher-1, contravoucher-2,contravoucher-3")]
        public JsonResult GetContraVoucherDetail(long Id)
        {
            var count = 1;
            decimal totalDebit = 0;
            decimal totalCredit = 0;
            decimal totalbalance = 0;
            var temp = ContraVoucherEntryManager.GetContraVoucherDetail(Id);

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
            return Json(new { data = lt, tDr = totalDebit, tCr = totalCredit, tbc = totalbalance, Success = true }, JsonRequestBehavior.AllowGet);              
        }

        [AjaxAuthorize(Roles = "contravoucher-2,contravoucher-3")]
        public ActionResult SaveMaster(Acc_VoucherMaster voucherMaster)
        {
            var voucher = ContraVoucherEntryManager.GetAccVoucherMasterById(voucherMaster.Id) ?? new Acc_VoucherMaster();

            voucher.VoucherType = voucherMaster.VoucherType;
            voucher.VoucherNo = voucherMaster.VoucherNo;
            voucher.VoucherDate = voucherMaster.VoucherDate;
            voucher.Particulars = voucherMaster.Particulars;
            voucher.SectorId = voucherMaster.SectorId;
            voucher.CostCentreId = voucherMaster.CostCentreId;
            voucher.CheckDate = voucherMaster.CheckDate;
            voucher.CheckNo = voucherMaster.CheckNo;
            voucher.FinancialPeriodId = voucherMaster.FinancialPeriodId;
            voucher.FinancialPeriodId = ContraVoucherEntryManager.GetPeriodId();
            voucher.IsActive = true;

            if (voucherMaster.Id == 0)
            {
                voucher.VoucherNo = ContraVoucherEntryManager.GetVoucherNo(ContraVoucherEntryManager.GetPeriodId(),
                    voucherMaster.VoucherType);
            }

            ContraVoucherEntryManager.SaveMaster(voucher);

            return Json(new {Success = false, Errors = "Delete successfully !"});
        }

        [AjaxAuthorize(Roles = "contravoucher-2,contravoucher-3")]
        public ActionResult SaveDetail(Acc_VoucherDetail voucherDetail)
        {
            ContraVoucherEntryManager.SaveDetail(voucherDetail);
            return Json(new {Success = false, Errors = "Delete successfully !"});
        }

        [AjaxAuthorize(Roles = "contravoucher-1,contravoucher-2,contravoucher-3")]
        public ActionResult GetBankAccountNames(string term)
        {
            List<string> tags = ContraVoucherEntryManager.GetAccountNames("BANK");

            return this.Json(tags.Where(t => t.Substring(11, t.Length - 11).StartsWith(term.ToLower())),
                JsonRequestBehavior.AllowGet);
        }

        [AjaxAuthorize(Roles = "contravoucher-1,contravoucher-2,contravoucher-3")]
        public ActionResult GetCashAccountNames(string term)
        {
            List<string> tags = ContraVoucherEntryManager.GetAccountNames("CASH");

            return this.Json(tags.Where(t => t.Substring(11, t.Length - 11).StartsWith(term.ToLower())),
                JsonRequestBehavior.AllowGet);
        }

        [AjaxAuthorize(Roles = "contravoucher-1,contravoucher-2,contravoucher-3")]
        public JsonResult GetCostCentreBySector(int sectorId)
        {
            var costCentre = ContraVoucherEntryManager.GetAllCostCentres(sectorId);
            return Json(costCentre, JsonRequestBehavior.AllowGet);
        }

        [AjaxAuthorize(Roles = "contravoucher-1,contravoucher-2,contravoucher-3")]
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

        [AjaxAuthorize(Roles = "contravoucher-1,contravoucher-2,contravoucher-3")]
        public ActionResult CheckGLHead(string glHead)
        {
            var value = ContraVoucherEntryManager.CheckGLHeadValidation(glHead);
            return Json(new {data = value, Success = true});
        }

        [AjaxAuthorize(Roles = "contravoucher-1,contravoucher-2,contravoucher-3")]
        public ActionResult GetAccountId(decimal accountCode)
        {
            string value = ContraVoucherEntryManager.GetAccountId(accountCode).ToString();
            return Json(new {data = value, Success = true});
        }

        //public ActionResult CheckVoucherLimit(string VoucherType, string Amount)
        //{
        //    bool result = ContraVoucherEntryManager.CheckVoucherLimit(VoucherType, Amount);
        //    return Json(new {data = result, Success = true});
        //}

        public ActionResult GetCashInHandBalance(Acc_ClosingBalanceViewModel Obj)
        {
            Obj.FpId = ContraVoucherEntryManager.GetPeriodId();
            string status = ContraVoucherEntryManager.GetClosingBalance(Obj);
            return Json(new { Success = true, Data = status });
        }
    }
}
