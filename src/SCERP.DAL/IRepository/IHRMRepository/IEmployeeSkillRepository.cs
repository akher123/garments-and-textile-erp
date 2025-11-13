using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;
using SCERP.Model.Custom;

namespace SCERP.DAL.IRepository.IHRMRepository
{
    public interface IEmployeeSkillRepository : IRepository<EmployeeSkill>
    {
        List<VEmployeeSkillDetail> GetAllEmployeeSkillDetails(int startPage, int pageSize, EmployeeSkill model, SearchFieldModel searchFieldModel, out int totalRecords);

        EmployeeSkill GetEmployeeSkillById(int employeeSkillId); 
    }  
}
