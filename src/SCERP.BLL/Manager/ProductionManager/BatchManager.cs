using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Transactions;
using SCERP.BLL.IManager.IPlanningManager;
using SCERP.BLL.IManager.IProductionManager;
using SCERP.Common;
using SCERP.DAL.IRepository.IProductionRepository;
using SCERP.Model.Production;

namespace SCERP.BLL.Manager.ProductionManager
{
    public class BatchManager : IBatchManager
    {
        private readonly IBatchRepository _batchRepository;
        private readonly IBatchDetailRepository _batchDetailRepository;
        private readonly ITimeAndActionManager _timeAndActionManager;
        public BatchManager(ITimeAndActionManager timeAndActionManager,IBatchRepository batchRepository, IBatchDetailRepository batchDetailRepository)
        {
            _batchRepository = batchRepository;
            _batchDetailRepository = batchDetailRepository;
            _timeAndActionManager = timeAndActionManager;
        }

        public List<VProBatch> GetBachListByPaging(VProBatch model, out int totalRecords)
        {
            IQueryable<VProBatch> vProBatches = _batchRepository.GetBachList();

            vProBatches = vProBatches.Where(
                   x =>
                      ((x.BatchNo.Trim().Contains(model.SearchString) || String.IsNullOrEmpty(model.SearchString.Trim().ToLower()))
                      || (x.PartyName.Trim().Contains(model.SearchString) || String.IsNullOrEmpty(model.SearchString.Trim().ToLower()))
                      || (x.MachineName.Trim().Contains(model.SearchString) || String.IsNullOrEmpty(model.SearchString.Trim().ToLower()))
                      || (x.ColorName.Trim().Contains(model.SearchString) || String.IsNullOrEmpty(model.SearchString.Trim().ToLower()))
                       || (x.OrderStyleRefId.Contains(model.SearchString) || String.IsNullOrEmpty(model.SearchString)))
                      
                      && (x.BtType == model.BtType || model.BtType == null) && (x.BatchStatus == model.BatchStatus || model.BatchStatus == 0)
                      && ((x.BatchDate >= model.FromDate || model.FromDate == null) && (x.BatchDate <= model.ToDate || model.ToDate == null)));
            var index = model.PageIndex;
            var pageSize = AppConfig.PageSize;
            totalRecords = vProBatches.Count();
            switch (model.sort)
            {
                case "BatchNo":
                    switch (model.sortdir)
                    {

                        case "DESC":
                            vProBatches = vProBatches.OrderByDescending(
                                x => x.BatchNo)
                              .Skip(index * pageSize)
                                .Take(pageSize);
                            break;
                        default:
                            vProBatches = vProBatches.OrderBy(
                                x => x.BatchNo)
                                .Skip(index * pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;

                case "BatchDate":
                    switch (model.sortdir)
                    {

                        case "DESC":
                            vProBatches = vProBatches.OrderByDescending(
                                x => x.BatchDate)
                                 .Skip(index * pageSize)
                                .Take(pageSize);
                            break;
                        default:
                            vProBatches = vProBatches.OrderBy(
                                x => x.BatchDate)
                                 .Skip(index * pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;
                case "PartyName":
                    switch (model.sortdir)
                    {

                        case "DESC":
                            vProBatches = vProBatches.OrderByDescending(
                                x => x.PartyName)
                               .Skip(index * pageSize)
                                .Take(pageSize);
                            break;
                        default:
                            vProBatches = vProBatches.OrderBy(
                                x => x.PartyName)
                               .Skip(index * pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;
                case "ColorName":
                    switch (model.sortdir)
                    {

                        case "DESC":
                            vProBatches = vProBatches.OrderByDescending(
                                x => x.ColorName)
                                .Skip(index * pageSize)
                                .Take(pageSize);
                            break;
                        default:
                            vProBatches = vProBatches.OrderBy(
                                x => x.ColorName)
                                .Skip(index * pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;
                case "BatchQty":
                    switch (model.sortdir)
                    {

                        case "DESC":
                            vProBatches = vProBatches.OrderByDescending(
                                x => x.BatchQty)
                                .Skip(index * pageSize)
                                .Take(pageSize);
                            break;
                        default:
                            vProBatches = vProBatches.OrderBy(
                                x => x.BatchQty)
                               .Skip(index * pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;

                default:
                    vProBatches = vProBatches.OrderByDescending(
                        x => x.BatchId)
                        .Skip(index * pageSize)
                        .Take(pageSize);
                    break;
            }
            return vProBatches.ToList();
        }

      

        public int SaveBatch(Pro_Batch model)
        {

            string prifix = "BT";
            model.IsActive = true;
            model.CreatedBy = PortalContext.CurrentUser.UserId;
            model.CreatedDate = DateTime.Now;
            model.BatchStatus =Convert.ToInt32(BatchStatus.Pending);
            model.CompId = PortalContext.CurrentUser.CompId;
            model.BtRefNo = _batchRepository.GetNewBtRefNo(prifix);
            int saved= _batchRepository.Save(model);
            if (saved > 0)
            {
                _timeAndActionManager.UpdateActualStartDate(TnaActivityKeyValue.Bulk_dyeing_Sloid_Fabric, model.BatchDate.GetValueOrDefault(), model.OrderStyleRefId, model.CompId);
            }
            return saved;
        }
        public int EditBatch(Pro_Batch model)
        {
            int edited = 0;
            using (var transaction = new TransactionScope())
            {
                _batchDetailRepository.Delete(x => x.BatchId == model.BatchId);
                var batch = _batchRepository.FindOne(x => x.IsActive && x.BatchId == model.BatchId);
                batch.BatchDate = model.BatchDate;
                batch.BatchNo = model.BatchNo;
                batch.MachineId = model.MachineId;
                batch.PartyId = model.PartyId;
                batch.BatchQty = model.BatchQty;
                batch.GroupSubProcessId = model.GroupSubProcessId;
                batch.BatchStatus = 1;
                batch.ItemId = model.ItemId;
                batch.Gsm = model.Gsm;
                batch.GrColorRefId = model.GrColorRefId;
                batch.BuyerRefId = model.BuyerRefId;
                batch.OrderNo = model.OrderNo;
                batch.OrderStyleRefId = model.OrderStyleRefId;
                //batch.CostRate = model.CostRate;
               // batch.BillRate = model.BillRate;
                batch.ApprovedLdNo = model.ApprovedLdNo;
                batch.JobRefId = model.JobRefId;
                batch.Remarks = model.Remarks;
            
                batch.EditedBy = PortalContext.CurrentUser.UserId;
                batch.EditedDate = DateTime.Now;
                edited = _batchRepository.Edit(batch);
                var detail = model.PROD_BatchDetail.Select(x =>
                {
                    x.BatchId = batch.BatchId;
                    return x;
                });
                edited += _batchDetailRepository.SaveList(detail.ToList());
                transaction.Complete();
            }
            return edited;
        }
        public VProBatch GetBachById(long batchId)
        {
            var batch = _batchRepository.GetBachList().FirstOrDefault(x=>x.BatchId==batchId);
            return batch;
        }

        public VProBatch GetBachByRefNo(string btRefNo)
        {
            return _batchRepository.GetBachList().FirstOrDefault(x => x.BtRefNo == btRefNo);
        }

        public bool IsBatchExist(Pro_Batch model)
        {
            return
                _batchRepository.Exists(
                    x => x.IsActive && x.BatchId != model.BatchId && x.BatchNo == model.BatchNo.Trim());
        }

        public int SaveInActiveBatch(int batchId)
        {
                int updatedRows = 0;
                Pro_Batch batch = _batchRepository.FindOne(x => x.BatchId == batchId);
                batch.BatchStatus = batch.BatchStatus==1?0:1;
                updatedRows+= _batchRepository.Edit(batch);
          
            return updatedRows;
        }

        public string GetBachNewRefNo( string prefix)
        {
            return _batchRepository.GetNewBtRefNo( prefix);
        }

        public int DeleteBatch(long batchId, string compId)
        {
            int deletedRow = 0;
            using (var transaction = new TransactionScope())
            {
                deletedRow += _batchDetailRepository.Delete(x => x.CompId == compId && x.BatchId == batchId);
                deletedRow += _batchRepository.Delete(x => x.CompId == compId && x.BatchId == batchId);
                transaction.Complete();
            }
            return deletedRow;
        }

        public int UpdateBatchStatus(Pro_Batch model)
        {
            int edited = 0;
            var batch = _batchRepository.FindOne(x => x.IsActive && x.BatchId == model.BatchId);
            batch.LoadingDateTime = model.LoadingDateTime;
            batch.UnLoadingDateTime = model.UnLoadingDateTime;
            batch.BatchStatus = model.BatchStatus;
            batch.ShadePerc = model.ShadePerc;
            batch.BillRate =Convert.ToDecimal( model.PROD_BatchDetail.Average(x=>x.Rate));
            foreach (PROD_BatchDetail batchDetail in model.PROD_BatchDetail)
            {
                var bd=  _batchDetailRepository.FindOne(x => x.BatchDetailId == batchDetail.BatchDetailId);
                bd.Rate = batchDetail.Rate;
                _batchDetailRepository.Edit(bd);
            }
            edited = _batchRepository.Edit(batch);
            return edited;
        }

        public List<VProBatch> GetBachStatus(DateTime? fromDate, DateTime? toDate, long partyId, int machineId)
        {
            IQueryable<VProBatch> vProBatches = _batchRepository.GetBachList();

            vProBatches = vProBatches.Where(
                   x =>
                      (x.PartyId==partyId || partyId==0)
                     && (x.MachineId==machineId||machineId==0)
                      && ((x.BatchDate >= fromDate|| fromDate== null) && (x.BatchDate <= toDate || toDate == null)));
            return vProBatches.ToList();
        }

        public object BatchAutoComplite(string compId, string searchString)
        {
           IQueryable<VProBatch> vProBatches = _batchRepository.GetBachList();
           return vProBatches.Where(x => x.CompId == compId && x.BatchNo.Contains(searchString)).Select(x => new { BatchNo = x.BatchNo, BatchId = x.BatchId, BtRefNo = x.BtRefNo }).ToList();
        }

        public List<Pro_Batch> GetAllBatch(string compId)
        {
            return _batchRepository.Filter(x => x.CompId == compId).ToList();
        }

        public IEnumerable GetBachByBatchId(long batchId, string compId)
        {
            return _batchRepository.GetBachByBatchId(batchId,compId).Select(x=>new
            {
                x.BatchDetailId,ItemName="Fabric Type:"+x.ItemName+"  "+"DIA :"+x.FdiaSizeName,x.Quantity
            }).ToList();
        }

        public double GetBatchItemQtyByBatchDetailId(long batchDetailId, string compId)
        {
          var batchDetai=  _batchDetailRepository.FindOne(x => x.BatchDetailId == batchDetailId && x.CompId == compId);
          if (batchDetai!=null)
            {
                return batchDetai.Quantity;
            }
            return 0;

        }

        public IEnumerable BatchAutoCompliteByParty(string searchString, long partyId)
        {
            var batchList = _batchRepository.Filter(
                x =>
                    x.CompId == PortalContext.CurrentUser.CompId && x.BatchNo.Contains(searchString) &&
                    x.PartyId == partyId).Take(10);
          
            return batchList.Select(x=>new
            {
                x.BatchNo,x.BtRefNo,x.BatchId
            }).ToList();
        }

        public List<Pro_Batch> GetBachList(string searchString,int? btType,int btStatus, int pageIndex, out int totalRecords)
        {
            IQueryable<Pro_Batch> vProBatches = _batchRepository.All();

            vProBatches = vProBatches.Where(
                   x =>
                      ((x.BatchNo.Trim().Contains(searchString) || String.IsNullOrEmpty(searchString.Trim().ToLower()))
                      && (x.BtType == btType || btType == null) && (x.BatchStatus == btStatus ||btStatus == 0)));
            var index =pageIndex;
            var pageSize = AppConfig.PageSize;
            totalRecords = vProBatches.Count();
            vProBatches = vProBatches.OrderByDescending(
                      x => x.BatchId)
                      .Skip(index * pageSize)
                      .Take(pageSize);
            return vProBatches.ToList();
        }

        public DataTable GetBachQtyByStyle(string orderStyleRefId)
        {
            string sql = @"select 
                            ISNULL((select top(1) ItemName from Inventory_Item where ItemId=BD.ItemId),'Total :') AS Fabric,
                            (select ColorName from OM_Color where ColorRefId=BT.GrColorRefId and CompId=BT.CompId) AS Color,
                            (select SizeName from OM_Size where SizeRefId=BD.MdiaSizeRefId and CompId=BT.CompId) AS McDia,
                            (select SizeName from OM_Size where SizeRefId=BD.FdiaSizeRefId and CompId=BT.CompId) AS FDia,
                            BD.GSM
                            ,SUM(BD.Quantity) AS Quantity 
                             from PROD_BatchDetail AS BD
                            INNER JOIN Pro_Batch AS BT ON BD.BatchId=BT.BatchId
                            where BT.OrderStyleRefId='{0}'
                            GROUP BY GROUPING SETS ((BD.ItemId,BD.GSM,BT.CompId,BT.GrColorRefId,BD.MdiaSizeRefId,BD.FdiaSizeRefId),())
                            ";
            sql = string.Format(sql, orderStyleRefId);
            return _batchRepository.ExecuteQuery(sql);
        }
    }
}
