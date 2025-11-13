using System;
using System.Linq;
using System.Transactions;
using System.Web.Mvc;
using iTextSharp.text;
using iTextSharp.text.pdf.qrcode;
using SCERP.Common;
using SCERP.Model;
using SCERP.Model.Custom;
using SCERP.Web.Areas.HRM.Models.ViewModels;
using System.Collections.Generic;

namespace SCERP.Web.Areas.HRM.Controllers
{
    public class EmployeeJobTypeController : BaseHrmController
    {
        private readonly int _pageSize = AppConfig.PageSize;

        public ActionResult Index(EmployeeJobTypeInfoViewModel model)
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
                model.JobTypes = SkillSetManager.GetSkillSetBySearchKey(string.Empty);

                //if (model.IsSearch)
                //{
                //    model.IsSearch = false;
                //    return View(model);
                //}

                var startPage = 0;
                if (model.page.HasValue && model.page.Value > 0)
                {
                    startPage = model.page.Value - 1;
                }

                model.EmployeeCompanyInfoModels = EmployeeCompanyInfoManager.GetEmployeesLatestCompanyInfo(startPage, _pageSize, model, model.SearchFieldModel);

                model.TotalRecords = model.EmployeeCompanyInfoModels[0].TotalRows;
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return View(model);
        }

        public ActionResult Edit(EmployeeJobTypeInfoViewModel model)
        {
            try
            {
                ModelState.Clear();

                model.Companies = CompanyManager.GetAllPermittedCompanies();
                model.EmployeeTypes = EmployeeTypeManager.GetAllPermittedEmployeeTypes();

                var employeeCompanyInfo = EmployeeCompanyInfoManager.GetEmployeeCompanyInfoById(null, model.EmployeeCompanyInfoId);

                if (employeeCompanyInfo != null)
                {
                    model.EmployeeCompanyId = employeeCompanyInfo.BranchUnitDepartment.BranchUnit.Branch.Company.Id;
                    model.EmployeeBranchId = employeeCompanyInfo.BranchUnitDepartment.BranchUnit.Branch.Id;
                    model.EmployeeBranchUnitId = employeeCompanyInfo.BranchUnitDepartment.BranchUnitId;
                    model.EmployeeBranchUnitDepartmentId = employeeCompanyInfo.BranchUnitDepartment.BranchUnitDepartmentId;
                    model.EmployeeDepartmentSectionId = employeeCompanyInfo.DepartmentSectionId;
                    model.EmployeeDepartmentLineId = employeeCompanyInfo.DepartmentLineId;
                    model.EmployeeTypeId = employeeCompanyInfo.EmployeeDesignation.EmployeeGrade.EmployeeTypeId;
                    model.EmployeeGradeId = employeeCompanyInfo.EmployeeDesignation.GradeId;
                    model.EmployeeDesignationId = employeeCompanyInfo.DesignationId;
                    model.JobTypeId = employeeCompanyInfo.JobTypeId;
                    model.IsEligibleForOvertime = employeeCompanyInfo.IsEligibleForOvertime;
                    model.PunchCardNo = employeeCompanyInfo.PunchCardNo;
                    model.FromDate = employeeCompanyInfo.FromDate;
                    model.ToDate = employeeCompanyInfo.ToDate;
                }

                model.Branches = BranchManager.GetAllPermittedBranchesByCompanyId(model.EmployeeCompanyId);
                model.BranchUnits = BranchUnitManager.GetAllPermittedBranchUnitsByBranchId(model.EmployeeBranchId);
                model.BranchUnitDepartments = BranchUnitDepartmentManager.GetAllPermittedBranchUnitDepartmentsByBranchUnitId(model.EmployeeBranchUnitId);

                model.DepartmentSections = DepartmentSectionManager.GetDepartmentSectionByBranchUnitDepartmentId(model.EmployeeBranchUnitDepartmentId);
                model.DepartmentLines = DepartmentLineManager.GetDepartmentLineByBranchUnitDepartmentId(model.EmployeeBranchUnitDepartmentId);

                model.EmployeeGrades = EmployeeGradeManager.GetEmployeeGradeByEmployeeTypeId(model.EmployeeTypeId);
                model.EmployeeDesignations = EmployeeDesignationManager.GetEmployeeDesignationByEmployeeGrade(model.EmployeeGradeId);

                model.JobTypes = SkillSetManager.GetSkillSetBySearchKey(string.Empty);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return View(model);
        }

        public ActionResult AssignEmployeeJobTypeIndex(EmployeeJobTypeInfoViewModel model)
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

                model.JobTypes = SkillSetManager.GetSkillSetBySearchKey(string.Empty);

                if (model.IsSearch)
                {
                    model.IsSearch = true;                    
                    return View(model);
                }

                model.EmployeeCompanyInfoModels = EmployeeCompanyInfoManager.GetEmployeesForAssigingJobType(model, model.SearchFieldModel);

            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return View(model);
        }

