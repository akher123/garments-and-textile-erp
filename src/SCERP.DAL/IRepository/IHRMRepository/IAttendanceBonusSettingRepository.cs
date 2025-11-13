using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;

namespace SCERP.DAL.IRepository.IHRMRepository
{
    public interface IAttendanceBonusSettingRepository : IRepository<AttendanceBonusSetting>
    {
        AttendanceBonusSetting GetAttendanceBonusSettingById(int? id);
        List<AttendanceBonusSetting> GetAllAttendanceBonusSettings();
        List<AttendanceBonusSetting> GetAllAttendanceBonusSettingsByPaging(int startPage, int pageSize, out int totalRecords, AttendanceBonusSetting attendanceBonusSetting);
        //List<AttendanceBonusSetting> GetAttendanceBonusSettingBySearchKey(int searchByCountry, string searchByAttendanceBonusSetting);
    }
}
