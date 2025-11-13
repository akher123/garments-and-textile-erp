using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using SCERP.Common;
using SCERP.Model.Custom;
using SCERP.Model;
using SCERP.BLL.Manager.HRMManager;
using SCERP.Web.Controllers;


namespace SCERP.Web.Areas.HRM.Controllers
{
    public class ShortLeaveController : BaseHrmController
    {
        private readonly int _pageSize = AppConfig.PageSize;
        private Guid? _employeeGuidId = PortalContext.CurrentUser.UserId;

        [AjaxAuthorize(Roles = "gatepass-1,gatepass-2,gatepass-3")]
        public ActionResult Index(ShortLeaveModel model)
        {
            ModelState.Clear();

            if (!string.IsNullOrWhiteSpace(model.EmployeeCardId))
            {
                Employee emp = EmployeeManager.GetEmployeeByCardId(model.EmployeeCardId);
                if (emp != null)
                    model.EmployeeId = emp.EmployeeId;
            }

            ViewBag.Title = "Gate Pass";

            if (model.IsSearch)
            {
                model.IsSearch = false;
                return View(model);
            }

            var startPage = 0;
            if (model.page.HasValue && model.page.Value > 0)
            {
                startPage = model.page.Value - 1;
            }

            int totalRecords = 0;
            model.VEmployeeShortLeave = EmployeeShortLeaveManager.GetAllEmployeeShortLeavesByPaging(startPage, _pageSize, out totalRecords, model) ?? new List<VEmployeeShortLeave>();
            model.TotalRecords = totalRecords;

            return View(model);
        }

        [AjaxAuthorize(Roles = "gatepass-2,gatepass-3")]
        public ActionResult Edit(ShortLeaveModel model)
        {
            ModelState.Clear();

            try
            {
                var reasonList = from ReasonType reasonType in Enum.GetValues(typeof(ReasonType))
                                 select new { Id = (int)reasonType, Name = reasonType.ToString() };

                model.Date = DateTime.Now;
                ViewBag.Title = "new";
                ViewBag.ReasonType = new SelectList(reasonList, "Id", "Name");

                if (model.Id > 0)
                {
                    var shortLeave = EmployeeShortLeaveManager.GetEmployeeShortLeaveById(model.Id) ?? new EmployeeShortLeave();

                    var employee = EmployeeManager.GetEmployeeById(shortLeave.EmployeeId) ?? new Employee();
                    model.EmployeeCardId = employee.EmployeeCardId;
                    model.ReasonDescription = shortLeave.ReasonDescription;
                    model.Date = shortLeave.Date;

                    DateTime dt = DateTime.Today;
                    dt = dt.Add(shortLeave.FromTime);

                    model.FromHourKey = dt.ToString("hh").Length < 2 ? "0" + dt.ToString("hh") : dt.ToString("hh");
                    model.FromMinuteKey = Convert.ToString(shortLeave.FromTime.Minutes).Length < 2 ? "0" + Convert.ToString(shortLeave.FromTime.Minutes) : Convert.ToString(shortLeave.FromTime.Minutes);
                    model.FromPeriodKey = Convert.ToString(dt.ToString("tt"));

                    dt = DateTime.Today;
                    dt = dt.Add(shortLeave.ToTime);

                    model.ToHourKey = dt.ToString("hh").Length < 2 ? "0" + dt.ToString("hh") : dt.ToString("hh");
                    model.ToMinuteKey = Convert.ToString(shortLeave.ToTime.Minutes).Length < 2 ? "0" + Convert.ToString(shortLeave.ToTime.Minutes) : Convert.ToString(shortLeave.ToTime.Minutes);
                    model.ToPeriodKey = Convert.ToString(dt.ToString("tt"));

                    model.TotalHours = shortLeave.TotalHours;

                    ViewBag.ReasonType = new SelectList(reasonList, "Id", "Name", shortLeave.ReasonType);
                    ViewBag.Title = "Edit";
                }
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return View(model);
        }

        [AjaxAuthorize(Roles = "gatepass-2,gatepass-3")]
        public ActionResult Save(ShortLeaveModel model)
        {
            var saveIndex = 0;

            var checkEmployeeCardId = EmployeeManager.CheckExistingEmployeeCardNumber(new Employee() { EmployeeCardId = model.EmployeeCardId });
            if (!checkEmployeeCardId)
            {
                return ErrorResult("Invalid Id or Access denied!");
            }
            var employee = EmployeeManager.GetEmployeeByCardId(model.EmployeeCardId) ?? new Employee();
            if ((employee.EmployeeId != null) || (employee.EmployeeId != Guid.Parse("00000000-0000-0000-0000-000000000000")))
                model.EmployeeId = employee.EmployeeId;

            var shortLeave = EmployeeShortLeaveManager.GetEmployeeShortLeaveById(model.Id) ?? new EmployeeShortLeave();
            shortLeave.EmployeeId = model.EmployeeId;
            shortLeave.ReasonType = model.ReasonType;
            shortLeave.ReasonDescription = model.ReasonDescription;
            if (model.Date != null) shortLeave.Date = (DateTime)model.Date;
            shortLeave.FromTime = DateTimeExtension.GetHHTime(model.FromHourKey, model.FromMinuteKey, model.FromPeriodKey);
            shortLeave.ToTime = DateTimeExtension.GetHHTime(model.ToHourKey, model.ToMinuteKey, model.ToPeriodKey);
            shortLeave.TotalHours = shortLeave.ToTime.Subtract(shortLeave.FromTime);

            TimeSpan? timespan = new TimeSpan(0, 0, 0);
            if (shortLeave.TotalHours.Value <= timespan)
                return (saveIndex > 0) ? Reload() : ErrorResult("Please Select valid From Time and To Time");

            shortLeave.CreatedBy = _employeeGuidId;

            saveIndex = (model.Id > 0) ? EmployeeShortLeaveManager.EditShortLeave(shortLeave) : EmployeeShortLeaveManager.SaveShortLeave(shortLeave);
            if(saveIndex == 2)
                return ErrorResult("Employee already got gate pass in this date and time limit !");

            return (saveIndex > 0) ? Reload() : ErrorResult("Failed to save data");
        }

        [AjaxAuthorize(Roles = "gatepass-3")]
        public ActionResult Delete(int id)
        {
            var deleted = 0;
            var shortleave = EmployeeShortLeaveManager.GetEmployeeShortLeaveById(id) ?? new EmployeeShortLeave();
            deleted = EmployeeShortLeaveManager.DeleteShortLeave(shortleave);
            return (deleted > 0) ? Reload() : ErrorResult("Failed to delete data");
        }

        public ActionResult GetEmployeeDetailByEmployeeCadId(string employeeCardId)
        {

            var checkEmployeeCardId =
                EmployeeManager.CheckExistingEmployeeCardNumber(new Employee() { EmployeeCardId = employeeCardId });
            if (!checkEmployeeCardId)
            {
                return Json(new { ValidStatus = false }, JsonRequestBehavior.AllowGet);
            }
            var employeeDetails = EmployeeDailyAttendanceManager.GetEmployeeByEmployeeCardId(employeeCardId);

            return employeeDetails == null ? Json(new { Sucess = "false" }) : Json(new { data = employeeDetails, Success = true });
        }

    }
}