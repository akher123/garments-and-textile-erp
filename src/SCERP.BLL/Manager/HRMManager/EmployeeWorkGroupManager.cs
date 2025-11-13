using System;
using System.Collections.Generic;
using System.Linq;
using SCERP.BLL.IManager.IHRMManager;
using SCERP.Common;
using SCERP.DAL;
using SCERP.DAL.IRepository.IHRMRepository;
using SCERP.DAL.Repository.HRMRepository;
using SCERP.Model;
using SCERP.Model.Custom;

namespace SCERP.BLL.Manager.HRMManager
{
    public class EmployeeWorkGroupManager : IEmployeeWorkGroupManager
    {
        private readonly IEmployeeWorkGroupRepository _employeeWorkGroupRepository = null;
        public EmployeeWorkGroupManager(SCERPDBContext context)
        {
            _employeeWorkGroupRepository = new EmployeeWorkGroupRepository(context);
        }

        public int SaveEmployeeWorkGroups(List<EmployeeWorkGroup> employeeWorkGroups)
        {
            var saveIndex = 0;
            try
            {
                saveIndex = _employeeWorkGroupRepository.SaveEmployeeWorkGroups(employeeWorkGroups);
            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message);
            }
            return saveIndex;
        }
        public int EditEmployeeWorkGroups(List<EmployeeWorkGroup> employeeWorkGroups)
        {
            var saveIndex = 0;
            try
            {
                foreach (var ewg in employeeWorkGroups)
                {
                    var employeeWorkGroup = _employeeWorkGroupRepository.FindOne(x => x.EmployeeWorkGroupId == ewg.EmployeeWorkGroupId && x.Status && x.IsActive);
                    employeeWorkGroup.Status = false;
                    employeeWorkGroup.EditedDate = DateTime.Now;
                    employeeWorkGroup.AssignedDate = ewg.AssignedDate;
                    employeeWorkGroup.EditedBy = PortalContext.CurrentUser.UserId;
                    saveIndex = _employeeWorkGroupRepository.Edit(employeeWorkGroup);
                    if (saveIndex > 0)
                    {
                        ewg.EmployeeId = employeeWorkGroup.EmployeeId;
                        ewg.CreatedDate = DateTime.Now;
                        ewg.CreatedBy = PortalContext.CurrentUser.UserId;
                        ewg.Status = true;
                        ewg.EmployeeWorkGroupId = 0;
                        saveIndex += _employeeWorkGroupRepository.Save(ewg);
                    }
                }
            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message);
            }
            return saveIndex;
        }

        public List<EmployeeWorkGroup> GetAllAssignedEmployeeWorkGroup(SearchFieldModel searchFieldModel)
        {
            List<EmployeeWorkGroup> employeeWorkGroups;
            try
            {
                employeeWorkGroups = _employeeWorkGroupRepository.GetAllAssignedEmployeeWorkGroup(searchFieldModel);
            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message);
            }
            return employeeWorkGroups;
        }

        public int DeleteEmployeeWorkGroup(int? id)
        {
            var deleteIndex = 0;
            try
            {
                var employeeWorkGroupObj = _employeeWorkGroupRepository.FindOne(x => x.IsActive && x.Status && x.EmployeeWorkGroupId == id);
                employeeWorkGroupObj.IsActive = false;
                deleteIndex = _employeeWorkGroupRepository.Edit(employeeWorkGroupObj);
            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message);
            }
            return deleteIndex;
        }
        public List<VEmployeeCompanyInfoDetail> GetAllUnAssignedEmployeeWorkGroup(int startPage, int pageSize, out int totalRecords,SearchFieldModel searchFieldModel, EmployeeWorkGroup model)
        {
            List<VEmployeeCompanyInfoDetail> employeeWorkGroups;
            try
            {
                employeeWorkGroups = _employeeWorkGroupRepository.GetAllUnAssignedEmployeeWorkGroup(startPage, pageSize,out totalRecords,searchFieldModel, model).ToList();

            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message);
            }
            return employeeWorkGroups;
        }


        public List<VEmployeeWorkGroupDetail> GetAllEmployeeWorkGroupByPaging(int startPage, int pageSize, out int totalRecords,
            SearchFieldModel searchFieldModel, EmployeeWorkGroup model)
        {
            List<VEmployeeWorkGroupDetail> employeeWorkGroups;
            try
            {
                employeeWorkGroups = _employeeWorkGroupRepository.GetAllEmployeeWorkGroupByPaging(startPage, pageSize, out totalRecords, model, searchFieldModel);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
            return employeeWorkGroups;
        }

        public List<VEmployeeWorkGroupDetail> GetEmployeeWorkGroupDetailBySearchKey(SearchFieldModel searchField)
        {
            List<VEmployeeWorkGroupDetail> employeeWorkGroups;
            try
            {
                employeeWorkGroups = _employeeWorkGroupRepository.GetEmployeeWorkGroupDetailBySearchKey(searchField);
            }
            catch (Exception exception)
            {

                throw;
            }
            return employeeWorkGroups;
        }
    }
}
