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
    public class EmployeeInOutProcessManager : BaseManager, IEmployeeInOutProcessManager
    {
        protected readonly IEmployeeInOutProcessRepository EmployeeInOutProcessRepository = null;

        public EmployeeInOutProcessManager(SCERPDBContext context)
        {
            EmployeeInOutProcessRepository = new EmployeeInOutProcessRepository(context);
        }

        public List<EmployeeInOut> GetEmployeeInOutProcessedInfo(int startPage, int pageSize, EmployeeInOut model, SearchFieldModel searchFieldModel)
        {
            return EmployeeInOutProcessRepository.GetEmployeeInOutProcessedInfo(startPage, pageSize, model, searchFieldModel);
        }

        public List<EmployeesForInOutProcessModel> GetEmployeeForInOutProcess(SearchFieldModel searchFieldModel, EmployeeInOut model)
        {
            List<EmployeesForInOutProcessModel> employeesForInOutProcess;
            try
            {
                employeesForInOutProcess = EmployeeInOutProcessRepository.GetEmployeeForInOutProcess(searchFieldModel, model);

            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message);
            }

            return employeesForInOutProcess.ToList();
        }

        public int ProcessBulkEmployeeInOut(SearchFieldModel searchFieldModel, EmployeeInOut model)
        {
            var processEmployeeInOut = 0;
            try
            {
                processEmployeeInOut = EmployeeInOutProcessRepository.ProcessBulkEmployeeInOut(searchFieldModel, model);

            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message);
            }

            return processEmployeeInOut;
        }
    }
}
