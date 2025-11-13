using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCERP.Web.Controllers;
using SCERP.BLL.IManager.ITaskManagementManager;
using SCERP.Web.Areas.TaskManagement.Models;
using SCERP.Common;
using System.IO;
using SCERP.Model.TaskManagementModel;

namespace SCERP.Web.Areas.TaskManagement.Controllers
{
    public class ReportImageController : BaseController
    {
        
        private readonly ISubjectManager _subjectManager;
        private readonly ITmModuleManager _tmModuleManager;
        private readonly IReportImageManager _reportImageManager;

        public ReportImageController(ISubjectManager subjectManager, ITmModuleManager tmModuleManager, IReportImageManager reportImageManager)
        {
            
            _subjectManager = subjectManager;
            _tmModuleManager = tmModuleManager;
            _reportImageManager = reportImageManager;
        }

        // GET: TaskManagement/ReportImage
        
        [AjaxAuthorize(Roles = "reportdemosettings-1,reportdemosettings-2,reportdemosettings-3")]
        public ActionResult Index(ReportImageViewModel model)
        {
            ModelState.Clear();
            try
            {
                var totalRecords = 0;
                model.ReportImages = _reportImageManager.GetAllReportImageByPaging(model, out totalRecords);
                model.TotalRecords = totalRecords;
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult("" + exception);
            }
            return View(model);
        }

        public ActionResult ShowReportImage(ReportImageViewModel model)
        {
            ModelState.Clear();
            try
            {
                var totalRecords = 0;
                model.ReportImages = _reportImageManager.GetAllShowReportImageByPaging(model, out totalRecords);
                model.TotalRecords = totalRecords;
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult("" + exception);
            }
            return View(model);
        }

        public ActionResult ShowReportImageNew(ReportImageViewModel model)
        {
            ModelState.Clear();
            try
            {
                var totalRecords = 0;
                model.ReportImages = _reportImageManager.GetAllReportImage();
                model.ReportImageList = _reportImageManager.GetAllReportImageByPaging(model, out totalRecords);
                model.Modules = _tmModuleManager.GetAllModule();
                model.Subjects = _subjectManager.GetALLSubject();
                model.TotalRecords = totalRecords;
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult("" + exception);
            }
            return View(model);
        }


        public ActionResult ShowReportImageAjax(int id)
        {
            
            ReportImageViewModel model = new ReportImageViewModel();
            try
            {
                model.ReportImage = _reportImageManager.GetReportByReporImageId(id);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult("" + exception);
            }
            return View(model);
        }
        [AjaxAuthorize(Roles = "reportdemosettings-2,reportdemosettings-3")]
        public ActionResult Save(ReportImageViewModel model)
        {
            var index = 0;
            string oldfiledelete=null;
            try
            {
                bool exist = _reportImageManager.IsReportImagetExist(model.ReportImage);
                if (!exist)
                {
                    if (model.ReportImage.ReportImageId > 0)
                    {
                        var reportimage= _reportImageManager.GetReportByReporImageId(model.ReportImage.ReportImageId);
                        if (model.ReportImage.ReportImageUrl != reportimage.ReportImageUrl)
                        {
                            oldfiledelete = DeleteDocumnetFile(reportimage.ReportImageUrl).ToString();
                        }
                        
                        index = _reportImageManager.EditReportImage(model.ReportImage);
                    }
                    else
                    {
                        
                        index = _reportImageManager.SaveReportImage(model.ReportImage);
                       
                    }
                }
                else
                {
                    return ErrorResult("Same Information Already Exist ! Please Entry Another One.");
                }

            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult("" + exception);
            }
            return index > 0 ? Reload() : ErrorResult("Fail To Save Task");
        }
        [AjaxAuthorize(Roles = "reportdemosettings-2,reportdemosettings-3")]
        public ActionResult Edit(ReportImageViewModel model)
        {
            ModelState.Clear();
            try
            {
                if (model.ReportImageId > 0)
                {
                    TmReportImageInfo reportImageInfo = _reportImageManager.GetReportByReporImageId(model.ReportImageId);
                    model.ReportImage = reportImageInfo;
                    model.Subjects = _subjectManager.GetALLSubject();
                    var subject = _subjectManager.GetSubjectBySubjectId(reportImageInfo.SubjectId);
                    var module = _tmModuleManager.GetModuleByModuleId(subject.ModuleId);
                    model.ModuleId = module.ModuleId;

                }
                else
                {
                    model.ReportImage.ReportNo = _reportImageManager.GetNewReportImageNumber();
                    
                }
                model.Modules = _tmModuleManager.GetAllModule();
                model.Subjects = _subjectManager.GetALLSubject();
                
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult("" + exception);
            }
            return View(model);
        }
        [AjaxAuthorize(Roles = "reportdemosettings-3")]
        public ActionResult Delete(int reportImageId)
        {
            var index = 0;
            try
            {
                var reportimage = _reportImageManager.GetReportByReporImageId(reportImageId);
                index = _reportImageManager.DeleteReportImage(reportImageId);
                if (index > 0)
                {
                    
                    var dletefile = DeleteDocumnetFile(reportimage.ReportImageUrl).ToString();

                }

            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult("" + exception);
            }
            return index > 0 ? Reload() : ErrorResult("Fail to Delete Subject");
        }

        public ActionResult DocumentFileUpload(HttpPostedFileBase file)
        {
            if (!Directory.Exists((Server.MapPath(@"~/Areas/TaskManagement/ReportImage"))))
            {
                Directory.CreateDirectory((Server.MapPath(@"~/Areas/TaskManagement/ReportImage")));
            }

            Guid flder = Guid.NewGuid();

            if (file != null && file.ContentLength > 0)
            {
                var documentFileName = flder + "_" + file.FileName;
                var path = Path.Combine(Server.MapPath(@"~/Areas/TaskManagement/ReportImage"), documentFileName);
                file.SaveAs(path);
                return Json(new {Success = true, PhotographPath = Url.Content(@"~/Areas/TaskManagement/ReportImage/" + documentFileName)}, JsonRequestBehavior.AllowGet);
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
    }
}