using System;
using System.Collections.Generic;
using System.Linq;
using SCERP.BLL.IManager.IInventoryManager;
using SCERP.Common;
using SCERP.DAL.IRepository;
using SCERP.DAL.IRepository.IInventoryRepository;
using SCERP.Model;
using SCERP.Model.InventoryModel;

namespace SCERP.BLL.Manager.InventoryManager
{
    public class StockRegisterManager : IStockRegisterManager
    {
        private readonly IStockRegisterRepository _stockRegisterRepository;
        private readonly IMaterialReceiveAgainstPoDetailRepository _againstPoDetail;
     
        public StockRegisterManager(IStockRegisterRepository stockRegisterRepository, IMaterialReceiveAgainstPoDetailRepository againstPoDetail)
        {
            _stockRegisterRepository = stockRegisterRepository;
            _againstPoDetail = againstPoDetail;
         
        }

        public List<VwStockPosition> GetStockPostion(DateTime? fromDate, DateTime? toDate, int groupId, int subGroupId)
        {
            return _stockRegisterRepository.GetStockPostion(fromDate, toDate, groupId, subGroupId);
        }

        public object GetYarnStockStatus(int itemId, string colorRefId)
        {
           
            var yarnReceivelist = _stockRegisterRepository.Filter(x => x.ItemId == itemId && x.ColorRefId == colorRefId && x.CompId == PortalContext.CurrentUser.CompId && x.StoreId == (int)StoreType.Yarn && (new[] { (int)ActionType.YarnReceive }).Contains(x.ActionType) && x.TransactionType == 1).ToList();
            var yarnIssueList = _stockRegisterRepository.Filter(x => x.ItemId == itemId && x.ColorRefId == colorRefId && x.CompId == PortalContext.CurrentUser.CompId && x.StoreId == (int)StoreType.Yarn && (x.ActionType == (int)ActionType.YarnIssue || x.ActionType == (int)ActionType.YarnDelivery || x.ActionType == (int)ActionType.YarnReturn) && x.TransactionType == 2).ToList();
            object status;
            if (yarnReceivelist.Any())
            {
                var sinh = yarnReceivelist.Sum(x => x.Quantity) - yarnIssueList.Sum(x => x.Quantity);
                var rAmt = yarnReceivelist.Sum(x => x.Quantity * x.Rate);
                var iAmnt = yarnIssueList.Sum(x => x.Quantity * x.Rate);
                var balanceAmt = (rAmt - iAmnt);


                if (sinh != 0)
                {
                    var rate = balanceAmt / sinh;
                    status = new { SinH = sinh, Rate = rate};
                }
                else
                {
                    status = new { SinH = sinh, Rate = 0};
                }

            }
            else
            {
                status = new { SinH = 0, Rate = 0 };
            }

            return status;

        }

        public List<VwStockPosition> GetStockPostionDetail(DateTime? fromDate, DateTime? toDate, int groupId, int subGroupId)
        {
            return _stockRegisterRepository.GetStockPostionDetail(fromDate, toDate, groupId, subGroupId);
        }

        public List<VwStockPosition> GetDyedYarnStockPostionDetail(DateTime? fromDate, DateTime? toDate, int groupId, int subGroupId)
        {
            return _stockRegisterRepository.GetDyedYarnStockPostionDetail(fromDate, toDate, groupId, subGroupId);
        }

        public bool IsYarnCountValid(int itemId,string fColorRefId, string colorRefId, string sizeRefId)
        {
            return _againstPoDetail.Exists(x => x.ItemId == itemId && x.ColorRefId == colorRefId && x.FColorRefId == fColorRefId && x.SizeRefId == sizeRefId);
        }

        public List<VwStockPosition> GetBuyerWiseStockPostionDetail(DateTime? fromDate, DateTime? toDate)
        {
            return _stockRegisterRepository.GetBuyerWiseStockPostionDetail(fromDate, toDate);
        }

        public object GetYarnStockStatusByLot(string colorRefId)
        {
            var store =
                _stockRegisterRepository.Filter(
                    x =>
                        x.ColorRefId == colorRefId && x.CompId == PortalContext.CurrentUser.CompId &&
                        x.StoreId == (int) StoreType.Yarn &&
                        (new[] { (int)ActionType.YarnReceive }).Contains(x.ActionType) && x.TransactionType == 1).OrderByDescending(x => x.AdvanceStoreLadgerId).FirstOrDefault() ?? new Inventory_StockRegister();

            VwMaterialReceiveAgainstPoDetail detail = _againstPoDetail.GetMaterialReceiveAgainstPoDetail( store.ItemId ,store.SourceId ,colorRefId , PortalContext.CurrentUser.CompId) ?? new VwMaterialReceiveAgainstPoDetail();

            var yarnReceivelist = _stockRegisterRepository.Filter(x =>x.ItemId==store.ItemId&& x.ColorRefId == colorRefId && x.CompId == PortalContext.CurrentUser.CompId && x.StoreId == (int)StoreType.Yarn && (new[] { (int)ActionType.YarnReceive }).Contains(x.ActionType) && x.TransactionType == 1).ToList();
            var yarnIssueList = _stockRegisterRepository.Filter(x => x.ItemId == store.ItemId && x.ColorRefId == colorRefId && x.CompId == PortalContext.CurrentUser.CompId && x.StoreId == (int)StoreType.Yarn && (x.ActionType == (int)ActionType.YarnIssue || x.ActionType == (int)ActionType.YarnDelivery || x.ActionType == (int)ActionType.YarnReturn) && x.TransactionType == 2).ToList();
            object status;
            if (yarnReceivelist.Any())
            {
                var sinh = yarnReceivelist.Sum(x => x.Quantity) - yarnIssueList.Sum(x => x.Quantity);
                var rAmt = yarnReceivelist.Sum(x => x.Quantity * x.Rate);
                var iAmnt = yarnIssueList.Sum(x => x.Quantity * x.Rate);
                var balanceAmt = (rAmt - iAmnt);
                if (sinh != 0)
                {
                    var rate = balanceAmt / sinh;
                    status = new { SinH = sinh, Rate = rate, ItemName = detail.ItemName, ItemId = store.ItemId, SizeRefId = detail.SizeRefId, SizeName = detail.SizeName,FColorRefId = detail.FColorRefId, FColorName = detail.FColorName, Brand = detail.Brand, ColorRefId = detail.ColorRefId, ColorName = detail.ColorName };
                }
                else
                {
                    status = new { SinH = sinh, Rate = 0, ItemName = detail.ItemName, ItemId = store.ItemId, SizeRefId = detail.SizeRefId, SizeName = detail.SizeName,FColorRefId = detail.FColorRefId, FColorName = detail.FColorName, Brand = detail.Brand, ColorRefId = detail.ColorRefId, ColorName = detail.ColorName };
                }

            }
            else
            {
                status = new { SinH = 0, Rate = 0, ItemName = detail.ItemName, ItemId = store.ItemId, SizeRefId = detail.SizeRefId, SizeName = detail.SizeName,FColorRefId=detail.FColorRefId, FColorName = detail.FColorName, Brand = detail.Brand,ColorRefId=detail.ColorRefId,ColorName=detail.ColorName };
            }

            return status;
        }
    }
}
