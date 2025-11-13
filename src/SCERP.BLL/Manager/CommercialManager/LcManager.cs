using System;
using System.Collections.Generic;
using System.Linq;
using SCERP.BLL.IManager.ICommercialManager;
using SCERP.Common;
using SCERP.DAL;
using SCERP.DAL.IRepository.ICommercialRepository;
using SCERP.DAL.Repository.CommercialRepository;
using SCERP.Model.CommercialModel;
using System.Data;
using SCERP.DAL.IRepository;

namespace SCERP.BLL.Manager.CommercialManager
{
    public class LcManager : ILcManager
    {
        private readonly ILcRepository _iLcRepository;
        private IRepository<VwCommLcInfo> _vwrepository;
        private IRepository<CommSalseContact> _lcScRepository;
        public LcManager(ILcRepository iLcRepository, IRepository<VwCommLcInfo> vwrepository, IRepository<CommSalseContact> lcScRepository)
        {
            _iLcRepository = iLcRepository;
            _vwrepository = vwrepository;
            _lcScRepository = lcScRepository;
        }

        public List<COMMLcInfo> GetAllLcInfosByPaging(int startPage, int pageSize, out int totalRecords, COMMLcInfo lcInfo)
        {
            List<COMMLcInfo> commLcInfos = null;
            commLcInfos = _iLcRepository.GetAllLcInfosByPaging(startPage, pageSize, out totalRecords, lcInfo);
            return commLcInfos;
        }

        public List<COMMLcInfo> GetAllLcInfos()
        {
            List<COMMLcInfo> commLcInfo = null;
            commLcInfo = _iLcRepository.Filter(x => x.IsActive&&x.RStatus=="O").OrderBy(x => x.LcNo).ThenBy(x=>x.SalesContactNo).ToList();
            return commLcInfo;
        }

        public COMMLcInfo GetLcInfoById(int? id)
        {
            COMMLcInfo commLcInfo = null;
            commLcInfo = _iLcRepository.GetLcInfoById(id);
            return commLcInfo;
        }

        public List<COMMLcInfo> GetLcInfoByLcId(int? id)
        {
            List<COMMLcInfo> commLcInfo = null;
            commLcInfo = _iLcRepository.GetLcInfoByLcId(id);
            return commLcInfo;
        }

        public bool CheckExistingLcInfo(COMMLcInfo lcInfo)
        {
            bool isExist = false;

            if (lcInfo.LcId == 0 && _iLcRepository.Exists(p => p.IsActive == true && p.LcNo.Trim().ToLower() == lcInfo.LcNo.Trim().ToLower()))
                isExist = true;

            else if (lcInfo.LcId > 0 && _iLcRepository.Exists(p => p.IsActive == true && p.LcId != lcInfo.LcId && p.LcNo.Trim().ToLower() == lcInfo.LcNo.Trim().ToLower()))
                isExist = true;

            return isExist;
        }

        public int SaveLcInfo(COMMLcInfo lcInfo)
        {
            int savedCommLcInfo = 0;
            lcInfo.CreatedDate = DateTime.Now;
            lcInfo.CreatedBy = PortalContext.CurrentUser.UserId;
            lcInfo.IsActive = true;
            lcInfo.RStatus = "O";
            savedCommLcInfo = _iLcRepository.Save(lcInfo);
            return savedCommLcInfo;
        }

        public int EditLcInfo(COMMLcInfo lcInfo)
        {
            int editedCommLcInfo = 0;
            lcInfo.EditedDate = DateTime.Now;
            lcInfo.EditedBy = PortalContext.CurrentUser.UserId;
            editedCommLcInfo = _iLcRepository.Edit(lcInfo);
            return editedCommLcInfo;
        }

        public int DeleteLcInfo(COMMLcInfo lcInfo)
        {
            int deletedCommLcInfo = 0;
            lcInfo.EditedDate = DateTime.Now;
            lcInfo.EditedBy = PortalContext.CurrentUser.UserId;
            lcInfo.IsActive = false;
            deletedCommLcInfo = _iLcRepository.Edit(lcInfo);
            return deletedCommLcInfo;
        }

        public List<COMMLcInfo> GetLcInfoBySearchKey(int searchByCountry, string searchByLcInfo)
        {
            List<COMMLcInfo> lcInfos = null;
            lcInfos = _iLcRepository.GetLcInfoBySearchKey(searchByCountry, searchByLcInfo);
            return lcInfos;
        }


        public COMMLcInfo GetLcInfoByLcNo(string lcNo)
        {
            COMMLcInfo lcInfo = null;
            lcInfo = _iLcRepository.GetLcInfoByLcNo(lcNo);
            return lcInfo;
        }

