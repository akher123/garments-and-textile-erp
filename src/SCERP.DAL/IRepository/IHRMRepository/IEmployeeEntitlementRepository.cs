using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;
using SCERP.Model.Custom;

namespace SCERP.DAL.IRepository.IHRMRepository
{
    public interface IEmployeeEntitlementRepository : IRepository<EmployeeEntitlement>
    {
        List<EmployeeEntitlement> GetEmployeeEntitlementInfoByEmployeeGuidId(Guid employeeGuid);
        EmployeeEntitlement GetEmployeeEntitlementInfoById(Guid employeeId, int id);
        EmployeeEntitlement GetLatestEmployeeEntitlementInfoByEmployeeGuidId(Guid employeeId, int entitlementId);
        int UpdateEmployeeEntitlementInfoDate(EmployeeEntitlement employeeEntitlement);
    }
}
