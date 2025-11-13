using System;
using System.Collections.Generic;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using SCERP.BLL.Manager.HRMManager;
using SCERP.Common;
using SCERP.Common.ReportHelper;
using SCERP.Web.Areas.HRM.Models.ViewModels;
using SCERP.Model;
using SCERP.Web.Controllers;

namespace SCERP.Web.Areas.HRM.Controllers
{
    public class SalaryIncrementLetterController : BaseHrmController
    {
        public ActionResult Index(ReportSearchViewModel model)
        {
            ModelState.Clear();

            try
            {
                var dateToday =DateTime.Now;
                var year = dateToday.Year;
                var month = dateToday.Month;

                
                //dateTime = dateTime.AddMonths(-1);
                model.FromDate= new DateTime(year, month, 26).AddMonths(-1);
                model.ToDate= new DateTime(year, month, 25);

            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return View(model);
        }

        public ActionResult Search(ReportSearchViewModel searchModel)
        {
            ModelState.Clear();

            var model = new EmployeeAppointmentViewModel();

            Employee employee = new Employee();

            if (!string.IsNullOrEmpty(searchModel.SearchByEmployeeCardId))
                employee = EmployeeManager.GetEmployeeByCardId(searchModel.SearchByEmployeeCardId);

            if (employee == null)
                return ErrorResult("Invalid ID or Access Denied!");

            string userName = PortalContext.CurrentUser.Name;

            if (searchModel.FromDate != null && searchModel.ToDate != null)
            {
                DateTime fromDate = searchModel.FromDate.Value;
                DateTime toDate = searchModel.ToDate.Value;

                model.AppointmentLetterInfo = SalaryIncrementManager.GetSalaryIncrementInfo(fromDate, toDate, searchModel.SearchByEmployeeCardId, userName);
            }
            return PartialView("_SalaryIncrementLetterPdf", model);
        }
    }
}