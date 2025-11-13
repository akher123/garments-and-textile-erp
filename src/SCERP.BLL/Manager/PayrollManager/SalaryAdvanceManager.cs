using SCERP.BLL.IManager.IPayrollManager;
using SCERP.Common;
using SCERP.DAL;
using SCERP.DAL.IRepository.IPayrollRepository;
using SCERP.DAL.Repository.PayrollRepository;
using SCERP.Model;
using SCERP.Model.Custom;
using System;
using System.Collections.Generic;
using System.Linq;


namespace SCERP.BLL.Manager.PayrollManager
{
    public class SalaryAdvanceManager : BaseManager, ISalaryAdvanceManager
    {

        private readonly ISalaryAdvanceRepository _salaryAdvanceRepository = null;

        public SalaryAdvanceManager(SCERPDBContext context)
        {
            _salaryAdvanceRepository = new SalaryAdvanceRepository(context);
        }

        public List<SalaryAdvanceView> GetAllSalaryAdvancesByPaging(int startPage, int pageSize, out int totalRecords, SalaryAdvance salaryAdvance, SearchFieldModel model)
        {
            List<SalaryAdvanceView> salaryAdvances = null;
            try
            {
                salaryAdvances = _salaryAdvanceRepository.GetAllSalaryAdvancesByPaging(startPage, pageSize, out totalRecords, salaryAdvance, model).ToList();
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                totalRecords = 0;
            }

            return salaryAdvances;
        }

        public List<SalaryAdvance> GetAllSalaryAdvances()
        {
            List<SalaryAdvance> salaryAdvance = null;

            try
            {
                salaryAdvance = _salaryAdvanceRepository.Filter(x => x.IsActive).OrderBy(x => x.ReceivedDate).ToList();
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                salaryAdvance = null;
            }

            return salaryAdvance;
        }

        public SalaryAdvance GetSalaryAdvanceById(int? id)
        {
            SalaryAdvance salaryAdvance = null;
            try
            {
                salaryAdvance = _salaryAdvanceRepository.GetSalaryAdvanceById(id);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                salaryAdvance = null;
            }

            return salaryAdvance;
        }
     
        public int SaveSalaryAdvance(SalaryAdvance salaryAdvance)
        {
            var savedSalaryAdvance = 0;
            try
            {
                salaryAdvance.CreatedDate = DateTime.Now;
                salaryAdvance.CreatedBy = PortalContext.CurrentUser.UserId;
                salaryAdvance.IsActive = true;
                savedSalaryAdvance = _salaryAdvanceRepository.Save(salaryAdvance);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return savedSalaryAdvance;
        }

        public int EditSalaryAdvance(SalaryAdvance salaryAdvance)
        {
            var editedSalaryAdvance = 0;
            try
            {
                salaryAdvance.EditedDate = DateTime.Now;
                salaryAdvance.EditedBy = PortalContext.CurrentUser.UserId;
                editedSalaryAdvance = _salaryAdvanceRepository.Edit(salaryAdvance);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return editedSalaryAdvance;
        }

        public int DeleteSalaryAdvance(SalaryAdvance salaryAdvance)
        {
            var deletedSalaryAdvance = 0;
            try
            {
                salaryAdvance.EditedDate = DateTime.Now;
                salaryAdvance.EditedBy = PortalContext.CurrentUser.UserId;
                salaryAdvance.IsActive = false;
                deletedSalaryAdvance = _salaryAdvanceRepository.Edit(salaryAdvance);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return deletedSalaryAdvance;
        }

        public List<SalaryAdvance> GetSalaryAdvanceBySearchKey(decimal searchByAmount, DateTime searchByFromDate, DateTime searchByToDate)
        {
            var salaryAdvances = new List<SalaryAdvance>();

            try
            {
                salaryAdvances = _salaryAdvanceRepository.GetSalaryAdvanceBySearchKey(searchByAmount, searchByFromDate, searchByToDate);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return salaryAdvances;
        }

        public List<EmployeesForAdvanceSalaryCustomModel> GetEmployeesForAdvanceSalary(SalaryAdvance model, SearchFieldModel searchFieldModel)
        {
            List<EmployeesForAdvanceSalaryCustomModel> employeesForAdvanceSalary;
            try
            {
                employeesForAdvanceSalary = _salaryAdvanceRepository.GetEmployeesForAdvanceSalary(model, searchFieldModel);
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return employeesForAdvanceSalary;
        }

        public int SaveEmployeeSalaryAdvance(List<SalaryAdvance> salaryAdvances)
        {
            return _salaryAdvanceRepository.SaveEmployeeSalaryAdvance(salaryAdvances);
        }
    }
}
