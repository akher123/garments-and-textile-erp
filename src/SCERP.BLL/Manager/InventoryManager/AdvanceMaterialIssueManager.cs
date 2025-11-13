using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Transactions;
using SCERP.BLL.IManager.IInventoryManager;
using SCERP.Common;
using SCERP.DAL.IRepository.IInventoryRepository;
using SCERP.Model.InventoryModel;

namespace SCERP.BLL.Manager.InventoryManager
{
    public class AdvanceMaterialIssueManager : IAdvanceMaterialIssueManager
    {
        private readonly IAdvanceMaterialIssueRepository _advanceMaterialIssueRepository;
        private readonly IAdvanceMaterialIssueDetailRepository _advanceMaterialIssueDetailRepository;
        private readonly IStockRegisterRepository _stockRegisterRepository;
        public AdvanceMaterialIssueManager(IAdvanceMaterialIssueRepository advanceMaterialIssueRepository, IAdvanceMaterialIssueDetailRepository advanceMaterialIssueDetailRepository, IStockRegisterRepository stockRegisterRepository)
        {
            _advanceMaterialIssueRepository = advanceMaterialIssueRepository;
            _advanceMaterialIssueDetailRepository = advanceMaterialIssueDetailRepository;
            _stockRegisterRepository = stockRegisterRepository;
        }

        public List<VwAdvanceMaterialIssue> GetAdvanceMaterialIssue(int pageIndex, string sort, string sortdir, DateTime? fromDate, DateTime? toDate,
            string searchString, int iType, out int totalRecords)
        {
            var pageSize = AppConfig.PageSize;
            string compId = PortalContext.CurrentUser.CompId;
            IQueryable<VwAdvanceMaterialIssue> materialIssues = _advanceMaterialIssueRepository.GetAdvanceMaterialIssue(compId, fromDate, toDate, searchString, iType);
            totalRecords = materialIssues.Count();
            materialIssues = materialIssues.OrderByDescending( x => x.IRefId).Skip(pageIndex * pageSize).Take(pageSize);
            return materialIssues.ToList();
        }

        public string GetNewRefId(int storeId)
        {
            string compId = PortalContext.CurrentUser.CompId;
            var refIdDesgit = _advanceMaterialIssueRepository.Filter(x => x.CompId == compId&&x.StoreId==storeId).Max(x => x.IRefId) ?? "0";
            string refId = refIdDesgit.IncrementOne().PadZero(10);
            return refId;
        }

        public Inventory_AdvanceMaterialIssue GetAdvanceMaterialIssueById(long advanceMaterialIssueId)
        {
            return _advanceMaterialIssueRepository.FindOne(x => x.AdvanceMaterialIssueId == advanceMaterialIssueId) ?? new Inventory_AdvanceMaterialIssue();
        }

        public int SaveAdvanceMaterialIssue(Inventory_AdvanceMaterialIssue materialIssue)
        {
            string compId = PortalContext.CurrentUser.CompId;
            int saveIndex = 0;
            using (var transaction = new TransactionScope())
            {
                _advanceMaterialIssueDetailRepository.Delete(x => x.AdvanceMaterialIssueId == materialIssue.AdvanceMaterialIssueId && x.CompId == compId);
                _stockRegisterRepository.Delete(x => x.SourceId == materialIssue.AdvanceMaterialIssueId && x.CompId == compId && x.ActionType ==materialIssue.IType&&x.TransactionType==2);
                saveIndex += _advanceMaterialIssueRepository.Save(materialIssue);
                var stokRegisters = materialIssue.Inventory_AdvanceMaterialIssueDetail.Select(x => new Inventory_StockRegister()
                 {
                     ItemId = x.ItemId,
                     ColorRefId = x.ColorRefId,
                     SizeRefId = x.SizeRefId,
                     TransactionDate = materialIssue.IRNoteDate,
                     TransactionType =(int) StoreLedgerTransactionType.Issue,
                     SourceId = materialIssue.AdvanceMaterialIssueId,
                     StoreId = materialIssue.StoreId,
                     Rate = x.IssueRate,
                     Quantity = x.IssueQty,
                     ActionType =materialIssue.IType,
                     CompId = compId,
                 }).ToList();
                saveIndex += _stockRegisterRepository.SaveList(stokRegisters);
                transaction.Complete();
            }
            return saveIndex;
        }

        public List<VwAdvanceMaterialIssueDetail> GetVwAdvanceMaterialssDtl(long advanceMaterialIssueId)
        {
            string compId = PortalContext.CurrentUser.CompId;
            return _advanceMaterialIssueDetailRepository.GetVwAdvanceMaterialssDtl(advanceMaterialIssueId, compId);
        }

