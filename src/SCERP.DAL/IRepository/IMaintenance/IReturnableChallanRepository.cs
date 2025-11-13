using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.Maintenance;

namespace SCERP.DAL.IRepository.IMaintenance
{
   public interface IReturnableChallanRepository:IRepository<Maintenance_ReturnableChallan>
    {
       List<VwReturnableChallan> GetReturnableChallanForReport(long returnableChallanId, string compId);
       DataTable GetReturnableChallanInfo(DateTime? dateFrom, DateTime? dateTo,string challanType, int challanStatus, string compId);
    }
}
