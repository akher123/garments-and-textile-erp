using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Transactions;
using SCERP.BLL.IManager.IProductionManager;
using SCERP.Common;
using SCERP.DAL.IRepository.IProductionRepository;
using SCERP.Model.Production;

namespace SCERP.BLL.Manager.ProductionManager
{
    public class CuttingBatchManager : ICuttingBatchManager
    {
        private readonly ICuttingBatchRepository _cuttingBatchRepository;
        private readonly IPartCuttingRepository _partCuttingRepository;
        private readonly ILayCuttingRepository _layCuttingRepository;
        private readonly IRollCuttingRepository _rollCuttingRepository;
        private readonly IBundleCuttingRepository _bundleCuttingRepository;
        private readonly IRejectAdjustmentRepository _rejectAdjustmentRepository;
        private readonly IRejectReplacementRepository rejectReplacementRepository;
        private IGraddingRepository _graddingRepository;
        public CuttingBatchManager(IRejectReplacementRepository rejectReplacementRepository,IGraddingRepository graddingRepository,IRejectAdjustmentRepository rejectAdjustmentRepository,IPartCuttingRepository partCuttingRepository,
            IBundleCuttingRepository bundleCuttingRepository, IRollCuttingRepository rollCuttingRepository,
            ICuttingBatchRepository cuttingBatchRepository, ILayCuttingRepository layCuttingRepository)
        {
            _graddingRepository = graddingRepository;
            _partCuttingRepository = partCuttingRepository;
            _cuttingBatchRepository = cuttingBatchRepository;
            _layCuttingRepository = layCuttingRepository;
            _rollCuttingRepository = rollCuttingRepository;
            _bundleCuttingRepository = bundleCuttingRepository;
            _rejectAdjustmentRepository = rejectAdjustmentRepository;
            this.rejectReplacementRepository = rejectReplacementRepository;
        }
    
      

        public string GetNewCuttingBatchRefId()
        {
            var maxRefId = _cuttingBatchRepository.Filter(x => x.CompId == PortalContext.CurrentUser.CompId)
                .Max(x => x.CuttingBatchRefId.Replace(" ", "").Substring(1)) ?? "0";
            string cuttingBatchRefId = "C" + maxRefId.IncrementOne().PadZero(5);
            return cuttingBatchRefId;
        }

        public PROD_CuttingBatch GetCuttingBatchByCuttingBatchId(string cuttingBatchRefId)
        {
            return
                _cuttingBatchRepository.FindOne(
                    x => x.CompId == PortalContext.CurrentUser.CompId && x.CuttingBatchRefId == cuttingBatchRefId);
        }

        public bool IsCuttingBatchExist(PROD_CuttingBatch model)
        {
            return false;
        }

        public int EditCuttingBatch(PROD_CuttingBatch model)
        {
            int edited = 0;
            using (var transaction = new TransactionScope())
            {

                _bundleCuttingRepository.Delete(
                    x => x.CuttingBatchId == model.CuttingBatchId && x.CompId == PortalContext.CurrentUser.CompId);
                _layCuttingRepository.Delete(
                    x => x.CuttingBatchId == model.CuttingBatchId && x.CompId == PortalContext.CurrentUser.CompId);
                _partCuttingRepository.Delete(
                    x => x.CuttingBatchId == model.CuttingBatchId && x.CompId == PortalContext.CurrentUser.CompId);
                _rollCuttingRepository.Delete(
                    x => x.CuttingBatchId == model.CuttingBatchId && x.CompId == PortalContext.CurrentUser.CompId);
                model.CompId = PortalContext.CurrentUser.CompId;
                var cutBatch =
                    _cuttingBatchRepository.FindOne(
                        x => x.CuttingBatchId == model.CuttingBatchId && x.CompId == PortalContext.CurrentUser.CompId);
                cutBatch.ColorRefId = model.ColorRefId;
                cutBatch.BuyerRefId = model.BuyerRefId;
                cutBatch.OrderNo = model.OrderNo;
                cutBatch.OrderStyleRefId = model.OrderStyleRefId;
                cutBatch.StyleRefId = model.StyleRefId;
                cutBatch.FIT = model.FIT;
                cutBatch.JobNo = model.JobNo;
                cutBatch.ConsPerDzn = model.ConsPerDzn;
                cutBatch.MarkerEffPct = model.MarkerEffPct;
                cutBatch.CuttingStatus = model.CuttingStatus;
                cutBatch.Rmks = model.Rmks;
                cutBatch.ApprovalStatus = model.ApprovalStatus;
                cutBatch.CuttingDate = model.CuttingDate;
                cutBatch.ComponentRefId = model.ComponentRefId;
                cutBatch.MachineId=model.MachineId;
                edited = _cuttingBatchRepository.Edit(cutBatch);

                _layCuttingRepository.SaveList(model.PROD_LayCutting.ToList());
                _partCuttingRepository.SaveList(model.PROD_PartCutting.ToList());
                _rollCuttingRepository.SaveList(model.PROD_RollCutting.ToList());
                transaction.Complete();
            }
            return edited;
        }

