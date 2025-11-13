using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;

namespace SCERP.DAL.IRepository.IHRMRepository
{
    public interface IEntitlementRepository : IRepository<Entitlement>
    {
        Entitlement GetEntitlementById(int? id);
        List<Entitlement> GetAllEducationLevelsByPaging(int startPage, int pageSize, out int totalRecords, Entitlement entitlement);
        List<Entitlement> GetEntitlementBySearchKey(Entitlement entitlement);
    }
}
