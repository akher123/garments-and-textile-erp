using System.Data.SqlClient;
using System.Linq;
using System.Text;
using SCERP.DAL.IRepository.IMerchandisingRepository;
using SCERP.Model.MerchandisingModel;

namespace SCERP.DAL.Repository.MerchandisingRepository
{
    public class MCostingRepository : Repository<OM_Costing>, IMCostingRepository
    {
        public MCostingRepository(SCERPDBContext context) : base(context)
        {
        }

        public OM_Costing GetUpdatedCosting(int costingId)
        {
            string sql = @"exec [dbo].[spCalcualteCosting] @CostingId";
            return Context.Database.SqlQuery<OM_Costing>(sql,new SqlParameter("@CostingId", costingId)).FirstOrDefault();
        }

        public int UpdateCosting(int costingId, string fieldName, string value)
        {
            string sql =string.Format(@"update OM_Costing set {0}='{1}' where CostingId={2}", fieldName, value, costingId);
            return Context.Database.ExecuteSqlCommand(sql);
            //return  Context.Database.ExecuteSqlCommand(sql,  
            //    new SqlParameter("@column_name", fieldName),
            //    new SqlParameter("@column_Value", value),
            //    new SqlParameter("@costingId", costingId));


        }
    }
}
