using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using SCERP.BLL.Manager.PlanningManager;
using SCERP.Common;
using SCERP.Common.ReportHelper;
using SCERP.Model;
using SCERP.Model.Custom;
using SCERP.Web.Areas.HRM.Models.ViewModels;
using SCERP.Web.Areas.Planning.Models.ViewModels;
using SCERP.Web.Controllers;
using SCERP.BLL.IManager.IPlanningManager;

namespace SCERP.Web.Areas.Planning.Controllers
{
    public class ProductionLineController : BaseController
    {
        private readonly int _pageSize = AppConfig.PageSize;

        private readonly IProductionLineManager ProductionLineManager;
        public ProductionLineController(IProductionLineManager ProductionLineManager)
        {
            this.ProductionLineManager = ProductionLineManager;
        }

        //[AjaxAuthorize(Roles = "productionline-1,productionline-2,productionline-3")]
        public ActionResult Index(ProductionLineViewModel model)
        {
            ModelState.Clear();
            try
            {
                model.Companies = CompanyManager.GetAllCompanies(PortalContext.CurrentUser.CompId);
                model.Lines = LineManager.GetAllLines();
                if (model.IsSearch)
                {
                    model.IsSearch = false;
                    return View(model);
                }

                model.Branches = BranchManager.GetAllBranchesByCompanyId(model.SearchFieldModel.SearchByCompanyId);
                model.BranchUnits = BranchUnitManager.GetBranchUnitByBranchId(model.SearchFieldModel.SearchByBranchId);
                model.BranchUnitDepartments = BranchUnitDepartmentManager.GetBranchUnitDepartmentsByBranchUnitId(model.SearchFieldModel.SearchByBranchUnitId);
                var startPage = 0;
                if (model.page.HasValue && model.page.Value > 0)
                {
                    startPage = model.page.Value - 1;
                }
                int totalRecords = 0;
                model.ProductionLines = ProductionLineManager.GetProductionLine(startPage, _pageSize, out totalRecords, model, model.SearchFieldModel);
                model.TotalRecords = totalRecords;
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return View(model);
        }

        //[AjaxAuthorize(Roles = "productionline-2,productionline-3")]
        public ActionResult Edit(ProductionLineViewModel model)
        {
            ModelState.Clear();
            try
            {
                model.Companies = CompanyManager.GetAllCompanies(PortalContext.CurrentUser.CompId);
                model.Lines = LineManager.GetAllLines();
                if (model.ProductionLineId > 0)
                {
                    PLAN_ProductionLine productionLine = ProductionLineManager.GetProductionLineById(model.ProductionLineId);
                    model.SearchFieldModel.SearchByCompanyId = productionLine.BranchUnitDepartment.BranchUnit.Branch.CompanyId;
                    model.SearchFieldModel.SearchByBranchId = productionLine.BranchUnitDepartment.BranchUnit.BranchId;
                    model.SearchFieldModel.SearchByBranchUnitId = productionLine.BranchUnitDepartment.BranchUnitId;

                    model.BranchUnitDepartmentId =productionLine.BranchUnitDepartmentId;
                    model.LineId = productionLine.LineId;
                    model.NoOfOperator = productionLine.NoOfOperator;
                    model.LineEfficiency = productionLine.LineEfficiency;
                    model.CapacityAvailable = productionLine.CapacityAvailable;

                    model.CreatedBy = productionLine.CreatedBy;
                    model.EditedBy = productionLine.EditedBy;
                    model.CreatedDate = productionLine.CreatedDate;
                    model.EditedDate = productionLine.EditedDate;
                    model.IsActive = productionLine.IsActive;

                    model.Branches = BranchManager.GetAllBranchesByCompanyId(model.SearchFieldModel.SearchByCompanyId);
                    model.BranchUnits = BranchUnitManager.GetBranchUnitByBranchId(model.SearchFieldModel.SearchByBranchId);
                    model.BranchUnitDepartments = BranchUnitDepartmentManager.GetBranchUnitDepartmentsByBranchUnitId(model.SearchFieldModel.SearchByBranchUnitId);

                }


            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return View(model);
        }

        //[AjaxAuthorize(Roles = "productionline-2,productionline-3")]
        public ActionResult Save(ProductionLineViewModel model)
        {
            var saveIndex = 0;          
            try
            {
                saveIndex = model.ProductionLineId > 0 ? ProductionLineManager.EditProductionLine(model) : ProductionLineManager.SaveProductionLine(model);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return (saveIndex > 0) ? Reload() : ErrorResult("Failed to save data");
        }

        //[AjaxAuthorize(Roles = "productionline-1,productionline-2,productionline-3")]
        public void GetExcel(ProductionLineViewModel model, SearchFieldModel searchField)
        {
            try
            {
                model.SearchFieldModel = searchField;
                model.ProductionLines = ProductionLineManager.GetProductionLineBySearchKey(model.SearchFieldModel);
                const string fileName = "ProductionLine";
                var boundFields = new List<BoundField>
                 {
                   new BoundField(){HeaderText = @"Company",DataField = "BranchUnitDepartment.BranchUnit.Branch.Company.Name"},
                   new BoundField(){HeaderText = @"Branch",DataField = "BranchUnitDepartment.BranchUnit.Branch.Name"},
                   new BoundField(){HeaderText = @"Unit",DataField = "BranchUnitDepartment.UnitDepartment.Unit.Name"},
                   new BoundField(){HeaderText = @"Department",DataField = "BranchUnitDepartment.UnitDepartment.Department.Name"},
                   new BoundField(){HeaderText = @"Line",DataField = "Line.Name"},
                 };
                ReportConverter.CustomGridView(boundFields, model.ProductionLines, fileName);
            }
            catch (Exception exception)
            {

                Errorlog.WriteLog(exception);
            }

        }

        //[AjaxAuthorize(Roles = "productionline-3")]
        public ActionResult Delete(ProductionLineViewModel model)
        {
            var deleteIndex = 0;
            try
            {
                deleteIndex = ProductionLineManager.DeleteProductionLine(model.ProductionLineId);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return (deleteIndex > 0) ? Reload() : ErrorResult("Failed to delete data");
        }

        //[AjaxAuthorize(Roles = "productionline-1,productionline-2,productionline-3")]
        public ActionResult Print(ProductionLineViewModel model, SearchFieldModel searchField)
        {
            try
            {
                model.SearchFieldModel = searchField;
                model.ProductionLines = ProductionLineManager.GetProductionLineBySearchKey(model.SearchFieldModel);
            }
            catch (Exception exception)
            {

                Errorlog.WriteLog(exception);
            }
            return PartialView("_ProductionLineReport", model);
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

        public JsonResult GetAllUnitDepatmeByBranchUnitId(int branchUnitId)
        {
            var branchUnitDepartments = BranchUnitDepartmentManager.GetBranchUnitDepartmentsByBranchUnitId(branchUnitId);
            return Json(new { Success = true, BranchUnitDepartments = branchUnitDepartments }, JsonRequestBehavior.AllowGet);
        }

    }
}