using System;
using System.Collections.Generic;
using System.Web.Mvc;
using SCERP.Common;
using SCERP.Model.CRMModel;
using SCERP.Web.Areas.CRM.Models.ViewModels;
using SCERP.Web.Controllers;
using System.Web;
using System.IO;

namespace SCERP.Web.Areas.CRM.Controllers
{
    public class ProjectDocumentInfoController : BaseController
    {

        private readonly int _pageSize = AppConfig.PageSize;

        public ActionResult Index(ProjectDocumentInfoViewModel model)
        {
            ModelState.Clear();

            CRMDocumentationReport documentationReport = model;
            documentationReport.ReportName = model.SearchKey;
            model.Modules = ProjectDocumentInfoManager.GetAllModulesInfo();

            if (model.IsSearch)
            {
                model.IsSearch = false;
                return View(model);
            }
             
            var startPage = 0;

            if (model.page.HasValue && model.page.Value > 0)
            {
                startPage = model.page.Value - 1;
            }

            int totalRecords = 0;
            model.DocumentationReport = ProjectDocumentInfoManager.GetAllDocumentationReportsByPaging(startPage, _pageSize, out totalRecords, documentationReport) ?? new List<CRMDocumentationReport>();
            model.TotalRecords = totalRecords;

            return View(model);
        }

        public ActionResult Edit(ProjectDocumentInfoViewModel model)
        {
            ModelState.Clear();

            model.ResponsibePersons = ProjectDocumentInfoManager.GetAllResponsiblePerson();
            model.LastUpdatdBy = ProjectDocumentInfoManager.GetAllResponsiblePerson();
            model.Modules = ProjectDocumentInfoManager.GetAllModulesInfo();

            try
            {
                if (model.Id > 0)
                {
                    var documentation = ProjectDocumentInfoManager.GetDocumentationReportById(model.Id);
                    model.RefNo = documentation.RefNo;
                    model.ReportName = documentation.ReportName;
                    model.Description = documentation.Description;
                    model.ModuleId = documentation.ModuleId;
                    model.ResponsiblePerson = documentation.ResponsiblePerson;
                    model.LastUpdateDate = documentation.LastUpdateDate;
                    model.LastUpdateBy = documentation.LastUpdateBy;

                    ViewBag.Title = "Edit Report";
                }
                else
                {
                    ViewBag.Title = "Add Report";
                }
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return View(model);
        }

        public ActionResult EditLiterature(ProjectDocumentInfoViewModel model)
        {
            ModelState.Clear();

            try
            {
                if (model.Id > 0)
                {
                    var documentation = ProjectDocumentInfoManager.GetDocumentationReportById(model.Id);
                    model.Literature = documentation.Literature;

                    ViewBag.Title = "Edit Literature";
                }
                else
                {
                    ViewBag.Title = "Add Literature";
                }
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return View(model);
        }

        public ActionResult Save(ProjectDocumentInfoViewModel model)
        {
            var isExist = ProjectDocumentInfoManager.CheckExistingDocumentationReport(model);

            if (isExist)
            {
                return ErrorResult(model.ReportName + " " + "Report ref no. already exist !");
            }

            var document = ProjectDocumentInfoManager.GetDocumentationReportById(model.Id) ?? new CRMDocumentationReport();

            document.RefNo = model.RefNo;
            document.ReportName = model.ReportName;
            document.ModuleId = model.ModuleId;
            if (model.PhotographPath != null)
                document.PhotographPath = model.PhotographPath;
            document.Description = model.Description;
            document.ResponsiblePerson = model.ResponsiblePerson;
            document.LastUpdateDate = model.LastUpdateDate;
            document.LastUpdateBy = model.LastUpdateBy;

            var saveIndex = (model.Id > 0) ? ProjectDocumentInfoManager.EditDocumentationReport(document) : ProjectDocumentInfoManager.SaveDocumentationReport(document);
            return (saveIndex > 0) ? Reload() : ErrorResult("Failed to save data");
        }

        public ActionResult SaveLiterature(ProjectDocumentInfoViewModel model)
        {
            var document = ProjectDocumentInfoManager.GetDocumentationReportById(model.Id) ?? new CRMDocumentationReport();

            document.Literature = model.Literature;

            var saveIndex = ProjectDocumentInfoManager.EditDocumentationReport(document);

            return (saveIndex > 0) ? Reload() : ErrorResult("Failed to save data");
        }

        public ActionResult Delete(int id)
        {
            var deleted = 0;
            var documentation = ProjectDocumentInfoManager.GetDocumentationReportById(id) ?? new CRMDocumentationReport();
            deleted = ProjectDocumentInfoManager.DeleteDocumentationReport(documentation);
            return (deleted > 0) ? Reload() : ErrorResult("Failed to delete data");
        }

        public ActionResult DocumentFileUpload(HttpPostedFileBase file)
        {
            if (!Directory.Exists((Server.MapPath(@"~/Areas/CRM/Photos"))))
            {
                Directory.CreateDirectory((Server.MapPath(@"~/Areas/CRM/Photos")));
            }

            Guid flder = Guid.NewGuid();

            if (file != null && file.ContentLength > 0)
            {
                var documentFileName = flder + "_" + file.FileName;
                var path = Path.Combine(Server.MapPath(@"~/Areas/CRM/Photos"), documentFileName);
                file.SaveAs(path);
                return Json(new {Success = true, PhotographPath = Url.Content(@"~/Areas/CRM/Photos/" + documentFileName)}, JsonRequestBehavior.AllowGet);
            }
            return Json(new {Success = false}, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteDocumnetFile(string filePath)
        {
            var mapPath = Server.MapPath(filePath);
            if (!System.IO.File.Exists(mapPath))
            {
                return Json(new {Success = false, FilePath = filePath});
            }
            System.IO.File.Delete(mapPath);
            return Json(new {Success = true, FilePath = filePath});
        }

        public ActionResult ShowPic(int id)
        {
            ModelState.Clear();

            var model = new ProjectDocumentInfoViewModel();

            try
            {
                if (id > 0)
                {
                    var documentation = ProjectDocumentInfoManager.GetDocumentationReportById(id);

                    if (documentation != null)
                    {
                        model.Id = documentation.Id;
                        model.PhotographPath = documentation.PhotographPath ?? "Not found !";
                    }

                    ViewBag.Title = "Edit Feedback";
                }
                else
                {
                    ViewBag.Title = "Add Feedback";
                }
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return View(model);
        }
    }
}