using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Transactions;
using CrystalDecisions.Shared;
using Microsoft.Win32;
using SCERP.BLL.IManager.IPlanningManager;
using SCERP.BLL.IManager.IProductionManager;
using SCERP.Common;
using SCERP.DAL.IRepository;
using SCERP.DAL.IRepository.IProductionRepository;
using SCERP.Model.Production;

namespace SCERP.BLL.Manager.ProductionManager
{
    public class SewingInputProcessManager : ISewingInputProcessManager
    {
        private readonly ITimeAndActionManager _timeAndActionManager;
        private readonly ISewingInputProcessRepository _sewingInputProcessRepository;
        private readonly ISewingInputProcessDetailRepository _sewingInputProcessDetailRepository;
        private readonly ICutBankRepository _cutBankRepository;
        private readonly IBundleCuttingRepository _bundleCuttingRepository;
     
        public SewingInputProcessManager(ITimeAndActionManager timeAndActionManager,IBundleCuttingRepository bundleCuttingRepository,ISewingInputProcessRepository sewingInputProcessRepository, ISewingInputProcessDetailRepository sewingInputProcessDetailRepository)
        {
            _sewingInputProcessRepository = sewingInputProcessRepository;
            _sewingInputProcessDetailRepository = sewingInputProcessDetailRepository;
            _bundleCuttingRepository = bundleCuttingRepository;
            _timeAndActionManager = timeAndActionManager;
        }
        public string GetNewSewingInputProcessRefId()
        {
           var maxSewingInputProcessRefId = _sewingInputProcessRepository.Filter(x=>x.CompId==PortalContext.CurrentUser.CompId).Max(x => x.SewingInputProcessRefId.Substring(2))??"0";
           return "SI"+maxSewingInputProcessRefId.IncrementOne().PadZero(8);

        }
        public int SaveSewingInputProcess(PROD_SewingInputProcess model)
        {
            int saveIndex = 0;
            model.SewingInputProcessRefId = GetNewSewingInputProcessRefId();
            model.Locked = false;
            saveIndex = _sewingInputProcessRepository.Save(model);
            if (saveIndex > 0)
            {
                // TNA Actual Start Date Update 
                _timeAndActionManager.UpdateActualStartDate(TnaActivityKeyValue.BULKSEWING, model.InputDate.GetValueOrDefault(), model.OrderStyleRefId, model.CompId);
            }
            return saveIndex;
        }
        public int EditSewingInputProcess(PROD_SewingInputProcess model)
        {
            int edited = 0;
            using (var transaction = new TransactionScope())
            {
               foreach (var sewingInputDetail in model.PROD_SewingInputProcessDetail)
                {
                    PROD_SewingInputProcessDetail detail =
                        _sewingInputProcessDetailRepository.FindOne(
                            x => x.CompId == sewingInputDetail.CompId && x.SewingInputProcessId == model.SewingInputProcessId && x.SizeRefId == sewingInputDetail.SizeRefId);
                    detail.InputQuantity=sewingInputDetail.InputQuantity;
                    edited += _sewingInputProcessDetailRepository.Edit(detail);
                }
               PROD_SewingInputProcess sewingInputProcess = _sewingInputProcessRepository.FindOne(x => x.CompId == model.CompId && x.SewingInputProcessId == model.SewingInputProcessId);
               sewingInputProcess.InputDate = model.InputDate;
               sewingInputProcess.LineId = model.LineId;
               sewingInputProcess.BatchNo = model.BatchNo;
               sewingInputProcess.JobNo = model.JobNo;
               sewingInputProcess.OrderShipRefId = model.OrderShipRefId;
               sewingInputProcess.OrderShipRefId = model.OrderShipRefId;
               sewingInputProcess.Remarks = model.Remarks;
               edited += _sewingInputProcessRepository.Edit(sewingInputProcess);
               transaction.Complete();
            }
            return edited;
        }

        public int DeleteSewingInputProcess(long sewingInputProcessId, string compId)
        {
            int deleted = 0;
            using (var transaction = new TransactionScope())
            {
                deleted += _sewingInputProcessDetailRepository.Delete(x => x.CompId == compId && x.SewingInputProcessId == sewingInputProcessId);
                deleted += _sewingInputProcessRepository.Delete(x => x.CompId == compId && x.SewingInputProcessId == sewingInputProcessId);
                transaction.Complete();
            }
            return deleted;
        }

