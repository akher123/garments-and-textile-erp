using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCERP.BLL.IManager.IProductionManager;
using SCERP.Common;
using SCERP.Model.Production;
using SCERP.Web.Areas.Production.Models;
using SCERP.Web.Controllers;

namespace SCERP.Web.Areas.Production.Controllers
{
    public class KnittingProcessorController : BaseController
    {

        private readonly IProcessorManager _processorManager;
        public KnittingProcessorController(IProcessorManager processorManager)
        {
            this._processorManager = processorManager;
        }

        [AjaxAuthorize(Roles = "knittingprocessor-1,knittingprocessor-2,knittingprocessor-3")]
        public ActionResult Index(ProcessorViewModel model)
        {
            ModelState.Clear();
            var totalRecords = 0;
            model.ProcessRefId = ProcessCode.KNITTING;
            model.Processors = _processorManager.GetProcessorByPaging(model, out totalRecords);
            model.TotalRecords = totalRecords;
            return View(model);
        }
        [HttpGet]
        [AjaxAuthorize(Roles = "knittingprocessor-2,knittingprocessor-3")]
        public ActionResult Edit(ProcessorViewModel model)
        {
            ModelState.Clear();
            if (model.ProcessorId > 0)
            {
                var prodProcessor = _processorManager.GetProcessorById(model.ProcessorId);
                model.ProcessRefId = prodProcessor.ProcessRefId;
                model.ProcessorRefId = prodProcessor.ProcessorRefId;
                model.ProcessorName = prodProcessor.ProcessorName;
            }
            else
            {
                model.ProcessorRefId = _processorManager.GetProcessorNewRefId();
            }

            return View(model);
        }
        [HttpPost]
        [AjaxAuthorize(Roles = "knittingprocessor-2,knittingprocessor-3")]
        public ActionResult Save(PROD_Processor model)
        {
            try
            {
                var saveIndex = 0;
                model.ProcessRefId = ProcessCode.KNITTING;
                saveIndex = model.ProcessorId > 0 ? _processorManager.EditProcessor(model) : _processorManager.SaveProcessor(model);
                return saveIndex > 0 ? Reload() : ErrorResult("Save Fail !!");
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult(exception.Message);
            }
        }
        [AjaxAuthorize(Roles = "knittingprocessor-3")]
        public ActionResult Delete(PROD_Processor model)
        {
            var saveIndex = _processorManager.DeleteProcessor(model.ProcessorRefId);
            if (saveIndex == -1)
            {
                return ErrorResult("Could not possible to delete Buyer because of it's all ready used");
            }
            return saveIndex > 0 ? Reload() : ErrorResult("Delate Fail");
        }
        [HttpPost]
        public JsonResult CheckExistingProcessor(PROD_Processor model)
        {
            var isExist = !_processorManager.CheckExistingProcessor(model);
            return Json(isExist, JsonRequestBehavior.AllowGet);
        }
    }
}