using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.BLL.IManager.IHRMManager;
using SCERP.Common;
using SCERP.DAL;
using SCERP.DAL.IRepository.IHRMRepository;
using SCERP.DAL.Repository.HRMRepository;
using SCERP.Model;
using System.Linq;

namespace SCERP.BLL.Manager.HRMManager
{
    public class EntitlementManager : BaseManager, IEntitlementManager
    {
        private readonly IEntitlementRepository _entitlementRepository = null;

        public EntitlementManager(SCERPDBContext context)
        {
            _entitlementRepository = new EntitlementRepository(context);
        }

        public List<Entitlement> GetAllEducationLevelsByPaging(int startPage, int pageSize, out int totalRecords, Entitlement entitlement)
        {
            List<Entitlement> entitlementList = null;

            try
            {
                entitlementList = _entitlementRepository.GetAllEducationLevelsByPaging(startPage, pageSize, out totalRecords, entitlement);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }

            return entitlementList;
        }

        public Entitlement GetEntitlementById(int? id)
        {
            Entitlement entitlement = null;
            try
            {
                entitlement = _entitlementRepository.GetEntitlementById(id);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }

            return entitlement;
        }

        public int SaveEntitlement(Entitlement entitlement)
        {
            int savedLeaveType = 0;
            try
            {
                entitlement.CreatedDate = DateTime.Now;
                entitlement.CreatedBy = PortalContext.CurrentUser.UserId;
                entitlement.IsActive = true;
                savedLeaveType = _entitlementRepository.Save(entitlement);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }

            return savedLeaveType;
        }

        public int EditEntitlement(Entitlement entitlement)
        {
            int editedEntitlement = 0;
            try
            {
                entitlement.EditedDate = DateTime.Now;
                entitlement.EditedBy = PortalContext.CurrentUser.UserId;
                editedEntitlement = _entitlementRepository.Edit(entitlement);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }

            return editedEntitlement;
        }

        public int DeleteEntitlement(Entitlement entitlement)
        {

            int deletedEntitlement = 0;
            try
            {
                entitlement.EditedDate = DateTime.Now;
                entitlement.EditedBy = PortalContext.CurrentUser.UserId;
                entitlement.IsActive = false;

                deletedEntitlement = _entitlementRepository.Edit(entitlement);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }

            return deletedEntitlement;
        }

        public List<Entitlement> GetEntitlementBySearchKey(Entitlement entitlement)
        {
            List<Entitlement> entitlementList;
            try
            {

                entitlementList = _entitlementRepository.GetEntitlementBySearchKey(entitlement);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }

            return entitlementList;
        }

        public bool IsEntitlementExist(Entitlement entitlement)
        {

            var isExist = false;
            try
            {
                isExist =
                    _entitlementRepository.Exists(
                        x =>
                            x.Id != entitlement.Id &&
                            x.Title.Replace(" ", "").ToLower().Equals(entitlement.Title.Replace(" ", "").ToLower()));
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
            return isExist;
        }


        public List<Entitlement> GetAllEntitlements()
        {
            List<Entitlement> entitlementList;
            try
            {
                entitlementList = _entitlementRepository.Filter(x => x.IsActive).OrderBy(x => x.Title).ToList();
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }

            return entitlementList;
        }
    }
}
