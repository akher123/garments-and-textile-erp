using System;
using System.Transactions;
using System.Web.Mvc;
using SCERP.BLL.Manager.HRMManager;
using SCERP.Common;
using SCERP.Model;
using SCERP.Web.Controllers;
namespace SCERP.Web.Areas.HRM.Controllers
{
    public class EmployeeCompanyInfoController : BaseHrmController
    {


        private Guid _employeeGuidId = Guid.Parse("00000000-0000-0000-0000-000000000000");

        [AjaxAuthorize(Roles = "employeecompanyinfo-1,employeecompanyinfo-2,employeecompanyinfo-3")]
        public ActionResult Index(Model.Custom.EmployeeCompanyInfoCustomModel model)
        {
            try
            {
                if (Session["EmployeeGuid"] != null)
                    _employeeGuidId = (Guid)Session["EmployeeGuid"];
                else
                {
                    return ErrorMessageResult();
                }

                model.EmployeeCompanyInfo.EmployeeId = _employeeGuidId;
                var employeeCompanyInfos = EmployeeCompanyInfoManager.GetEmployeeCompanyInfosByEmployeeGuidId(model.EmployeeCompanyInfo);
                model.EmployeeCompanyInfos = employeeCompanyInfos;
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return View(model);
        }

        [AjaxAuthorize(Roles = "employeecompanyinfo-2,employeecompanyinfo-3")]
        public ActionResult Add(Model.Custom.EmployeeCompanyInfoCustomModel model)
        {
            try
            {
                ModelState.Clear();

                if (Session["EmployeeGuid"] != null)
                    _employeeGuidId = (Guid)Session["EmployeeGuid"];

                model.Companies = CompanyManager.GetAllPermittedCompanies();
                model.EmployeeTypes = EmployeeTypeManager.GetAllPermittedEmployeeTypes();

                var employeeCompInfo = EmployeeCompanyInfoManager.GetEmployeeLatestCompanyInfoByEmployeeGuidId(new EmployeeCompanyInfo { EmployeeId = _employeeGuidId, FromDate = DateTime.Now });

                var employeeCompanyInfo = EmployeeCompanyInfoManager.GetEmployeeCompanyInfoById(_employeeGuidId, employeeCompInfo.EmployeeCompanyInfoId);


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

                model.SkillSets = SkillSetManager.GetSkillSetBySearchKey(string.Empty);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return View(model);
        }

        [AjaxAuthorize(Roles = "employeecompanyinfo-2,employeecompanyinfo-3")]
        public ActionResult Edit(Model.Custom.EmployeeCompanyInfoCustomModel model)
        {
            try
            {
                ModelState.Clear();

                if (Session["EmployeeGuid"] != null)
                    _employeeGuidId = (Guid)Session["EmployeeGuid"];
                model.Companies = CompanyManager.GetAllPermittedCompanies();
                model.EmployeeTypes = EmployeeTypeManager.GetAllPermittedEmployeeTypes();

                var employeeCompanyInfo = EmployeeCompanyInfoManager.GetEmployeeCompanyInfoById(_employeeGuidId, model.EmployeeCompanyInfoId);

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

                model.SkillSets = SkillSetManager.GetSkillSetBySearchKey(string.Empty);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return View(model);
        }

        [AjaxAuthorize(Roles = "employeecompanyinfo-2,employeecompanyinfo-3")]
        public ActionResult Save(Model.Custom.EmployeeCompanyInfoCustomModel model)
        {
            if (Session["EmployeeGuid"] != null)
                _employeeGuidId = (Guid)Session["EmployeeGuid"];

            model.EmployeeCompanyInfo.EmployeeId = _employeeGuidId;

            var saveIndex = 0;

            try
            {
                var isExist = EmployeeCompanyInfoManager.CheckExistingEmployeeCompanyInfo(model);

                if (isExist)
                {
                    return ErrorResult("Same departmental information for this person already exist!");
                }


                var employeeCompanyInfo = EmployeeCompanyInfoManager.GetEmployeeCompanyInfoById(model.EmployeeCompanyInfo.EmployeeId, model.EmployeeCompanyInfoId) ?? new EmployeeCompanyInfo();

                employeeCompanyInfo.EmployeeId = model.EmployeeCompanyInfo.EmployeeId;
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
                                EmployeeId = model.EmployeeCompanyInfo.EmployeeId,
                                FromDate = model.FromDate
                            };

                            var employeeLatestSalary = EmployeeSalaryManager.GetLatestEmployeeSalaryInfoByEmployeeGuidId(employeeSalary);

                            if (salarySetup.GrossSalary > employeeLatestSalary.GrossSalary)
                            {
                                if (model.FromDate != null)
                                {
                                    employeeLatestSalary.ToDate = model.FromDate.Value.AddDays(-1);

                                    EmployeeSalaryManager.UpdateEmployeeSalaryInfoDate(employeeLatestSalary);


                                    var employeeGuidId = model.EmployeeCompanyInfo.EmployeeId;
                                    employeeSalary.EmployeeId = employeeGuidId;
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
            if (Session["EmployeeGuid"] != null)
                _employeeGuidId = (Guid)Session["EmployeeGuid"];
            var deleteEmployeeCompanyInfo = 0;

            try
            {
                var employeeCompanyInfo = EmployeeCompanyInfoManager.GetEmployeeCompanyInfoById(_employeeGuidId, employeeCompanyInfoId);
                deleteEmployeeCompanyInfo = EmployeeCompanyInfoManager.DeleteEmployeeCompanyInfo(employeeCompanyInfo);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return deleteEmployeeCompanyInfo > 0 ? Reload() : ErrorResult();
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

        public JsonResult GetBranchUnitDepartmentsByBranchUnitId(int branchUnitId)
        {
            var branchUnitDepartments = BranchUnitDepartmentManager.GetAllPermittedBranchUnitDepartmentsByBranchUnitId(branchUnitId);
            return Json(new { Success = true, BranchUnitDepartments = branchUnitDepartments }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAllEmployeeGradesByEmployeeTypeId(int employeeTypeId)
        {
            var employeeGrades = EmployeeGradeManager.GetEmployeeGradeByEmployeeTypeId(employeeTypeId);
            return Json(new { Success = true, EmployeeGrades = employeeGrades }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAllEmployeeDesignationsByEmployeeGradeId(int employeeGradeId)
        {
            var employeeDesignations = EmployeeDesignationManager.GetEmployeeDesignationByEmployeeGrade(employeeGradeId);
            return Json(new { Success = true, EmployeeDesignations = employeeDesignations }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDepartmentSectionAndLineByBranchUnitDepartmentId(int branchUnitDepartmentId)
        {
            var departmentSections = DepartmentSectionManager.GetDepartmentSectionByBranchUnitDepartmentId(branchUnitDepartmentId);
            var departmentLines = DepartmentLineManager.GetDepartmentLineByBranchUnitDepartmentId(branchUnitDepartmentId);
            return Json(new { Success = true, DepartmentSections = departmentSections, DepartmentLines = departmentLines, }, JsonRequestBehavior.AllowGet);
        }

    }

}
