using System;
using System.Web.Mvc;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.Common;
using SCERP.Model;
using SCERP.Web.Areas.Merchandising.Models.ViewModel;
using SCERP.Web.Controllers;

namespace SCERP.Web.Areas.Merchandising.Controllers
{
    public class TempGroupController : BaseController
    {
        private readonly ITempGroupManager _tempGroupManager;

        public TempGroupController(ITempGroupManager tempGroupManager)
        {
            _tempGroupManager = tempGroupManager;
        }
        public ActionResult Index(TempGroupViewModel model)
        {
            try
            {
                var totalRecords = 0;
                model.TempGroups = _tempGroupManager.GetTempGroupByPaging(model.TempGroup.TempGroupName,model.PageIndex, model.sort, model.sortdir, out totalRecords);
                model.TotalRecords = totalRecords;
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult("Fail To Retrive Data :" + exception);
            }
            return View(model);
        }

        public ActionResult Edit(TempGroupViewModel model)
        {
            ModelState.Clear();
            try
            {
                if (model.TempGroup.TempGroupId > 0)
                {
                    OM_TempGroup tempGroup = _tempGroupManager.GeTempGroupById(model.TempGroup.TempGroupId, PortalContext.CurrentUser.CompId);
                    model.TempGroup.TempGroupName = tempGroup.TempGroupName;
                }
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult("Fail To Retrive Data :" + exception);
            }
            return View(model);
        }
        public ActionResult Save(TempGroupViewModel model)
        {
            ModelState.Clear();
            var index = 0;
            try
            {
                if (_tempGroupManager.IsTempGroupExist(model.TempGroup))
                {
                    return ErrorResult("Template Group :" + model.TempGroup.TempGroupName + " " + "Already Exist ! Please Entry another one");
                }
                else
                {
                    index = model.TempGroup.TempGroupId> 0 ? _tempGroupManager.EditTempGroup(model.TempGroup) : _tempGroupManager.SaveTempGroup(model.TempGroup);
                }
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult("Failed to Save/Edit Template Group :" + exception);
            }
            return index > 0 ? Reload() : ErrorResult("Failed to Save/Edit Template Group !");
        }

        public ActionResult Delete(int tempGroupId)
        {
            int index = 0;
            try
            {
                index = _tempGroupManager.DeleteTemplateGroup(tempGroupId,PortalContext.CurrentUser.CompId);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult("Failed to Delete Template Group :" + exception);
            }
            return index > 0 ? Reload() : ErrorResult("Failed to Delete Template Group !");
        }
    }
}