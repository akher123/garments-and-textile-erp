using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using SCERP.Common;
using SCERP.Common.ReportHelper;
using SCERP.Model;
using SCERP.Model.Custom;
using SCERP.Web.Areas.HRM.Models.ViewModels;
using SCERP.Web.Controllers;

namespace SCERP.Web.Areas.HRM.Controllers
{
    public class BranchUnitWorkShiftController : BaseHrmController
    {
        private readonly int _pageSize = AppConfig.PageSize;

        [AjaxAuthorize(Roles = "branchunitworkShift-1,branchunitworkShift-2,branchunitworkShift-3")]
        public ActionResult Index(BranchUnitWorkShiftViewModel model)
        {
            try
            {
                ModelState.Clear();
                ModelState.Remove("SearchFieldModel.SearchByWorkShiftId");
                var totalRecords = 0;
                var startPage = 0;
                if (model.page.HasValue && model.page.Value > 0)
                {
                    startPage = model.page.Value - 1;
                }
                model.Companies = CompanyManager.GetAllCompanies(PortalContext.CurrentUser.CompId);
                model.WorkShifts = WorkShiftManager.GetAllWorkShifts().ToList();
                //if (model.IsSearch)
                //{
                //    model.IsSearch = false;
                //    return View(model);
                //}
                model.WorkShiftId = model.SearchFieldModel.SearchByWorkShiftId;
                model.Branches = BranchManager.GetAllBranchesByCompanyId(model.SearchFieldModel.SearchByCompanyId);
                model.BranchUnits = BranchUnitManager.GetBranchUnitByBranchId(model.SearchFieldModel.SearchByBranchId);
                model.SearchFieldModel.SearchByWorkShiftId = model.WorkShiftId;
                model.BranchUnitWorkShifts = BranchUnitWorkShiftManager.GetBranchUnitWorkShiftsBuPaging(startPage, _pageSize, out totalRecords, model.SearchFieldModel, model);
                model.TotalRecords = totalRecords;
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return View(model);
        }

        [AjaxAuthorize(Roles = "branchunitworkShift-2,branchunitworkShift-3")]
        public ActionResult Edit(BranchUnitWorkShiftViewModel model)
        {
            ModelState.Clear();
            try
            {
                model.Companies = CompanyManager.GetAllCompanies(PortalContext.CurrentUser.CompId);
                model.WorkShifts = WorkShiftManager.GetAllWorkShifts().ToList();
                if (model.BranchUnitWorkShiftId > 0)
                {
                    BranchUnitWorkShift branchUnitWorkShift = BranchUnitWorkShiftManager.GetBranchUnitWorkShiftById(model.BranchUnitWorkShiftId);
                    model.BranchUnits = BranchUnitManager.GetBranchUnitByBranchId(branchUnitWorkShift.BranchUnit.BranchId) as IEnumerable;
                    model.Branches =
                        BranchManager.GetAllBranchesByCompanyId(branchUnitWorkShift.BranchUnit.Branch.CompanyId);
                    model.SerchCompanyId = branchUnitWorkShift.BranchUnit.Branch.CompanyId;
                    model.SerchBranchId = branchUnitWorkShift.BranchUnit.BranchId;
                    model.BranchUnitId = branchUnitWorkShift.BranchUnitId;
                    model.WorkShiftId = branchUnitWorkShift.WorkShiftId;
                    model.CreatedBy = branchUnitWorkShift.CreatedBy;
                    model.EditedBy = branchUnitWorkShift.EditedBy;
                    model.CreatedDate = branchUnitWorkShift.CreatedDate;
                    model.EditedDate = branchUnitWorkShift.EditedDate;
                    model.FromDate = branchUnitWorkShift.FromDate;
                    model.ToDate = branchUnitWorkShift.ToDate;
                    model.Description = branchUnitWorkShift.Description;
                    model.IsActive = branchUnitWorkShift.IsActive;

                }

            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return View(model);
        }

        [AjaxAuthorize(Roles = "branchunitworkShift-2,branchunitworkShift-3")]
        public ActionResult Save(BranchUnitWorkShift model)
        {
            var saveIndex = 0;

            bool isExist = BranchUnitWorkShiftManager.IsExistBranchUnitWorkShift(model);
            try
            {

                switch (isExist)
                {

                    case false:
                        {
                            saveIndex = model.BranchUnitWorkShiftId > 0 ? BranchUnitWorkShiftManager.EditBranchUnitWorkShift(model) : BranchUnitWorkShiftManager.SaveBranchUnitWorkShift(model);
                        }
                        break;
                    default:

                        return ErrorResult(string.Format("Branch Unit WorkShift already exist!"));
                }

            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return (saveIndex > 0) ? Reload() : ErrorResult("Failed to save data");
        }

        [AjaxAuthorize(Roles = "branchunitworkShift-3")]
        public ActionResult Delete(BranchUnitWorkShift model)
        {
            var deleteIndex = 0;
            try
            {
                deleteIndex = BranchUnitWorkShiftManager.DeleteBranchUnitWorkShift(model.BranchUnitWorkShiftId);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return (deleteIndex > 0) ? Reload() : ErrorResult("Failed to delete data");
        }

        [AjaxAuthorize(Roles = "branchunitworkShift-1,branchunitworkShift-2,branchunitworkShift-3")]
        public void GetExcel(BranchUnitWorkShiftViewModel model, SearchFieldModel searchFieldModel)
        {
            try
            {
                model.BranchUnitWorkShifts = BranchUnitWorkShiftManager.GetBranchUnitWorkShiftBySearchKey(searchFieldModel);
                const string fileName = "BranchUnitWorkshift";
                var boundFields = new List<BoundField>
                 {
                   new BoundField(){HeaderText = @"Company",DataField = "BranchUnit.Branch.Company.Name"},
                    new BoundField(){HeaderText = @"Branch",DataField = "BranchUnit.Branch.Name"},
                    new BoundField(){HeaderText = @"Unit",DataField = "BranchUnit.Unit.Name"},
                      new BoundField(){HeaderText = @"WorkShift",DataField = "WorkShift.Name"},
                       new BoundField(){HeaderText = @"FromDate",DataField = "FromDate"},
                       new BoundField(){HeaderText = @"ToDate",DataField = "ToDate"},
                          new BoundField(){HeaderText = @"Description",DataField = "Description"},
                 };
                ReportConverter.CustomGridView(boundFields, model.BranchUnitWorkShifts, fileName);
            }
            catch (Exception exception)
            {

                Errorlog.WriteLog(exception);
            }

        }

        [AjaxAuthorize(Roles = "branchunitworkShift-1,branchunitworkShift-2,branchunitworkShift-3")]
        public ActionResult Print(BranchUnitWorkShiftViewModel model, SearchFieldModel searchFieldModel)
        {
            try
            {
                model.BranchUnitWorkShifts = BranchUnitWorkShiftManager.GetBranchUnitWorkShiftBySearchKey(searchFieldModel);
            }
            catch (Exception exception)
            {

                Errorlog.WriteLog(exception);
            }
            return PartialView("_BranchUnitWorkShiftReport", model);
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
    }
}