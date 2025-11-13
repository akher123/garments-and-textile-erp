using System;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SCERP.BLL.Manager.HRMManager;
using SCERP.Common;
using SCERP.Web.Areas.HRM.Models.ViewModels;
using SCERP.Model;
using SCERP.BLL.IManager.IHRMManager;
using System.Collections.Generic;
using SCERP.Web.Controllers;

namespace SCERP.Web.Areas.HRM.Controllers
{
    public class CompanyOrganogramController : BaseHrmController
    {

        [AjaxAuthorize(Roles = "companyorganogram-1,companyorganogram-2,companyorganogram-3")]
        public ActionResult Index()
        {
            var designationList = CompanyOrganogramManager.GetAllDesignations();
            var topDesignation = CompanyOrganogramManager.GetTopDesignation();

            if (designationList != null && topDesignation != null)
                CompanyOrganogramManager.SetChildren(topDesignation, designationList);
            return View(topDesignation);
        }

        [AjaxAuthorize(Roles = "companyorganogram-2,companyorganogram-3")]
        public ActionResult Add()
        {
            var model = new SCERP.Model.EmployeeDesignationViewModel();
            var restDesignationList = EmployeeDesignationManager.GetRestEmployeeDesignations().ToList();
            ViewBag.RestDesignationId = new SelectList(restDesignationList, "DesignationId", "Title");
            var supervisorDesignationList = EmployeeDesignationManager.GetAllEmployeeDesignation();
            ViewBag.SupervisorDesignationId = new SelectList(supervisorDesignationList, "Id", "Title");
            return View(model);
        }

        [AjaxAuthorize(Roles = "companyorganogram-2,companyorganogram-3")]
        public ActionResult Edit()
        {
            var model = new SCERP.Model.EmployeeDesignationViewModel();
            var employeeDesignationList = CompanyOrganogramManager.GetAllDesignations().ToList();
            ViewBag.EmployeeDesignationId = new SelectList(employeeDesignationList, "DesignationId", "EmployeeDesignation.Title");

            var supervisorDesignationList = EmployeeDesignationManager.GetAllEmployeeDesignation().ToList();
            ViewBag.SupervisorDesignationId = new SelectList(supervisorDesignationList, "Id", "Title");
            return View(model);
        }

        [AjaxAuthorize(Roles = "companyorganogram-2,companyorganogram-3")]
        public ActionResult SaveHierarchy(SCERP.Model.EmployeeDesignationViewModel employeeDesignationViewModel)
        {
            var model = new CompanyOrganogram
            {
                DesignationId = employeeDesignationViewModel.DesignationId,
                ParentDesignationId = employeeDesignationViewModel.SupervisorDesignationId,
                CDT = DateTime.Now,
                CreatedBy = PortalContext.CurrentUser.UserId,
                IsActive = true
            };
            var saved = CompanyOrganogramManager.SaveHierarchy(model);
            return saved > 0 ? Reload() : ErrorMessageResult();
        }

        [AjaxAuthorize(Roles = "companyorganogram-2,companyorganogram-3")]
        public ActionResult EditHierarchy(SCERP.Model.EmployeeDesignationViewModel employeeDesignationViewModel)
        {
            int? edited = 0;
            var model = CompanyOrganogramManager.GetHierarchyByEmployeeDesignation(employeeDesignationViewModel.DesignationId);
            if (model != null)
            {
                model.DesignationId = employeeDesignationViewModel.DesignationId;
                model.ParentDesignationId = employeeDesignationViewModel.SupervisorDesignationId;
                model.EDT = DateTime.Now;
                model.EditedBy = PortalContext.CurrentUser.UserId;
                model.IsActive = true;
                edited = CompanyOrganogramManager.EditHierarchy(model);
            }
            return edited > 0 ? Reload() : ErrorMessageResult();
        }
    }
}