using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using SCERP.Common;
using SCERP.DAL.IRepository.IMerchandisingRepository;
using SCERP.Model;

namespace SCERP.DAL.Repository.MerchandisingRepository
{
    public class BuyerOrderRepository : Repository<OM_BuyerOrder>, IBuyerOrderRepository
    {

        public BuyerOrderRepository(SCERPDBContext context)
            : base(context)
        {
        }

        public IQueryable<VBuyerOrder> GetBuyerOrderViews(Expression<Func<VBuyerOrder, bool>> predicate)
        {
            return Context.VBuyerOrders.Where(predicate);
        }

        public string GetNewRefNo(string compId)
        {
            var sqlQuery = String.Format("Select ISNULL(MAX(substring(OrderNo,8,12)),0) as OrderNo from OM_BuyerOrder where CompId='{0}'", compId);
            var issueReceiveNo =
              Context.Database.SqlQuery<string>(sqlQuery)
                  .SingleOrDefault() ?? "0";
            var maxNumericValue = Convert.ToInt32(issueReceiveNo);
            var irNo = "PFL/ORD" + GetRefNumber(maxNumericValue, 5); // PFL/ORD
            return irNo;
        }

        public DataTable GetMerchaiserWiseOrderDataTable(DateTime? fromDate, DateTime? toDate)
        {
            

          
            using (SqlConnection connection = (SqlConnection)Context.Database.Connection)
            {
                using (SqlCommand cmd = new SqlCommand("SPOmMerchandiserWiseOrderSummary"))
                {
                    cmd.Connection = connection;
            
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = fromDate;
                    cmd.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = toDate;
                    cmd.Parameters.Add("@CompId", SqlDbType.VarChar).Value = PortalContext.CurrentUser.CompId;
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        sda.Fill(dt);
                        return dt;
                    }
                }
            }
          
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
