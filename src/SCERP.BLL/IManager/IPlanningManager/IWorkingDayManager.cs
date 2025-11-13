using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.Planning;

namespace SCERP.BLL.IManager.IPlanningManager
{
    public interface IWorkingDayManager
    {
        List<PLAN_WorkingDay> GetWorkingDay(int workingDayStatus,DateTime? fromDate, DateTime? toDate, int index,out int  totalRecords);
        int CreateWorkingDays(int cureentYar);

        PLAN_WorkingDay GetWorkingDayById(long workingDayId);
        int EditWorkingDays(PLAN_WorkingDay workingDay);
    }
}
