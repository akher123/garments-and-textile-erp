using System;
using System.Web.Mvc;
using SCERP.BLL.IManager.IHRMManager;
using SCERP.BLL.IManager.IProductionManager;
using SCERP.BLL.IManager.ITrackingManager;
using SCERP.Common;
using SCERP.Web.Areas.Tracking.Models.ViewModels;
using SCERP.Web.Controllers;

namespace SCERP.Web.Areas.Tracking.Controllers
{
    public class MachineLogController : BaseController
    {
        private readonly IMachineLogManager _machineLogManager;
        private readonly IEmployeeManager _employeeManager;
        private readonly IMachineManager _machineManager;
        private readonly IMachineActionManager _machineActionManager;
        public MachineLogController(IMachineLogManager machineLogManager, IEmployeeManager employeeManager, IMachineManager machineManager, IMachineActionManager machineActionManager)
        {
            _machineLogManager = machineLogManager;
            _employeeManager = employeeManager;
            _machineManager = machineManager;
            _machineActionManager = machineActionManager;

        }
        public ActionResult Index(MachineLogViewModel model)
        {
            try
            {
                var totalRecords = 0;
                model.MachineLogs = _machineLogManager.GetAllMachineLogByPaging(model, out totalRecords);
                model.TotalRecords = totalRecords;
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult("Fail To Retrive Data :" + exception);
            }
            return View(model);
        }
        public ActionResult Edit(MachineLogViewModel model)
        {
            ModelState.Clear();
            try
            {
                if (model.MachineLogId > 0)
                {
                    model.MachineLog = _machineLogManager.GetMachineLogByMachineLogId(model.MachineLogId);
                    model.EmployeeName = _employeeManager.GetEmployeeNameByEmployeeId(model.MachineLog.EmployeeId.GetValueOrDefault());
                    if (model.MachineLog.ActionDate != null)
                    {
                        DateTime actionDateTime = (DateTime)model.MachineLog.ActionDate;
                        model.MachineLog.ActionTime = actionDateTime.ToString("hh:mm tt");
                    }
                }
                else
                {
                    model.MachineLog.MachineLogRefId = _machineLogManager.GetNewMachineLogRefId();
                    model.MachineLog.ActionDate = DateTime.Now;
                    string time = DateTime.Now.ToString("hh:mm tt");
                    model.MachineLog.ActionTime = time;
                }
                model.ProductionMachines = _machineManager.GetMachines();
                model.MachineActionList = _machineActionManager.GetAllMachineAction();
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult("Fail To Retrive Data :" + exception);
            }

            return View(model);
        }
        public ActionResult Save(MachineLogViewModel model)
        {
            var index = 0;
            try
            {
                if (_machineLogManager.IsMachineLogExist(model.MachineLog))
                {
                    return ErrorResult(" This Information Already Exist ! Please Entry Another One");
                }
                index = model.MachineLog.MachineLogId > 0 ? _machineLogManager.EditMachineLog(model.MachineLog) : _machineLogManager.SaveMachineLog(model.MachineLog);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult("Failed to Save/Edit Machine Log :" + exception);
            }
            return index > 0 ? Reload() : ErrorResult("Fail to Save/Edit Machine Log !");
        }
        
        public ActionResult Delete(long machineLogId)
        {
            var index = 0;
            try
            {
                index = _machineLogManager.DeleteMachineLog(machineLogId);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult("Fail to Delete Machine Log :" + exception);
            }
            return index > 0 ? Reload() : ErrorResult("Fail to Delete Machine Log !");
        }
        public JsonResult GetEmployeesBySearchCharacter(string searchCharacter)
        {
            var employees = _employeeManager.GetEmployeesBySearchCharacter(searchCharacter);
            return Json(employees, JsonRequestBehavior.AllowGet);
        }
    }
}