using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using SCERP.DAL.IRepository.IAccountingRepository;
using SCERP.Model;

namespace SCERP.DAL.Repository.AccountingRepository
{
    public class DepreciationChartRepository : Repository<Acc_DepreciationChart>, IDepreciationChartRepository
    {
        public DepreciationChartRepository(SCERPDBContext context)
            : base(context)
        {
        }

        public override IQueryable<Acc_DepreciationChart> All()
        {
            return Context.Acc_DepreciationChart.Where(x => x.IsActive == true);
        }

        public Acc_DepreciationChart GetDepreciationById(int? id)
        {
            return Context.Acc_DepreciationChart.Find(id);
        }

        public List<Acc_DepreciationChart> GetAllDepreciation(int page, int records, string sort)
        {
            var depreciationChart = Context.Acc_DepreciationChart.Where(p => p.IsActive == true).ToList();

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

            depreciationChart = depreciationChart.Skip(page*records).Take(records).ToList();
            return depreciationChart;
        }

        public IQueryable<Acc_DepreciationChart> GetAllDepreciations()
        {
            var depreciationChart = Context.Acc_DepreciationChart.Where(r => r.IsActive == true);
            return depreciationChart;
        }

        public IQueryable<Acc_ControlAccounts> GetAllControlAccounts()
        {
            return Context.Acc_ControlAccounts.Where(p => p.IsActive == true && p.ParentCode == 101);
        }

        public string GetControlName(decimal ControlCode)
        {
            return Context.Acc_ControlAccounts.FirstOrDefault(p => p.ControlCode == ControlCode).ControlName;
        }
    }
}
