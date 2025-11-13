using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;
using SCERP.Model.MerchandisingModel;
using SCERP.Model.Production;
using System.Linq.Expressions;

namespace SCERP.DAL.IRepository.ICommercialRepository
{
    public interface ILcOrderRepository
    {
        IQueryable<VBuyerOrder> GetBuyerOrderViews(Expression<Func<VBuyerOrder, bool>> predicate);
    }
}