        public int SaveCuttingBatch(PROD_CuttingBatch model)
        {
            model.CompId = PortalContext.CurrentUser.CompId;
          //  model.CuttingBatchRefId = GetNewCuttingBatchRefId(); 
            return _cuttingBatchRepository.Save(model);
        }

     

        public Dictionary<string, List<string>> GetCuttingJobCards(string orederStyleRefId, string colorRefId, string componentRefId,string orderShipRefId)
        {
            List<SpCuttingJobCard> cuttingJobCards = _cuttingBatchRepository.GetCuttingJobCards(orederStyleRefId, colorRefId, componentRefId, PortalContext.CurrentUser.CompId, orderShipRefId);
            List<string> sizeList = cuttingJobCards.Select(x => x.SizeName).ToList();
            sizeList.Add("Total");
            List<string> quantityList = cuttingJobCards.Select(x => Convert.ToString(x.Quantity)).ToList();
            quantityList.Add(Convert.ToString(cuttingJobCards.Sum(x => x.Quantity)));
            List<string> cuttingQtyList = cuttingJobCards.Select(x => Convert.ToString(x.CuttingQuantity)).ToList();
            cuttingQtyList.Add(Convert.ToString(cuttingJobCards.Sum(x => x.CuttingQuantity)));
            List<string> cuttingPercent = cuttingJobCards.Select(x =>x.Quantity>0? String.Format("{0:0.00}"+" "+"%",(x.CuttingQuantity*100.00m)/x.Quantity):"0").ToList();
            cuttingPercent.Add(String.Format("{0:0.00}" + " " + "%", cuttingJobCards.Sum(x => x.CuttingQuantity) * 100.0m / (cuttingJobCards.Sum(x => x.Quantity)>0?cuttingJobCards.Sum(x => x.Quantity):1)));
            List<string> sizeRefIdList = cuttingJobCards.Select(x => Convert.ToString(x.SizeRefId)).ToList();
            Dictionary<string, List<string>> pivotTable = new Dictionary<string, List<string>>();
            pivotTable.Add("Size", sizeList);
            pivotTable.Add("Quantity", quantityList);
            pivotTable.Add("Cutting", cuttingQtyList);
            pivotTable.Add("Cutting(%)", cuttingPercent);
            pivotTable.Add("Ratio", sizeRefIdList);
            return pivotTable;
        }

