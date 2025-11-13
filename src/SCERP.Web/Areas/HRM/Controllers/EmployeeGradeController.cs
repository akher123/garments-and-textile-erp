using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SCERP.BLL.IManager.IHRMManager;
using SCERP.Common;
using SCERP.Common.ReportHelper;
using SCERP.Model;
using SCERP.Web.Areas.HRM.Models.ViewModels;
using SCERP.Web.Controllers;
using System.Web.UI.WebControls;


namespace SCERP.Web.Areas.HRM.Controllers
{
    public class EmployeeGradeController : BaseHrmController
    {
        private readonly int _pageSize = AppConfig.PageSize;

        [AjaxAuthorize(Roles = "employeegrade-1,employeegrade-2,employeegrade-3")]
        public ActionResult Index(EmployeeGradeViewModel model)
        {
            ModelState.Clear();

            var employeeTypeList = EmployeeTypeManager.GetAllEmployeeTypes();
            ViewBag.SearchByEmployeeType = new SelectList(employeeTypeList, "Id", "Title");

            EmployeeGrade employeeGrade = model;
            employeeGrade.Name = model.SearchByEmployeeGrade;
            employeeGrade.EmployeeTypeId = model.SearchByEmployeeType;

            //if (model.IsSearch)
            //{
            //    model.IsSearch = false;
            //    return View(model);
            //}

            var startPage = 0;
            if (model.page.HasValue && model.page.Value > 0)
            {
                startPage = model.page.Value - 1;
            }

            int totalRecords = 0;
            model.EmployeeGrades = EmployeeGradeManager.GetAllEmployeeGradesByPaging(startPage, _pageSize, employeeGrade, out totalRecords) ?? new List<EmployeeGrade>();
            model.TotalRecords = totalRecords;

            return View(model);
        }


        [AjaxAuthorize(Roles = "employeegrade-2,employeegrade-3")]
        public ActionResult Edit(EmployeeGradeViewModel model)
        {
            ModelState.Clear();
            try
            {
                var employeeTypes = EmployeeTypeManager.GetAllEmployeeTypes();
                model.EmployeeTypes = employeeTypes;
                if (model.Id > 0)
                {
                    var employeeGrade = EmployeeGradeManager.GetEmployeeGradeById(model.Id);
                    model.Name = employeeGrade.Name;
                    model.EmployeeTypeId = employeeGrade.EmployeeTypeId;
                    model.NameInBengali = employeeGrade.NameInBengali;
                    model.Description = employeeGrade.Description;
                    model.EmployeeTypes = employeeTypes;

                }
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return View(model);
        }


        [AjaxAuthorize(Roles = "employeegrade-2,employeegrade-3")]
        public ActionResult Save(EmployeeGradeViewModel model)
        {
            var isExist = EmployeeGradeManager.CheckExistingEmployeeGrade(model);

            if (isExist)
            {
                return ErrorResult(model.Name + " " + "Employee Grade already exist");
            }

            var employeeGrade = EmployeeGradeManager.GetEmployeeGradeById(model.Id) ?? new EmployeeGrade();
            employeeGrade.Name = model.Name;
            employeeGrade.NameInBengali = model.NameInBengali;
            employeeGrade.EmployeeTypeId = model.EmployeeTypeId;
            employeeGrade.Description = model.Description;

            var saveIndex = (model.Id > 0) ? EmployeeGradeManager.EditEmployeeGrade(employeeGrade) : EmployeeGradeManager.SaveEmployeeGrade(employeeGrade);
            return (saveIndex > 0) ? Reload() : ErrorResult("Failed to save data");
        }


        [AjaxAuthorize(Roles = "employeegrade-3")]
        public ActionResult Delete(int id)
        {
            var deleted = 0;
            var employeeGrade = EmployeeGradeManager.GetEmployeeGradeById(id) ?? new EmployeeGrade();
            deleted = EmployeeGradeManager.DeleteEmployeeGrade(employeeGrade);
            return (deleted > 0) ? Reload() : ErrorResult("Failed to delete data");
        }

        [AjaxAuthorize(Roles = "employeegrade-1,employeegrade-2,employeegrade-3")]
        public void GetExcel(EmployeeGradeViewModel model)
        {
            List<EmployeeGrade> employeeGrades = EmployeeGradeManager.GetAllEmployeeGradesBySearchKey(model.SearchByEmployeeGrade, model.SearchByEmployeeType);
            model.EmployeeGrades = employeeGrades;

            const string fileName = "EmployeeGrades";
            var boundFields = new List<BoundField>
            {
                new BoundField(){HeaderText = @"Employee Grade", DataField = "Name"},
                new BoundField(){HeaderText = @"Employee Type", DataField = "EmployeeType.Title"},
                new BoundField(){HeaderText = @"Minimum Salary", DataField = "MinimumSalary"},
                new BoundField(){HeaderText = @"Description", DataField = "Description"},
            };
            ReportConverter.CustomGridView(boundFields, model.EmployeeGrades, fileName);
        }

        [AjaxAuthorize(Roles = "employeegrade-1,employeegrade-2,employeegrade-3")]
        public ActionResult Print(EmployeeGradeViewModel model)
        {
            List<EmployeeGrade> employeeGrades = EmployeeGradeManager.GetAllEmployeeGradesBySearchKey(model.SearchByEmployeeGrade, model.SearchByEmployeeType);
            model.EmployeeGrades = employeeGrades;
            return View("_EmployeeGradePdfReport", model);
        }

    }
}
