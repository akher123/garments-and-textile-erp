using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.Production;

namespace SCERP.BLL.IManager.IProductionManager
{
    public interface INonProductiveTimeManager
    {
        int EditNonProductiveTime(PROD_NonProductiveTime nonProductiveTime);
        int SaveNonProductiveTime(PROD_NonProductiveTime nonProductiveTime);
        int DeleteNpt(int nonProductiveTimeId);
        PROD_NonProductiveTime GetNptById(int nonProductiveTimeId);
        string GetNptRefId(string compId);
        List<VwNonProductiveTime> GetNpts(DateTime? fromDate, string compId);
        List<VwNonProductiveTime> GetDateWiseNpts(DateTime? fromDate, DateTime? toDate, string compId);
    }
}
