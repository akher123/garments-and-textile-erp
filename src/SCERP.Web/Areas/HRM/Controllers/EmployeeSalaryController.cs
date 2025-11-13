using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using SCERP.BLL.IManager.IHRMManager;
using SCERP.Common;
using SCERP.Model;
using SCERP.Model.HRMModel;
using SCERP.Web.Areas.HRM.Models.ViewModels;
using SCERP.Web.Areas.Payroll.Controllers;
using SCERP.Web.Controllers;


namespace SCERP.Web.Areas.HRM.Controllers
{
    public class EmployeeSalaryController : BasePayrollController
    {
        private Guid _employeeGuidId = Guid.Parse("00000000-0000-0000-0000-000000000000");


        [AjaxAuthorize(Roles = "employeesalaryinfo-1,employeesalaryinfo-2,employeesalaryinfo-3")]
        public ActionResult Index(EmployeeSalaryViewModel model)
        {
            try
            {
                if (Session["EmployeeGuid"] != null)
                    _employeeGuidId = (Guid)Session["EmployeeGuid"];
                else
                {
                    return ErrorMessageResult();
                }

                var employeesalaryinfo = EmployeeSalaryManager.GetEmployeeSalaryById(_employeeGuidId);
                model.EmployeeSalary = employeesalaryinfo;
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return View(model);
        }


        [AjaxAuthorize(Roles = "employeesalaryinfo-2,employeesalaryinfo-3")]
        public ActionResult Edit(EmployeeSalaryViewModel model)
        {
            try
            {
                ModelState.Clear();

                if (Session["EmployeeGuid"] != null)
                    _employeeGuidId = (Guid)Session["EmployeeGuid"];

                if (model.Id > 0)
                {
                    var employeeSalary = EmployeeSalaryManager.GetEmployeeSalaryById(model.Id);
                    model.GrossSalary = employeeSalary.GrossSalary;
                    model.BasicSalary = employeeSalary.BasicSalary;
                    model.HouseRent = employeeSalary.HouseRent;
                    model.MedicalAllowance = employeeSalary.MedicalAllowance;
                    model.FoodAllowance = employeeSalary.FoodAllowance;
                    model.Conveyance = employeeSalary.Conveyance;
                    model.EntertainmentAllowance = employeeSalary.EntertainmentAllowance;
                    model.FromDate = employeeSalary.FromDate;
                    model.ToDate = employeeSalary.ToDate;
                }
                else
                {
                    var empCompanyInfo = new EmployeeCompanyInfo
                    {
                        EmployeeId = model.EmployeeId,
                        FromDate = model.FromDate
                    };

                    var employeeCompanyInfo = EmployeeCompanyInfoManager.GetEmployeeLatestCompanyInfoByEmployeeGuidId(empCompanyInfo);

                    if (employeeCompanyInfo != null)
                    {
                        var employeeSalaryInfo = SalarySetupManager.GetSalarySetupByEmployeeGrade(employeeCompanyInfo.EmployeeDesignation.EmployeeGrade.Id, empCompanyInfo.FromDate);

                        if (employeeSalaryInfo != null)
                        {
                            model.GrossSalary = employeeSalaryInfo.GrossSalary;
                            model.BasicSalary = employeeSalaryInfo.BasicSalary;
                            model.HouseRent = employeeSalaryInfo.HouseRent;
                            model.MedicalAllowance = employeeSalaryInfo.MedicalAllowance;
                            model.FoodAllowance = employeeSalaryInfo.FoodAllowance;
                            model.Conveyance = employeeSalaryInfo.Conveyance;
                            model.FromDate = employeeSalaryInfo.FromDate;
                            model.ToDate = employeeSalaryInfo.ToDate;
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return View(model);
        }


        [AjaxAuthorize(Roles = "employeesalaryinfo-2,employeesalaryinfo-3")]
        public ActionResult Save(EmployeeSalaryViewModel model)
        {

            if (Session["EmployeeGuid"] != null)
                _employeeGuidId = (Guid)Session["EmployeeGuid"];
            model.EmployeeId = _employeeGuidId;

            try
            {
                var empJobInfo = new EmployeeCompanyInfo { EmployeeId = _employeeGuidId, FromDate = model.FromDate };

                var employeeJobInfo = EmployeeCompanyInfoManager.GetEmployeeLatestJobInfoByEmployeeId(empJobInfo);
                if (employeeJobInfo == null) return ErrorResult("Departmental information doesn't exist!");

                var salarySetupNew = SalarySetupManager.GetSalarySetupByEmployeeGrade(employeeJobInfo.EmployeeDesignation.EmployeeGrade.Id, empJobInfo.FromDate);
                if (salarySetupNew == null) return ErrorResult("Salary setup doesn't exist for this type of employee!");

                var employeeSalaryInfo = EmployeeSalaryManager.GetEmployeeSalaryInfoById(model.EmployeeId, model.Id) ?? new EmployeeSalary();
                employeeSalaryInfo.EmployeeId = model.EmployeeId;
                employeeSalaryInfo.GrossSalary = model.GrossSalary;
                employeeSalaryInfo.BasicSalary = model.BasicSalary;
                employeeSalaryInfo.HouseRent = model.HouseRent;
                employeeSalaryInfo.MedicalAllowance = model.MedicalAllowance;
                employeeSalaryInfo.FoodAllowance = model.FoodAllowance;
                employeeSalaryInfo.Conveyance = model.Conveyance;
                employeeSalaryInfo.EntertainmentAllowance = model.EntertainmentAllowance;
                employeeSalaryInfo.FromDate = model.FromDate;
                employeeSalaryInfo.ToDate = model.ToDate;

                var isMinimum = SalarySetupManager.CheckMinimumSalary(employeeSalaryInfo, salarySetupNew);
          
                if (isMinimum)
                {
                    var isSumMatch = SalarySetupManager.CheckSumOfAllSalary(employeeSalaryInfo);
                    if (isSumMatch)
                    {

                        var latestEmployeeSalaryInfo = EmployeeSalaryManager.GetLatestEmployeeSalaryInfoByEmployeeGuidId(employeeSalaryInfo);

                        if (latestEmployeeSalaryInfo != null)
                        {
                            if (latestEmployeeSalaryInfo.FromDate >= employeeSalaryInfo.FromDate)
                                return ErrorResult("Invalid date!");
                        }

                        if (model.Id > 0)
                        {
                            EmployeeSalaryManager.EditEmployeeSalary(employeeSalaryInfo);
                        }
                        else
                        {
                            using (var transactionScope = new TransactionScope())
                            {
                                if (latestEmployeeSalaryInfo != null && employeeSalaryInfo.FromDate != null)
                                {
                                    latestEmployeeSalaryInfo.ToDate = employeeSalaryInfo.FromDate.Value.AddDays(-1);
                                    EmployeeSalaryManager.UpdateEmployeeSalaryInfoDate(latestEmployeeSalaryInfo);
                                }

                                EmployeeSalaryManager.SaveEmployeeSalary(employeeSalaryInfo);
                                transactionScope.Complete();
                            }
                        }
                    }
                    else
                    {
                        return ErrorResult("Gross salary doesn't match with other's sum!");
                    }
                }
                else
                {
                    return ErrorResult("Minimum salary error!");
                }
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return RedirectToAction("Index");
        }


        [AjaxAuthorize(Roles = "employeesalaryinfo-3")]
        public ActionResult Delete(int id)
        {
            var deleted = 0;

            try
            {
                var employeeSalary = EmployeeSalaryManager.GetEmployeeSalaryById(id) ?? new EmployeeSalary();
                deleted = EmployeeSalaryManager.DeleteEmployeeSalary(employeeSalary);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return (deleted > 0) ? Reload() : ErrorResult("Failed to save data!");
        }

        public ActionResult GetEmployeeSalaryData(decimal grossSalary)
        {
            ModelState.Clear();

            const bool value = true;
            var salaryData = new EmployeeSalary();

            if (Session["EmployeeGuid"] != null)
                _employeeGuidId = (Guid)Session["EmployeeGuid"];

            Employee employee = EmployeeManager.GetEmployeeById(_employeeGuidId);
            EmployeeCompanyInfo company = EmployeeCompanyInfoManager.GetEmployeeLatestCompanyInfoByEmployeeGuidId(new EmployeeCompanyInfo { EmployeeId = _employeeGuidId, FromDate = DateTime.Now });

            // Garments Worker
            if (company.BranchUnitId == 1 && (company.EmployeeTypeId == 4 || company.EmployeeTypeId == 5)) 
            {
                salaryData.BasicSalary = Convert.ToDecimal(Math.Round(Convert.ToDouble((grossSalary - 1850)) / 1.5));
                salaryData.HouseRent = Convert.ToDecimal(Math.Round(Convert.ToDouble(salaryData.BasicSalary) * .5));
                salaryData.FoodAllowance = 900;
                salaryData.Conveyance = 350;
                salaryData.MedicalAllowance = 600;
            }
            // Dyeing and Knitting Worker
            else if ((company.BranchUnitId == 2 || company.BranchUnitId == 3) && (company.EmployeeTypeId == 4 || company.EmployeeTypeId == 5)) 
            {
                salaryData.BasicSalary = Convert.ToDecimal(Math.Round(Convert.ToDouble((grossSalary - 850)) / 1.35));
                salaryData.HouseRent = Convert.ToDecimal(Math.Round(Convert.ToDouble(salaryData.BasicSalary) * .35));
                salaryData.Conveyance = 300;
                salaryData.MedicalAllowance = 550;
            }

            // For All Staff
            else if (company.EmployeeTypeId < 4) 
            {
                salaryData.BasicSalary = Convert.ToDecimal(Math.Round(Convert.ToDouble(grossSalary) * .45));
                salaryData.HouseRent = Convert.ToDecimal(Math.Round(Convert.ToDouble(grossSalary) * .35));
                salaryData.MedicalAllowance = Convert.ToDecimal(Math.Round(Convert.ToDouble(grossSalary) * .1));
                salaryData.EntertainmentAllowance = Convert.ToDecimal(Math.Round(Convert.ToDouble(grossSalary) * .05));
                salaryData.Conveyance = Convert.ToDecimal(Math.Round(Convert.ToDouble(grossSalary) * .05));
            }

            return salaryData == null ? Json(new { Sucess = "false" }) : Json(new { data = salaryData, Success = value });
        }
    }
}
