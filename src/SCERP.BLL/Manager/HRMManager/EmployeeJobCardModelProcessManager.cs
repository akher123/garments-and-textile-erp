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
    public class EmployeeJobCardModelProcessManager : BaseManager, IEmployeeJobCardModelProcessManager
    {
        protected readonly IEmployeeJobCardModelProcessRepository EmployeeJobCardModelProcessRepository = null;

        public EmployeeJobCardModelProcessManager(SCERPDBContext context)
        {
            EmployeeJobCardModelProcessRepository = new EmployeeJobCardModelProcessRepository(context);
        }

        public List<EmployeeJobCardModel> GetEmployeeJobCardModelProcessedInfo(int startPage, int pageSize, EmployeeJobCardModel model, SearchFieldModel searchFieldModel)
        {
            return EmployeeJobCardModelProcessRepository.GetEmployeeJobCardModelProcessedInfo(startPage, pageSize, model, searchFieldModel);
        }

        public List<EmployeesForJobCardModelProcessModel> GetEmployeesForJobCardModelProcess(SearchFieldModel searchFieldModel, EmployeeJobCardModel model)
        {
            List<EmployeesForJobCardModelProcessModel> employeesForJobCardModelProcess;
            try
            {
                employeesForJobCardModelProcess = EmployeeJobCardModelProcessRepository.GetEmployeesForJobCardModelProcess(searchFieldModel, model);

            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message);
            }

            return employeesForJobCardModelProcess.ToList();
        }

        public int ProcessBulkEmployeeJobCardModel(SearchFieldModel searchFieldModel, EmployeeJobCardModel model)
        {
            var processEmployeeJobCardModel = 0;
            try
            {
                processEmployeeJobCardModel = EmployeeJobCardModelProcessRepository.ProcessBulkEmployeeJobCardModel(searchFieldModel, model);

            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message);
            }

            return processEmployeeJobCardModel;
        }
    }
}
