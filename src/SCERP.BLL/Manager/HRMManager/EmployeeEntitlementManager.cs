using System;
using SCERP.BLL.IManager.IHRMManager;
using SCERP.Common;
using SCERP.DAL;
using SCERP.DAL.IRepository.IHRMRepository;
using SCERP.DAL.Repository.HRMRepository;
using SCERP.Model;
using System.Collections.Generic;


namespace SCERP.BLL.Manager.HRMManager
{
    public class EmployeeEntitlementManager : BaseManager, IEmployeeEntitlementManager
    {
        
        private readonly IEmployeeEntitlementRepository _employeeEntitlementRepository = null;

        public EmployeeEntitlementManager(SCERPDBContext context)
        {          
            this._employeeEntitlementRepository = new EmployeeEntitlementRepository(context);
        }

        public List<EmployeeEntitlement> GetEmployeeEntitlementInfoByEmployeeGuidId(Guid employeeId)
        {
            List<EmployeeEntitlement> employeeEntitlements;
            try
            {
                employeeEntitlements = _employeeEntitlementRepository.GetEmployeeEntitlementInfoByEmployeeGuidId(employeeId);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
            return employeeEntitlements;
        }

        public int SaveEmployeeEntitlementInfo(EmployeeEntitlement employeeEntitlement)
        {
            var saved = 0;
            try
            {
                employeeEntitlement.CreatedBy = PortalContext.CurrentUser.UserId;
                employeeEntitlement.CreatedDate = DateTime.Now;
                employeeEntitlement.IsActive = true;
                saved = _employeeEntitlementRepository.Save(employeeEntitlement);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }

            return saved;
        }

        public int EditEmployeeEntitlementInfo(EmployeeEntitlement employeeEntitlement)
        {
            var edit = 0;
            try
            {
                employeeEntitlement.EditedBy = PortalContext.CurrentUser.UserId;
                employeeEntitlement.EditedDate = DateTime.Now;
                edit = _employeeEntitlementRepository.Edit(employeeEntitlement);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
            return edit;
        }

        public int DeleteEmployeeEntitlementInfo(EmployeeEntitlement employeeEntitlement)
        {
            var deleted = 0;
            try
            {
                employeeEntitlement.EditedBy = PortalContext.CurrentUser.UserId;
                employeeEntitlement.EditedDate = DateTime.Now;
                employeeEntitlement.IsActive = false;
                deleted = _employeeEntitlementRepository.Edit(employeeEntitlement);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
            return deleted;
        }

        public EmployeeEntitlement GetEmployeeEntitlementInfoById(Guid employeeId, int id)
        {
            EmployeeEntitlement employeeEntitlement;

            try
            {
                employeeEntitlement = _employeeEntitlementRepository.GetEmployeeEntitlementInfoById(employeeId, id);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
            return employeeEntitlement;
        }

      
        public EmployeeEntitlement GetLatestEmployeeEntitlementInfoByEmployeeGuidId(Guid employeeId, int entitlementId)
        {
            EmployeeEntitlement employeeEntitlement = null;
            try
            {
                employeeEntitlement =
                    _employeeEntitlementRepository.GetLatestEmployeeEntitlementInfoByEmployeeGuidId(employeeId, entitlementId);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message, exception.InnerException);
            }
            return employeeEntitlement;
        }

        public int UpdateEmployeeEntitlementInfoDate(EmployeeEntitlement employeeEntitlement)
        {
            var updated = 0;
            try
            {
                employeeEntitlement.EditedBy = PortalContext.CurrentUser.UserId;
                employeeEntitlement.EditedDate = DateTime.Now;
                updated = _employeeEntitlementRepository.UpdateEmployeeEntitlementInfoDate(employeeEntitlement);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
            return updated;
        }

    }
}
