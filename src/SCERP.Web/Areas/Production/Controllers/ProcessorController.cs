using System;
using System.Web.Mvc;
using SCERP.BLL.IManager.IProductionManager;
using SCERP.BLL.Manager.ProductionManager;
using SCERP.Common;
using SCERP.Model.Production;
using SCERP.Web.Areas.Production.Models;
using SCERP.Web.Controllers;

namespace SCERP.Web.Areas.Production.Controllers
{
    public class ProcessorController : BaseController
    {
        private IProcessorManager processorManager;
        public ProcessorController(IProcessorManager processorManager)
        {
            this.processorManager = processorManager;
        }
        public ActionResult Index(ProcessorViewModel model)
        {
            ModelState.Clear();
            var totalRecords = 0;
            model.Processors = processorManager.GetProcessorByPaging(model, out totalRecords);
            model.TotalRecords = totalRecords;
            return View(model);
        }
        [HttpGet]
        public ActionResult Edit(ProcessorViewModel model)
        {
            ModelState.Clear();
            if (model.ProcessorId>0)
            {
                var prodProcessor = processorManager.GetProcessorById(model.ProcessorId);
                model.ProcessRefId = prodProcessor.ProcessRefId;
                model.ProcessorRefId = prodProcessor.ProcessorRefId;
                model.ProcessorName = prodProcessor.ProcessorName;
            }
            else
            {
                model.ProcessorRefId = processorManager.GetProcessorNewRefId();
            }
            model.Processes = processorManager.GetProcessList();
            return View(model);
        }
        [HttpPost]
        public ActionResult Save(PROD_Processor model)
        {
            try
            {
                var saveIndex = 0;
                 saveIndex = model.ProcessorId>0 ? processorManager.EditProcessor(model) : processorManager.SaveProcessor(model);
             

                return saveIndex > 0 ? Reload() : ErrorResult("Save Fail !!");
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult(exception.Message);
            }
        }

        public ActionResult Delete(PROD_Processor model)
        {
            var saveIndex = processorManager.DeleteProcessor(model.ProcessorRefId);
            if (saveIndex == -1)
            {
                return ErrorResult("Could not possible to delete Buyer because of it's all ready used");
            }
            return saveIndex > 0 ? Reload() : ErrorResult("Delate Fail");
        }
        [HttpPost]
        public JsonResult CheckExistingProcessor(PROD_Processor model)
        {
            var isExist = !processorManager.CheckExistingProcessor(model);
            return Json(isExist, JsonRequestBehavior.AllowGet);
        }
    }
}