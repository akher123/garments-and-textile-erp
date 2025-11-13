using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.DAL.IRepository.IInventoryRepository;
using SCERP.Model.InventoryModel;

namespace SCERP.DAL.Repository.InventoryRepository
{
    public class ReDyeingFabricReceiveRepository :Repository<Inventory_ReDyeingFabricReceive>, IReDyeingFabricReceiveRepository
    {
        public ReDyeingFabricReceiveRepository(SCERPDBContext context) : base(context)
        {
        }

        public List<VwReDyeingFabricReceiveDetail> GetVwReDyeingFabricReceiveDetailById(long reDyeingFabricReceiveId)
        {
            string sqlQuery=String.Format(@"select FRD.*,(select ItemName from VwProdBatchDetail where BatchDetailId=FRD.BatchDetailId) as ItemName,
                            (select BatchNo from Pro_Batch where BatchId=FRD.BatchId) as BatchNo
                                         from Inventory_ReDyeingFabricReceiveDetail as FRD
                                         where FRD.ReDyeingFabricReceiveId='{0}'",reDyeingFabricReceiveId);
           return Context.Database.SqlQuery<VwReDyeingFabricReceiveDetail>(sqlQuery).ToList();
        }
    }
}
