using System;
using System.Web.Mvc;
using SCERP.BLL.IManager.ITrackingManager;
using SCERP.Common;
using SCERP.Model.TrackingModel;
using SCERP.Web.Areas.Tracking.Models.ViewModels;
using SCERP.Web.Controllers;

namespace SCERP.Web.Areas.Tracking.Controllers
{
    public class MachineActionController : BaseController
    {
        private readonly IMachineActionManager _machineActionManager;

        public MachineActionController(IMachineActionManager machineActionManager)
        {
            _machineActionManager = machineActionManager;
        }
        public ActionResult Index(MachineActionViewModel model)
        {
            try
            {
                var totalRecords = 0;
                model.MachineActions = _machineActionManager.GetAllMachineActionByPaging(model, out totalRecords);
                model.TotalRecords = totalRecords;
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult("Fail To Retrive Data :" + exception);
            }
            return View(model);
        }
        [HttpPost]
        public ActionResult Save(MachineActionViewModel model)
        {
            var index = 0;
            try
            {
                TrackMachineAction machineAction = new TrackMachineAction
                {
                    MachineActionId = model.MachineActionId,
                    MachineActionName = model.MachineActionName,
                    Remarks = model.Remarks
                };
                if (_machineActionManager.IsMachineActonExist(model))
                {
                    return
                        ErrorResult("Machine Action :" + model.MachineActionName + " " +
                                    "Already Exist ! Please Entry Another One.");
                }
                else
                {
                    index = model.MachineActionId > 0 ? _machineActionManager.EditMachineAction(machineAction) : _machineActionManager.SaveMachineAction(machineAction);
                }
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult("Failed to Save/Edit Machine Action :" + exception);
            }
            return index > 0 ? Reload() : ErrorResult("Failed to Save/Edit Machine Action !");
        }
        [HttpGet]
        public ActionResult Edit(MachineActionViewModel model)
        {
            ModelState.Clear();
            try
            {
                if (model.MachineActionId > 0)
                {
                    TrackMachineAction machineAction = _machineActionManager.GetMachineActionByMachineActionId(model.MachineActionId);
                    model.MachineActionName = machineAction.MachineActionName;
                    model.Remarks = machineAction.Remarks;
                }
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult("Fail To Retrive Data :" + exception);
            }
            return View(model);
        }
        [HttpGet]
        public ActionResult Delete(int machineActionId)
        {
            var index = 0;
            try
            {
                index = _machineActionManager.DeleteMachineAction(machineActionId);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult("Fail To Delele Machine Action :" + exception);
            }
            return index > 0 ? Reload() : ErrorResult("Fail To Delele Machine Action !");
        }
    }
}