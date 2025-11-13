using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Security.Cryptography;
using SCERP.Common;
using SCERP.DAL.IRepository.IAccountingRepository;
using SCERP.Model;

namespace SCERP.DAL.Repository.AccountingRepository
{
    public class BankReconcilationListRepository : Repository<Acc_BankReconcilationMaster>,
        IBankReconcilationListRepository
    {
        public BankReconcilationListRepository(SCERPDBContext context)
            : base(context)
        {

        }

        public List<Acc_BankReconcilationList> GetAllReconcilationList(int page, int records, string sort,
            int? fpId, DateTime? FromDate, DateTime? ToDate, string AccountName, string SectorId)
        {
            var NoOfReconciled = from p in Context.Acc_BankReconciliationDetail
                group p by p.RefId
                into g
                select new
                {
                    RefId = g.Key,
                    Reconciled = g.Count()
                };

            var temp = Context.Acc_BankReconcilationMaster.Where(p => p.IsActive == false);

            if (fpId > 0)
            {
                temp = Context.Acc_BankReconcilationMaster.Where(p => p.FinancialPeriodId == fpId);
            }

            DateTime dateTime;
            if (DateTime.TryParse(FromDate.ToString(), out dateTime) &&
                DateTime.TryParse(ToDate.ToString(), out dateTime))
            {
                temp = temp.Where(p => p.FromDate >= FromDate && p.ToDate <= ToDate);
            }

            decimal accCode = 0;
            if (!string.IsNullOrEmpty(AccountName))
            {
                accCode = Convert.ToDecimal(AccountName.Substring(0, 10));
                var GlId = Context.Acc_GLAccounts.FirstOrDefault(p => p.AccountCode == accCode).Id;
                temp = temp.Where(p => p.GLID == GlId);
            }

            if (!string.IsNullOrEmpty(SectorId))
            {
                int sectorId = Convert.ToInt32(SectorId);
                temp = temp.Where(p => p.SectorId == sectorId);
            }

            var bankReconcil = (from p in temp

                join q in Context.Acc_CompanySector on p.SectorId equals q.Id
                join r in Context.Acc_FinancialPeriod on p.FinancialPeriodId equals r.Id
                join s in Context.Acc_GLAccounts on p.GLID equals s.Id
                join t in NoOfReconciled on p.Id equals t.RefId
                where p.IsActive == true
                select new Acc_BankReconcilationList
                {
                    Id = p.Id,
                    Sector = q.SectorName,
                    FinancialPeriod = r.PeriodName,
                    AccountName = s.AccountName,
                    FromDate = p.FromDate.Value,
                    ToDate = p.ToDate.Value,
                    NoOfReconciled = t.Reconciled,
                    NoOfPending = 1
                }).ToList();

            bankReconcil = bankReconcil.Skip(page*records).Take(records).ToList();
            return bankReconcil;
        }

        public Acc_BankReconcilationMaster GetBankReconcilationMasterById(long? id)
        {
            return Context.Acc_BankReconcilationMaster.Find(id);
        }
    }
}
