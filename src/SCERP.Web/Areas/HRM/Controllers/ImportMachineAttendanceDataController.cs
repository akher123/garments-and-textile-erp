using SCERP.BLL.Manager.HRMManager;
using SCERP.Common;
using SCERP.Model.Custom;
using SCERP.Web.Areas.HRM.Models.ViewModels;
using SCERP.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SCERP.Web.Areas.HRM.Controllers
{
    public class ImportMachineAttendanceDataController : BaseHrmController
    {
        public ActionResult Index(EmployeeDailyAttendanceViewModel model)
        {
            ModelState.Clear();
            return View(model);
        }

        public ActionResult ImportMachineData(EmployeeDailyAttendanceViewModel model)
        {
            try
            {
                var fromDate =model.FromDate;
                var toDate = model.ToDate;
                var attendance = EmployeeDailyAttendanceManager.ImportMachineAttendanceData(fromDate, toDate);
                if (!attendance)
                    return ErrorResult("Failed to import data!");
            }
            catch (Exception exception)
            {
             
                Errorlog.WriteLog(exception);
                return ErrorResult(exception.ToString());
            }
            return ErrorResult("Data imported successfully");
        }
    }
}