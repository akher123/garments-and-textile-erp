using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;
using SCERP.Model.Custom;

namespace SCERP.DAL.IRepository.IHRMRepository
{
    public interface IAttendanceBonusRepository : IRepository<AttendanceBonus>
    {
        List<AttendanceBonus> GetAttendanceBonusByPaging(int startPage, int pageSize, out int totalRecords, SearchFieldModel searchFieldModel, AttendanceBonus model);
        List<AttendanceBonus> GetAttendanceBonusesBySearchKey(int searchByEmployeeTypeId, int? searchByEmployeeDesignationId);
        AttendanceBonus GetAttendanceBonusById(int attendanceBonusId);
    }
}
