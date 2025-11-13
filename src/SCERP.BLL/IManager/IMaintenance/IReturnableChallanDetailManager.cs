using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.Maintenance;

namespace SCERP.BLL.IManager.IMaintenance
{
   public interface IReturnableChallanDetailManager  
    {
       List<Maintenance_ReturnableChallanDetail> GetReturnableChallanDetailByReturnableChallanId(long returnableChallanId, string compId);
       Maintenance_ReturnableChallanDetail GetDetailByReturnableChallanDetailId(long returnableChallanDetailId, string compId);
    }
}
