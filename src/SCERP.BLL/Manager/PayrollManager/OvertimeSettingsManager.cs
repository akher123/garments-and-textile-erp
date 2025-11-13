using System;
using System.Collections.Generic;
using System.Transactions;
using SCERP.BLL.IManager.IPayrollManager;
using SCERP.Common;
using SCERP.DAL;
using SCERP.DAL.IRepository.IPayrollRepository;
using SCERP.DAL.Repository.PayrollRepository;
using SCERP.Model;


namespace SCERP.BLL.Manager.PayrollManager
{
    public class OvertimeSettingsManager : BaseManager, IOvertimeSettingsManager
    {
        private readonly IOvertimeSettingsRepository _overtimeSettingsRepository;

        public OvertimeSettingsManager(SCERPDBContext context)
        {
            _overtimeSettingsRepository = new OvertimeSettingsRepository(context);
        }

        public List<OvertimeSettings> GetAllOvertimeSettings(int startPage, int pageSize, out int totalRecords, OvertimeSettings model)
        {
            totalRecords = 0;
            List<OvertimeSettings> overtimeSettings = null;
            try
            {

                overtimeSettings = _overtimeSettingsRepository.GetAllOvertimeSettings(startPage, pageSize, out totalRecords, model);

            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);

            }
            return overtimeSettings;
        }

        public OvertimeSettings GetAllOvertimeById(int id)
        {
            OvertimeSettings overtimeSettings = null;

            try
            {
                overtimeSettings = _overtimeSettingsRepository.FindOne(x => x.Id == id && x.IsActive);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return overtimeSettings;
        }

        public int EditOvertime(OvertimeSettings overtime)
        {
            var saveIndex = 0;
            try
            {
                var overtimeSetting = _overtimeSettingsRepository.FindOne(x => x.Id == overtime.Id && x.IsActive);
                overtimeSetting.OvertimeHours = overtime.OvertimeHours;
                overtimeSetting.OvertimeRate = overtime.OvertimeRate;
                overtimeSetting.FromDate = overtime.FromDate;
                overtimeSetting.ToDate = overtime.ToDate;
                overtimeSetting.EditedBy = PortalContext.CurrentUser.UserId;
                overtimeSetting.EditedDate = DateTime.Now;
                saveIndex = _overtimeSettingsRepository.Edit(overtimeSetting);

            }
            catch (Exception exception)
            {
                throw;
            }
            return saveIndex;
        }

        public int SaveOvertime(OvertimeSettings overtime)
        {
            var saveIndex = 0;
            try
            {
                var overtimeSetting = new OvertimeSettings
                {
                    OvertimeHours = overtime.OvertimeHours,
                    OvertimeRate = overtime.OvertimeRate,
                    FromDate = overtime.FromDate,
                    CreatedBy = PortalContext.CurrentUser.UserId,
                    CreatedDate = DateTime.Now,
                    IsActive = true
                };
                saveIndex = _overtimeSettingsRepository.Save(overtimeSetting);

            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);


            }
            return saveIndex;
        }


        public int DeleteOvertimeSettings(int id)
        {
            var status = 0;
            try
            {
                OvertimeSettings overtime = GetAllOvertimeById(id);
                overtime.IsActive = false;
                status = _overtimeSettingsRepository.Edit(overtime);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                status = 0;

            }
            return status;
        }

        public bool IsExistOvertimeSettings(OvertimeSettings overtime)
        {
            bool isExist;
            try
            {
                isExist = _overtimeSettingsRepository.Exists(x => x.Id != overtime.Id 
                                                               && x.IsActive 
                                                               && x.OvertimeHours == overtime.OvertimeHours 
                                                               && x.OvertimeRate == overtime.OvertimeRate 
                                                               && x.FromDate <= overtime.FromDate);
            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message);
            }
            return isExist;
        }

        public OvertimeSettings GetLatestOvertimeSettingInfo()
        {
            OvertimeSettings overtimeSettings = null;
            try
            {
                overtimeSettings = _overtimeSettingsRepository.GetLatestOvertimeSettingInfo();
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message, exception.InnerException);
            }
            return overtimeSettings;
        }

        public int UpdateLatestOvertimeSettingInfoDate(OvertimeSettings overtimeSettings)
        {
            var updated = 0;
            try
            {
                overtimeSettings.EditedBy = PortalContext.CurrentUser.UserId;
                overtimeSettings.EditedDate = DateTime.Now;
                updated = _overtimeSettingsRepository.UpdateLatestOvertimeSettingInfoDate(overtimeSettings);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
            return updated;
        }
    }
}
