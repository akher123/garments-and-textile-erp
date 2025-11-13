using SCERP.Common;
using SCERP.Common.ReportHelper;
using SCERP.Model;
using SCERP.Web.Areas.HRM.Models.ViewModels;
using SCERP.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace SCERP.Web.Areas.HRM.Controllers
{

    public class DepartmentController : BaseController
    {
        private readonly int _pageSize = AppConfig.PageSize;

        [AjaxAuthorize(Roles = "department-1,department-2,department-3")]
        public ActionResult Index(DepartmentViewModel model)
        {

            ModelState.Clear();
            Department department = model;
            department.Name = model.SearchKey;

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
            model.Departments = DepartmentManager.GetAllDepartmentsByPaging(startPage, _pageSize, out totalRecords, department) ?? new List<Department>();
            model.TotalRecords = totalRecords;

            return View(model);
        }

        [AjaxAuthorize(Roles = "department-2,department-3")]
        public ActionResult Edit(DepartmentViewModel model)
        {
            ModelState.Clear();

            try
            {
                if (model.Id > 0)
                {
                    var department = DepartmentManager.GetDepartmentById(model.Id);
                    model.Name = department.Name;
                    model.NameInBengali = department.NameInBengali;
                    model.Description = department.Description;
                    ViewBag.Title = "Edit Department";
                }
                else
                {
                    ViewBag.Title = "Add Department";
                }

            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return View(model);
        }

        [AjaxAuthorize(Roles = "department-2,department-3")]
        public ActionResult Save(DepartmentViewModel model)
        {
            var isExist = DepartmentManager.CheckExistingDepartment(model);

            if (isExist)
            {
                return ErrorResult(model.Name + " " + "Department already exist");
            }
            var department = DepartmentManager.GetDepartmentById(model.Id) ?? new Department();
            department.Name = model.Name;
            department.NameInBengali = model.NameInBengali;
            department.Description = model.Description;

            var saveIndex = (model.Id > 0) ? DepartmentManager.EditDepartment(department) : DepartmentManager.SaveDepartment(department);
            return (saveIndex > 0) ? Reload() : ErrorResult("Failed to save data");

        }

        [AjaxAuthorize(Roles = "department-3")]
        public ActionResult Delete(int id)
        {
            var deleted = 0;
            var department = DepartmentManager.GetDepartmentById(id) ?? new Department();
            deleted = DepartmentManager.DeleteDepartment(department);
            return (deleted > 0) ? Reload() : ErrorResult("Failed to delete data");
        }

        [AjaxAuthorize(Roles = "department-1,department-2,department-3")]
        public void GetExcel(DepartmentViewModel model)
        {
            List<Department> departments = DepartmentManager.GetDepartmentBySearchKey(model.SearchKey);
            model.Departments = departments;

            const string fileName = "Departments";
            var boundFields = new List<BoundField>
            {
                new BoundField(){HeaderText = @"Department Name", DataField = "Name"},
                new BoundField(){HeaderText = @"Description", DataField = "Description"}
            };
            ReportConverter.CustomGridView(boundFields, model.Departments, fileName);
        }

        [AjaxAuthorize(Roles = "department-1,department-2,department-3")]
        public ActionResult Print(DepartmentViewModel model)
        {
            List<Department> departments = DepartmentManager.GetDepartmentBySearchKey(model.SearchKey);
            model.Departments = departments;
            return View("_DepartmentPdfReport", model);
        }

    }
}
