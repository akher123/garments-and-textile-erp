using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using SCERP.BLL.IManager.IProductionManager;
using SCERP.Common;
using SCERP.DAL.IRepository.IMerchandisingRepository;
using SCERP.DAL.IRepository.IProductionRepository;
using SCERP.Model.Production;

namespace SCERP.BLL.Manager.ProductionManager
{
    public class SewingOutPutProcessManager : ISewingOutPutProcessManager
    {
        private readonly ISewingOutPutProcessRepository _sewingOutPutProcessRepository;
        private readonly ISewingOutputProcessDetailRepository _sewingOutputProcessDetailRepository;
        private readonly IBundleCuttingRepository _bundleCuttingRepository;
        private readonly IBuyOrdStyleColorRepository colorRepository;
        private readonly ISewingInputProcessRepository sewingInputProcessRepository;
        public SewingOutPutProcessManager(ISewingInputProcessRepository sewingInputProcessRepository,IBuyOrdStyleColorRepository colorRepository,IBundleCuttingRepository bundleCuttingRepository, ISewingOutPutProcessRepository sewingOutPutProcessRepository, ISewingOutputProcessDetailRepository sewingOutputProcessDetailRepository)
        {
            _sewingOutPutProcessRepository = sewingOutPutProcessRepository;
            _sewingOutputProcessDetailRepository = sewingOutputProcessDetailRepository;
            _bundleCuttingRepository = bundleCuttingRepository;
            this.colorRepository = colorRepository;
            this.sewingInputProcessRepository = sewingInputProcessRepository;
        }
        public string GetNewSewingOutputProcessRefId()
        {
            var maxSewingOutputProcessRefId = _sewingOutPutProcessRepository.Filter(x => x.CompId == PortalContext.CurrentUser.CompId).Max(x => x.SewingOutPutProcessRefId.Substring(2)) ?? "0";
            return "SO" + maxSewingOutputProcessRefId.IncrementOne().PadZero(8);
        }

        public List<VwSewingOutputProcess> GetSewingOutputProcessByStyleColor(string orderStyleRefId, string colorRefId, string orderShipRefId)
        {
            return _sewingOutPutProcessRepository.GetSewingOutputProcessByStyleColor(PortalContext.CurrentUser.CompId, orderStyleRefId, colorRefId, orderShipRefId);
        }

        public int EditSewingOutputProcess(PROD_SewingOutPutProcess model)
        {
            int index = 0;

            using (var transaction = new TransactionScope())
            {
                index += _sewingOutputProcessDetailRepository.Delete(x => x.CompId == model.CompId && x.SewingOutPutProcessId == model.SewingOutPutProcessId);
                model.PROD_SewingOutPutProcessDetail = model.PROD_SewingOutPutProcessDetail.Select(x =>
                {
                    x.SewingOutPutProcessId = model.SewingOutPutProcessId;
                    return x;
                }).ToList();
                index += _sewingOutputProcessDetailRepository.SaveList(model.PROD_SewingOutPutProcessDetail.ToList());
                PROD_SewingOutPutProcess sewingOutputProcess = _sewingOutPutProcessRepository.FindOne(x => x.CompId == model.CompId && x.SewingOutPutProcessId == model.SewingOutPutProcessId);
                string time = DateTime.Now.ToString("h:mm:ss tt");
                sewingOutputProcess.OutputDate = model.OutputDate.ToMargeDateAndTime(time);
                sewingOutputProcess.OutputDate = model.OutputDate;
                sewingOutputProcess.LineId = model.LineId;
                sewingOutputProcess.HourId = model.HourId;
                sewingOutputProcess.ManPower = model.ManPower;
                sewingOutputProcess.Remarks = model.Remarks;
                index += _sewingOutPutProcessRepository.Edit(sewingOutputProcess);
                transaction.Complete();
            }
            _sewingOutPutProcessRepository.ExecuteQuery(String.Format("EXEC spMisHourlyAchievement '{0}','{1}'",
          PortalContext.CurrentUser.CompId, DateTime.Now));
          
            return index;
        }

        public int SaveSewingOutputProcess(PROD_SewingOutPutProcess model)
        {
            int saveIndex = 0;
            string time = DateTime.Now.ToString("h:mm:ss tt");
            model.OutputDate = model.OutputDate.ToMargeDateAndTime(time);
            model.SewingOutPutProcessRefId = GetNewSewingOutputProcessRefId();
            saveIndex = _sewingOutPutProcessRepository.Save(model);
            _sewingOutPutProcessRepository.ExecuteQuery(String.Format("EXEC spMisHourlyAchievement '{0}','{1}'",
                PortalContext.CurrentUser.CompId, DateTime.Now));
                 return saveIndex;
        }

