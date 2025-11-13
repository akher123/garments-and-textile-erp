using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.BLL.IManager.IInventoryManager
{
   public interface IStoreLedgerManager
    {
       object StockStatys(int? itemId);
       object SizeWizeStockInHand(int? itemId, int? sizeId);
       object BrandWizeStockInHand(int? itemId, int? sizeId, int? brandId);
       object OriginWizeStockInHand(int? itemId, int? sizeId, int? brandId, int? originId);

       object PresentStockInfo(int? itemId, int? sizeId, int? brandId, int? originId);
       DateTime? GetTransactionDateByGrnId(int goodsReceivingNotesId);
  
    }
}
