using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using SCERP.DAL.IRepository.IAccountingRepository;
using SCERP.Model;

namespace SCERP.DAL.Repository.AccountingRepository
{
    public class CompanySectorRepository : Repository<Acc_CompanySector>, ICompanySectorRepository
    {
        public CompanySectorRepository(SCERPDBContext context)
            : base(context)
        {

        }


        public override IQueryable<Acc_CompanySector> All()
        {
            return Context.Acc_CompanySector.Where(x => x.IsActive == true);
        }

        public Acc_CompanySector GetCompanySectorById(int? id)
        {
            return Context.Acc_CompanySector.Find(id);
        }

        public List<Acc_CompanySector> GetAllCompanySectors(int page, int records, string sort)
        {
            var CompanySector = Context.Acc_CompanySector.Where(p => p.IsActive == true).OrderBy(p=>p.SortOrder).ToList();

            switch (sort)
            {
                //case "Title":
                //    CompanySector = CompanySector.OrderBy(r => r.Title).ToList();
                //    break;
                //case "Description":
                //    CompanySector = CompanySector.OrderBy(r => r.Description).ToList();
                //    break;
                //default:
                //    CompanySector = CompanySector.OrderBy(r => r.Id).ToList();
                //    break;
            }

            CompanySector = CompanySector.Skip(page * records).Take(records).ToList();
            return CompanySector;
        }

        public IQueryable<Acc_CompanySector> GetAllCompanySectors()
        {
            var CompanySector = Context.Acc_CompanySector.Where(r => r.IsActive == true).OrderBy(x => x.SortOrder);
            return CompanySector;
        }
    }
}
