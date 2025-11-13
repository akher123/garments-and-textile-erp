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
    public class BranchUnitController : BaseController
    {
        private readonly int _pageSize = AppConfig.PageSize;

        [AjaxAuthorize(Roles = "branchunit-1,branchunit-2,branchunit-3")]
        public ActionResult Index(BranchUnitViewModel model)
        {
            try
            {
                ModelState.Clear();
                var totalRecords = 0;
                var startPage = 0;
                if (model.page.HasValue && model.page.Value > 0)
                {
                    startPage = model.page.Value - 1;
                }

                model.Branches = BranchManager.GetAllBranchesByCompanyId(model.SearchFieldModel.SearchByCompanyId);
                model.Companies = CompanyManager.GetAllCompanies(PortalContext.CurrentUser.CompId);
                model.Units = UnitManager.GetAllUnits();

                //if (model.IsSearch)
                //{
                //    model.IsSearch = false;
                //    return View(model);
                //}

                model.BranchUnits = BranchUnitManager.GetAllBranchUnit(startPage, _pageSize, out totalRecords, model.SearchFieldModel, model);
                model.TotalRecords = totalRecords;
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return View(model);
        }

        [AjaxAuthorize(Roles = "branchunit-2,branchunit-3")]
        public ActionResult Edit(BranchUnitViewModel model)
        {
            ModelState.Clear();
            try
            {
                model.Companies = CompanyManager.GetAllCompanies(PortalContext.CurrentUser.CompId);
                model.Units = UnitManager.GetAllUnits();
                if (model.BranchUnitId > 0)
                {
                    BranchUnit branchUnit = BranchUnitManager.GetBranchUnitById(model.BranchUnitId);
                    Branch aBranch = BranchManager.GetBranchById(branchUnit.BranchId);
                    model.Branches = BranchManager.GetAllBranchesByCompanyId(aBranch.CompanyId);
                    model.CompanyId = aBranch.CompanyId;
                    model.BranchUnitId = branchUnit.BranchUnitId;
                    model.BranchId = branchUnit.BranchId;
                    model.UnitId = branchUnit.UnitId;
                    model.CreatedBy = branchUnit.CreatedBy;
                    model.EditedBy = branchUnit.EditedBy;
                    model.CreatedDate = branchUnit.CreatedDate;
                    model.EditedDate = branchUnit.EditedDate;
                    model.IsActive = branchUnit.IsActive;

                }


            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return View(model);
        }

        [AjaxAuthorize(Roles = "branchunit-2,branchunit-3")]
        public ActionResult Save(BranchUnit model)
        {
            var saveIndex = 0;

            var isExist = BranchUnitManager.IsExistBranchUnit(model);
            try
            {
                switch (isExist)
                {

                    case false:
                        {
                            saveIndex = model.BranchUnitId > 0 ? BranchUnitManager.EditBranchUnit(model) : BranchUnitManager.SaveBranchUnit(model);
                        }
                        break;
                    default:

                        return ErrorResult(string.Format("Branch Unit already exist!"));
                }

            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return (saveIndex > 0) ? Reload() : ErrorResult("Failed to save data");
        }

        [AjaxAuthorize(Roles = "branchunit-3")]
        public ActionResult Delete(BranchUnit model)
        {
            var deleteIndex = 0;
            try
            {
                deleteIndex = BranchUnitManager.DeleteBranchUnit(model.BranchUnitId);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return (deleteIndex > 0) ? Reload() : ErrorResult("Failed to delete data");
        }

        public ActionResult GetAllBranchesByCompanyId(int companyId)
        {
            var branches = BranchManager.GetAllBranchesByCompanyId(companyId);
            return Json(new { Success = true, Branches = branches }, JsonRequestBehavior.AllowGet);
        }

        [AjaxAuthorize(Roles = "branchunit-1,branchunit-2,branchunit-3")]
        public void GetExcel(BranchUnitViewModel model)
        {
            try
            {
                model.SearchFieldModel.SearchByCompanyId = model.CompanyId;
                model.SearchFieldModel.SearchByBranchId = model.BranchId;
                model.SearchFieldModel.SearchByUnitId = model.UnitId;
                model.BranchUnits = BranchUnitManager.GetAllBranchUnitBySearchKey(model.SearchFieldModel);
                const string fileName = "BranchUnit";
                var boundFields = new List<BoundField>
                 {
                   new BoundField(){HeaderText = @"Company",DataField = "Branch.Company.Name"},
                    new BoundField(){HeaderText = @"Branch",DataField = "Branch.Name"},
                    new BoundField(){HeaderText = @"Unit",DataField = "Unit.Name"},
                 };
                ReportConverter.CustomGridView(boundFields, model.BranchUnits, fileName);
            }
            catch (Exception exception)
            {

                Errorlog.WriteLog(exception);
            }

        }

        [AjaxAuthorize(Roles = "branchunit-1,branchunit-2,branchunit-3")]
        public ActionResult Print(BranchUnitViewModel model)
        {
            try
            {
                model.SearchFieldModel.SearchByCompanyId = model.CompanyId;
                model.SearchFieldModel.SearchByBranchId = model.BranchId;
                model.SearchFieldModel.SearchByUnitId = model.UnitId;
                model.BranchUnits = BranchUnitManager.GetAllBranchUnitBySearchKey(model.SearchFieldModel);
            }
            catch (Exception exception)
            {

                Errorlog.WriteLog(exception);
            }
            return PartialView("_BranchUnitReport", model);
        }
    }
}