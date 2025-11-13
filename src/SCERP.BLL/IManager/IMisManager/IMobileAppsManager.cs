using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.BLL.IManager.IMisManager
{
    public interface IMobileAppsManager
    {
        DataTable GetMonthlyShipmentSummary(DateTime? fromDate, DateTime? toDate);

        DataTable GetSpMISReprotDashBoard();
    }
}
