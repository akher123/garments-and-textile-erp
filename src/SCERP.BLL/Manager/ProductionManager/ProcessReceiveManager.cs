using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using SCERP.BLL.IManager.IPlanningManager;
using SCERP.BLL.IManager.IProductionManager;
using SCERP.Common;
using SCERP.DAL.IRepository.IMerchandisingRepository;
using SCERP.DAL.IRepository.IProductionRepository;
using SCERP.Model.Production;

namespace SCERP.BLL.Manager.ProductionManager
{
    public class ProcessReceiveManager : IProcessReceiveManager
    {
        private readonly ITimeAndActionManager _timeAndActionManager;
        private readonly IProcessReceiveRepository _processReceiveRepository;
        private readonly IProcessReceiveDetailRepository _processReceiveDetailRepository;
        private readonly IOmSizeRepository _sizeRepository;
        private readonly ICuttingBatchRepository _cuttingBatchRepository;
        public ProcessReceiveManager(ITimeAndActionManager timeAndActionManager, ICuttingBatchRepository cuttingBatchRepository, IOmSizeRepository sizeRepository, IProcessReceiveRepository processReceiveRepository, IProcessReceiveDetailRepository processReceiveDetailRepository)
        {
            _cuttingBatchRepository = cuttingBatchRepository;
            _sizeRepository = sizeRepository;
            this._processReceiveRepository = processReceiveRepository;
            this._processReceiveDetailRepository = processReceiveDetailRepository;
            this._timeAndActionManager = timeAndActionManager;
        }

        public List<SpPodProcessReceiveBalance> GetProcessReceiveBalance(string printing, long partyId, string cuttingBatchRefId)
        {

            List<SpPodProcessReceiveBalance> receiveBalances = _processReceiveRepository.GetProcessReceiveBalance(printing, partyId, cuttingBatchRefId ?? "C");
            return receiveBalances;
        }

        public string GetProcessReceiveRefNo(string prifix, string processRefId)
        {
            string refNo = _processReceiveRepository.Filter(x => x.CompId == PortalContext.CurrentUser.CompId && x.ProcessRefId == processRefId).Max(x => x.RefNo.Substring(2, 7)) ?? "0";
            string newRefNo = prifix + refNo.IncrementOne().PadZero(6);
            return newRefNo;
        }

        public Dictionary<string, List<string>> GetPrintReciveBalanceDictionary(long cuttingBatchId, long cuttingTagId, string printing)
        {
            var pivotTable = new Dictionary<string, List<string>>();
            List<SpPodProcessReceiveBalance> receiveBalances = _processReceiveRepository.GetReceiveBalance(cuttingBatchId, cuttingTagId, printing);
            if (receiveBalances.Any())
            {
                List<string> sizeList = receiveBalances.Select(x => x.SizeName).ToList();
                sizeList.Add("TOTAL");
                List<string> sendquantityList = receiveBalances.Select(x => Convert.ToString(x.SendQuantity)).ToList();
                sendquantityList.Add(Convert.ToString(receiveBalances.Sum(x => x.SendQuantity)));
                List<string> sizeRefIdList = receiveBalances.Select(x => Convert.ToString(x.SizeRefId)).ToList();
                List<string> balanceQtyList = receiveBalances.Select(x => Convert.ToString(x.SendQuantity - x.RecvQuantity)).ToList();
                balanceQtyList.Add(Convert.ToString(receiveBalances.Sum(x => x.SendQuantity - x.RecvQuantity)));
                List<string> receiveQtyList = receiveBalances.Select(x => Convert.ToString(x.RecvQuantity)).ToList();
                receiveQtyList.Add(Convert.ToString(receiveBalances.Sum(x => x.RecvQuantity)));
                pivotTable = new Dictionary<string, List<string>>
            {
                {"SIZE", sizeList},
                {"SEND QTY", sendquantityList},
                {"SizeRefId", sizeRefIdList},
                {"RECV.QTY", receiveQtyList},
                {"BALANCE QTY", balanceQtyList},
            };
            }
            return pivotTable;

        }

        public List<PROD_ProcessReceive> GetProcessReceiveLsitByPaging(int pageIndex, string sort, string sortdir, string searchString, long partyId, string processRefId,
            out int totalRecords)
        {
            var pageSize = AppConfig.PageSize;
            IQueryable<PROD_ProcessReceive> processReceives = _processReceiveRepository
                .Filter(
                    x =>
                        x.ProcessRefId == processRefId && x.CompId == PortalContext.CurrentUser.CompId &&
                        (x.PartyId == partyId || partyId == 0) &&
                        ((x.RefNo.ToLower().Contains(searchString.ToLower()) || String.IsNullOrEmpty(searchString)) ||
                        (x.InvoiceNo.ToLower().Contains(searchString.ToLower()) || String.IsNullOrEmpty(searchString))));
            totalRecords = processReceives.Count();
            processReceives = processReceives.OrderByDescending(x => x.RefNo).Skip(pageIndex * pageSize).Take(pageSize);
            return processReceives.ToList();
        }

