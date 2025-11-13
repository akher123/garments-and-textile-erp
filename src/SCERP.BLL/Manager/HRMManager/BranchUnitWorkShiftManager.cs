using System;
using System.Collections;
using System.Collections.Generic;
using SCERP.BLL.IManager.IHRMManager;
using SCERP.Common;
using SCERP.DAL;
using SCERP.DAL.IRepository.IHRMRepository;
using SCERP.DAL.Repository.HRMRepository;
using SCERP.Model;
using SCERP.Model.Custom;

namespace SCERP.BLL.Manager.HRMManager
{
    public class BranchUnitWorkShiftManager : IBranchUnitWorkShiftManager
    {
        private readonly IBranchUnitWorkShiftRepository _branchUnitWorkShiftRepository;
        public BranchUnitWorkShiftManager(SCERPDBContext context)
        {
           _branchUnitWorkShiftRepository=new BranchUnitWorkShiftRepository(context);
        }

        public List<BranchUnitWorkShift> GetBranchUnitWorkShiftsBuPaging(int startPage, int pageSize, out int totalRecords,
            SearchFieldModel searchFieldModel, BranchUnitWorkShift model)
        {

            List<BranchUnitWorkShift> branchUnitWorkShifts;
            try
            {
                branchUnitWorkShifts = _branchUnitWorkShiftRepository.GetBranchUnitWorkShiftsBuPaging(startPage, pageSize, out totalRecords, searchFieldModel, model);
            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message);
            }
            return branchUnitWorkShifts;
        }

        public BranchUnitWorkShift GetBranchUnitWorkShiftById(int branchUnitWorkShiftId)
        {
            BranchUnitWorkShift branchUnitWorkShift;
            try
            {

                branchUnitWorkShift = _branchUnitWorkShiftRepository.GetBranchUnitDepartmentById(branchUnitWorkShiftId);
            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message);
            }
            return branchUnitWorkShift;
        }

        public bool IsExistBranchUnitWorkShift(BranchUnitWorkShift model)
        {
            bool isExist;
            try
            {
                isExist = _branchUnitWorkShiftRepository.Exists(x => x.IsActive && x.BranchUnitWorkShiftId != model.BranchUnitWorkShiftId
                                                                     && x.WorkShiftId == model.WorkShiftId && x.BranchUnitId == model.BranchUnitId && model.FromDate >= x.FromDate && model.FromDate <= x.ToDate);
            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message);
            }
            return isExist;
        }

        public int EditBranchUnitWorkShift(BranchUnitWorkShift model)
        {
            var editIndex = 0;
            try
            {
                var branchUnitWorkShiftObj = _branchUnitWorkShiftRepository.FindOne(x => x.IsActive && x.BranchUnitWorkShiftId == model.BranchUnitWorkShiftId);
                branchUnitWorkShiftObj.BranchUnitId = model.BranchUnitId;
                branchUnitWorkShiftObj.WorkShiftId = model.WorkShiftId;
                branchUnitWorkShiftObj.Description = model.Description;
                branchUnitWorkShiftObj.FromDate = model.FromDate;
                branchUnitWorkShiftObj.ToDate = model.ToDate;
                branchUnitWorkShiftObj.EditedDate = DateTime.Now;
                branchUnitWorkShiftObj.EditedBy = PortalContext.CurrentUser.UserId;
                editIndex = _branchUnitWorkShiftRepository.Edit(branchUnitWorkShiftObj);

            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message);
            }
            return editIndex;
        }

        public int SaveBranchUnitWorkShift(BranchUnitWorkShift model)
        {
            var saveIndex = 0;
            try
            {
                model.CreatedBy = PortalContext.CurrentUser.UserId;
                model.CreatedDate = DateTime.Now;
                model.Status = (Int16) StatusValue.Active;
                model.IsActive = true;
                saveIndex = _branchUnitWorkShiftRepository.Save(model);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
            return saveIndex;
        }

        public int DeleteBranchUnitWorkShift(int branchUnitWorkShiftId)
        {
            var deleteIndex = 0;
            try
            {
                var brUnitDeptObj = _branchUnitWorkShiftRepository.FindOne(x => x.IsActive && x.BranchUnitWorkShiftId == branchUnitWorkShiftId);
                brUnitDeptObj.IsActive = false;
                brUnitDeptObj.EditedDate = DateTime.Now;
                brUnitDeptObj.EditedBy = PortalContext.CurrentUser.UserId;
                deleteIndex = _branchUnitWorkShiftRepository.Edit(brUnitDeptObj);
            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message);
            }
            return deleteIndex;
        }

        public List<BranchUnitWorkShift> GetBranchUnitWorkShiftBySearchKey(SearchFieldModel searchFieldModel)
        {
            List<BranchUnitWorkShift> branchUnitWorkShifts;
            try
            {
                branchUnitWorkShifts = _branchUnitWorkShiftRepository.GetBranchUnitWorkShiftBySearchKey(searchFieldModel);
            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message);
            }
            return branchUnitWorkShifts;
        }

        public List<WorkShift> GetBranchUnitWorkShiftByBrancUnitId(int searchByBranchUnitId)
        {
            List<WorkShift> branchUnitWorkShifts;
            try
            {
                branchUnitWorkShifts = _branchUnitWorkShiftRepository.GetBranchUnitWorkShiftByBrancUnitId(searchByBranchUnitId);
            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message);
            }
            return branchUnitWorkShifts;
        }
    }
}
