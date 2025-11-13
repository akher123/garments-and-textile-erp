using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using SCERP.Common;
using SCERP.Model;
using SCERP.Web.Controllers;

namespace SCERP.Web.Areas.HRM.Controllers
{
    public class EmployeeEducationController : BaseHrmController
    {
        private Guid _employeeGuidId = Guid.Parse("00000000-0000-0000-0000-000000000000");

        [AjaxAuthorize(Roles = "employeeeducationinfo-1,employeeeducationinfo-2,employeeeducationinfo-3")]
        public ActionResult Index(Models.ViewModels.EmployeeEducationInfoViewModel model)
        {
            try
            {
                if (Session["EmployeeGuid"] != null)
                    _employeeGuidId = (Guid)Session["EmployeeGuid"];
                else
                {
                    return ErrorMessageResult();
                }
                var employeeEducations = EmployeeEducationManager.GetEmployeeEducationsByEmployeeId(_employeeGuidId);
                model.EmployeeEducations = employeeEducations;
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return View(model);
        }

        [AjaxAuthorize(Roles = "employeeeducationinfo-2,employeeeducationinfo-3")]
        public ActionResult Edit(Models.ViewModels.EmployeeEducationInfoViewModel model)
        {

            try
            {
                ModelState.Clear();

                if (Session["EmployeeGuid"] != null)
                    _employeeGuidId = (Guid)Session["EmployeeGuid"];
                model.EducationLevels = EducationLevelManager.GetAllEducationLevels();

                var employeeEducationInfo = EmployeeEducationManager.GetEmployeeEducationById(_employeeGuidId, model.Id);

                if (employeeEducationInfo != null)
                {
                    model.EmployeeId = _employeeGuidId;
                    model.EducationLevelId = employeeEducationInfo.EducationLevelId;
                    model.ExamTitle = employeeEducationInfo.ExamTitle;
                    model.ExamTitleInBengali = employeeEducationInfo.ExamTitleInBengali;
                    model.Institute = employeeEducationInfo.Institute;
                    model.InstituteInBengali = employeeEducationInfo.InstituteInBengali;
                    model.Result = employeeEducationInfo.Result;
                    model.ResultInBengali = employeeEducationInfo.ResultInBengali;
                    model.PassingYear = employeeEducationInfo.PassingYear;
                }
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }


            return View(model);
        }

        [AjaxAuthorize(Roles = "employeeeducationinfo-2,employeeeducationinfo-3")]
        public ActionResult Save(Models.ViewModels.EmployeeEducationInfoViewModel model)
        {
            if (Session["EmployeeGuid"] != null)
                _employeeGuidId = (Guid)Session["EmployeeGuid"];

            model.EmployeeId = _employeeGuidId;

            var saveIndex = 0;

            try
            {
                var isExist = EmployeeEducationManager.CheckExistingEmployeeEducationInfo(model);

                if (isExist)
                {
                    return ErrorResult("Same education information for this person already exist!");
                }

                var employeeEducationInfo = EmployeeEducationManager.GetEmployeeEducationById(model.EmployeeId, model.Id) ?? new EmployeeEducation();

                employeeEducationInfo.EmployeeId = model.EmployeeId;
                employeeEducationInfo.EducationLevelId = model.EducationLevelId;
                employeeEducationInfo.ExamTitle = model.ExamTitle;
                employeeEducationInfo.ExamTitleInBengali = model.ExamTitleInBengali;
                employeeEducationInfo.Institute = model.Institute;
                employeeEducationInfo.InstituteInBengali = model.InstituteInBengali;
                employeeEducationInfo.Result = model.Result;
                employeeEducationInfo.ResultInBengali = model.ResultInBengali;
                employeeEducationInfo.PassingYear = model.PassingYear;

                saveIndex = model.Id > 0 ? EmployeeEducationManager.EditEmployeeEducation(employeeEducationInfo) : EmployeeEducationManager.SaveEmployeeEeducation(employeeEducationInfo);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            if (saveIndex > 0)
            {
                return RedirectToAction("Index");
            }
            return ErrorResult("Failed to save data!");

        }

        [AjaxAuthorize(Roles = "employeeeducationinfo-3")]
        public ActionResult Delete(int id)
        {
            var deleteEmployeeEducation = 0;

            try
            {
                var employeeeducation = EmployeeEducationManager.GetEmployeeEducationById(id);
                deleteEmployeeEducation = EmployeeEducationManager.DeleteEmployeeEducation(employeeeducation);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return deleteEmployeeEducation != 0 ? Reload() : ErrorResult();
        }

    }
}
