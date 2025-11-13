using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using SCERP.DAL.IRepository.IProductionRepository;
using SCERP.Model.Production;

namespace SCERP.DAL.Repository.ProductionRepository
{
    public class CuttFabRejectRepository :Repository<PROD_CuttFabReject>, ICuttFabRejectRepository
    {
        public CuttFabRejectRepository(SCERPDBContext context) : base(context)
        {
        }


        public IQueryable<VwCuttFabReject> GetCuttFabRejects(Expression<Func<VwCuttFabReject, bool>> predicate)
        {
           return Context.VwCuttFabRejects.Where(predicate).AsQueryable();
        }
    }
}
