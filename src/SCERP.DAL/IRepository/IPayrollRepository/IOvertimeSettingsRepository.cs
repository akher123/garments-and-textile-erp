using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP;
using SCERP.Model;

namespace SCERP.DAL.IRepository.IPayrollRepository
{
    public interface IOvertimeSettingsRepository:IRepository<OvertimeSettings>
    {
    
        List<OvertimeSettings> GetAllOvertimeSettings(int startPage, int pageSize, out int totalRecords, OvertimeSettings model);

        OvertimeSettings GetLatestOvertimeSettingInfo();

        int UpdateLatestOvertimeSettingInfoDate(OvertimeSettings overtimeSettings);
    }
}
