using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using SCERP.Common;
using SCERP.Model;
using SCERP.Web.Controllers;


namespace SCERP.Web.Areas.Accounting.Controllers
{
    public class BankReconcilationListController : BaseAccountingController
    {
        public const int PageSize = 10;

        [AjaxAuthorize(Roles = "bankreconcilationlist-1,bankreconcilationlist-2,bankreconcilationlist-3")]
        public ActionResult Index(int? page, string sort, Model.Acc_BankReconcilationListViewModel model, int? fpId,
            DateTime? FromDate, DateTime? ToDate, string AccountName, string SectorId)
        {
            var startPage = 0;
            var records = 10;

            if (page.HasValue && page.Value > 0)
            {
                startPage = page.Value - 1;
            }

            ViewBag.FpId = new SelectList(BankReconcilationManager.GetFinancialPeriod().AsEnumerable(), "Id",
                "PeriodName");

            ViewBag.SectorId = new SelectList(BankReconcilationManager.GetAllCompanySector().AsEnumerable(), "Id",
                "SectorName");

            model.BankReconcilationLists = BankReconcilationListManager.GetAllReconcilationList(startPage, records, sort,
                fpId, FromDate, ToDate, AccountName, SectorId).ToList() ?? new List<Acc_BankReconcilationList>();

            model.TotalRecords = model.BankReconcilationLists.Count;
            return View(model);
        }

        [AjaxAuthorize(Roles = "bankreconcilationlist-1,bankreconcilationlist-2,bankreconcilationlist-3")]
        public ActionResult TagSearch(string term)
        {
            var tags = BankReconcilationManager.GetAccountNames();

            return this.Json(tags.Where(t => t.Substring(11, t.Length - 11).StartsWith(term.ToLower())),
                JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetPeriodFromToDate(int Id)
        {
            var result = VoucherListManager.GetPeriodFromToDate(Id);

            string fromDate = result[0].ToString("dd/MM/yyyy");
            string toDate = result[1].ToString("dd/MM/yyyy");

            return Json(new {Success = true, FromDate = fromDate, ToDate = toDate});
        }

        [AjaxAuthorize(Roles = "bankreconcilationlist-3")]
        public ActionResult Delete(int id)
        {
            var bankReconcilMaster = BankReconcilationListManager.GetBankReconcilationMasterById(id) ??
                                     new Acc_BankReconcilationMaster();
            BankReconcilationListManager.DeleteBankReconcilationMaster(bankReconcilMaster);
            return Reload();
        }

        [AjaxAuthorize(Roles = "bankreconcilationlist-2,bankreconcilationlist-3")]
        public ActionResult Edit(Acc_BankReconcilationMaster model)
        {
            ModelState.Clear();
            return RedirectToAction("Index", "BankReconcilation", new {Id = model.Id});
        }
    }
}