using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.MerchandisingModel;

namespace SCERP.DAL.IRepository.IMerchandisingRepository
{
    public interface IFabricOrderDetailRepository :IRepository<OM_FabricOrderDetail>
    {
        IQueryable<VwFabricOrderDetail> GetVwFabricOrders(Expression<Func<VwFabricOrderDetail, bool>> @where);

    }
}
