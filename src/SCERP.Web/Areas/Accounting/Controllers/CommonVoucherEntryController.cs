using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using SCERP.Model;
using SCERP.Web.Areas.Accounting.Models.ViewModels;

namespace SCERP.Web.Areas.Accounting.Controllers
{
    public class CommonVoucherEntryController : BaseAccountingController
    {
        public ActionResult Edit(JournalVoucherEntryViewModel model)
        {
            ModelState.Clear();

            ViewBag.VoucherTypeId =
                new SelectList(
                    new[]
                    {
                        new {Id = "JV", Value = "Journal Voucher"},
                        new {Id = "CP", Value = "Cash Payment"},
                        new {Id = "CR", Value = "Cash Receipt"},
                        new {Id = "BP", Value = "Bank Payment"},
                        new {Id = "BR", Value = "Bank Receipt"},
                        new {Id = "CV", Value = "Contra Voucher"}
                    },
                    "Id", "Value", "CP");

            model.VoucherDate = DateTime.Now;
            model.VoucherNo = JournalVoucherEntryManager.GetLatestVoucherNo();

            ViewBag.SectorId = new SelectList(JournalVoucherEntryManager.GetAllCompanySector().AsEnumerable(), "Id", "SectorName", 1);
            ViewBag.CostCentreId = new SelectList(JournalVoucherEntryManager.GetAllCostCentres(1).ToList(), "Id", "CostCentreName", 11);

            if (model.Id > 0)
            {
                var journal = JournalVoucherEntryManager.GetAccVoucherMasterById(model.Id);

                ViewBag.VoucherTypeId =
                    new SelectList(
                        new[]
                        {
                            new {Id = "JV", Value = "Journal Voucher"},
                            new {Id = "CP", Value = "Cash Payment"},
                            new {Id = "CR", Value = "Cash Receipt"},
                            new {Id = "BP", Value = "Bank Payment"},
                            new {Id = "BR", Value = "Bank Receipt"},
                            new {Id = "CV", Value = "Contra Voucher"}
                        },
                        "Id", "Value", journal.VoucherType);

                model.VoucherDate = journal.VoucherDate;
                model.Particulars = journal.Particulars;
                model.SectorId = journal.SectorId;
                model.VoucherRefNo = journal.VoucherRefNo;
                model.FinancialPeriodId = Convert.ToInt32(JournalVoucherEntryManager.GetFinancialPeriodById(journal.FinancialPeriodId.Value));

                ViewBag.CostCentreId = new SelectList(JournalVoucherEntryManager.GetAllCostCentres(journal.SectorId.Value).ToList(), "Id", "CostCentreName", journal.CostCentreId);
                ViewBag.SectorId = new SelectList(JournalVoucherEntryManager.GetAllCompanySector().AsEnumerable(), "Id", "SectorName", model.SectorId);
            }            

            if (Request.IsAjaxRequest())
            {
                return PartialView(model);
            }

            return View(model);
        }


        public ActionResult SaveMaster(Acc_VoucherMaster voucherMaster)
        {
            var voucher = JournalVoucherEntryManager.GetAccVoucherMasterById(voucherMaster.Id) ?? new Acc_VoucherMaster();

            voucher.VoucherType = voucherMaster.VoucherType;
            voucher.VoucherNo = voucherMaster.VoucherNo;
            voucher.VoucherDate = voucherMaster.VoucherDate;
            voucher.Particulars = voucherMaster.Particulars;
            voucher.SectorId = voucherMaster.SectorId;
            voucher.CostCentreId = voucherMaster.CostCentreId;      
            voucher.FinancialPeriodId = JournalVoucherEntryManager.GetPeriodId();
            voucher.VoucherRefNo = voucherMaster.VoucherRefNo;
            voucher.TotalAmountInWord = voucherMaster.TotalAmountInWord;
            voucher.IsActive = true;

            if (voucherMaster.Id == 0)
            {
                voucher.VoucherNo = JournalVoucherEntryManager.GetLatestVoucherNo();
            }

            JournalVoucherEntryManager.SaveMaster(voucher);

            return Json(new { Success = false, Errors = "Delete successfully !" });
        }

        public ActionResult SaveDetail(Acc_VoucherDetail voucherDetail)
        {
            JournalVoucherEntryManager.SaveDetail(voucherDetail);
            return Json(new { Success = false, Errors = "Delete successfully !" });
        }

        public JsonResult GetGlVoucherDetail(long Id)
        {
            var count = 1;
            decimal totalDebit = 0;
            decimal totalCredit = 0;
            decimal totalbalance = 0;
            var temp = JournalVoucherEntryManager.GetGlVoucherDetail(Id);
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

        public ActionResult TagSearch(string term)
        {
            List<string> tags = JournalVoucherEntryManager.GetAccountName();
            return this.Json(tags.Where(t => t.Substring(0, t.Length - 11).Contains(term.ToLower())).Skip(0).Take(10), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCostCentreBySector(int sectorId)
        {
            var costCentre = JournalVoucherEntryManager.GetAllCostCentres(sectorId);
            return Json(costCentre, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetInWords(string amount)
        {
            var inWords = Spell.SpellAmount.InWrods(Convert.ToDecimal(amount));
            return Json(new { data = inWords, Success = true });
        }

        public ActionResult CheckGLHead(string glHead)
        {
            var value = JournalVoucherEntryManager.CheckGLHeadValidation(glHead);
            return Json(new { data = value, Success = true });
        }

        public ActionResult GetAccountId(decimal accountCode)
        {
            string value = JournalVoucherEntryManager.GetAccountId(accountCode).ToString();
            return Json(new { data = value, Success = true });
        }
    }
}