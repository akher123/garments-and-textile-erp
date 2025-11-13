using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using SCERP.Common;
using System.Globalization;
using SCERP.Web.Controllers;

namespace SCERP.Web.Areas.HRM.Controllers
{
    public class EmployeeManualAttendanceController : BaseHrmController
    {
        [AjaxAuthorize(Roles = "manualattendance-1,manualattendance-2,manualattendance-3")]
        public ActionResult Index()
        {
            var hours = new List<string>();
            var minutes = new List<string>();
            var period = new List<string>();
            hours.Add("Hour");
            minutes.Add("Minute");
            period.Add("AM/PM");
            for (var i = 0; i <= 12; i++)
            {
                if (i < 10)
                    hours.Add("0" + i.ToString(CultureInfo.InvariantCulture));
                else
                    hours.Add(i.ToString(CultureInfo.InvariantCulture));
            }

            for (var i = 0; i < 60; i++)
            {
                if (i < 10)
                    minutes.Add("0" + i.ToString(CultureInfo.InvariantCulture));
                else
                    minutes.Add(i.ToString(CultureInfo.InvariantCulture));
            }
            period.Add("AM");
            period.Add("PM");

            ViewBag.Hours = new SelectList(hours.AsEnumerable());
            ViewBag.minutes = new SelectList(minutes.AsEnumerable());
            ViewBag.period = new SelectList(period.AsEnumerable());
            return View();
        }

        [AjaxAuthorize(Roles = "manualattendance-1,manualattendance-2,manualattendance-3")]
        public ActionResult GetEmployeeData(string employeeCardId, string dt)
        {
            var value = true;
            var date = DateTime.ParseExact(dt, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            var empData = EmployeeManualAttendanceManager.GetEmployeeData(employeeCardId, date);
            if (empData == null)
            {
                value = false;
            }
            return Json(new { data = empData, Success = value });
        }

        [AjaxAuthorize(Roles = "manualattendance-2,manualattendance-3")]
        public ActionResult SaveEmployeeManualAttendance(List<string> Values)
        {
            return ErrorMessageResult();
        }
    }
}