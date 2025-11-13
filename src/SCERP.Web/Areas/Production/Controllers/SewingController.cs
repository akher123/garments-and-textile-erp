using System;
using System.Linq;
using System.Web.Mvc;
using SCERP.BLL.IManager.IProductionManager;
using SCERP.Common;
using SCERP.Model.Planning;
using SCERP.Model.Production;
using SCERP.Web.Areas.Production.Models;
using SCERP.BLL.IManager.IPlanningManager;
using SCERP.Web.Controllers;

namespace SCERP.Web.Areas.Production.Controllers
{
    public class SewingController : BaseController
    {
        private readonly IProcessorManager _processorManager;
        private readonly IProductionManager _productionManager;
        private readonly IProgramManager _programManager;
        private IMachineManager _machineManager;
        public SewingController(IMachineManager machineManager,IProcessorManager processorManager, IProductionManager productionManager, IProgramManager programManager)
        {
            this._processorManager = processorManager;
            this._productionManager = productionManager;
            _programManager = programManager;
            _machineManager = machineManager;
        }
        public ActionResult Index(ProductionViewModel model)
        {
            ModelState.Clear();
            var totalRecords = 0;
            model.VwPrograms = _programManager.GetVwProgramsByPaging(ProcessType.SEWING, out totalRecords);
            model.TotalRecords = totalRecords;
            return View(model);
        }
        public ActionResult SewingList(ProductionViewModel model)
        {
            ModelState.Clear();
            model.ProcessRefId = ProcessType.SEWING; 
            model.PType = "O";
            model.OutProdProductions = _productionManager.GetProductionByPaging(model);
            model.PType = "I";
            model.InProdProductions = _productionManager.GetProductionByPaging(model);
            return PartialView("_SewingList", model);
        }
        public ActionResult SInEdit(ProductionViewModel model)
        {
            ModelState.Clear();
            model.Production.ProcessRefId = ProcessType.SEWING; 
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
                model.Production.ProductionRefId = _productionManager.GetProductionRefId(ReferencePrefix.SweingProduction);
                model.Production.ProdDate = DateTime.Now;
                model.Production.ProrgramRefId = model.ProrgramRefId;
            }

           
            model.Processors = _processorManager.GetProcessorLsit();

            return View(model);
        }
        public ActionResult SOutEdit(ProductionViewModel model)
        {
            ModelState.Clear();
            model.Production.ProcessRefId = ProcessType.SEWING; 
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
                            return RedirectToAction("SInList", new { ProrgramRefId = model.Production.ProrgramRefId });
                        case "O":
                            return RedirectToAction("SOutList", new { ProrgramRefId = model.Production.ProrgramRefId });
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

        public ActionResult SInList(ProductionViewModel model)
        {
            ModelState.Clear();
            model.ProcessRefId = ProcessType.SEWING; 
            model.PType = "I";
            model.InProdProductions = _productionManager.GetProductionByPaging(model);
            return PartialView("_SInList", model);
        }

        public ActionResult SOutList(ProductionViewModel model)
        {
            ModelState.Clear();
            model.ProcessRefId = ProcessType.SEWING; 
            model.PType = "O";
            model.OutProdProductions = _productionManager.GetProductionByPaging(model);
            return PartialView("_SOutList", model);
        }

        public ActionResult Delete(ProductionViewModel model)
        {
            string erorrMessage = "";
            ModelState.Clear();
            model.ProcessRefId = ProcessType.SEWING; 
            var delete = _productionManager.DeleteProduction(model);
            try
            {
                if (delete > 0)
                {
                    switch (model.PType)
                    {
                        case "I":
                            return RedirectToAction("SInList", new { ProrgramRefId = model.ProrgramRefId });
                        case "O":
                            return RedirectToAction("SOutList", new { ProrgramRefId = model.ProrgramRefId });
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
	}
}