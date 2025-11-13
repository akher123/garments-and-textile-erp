using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using SCERP.DAL.IRepository.IMerchandisingRepository;
using SCERP.Model;
using SCERP.Model.MerchandisingModel;
using SCERP.Model.Production;

namespace SCERP.DAL.Repository.MerchandisingRepository
{
    public class BuyOrdShipDetailRepository :Repository<OM_BuyOrdShipDetail>, IBuyOrdShipDetailRepository
    {
        public BuyOrdShipDetailRepository(SCERPDBContext context) : base(context)
        {
        }

        public IQueryable<VBuyOrdShipDetail> GetVBuyOrdShipDetail(Expression<Func<VBuyOrdShipDetail, bool>> predicate)
        {
           return Context.VBuyOrdShipDetails.Where(predicate);
        }

        public IQueryable<VOrderStyleDetail> GetVOrderStyleDetails(Expression<Func<VOrderStyleDetail, bool>> predicate)
        {
            return Context.VOrderStyleDetails.OrderBy(x=>x.ColorName).ThenBy(x=>x.ColorName).Where(predicate);
        }
    }
}
