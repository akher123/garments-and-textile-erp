using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using SCERP.Common;
using SCERP.DAL.IRepository.ICommercialRepository;
using SCERP.Model.CommercialModel;


namespace SCERP.DAL.Repository.CommercialRepository
{
    public class BbBbLcRepository : Repository<CommBbLcInfo>, IBbLcRepository
    {
        private readonly string _companyId;

        public BbBbLcRepository(SCERPDBContext context)
            : base(context)
        {
            this._companyId = PortalContext.CurrentUser.CompId;
        }

        public CommBbLcInfo GetBbLcInfoById(int? id)
        {
            return Context.CommBbLcInfos.FirstOrDefault(x => x.BbLcId == id);
        }

        public CommBbLcInfo GetBbLcIdByBbLcNo(string bbLcNo)
        {
            return Context.CommBbLcInfos.FirstOrDefault(p => p.BbLcNo.Trim().ToLower() == bbLcNo.Trim().ToLower());
        }

        public List<CommBbLcInfo> GetAllBbLcInfos()
        {
            return Context.CommBbLcInfos.Where(x => x.IsActive == true).OrderBy(y => y.BbLcDate).ToList();
        }

        public List<CommBbLcInfo> GetBbLcInfoBySearchKey(int searchByCountry, string searchByBbLcInfo)
        {
            List<CommBbLcInfo> bbLcInfos = null;
            bbLcInfos = Context.CommBbLcInfos.Where(x => x.IsActive).ToList();
            return bbLcInfos;
        }

        public List<CommBbLcInfo> GetAllBbLcInfosByPaging(int startPage, int pageSize, out int totalRecords, CommBbLcInfo bbLcInfo)
        {
            long? supplierId = bbLcInfo.SupplierCompanyRefId ?? 0;
            int? bbLcType = bbLcInfo.BbLcType ?? 0;
            DateTime? fromDate = bbLcInfo.FromDate ?? new DateTime(2000, 01, 01);
            DateTime? toDate = bbLcInfo.ToDate ?? new DateTime(2000, 01, 01);

            List<CommBbLcInfo> bbLcInfos = Context.Database.SqlQuery<CommBbLcInfo>("SPCommGetBbLcInfo @CompanyId, @SupplierId, @LcType, @FromDate, @ToDate", new SqlParameter("CompanyId", _companyId), new SqlParameter("SupplierId", supplierId), new SqlParameter("LcType", bbLcType), new SqlParameter("FromDate", fromDate), new SqlParameter("ToDate", toDate)).ToList();
            totalRecords = bbLcInfos.Count();

            switch (bbLcInfo.sort)
            {
                case "Id":

                    switch (bbLcInfo.sortdir)
                    {
                        case "DESC":
                            bbLcInfos = bbLcInfos
                                .OrderByDescending(r => r.BbLcId).ThenBy(x => x.BbLcId)
                                .Skip(startPage*pageSize)
                                .Take(pageSize).ToList();
                            break;
                        default:
                            bbLcInfos = bbLcInfos
                                .OrderBy(r => r.BbLcId).ThenBy(x => x.BbLcId)
                                .Skip(startPage*pageSize)
                                .Take(pageSize).ToList();
                            break;
                    }
                    break;

                default:
                    bbLcInfos = bbLcInfos
                        .OrderBy(r => r.BbLcId).ThenBy(x => x.BbLcId)
                        .Skip(startPage*pageSize)
                        .Take(pageSize).ToList();
                    break;
            }
             bbLcInfos= bbLcInfos.Where(x => x.LcRefId == bbLcInfo.LcRefId || bbLcInfo.LcRefId == null).ToList();
            return bbLcInfos.ToList();
        }
    }
}