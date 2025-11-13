using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.InventoryModel;

namespace SCERP.BLL.IManager.IInventoryManager
{
   public interface IStockRegisterManager
    {
       List<VwStockPosition> GetStockPostion(DateTime? fromDate, DateTime? toDate, int groupId, int subGroupId);
       object GetYarnStockStatus(int itemId, string colorRefId);
       List<VwStockPosition> GetStockPostionDetail(DateTime? fromDate, DateTime? toDate, int groupId, int subGroupId);
       List<VwStockPosition> GetDyedYarnStockPostionDetail(DateTime? modelFromDate, DateTime? modelToDate, int groupId, int subGroupId);
        bool IsYarnCountValid(int itemId,string fColorRefId, string colorRefId,string sizeRefId);
       List<VwStockPosition> GetBuyerWiseStockPostionDetail(DateTime? fromDate, DateTime? toDate);
       object GetYarnStockStatusByLot(string colorRefId);
    }
}