        public int UpdateChashIncentive(COMMLcInfo commLcInfo)
        {
            COMMLcInfo lcInfo = _iLcRepository.FindOne(x => x.IsActive && x.LcId == commLcInfo.LcId);
            lcInfo.AppliedDate = commLcInfo.AppliedDate;
            lcInfo.IncentiveClaimValue = commLcInfo.IncentiveClaimValue;
            lcInfo.NewMarketCliam = commLcInfo.NewMarketCliam;
            lcInfo.BTMACertificate = commLcInfo.BTMACertificate;
            lcInfo.BkmeaCertificat = commLcInfo.BkmeaCertificat;
            lcInfo.FirstAuditStatus = commLcInfo.FirstAuditStatus;
            lcInfo.CertificateOvservation = commLcInfo.CertificateOvservation;
            lcInfo.FinalClaimAmount = commLcInfo.FinalClaimAmount;
            lcInfo.ReceiveAmount = commLcInfo.ReceiveAmount;
            lcInfo.ReceiveDate = commLcInfo.ReceiveDate;
            lcInfo.CashIncentiveRemarks = commLcInfo.CashIncentiveRemarks;
            return _iLcRepository.Edit(lcInfo);
        }

        public List<COMMLcInfo> GetLcInfosByPaging(int pageIndex, int pageSize, out int totalRecords, long? buyerId, DateTime? fromDate, DateTime? toDate, string searchString, string completeStatus)
        {

            IQueryable<COMMLcInfo> commLcInfos = _iLcRepository.Filter( x =>x.IsActive &&
                        ((x.BuyerId == buyerId || buyerId == null) &&
                         (x.LcNo.Contains(searchString) || searchString == null)) &&
                        ((x.LcDate >= fromDate || fromDate == null) && (x.LcDate <= toDate || toDate == null)));

            if (completeStatus == "Completed")
                commLcInfos = commLcInfos.Where(p => p.IncentiveClaimValue > 0);

            else if (completeStatus == "Pending")
                commLcInfos = commLcInfos.Where(p => p.IncentiveClaimValue == null);

            totalRecords = commLcInfos.Count();
            commLcInfos = commLcInfos.OrderByDescending(x => x.IncentiveClaimValue).Skip(pageIndex*pageSize)
                .Take(pageSize);


            return commLcInfos.ToList();
        }

        public List<COMMLcInfo> GetCashIncentiveReport(long? buyerId, DateTime? fromDate, DateTime? toDate, string searchString)
        {
            IQueryable<COMMLcInfo> commLcInfos =
                _iLcRepository.Filter(
                    x =>
                        x.IsActive &&
                        ((x.BuyerId == buyerId || buyerId == null) &&
                         (x.LcNo.Contains(searchString) || searchString == null)) &&
                        ((x.LcDate >= fromDate || fromDate == null) && (x.LcDate <= toDate || toDate == null))).OrderByDescending(x => x.LcDate);
            return commLcInfos.ToList();
        }

        public List<COMMLcInfo> GetCashIncentiveByDateReport(DateTime? fromDate, DateTime? toDate, string searchString)
        {
            IQueryable<COMMLcInfo> commLcInfos =
                _iLcRepository.Filter(
                    x =>
                        x.IsActive &&
                        (
                         (x.LcNo.Contains(searchString) || searchString == null)) &&
                        ((x.LcDate >= fromDate || fromDate == null) && (x.LcDate <= toDate || toDate == null))).OrderByDescending(x => x.LcDate);
            return commLcInfos.ToList();
        }
        public List<CommBank> GetBankInfo(string bankType)
        {
            return _iLcRepository.GetBankInfo(bankType);
        }

        public List<VwCommLcInfo> GetLcInfos(int pageIndex, out int totalRecords, string modelRStatus, int? receivingBankId, long? buyerId, string searchString)
        {
            int[] lcIds = _lcScRepository.Filter(x => x.LcNo.Contains(searchString)).Select(x => x.LcId).ToArray();

            IQueryable<VwCommLcInfo> lcInfos = _vwrepository.Filter(x => x.IsActive==true && x.RStatus == modelRStatus 
               &&(x.ReceivingBankId == receivingBankId || receivingBankId == null) && (x.BuyerId == buyerId || buyerId == null) && (x.LcNo.Contains(searchString) || searchString == null) || lcIds.Contains(x.LcId) || (x.UdEoNo.Contains(searchString) || String.IsNullOrEmpty(searchString)));
           
            var pageSize = AppConfig.PageSize;

            totalRecords = lcInfos.Count();
            lcInfos = lcInfos.OrderByDescending(
                    x => x.LcDate)
                .Skip(pageIndex * pageSize)
                .Take(pageSize);
            return lcInfos.ToList();
        }

        public List<COMMLcInfo> GetAllGroupLcs(string compId)
        {
            return _iLcRepository.Filter(x => x.LcType == 2 && x.RStatus == "O").ToList();
        }
    }
}