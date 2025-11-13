using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using SCERP.DAL.IRepository.IMerchandisingRepository;
using SCERP.Model.MerchandisingModel;

namespace SCERP.DAL.Repository.MerchandisingRepository
{
   public class FabricOrderDetailRepository:Repository<OM_FabricOrderDetail>,IFabricOrderDetailRepository
    {
       public FabricOrderDetailRepository(SCERPDBContext context) : base(context)
       {
       }

       public IQueryable<VwFabricOrderDetail> GetVwFabricOrders(Expression<Func<VwFabricOrderDetail, bool>> @where)
       {
           return Context.VwStyleFabricOrderDetails.AsNoTracking().Where(@where);
       }
    }
}
