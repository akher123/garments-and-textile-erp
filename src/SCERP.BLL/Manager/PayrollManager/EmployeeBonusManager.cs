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
    public class EmployeeBonusManager : BaseManager, IEmployeeBonusManager
    {

        private readonly IEmployeeBonusRepository _employeeBonusRepository = null;

        public EmployeeBonusManager(SCERPDBContext context)
        {
            _employeeBonusRepository = new EmployeeBonusRepository(context);
        }

        public List<EmployeeBonusView> GetAllEmployeeBonusesByPaging(int startPage, int pageSize, out int totalRecords, EmployeeBonus employeeBonus, SearchFieldModel model)
        {
            List<EmployeeBonusView> employeeBonuses = null;
            try
            {
                employeeBonuses = _employeeBonusRepository.GetAllEmployeeBonusesByPaging(startPage, pageSize, out totalRecords, employeeBonus, model).ToList();
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                totalRecords = 0;
            }

            return employeeBonuses;
        }

        public List<EmployeeBonus> GetAllEmployeeBonuses()
        {
            List<EmployeeBonus> employeeBonuses = null;

            try
            {
                employeeBonuses = _employeeBonusRepository.Filter(x => x.IsActive).OrderBy(x => x.EffectiveDate).ToList();
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return employeeBonuses;
        }

        public EmployeeBonus GetEmployeeBonusById(int? id)
        {
            EmployeeBonus employeeBonus = null;
            try
            {
                employeeBonus = _employeeBonusRepository.GetEmployeeBonusById(id);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return employeeBonus;
        }

        public int SaveEmployeeBonus(EmployeeBonus employeeBonus)
        {
            var savedEmployeeBonus = 0;
            try
            {
                employeeBonus.CreatedDate = DateTime.Now;
                employeeBonus.CreatedBy = PortalContext.CurrentUser.UserId;
                employeeBonus.IsActive = true;
                savedEmployeeBonus = _employeeBonusRepository.Save(employeeBonus);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return savedEmployeeBonus;
        }

        public int EditEmployeeBonus(EmployeeBonus employeeBonus)
        {
            var editedEmployeeBonus = 0;
            try
            {
                employeeBonus.EditedDate = DateTime.Now;
                employeeBonus.EditedBy = PortalContext.CurrentUser.UserId;
                editedEmployeeBonus = _employeeBonusRepository.Edit(employeeBonus);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return editedEmployeeBonus;
        }

        public int DeleteEmployeeBonus(EmployeeBonus employeeBonus)
        {
            var deletedEmployeeBonus = 0;
            try
            {
                employeeBonus.EditedDate = DateTime.Now;
                employeeBonus.EditedBy = PortalContext.CurrentUser.UserId;
                employeeBonus.IsActive = false;
                deletedEmployeeBonus = _employeeBonusRepository.Edit(employeeBonus);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return deletedEmployeeBonus;
        }

        public List<EmployeeBonus> GetEmployeeBonusBySearchKey(decimal searchByAmount, DateTime searchByFromDate, DateTime searchByToDate)
        {
            var employeeBonuses = new List<EmployeeBonus>();

            try
            {
                employeeBonuses = _employeeBonusRepository.GetEmployeeBonusBySearchKey(searchByAmount, searchByFromDate, searchByToDate);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return employeeBonuses;
        }

        public List<EmployeesForBonusCustomModel> GetEmployeesForBonus(EmployeeBonus model, SearchFieldModel searchFieldModel)
        {
            List<EmployeesForBonusCustomModel> employeesForBonus;
            try
            {
                employeesForBonus = _employeeBonusRepository.GetEmployeesForBonus(model, searchFieldModel);
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return employeesForBonus;
        }

        public int SaveEmployeeBonus(List<EmployeeBonus> employeeBonuses)
        {
            return _employeeBonusRepository.SaveEmployeeBonus(employeeBonuses);
        }

    }
}
