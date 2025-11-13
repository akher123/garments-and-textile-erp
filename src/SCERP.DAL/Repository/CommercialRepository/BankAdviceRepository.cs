using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using SCERP.Common;
using SCERP.DAL.IRepository.ICommercialRepository;
using SCERP.Model.CommercialModel;

namespace SCERP.DAL.Repository.CommercialRepository
{
    public class BankAdviceRepository : Repository<CommBankAdvice>, IBankAdviceRepository
    {
        private readonly string _companyId;

        public BankAdviceRepository(SCERPDBContext context)
            : base(context)
        {
            this._companyId = PortalContext.CurrentUser.CompId;
        }


        public CommBankAdvice GetBankAdviceById(int? id)
        {
            return Context.CommBankAdvices.FirstOrDefault(x => x.BankAdviceId == id);
        }

        public CommBankAdvice GetBankAdviceByExportAndHeadId(long? exportId, int? accHeadId, out int count)
        {
            count = Context.CommBankAdvices.Count(p => p.ExportId == exportId && p.AccHeadId == accHeadId);
            return Context.CommBankAdvices.SingleOrDefault(p => p.ExportId == exportId && p.AccHeadId == accHeadId);
        }

        public List<CommBankAdvice> GetBankAdviceByExportId(Int64 id)
        {
            return Context.CommBankAdvices.Where(p => p.ExportId == id).ToList();
        }

        public List<CommAccHead> GetAccHead(string type)
        {
            return Context.CommAccHeads.Where(p => p.IsActive && p.AccHeadType.Trim().ToLower() == type.Trim().ToLower()).ToList();
        }

        public List<CommAccHead> GetAccHead(string type, Int64 exportId)
        {
            List<CommAccHead> accHead = Context.Database.SqlQuery<CommAccHead>("SPCommGetBankAdvice @ExportId, @Type, @CompId", new SqlParameter("ExportId", exportId), new SqlParameter("Type", type), new SqlParameter("CompId", _companyId)).ToList();
            return accHead;
        }

        public List<CommBankAdvice> GetAllBankAdvices()
        {
            return Context.CommBankAdvices.Where(x => x.IsActive == true).OrderBy(y => y.ExportId).ToList();
        }

        public List<CommBankAdvice> GetBankAdviceBySearchKey(int searchByCountry, string searchByBankAdvice)
        {
            List<CommBankAdvice> bankAdvices = null;
            bankAdvices = Context.CommBankAdvices.Where(x => x.IsActive).ToList();
            return bankAdvices;
        }

        public List<CommBankAdvice> GetAllBankAdvicesByPaging(int startPage, int pageSize, out int totalRecords, CommBankAdvice bankAdvice)
        {
            long? buyerId = bankAdvice.BankAdviceId;
            int? lcType = bankAdvice.BankAdviceId;
            DateTime? fromDate = bankAdvice.FromDate ?? new DateTime(2000, 01, 01);
            DateTime? toDate = bankAdvice.ToDate ?? new DateTime(2000, 01, 01);

            List<CommBankAdvice> bankAdvices = Context.Database.SqlQuery<CommBankAdvice>("SPCommGetBankAdvice @CompanyId, @BuyerId, @LcType, @FromDate, @ToDate, @LcBank, @BankAdviceBank", new SqlParameter("CompanyId", _companyId), new SqlParameter("BuyerId", buyerId), new SqlParameter("LcType", lcType), new SqlParameter("FromDate", fromDate), new SqlParameter("ToDate", toDate)).ToList();
            totalRecords = bankAdvices.Count();

            switch (bankAdvice.sort)
            {
                case "Id":

                    switch (bankAdvice.sortdir)
                    {
                        case "DESC":
                            bankAdvices = bankAdvices
                                .OrderByDescending(r => r.BankAdviceId).ThenBy(x => x.BankAdviceId)
                                .Skip(startPage*pageSize)
                                .Take(pageSize).ToList();
                            break;
                        default:
                            bankAdvices = bankAdvices
                                .OrderBy(r => r.BankAdviceId).ThenBy(x => x.BankAdviceId)
                                .Skip(startPage*pageSize)
                                .Take(pageSize).ToList();
                            break;
                    }
                    break;

                default:
                    bankAdvices = bankAdvices
                        .OrderBy(r => r.BankAdviceId).ThenBy(x => x.BankAdviceId)
                        .Skip(startPage*pageSize)
                        .Take(pageSize).ToList();
                    break;
            }
            return bankAdvices.ToList();
        }
    }
}
