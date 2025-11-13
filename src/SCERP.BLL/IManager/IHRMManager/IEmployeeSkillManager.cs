using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;
using SCERP.Model.Custom;

namespace SCERP.BLL.IManager.IHRMManager
{
    public interface IEmployeeSkillManager
    {
        EmployeeSkill GetEmployeeSkillById(int employeeSkillId); 
        int SaveEmployeeSkill(EmployeeSkill employeeSkill);

        List<VEmployeeSkillDetail> GetAllEmployeeSkillDetails(int startPage, int pageSize, EmployeeSkill model, SearchFieldModel searchFieldModel, out int totalRecords);

        int EditEmployeeSkill(EmployeeSkill employeeSkill);
        int DeleteEmployeeSkillById(int employeeSkillId); 
    }
}  
 