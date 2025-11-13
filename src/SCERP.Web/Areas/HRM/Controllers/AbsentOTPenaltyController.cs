using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using SCERP.BLL.Manager.HRMManager;
using SCERP.Common;
using SCERP.Common.ReportHelper;
using SCERP.Model;
using SCERP.Model.Custom;
using SCERP.Web.Areas.HRM.Models.ViewModels;
using SCERP.Web.Controllers;
using SCERP.BLL.IManager.IHRMManager;
using SCERP.Model.HRMModel;

namespace SCERP.Web.Areas.HRM.Controllers
{
    public class AbsentOtPenaltyController : BaseHrmController
    {
        private readonly IPenaltyManager _penaltyManager;

        public AbsentOtPenaltyController(IPenaltyManager penaltyManager)
        {
            _penaltyManager = penaltyManager;
        }

        public ActionResult Index(AbsentOtPenaltyViewModel model)
        {
            ModelState.Clear();

            try
            {
                {
                    
                    model.Companies = CompanyManager.GetAllPermittedCompanies();
                    model.Branches = BranchManager.GetAllPermittedBranchesByCompanyId(model.SearchFieldModel.SearchByCompanyId);
                    model.BranchUnits = BranchUnitManager.GetAllPermittedBranchUnitsByBranchId(model.SearchFieldModel.SearchByBranchId);
                    model.BranchUnitDepartments = BranchUnitDepartmentManager.GetAllPermittedBranchUnitDepartmentsByBranchUnitId(model.SearchFieldModel.SearchByBranchUnitId);
                    model.DepartmentSections = DepartmentSectionManager.GetDepartmentSectionByBranchUnitDepartmentId(model.SearchFieldModel.SearchByBranchUnitDepartmentId);
                    model.DepartmentLines = DepartmentLineManager.GetDepartmentLineByBranchUnitDepartmentId(model.SearchFieldModel.SearchByBranchUnitDepartmentId);

                    model.PenaltyEmployees = new List<SPGetAbsentOtPenaltyEmployee>();

                    if (model.IsAssigneeSerached)
                    {
                        model.IsAssigneeSerached = false;
                        model.SearchFieldModel.StartDate = DateTime.Now;
                        model.SearchFieldModel.EndDate = DateTime.Now;
                        return View(model);
                    }
                    model.FromPenaltyDate = model.SearchFieldModel.StartDate;
                    model.PenaltyEmployees = _penaltyManager.GetAbsentOtPenaltyEmployee(model.SearchFieldModel);
                }
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return View(model);
        }

        public ActionResult Save(AbsentOtPenaltyViewModel model)
        {
            var saveIndex = 0;
            try
            {
                var absentPenaltyEmployees = model.AbsentPenaltyEmployee();

                if (absentPenaltyEmployees.Count > 0)
                {
                    saveIndex = _penaltyManager.SavePenaltyEmployee(absentPenaltyEmployees, model.FromPenaltyDate.Value);
                }
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return (saveIndex > 0) ? Reload() : ErrorResult("Failed to save data");
        }

        public ActionResult GetAllBranchesByCompanyId(int companyId)
        {
            var branches = BranchManager.GetAllPermittedBranchesByCompanyId(companyId);
            return Json(new {Success = true, Branches = branches}, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAllBranchUnitByBranchId(int branchId)
        {
            var brancheUnits = BranchUnitManager.GetAllPermittedBranchUnitsByBranchId(branchId);
            return Json(new {Success = true, BrancheUnits = brancheUnits}, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAllUnitDepatmeByBranchUnitId(int branchUnitId)
        {
            var branchUnitDepartments = BranchUnitDepartmentManager.GetAllPermittedBranchUnitDepartmentsByBranchUnitId(branchUnitId);
            return Json(new {Success = true, BranchUnitDepartments = branchUnitDepartments}, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDepartmentSectionAndLineByBranchUnitDepartmentId(int branchUnitDepartmentId)
        {
            var departmentSections = DepartmentSectionManager.GetDepartmentSectionByBranchUnitDepartmentId(branchUnitDepartmentId);
            var departmentLines = DepartmentLineManager.GetDepartmentLineByBranchUnitDepartmentId(branchUnitDepartmentId);
            return Json(new {Success = true, DepartmentSections = departmentSections, DepartmentLines = departmentLines,}, JsonRequestBehavior.AllowGet);
        }
    }
}