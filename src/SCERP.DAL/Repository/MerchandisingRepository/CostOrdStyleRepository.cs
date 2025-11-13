using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using SCERP.DAL.IRepository.IMerchandisingRepository;
using SCERP.Model;
using SCERP.Model.MerchandisingModel;

namespace SCERP.DAL.Repository.MerchandisingRepository
{
    public class CostOrdStyleRepository :Repository<OM_CostOrdStyle>, ICostOrdStyleRepository
    {
        public CostOrdStyleRepository(SCERPDBContext context) : base(context)
        {
        }

        public IQueryable<VCostOrderStyle> GetVCostCostOrdStyle(Expression<Func<VCostOrderStyle, bool>> predicates)
        {
            return Context.VCostOrderStyles.Where(predicates);
        }
    }
}
