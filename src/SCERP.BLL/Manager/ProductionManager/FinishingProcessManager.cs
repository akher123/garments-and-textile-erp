using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using SCERP.BLL.IManager.IPlanningManager;
using SCERP.BLL.IManager.IProductionManager;
using SCERP.Common;
using SCERP.DAL.IRepository.IAccountingRepository;
using SCERP.DAL.IRepository.IMerchandisingRepository;
using SCERP.DAL.IRepository.IProductionRepository;
using SCERP.Model.Production;

namespace SCERP.BLL.Manager.ProductionManager
{
    public class FinishingProcessManager : IFinishingProcessManager
    {
        private readonly IFinishingProcessRepository _finishingProcessRepository;
        private readonly IFinishingProcessDetailRepository _finishingProcessDetailRepository;
        private readonly ITimeAndActionManager _timeAndActionManager;
        private readonly IBuyOrdStyleColorRepository colorRepository;
        public string _compId;
        public FinishingProcessManager(IBuyOrdStyleColorRepository colorRepository,ITimeAndActionManager timeAndActionManager, IFinishingProcessRepository finishingProcessRepository, IFinishingProcessDetailRepository finishingProcessDetailRepository)
        {
            _finishingProcessRepository = finishingProcessRepository;
            _finishingProcessDetailRepository = finishingProcessDetailRepository;
            _timeAndActionManager = timeAndActionManager;
            _compId = PortalContext.CurrentUser.CompId;
            this.colorRepository = colorRepository;
        }

        public string GetNewFinishingProcessRefId(int finishType, string prifix)
        {
            var maxRefSl = _finishingProcessRepository.Filter(
                  x =>
                      x.CompId == _compId && x.FType == finishType &&
                      x.FinishingProcessRefId.Substring(0, prifix.Length).Equals(prifix))
                  .Max(x => x.FinishingProcessRefId.Substring(prifix.Length, 10));

            var finishingProcessRefId = prifix + maxRefSl.IncrementOne().PadZero(8);

            return finishingProcessRefId;
        }

        public List<VwFinishingProcessDetail> GetFinishingProcessDetailByStyleColor(string orderStyleRefId, string colorRefId, long finishType)
        {
            return _finishingProcessRepository.GetFinishingProcessDetailByStyleColor(_compId, orderStyleRefId, colorRefId, finishType);
        }

        public List<VwFinishingProcess> GetSewingInputProcessByStyleColor(string orderStyleRefId, string colorRefId, int finishType)
        {
            return _finishingProcessRepository.GetSewingInputProcessByStyleColor(_compId, orderStyleRefId, colorRefId, finishType);
        }

        public int EditFinishingProcess(PROD_FinishingProcess model)
        {
            var edited = 0;
            using (var transaction = new TransactionScope())
            {
                _finishingProcessDetailRepository.Delete(x => x.FinishingProcessId == model.FinishingProcessId);

                foreach (var finishDetail in model.PROD_FinishingProcessDetail)
                {
                    finishDetail.CompId = PortalContext.CurrentUser.CompId;
                    finishDetail.FinishingProcessId = model.FinishingProcessId;
                    edited += _finishingProcessDetailRepository.Save(finishDetail);
                }
                PROD_FinishingProcess finishingProcess = _finishingProcessRepository.FindOne(x => x.CompId == model.CompId && x.FinishingProcessId == model.FinishingProcessId);
                finishingProcess.InputDate = model.InputDate;
                finishingProcess.HourId = model.HourId;

                finishingProcess.Remarks = model.Remarks;
                edited += _finishingProcessRepository.Edit(finishingProcess);
                transaction.Complete();
            }
            return edited;
        }

        public int SaveFinishingProcess(PROD_FinishingProcess finishingProcess)
        {
            finishingProcess.CompId = PortalContext.CurrentUser.CompId;
            int saved = _finishingProcessRepository.Save(finishingProcess);
            if (saved > 0)
            {
                _timeAndActionManager.UpdateActualStartDate(TnaActivityKeyValue.SEWING_FINISHING, finishingProcess.InputDate.GetValueOrDefault(), finishingProcess.OrderStyleRefId, finishingProcess.CompId);
            }
            return saved;

        }



