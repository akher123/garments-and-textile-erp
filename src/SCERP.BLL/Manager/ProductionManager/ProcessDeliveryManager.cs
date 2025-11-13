
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using SCERP.BLL.IManager.IPlanningManager;
using SCERP.BLL.IManager.IProductionManager;
using SCERP.Common;
using SCERP.DAL.IRepository.IMerchandisingRepository;
using SCERP.DAL.IRepository.IProductionRepository;
using SCERP.Model;
using SCERP.Model.Production;

namespace SCERP.BLL.Manager.ProductionManager
{
    public class ProcessDeliveryManager : IProcessDeliveryManager
    {
        private readonly ITimeAndActionManager _timeAndActionManager;
        private readonly IProcessDeliveryRepository _processDeliveryRepository;
        private readonly ICuttingBatchRepository _cuttingBatchRepository;
        private readonly IProcessDeliveryDetailRepository _deliveryDetailRepository;
        private readonly IOmSizeRepository _sizeRepository;
        private readonly IBuyOrdStyleColorRepository colorRepository;
        public ProcessDeliveryManager(IBuyOrdStyleColorRepository colorRepository, ITimeAndActionManager timeAndActionManager, IOmSizeRepository sizeRepository, ICuttingBatchRepository cuttingBatchRepository, IProcessDeliveryRepository processDeliveryRepository, IProcessDeliveryDetailRepository deliveryDetailRepository)
        {
            _sizeRepository = sizeRepository;
            _processDeliveryRepository = processDeliveryRepository;
            _deliveryDetailRepository = deliveryDetailRepository;
            _cuttingBatchRepository = cuttingBatchRepository;
            _timeAndActionManager = timeAndActionManager;
            this.colorRepository = colorRepository;
        }

        public List<PROD_ProcessDelivery> GetProcessDelivery(int pageIndex, out int totalRecords)
        {
            var pageSize = AppConfig.PageSize;
            IQueryable<PROD_ProcessDelivery> deliveries = _processDeliveryRepository.GetWithInclude(x => x.CompId == PortalContext.CurrentUser.CompId, "Party");
            totalRecords = deliveries.Count();
            deliveries = deliveries.OrderBy(x => x.ProcessDeliveryId).Skip(pageIndex * pageSize)
                     .Take(pageSize);
            return deliveries.ToList();
        }


        public int SaveProcessDelivery(PROD_ProcessDelivery delivery)
        {

            delivery.CompId = PortalContext.CurrentUser.CompId;
            delivery.InvoiceNo = delivery.RefNo;
            delivery.PreparedBy = PortalContext.CurrentUser.UserId.GetValueOrDefault();
            int saved = _processDeliveryRepository.Save(delivery);
            _timeAndActionManager.UpdateActualStartDate(TnaActivityKeyValue.PRINT_EM_SEND, delivery.InvDate, delivery.OrderStyleRefId, delivery.CompId);
            return saved;
        }

        public string GetPrintingDeliveryRefNo()
        {
            string refNo = _processDeliveryRepository.Filter(x => x.CompId == PortalContext.CurrentUser.CompId && x.ProcessRefId == ProcessCode.PRINTING).Max(x => x.RefNo.Substring(1, 7)) ?? "0";
            string newRefNo = "P" + refNo.IncrementOne().PadZero(7);
            return newRefNo;
        }


        public Dictionary<string, Dictionary<string, PROD_ProcessDeliveryDetail>> GetProcessDeliveryDictionary(long processDeliveryId)
        {
            var doDictionary = new Dictionary<string, Dictionary<string, PROD_ProcessDeliveryDetail>>();
            var processDelivery = _deliveryDetailRepository.Filter(x => x.CompId == PortalContext.CurrentUser.CompId && x.ProcessDeliveryId == processDeliveryId);

            var cuttingBatchList = processDelivery.Select(x => new { x.CuttingBatchId, x.CuttingTagId, x.ColorRefId }).Distinct();
            foreach (var cb in cuttingBatchList)
            {

                var cuttingBatchRefId = _cuttingBatchRepository.FindOne(x => x.CuttingBatchId == cb.CuttingBatchId).CuttingBatchRefId;
                doDictionary.Add(cuttingBatchRefId + "-" + cb.CuttingTagId + "C" + cb.ColorRefId, processDelivery.Where(x => x.CuttingBatchId == cb.CuttingBatchId && x.CuttingTagId == cb.CuttingTagId).ToList().ToDictionary(x => x.SizeRefId, x => x));
            }
            return doDictionary;
        }

        public PROD_ProcessDelivery GetProcessDeliveryById(long processDeliveryId)
        {
            return _processDeliveryRepository.FindOne(x => x.ProcessDeliveryId == processDeliveryId);
        }

