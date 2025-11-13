using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;
using SCERP.Model.MerchandisingModel;

namespace SCERP.DAL.IRepository.IMerchandisingRepository
{
    public interface IBuyOrdShipDetailRepository:IRepository<OM_BuyOrdShipDetail>
    {
        IQueryable<VBuyOrdShipDetail> GetVBuyOrdShipDetail(Expression<Func<VBuyOrdShipDetail, bool>> predicate);
        IQueryable<VOrderStyleDetail> GetVOrderStyleDetails(Expression<Func<VOrderStyleDetail, bool>> predicate);
    }
}
