using System;
using System.Linq;
using System.Linq.Expressions;
using SCERP.DAL.IRepository.IProductionRepository;
using SCERP.Model.Production;

namespace SCERP.DAL.Repository.ProductionRepository
{
    public class ProductionDetailRepository : Repository<PROD_ProductionDetaill>, IProductionDetailRepository
    {
        public ProductionDetailRepository(SCERPDBContext context) : base(context)
        {

        }

        public IQueryable<VProductionDetail> GetVProductionDetails(Expression<Func<VProductionDetail, bool>> predicate)
        {
            return Context.VProductionDetails.Where(predicate);
        }
    }
}
