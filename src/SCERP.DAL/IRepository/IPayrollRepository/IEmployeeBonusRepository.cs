using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;
using SCERP.Model.Custom;

namespace SCERP.DAL.IRepository.IPayrollRepository
{
    public interface IEmployeeBonusRepository : IRepository<EmployeeBonus>
    {
        EmployeeBonus GetEmployeeBonusById(int? id);
        List<EmployeeBonus> GetAllEmployeeBonuses();
        List<EmployeeBonusView> GetAllEmployeeBonusesByPaging(int startPage, int pageSize, out int totalRecords, EmployeeBonus employeeBonus, SearchFieldModel model);
        List<EmployeeBonus> GetEmployeeBonusBySearchKey(decimal searchByAmount, DateTime searchByFromDate, DateTime searchByToDate);
        List<EmployeesForBonusCustomModel> GetEmployeesForBonus(EmployeeBonus model,SearchFieldModel searchFieldModel);
        int SaveEmployeeBonus(List<EmployeeBonus> employeeBonuses);
    }
}
