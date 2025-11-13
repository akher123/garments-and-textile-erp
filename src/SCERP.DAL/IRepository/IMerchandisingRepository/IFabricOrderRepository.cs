using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.MerchandisingModel;
using SCERP.Model.Production;

namespace SCERP.DAL.IRepository.IMerchandisingRepository
{
   public interface IFabricOrderRepository:IRepository<OM_FabricOrder>
   {
       List<VwCompConsumptionOrderStyle> GeFabricConsStyleList(string orderNo, Guid? employeeId, string compId);
       List<VwCompConsumptionOrderStyle> GeFabricOrderDetailById(int fabricOrderId, string compId);

       IQueryable<VwFabricOrder> GetVwFabricOrders(Expression<Func<VwFabricOrder, bool>> @where);
   }
}
