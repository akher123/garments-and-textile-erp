using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using SCERP.Common;
using SCERP.DAL.IRepository.ICommercialRepository;
using SCERP.Model.CommercialModel;
using System.Data;

namespace SCERP.DAL.Repository.CommercialRepository
{
    public class LcRepository : Repository<COMMLcInfo>, ILcRepository
    {
        private readonly string _companyId;

        public LcRepository(SCERPDBContext context)
            : base(context)
        {
            this._companyId = PortalContext.CurrentUser.CompId;
        }

        public COMMLcInfo GetLcInfoById(int? id)
        {
            return Context.COMMLcInfos.FirstOrDefault(x => x.LcId == id);
        }

        public List<COMMLcInfo> GetLcInfoByLcId(int? id)
        {
            return Context.COMMLcInfos.Where(x => x.LcId == id && x.IsActive == true).ToList();
        }


        public COMMLcInfo GetLcInfoByLcNo(string lcNo)
        {
            return Context.COMMLcInfos.FirstOrDefault(x => x.LcNo.Trim().ToLower() == lcNo.Trim().ToLower());
        }

        public List<COMMLcInfo> GetAllLcInfos()
        {
            return Context.COMMLcInfos.Where(x => x.IsActive == true).OrderBy(y => y.LcDate).ToList();
        }

        public List<COMMLcInfo> GetLcInfoBySearchKey(int searchByCountry, string searchByLcInfo)
        {
            List<COMMLcInfo> lcInfos = null;
            lcInfos = Context.COMMLcInfos.Where(x => x.IsActive).ToList();
            return lcInfos;
        }

        public List<COMMLcInfo> GetAllLcInfosByPaging(int startPage, int pageSize, out int totalRecords, COMMLcInfo lcInfo)
        {
            long? buyerId = lcInfo.BuyerId ?? 0;
            int? lcType = lcInfo.LcType ?? 0;
            DateTime? fromDate = lcInfo.FromDate ?? new DateTime(2000, 01, 01);
            DateTime? toDate = lcInfo.ToDate ?? new DateTime(2000, 01, 01);
            string lcBank = lcInfo.LcIssuingBank ?? "";
            string receiveBank = lcInfo.ReceivingBank ?? "";
            string lcNo = lcInfo.LcNo ?? "";

            List<COMMLcInfo> lcInfos = Context.Database.SqlQuery<COMMLcInfo>("SPCommGetLcInfo @CompanyId, @BuyerId, @LcType, @FromDate, @ToDate, @LcBank, @ReceiveBank, @LCNO", new SqlParameter("CompanyId", _companyId), new SqlParameter("BuyerId", buyerId), new SqlParameter("LcType", lcType), new SqlParameter("FromDate", fromDate), new SqlParameter("ToDate", toDate), new SqlParameter("LcBank", lcBank), new SqlParameter("ReceiveBank", receiveBank), new SqlParameter("LCNO", lcNo)).ToList();
            lcInfos = lcInfos.Where(x => x.RStatus == lcInfo.RStatus).ToList();
            totalRecords = lcInfos.Count();

            switch (lcInfo.sort)
            {
                case "Id":

                    switch (lcInfo.sortdir)
                    {
                        case "DESC":
                            lcInfos = lcInfos
                                .OrderByDescending(r => r.LcId).ThenBy(x => x.LcId)
                                .Skip(startPage*pageSize)
                                .Take(pageSize).ToList();
                            break;
                        default:
                            lcInfos = lcInfos
                                .OrderBy(r => r.LcId).ThenBy(x => x.LcId)
                                .Skip(startPage*pageSize)
                                .Take(pageSize).ToList();
                            break;
                    }
                    break;

                default:
                    lcInfos = lcInfos
                        .OrderBy(r => r.CreatedDate).ThenBy(x => x.LcDate)
                        .Skip(startPage*pageSize)
                        .Take(pageSize).ToList();
                    break;
            }
            return lcInfos;
        }

        public List<CommBank> GetBankInfo(string bankType)
        {
            List<CommBank> banks = Context.CommBanks.Where(p => p.BankType.ToLower() == bankType.ToLower() && p.IsActive).ToList();
            return banks;
        }

        public IQueryable<VwCommLcInfo> GetLcInfos(string modelRStatus, int? receivingBankId,string searchString)
        {
            
            return Context.VwCommLcInfos.Where(x => x.IsActive && x.RStatus == modelRStatus &&
                                                    (x.ReceivingBankId == receivingBankId || receivingBankId == null) && (x.LcNo.Contains(searchString) || searchString == null));
        }
    }
}