        public int DeleteSewingOutputProcess(long sewingOutPutProcessId, string compId)
        {
            int deleted = 0;
            using (var transaction = new TransactionScope())
            {
                deleted += _sewingOutputProcessDetailRepository.Delete(x => x.CompId == compId && x.SewingOutPutProcessId == sewingOutPutProcessId);
                deleted += _sewingOutPutProcessRepository.Delete(x => x.CompId == compId && x.SewingOutPutProcessId == sewingOutPutProcessId);
                transaction.Complete();
            }
            return deleted;
        }

        public PROD_SewingOutPutProcess GetSewintOutputProcessBySewingOutputProcessId(long sewingOutPutProcessId, string compId)
        {
            return _sewingOutPutProcessRepository.FindOne(x => x.SewingOutPutProcessId == sewingOutPutProcessId && x.CompId == compId);
        }

        public List<VwSewingOutput> GetAllSewingOutputInfo(long sewingOutPutProcessId, string compId)
        {
            List<VwSewingOutput> sewingOutputProcessDetails = _sewingOutPutProcessRepository.GetAllSewingOutputInfo(sewingOutPutProcessId, compId);
            return sewingOutputProcessDetails.ToList();
        }

        public List<VwSewingOutputProcess> GetDailySewingOut(int pageIndex, DateTime outputDate, int lineId, out int totalRecord, out long totalQty)
        {
            int pageSize = AppConfig.PageSize+10;
            string compId = PortalContext.CurrentUser.CompId;
            IQueryable<VwSewingOutputProcess> sewingOutputProcesses = _sewingOutPutProcessRepository.GetDailySewingOut(compId, outputDate, lineId);
            sewingOutputProcesses = sewingOutputProcesses.OrderBy(x => x.HourId);
            totalRecord = sewingOutputProcesses.Count();
            totalQty = (long) sewingOutputProcesses.ToList().Sum(x => x.OutputQuantity);
            //Don't delete: IF Paging need only active the comenting code
            //sewingOutputProcesses = sewingOutputProcesses
            //            .OrderBy(r => r.HourId)
            //            .Skip(pageIndex * pageSize)
            //            .Take(pageSize);
            return sewingOutputProcesses.ToList();
        }

        public List<VwSewingOutputProcess> GetDailySewingOutForReport(DateTime outputDate, int lineId)
        {
            string compId = PortalContext.CurrentUser.CompId;
            IQueryable<VwSewingOutputProcess> sewingOutputProcesses = _sewingOutPutProcessRepository.GetDailySewingOutForReport(compId, outputDate, lineId);
            return sewingOutputProcesses.ToList();
        }

        public DataTable GetSewingWIP(DateTime outputDate, int hourId, string compId)
        {
            return _sewingOutPutProcessRepository.GetSewingWIP(outputDate,hourId,compId);
        }

        public bool IsSewingOutputExist(PROD_SewingOutPutProcess model)
        {
            model.CompId = PortalContext.CurrentUser.CompId;
            return
                _sewingOutPutProcessRepository.Exists(
                    x =>
                        x.OutputDate == model.OutputDate && x.OrderStyleRefId == model.OrderStyleRefId &&
                        x.ColorRefId == model.ColorRefId && x.LineId == model.LineId && x.HourId == model.HourId &&
                        x.SewingOutPutProcessId != model.SewingOutPutProcessId && x.CompId==model.CompId);
        }

        public DataTable GetSewingWIPDetail(DateTime outputDate, string compId)
        {
            return _sewingOutPutProcessRepository.GetSewingWIPDetail(outputDate, compId);
        }

        public DataTable GetHourlyProduction(DateTime prodDate,string compId)
        {
            return _sewingOutPutProcessRepository.GetHourlyProduction(prodDate,compId);
        }

        public VwProductionForecast ProductionForecast(DateTime currentDate, string compId)
        {
            return _sewingOutPutProcessRepository.ProductionForecast(currentDate, compId);
        }

        public DataTable GetSewingWIPSummary(DateTime outputDate, string compId)
        {
            return _sewingOutPutProcessRepository.GetSewingWIPSummary(outputDate, compId);
        }

        public int GetTotalProductionHours(DateTime outputDate, string compId)
        {
            return _sewingOutPutProcessRepository.GetTotalProductionHours(outputDate, compId);
              
        }

        public VwProductionForecast ProductionForecastLastMonth(DateTime addMonths, string compId)
        {
            return _sewingOutPutProcessRepository.ProductionForecastLastMonth(addMonths, compId);
        }

        public List<VwSewingOutputProcess> GetDailySewingOutData(DateTime date, int lineId, string compId)
        {
            IQueryable<VwSewingOutputProcess> sewingOutputProcesses = _sewingOutPutProcessRepository.GetDailySewingOut(compId, date, lineId);
            return sewingOutputProcesses.OrderByDescending(x => x.SewingOutPutProcessId).ToList();
        }

