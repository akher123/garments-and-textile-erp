using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using SCERP.DAL.IRepository.IAccountingRepository;
using SCERP.Model;

namespace SCERP.DAL.Repository.AccountingRepository
{
    public class FinancialPeriodRepository : Repository<Acc_FinancialPeriod>, IFinancialPeriodRepository
    {
        public FinancialPeriodRepository(SCERPDBContext context)
            : base(context)
        {

        }

        public override IQueryable<Acc_FinancialPeriod> All()
        {
            return Context.Acc_FinancialPeriod.Where(x => x.IsActive == true);
        }

        public Acc_FinancialPeriod GetFinancialPeriodById(int? id)
        {
            return Context.Acc_FinancialPeriod.Find(id);
        }

        public List<Acc_FinancialPeriod> GetAllFinancialPeriods(int page, int records, string sort)
        {
            var FinancialPeriod = Context.Acc_FinancialPeriod.Where(p => p.IsActive == true).OrderBy(p => p.SortOrder).ToList();

            foreach (var t in FinancialPeriod)
            {
                if (t.ActiveStatus == false)
                    t.ActiveStatus = null;
            }

            switch (sort)
            {
                //case "Title":
                //    FinancialPeriod = FinancialPeriod.OrderBy(r => r.Title).ToList();
                //    break;
                //case "Description":
                //    FinancialPeriod = FinancialPeriod.OrderBy(r => r.Description).ToList();
                //    break;
                //default:
                //    FinancialPeriod = FinancialPeriod.OrderBy(r => r.Id).ToList();
                //    break;
            }

            FinancialPeriod = FinancialPeriod.Skip(page * records).Take(records).ToList();
            return FinancialPeriod;
        }

        public IQueryable<Acc_FinancialPeriod> GetAllFinancialPeriods()
        {
            var FinancialPeriod = Context.Acc_FinancialPeriod.Where(r => r.IsActive == true).OrderBy(x => x.SortOrder);
            return FinancialPeriod;
        }
    }
}
