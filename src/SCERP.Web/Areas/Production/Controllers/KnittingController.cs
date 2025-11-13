using System;
using System.Linq;
using System.Web.Mvc;
using SCERP.BLL.IManager.IProductionManager;
using SCERP.Common;
using SCERP.Model.Production;
using SCERP.Web.Areas.Production.Models;
using SCERP.BLL.IManager.IPlanningManager;
using SCERP.Web.Controllers;

namespace SCERP.Web.Areas.Production.Controllers
{
    public class KnittingController : BaseController
    {
        private readonly IProcessorManager _processorManager;
        private readonly IProgramManager _programManager;
        private readonly IProductionManager _productionManager;
        private IMachineManager _machineManager;
        public KnittingController(IMachineManager machineManager,IProgramManager programManager, IProcessorManager processorManager, IProductionManager productionManager)
        {
            this._programManager = programManager;
            this._processorManager = processorManager;
            this._productionManager = productionManager;
            _machineManager = machineManager;
        }
        public ActionResult Index(ProductionViewModel model)
        {
            ModelState.Clear();
            var totalRecords = 0;
            model.VwPrograms = _programManager.GetVwProgramsByPaging(ProcessType.KNITTING, out totalRecords);
            model.TotalRecords = totalRecords;
            return View(model);
        }
        public ActionResult KnittingList(ProductionViewModel model)
        {
            ModelState.Clear();
            model.ProcessRefId = ProcessType.KNITTING; 
            model.PType = "O";
            model.OutProdProductions = _productionManager.GetProductionByPaging(model);
            model.PType = "I";
            model.InProdProductions = _productionManager.GetProductionByPaging(model);
            return PartialView("_KnittingList", model);
        }
        public ActionResult KInEdit(ProductionViewModel model)
        {
            ModelState.Clear();
            model.Production.ProcessRefId = ProcessType.KNITTING;
            model.Production.PType = "I";
            if (model.ProductionId > 0)
            {
                var production = _productionManager.GetProductionById(model.ProductionId);
                model.ProductionDetails = _productionManager.GetProductionDetailsByProductionIed(model.ProductionId);
                model.Production = production;
                model.VProgramDetails = _productionManager.GetVProgramLis(production.ProrgramRefId, production.ProductionRefId, production.PType);
                model.Machines = _machineManager.AutocompliteMechineByProcessor(model.Production.ProcessorRefId);
            }
            else
            {
                model.ProductionDetails = _productionManager.GetProgramDetailsByProcessRefId(model.Production.ProcessRefId, model.ProrgramRefId, model.Production.PType);
                model.Production.ProductionRefId = _productionManager.GetProductionRefId(ReferencePrefix.KnittingProduction);
                model.Production.ProdDate = DateTime.Now;
                model.Production.ProrgramRefId = model.ProrgramRefId;
            }
            model.Processors = _processorManager.GetProcessorLsit();
            return View(model);
        }
        public ActionResult KOutEdit(ProductionViewModel model)
        {
            ModelState.Clear();
            model.Production.ProcessRefId = ProcessType.KNITTING;
            model.Production.PType = "O";
            if (model.ProductionId > 0)
            {
                var production = _productionManager.GetProductionById(model.ProductionId);
                model.ProductionDetails = _productionManager.GetProductionDetailsByProductionIed(model.ProductionId);
                model.Production = production;
                model.VProgramDetails = _productionManager.GetVProgramLis(production.ProrgramRefId, production.ProductionRefId, production.PType);
                model.Machines = _machineManager.AutocompliteMechineByProcessor(model.Production.ProcessorRefId);
            }
            else
            {
                model.ProductionDetails = _productionManager.GetProgramDetailsByProcessRefId(model.Production.ProcessRefId, model.ProrgramRefId, model.Production.PType);
                model.Production.ProductionRefId = _productionManager.GetProductionRefId(ReferencePrefix.SweingProduction);
                model.Production.ProdDate = DateTime.Now;
                model.Production.ProrgramRefId = model.ProrgramRefId;
            }
            model.Processors = _processorManager.GetProcessorLsit();

            return View(model);
        }
        public ActionResult Save(ProductionViewModel model)
        {
            string exception = "";
            try
            {
                model.Production.PROD_ProductionDetaill = model.ProductionDetails.Where(x => x.EntryQty > 0).Select(x => new PROD_ProductionDetaill()
                {
                    ProductionRefId = model.Production.ProductionRefId,
                    ColorRefId = x.ColorRefId,
                    SizeRefId = x.SizeRefId,
                    MeasurementUinitId = x.MeasurementUinitId,
                    ItemCode = x.ItemCode,
                    Qty = x.EntryQty,
                    CompId = PortalContext.CurrentUser.CompId,
                }).ToList();
                var saveIndex = model.Production.ProductionId > 0 ? _productionManager.EditProduction(model.Production) : _productionManager.SaveProduction(model.Production);
                if (saveIndex > 0)
                {
                    switch (model.Production.PType)
                    {
                        case "I":
                            return RedirectToAction("KInList", new { ProrgramRefId = model.Production.ProrgramRefId });
                        case "O":
                            return RedirectToAction("KOutList", new { ProrgramRefId = model.Production.ProrgramRefId });
                    }
                }
                else
                {
                    return ErrorResult("Save Failed !!");
                }
            }
            catch (Exception ex)
            {
                Errorlog.WriteLog(ex);
                exception = ex.Message;
            }
            return ErrorResult(exception);
        }

        public ActionResult KInList(ProductionViewModel model)
        {
            ModelState.Clear();
            model.ProcessRefId = ProcessType.KNITTING;
            model.PType = "I";
            model.InProdProductions = _productionManager.GetProductionByPaging(model);
            return PartialView("_KInList", model);
        }

        public ActionResult KOutList(ProductionViewModel model)
        {
            ModelState.Clear();
            model.ProcessRefId = ProcessType.KNITTING;
            model.PType = "O";
            model.OutProdProductions = _productionManager.GetProductionByPaging(model);
            return PartialView("_KOutList", model);
        }

        public ActionResult Delete(ProductionViewModel model)
        {
            string erorrMessage = "";
            ModelState.Clear();
            model.ProcessRefId = ProcessType.KNITTING;
            var delete = _productionManager.DeleteProduction(model);
            try
            {
                if (delete > 0)
                {
                    switch (model.PType)
                    {
                        case "I":
                            return RedirectToAction("KInList", new { ProrgramRefId = model.ProrgramRefId });
                        case "O":
                            return RedirectToAction("KOutList", new { ProrgramRefId = model.ProrgramRefId });
                    }
                }
                else
                {
                    return ErrorResult("Delete Failed !!");
                }

            }
            catch (Exception exception)
            {
                erorrMessage = exception.Message;
                Errorlog.WriteLog(exception);
            }

            return ErrorResult(erorrMessage);
        }

        public ActionResult AutocompliteMechineByProcessor(string processorRefId)
        {
            var machineList = _machineManager.AutocompliteMechineByProcessor(processorRefId);
            return Json(machineList, JsonRequestBehavior.AllowGet);
        }
    }
}