        public int SaveBarcodeSewingOutputProcess(PROD_SewingOutPutProcess sewingOutPutProcess)
        {

            long bundleId = Convert.ToInt64(sewingOutPutProcess.JobNo);

            PROD_BundleCutting bundleCutting =
                _bundleCuttingRepository.All()
                    .Include(x => x.PROD_CuttingBatch)
                    .FirstOrDefault(x => x.BundleCuttingId == bundleId);
            sewingOutPutProcess.CompId = PortalContext.CurrentUser.CompId;
            sewingOutPutProcess.BuyerRefId = bundleCutting.PROD_CuttingBatch.BuyerRefId;
            sewingOutPutProcess.OrderNo = bundleCutting.PROD_CuttingBatch.OrderNo;
            sewingOutPutProcess.OrderStyleRefId = bundleCutting.PROD_CuttingBatch.OrderStyleRefId;
            sewingOutPutProcess.ColorRefId = bundleCutting.PROD_CuttingBatch.ColorRefId;
            sewingOutPutProcess.SewingOutPutProcessRefId = GetNewSewingOutputProcessRefId();
            sewingOutPutProcess.PreparedBy = PortalContext.CurrentUser.UserId.GetValueOrDefault();

            sewingOutPutProcess.PROD_SewingOutPutProcessDetail = new List<PROD_SewingOutPutProcessDetail>();
            sewingOutPutProcess.PROD_SewingOutPutProcessDetail.Add(new PROD_SewingOutPutProcessDetail()
            {
                CompId = sewingOutPutProcess.CompId,
                Quantity = bundleCutting.Quantity.GetValueOrDefault(),
                SizeRefId = bundleCutting.SizeRefId
            });

            return _sewingOutPutProcessRepository.Save(sewingOutPutProcess);
        }

        public Dictionary<string, Dictionary<string, List<string>>> GetSewingDictionaryByStyle(string orderStyleRefId)
        {

            var dic = new Dictionary<string, Dictionary<string, List<string>>>();
            List<VwSewingOutput> sewingOutputList = sewingInputProcessRepository.GetVwSewingInput(PortalContext.CurrentUser.CompId, orderStyleRefId);
            var colors = colorRepository.GetBuyOrdStyleColor(orderStyleRefId, PortalContext.CurrentUser.CompId);
            foreach (var color in colors)
            {
                var dictionary = new Dictionary<string, List<string>>();
                var sewingOutputs = sewingOutputList.Where(x => x.ColorRefId == color.ColorRefId && x.OrderStyleRefId == orderStyleRefId);
                List<string> sizeList = sewingOutputs.Select(x => x.SizeName).ToList();
                sizeList.Add("TotalQty");
                List<string> sizeRefIdList = sewingOutputs.Select(x => x.SizeRefId).ToList();
                List<string> orderQtyList = sewingOutputs.Select(x => Convert.ToString(x.OrderQty)).ToList();
                orderQtyList.Add(Convert.ToString(sewingOutputs.Sum(x => x.OrderQty)));
                List<string> totalInputList = sewingOutputs.Select(x => Convert.ToString(x.TotalInput)).ToList();
                totalInputList.Add(Convert.ToString(sewingOutputs.Sum(x => x.TotalInput)));
                List<string> totalOutputQtylist = sewingOutputs.Select(x => Convert.ToString(x.TotalOutput)).ToList();
                totalOutputQtylist.Add(Convert.ToString(sewingOutputs.Sum(x => x.TotalOutput)));
                List<string> outputPercentList = sewingOutputs.Select(x => x.OrderQty > 0 ? String.Format("{0:0.00}" + " " + "%", (x.TotalOutput * 100.00m) / x.OrderQty) : "0").ToList();
                outputPercentList.Add(String.Format("{0:0.00}" + " " + "%", sewingOutputs.Sum(x => x.TotalOutput) * 100.0m / sewingOutputs.Sum(x => x.OrderQty)));
                dictionary.Add("Size", sizeList);
                dictionary.Add("OrderQty", orderQtyList);
                dictionary.Add("InputQty", totalInputList);
                dictionary.Add("TOutputQty", totalOutputQtylist);
                dictionary.Add("Output(%)", outputPercentList);
                dic.Add(color.ColorName, dictionary);
            }
            return dic;

         
        }

        public string GetLastSwingOutputDateTime(string compId)
        {
            string ledate= _sewingOutPutProcessRepository.GetLastSwingOutputDateTime(compId);
            if (string.IsNullOrEmpty(ledate))
            {
                ledate = "SEWING NOT STRAT";
            }
            return ledate;
        }
    }
}
