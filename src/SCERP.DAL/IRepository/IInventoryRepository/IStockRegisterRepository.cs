using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.InventoryModel;

namespace SCERP.DAL.IRepository.IInventoryRepository
{
   public interface IStockRegisterRepository:IRepository<Inventory_StockRegister>
   {
       List<VwStockPosition> GetStockPostion(DateTime? fromDate, DateTime? toDate, int groupId, int subGroupId);
       List<VwStockPosition> GetStockPostionDetail(DateTime? fromDate, DateTime? toDate, int groupId, int subGroupId);
       List<VwStockPosition> GetDyedYarnStockPostionDetail(DateTime? fromDate, DateTime? toDate, int groupId, int subGroupId);
       List<VwStockPosition> GetBuyerWiseStockPostionDetail(DateTime? fromDate, DateTime? toDate);
   }
}
