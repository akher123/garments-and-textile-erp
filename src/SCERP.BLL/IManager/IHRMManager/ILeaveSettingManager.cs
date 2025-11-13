using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;
using SCERP.Model.Custom;


namespace SCERP.BLL.IManager.IHRMManager
{
    public interface ILeaveSettingManager
    {
        List<LeaveSetting> GetAllLeaveSettings(int startPage, int pageSize, LeaveSetting model,
            SearchFieldModel searchFieldModel, out int totalRecords);

        LeaveSetting GetLeaveSettingById(int? id);

        int SaveLeaveSetting(LeaveSetting aLeaveSetting);

        int DeleteLeaveSetting(int id);

        List<LeaveType> GetAllLeaveType();

        List<EmployeeType> GetAllEmployeeType();
        bool IsExistLeaveSetting(LeaveSetting model);
        List<LeaveSetting> GetAllLeaveSettingsBySearchKey(LeaveSetting leaveSetting, SearchFieldModel searchFieldModel);

    }
}
