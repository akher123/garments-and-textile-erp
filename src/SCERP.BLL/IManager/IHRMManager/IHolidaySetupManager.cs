using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;

namespace SCERP.BLL.IManager.IHRMManager
{
    public  interface IHolidaySetupManager
    {
        int SaveHolidaye(HolidaysSetup holidaysSetup);

        List<HolidaysSetup> GetAllHolidays();

        HolidaysSetup GetHolidayById(int id);

        int EditHoliday(HolidaysSetup holidaysSetupObj);

        int DeleteHoliday(int id);

        List<Weekend> GetAllWeekends();

        int UpdateWeekends(List<int> weekends);

        List<Weekend> GetAllActiveWeekends();

        HolidaysSetup GetHolidayDetails(int id);
    }
}
