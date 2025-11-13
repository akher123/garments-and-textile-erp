using System;
using System.Collections.Generic;
using System.Linq;
using SCERP.BLL.IManager.IHRMManager;
using SCERP.DAL;
using SCERP.DAL.IRepository.IHRMRepository;
using SCERP.DAL.Repository.HRMRepository;
using SCERP.Model;
using SCERP.Model.Custom;


namespace SCERP.BLL.Manager.HRMManager
{
    public class EmployeeJobCardProcessManager : BaseManager, IEmployeeJobCardProcessManager
    {
        protected readonly IEmployeeJobCardProcessRepository EmployeeJobCardProcessRepository = null;

        public EmployeeJobCardProcessManager(SCERPDBContext context)
        {
            EmployeeJobCardProcessRepository = new EmployeeJobCardProcessRepository(context);
        }

        public List<EmployeeJobCard> GetEmployeeJobCardProcessedInfo(int startPage, int pageSize, EmployeeJobCard model, SearchFieldModel searchFieldModel)
        {
            return EmployeeJobCardProcessRepository.GetEmployeeJobCardProcessedInfo(startPage, pageSize, model, searchFieldModel);
        }

        public List<EmployeesForJobCardProcessModel> GetEmployeesForJobCardProcess(SearchFieldModel searchFieldModel, EmployeeJobCard model)
        {
            List<EmployeesForJobCardProcessModel> employeesForJobCardProcess;
            try
            {
                employeesForJobCardProcess = EmployeeJobCardProcessRepository.GetEmployeesForJobCardProcess(searchFieldModel, model);

            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message);
            }

            return employeesForJobCardProcess.ToList();
        }

        public int ProcessBulkEmployeeJobCard(SearchFieldModel searchFieldModel, EmployeeJobCard model)
        {
            var processEmployeeJobCard = 0;
            try
            {
                processEmployeeJobCard = EmployeeJobCardProcessRepository.ProcessBulkEmployeeJobCard(searchFieldModel, model);

            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message);
            }

            return processEmployeeJobCard;
        }
    }
}