        public List<VwSewingInputProcess> GetSewingInputByPaging(DateTime? inputDate, int lineId, int pageIndex, out int totalRecords, out int totalInput)
        {
            var index = pageIndex;
            const int pageSize = 20;
            string compId = PortalContext.CurrentUser.CompId;
            var sewingInputList = _sewingInputProcessRepository.GetSewingInputByPaging(inputDate, lineId, compId);
            totalRecords = sewingInputList.Count();
            totalInput = sewingInputList.ToList().Sum(x => x.InputQuantity);
            sewingInputList = sewingInputList.OrderByDescending(r => r.SewingInputProcessId);
            //Don't delete: IF Paging need only active the comenting code
            //sewingInputList = sewingInputList
            //            .OrderByDescending(r => r.SewingInputProcessId)
            //            .Skip(index * pageSize)
            //            .Take(pageSize);
            return sewingInputList.ToList();
        }

        public bool IsSewingInputExist(PROD_SewingInputProcess model)
        {
            model.CompId = PortalContext.CurrentUser.CompId;

            return _sewingInputProcessRepository.IsSewingInputExist(model);
        }

        public int SaveSweingInBarcode(PROD_SewingInputProcess sewingInputProcess)
        {

            long bundleId =Convert.ToInt64(sewingInputProcess.JobNo);

            PROD_BundleCutting bundleCutting =
                _bundleCuttingRepository.All()
                    .Include(x => x.PROD_CuttingBatch)
                    .FirstOrDefault(x => x.BundleCuttingId == bundleId);
            sewingInputProcess.CompId = PortalContext.CurrentUser.CompId;
            sewingInputProcess.BuyerRefId = bundleCutting.PROD_CuttingBatch.BuyerRefId;
            sewingInputProcess.OrderNo = bundleCutting.PROD_CuttingBatch.OrderNo;
            sewingInputProcess.OrderStyleRefId = bundleCutting.PROD_CuttingBatch.OrderStyleRefId;
            sewingInputProcess.ColorRefId = bundleCutting.PROD_CuttingBatch.ColorRefId;
            sewingInputProcess.SewingInputProcessRefId = GetNewSewingInputProcessRefId();
            sewingInputProcess.PreparedBy = PortalContext.CurrentUser.UserId.GetValueOrDefault();

            sewingInputProcess.PROD_SewingInputProcessDetail=new List<PROD_SewingInputProcessDetail>();
            sewingInputProcess.PROD_SewingInputProcessDetail.Add(new PROD_SewingInputProcessDetail()
             {
                 CompId = sewingInputProcess.CompId ,
                 InputQuantity = bundleCutting.Quantity.GetValueOrDefault(),
                 SizeRefId = bundleCutting.SizeRefId
             });

           return _sewingInputProcessRepository.Save(sewingInputProcess);
        }

        public List<VwSewingInputProcess> DailySweingInPut(DateTime date,int lineId, string compId)
        {
            return
                _sewingInputProcessRepository.GetSewingInputByPaging(date, lineId, compId)
                    .OrderByDescending(x => x.SewingInputProcessId)
                    .ToList();
        }

        public PROD_SewingInputProcess GetInputByBundleId(string bundleId)
        {
            return
                _sewingInputProcessRepository.FindOne(x => x.JobNo == bundleId);

        }

        public List<VwSewingOutput> GetVwSewingInput(string orderStyleRefId, string colorRefId, string orderShipRefId)
        {
            return _sewingInputProcessRepository.GetVwSewingInput(PortalContext.CurrentUser.CompId, orderStyleRefId,
                colorRefId, orderShipRefId);
        }

        public List<VwSewingInputProcess> GetSewingInputProcessByStyleColor(string orderStyleRefId, string colorRefId,string orderShipRefId)
        {
            return _sewingInputProcessRepository.GetSewingInputProcessByStyleColor(PortalContext.CurrentUser.CompId, orderStyleRefId, colorRefId, orderShipRefId);
        }

        public List<VwSewingInputProcessDetail> GetAllSewingInputInfo(long sewingInputProcessId)
        {
            List<VwSewingInputProcessDetail> sewingInputProcessDetails = _sewingInputProcessRepository.GetAllSewingInputInfo(PortalContext.CurrentUser.CompId,sewingInputProcessId);
            return sewingInputProcessDetails.ToList();
        }
        public PROD_SewingInputProcess GetSewintInputProcessBySewingInputProcessId(long sewingInputProcessId, string compId)
        {
            return
                _sewingInputProcessRepository.FindOne(x => x.SewingInputProcessId == sewingInputProcessId && x.CompId == compId);
        }

        public int SweingAccessoriesIssueLock(long sewingInputProcessId)
        {
            var issue=_sewingInputProcessRepository.FindOne(x => x.SewingInputProcessId == sewingInputProcessId);
            if (issue.Locked.GetValueOrDefault() == false)
            {
                issue.LockedBy = PortalContext.CurrentUser.UserId;
                issue.Locked = true;
            }
            else
            {
                issue.LockedBy = null;
                issue.Locked = false;
            }

            return _sewingInputProcessRepository.Edit(issue);
        }
    }
}
