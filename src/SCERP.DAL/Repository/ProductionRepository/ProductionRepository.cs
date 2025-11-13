using System;
using System.Linq;
using System.Linq.Expressions;
using SCERP.DAL.IRepository.IMerchandisingRepository;
using SCERP.Model.Production;

namespace SCERP.DAL.Repository.ProductionRepository
{
    public class ProductionRepository : Repository<PROD_Production>, IProductionRepository
    {
        public ProductionRepository(SCERPDBContext context) : base(context)
        {
        }
        public IQueryable<VwProduction> GetVwProductionList(Expression<Func<VwProduction, bool>> predicates)
        {
            return Context.VwProductions.Where(predicates);
        }

        private string GetRefNumber(int maxNumericValue, int length)
        {
            var refNumber = Convert.ToString(maxNumericValue + 1);
            while (refNumber.Length != length)
            {
                refNumber = "0" + refNumber;
            }
            return refNumber;
        }
    }
}
