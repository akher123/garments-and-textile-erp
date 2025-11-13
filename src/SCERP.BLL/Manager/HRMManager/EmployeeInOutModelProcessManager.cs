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
    public class EmployeeInOutModelProcessManager : BaseManager, IEmployeeInOutModelProcessManager
    {
        protected readonly IEmployeeInOutModelProcessRepository EmployeeInOutModelProcessRepository = null;

        public EmployeeInOutModelProcessManager(SCERPDBContext context)
        {
            EmployeeInOutModelProcessRepository = new EmployeeInOutModelProcessRepository(context);
        }

        public List<EmployeeInOutModel> GetEmployeeInOutModelProcessedInfo(int startPage, int pageSize, EmployeeInOutModel model, SearchFieldModel searchFieldModel)
        {
            return EmployeeInOutModelProcessRepository.GetEmployeeInOutModelProcessedInfo(startPage, pageSize, model, searchFieldModel);
        }

        public List<EmployeesForInOutModelProcessModel> GetEmployeeForInOutModelProcess(SearchFieldModel searchFieldModel, EmployeeInOutModel model)
        {
            List<EmployeesForInOutModelProcessModel> employeesForInOutModelProcess;
            try
            {
                employeesForInOutModelProcess = EmployeeInOutModelProcessRepository.GetEmployeesForInOutModelProcess(searchFieldModel, model);

            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message);
            }

            return employeesForInOutModelProcess.ToList();
        }

        public int ProcessBulkEmployeeInOutModel(SearchFieldModel searchFieldModel, EmployeeInOutModel model)
        {
            var processEmployeeInOutModel = 0;
            try
            {
                processEmployeeInOutModel = EmployeeInOutModelProcessRepository.ProcessBulkEmployeeInOutModel(searchFieldModel, model);

            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message);
            }

            return processEmployeeInOutModel;
        }
    }
}
