using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using SCERP.Common;
using SCERP.DAL.IRepository.ICommercialRepository;
using SCERP.Model.CommercialModel;


namespace SCERP.DAL.Repository.CommercialRepository
{
    public class ReceiveRepository : Repository<CommReceive>, IReceiveRepository
    {
        private readonly string _companyId;

        public ReceiveRepository(SCERPDBContext context)
            : base(context)
        {
            this._companyId = PortalContext.CurrentUser.CompId;
        }

        public CommReceive GetReceiveById(int? id)
        {
            return Context.CommReceives.FirstOrDefault(x => x.ReceiveId == id);
        }

        public List<CommReceive> GetAllReceives()
        {
            return Context.CommReceives.Where(x => x.IsActive == true).OrderBy(y => y.ReceiveDate).ToList();
        }

        public List<CommReceive> GetReceiveBySearchKey(int searchByCountry, string searchByReceive)
        {
            List<CommReceive> Receives = null;
            Receives = Context.CommReceives.Where(x => x.IsActive).ToList();
            return Receives;
        }

        public List<CommReceive> GetAllReceivesByPaging(int startPage, int pageSize, out int totalRecords, CommReceive Receive)
        {
            long? buyerId = Receive.ReceiveId;
            int? lcType = Receive.ReceiveId;
            DateTime? fromDate = Receive.FromDate ?? new DateTime(2000, 01, 01);
            DateTime? toDate = Receive.ToDate ?? new DateTime(2000, 01, 01);
            string lcBank = Receive.MushakChallanNo ?? "";
            string receiveBank = Receive.MushakChallanNo ?? "";

            List<CommReceive> Receives = Context.Database.SqlQuery<CommReceive>("SPCommGetReceive @CompanyId, @BuyerId, @LcType, @FromDate, @ToDate, @LcBank, @ReceiveBank", new SqlParameter("CompanyId", _companyId), new SqlParameter("BuyerId", buyerId), new SqlParameter("LcType", lcType), new SqlParameter("FromDate", fromDate), new SqlParameter("ToDate", toDate), new SqlParameter("LcBank", lcBank), new SqlParameter("ReceiveBank", receiveBank)).ToList();
            totalRecords = Receives.Count();

            switch (Receive.sort)
            {
                case "Id":

                    switch (Receive.sortdir)
                    {
                        case "DESC":
                            Receives = Receives
                                .OrderByDescending(r => r.ReceiveId).ThenBy(x => x.ReceiveId)
                                .Skip(startPage*pageSize)
                                .Take(pageSize).ToList();
                            break;
                        default:
                            Receives = Receives
                                .OrderBy(r => r.ReceiveId).ThenBy(x => x.ReceiveId)
                                .Skip(startPage*pageSize)
                                .Take(pageSize).ToList();
                            break;
                    }
                    break;

                default:
                    Receives = Receives
                        .OrderBy(r => r.ReceiveId).ThenBy(x => x.ReceiveId)
                        .Skip(startPage*pageSize)
                        .Take(pageSize).ToList();
                    break;
            }
            return Receives.ToList();
        }
    }
}