        public VwAdvanceMaterialIssue GetVwAdvanceMaterialIssueById(long advanceMaterialIssueId)
        {
            string compId = PortalContext.CurrentUser.CompId;
            return _advanceMaterialIssueRepository.GetVwAdvanceMaterialIssueById(advanceMaterialIssueId, compId);
        }

        public List<VwAdvanceMaterialIssue> GetAdvanceMaterialIssues(DateTime? fromDate, DateTime? toDate, string searchString, int storeId)
        {
            string compId = PortalContext.CurrentUser.CompId;
            return _advanceMaterialIssueRepository.GetAdvanceMaterialIssue(compId, fromDate, toDate, searchString, storeId).ToList();
        }

        public int DeleteAdvanceMaterialIssue(long advanceMaterialIssueId, int iType)
        {
            string compId = PortalContext.CurrentUser.CompId;
            int delteIndex = 0;
            using (var transaction = new TransactionScope())
            {
                delteIndex+= _advanceMaterialIssueDetailRepository.Delete(x => x.AdvanceMaterialIssueId == advanceMaterialIssueId && x.CompId == compId);
                delteIndex += _stockRegisterRepository.Delete(x => x.SourceId == advanceMaterialIssueId && x.ActionType == iType && x.CompId == compId);
                delteIndex += _advanceMaterialIssueRepository.Delete( x => x.AdvanceMaterialIssueId == advanceMaterialIssueId&&x.IType==iType && x.CompId == compId);
                transaction.Complete();
            }
            return delteIndex;
        }

        public string GetAccNewRefId()
        {
            string compId = PortalContext.CurrentUser.CompId;
            int storeId =(int) StoreType.Acessories;
            var refIdDesgit = _advanceMaterialIssueRepository.Filter(x => x.CompId == compId && x.StoreId == storeId).Max(x => x.IRefId.Substring(2)) ?? "0";
            string refId = refIdDesgit.IncrementOne().PadZero(8);
            return "AI"+refId;
        }

        public int LoackYarnIssue(long advanceMaterialIssueId)
        {
            var issue = GetAdvanceMaterialIssueById(advanceMaterialIssueId);
            if (issue.LockStatus==false)
            {
                issue.ApprovedBy = PortalContext.CurrentUser.UserId;
                issue.LockStatus = true;
            }
            else
            {
                issue.ApprovedBy =null;
                issue.LockStatus = false;
            }
         
            return _advanceMaterialIssueRepository.Edit(issue);
        }

        public IEnumerable GetDeliverdYarn(string yarndyeing,int storeId)
        {
           return  _advanceMaterialIssueRepository.Filter(x => x.StoreId == storeId && x.ProcessRefId == yarndyeing).Select(x=>new
            {
                Value = x.IRefId,
                Text = x.IRefId + "(Order :" + x.OrderNo + ",Style No :" + x.StyleNo + ")"
            }).OrderByDescending(x=>x.Value).ToList();
        }

      

        public Dictionary<string,VwMaterialReceiveAgainstPoDetail> GetDeliverdYarnDetail(string refId,string compId)
        {
            List<VwAdvanceMaterialIssueDetail> details =  _advanceMaterialIssueDetailRepository.GetDeliverdYarnDetail(refId, compId);
            var  dictionary= new Dictionary<string, VwMaterialReceiveAgainstPoDetail>();
            if (details.Any())
            {
               dictionary=details.ToDictionary(x => Convert.ToString(x.AdvanceMaterialIssueDetailId), x => new VwMaterialReceiveAgainstPoDetail()
                {
                    ItemId = x.ItemId,
                    ItemName = x.ItemName,
                    ColorRefId = x.ColorRefId,
                    SizeName = x.SizeName,
                    ColorName = x.ColorName,
                    SizeRefId = x.SizeRefId,
                    FColorRefId = x.GColorRefId,
                    FColorName = x.GColorName,
                    ReceivedRate = x.IssueRate,
                    ReceivedQty = x.IssueQty,
                });   
            }
            return dictionary;

        }

        public Inventory_AdvanceMaterialIssue GetYarnDeliveryByRefd(string piBookingRefId, string compId)
        {
           return _advanceMaterialIssueRepository.FindOne(x => x.IRefId == piBookingRefId && x.CompId == compId);
        }

