using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using SCERP.DAL.IRepository.IAccountingRepository;
using SCERP.Model;
using SCERP.Model.AccountingModel;

namespace SCERP.DAL.Repository.AccountingRepository
{
    public class CostCentreRepository : Repository<Acc_CostCentre>, ICostCentreRepository
    {
        public CostCentreRepository(SCERPDBContext context)
            : base(context)
        {

        }

        public override IQueryable<Acc_CostCentre> All()
        {
            return Context.Acc_CostCentre.Where(x => x.IsActive == true);
        }

        public Acc_CostCentre GetCostCentreById(int? id)
        {
            return Context.Acc_CostCentre.Find(id);
        }

        public List<Acc_CostCentre> GetAllCostCentres(int page, int records, string sort)
        {
            var CostCentre = Context.Acc_CostCentre.Include("Acc_CompanySector").Where(p => p.IsActive == true).OrderBy(p => p.SortOrder).ToList();

            switch (sort)
            {
                //case "Title":
                //    CostCentre = CostCentre.OrderBy(r => r.Title).ToList();
                //    break;
                //case "Description":
                //    CostCentre = CostCentre.OrderBy(r => r.Description).ToList();
                //    break;
                //default:
                //    CostCentre = CostCentre.OrderBy(r => r.Id).ToList();
                //    break;
            }

            CostCentre = CostCentre.Skip(page * records).Take(records).ToList();
            return CostCentre;
        }

        public IQueryable<Acc_CostCentre> GetAllCostCentres()
        {
            var CostCentre = Context.Acc_CostCentre.Where(r => r.IsActive == true).OrderBy(x => x.SortOrder);
            return CostCentre;
        }

        public IQueryable<Acc_CompanySector> GetAllCompanySector()
        {
            return Context.Acc_CompanySector.Where(x => x.IsActive == true);
        }

        public Acc_CostCentreMultiLayer GetNewCostCentreById(int? id)
        {
            return Context.Acc_CostCentreMultiLayer.Find(id);
        }
       
    }
}
