using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using iTextSharp.text.pdf.qrcode;
using SCERP.Common;
using SCERP.Model;
using SCERP.Model.CRMModel;
using SCERP.Web.Areas.CRM.Models;
using SCERP.Web.Areas.CRM.Models.ViewModels;
using SCERP.Web.Controllers;

namespace SCERP.Web.Areas.CRM.Controllers
{
    public class FeedbackController : BaseController
    {
        private readonly int _pageSize = AppConfig.PageSize;

        public ActionResult Index(FeedbackViewModel model)
        {
            ModelState.Clear();

            CRMFeedback documentationReport = model;
            documentationReport.Subject = model.SearchKey;

            var startPage = 0;

            if (model.page.HasValue && model.page.Value > 0)
            {
                startPage = model.page.Value - 1;
            }

            int totalRecords = 0;

            List<CRMFeedback> feedbacks = FeedbackManager.GetAllFeedbacksByPaging(startPage, _pageSize, out totalRecords, documentationReport) ?? new List<CRMFeedback>();

            foreach (var t in feedbacks)
            {
                if (t.CreatedBy != null) t.CreatedByDisplayName = EmployeeManager.GetEmployeeById(t.CreatedBy.Value).Name;
            }
            model.Feedback = feedbacks;
            model.TotalRecords = totalRecords;

            return View(model);
        }

        public ActionResult Edit(FeedbackViewModel model)
        {
            ModelState.Clear();

            model.CreatedDate = DateTime.Now;

            try
            {
                if (model.Id > 0)
                {
                    var documentation = FeedbackManager.GetFeedbackById(model.Id);

                    model.Subject = documentation.Subject;
                    model.Description = documentation.Description;

                    if (documentation.CreatedBy != null) model.CreatedByDisplayName = EmployeeManager.GetEmployeeById(documentation.CreatedBy.Value).Name;

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

        public ActionResult Save(FeedbackViewModel model)
        {
            var isExist = FeedbackManager.CheckExistingFeedback(model);

            if (isExist)
            {
                return ErrorResult(model.Subject + " name already exist !");
            }

            var feedback = FeedbackManager.GetFeedbackById(model.Id) ?? new CRMFeedback();

            if (feedback.JobStatus == Convert.ToInt32(FeedBackStatus.Open) || feedback.JobStatus == Convert.ToInt32(FeedBackStatus.Closed))
            {
                return ErrorResult("You can not edit this feedback !");
            }

            feedback.JobStatus = model.Id > 0 ? feedback.JobStatus : Convert.ToInt32(FeedBackStatus.Waiting);

            feedback.Subject = model.Subject;
            feedback.Description = model.Description;
            feedback.ModuleId = model.ModuleId;
            feedback.JobStatus = model.JobStatus;
            if (model.PhotographPath != null)
                feedback.PhotographPath = model.PhotographPath;

            if (model.Id == 0)
            {
                feedback.CreatedDate = DateTime.Now;
            }

            var saveIndex = (model.Id > 0) ? FeedbackManager.EditFeedback(feedback) : FeedbackManager.SaveFeedback(feedback);
            return (saveIndex > 0) ? Reload() : ErrorResult("Failed to save data");
        }

        public ActionResult Delete(int id)
        {
            var deleted = 0;
            var feedback = FeedbackManager.GetFeedbackById(id) ?? new CRMFeedback();
            deleted = FeedbackManager.DeleteFeedback(feedback);
            return (deleted > 0) ? Reload() : ErrorResult("Failed to delete data");
        }

        public ActionResult ShowPic(int id)
        {
            ModelState.Clear();

            var model = new FeedbackViewModel();

            try
            {
                if (id > 0)
                {
                    var documentation = FeedbackManager.GetFeedbackById(id);

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



        //**************** Feedback of all user *****************

        public ActionResult IndexAllUser(FeedbackViewModel model)
        {
            ModelState.Clear();

            CRMFeedback documentationReport = model;
            documentationReport.Subject = model.SearchKey;

            var startPage = 0;

            if (model.page.HasValue && model.page.Value > 0)
            {
                startPage = model.page.Value - 1;
            }

            int totalRecords = 0;

            List<CRMFeedback> feedbacks = FeedbackManager.GetAllFeedbacksAllUserByPaging(startPage, _pageSize, out totalRecords, documentationReport) ?? new List<CRMFeedback>();

            foreach (var t in feedbacks)
            {
                if (t.CreatedBy != null) t.CreatedByDisplayName = EmployeeManager.GetEmployeeById(t.CreatedBy.Value).Name;
            }
            model.Feedback = feedbacks;
            model.TotalRecords = totalRecords;

            return View(model);
        }

        public ActionResult EditAllUser(FeedbackViewModel model)
        {
            ModelState.Clear();

            model.CreatedDate = DateTime.Now;
            model.Modules = FeedbackManager.GetAllModuleNames();

            var feedBackStatusList = from FeedBackStatus feedback in Enum.GetValues(typeof (FeedBackStatus)) select new {Id = (int) feedback, Name = feedback.ToString()};

            try
            {
                if (model.Id > 0)
                {
                    var documentation = FeedbackManager.GetFeedbackById(model.Id);

                    model.Subject = documentation.Subject;
                    model.Description = documentation.Description;
                    model.CreatedDate = documentation.CreatedDate;
                    model.JobStatus = documentation.JobStatus;
                    if (documentation.CreatedBy != null) model.CreatedByDisplayName = EmployeeManager.GetEmployeeById(documentation.CreatedBy.Value).Name;
                    model.ModuleId = documentation.ModuleId;

                    ViewBag.Title = "Edit Feedback";
                }
                else
                {
                    ViewBag.Title = "Add Feedback";
                }
                model.FeedbackStatus = feedBackStatusList;
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return View(model);
        }

        public ActionResult SaveAllUser(FeedbackViewModel model)
        {
            var isExist = FeedbackManager.CheckExistingFeedback(model);

            if (isExist)
            {
                return ErrorResult(model.Subject + " name already exist !");
            }

            var feedback = FeedbackManager.GetFeedbackById(model.Id) ?? new CRMFeedback();

            //if (feedback.JobStatus == Convert.ToInt32(FeedBackStatus.Open) || feedback.JobStatus == Convert.ToInt32(FeedBackStatus.Closed))
            //    return ErrorResult("You can not edit this feedback !");

            feedback.JobStatus = model.Id > 0 ? feedback.JobStatus : Convert.ToInt32(FeedBackStatus.Waiting);

            feedback.Subject = model.Subject;
            feedback.Description = model.Description;
            feedback.CreatedDate = model.CreatedDate;
            feedback.ModuleId = model.ModuleId;
            feedback.JobStatus = model.JobStatus;

            var saveIndex = (model.Id > 0) ? FeedbackManager.EditFeedback(feedback) : FeedbackManager.SaveFeedback(feedback);
            return (saveIndex > 0) ? Reload() : ErrorResult("Failed to save data");
        }
    }
}