using System;
using System.Data.Entity;
using SCERP.BLL.IManager.IHRMManager;
using SCERP.Common;
using SCERP.DAL;
using SCERP.DAL.IRepository.IHRMRepository;
using SCERP.DAL.Repository.HRMRepository;
using System.Linq;
using SCERP.Model;
using System.Collections.Generic;
using SCERP.Model.Custom;


namespace SCERP.BLL.Manager.HRMManager
{
    public class WorkGroupManager : BaseManager, IWorkGroupManager
    {
        private readonly IWorkGroupRepository _workGroupRepository = null;
        public WorkGroupManager(SCERPDBContext context)
        {
            _workGroupRepository = new WorkGroupRepository(context);
        }

        public List<WorkGroup> GetAllWorkGroupsByPaging(int startPage, int pageSize, SearchFieldModel searchFieldModel, WorkGroup model, out int totalRecords)
        {
            List<WorkGroup> workGroups;
            try
            {
                workGroups = _workGroupRepository.GetAllWorkGroupsByPaging(startPage, pageSize, searchFieldModel, model, out totalRecords);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
            return workGroups;
        }

        public WorkGroup GetWorkGroupById(int workGroupId)
        {
            WorkGroup workGroup = null;
            try
            {
                workGroup = _workGroupRepository.GetWorkGroupById(workGroupId);

            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
            return workGroup;
        }

        public bool CheckExistingWorkGroup(WorkGroup model)
        {
            bool isExist = false;
            try
            {
                isExist = _workGroupRepository.Exists((x => x.IsActive
                          && (x.WorkGroupId != model.WorkGroupId)
                          && (x.BranchUnitId == model.BranchUnitId)
                          && (x.Name.Replace(" ", "").ToLower() == model.Name.Replace(" ", "").ToLower())));
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
            return isExist;
        }

        public int EditWorkGroup(WorkGroup workGroup)
        {
            var editIndex = 0;
            try
            {
                workGroup.EditedDate = DateTime.Now;
                workGroup.EditedBy = PortalContext.CurrentUser.UserId;
                editIndex = _workGroupRepository.Edit(workGroup);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
            return editIndex;
        }

        public int SaveWorkGroup(WorkGroup workGroup)
        {
            var saveIndex = 0;
            try
            {
                workGroup.CreatedBy = PortalContext.CurrentUser.UserId;
                workGroup.CreatedDate = DateTime.Now;
                workGroup.IsActive = true;
                saveIndex = _workGroupRepository.Save(workGroup);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
            return saveIndex;
        }

        public int DeleteWorkGroup(int workGroupId)
        {
            var deleteIndex = 0;
            try
            {
                var workGroup = _workGroupRepository.FindOne(x => x.IsActive == true && x.WorkGroupId == workGroupId);
                workGroup.EditedDate = DateTime.Now;
                workGroup.EditedBy = PortalContext.CurrentUser.UserId;
                workGroup.IsActive = false;
                deleteIndex = _workGroupRepository.Edit(workGroup);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
            return deleteIndex;
        }

        public List<WorkGroup> GetAllWorkGroupsBySearchKey(int companyId, int branchId, int unitId, string workGroupName)
        {
            List<WorkGroup> workGroups = null;
            try
            {

                workGroups = _workGroupRepository.All().Where(x => x.IsActive == true
                                                        && (x.BranchUnit.Branch.Company.Id == companyId || companyId == 0) &&
                                                        (x.BranchUnit.Branch.Id == branchId || branchId == 0) &&
                                                        (x.BranchUnit.BranchUnitId == unitId || unitId == 0) &&
                                                        (((x.Name.Replace(" ", "").ToLower()).Contains(workGroupName.Replace(" ", "").ToLower()))
                                                        || string.IsNullOrEmpty(workGroupName)))
                                                        .Include(x => x.BranchUnit)
                                                        .Include(x => x.BranchUnit.Branch)
                                                        .Include(x => x.BranchUnit.Branch.Company)
                                                        .Include(x => x.BranchUnit.Unit)
                                                        .ToList();
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
            return workGroups;
        }

        public List<WorkGroup> GetAllWorkGroups()
        {
            List<WorkGroup> workGroups;
            try
            {
                workGroups = _workGroupRepository.Filter(x => x.IsActive).OrderBy(x=>x.Name).ToList();
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
            return workGroups;
        }


        public List<WorkGroup> GetWorkGroupsByBranchUnitId(int? branchUnitId)
        {
            var workGroups = new List<WorkGroup>();
            try
            {
                workGroups=  _workGroupRepository.Filter(x => x.BranchUnitId == branchUnitId && x.IsActive).OrderBy(x=>x.Name).ToList();
            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message, exception.InnerException);
            }
            return workGroups;
        }
    }
}


