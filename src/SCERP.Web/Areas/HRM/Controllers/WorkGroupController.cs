using System;
using System.Collections.Generic;
using System.Web.Mvc;
using SCERP.Common;
using SCERP.Common.ReportHelper;
using SCERP.Model;
using SCERP.Model.Custom;
using SCERP.Web.Areas.HRM.Models.ViewModels;
using SCERP.Web.Controllers;
using System.Web.UI.WebControls;


namespace SCERP.Web.Areas.HRM.Controllers
{
    public class WorkGroupController : BaseHrmController
    {
        private readonly int _pageSize = AppConfig.PageSize;

        [AjaxAuthorize(Roles = "workgroup-1,workgroup-2,workgroup-3")]
        public ActionResult Index(WorkGroupViewModel model)
        {
            ModelState.Clear();

            model.Companies = CompanyManager.GetAllCompanies(PortalContext.CurrentUser.CompId);
            model.Branches = BranchManager.GetAllBranchesByCompanyId(model.SearchFieldModel.SearchByCompanyId);
            model.Units = BranchUnitManager.GetBranchUnitByBranchId(model.SearchFieldModel.SearchByBranchId);

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

            var totalRecords = 0;
            model.WorkGroups = WorkGroupManager.GetAllWorkGroupsByPaging(startPage, _pageSize, model.SearchFieldModel, model, out totalRecords) ?? new List<WorkGroup>();
            model.TotalRecords = totalRecords;

            return View(model);
        }

        [AjaxAuthorize(Roles = "workgroup-2,workgroup-3")]
        public ActionResult Edit(WorkGroupViewModel model)
        {
            ModelState.Clear();


            model.Companies = CompanyManager.GetAllCompanies(PortalContext.CurrentUser.CompId);

            try
            {
                if (model.WorkGroupId > 0)
                {
                    var workGroup = WorkGroupManager.GetWorkGroupById(model.WorkGroupId);

                    model.WorkGroupCompanyId = workGroup.BranchUnit.Branch.Company.Id;
                    model.WorkGroupBranchId = workGroup.BranchUnit.Branch.Id;
                    model.WorkGroupBranchUnitId = workGroup.BranchUnitId;

                    model.Name = workGroup.Name;
                    model.NameInBengali = workGroup.NameInBengali;
                    model.Description = workGroup.Description;

                    ViewBag.Title = "Edit";
                }
                else
                {
                    ViewBag.Title = "Add";
                }

                model.Branches = BranchManager.GetAllBranchesByCompanyId(model.WorkGroupCompanyId);
                model.Units = BranchUnitManager.GetBranchUnitByBranchId(model.WorkGroupBranchId);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return View(model);

        }

        [AjaxAuthorize(Roles = "workgroup-2,workgroup-3")]
        public ActionResult Save(WorkGroupViewModel model)
        {
            var saveIndex = 0;
            try
            {
                model.BranchUnitId = model.WorkGroupBranchUnitId;
                var isExist = WorkGroupManager.CheckExistingWorkGroup(model);

                if (isExist)
                {
                    return ErrorResult(model.Name + " " + "Work Group already exist");
                }

                var workGroup = WorkGroupManager.GetWorkGroupById(model.WorkGroupId) ?? new WorkGroup();
                workGroup.BranchUnitId = model.WorkGroupBranchUnitId;
                workGroup.Name = model.Name;
                workGroup.NameInBengali = model.NameInBengali;
                workGroup.Description = model.Description;

                saveIndex = (model.WorkGroupId > 0) ? WorkGroupManager.EditWorkGroup(workGroup) : WorkGroupManager.SaveWorkGroup(workGroup);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }

            return (saveIndex > 0) ? Reload() : ErrorResult("Failed to save data");
        }


        [AjaxAuthorize(Roles = "workgroup-3")]
        public ActionResult Delete(int workGroupId)
        {
            var deleted = 0;
            try
            {
                var workGroup = WorkGroupManager.GetWorkGroupById(workGroupId) ?? new WorkGroup();
                deleted = WorkGroupManager.DeleteWorkGroup(workGroup.WorkGroupId);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }

            return (deleted > 0) ? Reload() : ErrorResult("Failed to delete data");
        }


        public ActionResult GetAllBranchesByCompanyId(int companyId)
        {
            var branches = BranchManager.GetAllBranchesByCompanyId(companyId);
            return Json(new { Success = true, Branches = branches }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAllBranchUnitByBranchId(int branchId)
        {
            var brancheUnits = BranchUnitManager.GetBranchUnitByBranchId(branchId);
            return Json(new { Success = true, BrancheUnits = brancheUnits }, JsonRequestBehavior.AllowGet);
        }

        [AjaxAuthorize(Roles = "workgroup-1,workgroup-2,workgroup-3")]
        public void GetExcel(WorkGroupViewModel model, SearchFieldModel searchFieldModel)
        {
            List<WorkGroup> workGroups = WorkGroupManager.GetAllWorkGroupsBySearchKey(searchFieldModel.SearchByCompanyId, searchFieldModel.SearchByBranchId, searchFieldModel.SearchByUnitId, searchFieldModel.SearchByWorkGroupName);
            model.WorkGroups = workGroups;

            const string fileName = "WorkGroupList";
            var boundFields = new List<BoundField>
            {
               new BoundField(){HeaderText = @"Work Group", DataField = "Name"},
               new BoundField(){HeaderText = @"Company", DataField = "BranchUnit.Branch.Company.Name"},
               new BoundField(){HeaderText = @"Branch", DataField = "BranchUnit.Branch.Name"},
               new BoundField(){HeaderText = @"Unit", DataField = "BranchUnit.Unit.Name"}
            };
            ReportConverter.CustomGridView(boundFields, model.WorkGroups, fileName);
        }

        [AjaxAuthorize(Roles = "workgroup-1,workgroup-2,workgroup-3")]
        public ActionResult Print(WorkGroupViewModel model, SearchFieldModel searchFieldModel)
        {
            List<WorkGroup> workGroups = WorkGroupManager.GetAllWorkGroupsBySearchKey(searchFieldModel.SearchByCompanyId, searchFieldModel.SearchByBranchId, searchFieldModel.SearchByUnitId, searchFieldModel.SearchByWorkGroupName);
            model.WorkGroups = workGroups;

            return View("_WorkGroupPdfReportViewer", model);
        }
    }
}
