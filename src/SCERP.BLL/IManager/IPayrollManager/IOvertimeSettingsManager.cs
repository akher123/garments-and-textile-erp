using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;

namespace SCERP.BLL.IManager.IPayrollManager
{
    public interface IOvertimeSettingsManager
    {
        List<OvertimeSettings> GetAllOvertimeSettings(int startPage, int pageSize, out int totalRecords, OvertimeSettings model);

        OvertimeSettings GetAllOvertimeById(int p);

        int EditOvertime(OvertimeSettings overtime);

        int SaveOvertime(OvertimeSettings overtime);

        int DeleteOvertimeSettings(int id);

        bool IsExistOvertimeSettings(OvertimeSettings overtime);

        OvertimeSettings GetLatestOvertimeSettingInfo();

        int UpdateLatestOvertimeSettingInfoDate(OvertimeSettings overtimeSettings);
    }
}