        public PROD_FinishingProcess GeFabricProcessById(long finishingProcessId)
        {
            return _finishingProcessRepository.FindOne(x => x.FinishingProcessId == finishingProcessId);
        }

        public List<VwFinishingProcessDetail> GetPolyFinishingProcessDetailByStyleColor(string orderStyleRefId, string colorRefId, long finishingProcessId)
        {
            return _finishingProcessRepository.GetPolyFinishingProcessDetailByStyleColor(_compId, orderStyleRefId, colorRefId,
                finishingProcessId);
        }

        public int DeleteFinishingProcess(long finishingProcessId)
        {
            var deleted = 0;
            using (var transaction = new TransactionScope())
            {
                _finishingProcessDetailRepository.Delete(x => x.FinishingProcessId == finishingProcessId);
                deleted = _finishingProcessRepository.Delete(x => x.FinishingProcessId == finishingProcessId);
                transaction.Complete();
            }
            return deleted;
        }

        public List<VwFinishingProcess> GetFinishingProcess(DateTime? inputDate, int finishType)
        {
            return
                _finishingProcessRepository.GetFinishingProcess(inputDate, finishType, _compId);
        }

        public Dictionary<string, Dictionary<string, List<string>>> GetFinishingDictionaryByStyle(string orderStyleRefId)
        {

            var dic = new Dictionary<string, Dictionary<string, List<string>>>();
            var colors = colorRepository.GetBuyOrdStyleColor(orderStyleRefId, PortalContext.CurrentUser.CompId);
            foreach (var color in colors)
            {
                var dictionary = new Dictionary<string, List<string>>();
                var finishDetails = _finishingProcessRepository.GetFinishingProcessDetailByStyleColorStatus(_compId, orderStyleRefId, color.ColorRefId, (int)FinishType.Poly);
                List<string> sizeList = finishDetails.Select(x => x.SizeName).ToList();
                sizeList.Add("TotalQty");
                List<string> ttlOrderQtyList = finishDetails.Select(x => Convert.ToString(x.TtlOrderQty)).ToList();
                ttlOrderQtyList.Add(Convert.ToString(finishDetails.Sum(x => x.TtlOrderQty)));
                List<string> ttlCuttQty = finishDetails.Select(x => Convert.ToString(x.TotalCuttQty)).ToList();
                ttlCuttQty.Add(Convert.ToString(finishDetails.Sum(x => x.TotalCuttQty)));
                List<string> swInputQtyList = finishDetails.Select(x => Convert.ToString(x.TtlSwInputQty)).ToList();
                swInputQtyList.Add(Convert.ToString(finishDetails.Sum(x => x.TtlSwInputQty)));

                List<string> swoutputQtyList = finishDetails.Select(x => Convert.ToString(x.TtlSwOutQty)).ToList();
                swoutputQtyList.Add(Convert.ToString(finishDetails.Sum(x => x.TtlSwOutQty)));

                List<string> ttlSwOutQtyList = finishDetails.Select(x => Convert.ToString(x.TinQuantity)).ToList();
                ttlSwOutQtyList.Add(Convert.ToString(finishDetails.Sum(x => x.TinQuantity)));

                List<string> inputPercentList = finishDetails.Select(x => x.TtlOrderQty > 0 ? String.Format("{0:0.00}" + " " + "%", (x.TinQuantity * 100.00m) / x.TtlOrderQty) : "0").ToList();
                inputPercentList.Add(String.Format("{0:0.00}" + " " + "%", finishDetails.Sum(x => x.TinQuantity) * 100.0m / finishDetails.Sum(x => x.TtlOrderQty)));

                dictionary.Add("Size", sizeList);
                dictionary.Add("OrderQty", ttlOrderQtyList);
                dictionary.Add("TtlCutting", ttlCuttQty);
                dictionary.Add("SewingInQty", swInputQtyList);
                dictionary.Add("SewingOutQty", swoutputQtyList);
                dictionary.Add("IronQty", swoutputQtyList);
                dictionary.Add("TotalPoly", ttlSwOutQtyList);
                dictionary.Add("Poly %", inputPercentList);
                dic.Add(color.ColorName, dictionary);
            }
            return dic;
        }
    }
}
