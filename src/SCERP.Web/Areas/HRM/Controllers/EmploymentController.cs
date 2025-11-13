using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SCERP.Common;
using SCERP.Web.Areas.HRM.Models.ViewModels;
using SCERP.Model;
using SCERP.BLL.IManager.IHRMManager;
using SCERP.Web.Controllers;

namespace SCERP.Web.Areas.HRM.Controllers
{
    public class EmploymentController : BaseHrmController
    {
        private Guid _employeeGuidId = Guid.Parse("00000000-0000-0000-0000-000000000000");

        [AjaxAuthorize(Roles = "employeeemploymentinfo-1,employeeemploymentinfo-2,employeeemploymentinfo-3")]
        public ActionResult Index(EmploymentViewModel model)
        {
            try
            {
                if (Session["EmployeeGuid"] != null)
                    _employeeGuidId = (Guid)Session["EmployeeGuid"];
                else
                {
                    return ErrorMessageResult();
                }

                var employments = EmploymentManager.GetEmploymentsByEmployeeId(_employeeGuidId);
                model.Employments = employments;
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return View(model);
        }

        [AjaxAuthorize(Roles = "employeeemploymentinfo-2,employeeemploymentinfo-3")]
        public ActionResult Edit(EmploymentViewModel model)
        {

            try
            {
                ModelState.Clear();

                if (Session["EmployeeGuid"] != null)
                    _employeeGuidId = (Guid)Session["EmployeeGuid"];

                var employment = EmploymentManager.GetEmploymentById(model.Id);

                if (employment != null)
                {
                    model.EmployeeId = _employeeGuidId;
                    model.CompanyName = employment.CompanyName;
                    model.CompanyAddress = employment.CompanyAddress;
                    model.Department = employment.Department;
                    model.Designation = employment.Designation;
                    model.Respinsibilities = employment.Respinsibilities;
                    model.FromDate = employment.FromDate;
                    model.ToDate = employment.ToDate;
                }
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return View(model);
        }

        [AjaxAuthorize(Roles = "employeeemploymentinfo-2,employeeemploymentinfo-3")]
        public ActionResult Save(EmploymentViewModel model)
        {
            if (Session["EmployeeGuid"] != null)
                _employeeGuidId = (Guid)Session["EmployeeGuid"];

            model.EmployeeId = _employeeGuidId;

            var saveIndex = 0;

            try
            {
                var isExist = EmploymentManager.CheckExistingEmploymentInfo(model);

                if (isExist)
                {
                    return ErrorResult("Same employment info for this person already exist!");
                }

                var employmentInfo = EmploymentManager.GetEmploymentById(model.Id) ?? new Employment();

                employmentInfo.EmployeeId = model.EmployeeId;
                employmentInfo.CompanyName = model.CompanyName;
                employmentInfo.CompanyAddress = model.CompanyAddress;
                employmentInfo.Department = model.Department;
                employmentInfo.Designation = model.Designation;
                employmentInfo.Respinsibilities = model.Respinsibilities;
                employmentInfo.FromDate = model.FromDate;
                employmentInfo.ToDate = model.ToDate;

                saveIndex = model.Id > 0 ? EmploymentManager.EditEmployment(employmentInfo) : EmploymentManager.SaveEmployment(employmentInfo);
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

        [AjaxAuthorize(Roles = "employeeemploymentinfo-3")]
        public ActionResult Delete(int id)
        {
            var deleteEmployment = 0;

            try
            {
                var employment = EmploymentManager.GetEmploymentById(id);
                deleteEmployment = EmploymentManager.DeleteEmployment(employment);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return deleteEmployment > 0 ? Reload() : ErrorResult("Failed to delete data!");
        }
    }
}
