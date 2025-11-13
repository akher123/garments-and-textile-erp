using System;
using System.Collections;
using SCERP.BLL.IManager.IHRMManager;
using SCERP.Common;
using SCERP.DAL;
using SCERP.DAL.IRepository.IHRMRepository;
using SCERP.DAL.Repository.HRMRepository;
using System.Linq;
using SCERP.Model;
using System.Collections.Generic;

namespace SCERP.BLL.Manager.HRMManager
{
    public class EmployeeTypeManager : BaseManager, IEmployeeTypeManager
    {

        private readonly IEmployeeTypeRepository _employeeTypeRepository = null;

        public EmployeeTypeManager(SCERPDBContext context)
        {
            _employeeTypeRepository = new EmployeeTypeRepository(context);
        }

        public List<EmployeeType> GetAllEmployeeTypesByPaging(int startPage, int pageSize, out int totalRecords, EmployeeType employeeType)
        {
            var employeeTypes = new List<EmployeeType>();
            try
            {
                employeeTypes = _employeeTypeRepository.GetAllEmployeeTypesByPaging(startPage, pageSize, out totalRecords, employeeType).ToList();
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                totalRecords = 0;
            }

            return employeeTypes;
        }

        public EmployeeType GetEmployeeTypeById(int? id)
        {
            return _employeeTypeRepository.GetEmployeeTypeById(id);
        }

        public bool CheckExistingEmployeeType(EmployeeType employeeType)
        {
            bool isExist = false;
            try
            {
                isExist =
                    _employeeTypeRepository.Exists(
                        x =>
                            x.IsActive == true &&
                            x.Id != employeeType.Id &&
                            x.Title.Replace(" ", "").ToLower().Equals(employeeType.Title.Replace(" ", "").ToLower()));
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return isExist;
        }

        public List<EmployeeType> GetEmployeeTypeBySearchKey(string searchKey)
        {
            var employeeTypes = new List<EmployeeType>();
            try
            {
                employeeTypes = _employeeTypeRepository.Filter(x => x.Title.Replace(" ", "").ToLower().Contains(searchKey.Replace(" ", "").ToLower()) || String.IsNullOrEmpty(searchKey)).ToList();

            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return employeeTypes;
        }

        public int SaveEmployeeType(EmployeeType employeeType)
        {
            int savedEmployeeType = 0;
            try
            {
                employeeType.CDT = DateTime.Now;
                employeeType.CreatedBy = PortalContext.CurrentUser.UserId;
                employeeType.IsActive = true;
                savedEmployeeType = _employeeTypeRepository.Save(employeeType);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
         
            }

            return savedEmployeeType;
        }

        public int EditEmployeeType(EmployeeType employeeType)
        {
            int editedEmployeeType = 0;
            try
            {
                employeeType.EDT = DateTime.Now;
                employeeType.EditedBy = PortalContext.CurrentUser.UserId;
                editedEmployeeType = _employeeTypeRepository.Edit(employeeType);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                editedEmployeeType = 0;
            }

            return editedEmployeeType;
        }

        public int DeleteEmployeeType(EmployeeType employeeType)
        {

            int deletedEmployeeType = 0;
            try
            {
                employeeType.EDT = DateTime.Now;
                employeeType.EditedBy = PortalContext.CurrentUser.UserId;
                employeeType.IsActive = false;

                deletedEmployeeType = _employeeTypeRepository.Edit(employeeType);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
       
            }

            return deletedEmployeeType;
        }


        public List<EmployeeType> GetAllEmployeeTypes()
        {
            List<EmployeeType> employeeTypes;

            try
            {
                employeeTypes = _employeeTypeRepository.Filter(x=>x.IsActive).ToList();
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                employeeTypes = null;
            }

            return employeeTypes;
        }

        public IEnumerable GetAllPermittedEmployeeTypes() // From portal context
        {

            IEnumerable employeeTypes;
            try
            {
                employeeTypes = PortalContext.CurrentUser.PermissionContext.EmployeeTypeList;
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }

            return employeeTypes;
        }


        public List<Common.PermissionModel.UserEmployeeType> GetAllPermittedEmployeeTypeId() // From portal context
        {

            List<Common.PermissionModel.UserEmployeeType> employeeTypes;
            try
            {
                employeeTypes = PortalContext.CurrentUser.PermissionContext.EmployeeTypeList;
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }

            return employeeTypes;
        }
    }
}
