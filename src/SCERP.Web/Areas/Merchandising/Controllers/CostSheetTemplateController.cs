using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.Common;
using SCERP.DAL.IRepository.IMerchandisingRepository;
using SCERP.Model;
using SCERP.Web.Areas.Merchandising.Models.ViewModel;
using SCERP.Web.Controllers;

namespace SCERP.Web.Areas.Merchandising.Controllers
{
    public class CostSheetTemplateController : BaseController
    {
        private readonly ICostSheetTemplateManager _costSheetTemplateManager;
        private readonly IItemTypeManager _itemTypeManager;
        private readonly ITempGroupManager _tempGroupManager;

        public CostSheetTemplateController(ICostSheetTemplateManager costSheetTemplateManager, IItemTypeManager itemTypeManager, ITempGroupManager tempGroupManager)
        {
            _costSheetTemplateManager = costSheetTemplateManager;
            _itemTypeManager = itemTypeManager;
            _tempGroupManager = tempGroupManager;
        }
        public ActionResult Index(CostSheetTemplateViewModel model)
        {
            try
            {
                var totalRecords = 0;
                model.CostSheetTemplates = _costSheetTemplateManager.GetCostSheetTemplateByPaging(model.CostSheetTemplate.Particular, model.PageIndex, model.sort, model.sortdir, out totalRecords);
                model.TotalRecords = totalRecords;
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult("Fail To Retrive Data :" + exception);
            }
            return View(model);
        }
        public ActionResult Edit(CostSheetTemplateViewModel model)
        {
            ModelState.Clear();
            try
            {
                if (model.CostSheetTemplate.TemplateId > 0)
                {
                    OM_CostSheetTemplate costSheetTemplate = _costSheetTemplateManager.GeTcostSheetTemplateByTemplateId(model.CostSheetTemplate.TemplateId, PortalContext.CurrentUser.CompId);
                    model.CostSheetTemplate.Particular = costSheetTemplate.Particular;
                    model.CostSheetTemplate.SerialNo = costSheetTemplate.SerialNo;
                    model.CostSheetTemplate.ItemTypeId = costSheetTemplate.ItemTypeId;
                    model.CostSheetTemplate.TempGroupId = costSheetTemplate.TempGroupId;
                    model.CostSheetTemplate.UnitName = costSheetTemplate.UnitName;
                }
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult("Fail To Retrive Data :" + exception);
            }
             model.ItemTypes = _itemTypeManager.GetAllItemType(PortalContext.CurrentUser.CompId);
            model.TempGroups = _tempGroupManager.GetAllTempGroup(PortalContext.CurrentUser.CompId);
            return View(model);
        }
        public ActionResult Save(CostSheetTemplateViewModel model)
        {
            ModelState.Clear();
            var index = 0;
            try
            {
                if (_costSheetTemplateManager.IsCostSheetTemplateExist(model.CostSheetTemplate))
                {
                    return ErrorResult("This Information Already Exist ! Please Entry another one");
                }
                else
                {
                    index = model.CostSheetTemplate.TemplateId > 0 ? _costSheetTemplateManager.EditCostSheetTemplate(model.CostSheetTemplate) : _costSheetTemplateManager.SaveCostSheetTemplate(model.CostSheetTemplate);
                }
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult("Failed to Save/Edit Cost Sheet Template :" + exception);
            }
            return index > 0 ? Reload() : ErrorResult("Failed to Save/Edit Cost Sheet Template !");
        }

        public ActionResult Delete(int templateId)
        {
            int index = 0;
            try
            {
                index = _costSheetTemplateManager.DeleteCostSheetTemplate(templateId, PortalContext.CurrentUser.CompId);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult("Failed to Delete Cost Sheet Template :" + exception);
            }
            return index > 0 ? Reload() : ErrorResult("Failed to Delete Cost Sheet Template !");
        }
    }
}
