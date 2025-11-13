using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.DAL.IRepository.IMaintenance;
using SCERP.Model.Maintenance;
using SCERP.Model.Production;

namespace SCERP.DAL.Repository.Maintenance
{
    public class ReturnableChallanReceiveMasterRepository : Repository<Maintenance_ReturnableChallanReceiveMaster>, IReturnableChallanReceiveMasterRepository
   {
        private readonly SCERPDBContext _context;
        public ReturnableChallanReceiveMasterRepository(SCERPDBContext context) : base(context)
        {

            _context = context;
        }


        public DataTable GetReturnableChallanReceive(long returnableChallanReceiveMasterId, string compId)
        {

            SqlConnection connection = (SqlConnection)_context.Database.Connection;
            using (SqlCommand cmd = new SqlCommand("SpReturnableChallanReceiveReport"))
            {
                cmd.Connection = connection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ReturnableChallanReceiveMasterId", SqlDbType.Int).Value = returnableChallanReceiveMasterId;
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
