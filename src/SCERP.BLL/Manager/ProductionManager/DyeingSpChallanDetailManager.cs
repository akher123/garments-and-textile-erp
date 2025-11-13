using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.BLL.IManager.IProductionManager;
using SCERP.DAL.IRepository.IProductionRepository;
using SCERP.Model.Production;

namespace SCERP.BLL.Manager.ProductionManager
{
   public class DyeingSpChallanDetailManager: IDyeingSpChallanDetailManager
   {
       private readonly IDyeingSpChallanDetailRepository _dyeingSpChallanDetailRepository;

       public DyeingSpChallanDetailManager(IDyeingSpChallanDetailRepository dyeingSpChallanDetailRepository)
       {
           _dyeingSpChallanDetailRepository = dyeingSpChallanDetailRepository;
       }

       public List<VwProdDyeingSpChallanDetail> GetDyeingSpChallanDetailByDyeingSpChallanId(long dyeingSpChallanId)
       {
           return _dyeingSpChallanDetailRepository.GetDyeingSpChallanDetailByDyeingSpChallanId(dyeingSpChallanId);
       }

       public PROD_DyeingSpChallanDetail GetAnDyeingSpChallanDetailById(long dyeingSpChallanId)
       {
           return _dyeingSpChallanDetailRepository.FindOne(x => x.DyeingSpChallanId == dyeingSpChallanId);
       }
   }
}
