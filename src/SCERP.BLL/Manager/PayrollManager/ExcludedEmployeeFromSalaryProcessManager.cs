using System;
using System.Collections.Generic;
using SCERP.BLL.IManager.IPayrollManager;
using SCERP.Common;
using SCERP.DAL;
using SCERP.DAL.IRepository.IPayrollRepository;
using SCERP.DAL.Repository.PayrollRepository;
using SCERP.Model;
using SCERP.Model.Custom;

namespace SCERP.BLL.Manager.PayrollManager
{
    public class ExcludedEmployeeFromSalaryProcessManager : BaseManager, IExcludedEmployeeFromSalaryProcessManager
    {
        protected readonly IExcludedEmployeeFromSalaryProcessRepository _excludedEmployeeFromSalaryProcessRepository = null;

        public ExcludedEmployeeFromSalaryProcessManager(SCERPDBContext context)
        {
            _excludedEmployeeFromSalaryProcessRepository = new ExcludedEmployeeFromSalaryProcessRepository(context);
        }


        public List<PayrollExcludedEmployeeFromSalaryProcess> GetExcludedEmployeeFromSalaryProcessInfo(int startPage,
            int pageSize, PayrollExcludedEmployeeFromSalaryProcess model, SearchFieldModel searchFieldModel,
            out int totalRecords)
        {
            return _excludedEmployeeFromSalaryProcessRepository.GetExcludedEmployeeFromSalaryProcessInfo(startPage, pageSize, model, searchFieldModel, out totalRecords);
        }


        public PayrollExcludedEmployeeFromSalaryProcess GetExcludedEmployeeFromSalaryProcessById(int excludedEmployeeFromSalaryProcessId)
        {
            return
                _excludedEmployeeFromSalaryProcessRepository.GetExcludedEmployeeFromSalaryProcessById(
                    excludedEmployeeFromSalaryProcessId);
        }

        public int SaveExcludedEmployeeFromSalaryProcess(PayrollExcludedEmployeeFromSalaryProcess payrollExcludedEmployeeFromSalaryProcess)
        {          
            payrollExcludedEmployeeFromSalaryProcess.CreatedDate = DateTime.Now;
            payrollExcludedEmployeeFromSalaryProcess.CreatedBy = PortalContext.CurrentUser.UserId;
            payrollExcludedEmployeeFromSalaryProcess.IsActive = true;
            return _excludedEmployeeFromSalaryProcessRepository.SaveExcludedEmployeeFromSalaryProcess(payrollExcludedEmployeeFromSalaryProcess);
        }

        public int EditExcludedEmployeeFromSalaryProcess(PayrollExcludedEmployeeFromSalaryProcess payrollExcludedEmployeeFromSalaryProcess)
        {
            payrollExcludedEmployeeFromSalaryProcess.EditedDate = DateTime.Now;
            payrollExcludedEmployeeFromSalaryProcess.EditedBy = PortalContext.CurrentUser.UserId;
            return _excludedEmployeeFromSalaryProcessRepository.EditExcludedEmployeeFromSalaryProcess(payrollExcludedEmployeeFromSalaryProcess);
        }

        public List<ExludedEmployeeFromSalaryProcessInfoCustomModel> GetEmployeesForExcludingFromSalaryProcess(
            SearchFieldModel searchFieldModel, ExludedEmployeeFromSalaryProcessInfoCustomModel model)
        {
            return
                _excludedEmployeeFromSalaryProcessRepository.GetEmployeesForExcludingFromSalaryProcess(
                    searchFieldModel, model);
        }

        public int ProcessBulkEmployeesForExcludingFromSalaryProcess(SearchFieldModel searchFieldModel, ExludedEmployeeFromSalaryProcessInfoCustomModel model)
        {
            return _excludedEmployeeFromSalaryProcessRepository.ProcessBulkEmployeesForExcludingFromSalaryProcess(searchFieldModel, model);
        }

    }
}
