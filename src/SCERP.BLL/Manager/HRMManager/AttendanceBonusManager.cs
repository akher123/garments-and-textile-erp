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
    public class AttendanceBonusManager : IAttendanceBonusManager
    {

        private readonly IAttendanceBonusRepository _attendanceBonusRepository;

        public AttendanceBonusManager(SCERPDBContext context)
        {
            _attendanceBonusRepository = new AttendanceBonusRepository(context);
        }

        public List<AttendanceBonus> GetAttendanceBonusByPaging(int startPage, int pageSize, out int totalRecords, SearchFieldModel searchFieldModel, AttendanceBonus model)
        {
            return _attendanceBonusRepository.GetAttendanceBonusByPaging(startPage, pageSize, out totalRecords, searchFieldModel, model);
        }

        public int EditAttendanceBonus(AttendanceBonus model)
        {
            var editIndex = 0;
            try
            {
                var attendanceBonusObj = _attendanceBonusRepository.FindOne(x => x.IsActive && x.AttendanceBonusId == model.AttendanceBonusId);               
                attendanceBonusObj.DesignationId = model.DesignationId;
                attendanceBonusObj.Amount = model.Amount;
                attendanceBonusObj.FromDate = model.FromDate;
                attendanceBonusObj.ToDate = model.ToDate;
                attendanceBonusObj.EditedDate = DateTime.Now;
                attendanceBonusObj.EditedBy = PortalContext.CurrentUser.UserId;
                editIndex = _attendanceBonusRepository.Edit(attendanceBonusObj);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
            return editIndex;
        }

        public int SaveAttendanceBonus(AttendanceBonus model)
        {
            var saveIndex = 0;
            try
            {
                model.CreatedBy = PortalContext.CurrentUser.UserId;
                model.CreatedDate = DateTime.Now;
                model.IsActive = true;
                saveIndex = _attendanceBonusRepository.Save(model);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
            return saveIndex;
        }

        public int DeleteAttendanceBonus(int attendanceBonusId)
        {
            var deleteIndex = 0;
            try
            {
                var attendancetObj = _attendanceBonusRepository.FindOne(x => x.IsActive && x.AttendanceBonusId == attendanceBonusId);
                attendancetObj.IsActive = false;
                attendancetObj.EditedDate = DateTime.Now;
                attendancetObj.EditedBy = PortalContext.CurrentUser.UserId;
                deleteIndex = _attendanceBonusRepository.Edit(attendancetObj);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
            return deleteIndex;
        }

        public List<AttendanceBonus> GetAttendanceBonusBySearchKey(int searchByEmployeeTypeId, int? searchByEmployeeDesignationId)
        {
            List<AttendanceBonus> attendanceBonus;
            try
            {
                attendanceBonus = _attendanceBonusRepository.GetAttendanceBonusesBySearchKey(searchByEmployeeTypeId, searchByEmployeeDesignationId);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
            return attendanceBonus;
        }

        public AttendanceBonus GetAttendanceBonusById(int attendanceBonusId)
        {
            AttendanceBonus attendanceBonus;

            try
            {
                attendanceBonus = _attendanceBonusRepository.GetAttendanceBonusById(attendanceBonusId);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
            return attendanceBonus;
        }
    }
}
