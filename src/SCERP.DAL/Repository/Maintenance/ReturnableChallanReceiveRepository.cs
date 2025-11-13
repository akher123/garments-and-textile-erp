using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.DAL.IRepository.IMaintenance;
using SCERP.Model.Maintenance;

namespace SCERP.DAL.Repository.Maintenance
{
    public class ReturnableChallanReceiveRepository : Repository<Maintenance_ReturnableChallanReceive>, IReturnableChallanReceiveRepository
   {
        public ReturnableChallanReceiveRepository(SCERPDBContext context) : base(context)
        {
        }


        public List<VwReturnableChallanReceive> GetAllReturnableChallanReceiveByReturnableChallanId(long returnableChallanId, string compId)
        {
            return
                Context.VwReturnableChallanReceives.Where(
                    x => x.CompId == compId && x.ReturnableChallanId == returnableChallanId).ToList();
        }

        public List<VwReceiveDetail> GetChallanReceiveByDetailId(long returnableChallanDetailId, string compId)
        {
            return Context.VwReceiveDetails.Where(x => x.CompId == compId && x.ReturnableChallanDetailId == returnableChallanDetailId).ToList();
        }

        public List<VwChallanReceiveMaster> GetReturnableChallanReceiveByReturnableChallanReceiveMasterId(long returnableChallanReceiveMasterId, string compId)
        {
            return
                Context.VwChallanReceiveMasters.Where(x => x.CompId == compId && x.ReturnableChallanReceiveMasterId == returnableChallanReceiveMasterId)
                    .ToList();
        }
   }
}
