using System;
using SCERP.BLL.IManager.IHRMManager;
using SCERP.Common;
using SCERP.DAL;
using SCERP.DAL.IRepository.IHRMRepository;
using SCERP.DAL.Repository.HRMRepository;
using System.Linq;
using SCERP.Model;
using System.Collections.Generic;


namespace SCERP.BLL.Manager.HRMManager
{
    public class EmployeeSalaryManager : BaseManager, IEmployeeSalaryManager
    {

        private readonly IEmployeeSalaryRepository _employeeSalaryRepository = null;

        public List<EmployeeSalary> GetAllEmployeeSalarys(int page, int records, string sort)
        {

            return _employeeSalaryRepository.GetAllEmployeeSalarys(page, records, sort);
        }

        public EmployeeSalary GetEmployeeSalaryById(int? id)
        {
            return _employeeSalaryRepository.GetEmployeeSalaryById(id);
        }

        public int SaveEmployeeSalary(EmployeeSalary aEmployeeSalary)
        {
            var savedEmployeeSalary = 0;
            try
            {
                aEmployeeSalary.CreatedBy = PortalContext.CurrentUser.UserId;
                aEmployeeSalary.CreatedDate = DateTime.Now;
                aEmployeeSalary.IsActive = true;
                savedEmployeeSalary = _employeeSalaryRepository.Save(aEmployeeSalary);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
            return savedEmployeeSalary;
        }

        public int DeleteEmployeeSalary(EmployeeSalary employeeSalary)
        {
            int deleted = 0;

            try
            {
                employeeSalary.EditedBy = PortalContext.CurrentUser.UserId;
                employeeSalary.EditedDate = DateTime.Now;
                employeeSalary.IsActive = false;
                deleted = _employeeSalaryRepository.Edit(employeeSalary);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
            return deleted;
        }

        public EmployeeSalaryManager(SCERPDBContext context)
        {

            _employeeSalaryRepository = new EmployeeSalaryRepository(context);
        }

        public List<EmployeeSalary> GetEmployeeSalaryById(Guid employeeId)
        {
            List<EmployeeSalary> employeeSalaries = null;

            try
            {
                employeeSalaries = _employeeSalaryRepository.GetEmployeeSalaryById(employeeId);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
            return employeeSalaries;
        }

        public int EditEmployeeSalary(EmployeeSalary employeeSalary)
        {
            var edited = 0;

            try
            {
                var employeeSalaryObj = _employeeSalaryRepository.GetById(employeeSalary.Id);

                employeeSalaryObj.GrossSalary = employeeSalary.GrossSalary;
                employeeSalaryObj.BasicSalary = employeeSalary.BasicSalary;
                employeeSalaryObj.HouseRent = employeeSalary.HouseRent;
                employeeSalaryObj.MedicalAllowance = employeeSalary.MedicalAllowance;
                employeeSalaryObj.FoodAllowance = employeeSalary.FoodAllowance;
                employeeSalaryObj.Conveyance = employeeSalary.Conveyance;

                employeeSalaryObj.EditedDate = DateTime.Now;
                employeeSalaryObj.EditedBy = PortalContext.CurrentUser.UserId;
                employeeSalaryObj.IsActive = true;

                edited = _employeeSalaryRepository.Edit(employeeSalaryObj);
            }
            catch (Exception exception)
            {
                edited = 0;
            }

            return edited;

        }

        public EmployeeSalary GetEmployeeSalary(int id)
        {
            return _employeeSalaryRepository.GetEmployeeSalary(id);
        }


        public IQueryable<Employee> GetEmployee()
        {
            return _employeeSalaryRepository.GetEmployee();
        }

        public IQueryable<EmployeeSalary> GetEmployeeSalary()
        {
            return _employeeSalaryRepository.GetEmployeeSalary();
        }

        public decimal GetBasicSalaryByEmployeeId(Guid employeeId)
        {
            var basicSalary = _employeeSalaryRepository.FindOne(x => x.EmployeeId == employeeId).BasicSalary;
            if (basicSalary != null) return (decimal)basicSalary;
            return 0m;
        }

        public EmployeeSalary GetEmployeeSalaryInfoById(Guid employeeId, int id)
        {
            EmployeeSalary employeeSalary;

            try
            {
                employeeSalary = _employeeSalaryRepository.GetEmployeeSalaryInfoById(employeeId, id);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
            return employeeSalary;
        }

        public EmployeeSalary GetLatestEmployeeSalaryInfoByEmployeeGuidId(EmployeeSalary employeeSalary)
        {
            try
            {
                return _employeeSalaryRepository.GetLatestEmployeeSalaryInfoByEmployeeGuidId(employeeSalary);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message, exception.InnerException);
            }
        }

        public int UpdateEmployeeSalaryInfoDate(EmployeeSalary employeeSalary)
        {
            var updated = 0;
            try
            {
                employeeSalary.EditedBy = PortalContext.CurrentUser.UserId;
                employeeSalary.EditedDate = DateTime.Now;
                updated = _employeeSalaryRepository.Edit(employeeSalary);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
            return updated;
        }

        public int GetEmployeeTypeByEmployeeId(Guid EmployeeId)
        {
            return _employeeSalaryRepository.GetEmployeeTypeByEmployeeId(EmployeeId);
        }
    }
}
