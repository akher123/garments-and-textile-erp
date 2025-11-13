using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using SCERP.DAL.IRepository.IMerchandisingRepository;
using SCERP.Model.MerchandisingModel;
using SCERP.Model.Production;

namespace SCERP.DAL.Repository.MerchandisingRepository
{
    public class FabricOrderRepository:Repository<OM_FabricOrder>,IFabricOrderRepository
    {
        public FabricOrderRepository(SCERPDBContext context) : base(context)
        {
        }

        public List<VwCompConsumptionOrderStyle> GeFabricConsStyleList(string orderNo, Guid? employeeId, string compId)
        {
            
            string sqlQuery = String.Format(@"select * from VwCompConsumptionOrderStyle where  OrderNo= '{2}'  and CompId='{0}' and MerchandiserId in ( select MerchandiserRefId  from UserMerchandiser where CompId='{0}' and EmployeeId='{1}')", compId, employeeId, orderNo);
            return Context.Database.SqlQuery<VwCompConsumptionOrderStyle>(sqlQuery).ToList();
        }

        public List<VwCompConsumptionOrderStyle> GeFabricOrderDetailById(int fabricOrderId, string compId)
        {
            string sqlQuery = String.Format(@"select c.* from VwCompConsumptionOrderStyle as c
                                     inner join OM_FabricOrderDetail as FC on C.OrderStyleRefId=FC.OrderStyleRefId and C.CompId=FC.CompId where  FC.CompId='{0}' and FabricOrderId='{1}'", compId, fabricOrderId);
            return Context.Database.SqlQuery<VwCompConsumptionOrderStyle>(sqlQuery).ToList();
        }

        public IQueryable<VwFabricOrder> GetVwFabricOrders(Expression<Func<VwFabricOrder, bool>> @where)
        {
            return Context.VwFabricOrders.Where(@where);
        }
    }
}
