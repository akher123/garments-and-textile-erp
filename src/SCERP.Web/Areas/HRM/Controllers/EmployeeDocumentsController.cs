using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SCERP.BLL.IManager.IHRMManager;
using SCERP.Model;
using SCERP.Web.Areas.HRM.Models.ViewModels;
using System.IO;
using SCERP.Web.Controllers;
using SCERP.Common;

namespace SCERP.Web.Areas.HRM.Controllers
{
    public class EmployeeDocumentsController : BaseHrmController
    {
        private readonly int _pageSize = AppConfig.PageSize;

        private Guid _employeeGuidId = Guid.Parse("00000000-0000-0000-0000-000000000000");

        [AjaxAuthorize(Roles = "employeedocuments-1,employeedocuments-2,employeedocuments-3")]
        public ActionResult Index(EmployeeDocumentViewModel model)
        {
            if (Session["EmployeeGuid"] != null)
            {
                _employeeGuidId = (Guid)Session["EmployeeGuid"];
            }

            model.EmployeeDocuments = EmployeeDocumentManager.GetAllEmployeeDocumentsByEmployeeGuidId(_employeeGuidId);

            return View(model);
        }

        [AjaxAuthorize(Roles = "employeedocuments-2,employeedocuments-3")]
        public ActionResult Save(EmployeeDocument document)
        {
            var saveEmployeeDocument = 0;
            if (Session["EmployeeGuid"] != null)
            {
                _employeeGuidId = (Guid)Session["EmployeeGuid"];
            }
            document.EmployeeId = _employeeGuidId;

            if (!String.IsNullOrEmpty(document.Path))
            {
                if (document.Id > 0)
                {
                    var employeeDocument = EmployeeDocumentManager.GetEmployeeDocumentById(document.Id);
                    employeeDocument.EditedBy = PortalContext.CurrentUser.UserId;
                    employeeDocument.EditedDate = DateTime.Now;
                    employeeDocument.Title = document.Title;
                    employeeDocument.Description = document.Description;
                    if (employeeDocument.Path != document.Path)
                    {
                        var mapPath = Server.MapPath(employeeDocument.Path);
                        if (System.IO.File.Exists(mapPath))
                        {
                            System.IO.File.Delete(mapPath);
                        }

                    }
                    employeeDocument.Path = document.Path;
                    saveEmployeeDocument = EmployeeDocumentManager.EditEmployeeDocument(employeeDocument);
                }
                else
                {
                    saveEmployeeDocument = EmployeeDocumentManager.SaveEmployeeDocument(document);
                }
                if (saveEmployeeDocument > 0)
                {
                    return RedirectToAction("Index");
                }
                return ErrorResult("Failed to save data!");

            }
            else
            {
                return ErrorResult("Employee Document not uploaded");
            }

        }

        [AjaxAuthorize(Roles = "employeedocuments-2,employeedocuments-3")]
        public FileResult Download(int id)
        {

            var path = EmployeeDocumentManager.GetEmployeeDocumentById(id).Path;
            if (!System.IO.File.Exists(Server.MapPath(path))) return null;
            var fileName = path.Split('/').ElementAt(4);
            return File(Server.MapPath(path), System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

        [AjaxAuthorize(Roles = "employeedocuments-2,employeedocuments-3")]
        public ActionResult Edit(EmployeeDocument model)
        {
            ModelState.Clear();

            if (model.Id != 0)
            {
                var employeeDocument = EmployeeDocumentManager.GetEmployeeDocumentById(model.Id);

                model.Title = employeeDocument.Title;
                model.Description = employeeDocument.Description;
                model.Path = employeeDocument.Path;
            }
            return View(model);
        }

        [AjaxAuthorize(Roles = "employeedocuments-3")]
        public ActionResult Delete(int id)
        {
            var employeeDocument = EmployeeDocumentManager.GetEmployeeDocumentById(id) ?? new EmployeeDocument();
            EmployeeDocumentManager.DeleteEmployeeDocument(employeeDocument);
            return Reload();
        }

        [AjaxAuthorize(Roles = "employeedocuments-2,employeedocuments-3")]
        public ActionResult DocumentFileUpload(HttpPostedFileBase file)
        {
            if (!Directory.Exists((Server.MapPath(@"~/Areas/HRM/EmployeeDocumentFile"))))
            {
                Directory.CreateDirectory((Server.MapPath(@"~/Areas/HRM/EmployeeDocumentFile")));
            }
            if (Session["EmployeeGuid"] != null)
            {
                _employeeGuidId = (Guid)Session["EmployeeGuid"];
            }
            if (file != null && file.ContentLength > 0)
            {
                var documentFileName = _employeeGuidId + "_" + file.FileName;
                var path = Path.Combine(Server.MapPath(@"~/Areas/HRM/EmployeeDocumentFile"), documentFileName);
                file.SaveAs(path);
                return Json(new { Success = true, DocumentFilePath = Url.Content(@"~/Areas/HRM/EmployeeDocumentFile/" + documentFileName) }, JsonRequestBehavior.AllowGet);

            }
            return Json(new { Success = false }, JsonRequestBehavior.AllowGet);
        }

        [AjaxAuthorize(Roles = "employeedocuments-3")]
        public ActionResult DeleteDocumnetFile(string filePath)
        {

            var mapPath = Server.MapPath(filePath);
            if (!System.IO.File.Exists(mapPath))
            {
                return Json(new { Success = false, FilePath = filePath });
            }
            System.IO.File.Delete(mapPath);
            return Json(new { Success = true, FilePath = filePath });
        }
    }
}