        public Dictionary<string, List<string>> GetSizeNameDictionry(long processDeliveryId)
        {
            var sizeDictionary = new Dictionary<string, List<string>>();
            var processDelivery = _deliveryDetailRepository.Filter(x => x.CompId == PortalContext.CurrentUser.CompId && x.ProcessDeliveryId == processDeliveryId);
            var cuttingBatchList = processDelivery.Select(x => new { x.CuttingBatchId, x.CuttingTagId, x.ColorRefId }).Distinct();
            foreach (var cb in cuttingBatchList)
            {
                var sizelist = (from pdd in _deliveryDetailRepository.Filter(x => x.CompId == PortalContext.CurrentUser.CompId && x.CuttingBatchId == cb.CuttingBatchId && x.CuttingTagId == cb.CuttingTagId && x.ProcessDeliveryId == processDeliveryId).ToList()
                                join sz in _sizeRepository.Filter(x => x.CompId == PortalContext.CurrentUser.CompId).ToList() on pdd.SizeRefId equals sz.SizeRefId
                                select sz.SizeName).ToList();
                var cuttingBatchRefId = _cuttingBatchRepository.FindOne(x => x.CuttingBatchId == cb.CuttingBatchId).CuttingBatchRefId;
                sizelist.Add("Total");
                sizeDictionary.Add(cuttingBatchRefId + "-" + cb.CuttingTagId + "C" + cb.ColorRefId, sizelist);
            }
            return sizeDictionary;
        }

        public int EditProcessDelivery(PROD_ProcessDelivery delivery)
        {
            int edited = 0;
            using (var transaction = new TransactionScope())
            {
                var processDelivery = _processDeliveryRepository.FindOne(x => x.CompId == PortalContext.CurrentUser.CompId && x.ProcessDeliveryId == delivery.ProcessDeliveryId);
                processDelivery.InvDate = delivery.InvDate;
                processDelivery.EditedBy = PortalContext.CurrentUser.UserId;
                processDelivery.InvoiceNo = delivery.InvoiceNo;
                processDelivery.Remarks = delivery.Remarks;
                processDelivery.PartyId = delivery.PartyId;
                _deliveryDetailRepository.Delete(x => x.ProcessDeliveryId == delivery.ProcessDeliveryId && x.CompId == PortalContext.CurrentUser.CompId);
                edited += _processDeliveryRepository.Edit(processDelivery);
                edited += _deliveryDetailRepository.SaveList(delivery.PROD_ProcessDeliveryDetail.ToList());
                transaction.Complete();
            }

            return edited;


        }

        public int DeleteProcessDelivery(long processDeliveryId)
        {
            int deleted = 0;
            using (var transaction = new TransactionScope())
            {
                deleted += _deliveryDetailRepository.Delete(x => x.CompId == PortalContext.CurrentUser.CompId && x.ProcessDeliveryId == processDeliveryId);
                deleted += _processDeliveryRepository.Delete(x => x.CompId == PortalContext.CurrentUser.CompId && x.ProcessDeliveryId == processDeliveryId);
                transaction.Complete();
            }
            return deleted;
        }

        public List<VwProcessDelivery> GetProcessDelivery(int pageIndex, string processRefId, long partyId, string searchString, string orderStyleRefId, out int totalRecords)
        {
            var pageSize = AppConfig.PageSize;
            IQueryable<VwProcessDelivery> deliveries = _processDeliveryRepository.GetProcessDelivery(processRefId, PortalContext.CurrentUser.CompId);
            deliveries = deliveries.Where(x => (x.PartyId == partyId || partyId == 0) && (x.OrderStyleRefId == orderStyleRefId || String.IsNullOrEmpty(orderStyleRefId)) && (x.RefNo == searchString || searchString == null));
            totalRecords = deliveries.Count();
            deliveries = deliveries.OrderByDescending(x => x.ProcessDeliveryId).Skip(pageIndex * pageSize).Take(pageSize);
            return deliveries.ToList();
        }
        public string GetEmbroideryDeliveryRefNo()
        {
            string refNo = _processDeliveryRepository.Filter(x => x.CompId == PortalContext.CurrentUser.CompId && x.ProcessRefId == ProcessCode.EMBROIDARY).Max(x => x.RefNo.Substring(1, 7)) ?? "0";
            string newRefNo = "E" + refNo.IncrementOne().PadZero(7);
            return newRefNo;
        }
        public List<OM_Buyer> GetBuyerByPartyId(long partyId)
        {
            return _processDeliveryRepository.GetBuyerByPartyId(partyId, PortalContext.CurrentUser.CompId);
        }
        public Dictionary<string, List<string>> GetJobWiserBalanceDelivery(long cuttingBatchId, long cuttingTagId, string processRefId)
        {
            var pivotTable = new Dictionary<string, List<string>>();
            List<SpProdJobWiseRejectAdjusment> rejectAdjusments = _processDeliveryRepository.GetJobWiserBalanceDelivery(PortalContext.CurrentUser.CompId, cuttingBatchId, cuttingTagId, processRefId);

            if (rejectAdjusments.Any())
            {
                List<string> sizeList = rejectAdjusments.Select(x => x.SizeName).ToList();
                sizeList.Add("Total");
                List<string> quantityList = rejectAdjusments.Select(x => Convert.ToString(x.Quantity)).ToList();
                quantityList.Add(Convert.ToString(rejectAdjusments.Sum(x => x.Quantity)));
                List<string> sizeRefIdList = rejectAdjusments.Select(x => Convert.ToString(x.SizeRefId)).ToList();
                List<string> okQtyList = rejectAdjusments.Select(x => Convert.ToString(x.Quantity - x.RejectQty)).ToList();
                okQtyList.Add(Convert.ToString(rejectAdjusments.Sum(x => x.Quantity - x.RejectQty)));
                List<string> deliveryQtyList = rejectAdjusments.Select(x => Convert.ToString(x.TotalDelivery)).ToList();
                deliveryQtyList.Add(Convert.ToString(rejectAdjusments.Sum(x => x.TotalDelivery)));
                pivotTable = new Dictionary<string, List<string>>
            {
                {"Size", sizeList},
                {"Quantity", quantityList},
                {"SizeRefId", sizeRefIdList},
                {"Final Cutt", okQtyList},
                {"Total Sent", deliveryQtyList},
            };
            }

            return pivotTable;
        }

