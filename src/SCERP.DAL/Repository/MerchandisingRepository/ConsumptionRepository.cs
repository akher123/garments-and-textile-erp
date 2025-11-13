using System;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Remoting.Contexts;
using SCERP.DAL.IRepository.IMerchandisingRepository;
using SCERP.Model;
using SCERP.Model.MerchandisingModel;

namespace SCERP.DAL.Repository.MerchandisingRepository
{
    public class ConsumptionRepository :Repository<OM_Consumption>, IConsumptionRepository
    {
        public ConsumptionRepository(SCERPDBContext context) : base(context)
        {

        }

        public IQueryable<VConsumption> GetConsumptions(Expression<Func<VConsumption, bool>> predicate)
        {
            return Context.VConsumptions.Where(predicate);
        }

        public string GetNewConsRefId(string compId)
        {
            var sqlQuery = String.Format("Select  substring(MAX(ConsRefId),5,10)from OM_Consumption where CompId='{0}'",
               compId);
            var issueReceiveNo =
              Context.Database.SqlQuery<string>(sqlQuery)
                  .SingleOrDefault() ?? "0";
            var maxNumericValue = Convert.ToInt32(issueReceiveNo);
            var irNo = "CON/" + GetRefNumber(maxNumericValue, 6); // CON/
            return irNo;
        }

        public int UpdateFabricConsuption(string consRefId,string compid)
        {
            var consRefIdParam = new SqlParameter { ParameterName = "ConsRefId", Value = consRefId };
            var compidParam = new SqlParameter { ParameterName = "CompId", Value = compid };
            return Context.Database.ExecuteSqlCommand("SpUpdateFabricConsumption @ConsRefId,@CompId", consRefIdParam, compidParam);
        }

        public IQueryable<VwConsuptionOrderStyle> GetVwConsuptionOrderStyle(Expression<Func<VwConsuptionOrderStyle, bool>> where)
        {
            return Context.VwConsuptionOrderStyles.Where(where);
        }

        public IQueryable<VwConsuptionOrderStyle> GetVwConsuptionStyle(string compId, Guid? employeeId,string searchString)
        {
            string lowerString = searchString??"";
            string sqlQuery = String.Format(@"select * from VwConsuptionOrderStyle where  (StyleName like '%{2}%' or RefNo like '%{2}%' or OrderStyleRefId like '%{2}%') and CompId='{0}'  and MerchandiserId in ( select MerchandiserRefId  from UserMerchandiser where CompId='{0}' and EmployeeId='{1}')", compId, employeeId, lowerString);
            return Context.Database.SqlQuery<VwConsuptionOrderStyle>(sqlQuery).AsQueryable();
           
        }

    

        private string GetRefNumber(int maxNumericValue, int length)
        {
            var refNumber = Convert.ToString(maxNumericValue + 1);
            while (refNumber.Length != length)
            {
                refNumber = "0" + refNumber;
            }
            return refNumber;
        }
    }
}
