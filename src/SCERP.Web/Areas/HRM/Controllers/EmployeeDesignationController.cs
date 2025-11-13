using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using SCERP.Common;
using SCERP.Common.ReportHelper;
using SCERP.Model;
using SCERP.Web.Controllers;

namespace SCERP.Web.Areas.HRM.Controllers
{
    public class EmployeeDesignationController : BaseHrmController
    {
        private readonly int _pageSize = AppConfig.PageSize;

        [AjaxAuthorize(Roles = "employeedesignation-1,employeedesignation-2,employeedesignation-3")]
        public ActionResult Index(Models.ViewModels.EmployeeDesignationViewModel model)
        {
            ModelState.Clear();

            var employeeTypes = EmployeeTypeManager.GetAllEmployeeTypes();
            ViewBag.SearchByEmployeeTypeId = new SelectList(employeeTypes, "Id", "Title", model.SearchByEmployeeTypeId);

            var employeeTypeId = model.SearchByEmployeeTypeId;
            var employeeGrades = EmployeeGradeManager.GetEmployeeGradeByEmployeeTypeId(employeeTypeId);
            ViewBag.SearchByEmployeeGradeId = new SelectList(employeeGrades, "Id", "Name", model.SearchByEmployeeGradeId);

            EmployeeDesignation employeeDesignation = model;
            employeeDesignation.EmployeeTypeId = employeeTypeId;
            employeeDesignation.GradeId = model.SearchByEmployeeGradeId;
            employeeDesignation.Title = model.SearchByEmployeeDesignationTitle;

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
            model.EmployeeDesignations = EmployeeDesignationManager.GetAllEmployeeDesignationsByPaging(startPage, _pageSize, employeeDesignation, out totalRecords) ?? new List<EmployeeDesignation>();
            model.TotalRecords = totalRecords;

            return View(model);
        }

        [AjaxAuthorize(Roles = "employeedesignation-2,employeedesignation-3")]
        public ActionResult Edit(EmployeeDesignation model)
        {
            ModelState.Clear();
            var employeeTypes = EmployeeTypeManager.GetAllEmployeeTypes();
            try
            {
                if (model.Id > 0)
                {
                    var designation = EmployeeDesignationManager.GetEmployeeDesignationById(model.Id);
                    ViewBag.EmployeeTypeItemList = new SelectList(employeeTypes, "Id", "Title", designation.EmployeeTypeId);
                    var grades = EmployeeGradeManager.GetEmployeeGradeByEmployeeTypeId(designation.EmployeeTypeId);
                    ViewBag.EmployeeGradeItemList = new SelectList(grades, "Id", "Name", designation.GradeId);
                    model.Title = designation.Title;
                    model.TitleInBengali = designation.TitleInBengali;
                    model.Description = designation.Description;
                    ViewBag.Title = "Edit";
                }
                else
                {
                    ViewBag.EmployeeTypeItemList = new SelectList(employeeTypes, "Id", "Title");
                    ViewBag.EmployeeGradeItemList = new SelectList(new List<EmployeeGrade>(), "Id", "Name");
                    ViewBag.Title = "Add";
                }
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return View(model);

        }

        [AjaxAuthorize(Roles = "employeedesignation-2,employeedesignation-3")]
        public ActionResult Save(EmployeeDesignation model)
        {
            var isExist = EmployeeDesignationManager.CheckExistingEmployeeDesignation(model);

            if (isExist)
            {
                return ErrorResult(model.Title + " " + "Designation already exist");
            }

            var employeeDesignation = EmployeeDesignationManager.GetEmployeeDesignationById(model.Id) ?? new EmployeeDesignation();
            employeeDesignation.EmployeeTypeId = model.EmployeeType.Id;
            employeeDesignation.GradeId = model.EmployeeGrade.Id;
            employeeDesignation.Title = model.Title;
            employeeDesignation.TitleInBengali = model.TitleInBengali;
            employeeDesignation.Description = model.Description;

            var saveIndex = (model.Id > 0) ? EmployeeDesignationManager.EditEmployeeDesignation(employeeDesignation) : EmployeeDesignationManager.SaveEmployeeDesignation(employeeDesignation);
            return (saveIndex > 0) ? Reload() : ErrorResult("Failed to save data");
        }

        [AjaxAuthorize(Roles = "employeedesignation-3")]
        public ActionResult Delete(int id)
        {
            var deleted = 0;
            var employeedesignation = EmployeeDesignationManager.GetEmployeeDesignationById(id) ?? new EmployeeDesignation();
            deleted = EmployeeDesignationManager.DeleteEmployeeDesignation(employeedesignation);
            return (deleted > 0) ? Reload() : ErrorResult("Failed to delete data");
        }

        [AjaxAuthorize(Roles = "employeedesignation-1,employeedesignation-2,employeedesignation-3")]
        public void GetExcel(Models.ViewModels.EmployeeDesignationViewModel model)
        {
            List<EmployeeDesignation> employeeDesignations = EmployeeDesignationManager.GetAllEmployeeDesignationsBySearchKey(model.SearchByEmployeeTypeId, model.SearchByEmployeeGradeId, model.SearchByEmployeeDesignationTitle);
            model.EmployeeDesignations = employeeDesignations;

            const string fileName = "EmployeeDesignation";
            var boundFields = new List<BoundField>
            {
               new BoundField(){HeaderText = @"Designation",DataField = "Title"},
               new BoundField(){HeaderText = @"Employee Type",DataField = "EmployeeType.Title"},
               new BoundField(){HeaderText = @"Employee Grade",DataField = "EmployeeGrade.Name"},
               new BoundField(){HeaderText = @"Description",DataField = "Description"},
            };
            ReportConverter.CustomGridView(boundFields, model.EmployeeDesignations, fileName);
        }

        [AjaxAuthorize(Roles = "employeedesignation-1,employeedesignation-2,employeedesignation-3")]
        public ActionResult Print(Models.ViewModels.EmployeeDesignationViewModel model)
        {
            List<EmployeeDesignation> employeeDesignations = EmployeeDesignationManager.GetAllEmployeeDesignationsBySearchKey(model.SearchByEmployeeTypeId, model.SearchByEmployeeGradeId, model.SearchByEmployeeDesignationTitle);
            model.EmployeeDesignations = employeeDesignations;

            return View("_EmployeeDesignationPdfReportViewer", model);
        }

        public ActionResult GetGradeByEmployeeTypeId(int id)
        {
            var employeeGrades = EmployeeGradeManager.GetEmployeeGradeByEmployeeTypeId(id);
            return Json(employeeGrades, JsonRequestBehavior.AllowGet);
        }

    }
}
