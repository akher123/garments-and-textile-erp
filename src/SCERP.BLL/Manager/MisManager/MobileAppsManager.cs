using SCERP.BLL.IManager.IMisManager;
using SCERP.DAL.IRepository.IMisRepository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.BLL.Manager.MisManager
{
    public class MobileAppsManager : IMobileAppsManager
    {
        private readonly IMobileAppsRepository _mobileAppsRepository;

        public MobileAppsManager(IMobileAppsRepository mobileAppsRepository)
        {
            _mobileAppsRepository = mobileAppsRepository;
        }
        public DataTable GetMonthlyShipmentSummary(DateTime? fromDate, DateTime? toDate)
        {
            return _mobileAppsRepository.GetMonthlyShipmentSummary("001", fromDate, toDate);
        }

        public DataTable GetSpMISReprotDashBoard()
        {
            DataTable dataTable = _mobileAppsRepository.GetSpMISReprotDashBoard("001");
            return dataTable;
        }
    }
}
