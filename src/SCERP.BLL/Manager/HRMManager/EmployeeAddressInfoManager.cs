using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using SCERP.BLL.IManager.IHRMManager;
using SCERP.Common;
using SCERP.DAL.IRepository.IHRMRepository;
using SCERP.DAL.Repository.HRMRepository;
using SCERP.Model;
using SCERP.DAL;
using SCERP.Model.Custom;

namespace SCERP.BLL.Manager.HRMManager
{
    public class EmployeeAddressInfoManager : BaseManager, IEmployeeAddressInfoManager
    {
        private readonly IEmployeePresentAddressRepository _employeePresentAddressRepository = null;
        private readonly IEmployeePermanentAddressRepository _employeePermanentAddressRepository = null;

        public EmployeeAddressInfoManager(SCERPDBContext context)
        {
            _employeePresentAddressRepository = new EmployeePresentAddressRepository(context);
            _employeePermanentAddressRepository = new EmployeePermanentAddressRepository(context);
        }

        public EmployeeAddressInfoCustomModel GetEmployeeAddressInfoByEmployeeGuidId(Guid employeeGuid)
        {
            var employeeAddressInfos=new EmployeeAddressInfoCustomModel();

            try
            {
                var employeePresentAddresses = _employeePresentAddressRepository.GetEmployeePresentAddressesByEmployeeGuidId(employeeGuid);
                var employeePermanentAddresses = _employeePermanentAddressRepository.GetEmployeePermanentAddressesByEmployeeGuidId(employeeGuid);

                employeeAddressInfos.EmployeePresentAddresses = employeePresentAddresses;
                employeeAddressInfos.EmployeePermanentAddresses = employeePermanentAddresses;
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }

            return employeeAddressInfos;
        }

        public EmployeePresentAddress GetEmployeePresentAddressById(Guid employeeGuid, int id)
        {
            EmployeePresentAddress employeePresentAddress = null;
            
            try
            {
                employeePresentAddress = _employeePresentAddressRepository.GetEmployeePresentAddressById(employeeGuid,id);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }

            return employeePresentAddress;
        }

        public EmployeePermanentAddress GetEmployeePermanentAddressById(Guid employeeGuid, int id)
        {
            EmployeePermanentAddress employeePermanentAddress = null;

            try
            {
                employeePermanentAddress = _employeePermanentAddressRepository.GetEmployeePermanentAddressById(employeeGuid,id);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }

            return employeePermanentAddress;
        }

        public int SaveEmployeePresentAddress(EmployeePresentAddress employeePresentAddress)
        {
            var saved = 0;
            try
            {
                employeePresentAddress.CreatedBy = PortalContext.CurrentUser.UserId;
                employeePresentAddress.CreatedDate = DateTime.Now;
                employeePresentAddress.IsActive = true;
                saved = _employeePresentAddressRepository.Save(employeePresentAddress);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }

            return saved;
        }

        public int EditEmployeePresentAddress(EmployeePresentAddress employeePresentAddress)
        {
            var edit = 0;
            try
            {
                employeePresentAddress.EditedBy = PortalContext.CurrentUser.UserId;
                employeePresentAddress.EditedDate = DateTime.Now;
                edit = _employeePresentAddressRepository.Edit(employeePresentAddress);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
            return edit;
        }

        public EmployeePresentAddress GetLatestEmployeePresentAddressByEmployeeGuidId(Guid employeeId)
        {
            EmployeePresentAddress employeePresentAddress = null;
            try
            {
                employeePresentAddress =
                    _employeePresentAddressRepository.GetLatestEmployeePresentAddressByEmployeeGuidId(employeeId);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
            return employeePresentAddress;
        }

        public int UpdateEmployeePresentAddress(EmployeePresentAddress employeePresentAddress)
        {
            var updated = 0;
            try
            {
                employeePresentAddress.EditedBy = PortalContext.CurrentUser.UserId;
                employeePresentAddress.EditedDate = DateTime.Now;
                employeePresentAddress.Status = (int)StatusValue.InActive;
                updated = _employeePresentAddressRepository.Edit(employeePresentAddress);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
            return updated;
        }

        public int DeleteEmployeePresentAddress(EmployeePresentAddress employeePresentAddress)
        {
            var deleted = 0;
            try
            {
                employeePresentAddress.EditedBy = PortalContext.CurrentUser.UserId;
                employeePresentAddress.EditedDate = DateTime.Now;
                employeePresentAddress.IsActive = false;
                deleted = _employeePresentAddressRepository.Edit(employeePresentAddress);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
            return deleted;
        }

        public int SaveEmployeePermanentAddress(EmployeePermanentAddress employeePermanentAddress)
        {
            var saved = 0;
            try
            {
                employeePermanentAddress.CreatedBy = PortalContext.CurrentUser.UserId;
                employeePermanentAddress.CreatedDate = DateTime.Now;
                employeePermanentAddress.IsActive = true;
                saved = _employeePermanentAddressRepository.Save(employeePermanentAddress);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }

            return saved;
        }

        public int EditEmployeePermanentAddress(EmployeePermanentAddress employeePermanentAddress)
        {
            var edit = 0;
            try
            {
                employeePermanentAddress.EditedBy = PortalContext.CurrentUser.UserId;
                employeePermanentAddress.EditedDate = DateTime.Now;
                edit = _employeePermanentAddressRepository.Edit(employeePermanentAddress);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
            return edit;
        }

        public EmployeePermanentAddress GetLatestEmployeePermanentAddressByEmployeeGuidId(Guid employeeId)
        {
            EmployeePermanentAddress employeePermanentAddress = null;
            try
            {
                employeePermanentAddress =
                    _employeePermanentAddressRepository.GetLatestEmployeePermanentAddressByEmployeeGuidId(employeeId);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
            return employeePermanentAddress;
        }

        public int UpdateEmployeePermanentAddress(EmployeePermanentAddress employeePermanentAddress)
        {
            var updated = 0;
            try
            {
                employeePermanentAddress.EditedBy = PortalContext.CurrentUser.UserId;
                employeePermanentAddress.EditedDate = DateTime.Now;
                employeePermanentAddress.Status = (int)StatusValue.InActive;
                updated = _employeePermanentAddressRepository.Edit(employeePermanentAddress);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
            return updated;
        }

        public int DeleteEmployeePermanentAddress(EmployeePermanentAddress employeePermanentAddress)
        {
            var deleted = 0;
            try
            {
                employeePermanentAddress.EditedBy = PortalContext.CurrentUser.UserId;
                employeePermanentAddress.EditedDate = DateTime.Now;
                employeePermanentAddress.IsActive = false;
                deleted = _employeePermanentAddressRepository.Edit(employeePermanentAddress);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
            return deleted;
        }      
    }
}
