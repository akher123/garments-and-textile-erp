using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.DAL.IRepository.IMaintenance;
using SCERP.Model.Maintenance;

namespace SCERP.DAL.Repository.Maintenance
{
    public class ReturnableChallanRepository : Repository<Maintenance_ReturnableChallan>, IReturnableChallanRepository
    {
        private readonly SCERPDBContext _context;
        public ReturnableChallanRepository(SCERPDBContext context)
            : base(context)
        {
            _context = context;
        }

        public List<VwReturnableChallan> GetReturnableChallanForReport(long returnableChallanId, string compId)
        {
            string sqlQuery = String.Format(@"select  *from  VwReturnableChallan where CompId='{0}' and ReturnableChallanId='{1}'", compId, returnableChallanId);
            return Context.Database.SqlQuery<VwReturnableChallan>(sqlQuery).ToList();
        }

        public DataTable GetReturnableChallanInfo(DateTime? dateFrom, DateTime? dateTo,string challanType, int challanStatus, string compId)
        {
            SqlConnection connection = (SqlConnection)_context.Database.Connection;

            using (SqlCommand cmd = new SqlCommand("SpReturnableChallan"))
            {
                cmd.Connection = connection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = dateFrom;
                cmd.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = dateTo;
                cmd.Parameters.Add("@ReceiveType", SqlDbType.Int).Value = challanStatus;
                cmd.Parameters.Add("@ChallanType", SqlDbType.Char).Value = challanType;
                cmd.Parameters.Add("@CompId", SqlDbType.VarChar).Value = compId;
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    return dt;
                }
            }
        }
    }
}
