using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Common;
using SCERP.DAL.IRepository.IProductionRepository;
using SCERP.Model.Production;

namespace SCERP.DAL.Repository.ProductionRepository
{
    public class DyeingSpChallanDetailRepository : Repository<PROD_DyeingSpChallanDetail>, IDyeingSpChallanDetailRepository
    {
        public DyeingSpChallanDetailRepository(SCERPDBContext context) : base(context)
        {
        }

        public List<VwProdDyeingSpChallanDetail> GetDyeingSpChallanDetailByDyeingSpChallanId(long dyeingSpChallanId)
        {
            return Context.VDyeingSpChallanDetail.Where(x => x.DyeingSpChallanId == dyeingSpChallanId).ToList();
        }

        public IEnumerable<dynamic> GetBatchItemQtyByBatchDetailId(long batchDetailId, string compId)
        {
            //string sqlQurry =String.Format(@"select (Quantity-ISNULL((select SUM(GreyWeight) from PROD_DyeingSpChallanDetail where BatchDetailId=BD.BatchDetailId),0)) as Quantity from PROD_BatchDetail as BD  where BD.BatchDetailId='{0}' and BD.CompId='{1}'",batchDetailId,compId);
           // return  Context.Database.SqlQuery<double>(sqlQurry).FirstOrDefault();
            string sqlQurry = String.Format(@"select 
          (Quantity-ISNULL((select SUM(GreyWeight) from PROD_DyeingSpChallanDetail where BatchDetailId=BD.BatchDetailId),0)) as BalanceWt,
          (ISNULL(FLength,0)-ISNULL((select SUM(CcuffQty) from PROD_DyeingSpChallanDetail where BatchDetailId=BD.BatchDetailId),0)) as CcuffQty 
          from PROD_BatchDetail as BD  where BD.BatchDetailId='{0}' and BD.CompId='{1}'", batchDetailId, compId);
           // return Context.Database.SqlQuery<double>(sqlQurry).FirstOrDefault();
             DataTable dataTable=  base.ExecuteQuery(sqlQurry);
            return dataTable.Todynamic();

        }
    }
}
