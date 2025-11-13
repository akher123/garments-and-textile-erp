using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.InventoryModel;

namespace SCERP.DAL.IRepository.IInventoryRepository
{
   public interface IFinishFabStoreRepository:IRepository<Inventory_FinishFabStore>
   {
       List<SpInvFinishFabStore> GetDetailChallanBy(long dyeingSpChallanId, long finishFabStoreId);
       IEnumerable GetBatchDeailsById(long batchId);
       object GetFinishFabIssueDetail(long batchDetailId);
   }
}
