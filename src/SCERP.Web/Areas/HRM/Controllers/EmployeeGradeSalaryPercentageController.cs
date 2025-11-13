using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using SCERP.BLL.Manager.HRMManager;
using SCERP.Common;
using SCERP.Common.ReportHelper;
using SCERP.Model;
using SCERP.Model.Custom;
using SCERP.Web.Areas.HRM.Models.ViewModels;
using SCERP.Web.Controllers;

namespace SCERP.Web.Areas.HRM.Controllers
{
    public class EmployeeGradeSalaryPercentageController : BaseHrmController
    {
        private readonly int _pageSize = AppConfig.PageSize;

        [AjaxAuthorize(Roles = "salarypercentage-1,salarypercentage-2,salarypercentage-3")]
        public ActionResult Index(EmployeeGradeSalaryPercentageViewModel model)
        {
            try
            {
                ModelState.Clear();
                var totalRecords = 0;
                var startPage = 0;
                if (model.page.HasValue && model.page.Value > 0)
                {
                    startPage = model.page.Value - 1;
                }
                model.EmployeeTypes = EmployeeTypeManager.GetAllEmployeeTypes();
                if (model.IsSearch)
                {
                    model.IsSearch = false;
                    return View(model);
                }
                model.EmployeeGrades = EmployeeGradeManager.GetEmployeeGradeByEmployeeTypeId(model.SearchFieldModel.SearchByEmployeeTypeId);
                model.EmployeeGradeSalaryPercentages = EmployeeGradeSalaryPercentageManager.GetEmployeeGradeSalaryPercentages(startPage, _pageSize, model, model.SearchFieldModel, out totalRecords);
                model.TotalRecords = totalRecords;
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return View(model);
        }

        [AjaxAuthorize(Roles = "salarypercentage-2,salarypercentage-3")]
        public ActionResult Edit(EmployeeGradeSalaryPercentageViewModel model)
        {
            ModelState.Clear();
            model.EmployeeTypes = EmployeeTypeManager.GetAllEmployeeTypes();

            if (model.EmployeeGradeSalaryPercentageId > 0)
            {
                var employeeTypeSalaryPercentage = EmployeeGradeSalaryPercentageManager.GetEmployeeGradeSalaryPercentageById(model.EmployeeGradeSalaryPercentageId);
                model.EmployeeGradeId = employeeTypeSalaryPercentage.EmployeeGradeId;
                model.EmployeeTypeId = employeeTypeSalaryPercentage.EmployeeGrade.EmployeeTypeId;
                model.Medical = employeeTypeSalaryPercentage.Medical;
                model.Conveyance = employeeTypeSalaryPercentage.Conveyance;
                model.Food = employeeTypeSalaryPercentage.Food;
                model.HouseRentPercentage = employeeTypeSalaryPercentage.HouseRentPercentage;
                model.BasicPercentageRate = employeeTypeSalaryPercentage.BasicPercentageRate;
                model.EmployeeGrades = EmployeeGradeManager.GetEmployeeGradeByEmployeeTypeId(model.EmployeeTypeId);
            }

            return View(model);
        }

        [AjaxAuthorize(Roles = "salarypercentage-2,salarypercentage-3")]
        public ActionResult Save(EmployeeGradeSalaryPercentage model)
        {
            var saveIndex = 0;
            try
            {
                saveIndex = model.EmployeeGradeSalaryPercentageId > 0 ? EmployeeGradeSalaryPercentageManager.EditEmployeeGradeSalaryPercentage(model) : EmployeeGradeSalaryPercentageManager.SaveEmployeeGradeSalaryPercentage(model);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return (saveIndex > 0) ? Reload() : ErrorResult("Failed to save data");

        }

         [AjaxAuthorize(Roles = "salarypercentage-3")]
        public ActionResult Delete(EmployeeGradeSalaryPercentage model)
        {
            var deleteIndex = 0;
            try
            {
                deleteIndex = EmployeeGradeSalaryPercentageManager.DeleteEmployeeGradeSalaryPercentage(model.EmployeeGradeSalaryPercentageId);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return (deleteIndex > 0) ? Reload() : ErrorResult("Failed to delete data");

        }

        [AjaxAuthorize(Roles = "salarypercentage-1,salarypercentage-2,salarypercentage-3")]
        public void GetExcel(EmployeeGradeSalaryPercentageViewModel model, SearchFieldModel searchFieldModel)
        {
            try
            {

                model.EmployeeGradeSalaryPercentages = EmployeeGradeSalaryPercentageManager.GetEmployeeGradeSalaryPercentageBySearchKey(searchFieldModel);
                const string fileName = @"EmployeeTypeSalaryPercentage List";
                var boundFields = new List<BoundField>
            {
               new BoundField(){HeaderText = @"EmployeeType",DataField = "EmployeeGrade.EmployeeType.Title"},
                   new BoundField(){HeaderText = @"EmployeeGrade",DataField = "EmployeeGrade.Name"},
              new BoundField(){HeaderText = @"Medical",DataField = "Medical"},
              new BoundField(){HeaderText = @"Conveyance",DataField = "Conveyance"},
               new BoundField(){HeaderText = @"Food",DataField = "Food"},
                  new BoundField(){HeaderText = @"House Rent(%)",DataField = "HouseRentPercentage"},
                   new BoundField(){HeaderText = @"Basic salary Rate",DataField ="BasicPercentageRate"},
   
            };
                ReportConverter.CustomGridView(boundFields, model.EmployeeGradeSalaryPercentages, fileName);
            }
            catch (Exception exception)
            {

                Errorlog.WriteLog(exception);
            }

        }

        [AjaxAuthorize(Roles = "salarypercentage-1,salarypercentage-2,salarypercentage-3")]
        public ActionResult Print(EmployeeGradeSalaryPercentageViewModel model, SearchFieldModel searchField)
        {
            try
            {
                model.EmployeeGradeSalaryPercentages = EmployeeGradeSalaryPercentageManager.GetEmployeeGradeSalaryPercentageBySearchKey(searchField);
            }
            catch (Exception exception)
            {

                Errorlog.WriteLog(exception);
            }
            return PartialView("_EmployeeGradeSalaryPercentageReport", model);
        }


        public JsonResult IsEmployeeTypeExist(EmployeeGradeSalaryPercentageViewModel model)
        {
            bool isExist = !EmployeeGradeSalaryPercentageManager.IsEmployeeGradeExist(model);
            return Json(isExist, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetEmpGradeByEmpType(int id)
        {
            var grade = EmployeeGradeManager.GetEmployeeGradeByEmployeeTypeId(id);
            return Json(new { Success = true, gradeList = grade }, JsonRequestBehavior.AllowGet);
        }


    }
}