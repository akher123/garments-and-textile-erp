using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Transactions;
using SCERP.BLL.IManager.IInventoryManager;
using SCERP.BLL.IManager.IPlanningManager;
using SCERP.Common;
using SCERP.DAL.IRepository;
using SCERP.DAL.IRepository.IInventoryRepository;
using SCERP.DAL.IRepository.IProductionRepository;
using SCERP.Model.InventoryModel;

namespace SCERP.BLL.Manager.InventoryManager
{
    public class FinishFabStoreManager : IFinishFabStoreManager
    {
        private readonly IFinishFabStoreRepository _fabStoreRepository;
        private readonly IRepository<Inventory_FinishFabDetailStore> _finishFabDetailStoRepository;
        private readonly ITimeAndActionManager _timeAndActionManager;
        private readonly IBatchRepository batchRepository;
        public FinishFabStoreManager(ITimeAndActionManager timeAndActionManager,IBatchRepository batchRepository, IFinishFabStoreRepository fabStoreRepository, IRepository<Inventory_FinishFabDetailStore> finishFabDetailStoRepository)
        {
            _fabStoreRepository = fabStoreRepository;
            _finishFabDetailStoRepository = finishFabDetailStoRepository;
            this.batchRepository = batchRepository;
            this._timeAndActionManager = timeAndActionManager;
        }

        public List<Inventory_FinishFabStore> GetFinishFabricLsitByPaging(string compId, string searchString, int pageIndex, out int totalRecords)
        {
            var finishFabricList =
                _fabStoreRepository.GetWithInclude(x => x.CompId == compId && (x.FinishFabRefId.Contains(searchString) || String.IsNullOrEmpty(searchString)), "PROD_DyeingSpChallan.Party");
            var pageSize = AppConfig.PageSize;
            totalRecords = finishFabricList.Count();
            finishFabricList = finishFabricList
                    .OrderByDescending(r => r.FinishFabStoreId)
                    .Skip(pageIndex * pageSize)
                    .Take(pageSize);
            return finishFabricList.ToList();
        }

        public Inventory_FinishFabStore GetFinishFabricById(long finishFabStoreId)
        {
            var finishFab =
                 _fabStoreRepository.GetWithInclude(x => x.CompId == PortalContext.CurrentUser.CompId, "PROD_DyeingSpChallan").FirstOrDefault(x => x.FinishFabStoreId == finishFabStoreId);
            return finishFab;
        }

        public string GetNewRefId(string compId)
        {
            string prefix = "FF";
            var refString = _fabStoreRepository.Filter(x => x.CompId == compId && x.FinishFabRefId.Substring(0, prefix.Length) == prefix)
                  .Max(x => x.FinishFabRefId.Substring(prefix.Length, 8)) ?? "0";
            var programRefId = prefix + refString.IncrementOne().PadZero(6);
            return programRefId;
        }

        public int EditFinishFabric(Inventory_FinishFabStore model)
        {
            var edited = 0;
            using (var transaction = new TransactionScope())
            {
                var finishFab = _fabStoreRepository.FindOne(x => x.FinishFabStoreId == model.FinishFabStoreId);
                finishFab.InvoiceNo = model.InvoiceNo;
                finishFab.InvoiceDate = model.InvoiceDate;
                finishFab.GateEntryNo = model.GateEntryNo;
                finishFab.GateEntryDate = model.GateEntryDate;
                finishFab.RcvRegNo = model.RcvRegNo;
                finishFab.Remarks = model.Remarks;
                finishFab.EditedBy = model.EditedBy;
                finishFab.DyeingSpChallanId = model.DyeingSpChallanId;
                _finishFabDetailStoRepository.Delete(x => x.FinishFabStoreId == model.FinishFabStoreId);
                _finishFabDetailStoRepository.SaveList(model.Inventory_FinishFabDetailStore.ToList());
                edited = _fabStoreRepository.Edit(finishFab);
                transaction.Complete();
            }
            return edited;
        }

