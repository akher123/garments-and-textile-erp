using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCERP.BLL.IManager.IHRMManager;
using SCERP.Common;
using SCERP.Model.HRMModel;
using SCERP.Web.Areas.HRM.Models.ViewModels;
using SCERP.Web.Controllers;

namespace SCERP.Web.Areas.HRM.Controllers
{
    public class PenaltyTypeController : BaseController
    {
        private readonly IPenaltyTypeManager _penaltyTypeManager;
        public PenaltyTypeController(IPenaltyTypeManager penaltyTypeManager)
        {
            _penaltyTypeManager = penaltyTypeManager;
        }

        public ActionResult Index(PenaltyTypeViewModel model)
        {
            var totalRecords = 0;
            model.PenaltyTypes = _penaltyTypeManager.GetAllPenaltyTypeByPaging(model, out totalRecords);
            model.TotalRecords = totalRecords;
            return View(model);
        }

        public ActionResult Save(PenaltyTypeViewModel model)
        {
            var saveStatus = 0;
            try
            {
                if (model.PenaltyTypeId > 0)
                {
                    saveStatus = _penaltyTypeManager.EditPenaltyType(model);
                }
                else
                {
                    bool isExist = _penaltyTypeManager.IsPenaltyTypeExist(model);
                    if (!isExist)
                    {
                     var penaltyType = new HrmPenaltyType { Type = model.Type, Description = model.Description };
                    saveStatus = _penaltyTypeManager.SavePenaltyType(penaltyType);
                    }
                    else
                    {
                        return ErrorResult("Penalty Type :" + model.Type + " " + "Already Exist ! Please Entry another one");
                    }
                    
                }
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return saveStatus > 0 ? Reload() : ErrorResult("Failed to Save PenaltyType !");
        }

        public ActionResult Edit(PenaltyTypeViewModel model)
        {
            ModelState.Clear();
            if (model.PenaltyTypeId > 0)
            {
                HrmPenaltyType penaltyType = _penaltyTypeManager.GetPenaltyTypeByPenaltyTypeId(model.PenaltyTypeId);
                model.Type = penaltyType.Type;
                model.Description = penaltyType.Description;
            }
            return View(model);
        }

        public ActionResult Delete(int penaltyTypeId)
        {
            var index = _penaltyTypeManager.DeletePenaltyType(penaltyTypeId);
            return index > 0 ? Reload() : ErrorResult("Failed to delete!");
        }
    }
}