        public ActionResult Save(EmployeeJobTypeInfoViewModel model)
        {          
            var saveIndex = 0;

            try
            {
                var employeeCompanyInfo = EmployeeCompanyInfoManager.GetEmployeeCompanyInfoById(null,model.EmployeeCompanyInfoId);

                if (employeeCompanyInfo == null)
                {
                    return ErrorResult("Department info doesn't exist!");
                }

                var employeeCompanyInfoCustomModel = new EmployeeCompanyInfoCustomModel
                {
                    EmployeeCompanyInfoId = model.EmployeeCompanyInfoId,
                    EmployeeCompanyInfo = new EmployeeCompanyInfo { EmployeeId = employeeCompanyInfo.EmployeeId },
                    BranchUnitDepartmentId = model.EmployeeBranchUnitDepartmentId,
                    DesignationId = model.DesignationId,
                    JobTypeId = model.EmployeeDesignationId
                };

                var isExist = EmployeeCompanyInfoManager.CheckExistingEmployeeCompanyInfo(employeeCompanyInfoCustomModel);

                if (isExist)
                {
                    return ErrorResult("Same departmental information for this person already exist!");
                }

                employeeCompanyInfo.BranchUnitDepartmentId = model.EmployeeBranchUnitDepartmentId;
                employeeCompanyInfo.DepartmentSectionId = model.EmployeeDepartmentSectionId;
                employeeCompanyInfo.DepartmentLineId = model.EmployeeDepartmentLineId;
                employeeCompanyInfo.DesignationId = model.EmployeeDesignationId;
                employeeCompanyInfo.JobTypeId = model.JobTypeId;
                employeeCompanyInfo.IsEligibleForOvertime = model.IsEligibleForOvertime;
                employeeCompanyInfo.PunchCardNo = model.PunchCardNo;
                employeeCompanyInfo.FromDate = model.FromDate;
                employeeCompanyInfo.ToDate = model.ToDate;

                var latestEmployeeCompanyInfo = EmployeeCompanyInfoManager.GetEmployeeLatestCompanyInfoByEmployeeGuidId(employeeCompanyInfo);

                if (latestEmployeeCompanyInfo.FromDate > employeeCompanyInfo.FromDate)
                    return ErrorResult("Invalid date!");

                using (var transactionScope = new TransactionScope())
                {

                    if (latestEmployeeCompanyInfo.EmployeeCompanyInfoId > 0)
                    {

                        if ((latestEmployeeCompanyInfo.BranchUnitDepartmentId != model.EmployeeBranchUnitDepartmentId) ||
                            (latestEmployeeCompanyInfo.DesignationId != model.EmployeeDesignationId))
                        {
                            var salarySetup =
                                SalarySetupManager.GetSalarySetupByEmployeeGrade(model.EmployeeGradeId, employeeCompanyInfo.FromDate);
                            if (salarySetup == null)
                                return ErrorResult("Salary setup for this employee's type and grade doesn't exist!");

                            var employeeSalary = new EmployeeSalary
                            {
                                EmployeeId = employeeCompanyInfo.EmployeeId,
                                FromDate = model.FromDate
                            };

                            var employeeLatestSalary = EmployeeSalaryManager.GetLatestEmployeeSalaryInfoByEmployeeGuidId(employeeSalary);

                            if (salarySetup.GrossSalary > employeeLatestSalary.GrossSalary)
                            {
                                if (model.FromDate != null)
                                {
                                    employeeLatestSalary.ToDate = model.FromDate.Value.AddDays(-1);

                                    EmployeeSalaryManager.UpdateEmployeeSalaryInfoDate(employeeLatestSalary);

                                    employeeSalary.EmployeeId = employeeCompanyInfo.EmployeeId;
                                    employeeSalary.GrossSalary = salarySetup.GrossSalary;
                                    employeeSalary.BasicSalary = salarySetup.BasicSalary;
                                    employeeSalary.HouseRent = salarySetup.HouseRent;
                                    employeeSalary.MedicalAllowance = salarySetup.MedicalAllowance;
                                    employeeSalary.FoodAllowance = salarySetup.FoodAllowance;
                                    employeeSalary.Conveyance = salarySetup.Conveyance;
                                    employeeSalary.EntertainmentAllowance = salarySetup.EntertainmentAllowance;
                                    employeeSalary.FromDate = model.FromDate;
                                    employeeSalary.ToDate = model.ToDate;
                                    EmployeeSalaryManager.SaveEmployeeSalary(employeeSalary);
                                }
                            }
                        }
                    }


                    if (model.EmployeeCompanyInfoId > 0)
                    {
                        saveIndex = EmployeeCompanyInfoManager.EditEmployeeCompanyInfo(employeeCompanyInfo);
                    }
                    else
                    {
                        if (employeeCompanyInfo.FromDate != null)
                            latestEmployeeCompanyInfo.ToDate = employeeCompanyInfo.FromDate.Value.AddDays(-1);

                        EmployeeCompanyInfoManager.UpdateEmployeeCompanyInfoDate(latestEmployeeCompanyInfo);

                        saveIndex = EmployeeCompanyInfoManager.SaveEmployeeCompanyInfo(employeeCompanyInfo);
                    }

                    transactionScope.Complete();
                }
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

        [AjaxAuthorize(Roles = "employeecompanyinfo-3")]
        public ActionResult Delete(int employeeCompanyInfoId)
        {
            var deleteEmployeeCompanyInfo = 0;

            try
            {
                var employeeCompanyInfo = EmployeeCompanyInfoManager.GetEmployeeCompanyInfoById(null, employeeCompanyInfoId);
                deleteEmployeeCompanyInfo = EmployeeCompanyInfoManager.DeleteEmployeeCompanyInfo(employeeCompanyInfo);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return deleteEmployeeCompanyInfo > 0 ? Reload() : ErrorResult();
        }

        public ActionResult AssignEmployeeJobType(EmployeeJobTypeInfoViewModel model)
        {
            try
            {
                var saveIndex = 0;
                model.SearchFieldModel.EmployeeIdList = new List<Guid>();
                model.SearchFieldModel.EmployeeIdList = model.EmployeeIdList;

                if (model.SearchFieldModel.EmployeeIdList.Count > 0)
                {
                    saveIndex = EmployeeCompanyInfoManager.AssignBulkEmployeeJobType(model.SearchFieldModel, model);
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