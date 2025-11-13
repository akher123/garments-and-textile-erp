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
    public class SkillMatrixGradeController : BaseController
    {
        private readonly ISkillMatrixGradeManager _skillMatrixGradeManager;

        public SkillMatrixGradeController(ISkillMatrixGradeManager skillMatrixGradeManager)
        {
            _skillMatrixGradeManager = skillMatrixGradeManager;
        }
        public ActionResult Index(SkillMatrixGradeViewModel model)
        {
            try
            {
                var totalRecords = 0;
                model.SkillMatrixGrades = _skillMatrixGradeManager.GetAllSkillMatrixGradeByPaging(model.PageIndex, model.sort, model.sortdir, out totalRecords, model.SearchString);
                model.TotalRecords = totalRecords;
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult("Fail To Retrive Data :" + exception);
            }
            return View(model);
        }

        public ActionResult Edit(SkillMatrixGradeViewModel model)
        {
            ModelState.Clear();
            try
            {
                if (model.SkillMatrixGrade.SkillMatrixGradeId > 0)
                {
                    HrmSkillMatrixGrade skillMatrixGrade = _skillMatrixGradeManager.GetSkillMatrixGradeBySkillMatrixGradeId(model.SkillMatrixGrade.SkillMatrixGradeId, PortalContext.CurrentUser.CompId);
                    model.SkillMatrixGrade.GradeName = skillMatrixGrade.GradeName;
                    model.SkillMatrixGrade.GradeValueFrom = skillMatrixGrade.GradeValueFrom;
                    model.SkillMatrixGrade.GradeValueTo = skillMatrixGrade.GradeValueTo;
                    model.SkillMatrixGrade.IsActive = skillMatrixGrade.IsActive;
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
        public ActionResult Save(SkillMatrixGradeViewModel model)
        {
            ModelState.Clear();
            var index = 0;
            try
            {
                bool isExist = _skillMatrixGradeManager.IsSkillMatrixGradeExist(model.SkillMatrixGrade);
                if (isExist)
                {
                    return ErrorResult("Already Exist ! Please Entry another one");
                }
                else
                {
                    index = model.SkillMatrixGrade.SkillMatrixGradeId > 0 ? _skillMatrixGradeManager.EditSkillMatrixGrade(model.SkillMatrixGrade) : _skillMatrixGradeManager.SaveSkillMatrixGrade(model.SkillMatrixGrade);
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
        public ActionResult Delete(int skillMatrixGradeId)
        {
            var index = 0;
            try
            {
                index = _skillMatrixGradeManager.DeleteSkillMatrixGrade(skillMatrixGradeId);
                if (index == -1)// This hour Id used by another table
                {
                    return ErrorResult("Could not possible to delete Skill Matrix Process because of it's already used.");
                }
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult("Fail To Delete Skill Matrix Grade :" + exception);
            }
            return index > 0 ? Reload() : ErrorResult("Fail To Delete Skill Matrix Grade !");
        }
    }
}