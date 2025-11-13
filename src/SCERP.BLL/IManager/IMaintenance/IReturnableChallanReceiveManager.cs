using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.Maintenance;

namespace SCERP.BLL.IManager.IMaintenance
{
   public interface IReturnableChallanReceiveManager 
    {
       List<VwReturnableChallanReceive> GetAllReturnableChallanReceiveByReturnableChallanId(long returnableChallanId, string compId);
       List<VwReceiveDetail> GetChallanReceiveByDetailId(long returnableChallanDetailId, string compId);
       int EditReturnableChallanRecieve(Maintenance_ReturnableChallanReceive model);
       int SaveReturnableChallanReceive(Maintenance_ReturnableChallanReceive model);
       Maintenance_ReturnableChallanReceive GetChallanReceiveByReturnableChallanReceiveId(long returnableChallanReceiveId);

       int DeleteReturnableChallanReceive(Maintenance_ReturnableChallanReceive model);
       List<VwChallanReceiveMaster> GetReturnableChallanReceiveByReturnableChallanReceiveMasterId(long returnableChallanReceiveMasterId, string compId); 
    }
}