        public int SaveFinishFabric(Inventory_FinishFabStore finishFabStore)
        {
            int saved = _fabStoreRepository.Save(finishFabStore);
            if (finishFabStore.Inventory_FinishFabDetailStore.Any() && saved > 0)
            {
                List<long> batchIds = finishFabStore.Inventory_FinishFabDetailStore.Select(x => x.BatchId).Distinct().ToList();
                List<string> orderStyleRefIds = batchRepository.Filter(x => batchIds.Contains(x.BatchId)).Select(x => x.OrderStyleRefId).ToList();
                foreach (var orderStyleRefId in orderStyleRefIds)
                {
                    _timeAndActionManager.UpdateActualStartDate(TnaActivityKeyValue.FINISH_FABRICS_DELIVERY, finishFabStore.InvoiceDate.GetValueOrDefault(), orderStyleRefId, finishFabStore.CompId);
                }
            }
            return saved;
        }


        public int DeleteFinishFabric(long finishFabStoreId)
        {
            var deleted = 0;
            using (var transaction = new TransactionScope())
            {
                deleted = _finishFabDetailStoRepository.Delete(x => x.FinishFabStoreId == finishFabStoreId);
                deleted += _fabStoreRepository.Delete(x => x.FinishFabStoreId == finishFabStoreId);
                transaction.Complete();
            }
            return deleted;
        }

        public List<SpInvFinishFabStore> GetDetailChallanBy(long dyeingSpChallanId, long finishFabStoreId)
        {
            return _fabStoreRepository.GetDetailChallanBy(dyeingSpChallanId, finishFabStoreId);
        }

        public IEnumerable GetBatchDeailsById(long batchId)
        {
            return _fabStoreRepository.GetBatchDeailsById(batchId);
        }

        public object GetFinishFabIssueDetail(long batchDetailId)
        {
            return _fabStoreRepository.GetFinishFabIssueDetail(batchDetailId);
        }

        public DataTable GetFinishFabricDeliveryDataTable(long finishFabIssueId)
        {
            string sqlQuery = String.Format("exec SpInvFinishFabricDeliveryChallan '{0}'", finishFabIssueId);
            return _fabStoreRepository.ExecuteQuery(sqlQuery);
        }

        public DataTable GetFinishFabricDeliveryByStyle(string orderStyleRefId)
        {
            string sql = @"select 
                            ISNULL((select top(1) ItemName from Inventory_Item where ItemId=BD.ItemId),'Total :') AS Fabric,
                            (select ColorName from OM_Color where ColorRefId=BT.GrColorRefId and CompId=BT.CompId) AS Color,
                            (select SizeName from OM_Size where SizeRefId=BD.MdiaSizeRefId and CompId=BT.CompId) AS McDia,
                            (select SizeName from OM_Size where SizeRefId=BD.FdiaSizeRefId and CompId=BT.CompId) AS FDia,
                            BD.GSM
                            ,SUM(SPD.GreyWt) AS GQuantity 
                            ,SUM(SPD.RcvQty) AS FQuantity 
                            ,CAST(ROUND((SUM(SPD.GreyWt)-SUM(SPD.RcvQty))*100/SUM(SPD.GreyWt),2) AS varchar(20))+' %' AS PLQuantity 
                            from Inventory_FinishFabDetailStore AS SPD
                            INNER JOIN PROD_BatchDetail AS BD ON SPD.BatchDetailId=BD.BatchDetailId 
                            INNER JOIN Pro_Batch AS BT ON BD.BatchId=BT.BatchId
                            where BT.OrderStyleRefId='{0}'
                            GROUP BY GROUPING SETS ((BD.ItemId,BD.GSM,BT.CompId,BT.GrColorRefId,BD.MdiaSizeRefId,BD.FdiaSizeRefId),())";
            sql = string.Format(sql, orderStyleRefId);
            return _fabStoreRepository.ExecuteQuery(sql);
        }
    }
}
