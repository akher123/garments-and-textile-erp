using System;
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
    public class LeaveSettingManager : BaseManager, ILeaveSettingManager
    {

        private readonly ILeaveSettingRepository _leaveSettingRepository = null;

        public LeaveSettingManager(SCERPDBContext context)
        {
            this._leaveSettingRepository = new LeaveSettingRepository(context);
        }



        public List<LeaveSetting> GetAllLeaveSettings(int startPage, int pageSize, LeaveSetting model, SearchFieldModel searchFieldModel, out int totalRecords)
        {
            List<LeaveSetting> leaveSettings;
            try
            {
                leaveSettings = _leaveSettingRepository.GetAllLeaveSettings(startPage, pageSize, model,searchFieldModel, out totalRecords);
            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message);

            }
            return leaveSettings;
        }

        public LeaveSetting GetLeaveSettingById(int? id)
        {
            return _leaveSettingRepository.GetLeaveSettingById(id);
        }

        public int SaveLeaveSetting(LeaveSetting aLeaveSetting)
        {
            var savedLeaveSetting = 0;

            try
            {
                if (aLeaveSetting.Id > 0)
                {
                    aLeaveSetting.EditedBy = PortalContext.CurrentUser.UserId;
                    aLeaveSetting.EditedDate = DateTime.Now;
                }
                else
                {
                    aLeaveSetting.CreatedBy = PortalContext.CurrentUser.UserId;
                    aLeaveSetting.CreatedDate = DateTime.Now;
                }

                aLeaveSetting.IsActive = true;
                savedLeaveSetting = _leaveSettingRepository.Save(aLeaveSetting);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return savedLeaveSetting;
        }

        public int DeleteLeaveSetting(int id)
        {
            var deleteIndex = 0;
            try
            {
                LeaveSetting leaveSetting = _leaveSettingRepository.FindOne(x => x.Id == id);
                leaveSetting.IsActive = false;
                deleteIndex = _leaveSettingRepository.Edit(leaveSetting);
            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message);
            }
            return deleteIndex;

        }

        public List<LeaveType> GetAllLeaveType()
        {
            List<LeaveType> leaveTypes;
            try
            {

                leaveTypes = _leaveSettingRepository.GetAllLeaveType();
            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message);
            }
            return leaveTypes;

        }

        public List<EmployeeType> GetAllEmployeeType()
        {
            List<EmployeeType> employeesTypes;
            try
            {

                employeesTypes = _leaveSettingRepository.GetAllEmployeeType();
            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message);
            }
            return employeesTypes;
        }

        public bool IsExistLeaveSetting(LeaveSetting model)
        {
            bool isExist;
            try
            {
                isExist =
                _leaveSettingRepository.Exists(
                    x =>
                        x.IsActive == true &&
                        x.Id != model.Id &&
                        x.BranchUnitId == model.BranchUnitId &&
                        x.LeaveTypeId == model.LeaveTypeId &&
                        x.EmployeeTypeId == model.EmployeeTypeId
                       );
            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message);
            }
            return isExist;
        }

        public List<LeaveSetting> GetAllLeaveSettingsBySearchKey(LeaveSetting leaveSetting, SearchFieldModel searchFieldModel)
        {
            List<LeaveSetting> leaveSettings;
            try
            {
                leaveSettings = _leaveSettingRepository.GetAllLeaveSettingsBySearchKey(leaveSetting, searchFieldModel);
            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message);
            }
            return leaveSettings;
        }
    }
}
