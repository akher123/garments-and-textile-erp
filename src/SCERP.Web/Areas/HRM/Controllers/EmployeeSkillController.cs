using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCERP.BLL.Manager.HRMManager;
using SCERP.Common;
using SCERP.Model;
using SCERP.Web.Areas.HRM.Models.ViewModels;
using SCERP.Web.Controllers;

namespace SCERP.Web.Areas.HRM.Controllers
{
    public class EmployeeSkillController : BaseHrmController
    {
        private readonly int _pageSize = AppConfig.PageSize;


        [AjaxAuthorize(Roles = "employeeskill-1,employeeskill-2,employeeskill-3")]
        public ActionResult Index(EmployeeSkillViewModel model)
        {
            try
            {
                ModelState.Clear();

                if (!model.IsSearch)
                {
                    model.IsSearch = true;
                    return View(model);
                }
                var startPage = 0;
                if (model.page.HasValue && model.page.Value > 0)
                {
                    startPage = model.page.Value - 1;
                }
                var totalRecords = 0;
                model.VEmployeeSkillDetails = EmployeeSkillManager.GetAllEmployeeSkillDetails(startPage, _pageSize, model, model.SearchFieldModel, out totalRecords);
                model.TotalRecords = totalRecords;
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return View(model);
        }

        [AjaxAuthorize(Roles = "employeeskill-2,employeeskill-3")]
        public ActionResult Edit(EmployeeSkillViewModel model)
        {
            try
            {
                var skillsetdifficulty = SkillSetDifficultyManager.GetAllSkillSetDifficulty();
                var skillsetcategories = SkillSetCategoryManager.GetAllSkillSetCategory();
                var skilloperation = SkillOperationManager.GetAllSkillOperationManager();

                model.SkillOperations = skilloperation;
                model.SkillSetCategories = skillsetcategories;
                model.SkillSetDifficulties = skillsetdifficulty;

                if (model.EmployeeSkillId > 0)
                {
                    var employeeskill = EmployeeSkillManager.GetEmployeeSkillById(model.EmployeeSkillId);
                    model.EmployeeSkillId = employeeskill.EmployeeSkillId;
                    model.EmployeeCardId = employeeskill.Employee.EmployeeCardId;
                    model.VEmployeeCompanyInfoDetail = EmployeeDailyAttendanceManager.GetEmployeeByEmployeeCardId(employeeskill.Employee.EmployeeCardId);
                    model.SkillSetDifficultyId = employeeskill.SkillOperation.SkillSetDifficultyId;
                    model.CategoryId = employeeskill.SkillOperation.CategoryId;
                    model.SkillOperationId = employeeskill.SkillOperationId;
                    model.Efficiency = employeeskill.Efficiency;
                    model.FromDate = employeeskill.FromDate;
                    model.ToDate = employeeskill.ToDate;
                }
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return View(model);
        }

        [AjaxAuthorize(Roles = "employeeskill-1,employeeskill-2,employeeskill-3")]
        public ActionResult Save(EmployeeSkillViewModel model)
        {
            var saveIndex = 0;
            try
            {
                var employeeSkill = EmployeeSkillManager.GetEmployeeSkillById(model.EmployeeSkillId) ?? new EmployeeSkill();
                employeeSkill.EmployeeId = model.EmployeeId;
                employeeSkill.SkillOperationId = model.SkillOperationId;
                employeeSkill.Efficiency = model.Efficiency;
                employeeSkill.FromDate = model.FromDate;
                employeeSkill.ToDate = model.ToDate;
                saveIndex = model.EmployeeSkillId > 0 ? EmployeeSkillManager.EditEmployeeSkill(employeeSkill) : EmployeeSkillManager.SaveEmployeeSkill(employeeSkill);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return (saveIndex > 0) ? Reload() : ErrorResult("Failed to save data");
        }

        [AjaxAuthorize(Roles = "employeeskill-3")]
        public ActionResult Delete(EmployeeSkillViewModel model)
        {
            var deleted = 0;

            try
            {
                deleted = EmployeeSkillManager.DeleteEmployeeSkillById(model.EmployeeSkillId);
            }
            catch (Exception exception)
            {

                Errorlog.WriteLog(exception);
            }

            return (deleted > 0) ? Reload() : ErrorResult("Failed to delete data!");

        }

        public ActionResult GetEmployeeDetailByEmployeeCadId(EmployeeSkillViewModel model)
        {
            var checkEmployeeCardId =
                EmployeeManager.CheckExistingEmployeeCardNumber(new Employee() { EmployeeCardId = model.EmployeeCardId });
            if (!checkEmployeeCardId)
            {

                return Json(new { Message = "Employee not found aggainst this Employee ID !", ValidStatus = false }, JsonRequestBehavior.AllowGet);
            }
            var employeeDetails = EmployeeDailyAttendanceManager.GetEmployeeByEmployeeCardId(model.EmployeeCardId);

            return Json(new { EmployeeDetailView = RenderViewToString("_EmployeeDetails", employeeDetails), Success = true }, JsonRequestBehavior.AllowGet);
        }

    }
}