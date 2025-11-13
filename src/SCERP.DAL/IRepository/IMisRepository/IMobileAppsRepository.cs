using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.DAL.IRepository.IMisRepository
{
    public interface  IMobileAppsRepository
    {
        DataTable GetMonthlyShipmentSummary(string compId, DateTime? fromDate, DateTime? toDate);

        DataTable GetSpMISReprotDashBoard(string compId);

    }
}
