using SCERP.Common;
using SCERP.Model.Custom;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SCERP.Web.Areas.HRM.Controllers
{
    public class EmployeeInOutEditController : BaseHrmController
    {
        public ActionResult Index(EmployeeInOutEditView model)
        {
            ModelState.Clear();

            ViewBag.InOutName = new SelectList(new[] {new {Id = "EmployeeInOut_10PM", Value = "EmployeeInOut_10PM"}, new {Id = "EmployeeInOut_10PM_NoWeekend", Value = "EmployeeInOut_10PM_NoWeekend"}}, "Id", "Value", model.InOutName);

            EmployeeInOutEditView edit = HrmReportManager.GetEmployeeInOutInfo(model.InOutName, model.EmployeeCardId, model.TransactionDate);

            if (edit != null)
            {
                model = edit;
            }
            else
            {
                model.TransactionDate = DateTime.Now;
            }

            ViewBag.Status = new SelectList(new[] {new {Id = "Present", Value = "Present"}, new {Id = "Absent", Value = "Absent"}, new {Id = "Holiday", Value = "Holiday"}, new {Id = "Weekend", Value = "Weekend"}, new {Id = "Late", Value = "Late"}, new {Id = "Leave", Value = "Leave"}, new {Id = "OSD", Value = "OSD"}}, "Id", "Value", model.Status);

            return View("~/Areas/HRM/Views/EmployeeInOutEdit/Index.cshtml", model);
        }

        public ActionResult Save(EmployeeInOutEditView model)
        {
            int result = 0;

            try
            {
                result = HrmReportManager.EditEmployeeInOut(model);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return (result > 0) ? Reload() : ErrorResult("Failed to save data");
        }
    }
}