using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;
using SCERP.Model.Custom;


namespace SCERP.DAL.IRepository.IHRMRepository
{
    public interface ILeaveSettingRepository : IRepository<LeaveSetting>
    {
        LeaveSetting GetLeaveSettingById(int? id);

        List<LeaveSetting> GetAllLeaveSettings(int startPage, int pageSize, LeaveSetting model,
            SearchFieldModel searchFieldModel, out int totalRecords);
        List<LeaveType> GetAllLeaveType();
        List<EmployeeType> GetAllEmployeeType();
        List<LeaveSetting> GetAllLeaveSettingsBySearchKey(LeaveSetting leaveSetting, SearchFieldModel searchFieldModel);
    }
}