        public int GenerateBundleChat(long cuttingBatchId)
        {
            _bundleCuttingRepository.Delete(x => x.CuttingBatchId == cuttingBatchId && x.CompId == PortalContext.CurrentUser.CompId);
            var partcutting =
                _partCuttingRepository.FindOne(
                    x => x.CompId == PortalContext.CurrentUser.CompId && x.CuttingBatchId == cuttingBatchId);
            var cuttingBatch = _cuttingBatchRepository.FindOne(x => x.CuttingBatchId == cuttingBatchId && x.CompId == PortalContext.CurrentUser.CompId);
            var layCuttings = _layCuttingRepository.Filter(x => x.CuttingBatchId == cuttingBatchId && x.CompId == PortalContext.CurrentUser.CompId);
            int? rollStart = 0;
            int? rollEnd = 0;
            int? start = 0;
            int? end = 0;
            List<PROD_BundleCutting> bundleCuttings = new List<PROD_BundleCutting>();
            PROD_BundleCutting bundleCutting = null;
            var rollCuttings = _rollCuttingRepository.Filter(x => x.CuttingBatchId == cuttingBatchId && x.CompId == PortalContext.CurrentUser.CompId);
            foreach (PROD_LayCutting prodLayCutting in layCuttings)
            {
                start = 0;
                end = 0;
                for (int i = 0; i < prodLayCutting.Ratio; i++)
                {
                    start = start + end;

                    foreach (PROD_RollCutting rollCutting in rollCuttings)
                    {
                        rollStart = start + rollCutting.RollSart;
                        rollEnd = start + rollCutting.RollEnd;
                        end = rollCutting.RollEnd;
                        bundleCutting = new PROD_BundleCutting();
                        bundleCutting.CuttingBatchRefId = cuttingBatch.CuttingBatchRefId;
                        bundleCutting.CuttingBatchId = cuttingBatch.CuttingBatchId;
                        bundleCutting.CompId = cuttingBatch.CompId;
                        bundleCutting.RollNo = rollCutting.RollNo;
                        bundleCutting.SizeRefId = prodLayCutting.SizeRefId;
                        bundleCutting.XSC = Convert.ToString(i + 1).PadZero(2);
                        bundleCutting.ColorRefId = rollCutting.ColorRefId;
                        bundleCutting.ComponentRefId = partcutting.ComponentRefId;
                        bundleCutting.Quantity = rollCutting.Quantity;
                        bundleCutting.BundleStart = rollStart;
                        bundleCutting.BundleEnd = rollEnd;
                        bundleCutting.PSL = rollCutting.RollNo;
                        bundleCutting.SSL = prodLayCutting.LaySl;
                        bundleCutting.BatchNo = rollCutting.BatchNo;
                        bundleCutting.Combo = rollCutting.Combo;
                        bundleCutting.RollName = rollCutting.RollName;
                        bundleCutting.OrderStyleRefId = cuttingBatch.OrderStyleRefId;
                        bundleCuttings.Add(bundleCutting);
                    }



                }

            }
            return _bundleCuttingRepository.SaveList(bundleCuttings);
        }

        public int DeleteCuttingBatchByCuttingBatchId(long cuttingBatchId)
        {
            int index = 0;
            using (var transaction = new TransactionScope())
            {
               index +=_bundleCuttingRepository.Delete(
                    x => x.CuttingBatchId == cuttingBatchId && x.CompId == PortalContext.CurrentUser.CompId);
               index += _layCuttingRepository.Delete(
                    x => x.CuttingBatchId == cuttingBatchId && x.CompId == PortalContext.CurrentUser.CompId);
               index += _partCuttingRepository.Delete(
                    x => x.CuttingBatchId == cuttingBatchId && x.CompId == PortalContext.CurrentUser.CompId);
               index += _rollCuttingRepository.Delete(
                    x => x.CuttingBatchId == cuttingBatchId && x.CompId == PortalContext.CurrentUser.CompId);
                index +=_rejectAdjustmentRepository.Delete(
                    x => x.CuttingBatchId == cuttingBatchId && x.CompId == PortalContext.CurrentUser.CompId);
                index +=
                    _graddingRepository.Delete(
                        x => x.CuttingBatchId == cuttingBatchId && x.CompId == PortalContext.CurrentUser.CompId);
                index += rejectReplacementRepository.Delete(x => x.CuttingBatchId == cuttingBatchId && x.CompId == PortalContext.CurrentUser.CompId);
                 index += _cuttingBatchRepository.Delete(
                  x => x.CuttingBatchId == cuttingBatchId && x.CompId == PortalContext.CurrentUser.CompId);
                transaction.Complete();
            }
            return index;
        }

        public List<VwCuttingBatch> GetAllCuttingBatchList(string buyerRefId, string orderNo, string orderStyleRefId, string colorRefId, string componentRefId,string orderShipRefId)
        {
            IQueryable<VwCuttingBatch> cuttingBatchList =
           _cuttingBatchRepository
           .GetAllVwCuttingBatches(x => x.CompId == PortalContext.CurrentUser.CompId 
               && (x.BuyerRefId == buyerRefId) 
               && (x.OrderNo == orderNo || String.IsNullOrEmpty(orderNo)) 
               && (x.OrderStyleRefId == orderStyleRefId || String.IsNullOrEmpty(orderStyleRefId))
              && (x.StyleRefId == orderShipRefId || String.IsNullOrEmpty(orderShipRefId))
               && (x.ComponentRefId == componentRefId || componentRefId==null) 
               && (x.ColorRefId == colorRefId || String.IsNullOrEmpty(colorRefId)));

            return cuttingBatchList.ToList().OrderBy(x => x.JobNo).ToList();
          
           
        }

        public PROD_CuttingBatch GetCuttingBatchById(long cuttingBatchId)
        {
            return
               _cuttingBatchRepository.FindOne(
                   x =>x.CuttingBatchId == cuttingBatchId);
        }

