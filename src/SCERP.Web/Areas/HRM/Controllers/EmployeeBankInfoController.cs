using System;
using System.Linq;
using System.Net;
using System.Transactions;
using System.Web.Mvc;
using SCERP.BLL.IManager.IHRMManager;
using SCERP.Common;
using SCERP.Web.Areas.HRM.Models.ViewModels;
using SCERP.Model;
using SCERP.Web.Controllers;

namespace SCERP.Web.Areas.HRM.Controllers
{
    public class EmployeeBankInfoController : BaseHrmController
    {
        private Guid _employeeGuidId = Guid.Parse("00000000-0000-0000-0000-000000000000");

        [AjaxAuthorize(Roles = "employeebankinfo-1,employeebankinfo-2,employeebankinfo-3")]
        public ActionResult Index(EmployeeBankInfoViewModel model)
        {
            try
            {
                if (Session["EmployeeGuid"] != null)
                    _employeeGuidId = (Guid)Session["EmployeeGuid"];
                else
                {
                    return ErrorResult("Employee doesn't exist!");
                }

                var employeeBankInfos = EmployeeBankInfoManager.GetEmployeeBankInfoByEmployeeId(_employeeGuidId);
                model.EmployeeBankInfos = employeeBankInfos;
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return View(model);
        }

        [AjaxAuthorize(Roles = "employeebankinfo-2,employeebankinfo-3")]
        public ActionResult Edit(EmployeeBankInfoViewModel model)
        {

            try
            {
                ModelState.Clear();

                if (Session["EmployeeGuid"] != null)
                    _employeeGuidId = (Guid)Session["EmployeeGuid"];

                model.BankAccountTypes = EmployeeBankInfoManager.GetAllBankAccountTypes();

                var employeeBankInfo = EmployeeBankInfoManager.GetEmployeeBankInfoById(_employeeGuidId, model.Id);
                var empInfo = EmployeeManager.GetEmployeeById(_employeeGuidId);

                if (employeeBankInfo != null)
                {
                    model.EmployeeId = _employeeGuidId;
                    model.BankName = employeeBankInfo.BankName;
                    model.BranchName = employeeBankInfo.BranchName;
                    model.AccountName = employeeBankInfo.AccountName;
                    model.AccountNumber = employeeBankInfo.AccountNumber;
                    model.FromDate = employeeBankInfo.FromDate;
                    model.ToDate = employeeBankInfo.ToDate;
                }
                else
                {
                    model.BankName = "IFIC Bank";
                    model.BranchName = "Narayanganj";
                    model.AccountTypeId = 2;
                    model.AccountName = empInfo.Name;
                }
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return View(model);
        }

        [AjaxAuthorize(Roles = "employeebankinfo-2,employeebankinfo-3")]
        public ActionResult Save(EmployeeBankInfoViewModel model)
        {
            if (Session["EmployeeGuid"] != null)
                _employeeGuidId = (Guid)Session["EmployeeGuid"];
            model.EmployeeId = _employeeGuidId;
            var saveIndex = 0;

            try
            {
                var employeeBankInfo = EmployeeBankInfoManager.GetEmployeeBankInfoById(_employeeGuidId, model.Id) ?? new EmployeeBankInfo();
                employeeBankInfo.EmployeeId = _employeeGuidId;
                employeeBankInfo.BankName = model.BankName;
                employeeBankInfo.BranchName = model.BranchName;
                employeeBankInfo.AccountTypeId = model.AccountTypeId;
                employeeBankInfo.AccountName = model.AccountName;
                employeeBankInfo.AccountNumber = model.AccountNumber;
                employeeBankInfo.FromDate = model.FromDate;
                employeeBankInfo.ToDate = model.ToDate;
                if (model.Id > 0)
                {
                    saveIndex = EmployeeBankInfoManager.EditEmployeeBankInfo(employeeBankInfo);
                }
                else
                {
                    using (var transactionScope = new TransactionScope())
                    {
                        var latestEmployeeBankInfo = EmployeeBankInfoManager.GetLatestEmployeeBankInfoByEmployeeGuidId(model.EmployeeId);
                        if (latestEmployeeBankInfo != null)
                        {
                            if (latestEmployeeBankInfo.FromDate > employeeBankInfo.FromDate)
                                return ErrorResult("Invalid date!");

                            if (employeeBankInfo.FromDate != null)
                                latestEmployeeBankInfo.ToDate = employeeBankInfo.FromDate.Value.AddDays(-1);

                            EmployeeBankInfoManager.UpdateEmployeeBankInfoDate(latestEmployeeBankInfo);
                        }
                        saveIndex = EmployeeBankInfoManager.SaveEmployeeBankInfo(employeeBankInfo);
                        transactionScope.Complete();
                    }
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

        [AjaxAuthorize(Roles = "employeebankinfo-3")]
        public ActionResult Delete(int id)
        {
            var deleteEmployeeBankInfo = 0;

            try
            {
                var employeeBankInfo = EmployeeBankInfoManager.GetEmployeeBankInfoById(id);
                deleteEmployeeBankInfo = EmployeeBankInfoManager.DeleteEmployeeBankInfo(employeeBankInfo);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return deleteEmployeeBankInfo >= 0 ? Reload() : ErrorResult("Failed to delete data!");
        }
    }
}
