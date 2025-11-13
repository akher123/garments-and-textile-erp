using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;

namespace SCERP.BLL.IManager.IHRMManager
{
    public interface IEmploymentManager
    {
        List<Employment> GetEmploymentsByEmployeeId(Guid employeeId);
        Employment GetEmploymentById(int id);
        Employment GetEmploymentById(Guid? employeeId, int? id);
        int EditEmployment(Employment employment);
        int SaveEmployment(Employment employment);
        int DeleteEmployment(Employment employment);
        bool CheckExistingEmploymentInfo(Employment employment);
    }
}