        public List<PROD_CuttingBatch> GetJobNoByComponentRefId(string colorRefId, string componentRefId, string orderStyleRefId,string orderShipRefId)
        {
            return
                _cuttingBatchRepository.Filter(
                    x => x.CompId == PortalContext.CurrentUser.CompId &&x.ColorRefId==colorRefId&& x.ComponentRefId == componentRefId && x.OrderStyleRefId == orderStyleRefId&&(x.StyleRefId==orderShipRefId || string.IsNullOrEmpty(orderShipRefId))).ToList();
        }
        public bool IsOrderStyleRefIdExist(string orderStyleRefId)
        {
            string compId = PortalContext.CurrentUser.CompId;
            return _cuttingBatchRepository.Exists(x => x.CompId == compId && x.OrderStyleRefId == orderStyleRefId);
        }

        public List<VwCuttingApproval> GetCuttingApproval(string buyerRefId, string orderNo, string orderStyleRefId, string colorRefId,
            string componentRefId, string approvalStatus)
        {
            approvalStatus = approvalStatus ?? "P";
            return _cuttingBatchRepository.GetCuttingApproval(PortalContext.CurrentUser.CompId,buyerRefId,orderNo, orderStyleRefId,
                colorRefId, componentRefId, approvalStatus);
        }

        public List<VwCuttingBatch> GetCuttingBatchList(string buyerRefId, string orderNo, string orderStyleRefId, string colorRefId,
            string componentRefId, string cuttingBatchRefId,long cuttingBatchId)
        {
           IQueryable<VwCuttingBatch> cuttingBatchList =
        _cuttingBatchRepository
        .GetAllVwCuttingBatches(x => x.CompId == PortalContext.CurrentUser.CompId
            && (x.BuyerRefId == buyerRefId || buyerRefId==null)
            && (x.OrderNo == orderNo || String.IsNullOrEmpty(orderNo))
           && (x.OrderStyleRefId == orderStyleRefId || String.IsNullOrEmpty(orderStyleRefId))
           && (x.ComponentRefId == componentRefId || componentRefId == null)
            && (x.CuttingBatchRefId == cuttingBatchRefId || cuttingBatchRefId == null)
               && (x.CuttingBatchId == cuttingBatchId || cuttingBatchId ==0)
            && (x.ColorRefId == colorRefId || String.IsNullOrEmpty(colorRefId)));
            return cuttingBatchList.ToList();
        }

        public List<VwCuttingBatch> GetAllCuttingBatchForReport(DateTime? cuttingDate, string searchString,int? matchineId)
        {

            var cuttingBatchList = _cuttingBatchRepository.GetAllCuttingBatchListByPaging(cuttingDate, searchString, matchineId);
            cuttingBatchList = cuttingBatchList.OrderByDescending(r => r.CuttingBatchId);
            return cuttingBatchList.ToList();
        }

        public DataTable GetDailyMonthWiseCutting(int yearId, string compId)
        {
            string command = String.Format("exec SpDayMonthWiseCutting '{0}','{1}'", yearId, compId);
            DataTable cuttingDt = _cuttingBatchRepository.ExecuteQuery(command);
            return cuttingDt;
        }

        public List<VwCuttingBatch> GetAllCuttingBatchListByPaging(DateTime? cuttingDate, int? matchineId, string searchString, int pageIndex,
            out int totalRecords, out int totalBody)
        {
          
            var cuttingBatchList = _cuttingBatchRepository.GetAllCuttingBatchListByPaging(cuttingDate, searchString,matchineId);
            cuttingBatchList=cuttingBatchList.OrderByDescending(r => r.CuttingBatchId);
            totalBody = cuttingBatchList.Where(x => x.ComponentRefId ==PortalContext.CurrentUser.CompId).ToList().Sum(x => x.Total); 
            totalRecords = cuttingBatchList.Count();
           
            return cuttingBatchList.ToList();
        }

        public int SaveApprovalStatus(long cuttingBatchId)
        {
            var cuttingBatch =
                _cuttingBatchRepository.FindOne(
                    x => x.CompId == PortalContext.CurrentUser.CompId && x.CuttingBatchId == cuttingBatchId);
            cuttingBatch.ApprovalStatus = cuttingBatch.ApprovalStatus == "P" ? "A" : "P";
            return _cuttingBatchRepository.Edit(cuttingBatch);
        }
    }
}
