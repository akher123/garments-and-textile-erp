using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;

namespace SCERP.BLL.IManager.IHRMManager
{
    public interface IEmployeeEntitlementManager
    {
       
        List<EmployeeEntitlement> GetEmployeeEntitlementInfoByEmployeeGuidId(Guid employeeId);

        int SaveEmployeeEntitlementInfo(EmployeeEntitlement employeeEntitlement);

        int EditEmployeeEntitlementInfo(EmployeeEntitlement employeeEntitlement);

        int DeleteEmployeeEntitlementInfo(EmployeeEntitlement employeeEntitlement);

        EmployeeEntitlement GetEmployeeEntitlementInfoById(Guid employeeId, int id);

        EmployeeEntitlement GetLatestEmployeeEntitlementInfoByEmployeeGuidId(Guid employeeId, int entitlementId);

        int UpdateEmployeeEntitlementInfoDate(EmployeeEntitlement employeeEntitlement);
    }
}
