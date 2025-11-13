using System;
using System.Linq;
using System.Web.Mvc;
using iTextSharp.text;
using iTextSharp.text.pdf.qrcode;
using SCERP.BLL.Manager.HRMManager;
using SCERP.Common;
using SCERP.Model;
using SCERP.Web.Areas.HRM.Models.ViewModels;
using System.Collections.Generic;

namespace SCERP.Web.Areas.HRM.Controllers
{
    public class EmployeeJobCardProcessController : BaseHrmController
    {
        private readonly int _pageSize = AppConfig.PageSize;

        public ActionResult Index(EmployeeJobCardProcessViewModel model)
        {
            ModelState.Clear();
            try
            {
                model.Companies = CompanyManager.GetAllPermittedCompanies();
                model.Branches = BranchManager.GetAllPermittedBranchesByCompanyId(model.SearchFieldModel.SearchByCompanyId);
                model.BranchUnits = BranchUnitManager.GetAllPermittedBranchUnitsByBranchId(model.SearchFieldModel.SearchByBranchId);
                model.BranchUnitDepartments = BranchUnitDepartmentManager.GetAllPermittedBranchUnitDepartmentsByBranchUnitId(model.SearchFieldModel.SearchByBranchUnitId);
                model.DepartmentSections = DepartmentSectionManager.GetDepartmentSectionByBranchUnitDepartmentId(model.SearchFieldModel.SearchByBranchUnitDepartmentId);
                model.DepartmentLines = DepartmentLineManager.GetDepartmentLineByBranchUnitDepartmentId(model.SearchFieldModel.SearchByBranchUnitDepartmentId);
                model.EmployeeTypes = EmployeeTypeManager.GetAllPermittedEmployeeTypes();

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

                model.EmployeeJobCardRecords = EmployeeJobCardProcessManager.GetEmployeeJobCardProcessedInfo(startPage, _pageSize, model, model.SearchFieldModel);

                model.TotalRecords = model.EmployeeJobCardRecords[0].TotalRows;
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return View(model);
        }


        //[AjaxAuthorize(Roles = "employeeworkgroup-2,employeeworkgroup-3")]
        public ActionResult Process(EmployeeJobCardProcessViewModel model)
        {
            ModelState.Clear();
            try
            {
                model.Companies = CompanyManager.GetAllPermittedCompanies();
                model.Branches = BranchManager.GetAllPermittedBranchesByCompanyId(model.SearchFieldModel.SearchByCompanyId);
                model.BranchUnits = BranchUnitManager.GetAllPermittedBranchUnitsByBranchId(model.SearchFieldModel.SearchByBranchId);
                model.BranchUnitDepartments = BranchUnitDepartmentManager.GetAllPermittedBranchUnitDepartmentsByBranchUnitId(model.SearchFieldModel.SearchByBranchUnitId);
                model.DepartmentSections = DepartmentSectionManager.GetDepartmentSectionByBranchUnitDepartmentId(model.SearchFieldModel.SearchByBranchUnitDepartmentId);
                model.DepartmentLines = DepartmentLineManager.GetDepartmentLineByBranchUnitDepartmentId(model.SearchFieldModel.SearchByBranchUnitDepartmentId);
                model.EmployeeTypes = EmployeeTypeManager.GetAllPermittedEmployeeTypes();

                if (!model.IsSearch)
                {
                    model.IsSearch = true;
                    return View(model);
                }

                model.EmployeesForJobCardProcess = EmployeeJobCardProcessManager.GetEmployeesForJobCardProcess(model.SearchFieldModel, model);
               
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return View(model);
        }


        //[AjaxAuthorize(Roles = "employeeworkgroup-2,employeeworkgroup-3")]
        public ActionResult ProcessBulkEmployeeJobCard(EmployeeJobCardProcessViewModel model)
        {
            try
            {
                var saveIndex = 0;
                model.SearchFieldModel.EmployeeIdList = new List<Guid>();
                model.SearchFieldModel.EmployeeIdList = model.EmployeeIdList;

                if (model.SearchFieldModel.EmployeeIdList.Count > 0)
                {
                    saveIndex = EmployeeJobCardProcessManager.ProcessBulkEmployeeJobCard(model.SearchFieldModel, model);
                }              
                else
                {
                    return ErrorResult("Please select any one for processing");
                }

                return (saveIndex > 0) ? Reload() : ErrorResult("Failed to save data");
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }


            return View(model);
        }


        public ActionResult ModelIndex(EmployeeJobCardModelProcessViewModel model)
        {
            ModelState.Clear();
            try
            {
                model.Companies = CompanyManager.GetAllPermittedCompanies();
                model.Branches = BranchManager.GetAllPermittedBranchesByCompanyId(model.SearchFieldModel.SearchByCompanyId);
                model.BranchUnits = BranchUnitManager.GetAllPermittedBranchUnitsByBranchId(model.SearchFieldModel.SearchByBranchId);
                model.BranchUnitDepartments = BranchUnitDepartmentManager.GetAllPermittedBranchUnitDepartmentsByBranchUnitId(model.SearchFieldModel.SearchByBranchUnitId);
                model.DepartmentSections = DepartmentSectionManager.GetDepartmentSectionByBranchUnitDepartmentId(model.SearchFieldModel.SearchByBranchUnitDepartmentId);
                model.DepartmentLines = DepartmentLineManager.GetDepartmentLineByBranchUnitDepartmentId(model.SearchFieldModel.SearchByBranchUnitDepartmentId);
                model.EmployeeTypes = EmployeeTypeManager.GetAllPermittedEmployeeTypes();

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

                model.EmployeeJobCardModelRecords = EmployeeJobCardModelProcessManager.GetEmployeeJobCardModelProcessedInfo(startPage, _pageSize, model, model.SearchFieldModel);

                model.TotalRecords = model.EmployeeJobCardModelRecords[0].TotalRows;
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return View(model);
        }


        //[AjaxAuthorize(Roles = "employeeworkgroup-2,employeeworkgroup-3")]
        public ActionResult ModelProcess(EmployeeJobCardModelProcessViewModel model)
        {
            ModelState.Clear();
            try
            {
                model.Companies = CompanyManager.GetAllPermittedCompanies();
                model.Branches = BranchManager.GetAllPermittedBranchesByCompanyId(model.SearchFieldModel.SearchByCompanyId);
                model.BranchUnits = BranchUnitManager.GetAllPermittedBranchUnitsByBranchId(model.SearchFieldModel.SearchByBranchId);
                model.BranchUnitDepartments = BranchUnitDepartmentManager.GetAllPermittedBranchUnitDepartmentsByBranchUnitId(model.SearchFieldModel.SearchByBranchUnitId);
                model.DepartmentSections = DepartmentSectionManager.GetDepartmentSectionByBranchUnitDepartmentId(model.SearchFieldModel.SearchByBranchUnitDepartmentId);
                model.DepartmentLines = DepartmentLineManager.GetDepartmentLineByBranchUnitDepartmentId(model.SearchFieldModel.SearchByBranchUnitDepartmentId);
                model.EmployeeTypes = EmployeeTypeManager.GetAllPermittedEmployeeTypes();

                if (!model.IsSearch)
                {
                    model.IsSearch = true;
                    return View(model);
                }

                model.EmployeesForJobCardModelProcess = EmployeeJobCardModelProcessManager.GetEmployeesForJobCardModelProcess(model.SearchFieldModel, model);

            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return View(model);
        }


        //[AjaxAuthorize(Roles = "employeeworkgroup-2,employeeworkgroup-3")]
        public ActionResult ProcessBulkEmployeeJobCardModel(EmployeeJobCardModelProcessViewModel model)
        {
            try
            {
                var saveIndex = 0;
                model.SearchFieldModel.EmployeeIdList = new List<Guid>();
                model.SearchFieldModel.EmployeeIdList = model.EmployeeIdList;

                if (model.SearchFieldModel.EmployeeIdList.Count > 0)
                {
                    saveIndex = EmployeeJobCardModelProcessManager.ProcessBulkEmployeeJobCardModel(model.SearchFieldModel, model);
                }
                else
                {
                    return ErrorResult("Please select any one for processing");
                }

                return (saveIndex > 0) ? Reload() : ErrorResult("Failed to save data");
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }


            return View(model);
        }

        public ActionResult GetAllBranchesByCompanyId(int companyId)
        {
            var branches = BranchManager.GetAllPermittedBranchesByCompanyId(companyId);
            return Json(new { Success = true, Branches = branches }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAllBranchUnitByBranchId(int branchId)
        {
            var brancheUnits = BranchUnitManager.GetAllPermittedBranchUnitsByBranchId(branchId);
            return Json(new { Success = true, BrancheUnits = brancheUnits }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAllBranchUnitDepartmentByBranchUnitId(int branchUnitId)
        {
            var branchUnitDepartments = BranchUnitDepartmentManager.GetAllPermittedBranchUnitDepartmentsByBranchUnitId(branchUnitId);
            return Json(new { Success = true, BranchUnitDepartments = branchUnitDepartments }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDepartmentSectionAndLineByBranchUnitDepartmentId(int branchUnitDepartmentId)
        {
            var departmentSections = DepartmentSectionManager.GetDepartmentSectionByBranchUnitDepartmentId(branchUnitDepartmentId);
            var departmentLines = DepartmentLineManager.GetDepartmentLineByBranchUnitDepartmentId(branchUnitDepartmentId);
            return Json(new { Success = true, DepartmentSections = departmentSections, DepartmentLines = departmentLines, }, JsonRequestBehavior.AllowGet);
        }
      

    }
}