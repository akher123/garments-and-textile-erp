using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;
using System.Linq;

namespace SCERP.BLL.IManager.IHRMManager
{
    public interface IEntitlementManager
    {
        List<Entitlement> GetAllEducationLevelsByPaging(int startPage, int pageSize, out int totalRecords,Entitlement entitlement);

        Entitlement GetEntitlementById(int? id);

        int SaveEntitlement(Entitlement entitlement);

        int EditEntitlement(Entitlement entitlement);

        int DeleteEntitlement(Entitlement entitlement);

        List<Entitlement> GetEntitlementBySearchKey(Entitlement entitlement);

        bool IsEntitlementExist(Entitlement entitlement);

        List<Entitlement> GetAllEntitlements();
    }
}
