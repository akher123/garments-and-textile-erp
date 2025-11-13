using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;
using System.Linq;

namespace SCERP.BLL.IManager.IHRMManager
{
    public interface IAttendanceBonusSettingManager
    {
        List<AttendanceBonusSetting> GetAllAttendanceBonusSettingsByPaging(int startPage, int pageSize, out int totalRecords, AttendanceBonusSetting attendanceBonusSetting);

        List<AttendanceBonusSetting> GetAllAttendanceBonusSettings();

        AttendanceBonusSetting GetAttendanceBonusSettingById(int? id);

        int SaveAttendanceBonusSetting(AttendanceBonusSetting attendanceBonusSetting);

        int EditAttendanceBonusSetting(AttendanceBonusSetting attendanceBonusSetting);

        int DeleteAttendanceBonusSetting(AttendanceBonusSetting attendanceBonusSetting);

    }
}
