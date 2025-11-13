using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCERP.BLL.IManager.IHRMManager;
using SCERP.Common;
using SCERP.Model.HRMModel;
using SCERP.Model.PayrollModel;
using SCERP.Web.Areas.HRM.Models.ViewModels;
using SCERP.Web.Areas.Payroll.Models.ViewModels;
using SCERP.Web.Controllers;

namespace SCERP.Web.Areas.HRM.Controllers
{
    public class EmployeeManualOTController : BaseController
    {
        private readonly IEmployeeManualOverTimeManager _employeeManualOverTimeManager;
        public EmployeeManualOTController(IEmployeeManualOverTimeManager employeeManualOverTimeManager)
        {
            _employeeManualOverTimeManager = employeeManualOverTimeManager;
        }

        // GET: HRM/EmployeeManualOT
        [AjaxAuthorize(Roles = "manualovertime-1,manualovertime-2,manualovertime-3")]
        public ActionResult Index(EmployeeManualOverTimeViewModel model)
        {
            ModelState.Clear();
            var totalRecords = 0;
            
            model.EmployeeManualOTs = _employeeManualOverTimeManager.GetAllEmployeeManualOverTimeByPaging(model, out totalRecords);
            model.TotalRecords = totalRecords;
            return View(model);
            
        }
        [AjaxAuthorize(Roles = "manualovertime-2,manualovertime-3")]
        public ActionResult Edit(EmployeeManualOverTimeViewModel model)
        {
            ModelState.Clear();
            try
            {
                if (model.EmployeeManualOverTimeId > 0)
                {


                    EmployeeManualOverTime employeeManualOverTime = _employeeManualOverTimeManager.GetEmployeeManualOverTimeById(model.EmployeeManualOverTimeId);
                    model.EmployeeManualOT = employeeManualOverTime;




                }
                else
                {


                    model.EmployeeManualOT.Date=DateTime.Today;



                }


            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult("" + exception);
            }
            return View(model);
        }
        [AjaxAuthorize(Roles = "manualovertime-2,manualovertime-3")]
        public ActionResult Save(EmployeeManualOverTimeViewModel model)
        {
            var index = 0;

            try
            {
                bool exist = _employeeManualOverTimeManager.IsEmployeeManualOverTimeExist(model.EmployeeManualOT);
                if (!exist)
                {
                    if (model.EmployeeManualOT.EmployeeManualOverTimeId > 0)
                    {
                        var employee = EmployeeManager.GetEmployeeByCardId(model.EmployeeManualOT.EmployeeCardId);
                        model.EmployeeManualOT.EmployeeId = employee.EmployeeId;
                        model.EmployeeManualOT.EditedDate = DateTime.Now;
                        model.EmployeeManualOT.EditedBy = PortalContext.CurrentUser.UserId;
                        index = _employeeManualOverTimeManager.EditEmployeeManualOverTime(model.EmployeeManualOT);
                    }
                    else
                    {

                        var employee = EmployeeManager.GetEmployeeByCardId(model.EmployeeManualOT.EmployeeCardId);
                        model.EmployeeManualOT.EmployeeId = employee.EmployeeId;
                        model.EmployeeManualOT.CreatedDate = DateTime.Now;
                        model.EmployeeManualOT.CreatedBy = PortalContext.CurrentUser.UserId;
                        model.EmployeeManualOT.IsActive = true;
                        index = _employeeManualOverTimeManager.SaveEmployeeManualOverTime(model.EmployeeManualOT);

                    }
                }
                else
                {
                    return ErrorResult("Same Information Already Exist ! Please Entry Another One.");
                }

            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult("" + exception);
            }
            return index > 0 ? Reload() : ErrorResult("Fail To Save Task");
        }
        [AjaxAuthorize(Roles = "manualovertime-3")]
        public ActionResult Delete(int employeeManualOverTimeId)
        {
            var index = 0;
            try
            {

                index = _employeeManualOverTimeManager.DeleteEmployeeManualOverTime(employeeManualOverTimeId);


            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult("" + exception);
            }
            return index > 0 ? Reload() : ErrorResult("Fail to Delete Subject");
        }



    }
}