using SCERP.BLL.IManager.IHRMManager;
using SCERP.Common;
using SCERP.DAL;
using SCERP.DAL.IRepository.IHRMRepository;
using SCERP.DAL.Repository.HRMRepository;
using SCERP.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SCERP.BLL.Manager.HRMManager
{
    public class AttendanceBonusSettingManager : BaseManager, IAttendanceBonusSettingManager
    {

        private readonly IAttendanceBonusSettingRepository _attendanceBonusSettingRepository = null;

        public AttendanceBonusSettingManager(SCERPDBContext context)
        {
            _attendanceBonusSettingRepository = new AttendanceBonusSettingRepository(context);
        }

        public List<AttendanceBonusSetting> GetAllAttendanceBonusSettingsByPaging(int startPage, int pageSize, out int totalRecords, AttendanceBonusSetting attendanceBonusSetting)
        {
            List<AttendanceBonusSetting> attendanceBonusSettings = null;
            try
            {
                attendanceBonusSettings = _attendanceBonusSettingRepository.GetAllAttendanceBonusSettingsByPaging(startPage, pageSize, out totalRecords, attendanceBonusSetting).ToList();
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                totalRecords = 0;
            }

            return attendanceBonusSettings;
        }

        public List<AttendanceBonusSetting> GetAllAttendanceBonusSettings()
        {
            List<AttendanceBonusSetting> attendanceBonusSetting = null;

            try
            {
                attendanceBonusSetting = _attendanceBonusSettingRepository.Filter(x => x.IsActive).OrderBy(x => x.FromDate).ToList();
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                attendanceBonusSetting = null;
            }

            return attendanceBonusSetting;
        }

        public AttendanceBonusSetting GetAttendanceBonusSettingById(int? id)
        {
            AttendanceBonusSetting attendanceBonusSetting = null;
            try
            {
                attendanceBonusSetting = _attendanceBonusSettingRepository.GetAttendanceBonusSettingById(id);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                attendanceBonusSetting = null;
            }

            return attendanceBonusSetting;
        }

        public int SaveAttendanceBonusSetting(AttendanceBonusSetting attendanceBonusSetting)
        {
            var savedAttendanceBonusSetting = 0;
            try
            {
                attendanceBonusSetting.CDT = DateTime.Now;
                attendanceBonusSetting.CreatedBy = PortalContext.CurrentUser.UserId;
                attendanceBonusSetting.IsActive = true;
                savedAttendanceBonusSetting = _attendanceBonusSettingRepository.Save(attendanceBonusSetting);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return savedAttendanceBonusSetting;
        }

        public int EditAttendanceBonusSetting(AttendanceBonusSetting attendanceBonusSetting)
        {
            var editedAttendanceBonusSetting = 0;
            try
            {
                attendanceBonusSetting.EDT = DateTime.Now;
                attendanceBonusSetting.EditedBy = PortalContext.CurrentUser.UserId;
                editedAttendanceBonusSetting = _attendanceBonusSettingRepository.Edit(attendanceBonusSetting);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return editedAttendanceBonusSetting;
        }

        public int DeleteAttendanceBonusSetting(AttendanceBonusSetting attendanceBonusSetting)
        {
            var deletedAttendanceBonusSetting = 0;
            try
            {
                attendanceBonusSetting.EDT = DateTime.Now;
                attendanceBonusSetting.EditedBy = PortalContext.CurrentUser.UserId;
                attendanceBonusSetting.IsActive = false;
                deletedAttendanceBonusSetting = _attendanceBonusSettingRepository.Edit(attendanceBonusSetting);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return deletedAttendanceBonusSetting;
        }
    }
}
