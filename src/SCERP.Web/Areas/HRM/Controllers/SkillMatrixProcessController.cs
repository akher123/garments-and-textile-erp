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
    public class SkillMatrixProcessController : BaseController
    {
        private readonly ISkillMatrixProcessManager _skillMatrixProcessManager;

        public SkillMatrixProcessController(ISkillMatrixProcessManager skillMatrixProcessManager)
        {
            _skillMatrixProcessManager = skillMatrixProcessManager;
        }
        public ActionResult Index(SkillMatrixProcessViewModel model)
        {
            try
            {
                var totalRecords = 0;
                model.SkillMatrixProcesses = _skillMatrixProcessManager.GetAllSkillMatrixProcessByPaging(model.PageIndex, model.sort, model.sortdir, out totalRecords, model.SearchString);
                model.TotalRecords = totalRecords;
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult("Fail To Retrive Data :" + exception);
            }
            return View(model);
        }

        public ActionResult Edit(SkillMatrixProcessViewModel model)
        {
            ModelState.Clear();
            try
            {
                if (model.SkillMatrixProcess.SkillMatrixProcessId > 0)
                {
                    HrmSkillMatrixProcess skillMatrixProcess = _skillMatrixProcessManager.GetSkillMatrixProcessBySkillMatrixProcessId(model.SkillMatrixProcess.SkillMatrixProcessId, PortalContext.CurrentUser.CompId);
                    model.SkillMatrixProcess.ProcessName = skillMatrixProcess.ProcessName;
                    model.SkillMatrixProcess.DisplayOrder = skillMatrixProcess.DisplayOrder;
                    model.SkillMatrixProcess.IsActive = skillMatrixProcess.IsActive;
                }
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult("Fail To Retrive Data :" + exception);
            }
            
            return View(model);
        }
        [HttpPost]
        public ActionResult Save(SkillMatrixProcessViewModel model)
        {
            ModelState.Clear();
            var index = 0;
            try
            {
                if (_skillMatrixProcessManager.IsSkillMatrixProcessExist(model.SkillMatrixProcess))
                {
                    return ErrorResult("Already Exist ! Please Entry another one");
                }
                else
                {
                    index = model.SkillMatrixProcess.SkillMatrixProcessId > 0 ? _skillMatrixProcessManager.EditSkillMatrixProcess(model.SkillMatrixProcess) : _skillMatrixProcessManager.SaveSkillMatrixProcess(model.SkillMatrixProcess);
                }
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult("Failed to Save/Edit Skill Matrix Process :" + exception);
            }
            return index > 0 ? Reload() : ErrorResult("Failed to Save/Edit Skill Matrix Process !");
        }

        [HttpGet]
        public ActionResult Delete(int skillMatrixProcessId)
        {
            int index = 0;
             index = _skillMatrixProcessManager.DeleteSkillMatrixProcess(skillMatrixProcessId);
            return index > 0 ? Reload() : ErrorResult("Fail To Delete Skill Matrix Process !");
        }
	}
}