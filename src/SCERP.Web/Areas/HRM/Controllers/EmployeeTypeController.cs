using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using SCERP.Common;
using SCERP.Common.ReportHelper;
using SCERP.Model;
using SCERP.Web.Areas.HRM.Models.ViewModels;
using SCERP.Web.Controllers;


namespace SCERP.Web.Areas.HRM.Controllers
{
    public class EmployeeTypeController : BaseHrmController
    {
        private readonly int _pageSize = AppConfig.PageSize;

        [AjaxAuthorize(Roles = "employeetype-1,employeetype-2,employeetype-3")]
        public ActionResult Index(EmployeeTypeViewModel model)
        {

            ModelState.Clear();
            EmployeeType employeeType = model;
            employeeType.Title = model.SearchKey;

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
            model.EmployeeTypes = EmployeeTypeManager.GetAllEmployeeTypesByPaging(startPage, _pageSize, out totalRecords, employeeType) ?? new List<EmployeeType>();
            model.TotalRecords = totalRecords;

            return View(model);
        }

        [AjaxAuthorize(Roles = "employeetype-2,employeetype-3")]
        public ActionResult Edit(EmployeeTypeViewModel model)
        {
            ModelState.Clear();

            try
            {
                if (model.Id > 0)
                {
                    var employeeType = EmployeeTypeManager.GetEmployeeTypeById(model.Id);
                    model.Title = employeeType.Title;
                    model.TitleInBengali = employeeType.TitleInBengali;
                    model.Description = employeeType.Description;
                    ViewBag.Title = "Edit";
                }
                else
                {
                    ViewBag.Title = "Add";
                }

            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return View(model);
        }

        [AjaxAuthorize(Roles = "employeetype-2,employeetype-3")]
        public ActionResult Save(EmployeeTypeViewModel model)
        {
            var isExist = EmployeeTypeManager.CheckExistingEmployeeType(model);

            if (isExist)
            {
                return ErrorResult(model.Title + " " + "Employee Type already exist");
            }
            var employeeType = EmployeeTypeManager.GetEmployeeTypeById(model.Id) ?? new EmployeeType();
            employeeType.Title = model.Title;
            employeeType.TitleInBengali = model.TitleInBengali;
            employeeType.Description = model.Description;

            var saveIndex = (model.Id > 0) ? EmployeeTypeManager.EditEmployeeType(employeeType) : EmployeeTypeManager.SaveEmployeeType(employeeType);
            return (saveIndex > 0) ? Reload() : ErrorResult("Failed to save data");

        }

        [AjaxAuthorize(Roles = "employeetype-3")]
        public ActionResult Delete(int id)
        {
            var deleted = 0;
            var employeeType = EmployeeTypeManager.GetEmployeeTypeById(id) ?? new EmployeeType();
            deleted = EmployeeTypeManager.DeleteEmployeeType(employeeType);
            return (deleted > 0) ? Reload() : ErrorResult("Failed to delete data");
        }

        [AjaxAuthorize(Roles = "employeetype-1,employeetype-2,employeetype-3")]
        public void GetExcel(EmployeeTypeViewModel model)
        {
            List<EmployeeType> employeeTypes = EmployeeTypeManager.GetEmployeeTypeBySearchKey(model.SearchKey);
            model.EmployeeTypes = employeeTypes;

            const string fileName = "Departments";
            var boundFields = new List<BoundField>
            {
                new BoundField(){HeaderText = "Employee Type", DataField = "Title"},
                new BoundField(){HeaderText = "Description", DataField = "Description"}
            };
            ReportConverter.CustomGridView(boundFields, model.EmployeeTypes, fileName);
        }

        [AjaxAuthorize(Roles = "employeetype-1,employeetype-2,employeetype-3")]
        public ActionResult Print(EmployeeTypeViewModel model)
        {
            List<EmployeeType> employeeTypes = EmployeeTypeManager.GetEmployeeTypeBySearchKey(model.SearchKey);
            model.EmployeeTypes = employeeTypes;

            return View("_EmployeeTypePdfReport", model);
        }
    }
}
