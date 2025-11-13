using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;
using System.Linq;
using SCERP.Model.Custom;

namespace SCERP.BLL.IManager.IPayrollManager
{
    public interface IEmployeeBonusManager
    {
        List<EmployeeBonusView> GetAllEmployeeBonusesByPaging(int startPage, int pageSize, out int totalRecords, EmployeeBonus employeeBonus, SearchFieldModel model);

        List<EmployeeBonus> GetAllEmployeeBonuses();

        EmployeeBonus GetEmployeeBonusById(int? id);

        int SaveEmployeeBonus(EmployeeBonus employeeBonus);

        int EditEmployeeBonus(EmployeeBonus employeeBonus);

        int DeleteEmployeeBonus(EmployeeBonus employeeBonus);

        List<EmployeeBonus> GetEmployeeBonusBySearchKey(decimal searchByAmount, DateTime searchByFromDate, DateTime searchByToDate);

        List<EmployeesForBonusCustomModel> GetEmployeesForBonus(EmployeeBonus model,
            SearchFieldModel searchFieldModel);

        int SaveEmployeeBonus(List<EmployeeBonus> employeeBonuses);
    }
}
