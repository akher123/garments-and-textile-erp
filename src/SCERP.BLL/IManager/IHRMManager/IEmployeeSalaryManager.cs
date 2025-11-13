using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;

namespace SCERP.BLL.IManager.IHRMManager
{
    public interface IEmployeeSalaryManager
    {
        List<EmployeeSalary> GetAllEmployeeSalarys(int page, int records, string sort);
        EmployeeSalary GetEmployeeSalaryById(int? id);
        int SaveEmployeeSalary(EmployeeSalary aEmployeeSalary);
        int DeleteEmployeeSalary(EmployeeSalary employeeSalary);
        List<EmployeeSalary> GetEmployeeSalaryById(Guid employeeId);
        int EditEmployeeSalary(EmployeeSalary employeeSalary);
        EmployeeSalary GetEmployeeSalary(int id);
        IQueryable<Employee> GetEmployee();
        IQueryable<EmployeeSalary> GetEmployeeSalary();
        decimal GetBasicSalaryByEmployeeId(Guid employeeId);
        EmployeeSalary GetEmployeeSalaryInfoById(Guid employeeId, int id);
        EmployeeSalary GetLatestEmployeeSalaryInfoByEmployeeGuidId(EmployeeSalary employeeSalary);
        int UpdateEmployeeSalaryInfoDate(EmployeeSalary employeeSalary);
        int GetEmployeeTypeByEmployeeId(Guid EmployeeId);
    }
}
