using System;
using System.Linq;
using System.Web.Mvc;
using SCERP.Common;
using SCERP.Model;

namespace SCERP.Web.Areas.Accounting.Controllers
{
    public class VoucherListController : BaseAccountingController
    {
        public const int PageSize = 10;

        [AjaxAuthorize(Roles = "voucherlist-1,voucherlist-2,voucherlist-3")]
        public ActionResult Index(int? page, string sort, Model.VoucherListViewModel model, int? fpId, DateTime? FromDate, DateTime? ToDate, string VoucherTypeId, string VoucherNo)
        {
            var startPage = 0;
            var records = 10;

            if (page.HasValue && page.Value > 0)
            {
                startPage = page.Value - 1;
            }

            ViewBag.FpId = new SelectList(VoucherListManager.GetFinancialPeriod().AsEnumerable(), "Id", "PeriodName");

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
                    "Id", "Value");

            model.VoucherLists = VoucherListManager.GetAllVoucherList(startPage, records, sort, fpId, FromDate, ToDate, VoucherTypeId, VoucherNo).ToList();              
            model.TotalRecords = model.VoucherLists.Count;
            return View(model);
        }

        [AjaxAuthorize(Roles = "voucherlist-2,voucherlist-3")]
        public ActionResult Edit(Acc_VoucherMaster model)
        {
            ModelState.Clear();

            if (model.Id != 0)
            {
                var voucher = VoucherListManager.GetVoucherMasterById(model.Id);

                return RedirectToAction("Edit", "CommonVoucherEntry", new {Id = voucher.Id});
            }
            return View(model);
        }

        [AjaxAuthorize(Roles = "voucherlist-1,voucherlist-2,voucherlist-3")]
        public ActionResult CheckFromDateToDate(int Id, DateTime fromDate, DateTime toDate)
        {
            var result = VoucherListManager.CheckPeriodFromToDate(Id, fromDate, toDate);
            return Json(new {Success = true, Result = result});
        }

        [AjaxAuthorize(Roles = "voucherlist-1,voucherlist-2,voucherlist-3")]
        public ActionResult GetPeriodFromToDate(int Id)
        {
            var result = VoucherListManager.GetPeriodFromToDate(Id);

            string fromDate = result[0].ToString("dd/MM/yyyy");
            string toDate = result[1].ToString("dd/MM/yyyy");

            return Json(new {Success = true, FromDate = fromDate, ToDate = toDate});
        }

        public ActionResult GetPeriodFromToDateBalanceSheet(int Id)
        {
            var result = VoucherListManager.GetPeriodFromToDate(Id);
            DateTime fromDatePrevious = result[0].AddDays(-1);

            string fromDate = fromDatePrevious.ToString("dd/MM/yyyy");
            string toDate = result[1].ToString("dd/MM/yyyy");

            return Json(new { Success = true, FromDate = fromDate, ToDate = toDate });
        }

        [AjaxAuthorize(Roles = "voucherlist-3")]
        public ActionResult Delete(int id)
        {
            var voucherMaster = VoucherListManager.GetVoucherMasterById(id) ?? new Acc_VoucherMaster();
            VoucherListManager.DeleteVoucherList(voucherMaster);
            return Reload();
        }
    }
}