        public int SaveProcessReceive(PROD_ProcessReceive receive)
        {
            receive.Posted = POSTED.N.ToString();
            var rcvDtl = receive.PROD_ProcessReceiveDetail.FirstOrDefault();
            int saved = _processReceiveRepository.Save(receive);
            if (rcvDtl != null)// Print Emb Actual Received Start Date update to TNA
            {
                PROD_CuttingBatch btcutting = _cuttingBatchRepository.FindOne(x => x.CuttingBatchId == rcvDtl.CuttingBatchId);
                if (btcutting != null)
                {
                    _timeAndActionManager.UpdateActualStartDate(TnaActivityKeyValue.PRINT_EM_RCV, receive.InvoiceDate.GetValueOrDefault(), btcutting.OrderStyleRefId, receive.CompId);
                }
            }
            return saved;


        }

        public int EditProcessReceive(PROD_ProcessReceive receive)
        {
            int edited = 0;
            using (var transaction = new TransactionScope())
            {

                PROD_ProcessReceive processReceive = _processReceiveRepository.FindOne(x => x.ProcessReceiveId == receive.ProcessReceiveId);
                processReceive.Posted = POSTED.N.ToString();
                processReceive.InvoiceDate = receive.InvoiceDate;
                processReceive.InvoiceNo = receive.InvoiceNo;
                processReceive.GateEntrydate = receive.GateEntrydate;
                processReceive.GateEntryNo = receive.GateEntryNo;
                processReceive.Remarks = receive.Remarks;
                _processReceiveDetailRepository.Delete(x => x.ProcessReceiveId == processReceive.ProcessReceiveId);
                _processReceiveDetailRepository.SaveList(receive.PROD_ProcessReceiveDetail.ToList());
                edited = _processReceiveRepository.Edit(processReceive);
                transaction.Complete();
            }
            return edited;

        }



        public PROD_ProcessReceive GetProcessReceiveById(long processReceiveId)
        {
            return _processReceiveRepository.FindOne(x => x.ProcessReceiveId == processReceiveId);
        }

        public Dictionary<string, Dictionary<string, PROD_ProcessReceiveDetail>> GetProcessReceiveDetailDictionary(long processReceiveId)
        {
            var doDictionary = new Dictionary<string, Dictionary<string, PROD_ProcessReceiveDetail>>();
            var processDelivery = _processReceiveDetailRepository.Filter(x => x.CompId == PortalContext.CurrentUser.CompId && x.ProcessReceiveId == processReceiveId);
            var cuttingBatchList = processDelivery.Select(x => new { x.CuttingBatchId, x.CuttingTagId, x.ColorRefId }).Distinct();
            foreach (var cb in cuttingBatchList)
            {
                var cuttingBatchRefId = _cuttingBatchRepository.FindOne(x => x.CuttingBatchId == cb.CuttingBatchId).CuttingBatchRefId;
                doDictionary.Add(cuttingBatchRefId + "-" + cb.CuttingTagId + "C" + cb.ColorRefId, processDelivery.Where(x => x.CuttingBatchId == cb.CuttingBatchId && x.CuttingTagId == cb.CuttingTagId).ToList().ToDictionary(x => x.SizeRefId, x => x));
            }
            return doDictionary;
        }

        public Dictionary<string, List<string>> GetReceiveDictionary(long processReceiveId)
        {
            var sizeDictionary = new Dictionary<string, List<string>>();
            var processRecive = _processReceiveDetailRepository.Filter(x => x.CompId == PortalContext.CurrentUser.CompId && x.ProcessReceiveId == processReceiveId);
            var cuttingBatchList = processRecive.Select(x => new { x.CuttingBatchId, x.CuttingTagId, x.ColorRefId }).Distinct();
            foreach (var cb in cuttingBatchList)
            {
                var sizelist = (from pdd in _processReceiveDetailRepository.Filter(x => x.CompId == PortalContext.CurrentUser.CompId && x.CuttingBatchId == cb.CuttingBatchId && x.CuttingTagId == cb.CuttingTagId && x.ProcessReceiveId == processReceiveId).ToList()
                                join sz in _sizeRepository.Filter(x => x.CompId == PortalContext.CurrentUser.CompId).ToList() on pdd.SizeRefId equals sz.SizeRefId
                                select sz.SizeName).ToList();
                var cuttingBatchRefId = _cuttingBatchRepository.FindOne(x => x.CuttingBatchId == cb.CuttingBatchId).CuttingBatchRefId;
                sizelist.Add("Total");
                sizeDictionary.Add(cuttingBatchRefId + "-" + cb.CuttingTagId + "C" + cb.ColorRefId, sizelist);
            }
            return sizeDictionary;
        }

        public int DeleteProcessReceiveById(long processReceiveId)
        {
            int deleted = 0;
            using (var transaction = new TransactionScope())
            {
                _processReceiveDetailRepository.Delete(x => x.ProcessReceiveId == processReceiveId && x.CompId == PortalContext.CurrentUser.CompId);
                deleted = _processReceiveRepository.Delete(x => x.ProcessReceiveId == processReceiveId && x.CompId == PortalContext.CurrentUser.CompId);
                transaction.Complete();
            }

            return deleted;
        }
    }
}
