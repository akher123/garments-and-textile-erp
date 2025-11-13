using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.Common;
using SCERP.Model;
using SCERP.Model.MerchandisingModel;
using SCERP.Web.Areas.Merchandising.Models.ViewModel;
using SCERP.Web.Controllers;

namespace SCERP.Web.Areas.Merchandising.Controllers
{
    public class CostSheetMasterController : BaseController
    {
        private readonly ICostSheetMasterManager _costSheetMasterManager;
        private readonly IOmBuyerManager _buyerManager;
        private readonly IItemTypeManager _itemTypeManager;
        private readonly ICostSheetTemplateManager _costSheetTemplateManager;
        private readonly ICostSheetDetailManager _costSheetDetailManager;
        public CostSheetMasterController(ICostSheetMasterManager costSheetMasterManager, IOmBuyerManager buyerManager, IItemTypeManager itemTypeManager, ICostSheetTemplateManager costSheetTemplateManager, ICostSheetDetailManager costSheetDetailManager)
        {
            _costSheetMasterManager = costSheetMasterManager;
            _buyerManager = buyerManager;
            _itemTypeManager = itemTypeManager;
            _costSheetTemplateManager = costSheetTemplateManager;
            _costSheetDetailManager = costSheetDetailManager;
        }
        public ActionResult Index(CostSheetMasterViewModel model)
        {
            try
            {
                model.Buyers = _buyerManager.GetAllBuyers();
                var totalRecords = 0;
                model.CostSheetMasters = _costSheetMasterManager.GetCostSheetMasterByPaging(model.CostSheetMaster.BuyerId,model.CostSheetMaster.OrderNo,model.CostSheetMaster.Color, model.PageIndex, model.sort, model.sortdir, out totalRecords);
                model.TotalRecords = totalRecords;
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult("Fail To Retrive Data :" + exception);
            }
            return View(model);
        }

        public ActionResult Edit(CostSheetMasterViewModel model)
        {
            try
            { 
                if (model.CostSheetMaster.CostSheetMasterId>0)
                {
                    model.CostSheetMaster = _costSheetMasterManager.GetCostSheetMasterById(model.CostSheetMaster.CostSheetMasterId,PortalContext.CurrentUser.CompId);
                    List<OM_CostSheetTemplate> costSheetTemplateList = _costSheetMasterManager.GetCostSheetTemplateDetailByCostSheetMasterId(model.CostSheetMaster.CostSheetMasterId, PortalContext.CurrentUser.CompId);
                    model.CsTemDictionary = costSheetTemplateList.ToDictionary(x => x.TemplateId.ToString(), x => x);

                }
                else
                {
                    model.CostSheetMaster.CostSheetMasterRefId = _costSheetMasterManager.GetCostSheetMasterRefId();
                }
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                ErrorResult("Fail To Retrive Data" + exception);
            }
            model.Buyers = _buyerManager.GetAllBuyers();
            model.ItemTypes = _itemTypeManager.GetAllItemType(PortalContext.CurrentUser.CompId);
            return View(model);
        }

        public ActionResult GetCostSheetTemplateByItemTypeId(int itemTypeId)
        {
            CostSheetMasterViewModel model=new CostSheetMasterViewModel();

             List<OM_CostSheetTemplate> costSheetTemplateList = _costSheetTemplateManager.GetCostSheetTemplateByItemTypeId(itemTypeId, PortalContext.CurrentUser.CompId);
            model.CsTemDictionary = costSheetTemplateList.ToDictionary(x => x.TemplateId.ToString(), x => x);
            return PartialView("~/Areas/Merchandising/Views/CostSheetMaster/_Particular.cshtml", model);
        }

        public ActionResult Save(CostSheetMasterViewModel model)
        {
            int index = 0;
            try
            {
             model.CostSheetMaster.OM_CostSheetDetail=model.CsTemDictionary.Select(x => new OM_CostSheetDetail()
                {
                    TemplateId = x.Value.TemplateId,
                    ParticularRate = x.Value.ParticularRate,
                    CompId = PortalContext.CurrentUser.CompId
                }).ToList();
              index= model.CostSheetMaster.CostSheetMasterId > 0? _costSheetMasterManager.EditCostSheetMaster(model.CostSheetMaster): _costSheetMasterManager.SaveCostSheetMaster(model.CostSheetMaster);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult("Fail To Save/Edit.");
            }
            return index > 0 ? Reload() : ErrorResult("Fail To Save/Edit");
        }

        public ActionResult Delete(long costSheetMasterId)
        {
            var index = 0;
            try
            {
                index = _costSheetMasterManager.DeleteCostSheetMaster(costSheetMasterId);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult("Fail to Delete Cost Sheet :" + exception);
            }
            return index > 0 ? Reload() : ErrorResult("Fail to Delete Cost Sheet !");
        }
        public ActionResult InitialCostingReport(CostSheetMasterViewModel model)
        {
            try
            {
                    model.CostSheetMaster = _costSheetMasterManager.GetCostSheetMasterById(model.CostSheetMaster.CostSheetMasterId, PortalContext.CurrentUser.CompId);
                    List<OM_CostSheetTemplate> costSheetTemplateList = _costSheetMasterManager.GetCostSheetTemplateDetailByCostSheetMasterId(model.CostSheetMaster.CostSheetMasterId, PortalContext.CurrentUser.CompId);
                    model.CsTemDictionary = costSheetTemplateList.ToDictionary(x => x.TemplateId.ToString(), x => x);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                ErrorResult("Fail To Retrive Data" + exception);
            }
            return View(model);
        }
    }
}