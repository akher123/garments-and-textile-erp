using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using SCERP.Common;
using SCERP.DAL.IRepository.ICommercialRepository;
using SCERP.DAL.IRepository.IMerchandisingRepository;
using SCERP.Model;


namespace SCERP.DAL.Repository.CommercialRepository
{
    public class LcOrderRepository : Repository<OM_BuyerOrder>, ILcOrderRepository
    {
        public LcOrderRepository(SCERPDBContext context)
            : base(context)
        {
        }

        public IQueryable<VBuyerOrder> GetBuyerOrderViews(Expression<Func<VBuyerOrder, bool>> predicate)
        {
            return Context.VBuyerOrders.Where(predicate);
        }
    }
}
