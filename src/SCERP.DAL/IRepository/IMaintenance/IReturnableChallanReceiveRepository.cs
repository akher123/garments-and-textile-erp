using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.Maintenance;

namespace SCERP.DAL.IRepository.IMaintenance
{
    public interface IReturnableChallanReceiveRepository : IRepository<Maintenance_ReturnableChallanReceive>
    {
        List<VwReturnableChallanReceive> GetAllReturnableChallanReceiveByReturnableChallanId(long returnableChallanId, string compId);
        List<VwReceiveDetail> GetChallanReceiveByDetailId(long returnableChallanDetailId, string compId);
        List<VwChallanReceiveMaster> GetReturnableChallanReceiveByReturnableChallanReceiveMasterId(long returnableChallanReceiveMasterId, string compId); 
    }
}
