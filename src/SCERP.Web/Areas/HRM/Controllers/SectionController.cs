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
    public class SectionController : BaseController
    {
        private readonly int _pageSize = AppConfig.PageSize;

        [AjaxAuthorize(Roles = "section-1,section-2,section-3")]
        public ActionResult Index(SectionViewModel model)
        {
            try
            {
                ModelState.Clear();
                Section section = model;
                section.Name = model.SearchKey;

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
                model.SectionList = SectionManager.GetAllSections(startPage, _pageSize, model, out totalRecords);
                model.TotalRecords = totalRecords;
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return View(model);
        }

        [AjaxAuthorize(Roles = "section-2,section-3")]
        public ActionResult Edit(SectionViewModel model)
        {
            ModelState.Clear();
            try
            {
                if (model.SectionId > 0)
                {
                    var section = SectionManager.GetSectionById(model.SectionId);
                    model.Name = section.Name;
                    model.NameInBengali = section.NameInBengali;
                    model.Description = section.Description;
                    model.CreatedBy = section.CreatedBy;
                    model.EditedBy = section.EditedBy;
                    model.CreatedDate = section.CreatedDate;
                    model.EditedDate = section.EditedDate;
                    model.IsActive = section.IsActive;
                }
                model.Departments = DepartmentManager.GetAllDepartments();
                model.Branches = BranchManager.GetAllBranches();
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return View(model);
        }

        [AjaxAuthorize(Roles = "section-2,section-3")]
        public ActionResult Save(Section model)
        {
            var saveIndex = 0;
            bool isExist = SectionManager.IsExistSection(model);
            try
            {
                switch (isExist)
                {
                    case false:
                        {
                            saveIndex = model.SectionId > 0 ? SectionManager.EditSection(model) : SectionManager.SaveSection(model);
                        }
                        break;
                    default:
                        return ErrorResult(string.Format("{0} Section already exist!", model.Name));
                }

            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return (saveIndex > 0) ? Reload() : ErrorResult("Failed to save data");
        }

        [AjaxAuthorize(Roles = "section-3")]
        public ActionResult Delete(Section section)
        {
            var deleteIndex = 0;
            try
            {
                deleteIndex = SectionManager.DeleteSection(section.SectionId);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return (deleteIndex > 0) ? Reload() : ErrorResult("Failed to delete data");

        }

        [AjaxAuthorize(Roles = "section-1,section-2,section-3")]
        public void GetExcel(SectionViewModel model)
        {
            try
            {
                model.SectionList = SectionManager.GetAllSectionBySearchKey(model.SearchKey);
                const string fileName = "Section";
                var boundFields = new List<BoundField>
            {                        
              new BoundField(){HeaderText = @"Section Name",DataField = "Name"},
              new BoundField(){HeaderText = @"Description",DataField = "Description"},
   
            };
                ReportConverter.CustomGridView(boundFields, model.SectionList, fileName);
            }
            catch (Exception exception)
            {

                Errorlog.WriteLog(exception);
            }

        }

        [AjaxAuthorize(Roles = "section-1,section-2,section-3")]
        public ActionResult Print(SectionViewModel model)
        {
            try
            {
                model.SectionList = SectionManager.GetAllSectionBySearchKey(model.SearchKey);
            }
            catch (Exception exception)
            {

                Errorlog.WriteLog(exception);
            }
            return PartialView("_SectiontReport", model);
        }

    }
}