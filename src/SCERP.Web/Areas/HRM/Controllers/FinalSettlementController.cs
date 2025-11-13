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
    public class FinalSettlementController : BaseHrmController
    {

        private readonly int _pageSize = AppConfig.PageSize;

        [AjaxAuthorize(Roles = "finalSettlement-1,finalSettlement-2,finalSettlement-3")]
        public ActionResult Index(ReportSearchViewModel model)
        {
            ModelState.Clear();

            try
            {
                model.OthersDeduction = 0;
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return View(model);
        }

        [AjaxAuthorize(Roles = "finalSettlement-1,finalSettlement-2,finalSettlement-3")]
        public ActionResult Search(ReportSearchViewModel searchModel)
        {
            ModelState.Clear();

            Guid EmployeeId = new Guid();
            Employee employee = new Employee();
            var model = new EmployeeAppointmentViewModel();

            if (!string.IsNullOrEmpty(searchModel.SearchByEmployeeCardId))
                employee = EmployeeManager.GetAnyEmployeeByCardId(searchModel.SearchByEmployeeCardId);

            if (employee == null)
                return ErrorResult("Invalid ID or Access Denied!");

            EmployeeId = employee.EmployeeId;

            string userName = PortalContext.CurrentUser.Name;

            if (searchModel.Date != null)
            {
                DateTime prepareDate = searchModel.Date.Value;
                model.AppointmentLetterInfo = EmployeeAppointmentManager.GetFinalSettlementInfo(EmployeeId, userName, prepareDate, searchModel.OthersDeduction);
            }
            return PartialView("_FinalSettlement", model);
        }

        public ActionResult Index08PM(ReportSearchViewModel model)
        {
            ModelState.Clear();

            try
            {
                model.OthersDeduction = 0;
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return View(model);
        }

        public ActionResult Search08PM(ReportSearchViewModel searchModel)
        {
            ModelState.Clear();

            Guid EmployeeId = new Guid();
            Employee employee = new Employee();
            var model = new EmployeeAppointmentViewModel();

            if (!string.IsNullOrEmpty(searchModel.SearchByEmployeeCardId))
                employee = EmployeeManager.GetAnyEmployeeByCardId(searchModel.SearchByEmployeeCardId);

            if (employee == null)
                return ErrorResult("Invalid ID or Access Denied!");

            EmployeeId = employee.EmployeeId;

            string userName = PortalContext.CurrentUser.Name;

            if (searchModel.Date != null)
            {
                DateTime prepareDate = searchModel.Date.Value;
                model.AppointmentLetterInfo = EmployeeAppointmentManager.GetFinalSettlementInfo08PM(EmployeeId, userName, prepareDate, searchModel.OthersDeduction);
            }
            return PartialView("_FinalSettlement", model);
        }

        public ActionResult Index10PMNoWeekend(ReportSearchViewModel model)
        {
            ModelState.Clear();

            try
            {
                model.OthersDeduction = 0;
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return View(model);
        }

        public ActionResult Search10PMNoWeekend(ReportSearchViewModel searchModel)
        {
            ModelState.Clear();

            Guid EmployeeId = new Guid();
            Employee employee = new Employee();
            var model = new EmployeeAppointmentViewModel();

            if (!string.IsNullOrEmpty(searchModel.SearchByEmployeeCardId))
                employee = EmployeeManager.GetAnyEmployeeByCardId(searchModel.SearchByEmployeeCardId);

            if (employee == null)
                return ErrorResult("Invalid ID or Access Denied!");

            EmployeeId = employee.EmployeeId;

            string userName = PortalContext.CurrentUser.Name;

            if (searchModel.Date != null)
            {
                DateTime prepareDate = searchModel.Date.Value;
                model.AppointmentLetterInfo = EmployeeAppointmentManager.GetFinalSettlementInfo10PMNoWeekend(EmployeeId, userName, prepareDate, searchModel.OthersDeduction);
            }
            return PartialView("_FinalSettlement", model);
        }

        public ActionResult Index10PM(ReportSearchViewModel model)
        {
            ModelState.Clear();

            try
            {
                model.OthersDeduction = 0;
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return View(model);
        }

        public ActionResult Search10PM(ReportSearchViewModel searchModel)
        {
            ModelState.Clear();

            Guid EmployeeId = new Guid();
            Employee employee = new Employee();
            var model = new EmployeeAppointmentViewModel();

            if (!string.IsNullOrEmpty(searchModel.SearchByEmployeeCardId))
                employee = EmployeeManager.GetAnyEmployeeByCardId(searchModel.SearchByEmployeeCardId);

            if (employee == null)
                return ErrorResult("Invalid ID or Access Denied!");

            EmployeeId = employee.EmployeeId;

            string userName = PortalContext.CurrentUser.Name;

            if (searchModel.Date != null)
            {
                DateTime prepareDate = searchModel.Date.Value;
                model.AppointmentLetterInfo = EmployeeAppointmentManager.GetFinalSettlementInfo10PM(EmployeeId, userName, prepareDate, searchModel.OthersDeduction);
            }
            return PartialView("_FinalSettlement", model);
        }

        public ActionResult IndexOriginalNoWeekend(ReportSearchViewModel model)
        {
            ModelState.Clear();

            try
            {
                model.OthersDeduction = 0;
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return View(model);
        }

        public ActionResult SearchOriginalNoWeekend(ReportSearchViewModel searchModel)
        {
            ModelState.Clear();

            Guid EmployeeId = new Guid();
            Employee employee = new Employee();
            var model = new EmployeeAppointmentViewModel();

            if (!string.IsNullOrEmpty(searchModel.SearchByEmployeeCardId))
                employee = EmployeeManager.GetAnyEmployeeByCardId(searchModel.SearchByEmployeeCardId);

            if (employee == null)
                return ErrorResult("Invalid ID or Access Denied!");

            EmployeeId = employee.EmployeeId;

            string userName = PortalContext.CurrentUser.Name;

            if (searchModel.Date != null)
            {
                DateTime prepareDate = searchModel.Date.Value;
                model.AppointmentLetterInfo = EmployeeAppointmentManager.GetFinalSettlementInfoOriginalNoWeekend(EmployeeId, userName, prepareDate, searchModel.OthersDeduction);
            }
            return PartialView("_FinalSettlement", model);
        }
    }
}