using System;
using System.Web.Mvc;
using SCERP.BLL.IManager.IProductionManager;
using SCERP.Common;
using SCERP.Model;
using SCERP.Web.Areas.Production.Models;

namespace SCERP.Web.Areas.Production.Controllers
{
    public class MachineController : BaseProductionController
    {
        private readonly IProcessorManager _processorManager;
        private readonly IMachineManager _machineManager;
        public MachineController(IProcessorManager processorManager, IMachineManager machineManager)
        {
            this._processorManager = processorManager;
            _machineManager = machineManager;
        }
        [AjaxAuthorize(Roles = "machine-1,machine-2,machine-3")]
        public ActionResult Index(MachineViewModel model)
        {
            var totalRecords = 0;
            ModelState.Clear();
            model.Processors = _processorManager.GetProcessorLsit();
            model.Machines = _machineManager.GetMachineListByPaging(model, out totalRecords);
            model.TotalRecords = totalRecords;
            return View(model);
        }
        [AjaxAuthorize(Roles = "machine-2,machine-3")]
        public ActionResult Edit(MachineViewModel model)
        {
            ModelState.Clear();
            model.Processors = _processorManager.GetProcessorLsit();
            model.MachineRefId = _machineManager.GetNewMachineRefId();

            if (model.MachineId <= 0) return View(model);
            var machine = _machineManager.GetMachineById(model.MachineId);
            model.Name = machine.Name;
            model.NoMachine = machine.NoMachine;
            model.EfficiencyPer = machine.EfficiencyPer;
            model.IdelPer = machine.IdelPer;
            model.ProcessorRefId = machine.ProcessorRefId;
            model.Description = machine.Description;
            return View(model);
        }
        [AjaxAuthorize(Roles = "machine-2,machine-3")]
        public ActionResult Save(Production_Machine model)
        {
            var index = 0;
            var errorMessage = "";
            try
            {
                index = model.MachineId > 0 ? _machineManager.EditMachine(model) : _machineManager.SaveMachine(model);
            }
            catch (Exception exception)
            {
                errorMessage = exception.Message;
                Errorlog.WriteLog(exception);
            }
            return index > 0 ? Reload() : ErrorResult("Failed to save Machine !" + errorMessage);
        }
        [AjaxAuthorize(Roles = "machine-3")]
        public ActionResult Delete(Production_Machine machine)
        {
            var deleteIndex = 0;
            var errorMessage = "";
            try
            {
                deleteIndex = _machineManager.DeleteColor(machine.MachineId);
            }
            catch (Exception exception)
            {
                errorMessage = exception.Message;
                Errorlog.WriteLog(exception);
            }
            return (deleteIndex > 0) ? Reload() : ErrorResult("Failed to delete Machine !" + errorMessage);
        }
    }
}