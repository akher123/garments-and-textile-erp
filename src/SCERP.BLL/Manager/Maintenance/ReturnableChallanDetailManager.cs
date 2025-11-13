using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.BLL.IManager.IMaintenance;
using SCERP.DAL.IRepository.IMaintenance;
using SCERP.Model.Maintenance;

namespace SCERP.BLL.Manager.Maintenance
{
    public class ReturnableChallanDetailManager : IReturnableChallanDetailManager 
   {
       private readonly IReturnableChallanDetailRepository _returnableChallanDetailRepository;

       public ReturnableChallanDetailManager(IReturnableChallanDetailRepository returnableChallanDetailRepository)
       {
           _returnableChallanDetailRepository = returnableChallanDetailRepository;
       }

        public List<Maintenance_ReturnableChallanDetail> GetReturnableChallanDetailByReturnableChallanId(long returnableChallanId, string compId)
        {
            return
                _returnableChallanDetailRepository.Filter(
                    x => x.CompId == compId && x.ReturnableChallanId == returnableChallanId).ToList();
        }

        public Maintenance_ReturnableChallanDetail GetDetailByReturnableChallanDetailId(long returnableChallanDetailId, string compId)
        {
            return _returnableChallanDetailRepository.FindOne(x => x.CompId == compId && x.ReturnableChallanDetailId == returnableChallanDetailId);
        }
   }
}
