using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Common;
using SCERP.Model.Production;

namespace SCERP.DAL.IRepository.IProductionRepository
{
    public interface IDyeingJobOrderRepository:IRepository<PROD_DyeingJobOrder>
    {
        List<VwDyeingJobOrderDetail> GetDyeingJobOrderDetails(long dyeingJobOrderId);
        List<Dropdown> GetDyeingJobOrderByPartyId(long partyId);
        List<Dropdown> GetKnittingRollIssueChallan(string orderStyleRefId);
    }

}