        public List<PartyWiseCuttiongProcess> GetPartyWiseCuttingDeliveryProcess(long partyId, string orderStyleRefId, string colorRefId, string componentRefId,
            bool isPrintable, bool isEmbroidery, string processCode)
        {
            componentRefId = componentRefId ?? "001";//"Body Component Code"
            List<PartyWiseCuttiongProcess> partyWiseCuttiongProcesses = _processDeliveryRepository.GetPartyWiseCuttingDeliveryProcess(PortalContext.CurrentUser.CompId, partyId, orderStyleRefId, colorRefId, componentRefId, isPrintable, isEmbroidery, processCode);
            return partyWiseCuttiongProcesses.Where(x => x.FinalCutt - x.TotalSent != 0).ToList();
        }

        public List<VwProcessDelivery> ProcessDeliverySummaryReport(string processRetId, string serachString, long partyId)
        {
            IQueryable<VwProcessDelivery> deliveries = _processDeliveryRepository.GetProcessDelivery(processRetId, PortalContext.CurrentUser.CompId);
            deliveries = deliveries.Where(x => (x.PartyId == partyId || partyId == 0) && (x.InvoiceNo == serachString || serachString == null));
            return deliveries.ToList();
        }

        public Dictionary<string, Dictionary<string, List<string>>> GetProcessStatusByStyle(string orderStyleRefId, string ptype)
        {
            var dic = new Dictionary<string, Dictionary<string, List<string>>>();
            var colors = colorRepository.GetBuyOrdStyleColor(orderStyleRefId, PortalContext.CurrentUser.CompId);
            foreach (var color in colors)
            {
                var dictionary = new Dictionary<string, List<string>>();
                var processStatus = _processDeliveryRepository.GetProcessStatusByStyleAndColor(orderStyleRefId, color.ColorRefId, ptype);
                List<string> sizeList = processStatus.Select(x => x.SizeName).ToList();
                sizeList.Add("TotalQty");
                List<string> ttlOrderQtyList = processStatus.Select(x => Convert.ToString(x.OrderQty)).ToList();
                ttlOrderQtyList.Add(Convert.ToString(processStatus.Sum(x => x.OrderQty)));
                List<string> ttlSentQty = processStatus.Select(x => Convert.ToString(x.SentQty)).ToList();
                ttlSentQty.Add(Convert.ToString(processStatus.Sum(x => x.SentQty)));
                List<string> invQty = processStatus.Select(x => Convert.ToString(x.InvQty)).ToList();
                invQty.Add(Convert.ToString(processStatus.Sum(x => x.InvQty)));

                List<string> fabRejectQty = processStatus.Select(x => Convert.ToString(x.FabReject)).ToList();
                fabRejectQty.Add(Convert.ToString(processStatus.Sum(x => x.FabReject)));
                List<string> processRejects = processStatus.Select(x => Convert.ToString(x.ProcesReject)).ToList();
                processRejects.Add(Convert.ToString(processStatus.Sum(x => x.ProcesReject)));
                List<string> actualQty = processStatus.Select(x => Convert.ToString(x.InvQty - x.FabReject - x.ProcesReject)).ToList();
                actualQty.Add(Convert.ToString(processStatus.Sum(x => x.InvQty - x.FabReject - x.ProcesReject)));
                List<string> pcts = processStatus.Select(x => x.SentQty > 0 ? String.Format("{0:0.00}" + " " + "%", ((x.InvQty - x.ProcesReject - x.FabReject) * 100.00m) /x.SentQty ) : "").ToList();
                pcts.Add(String.Format("{0:0.00}" + " " + "%", processStatus.Sum(x => x.SentQty)>0? processStatus.Sum(x => x.InvQty - x.ProcesReject - x.FabReject) * 100.0m / processStatus.Sum(x => x.SentQty):0));
                dictionary.Add("Size", sizeList);
                dictionary.Add("OrderQty", ttlOrderQtyList);
                dictionary.Add("Sent", ttlSentQty);
                dictionary.Add("InvQty", invQty);
                dictionary.Add("FabReject", fabRejectQty);
                dictionary.Add("Process", processRejects);
             
                dictionary.Add("Actual", actualQty);
                dictionary.Add("%", pcts);
                dic.Add(color.ColorName, dictionary);
            }
            return dic;
        }
    }
}
