using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SCERP.BLL.IManager.IAccountingManager;
using SCERP.BLL.Manager.AccountingManager;
using SCERP.Common;
using SCERP.Model;
using SCERP.Web.Areas.Accounting.Models.ViewModels;
using System.IO;
using SCERP.Web.Controllers;


namespace SCERP.Web.Areas.Accounting.Controllers
{
    public class BankReconcilationController : BaseAccountingController
    {
        public const int PageSize = 10;
      
        [AjaxAuthorize(Roles = "bankreconcilation-1,bankreconcilation-2,bankreconcilation-3")]
        public ActionResult Index(int? page, string sort, Model.VoucherListViewModel model, int? fpId,
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

            model.VoucherLists = BankReconcilationManager.GetAllVoucherList(startPage, records, sort,
                fpId, FromDate, ToDate, AccountName, SectorId).ToList() ?? new List<VoucherList>();

            model.TotalRecords = model.VoucherLists.Count;
            return View(model);
        }

        [AjaxAuthorize(Roles = "bankreconcilation-2,bankreconcilation-3")]
        public ActionResult Edit(Acc_BankVoucherManual model)
        {
            ModelState.Clear();

            ViewBag.Type =
                new SelectList(
                    new[] { new { Id = "Receipt", Value = "Receipt" }, new { Id = "Payment", Value = "Payment" } }, "Id",
                    "Value");

            if (model.Id == 0) return Request.IsAjaxRequest() ? (ActionResult)PartialView(model) : View(model);

            return Request.IsAjaxRequest() ? (ActionResult)PartialView(model) : View(model);
        }

        [AjaxAuthorize(Roles = "bankreconcilation-2,bankreconcilation-3")]
        public ActionResult SaveBankReconMaster(string sectorId, string fpId, string accountName, DateTime? fromDate,
            DateTime? toDate)
        {
            var result = BankReconcilationManager.SaveBankReconMaster(sectorId, fpId, accountName, fromDate, toDate);

            if (result == "Success")
                return CreateJsonResult(new { Success = true, Reload = true });
            else
                return CreateJsonResult(new { Success = false, Reload = true });
        }

        [AjaxAuthorize(Roles = "bankreconcilation-2,bankreconcilation-3")]
        public ActionResult SaveBankReconDetail(List<long> detail)
        {
            var result = BankReconcilationManager.SaveBankReconDetail(detail);

            if (result == "Error")
                return CreateJsonResult(new { Success = false, Reload = true, voucherId = result });
            else
                return CreateJsonResult(new { Success = true, Reload = true, voucherId = result });
        }

        [AjaxAuthorize(Roles = "bankreconcilation-2,bankreconcilation-3")]
        public ActionResult SaveBankManual(Acc_BankVoucherManual voucherManual)
        {
            var result = BankReconcilationManager.SaveManualDetail(voucherManual);

            if (result == "Success")
                return Json(new { Success = true, Reload = true });
            else
                return Json(new { Success = false, Reload = true });
        }

        public ActionResult CheckFromDateToDate(int Id, DateTime fromDate, DateTime toDate)
        {
            var result = BankReconcilationManager.CheckPeriodFromToDate(Id, fromDate, toDate);
            return Json(new { Success = true, Result = result });
        }

        public ActionResult GetPeriodFromToDate(int Id)
        {
            var result = BankReconcilationManager.GetPeriodFromToDate(Id);

            string fromDate = result[0].ToString("dd/MM/yyyy");
            string toDate = result[1].ToString("dd/MM/yyyy");

            return Json(new { Success = true, FromDate = fromDate, ToDate = toDate });
        }

        [AjaxAuthorize(Roles = "bankreconcilation-1,bankreconcilation-2,bankreconcilation-3")]
        public ActionResult TagSearch(string term)
        {
            List<string> tags = BankReconcilationManager.GetAccountNames();

            return this.Json(tags.Where(t => t.Substring(11, t.Length - 11).StartsWith(term.ToLower())),
                JsonRequestBehavior.AllowGet);
        }

        public ActionResult CheckDuplicate(List<string> voucherId)
        {
            var result = BankReconcilationManager.CheckDuplicate(voucherId);

            if (result == "Success")
                return Json(new { Success = true, Reload = true });
            else
                return Json(new { Success = false, Reload = true });
        }
    }
}