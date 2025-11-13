using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using SCERP.Common;
using SCERP.DAL.IRepository.IHRMRepository;
using SCERP.Model;
using System.Data;
using System.Data.Entity;
using SCERP.Model.Custom;


namespace SCERP.DAL.Repository.HRMRepository
{
    public class EmployeeEntitlementRepository : Repository<EmployeeEntitlement>, IEmployeeEntitlementRepository
    {



        public EmployeeEntitlementRepository(SCERPDBContext context)
            : base(context)
        {

        }

        public List<EmployeeEntitlement> GetEmployeeEntitlementInfoByEmployeeGuidId(Guid employeeGuid)
        {
            IQueryable<EmployeeEntitlement> employeeEntitlements;
            try
            {
                employeeEntitlements =
                    Context.EmployeeEntitlements
                        .Where(x => x.IsActive)
                        .Where(x => (x.EmployeeId == employeeGuid || employeeGuid == null) && (x.IsActive == true))
                        .Include(x => x.Entitlement)
                        .OrderByDescending(x => x.Id);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
            return employeeEntitlements.ToList();
        }



        public EmployeeEntitlement GetEmployeeEntitlementInfoById(Guid employeeId, int id)
        {
            IQueryable<EmployeeEntitlement> employeeEntitlements;
            try
            {
                employeeEntitlements =
                    Context.EmployeeEntitlements
                        .Where(x => x.IsActive)
                        .Where(x => (x.EmployeeId == employeeId) && (x.Id == id));
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
            return employeeEntitlements.ToList().FirstOrDefault();
        }

        public EmployeeEntitlement GetLatestEmployeeEntitlementInfoByEmployeeGuidId(Guid employeeId, int entitlementId)
        {
            EmployeeEntitlement employeeEntitlements;
            try
            {
                employeeEntitlements =
                    Context.EmployeeEntitlements.Where(x => x.IsActive && x.EmployeeId == employeeId && x.EntitlementId == entitlementId).OrderByDescending(x => x.FromDate).ToList().FirstOrDefault();
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message, exception.InnerException);
            }
            return employeeEntitlements;
        }

        public int UpdateEmployeeEntitlementInfoDate(EmployeeEntitlement employeeEntitlement)
        {
            var updated = 0;
            try
            {
                updated = Edit(employeeEntitlement);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
            return updated;
        }
    }
}
