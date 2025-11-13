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
using System.Collections;
using System.Linq;

namespace SCERP.Web.Areas.HRM.Controllers
{
    public class EmployeeAppointmentController : BaseHrmController
    {
        private readonly int _pageSize = AppConfig.PageSize;

        [AjaxAuthorize(Roles = "appointmentletter-1,appointmentletter-2,appointmentletter-3")]
        public ActionResult Index(ReportSearchViewModel model)
        {
            ModelState.Clear();

            try
            {
                              
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return View(model);
        }       

        [AjaxAuthorize(Roles = "appointmentletter-1,appointmentletter-2,appointmentletter-3")]
        public ActionResult Search(ReportSearchViewModel searchModel)
        {
            ModelState.Clear();
            var model = new EmployeeAppointmentViewModel();

            Guid EmployeeId = new Guid();
            Employee employee = new Employee();

            if (!string.IsNullOrEmpty(searchModel.SearchByEmployeeCardId))
                employee = EmployeeManager.GetEmployeeByCardId(searchModel.SearchByEmployeeCardId);

            if (employee == null)
                return ErrorResult("Invalid ID or Access Denied!");

            EmployeeId = employee.EmployeeId;

            string userName = PortalContext.CurrentUser.Name;

            if (searchModel.Date != null)
            {
                DateTime prepareDate = searchModel.Date.Value;
                model.AppointmentLetterInfo = EmployeeAppointmentManager.GetEmployeeAppointmentInfo(EmployeeId, userName, prepareDate);
            }
            return PartialView("_AppointmentLetterPdf", model);
        }          


        [AjaxAuthorize(Roles = "appointmentletter-1,appointmentletter-2,appointmentletter-3")]
        public ActionResult IndexNew(ReportSearchViewModel model)
        {
            ModelState.Clear();

            try
            {
                IEnumerable employeeType = from EmployeeTypeReport empType in Enum.GetValues(typeof(EmployeeTypeReport))
                                           select new { Id = (int)empType, Title = empType.ToString() };

                model.EmployeeTypes = employeeType;
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return View(model);
        }

        [AjaxAuthorize(Roles = "appointmentletter-1,appointmentletter-2,appointmentletter-3")]
        public ActionResult SearchNew(ReportSearchViewModel searchModel)
        {
            ModelState.Clear();
            var model = new EmployeeAppointmentViewModel();

            Guid EmployeeId = new Guid();
            Employee employee = new Employee();

            if (!string.IsNullOrEmpty(searchModel.SearchByEmployeeCardId))
                employee = EmployeeManager.GetEmployeeByCardId(searchModel.SearchByEmployeeCardId);

            if (employee == null)
                return ErrorResult("Invalid ID or Access Denied!");

            EmployeeId = employee.EmployeeId;

            string userName = PortalContext.CurrentUser.Name;

            if (searchModel.Date != null)
            {
                DateTime prepareDate = searchModel.Date.Value;

                if (searchModel.SearchFieldModel.SearchByEmployeeTypeId == 1)
                    model.AppointmentLetterInfo = EmployeeAppointmentManager.GetEmployeeAppointmentInfoNew(EmployeeId, userName, prepareDate);
                else if (searchModel.SearchFieldModel.SearchByEmployeeTypeId == 2)
                    model.AppointmentLetterInfo = EmployeeAppointmentManager.GetEmployeeAppointmentInfoStaffNew(EmployeeId, userName, prepareDate);
            }
            return PartialView("_AppointmentLetterPdf", model);
        }

        public ActionResult GetAllEmployeeById(Guid Id)
        {
            var employees = EmployeeManager.GetEmployeeById(Id);
            return Json(new { Success = true, EmployeesList = employees }, JsonRequestBehavior.AllowGet);
        }
    }
}