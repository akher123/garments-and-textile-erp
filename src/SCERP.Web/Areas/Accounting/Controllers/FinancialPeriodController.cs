using System;
using System.Web.Mvc;
using SCERP.Common;
using SCERP.Model;
using SCERP.Web.Areas.Accounting.Models.ViewModels;


namespace SCERP.Web.Areas.Accounting.Controllers
{
    public class FinancialPeriodController : BaseAccountingController
    {
        public const int PageSize = 10;
        private Guid _employeeGuidId = Guid.Parse("7bca454d-187a-4885-9c65-07e4ef23ee8c");

        [AjaxAuthorize(Roles = "financialperiod-1,financialperiod-2,financialperiod-3")]
        public ActionResult Index(int? page, string sort, FinancialPeriodViewModel model)
        {
            var startPage = 0;

            if (page.HasValue && page.Value > 0)
            {
                startPage = page.Value - 1;
            }

            model.FinancialPeriod = FinancialPeriodManager.GetAllFinancialPeriods(startPage, PageSize, sort);
            model.TotalRecords = model.FinancialPeriod.Count;
            return View(model);
        }

        [AjaxAuthorize(Roles = "financialperiod-2,financialperiod-3")]
        public ActionResult Edit(Acc_FinancialPeriod model)
        {
            ModelState.Clear();

            if (model.Id != 0)
            {
                var sectorManager = FinancialPeriodManager.GetFinancialPeriodById(model.Id);

                model.PeriodName = sectorManager.PeriodName;
                model.PeriodStartDate = sectorManager.PeriodStartDate;
                model.PeriodEndDate = sectorManager.PeriodEndDate;
                model.SortOrder = sectorManager.SortOrder;

                ViewBag.ActiveStatus =
                    new SelectList(new[] {new {Id = true, Value = "Active"}, new {Id = false, Value = "Deactive"}}, "Id",
                        "Value", sectorManager.ActiveStatus);
            }
            else
            {
                model.PeriodStartDate = DateTime.Now;
                model.PeriodEndDate = DateTime.Now;

                ViewBag.ActiveStatus =
                    new SelectList(new[] {new {Id = true, Value = "Active"}, new {Id = false, Value = "Deactive"}}, "Id",
                        "Value");
            }

            if (Request.IsAjaxRequest())
            {
                return PartialView(model);
            }
            return View(model);
        }

        [AjaxAuthorize(Roles = "financialperiod-2,financialperiod-3")]
        public ActionResult Save(FinancialPeriodViewModel model)
        {
            if (Session["EmployeeGuid"] != null)
            {
                _employeeGuidId = (Guid) Session["EmployeeGuid"];
            }

            var financialPeriod = FinancialPeriodManager.GetFinancialPeriodById(model.Id) ?? new Acc_FinancialPeriod();

            financialPeriod.PeriodName = model.PeriodName;
            financialPeriod.PeriodStartDate = model.PeriodStartDate;
            financialPeriod.PeriodEndDate = model.PeriodEndDate;
            financialPeriod.SortOrder = model.SortOrder;
            financialPeriod.ActiveStatus = model.ActiveStatus;

            if (model.Id > 0)
            {
                financialPeriod.EDT = DateTime.Now;
                financialPeriod.EditedBy = PortalContext.CurrentUser.UserId;
            }
            else
            {
                financialPeriod.CDT = DateTime.Now;
                financialPeriod.CreatedBy = PortalContext.CurrentUser.UserId;
            }

            string message = "";

            var i = FinancialPeriodManager.SaveFinancialPeriod(financialPeriod);

            if (i == 2)
                message = "Duplicate Period Name Exists!";

            if (i == 3)
                message = "Start Date can not be greater than End date !";

            if (i == 4)
                message = "Start Date can not be within an existing period !";

            else if (i == 0)
                message = "Database error has occured !";

            else if (i == 1)
            {
                return Reload();
            }

            return CreateJsonResult(new {Success = false, Reload = true, Message = message});
        }

        [AjaxAuthorize(Roles = "financialperiod-3")]
        public ActionResult Delete(int id)
        {
            var financialPeriod = FinancialPeriodManager.GetFinancialPeriodById(id) ?? new Acc_FinancialPeriod();
            FinancialPeriodManager.DeleteFinancialPeriod(financialPeriod);
            return Reload();
        }
    }
}
