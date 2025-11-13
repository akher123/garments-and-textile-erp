using System;
using System.Collections.Generic;
using System.Linq;
using SCERP.BLL.IManager.IPayrollManager;
using SCERP.DAL;
using SCERP.DAL.IRepository.IPayrollRepository;
using SCERP.DAL.Repository.PayrollRepository;
using SCERP.Model;
using SCERP.Model.Custom;

namespace SCERP.BLL.Manager.PayrollManager
{
    public class EmployeeSalaryProcessManager : BaseManager, IEmployeeSalaryProcessManager
    {
        protected readonly IEmployeeSalaryProcessRepository EmployeeSalaryProcessRepository = null;

        public EmployeeSalaryProcessManager(SCERPDBContext context)
        {
            EmployeeSalaryProcessRepository = new EmployeeSalaryProcessRepository(context);
        }

        public List<EmployeeProcessedSalary> GetEmployeeSalaryProcessedInfo(int startPage, int pageSize, EmployeeProcessedSalary model, SearchFieldModel searchFieldModel)
        {
            return EmployeeSalaryProcessRepository.GetEmployeeSalaryProcessedInfo(startPage, pageSize, model, searchFieldModel);
        }


        public List<EmployeesForSalaryProcessModel> GetEmployeesForSalaryProcess(SearchFieldModel searchFieldModel, EmployeeProcessedSalary model)
        {
            List<EmployeesForSalaryProcessModel> employeesForSalaryProcess;
            try
            {
                employeesForSalaryProcess = EmployeeSalaryProcessRepository.GetEmployeesForSalaryProcess(searchFieldModel, model);
            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message);
            }

            return employeesForSalaryProcess.ToList();
        }


        public int ProcessBulkEmployeeSalary(SearchFieldModel searchFieldModel, EmployeeProcessedSalary model)
        {
            var processEmployeeSalary = 0;
            try
            {
                processEmployeeSalary = EmployeeSalaryProcessRepository.ProcessBulkEmployeeSalary(searchFieldModel, model);
            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message);
            }

            return processEmployeeSalary;
        }
    }
}
