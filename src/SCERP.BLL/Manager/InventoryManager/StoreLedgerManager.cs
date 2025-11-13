using System;
using System.Linq;
using SCERP.BLL.IManager.IInventoryManager;
using SCERP.Common;
using SCERP.DAL;
using SCERP.DAL.IRepository.IInventoryRepository;
using SCERP.DAL.Repository.InventoryRepository;
namespace SCERP.BLL.Manager.InventoryManager
{
    public class StoreLedgerManager : IStoreLedgerManager
    {
        private readonly IStoreLedgerRepository _storeLedgerRepository;

        public StoreLedgerManager(SCERPDBContext context)
        {
            _storeLedgerRepository = new StoreLedgerRepository(context);
        }

        public object StockStatys(int? itemId)
        {
            var receive = Convert.ToString((int)StoreLedgerTransactionType.Receive);
            var issue = Convert.ToString((int)StoreLedgerTransactionType.Issue);
            var totalRecived = _storeLedgerRepository.Filter(x => x.ItemId == itemId && x.TransactionType == receive&&x.IsActive).Sum(x => (decimal?)x.Quantity) ?? 0.0M;
            var totalIssued = _storeLedgerRepository.Filter(x => x.ItemId == itemId && x.TransactionType == issue && x.IsActive).Sum(x => (decimal?)x.Quantity) ?? 0.0M;
            var stockInHand = totalRecived - totalIssued;
            var receivedItems = _storeLedgerRepository.Filter(x => x.IsActive && x.ItemId == itemId && x.TransactionType == receive).ToList();
            var rate = receivedItems.Sum(x => x.UnitPrice) / (!receivedItems.Any() ? 1 : receivedItems.Count());
            return new { StockInHand = stockInHand, Rate = rate };
        }

        public object SizeWizeStockInHand(int? itemId, int? sizeId)
        {
            var receive = Convert.ToString((int)StoreLedgerTransactionType.Receive);
            var issue = Convert.ToString((int)StoreLedgerTransactionType.Issue);

            var totalRecived = _storeLedgerRepository
                .Filter(x => x.ItemId == itemId && (x.SizeId == sizeId||sizeId==null) && x.TransactionType == receive)
                 .Sum(x => (decimal?)x.Quantity) ?? 0.0M;
            var totalIssued =
              _storeLedgerRepository.Filter(x => x.ItemId == itemId && (x.SizeId == sizeId||sizeId==null)&& x.TransactionType == issue)
                  .Sum(x => (decimal?)x.Quantity) ?? 0.0M;

            var stockInHand = totalRecived - totalIssued;
            var receivedItems = _storeLedgerRepository.Filter(x => x.IsActive && x.ItemId == itemId && x.TransactionType == receive).ToList();
            var rate = receivedItems.Sum(x => x.UnitPrice) / (!receivedItems.Any() ? 1 : receivedItems.Count());
            return new { StockInHand = stockInHand, Rate = rate };
        }

        public object BrandWizeStockInHand(int? itemId, int? sizeId, int? brandId)
        {
            var receive = Convert.ToString((int)StoreLedgerTransactionType.Receive);
            var issue = Convert.ToString((int)StoreLedgerTransactionType.Issue);
            var totalRecived = _storeLedgerRepository
               .Filter(x => x.ItemId == itemId && (x.SizeId == sizeId || sizeId == null) && (x.BrandId == brandId || brandId == null) && x.TransactionType == receive)
                .Sum(x => (decimal?)x.Quantity) ?? 0.0M;
            var totalIssued =
              _storeLedgerRepository.Filter(x => x.ItemId == itemId && (x.SizeId == sizeId || sizeId == null) && (x.BrandId == brandId || brandId == null) && x.TransactionType == issue)
                  .Sum(x => (decimal?)x.Quantity) ?? 0.0M;
            var stockInHand = totalRecived - totalIssued;
            var receivedItems = _storeLedgerRepository.Filter(x => x.IsActive && x.ItemId == itemId && x.TransactionType == receive).ToList();
            var rate = receivedItems.Sum(x => x.UnitPrice) / (!receivedItems.Any() ? 1 : receivedItems.Count());
            return new { StockInHand = stockInHand, Rate = rate };
        }

        public object OriginWizeStockInHand(int? itemId, int? sizeId, int? brandId, int? originId)
        {
            var receive = Convert.ToString((int)StoreLedgerTransactionType.Receive);
            var issue = Convert.ToString((int)StoreLedgerTransactionType.Issue);
            var totalRecived = _storeLedgerRepository
             .Filter(x => x.ItemId == itemId && (x.SizeId == sizeId ||sizeId==null) && (x.BrandId == brandId||brandId==null) && (x.OriginId == originId||originId==null)&& x.TransactionType == receive)
              .Sum(x => (decimal?)x.Quantity) ?? 0.0M;
            var totalIssued =
              _storeLedgerRepository.Filter(x => x.ItemId == itemId && (x.SizeId == sizeId || sizeId == null) && (x.BrandId == brandId || brandId == null) && (x.OriginId == originId || originId == null) && x.TransactionType == issue)
                  .Sum(x => (decimal?)x.Quantity) ?? 0.0M;
            var stockInHand = totalRecived - totalIssued;
            var receivedItems = _storeLedgerRepository.Filter(x => x.IsActive && x.ItemId == itemId && x.TransactionType == receive).ToList();
            var rate = receivedItems.Sum(x => x.UnitPrice) / (!receivedItems.Any() ? 1 : receivedItems.Count());
            return new { StockInHand = stockInHand, Rate = rate };
        }

        public object PresentStockInfo(int? itemId, int? sizeId, int? brandId, int? originId)
        {
            var receive = Convert.ToString((int)StoreLedgerTransactionType.Receive);
            var issue = Convert.ToString((int)StoreLedgerTransactionType.Issue);
     
            var totalRecived = _storeLedgerRepository
                .Filter(x => x.ItemId == itemId && x.TransactionType == receive && (x.SizeId == sizeId || sizeId == null) && (x.BrandId == brandId || brandId == null) && (x.OriginId == originId || originId == null))
                 .Sum(x => (decimal?)x.Quantity) ?? 0.0M;
            var totalIssued =
              _storeLedgerRepository.Filter(x => x.ItemId == itemId && x.TransactionType == issue && (x.SizeId == sizeId || sizeId == null) && (x.BrandId == brandId || brandId == null) && (x.OriginId == originId || originId == null))
                  .Sum(x => (decimal?)x.Quantity) ?? 0.0M;

            var stockInHand = totalRecived - totalIssued;
            var inventoryStoreLedger = _storeLedgerRepository
                .Filter(x => x.ItemId == itemId && x.TransactionType == receive && (x.SizeId == sizeId || sizeId == null) && (x.BrandId == brandId || brandId == null) && (x.OriginId == originId || originId == null) ).OrderByDescending(x => x.StoreLedgerId).FirstOrDefault();
            var lastUnitPrice = 0.0M;
            if (inventoryStoreLedger != null)
            {
                lastUnitPrice = inventoryStoreLedger.UnitPrice;
            }
            return new
            {
                SuppliedUptoDate = totalRecived,
                StockInHand = stockInHand,
                LastUnitPrice = lastUnitPrice
            };

        }

        public DateTime? GetTransactionDateByGrnId(int goodsReceivingNotesId)
        {
         
           var grn= _storeLedgerRepository.FindOne(x => x.GoodsReceivingNoteId == goodsReceivingNotesId);
            if (grn!=null)
            {
                return grn.TransactionDate;
            }
            else
            {
                return null;
            }
        }

     
    }
}
