using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using SCERP.DAL.IRepository.IMerchandisingRepository;
using SCERP.Model;
using SCERP.Model.MerchandisingModel;

namespace SCERP.DAL.Repository.MerchandisingRepository
{
    public class CompConsumptionRepository :Repository<OM_CompConsumption>, ICompConsumptionRepository
    {
        public CompConsumptionRepository(SCERPDBContext context) : base(context)
        {
        }

        public IQueryable<VCompConsumption> GetVCompConsumptions(Expression<Func<VCompConsumption, bool>> predicate)
        {
            return Context.VCompConsumptions.Where(predicate);
        }

        public IQueryable<VwCompConsumptionOrderStyle> GetVwCompConsumptionOrderStyle(Expression<Func<VwCompConsumptionOrderStyle, bool>> predicate)
        {
            return Context.VwCompConsumptionOrderStyles.Where(predicate);
        }

        public IQueryable<VwCompConsumptionOrderStyle> GetVwCompConsumptionOrderStyle(string compId, Guid? employeeId,string serchString)
        {
            string serchKey = serchString ??"";
            string sqlQuery = String.Format(@"select * from VwCompConsumptionOrderStyle where  (StyleName like '%{2}%' or RefNo like '%{2}%' or OrderStyleRefId like '%{2}%') and CompId='{0}' and MerchandiserId in ( select MerchandiserRefId  from UserMerchandiser where CompId='{0}' and EmployeeId='{1}')", compId, employeeId,serchKey);
            return  Context.Database.SqlQuery<VwCompConsumptionOrderStyle>(sqlQuery).AsQueryable();
        }
    }
}
