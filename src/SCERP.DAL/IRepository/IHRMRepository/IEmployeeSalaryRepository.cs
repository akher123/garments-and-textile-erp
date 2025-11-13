using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;


namespace SCERP.DAL.IRepository.IHRMRepository
{
    public interface IEmployeeSalaryRepository : IRepository<EmployeeSalary>
    {
        EmployeeSalary GetEmployeeSalaryById(int? id);
        List<EmployeeSalary> GetAllEmployeeSalarys(int page, int records, string sort);
        List<EmployeeSalary> GetEmployeeSalaryById(Guid employeeId);
        EmployeeSalary GetEmployeeSalary(int id);
        IQueryable<Employee> GetEmployee();
        IQueryable<EmployeeSalary> GetEmployeeSalary();
        EmployeeSalary GetEmployeeSalaryInfoById(Guid employeeId, int id);
        EmployeeSalary GetLatestEmployeeSalaryInfoByEmployeeGuidId(EmployeeSalary employeeSalary);
        int UpdateEmployeeSalaryInfoDate(EmployeeSalary employeeSalary);
        int GetEmployeeTypeByEmployeeId(Guid EmployeeId);
    }
}