        public Dictionary<string, VwAdvanceMaterialIssueDetail> GetAccessoriesRcvSummary(string orderStyleRefId, string currentUserCompId)
        {
            List<AccessoriesReceiveBalance> accessoriesReceiveBalances =
                _advanceMaterialIssueDetailRepository.GetAccessoriesRcvSummary(orderStyleRefId, currentUserCompId);


            return accessoriesReceiveBalances.ToDictionary(x => x.PurchaseOrderDetailId.ToString(),
                x => new VwAdvanceMaterialIssueDetail()
                {
                    ItemId = x.ItemId,
                    ItemCode = x.ItemCode,
                    ItemName = x.ItemName,
                    ColorRefId = x.ColorRefId,
                    SizeRefId = x.SizeRefId,
                    
                    ColorName = x.ColorName,
                    SizeName = x.SizeName,
                    FColorRefId = x.FColorRefId,
                    GSizeRefId = x.GSizeRefId,
                    FColorName = x.FColorName,
                    GSizeName = x.GSizeName,
                    TotalRcvQty = x.TotalRcvQty,
                    StockInHand = x.TotalRcvQty - x.ToalIssueQty,
                    QtyInBag = x.ToalIssueQty,
                    IssueQty = 0,
                    IssueRate = x.Rate,
                    PurchaseOrderDetailId = x.PurchaseOrderDetailId
                    
                }
            );
        }

        public Dictionary<string, VwAdvanceMaterialIssueDetail> GetAccessorisEditRcvDetails(long materialIssueAdvanceMaterialIssueId)
        {
            List<AccessoriesReceiveBalance> accessoriesReceiveBalances =
                _advanceMaterialIssueDetailRepository.GetAccessorisEditRcvDetails(materialIssueAdvanceMaterialIssueId);
            return accessoriesReceiveBalances.ToDictionary(x => x.PurchaseOrderDetailId.ToString(),
                x => new VwAdvanceMaterialIssueDetail()
                {
                    AdvanceMaterialIssueDetailId = x.AdvanceMaterialIssueDetailId,
                    AdvanceMaterialIssueId = x.AdvanceMaterialIssueId,
                    ItemId = x.ItemId,
                    ItemCode = x.ItemCode,
                    ItemName = x.ItemName,
                    ColorRefId = x.ColorRefId,
                    SizeRefId = x.SizeRefId,

                    ColorName = x.ColorName,
                    SizeName = x.SizeName,
                    FColorRefId = x.FColorRefId,
                    GSizeRefId = x.GSizeRefId,
                    FColorName = x.FColorName,
                    GSizeName = x.GSizeName,
                    TotalRcvQty = x.TotalRcvQty,
                    StockInHand = x.TotalRcvQty - x.ToalIssueQty,
                    QtyInBag = x.ToalIssueQty,
                    IssueQty = x.ToalIssueQty,
                    IssueRate = x.Rate,
                    PurchaseOrderDetailId = x.PurchaseOrderDetailId

                }
            );
        }

        public DataTable GetAccessoriesIssueChallanDataTable(long advanceMaterialIssueId)
        {
            return _advanceMaterialIssueDetailRepository.GetAccessoriesIssueChallanDataTable(advanceMaterialIssueId);
        }

        public DataTable GetAccessoriesIssueDetailStatus(DateTime? fromDate, DateTime? toDate, string compid)
        {
            return _advanceMaterialIssueDetailRepository.GetAccessoriesIssueDetailStatus(fromDate, toDate,compid);
        }

        public List<Inventory_AdvanceMaterialIssue> GeChallanListByPartyId(int partyId)
        {
            int iType = (int) ActionType.YarnDelivery;
          return  _advanceMaterialIssueRepository.Filter(x => x.PartyId ==partyId && x.LockStatus == false&&x.IType==iType).ToList();
        }

        public DataTable GetDeliverdYarnByStyle(string orderStyleRefId)
        {
            string sql = @"select ISNULL((select top(1) ItemName from Inventory_Item where ItemId=MID.ItemId),'Total :') AS Yarn,
                (select ColorName from OM_Color where ColorRefId=MID.FColorRefId and CompId=MID.CompId) AS Color,
                (select ColorName from OM_Color where ColorRefId=MID.ColorRefId and CompId=MID.CompId) AS LotNo,
                (select SizeName from OM_Size where SizeRefId=MID.SizeRefId and CompId=MID.CompId) AS [Count]
                ,SUM(MID.IssueQty) AS Quantity from Inventory_AdvanceMaterialIssue AS MI
                INNER JOIN PLAN_Program AS PG ON MI.ProgramRefId=PG.ProgramRefId
                INNER JOIN Inventory_AdvanceMaterialIssueDetail AS MID ON MI.AdvanceMaterialIssueId=MID.AdvanceMaterialIssueId
                WHERE PG.OrderStyleRefId='{0}'
                GROUP BY GROUPING SETS ((MID.CompId,MID.SizeRefId,MID.ColorRefId,MID.FColorRefId,MID.ItemId), ())";
            sql = string.Format(sql, orderStyleRefId);
            return _advanceMaterialIssueRepository.ExecuteQuery(sql);
        }

        public List<ProgramYarnWithdrow> GetProgramYarnWithdrow(string programRefId)
        {
          return  _advanceMaterialIssueRepository.GetProgramYarnWithdrow(programRefId, PortalContext.CurrentUser.CompId);
        }
    }
}
