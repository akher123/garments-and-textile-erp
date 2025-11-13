using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCERP.BLL.IManager.IProductionManager;
using SCERP.Common;
using SCERP.Model;
using SCERP.Web.Areas.Production.Models;
using SCERP.Web.Controllers;

namespace SCERP.Web.Areas.Production.Controllers
{
    public class KnittingMachineController : BaseController
    {
        private readonly IProcessorManager _processorManager;
        private readonly IMachineManager _machineManager;
        public KnittingMachineController(IProcessorManager processorManager, IMachineManager machineManager)
        {
            this._processorManager = processorManager;
            _machineManager = machineManager;
        }
          [AjaxAuthorize(Roles = "knittingmachine-1,knittingmachine-2,knittingmachine-3")]
        public ActionResult Index(MachineViewModel model)
        {
            var totalRecords = 0;
            ModelState.Clear();
            model.ProcessRefId = ProcessCode.KNITTING;
            model.Machines = _machineManager.GetMachineListByPaging(model, out totalRecords);
            model.TotalRecords = totalRecords;
            return View(model);
        }
             [AjaxAuthorize(Roles = "knittingmachine-2,knittingmachine-3")]
        public ActionResult Edit(MachineViewModel model)
        {
            ModelState.Clear();
            model.Processors = _processorManager.GetProcessorByProcessRefId(ProcessCode.KNITTING,PortalContext.CurrentUser.CompId);
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
        [AjaxAuthorize(Roles = "knittingmachine-2,knittingmachine-3")]
        public ActionResult Save(Production_Machine model)
        {
            var index = 0;
            var errorMessage="";
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
               [AjaxAuthorize(Roles = "knittingmachine-3")]
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