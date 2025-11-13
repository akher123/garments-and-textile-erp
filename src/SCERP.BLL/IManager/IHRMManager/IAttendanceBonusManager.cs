using System.Collections;
using System.Collections.Generic;
using SCERP.Model;
using SCERP.Model.Custom;

namespace SCERP.BLL.IManager.IHRMManager
{
    public interface IAttendanceBonusManager
    {
        List<AttendanceBonus> GetAttendanceBonusByPaging(int startPage, int pageSize, out int totalRecords, SearchFieldModel searchFieldModel, AttendanceBonus model);
        int EditAttendanceBonus(AttendanceBonus model);
        int SaveAttendanceBonus(AttendanceBonus model);
        int DeleteAttendanceBonus(int attendanceBonusId);
        AttendanceBonus GetAttendanceBonusById(int attendanceBonusId);

        List<AttendanceBonus> GetAttendanceBonusBySearchKey(int searchByEmployeeTypeId,
            int? searchByEmployeeDesignationId);
    }
}
