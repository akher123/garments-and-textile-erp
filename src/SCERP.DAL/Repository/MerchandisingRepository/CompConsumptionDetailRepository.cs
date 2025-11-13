using System;
using System.Linq;
using System.Linq.Expressions;
using SCERP.DAL.IRepository.IMerchandisingRepository;
using SCERP.Model;
using SCERP.Model.MerchandisingModel;

namespace SCERP.DAL.Repository.MerchandisingRepository
{
    public class CompConsumptionDetailRepository :Repository<OM_CompConsumptionDetail>, ICompConsumptionDetailRepository
    {
        public CompConsumptionDetailRepository(SCERPDBContext context) : base(context)
        {
        }

        public IQueryable<VCompConsumptionDetail> GetVCompConsumptionDetails(Expression<Func<VCompConsumptionDetail, bool>> predicate)
        {
            return Context.VCompConsumptionDetails.Where(predicate);
        }
    }
}
