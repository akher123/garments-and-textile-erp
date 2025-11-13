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
    public class LinesController : BaseController
    {
        private readonly int _pageSize = AppConfig.PageSize;

        [AjaxAuthorize(Roles = "line-1,line-2,line-3")]
        public ActionResult Index(LineViewModel model)
        {
            ModelState.Clear();

            Line line = model;
            line.Name = model.SearchByLineName;

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
            model.Lines = LineManager.GetAllLinesByPaging(startPage, _pageSize, line, out totalRecords) ?? new List<Line>();
            model.TotalRecords = totalRecords;

            return View(model);
        }

        [AjaxAuthorize(Roles = "line-2,line-3")]
        public ActionResult Edit(LineViewModel model)
        {
            ModelState.Clear();
            var companies = CompanyManager.GetAllCompanies(PortalContext.CurrentUser.CompId);
            try
            {
                if (model.LineId > 0)
                {
                    var line = LineManager.GetLineById(model.LineId);
                    model.Name = line.Name;
                    model.NameInBengali = line.NameInBengali;
                    model.Description = line.Description;

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

        [AjaxAuthorize(Roles = "line-2,line-3")]
        public ActionResult Save(LineViewModel model)
        {
            var isExist = LineManager.CheckExistingLine(model);

            if (isExist)
            {
                return ErrorResult(model.Name + " " + "Line already exist");
            }

            var line = LineManager.GetLineById(model.LineId) ?? new Line();
            line.Name = model.Name;
            line.NameInBengali = model.NameInBengali;
            line.Description = model.Description;

            var saveIndex = (model.LineId > 0) ? LineManager.EditLine(line) : LineManager.SaveLine(line);
            return (saveIndex > 0) ? Reload() : ErrorResult("Failed to save data");
        }

        [AjaxAuthorize(Roles = "line-3")]
        public ActionResult Delete(int LineId)
        {
            var deleted = 0;
            var line = LineManager.GetLineById(LineId) ?? new Line();
            deleted = LineManager.DeleteLine(line.LineId);
            return (deleted > 0) ? Reload() : ErrorResult("Failed to delete data");
        }

        [AjaxAuthorize(Roles = "line-1,line-2,line-3")]
        public void GetExcel(LineViewModel model)
        {
            List<Line> lines = LineManager.GetAllLinesBySearchKey(model.SearchByLineName);
            model.Lines = lines;

            const string fileName = "Line";
            var boundFields = new List<BoundField>
            {
               new BoundField(){HeaderText = @"Line Name",DataField = "Name"},
               new BoundField(){HeaderText = @"Description",DataField = "Description"}
            };
            ReportConverter.CustomGridView(boundFields, model.Lines, fileName);
        }

        [AjaxAuthorize(Roles = "line-1,line-2,line-3")]
        public ActionResult Print(LineViewModel model)
        {
            List<Line> lines = LineManager.GetAllLinesBySearchKey(model.SearchByLineName);
            model.Lines = lines;
            return View("_LinePdfReportViewer", model);
        }

        public ActionResult GetAllBranchesByCompanyId(int companyId)
        {
            var branches = BranchManager.GetAllBranchesByCompanyId(companyId);
            return Json(new { Success = true, Branches = branches }, JsonRequestBehavior.AllowGet);
        }

    }
}