using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;
using SCERP.Model.Custom;

namespace SCERP.DAL.IRepository.IHRMRepository
{
    public interface IEmployeeFamilyInfoRepository : IRepository<EmployeeFamilyInfo>
    {
        List<EmployeeFamilyInfo> GetEmployeeFamilyInfoByEmployeeGuidId(Guid employeeGuid);

        EmployeeFamilyInfo GetEmployeeFamilyInfoById(Guid employeeId, int id);
    }
}
