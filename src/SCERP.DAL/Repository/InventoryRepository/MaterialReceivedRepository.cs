using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.DAL.IRepository.IInventoryRepository;
using SCERP.Model.InventoryModel;

namespace SCERP.DAL.Repository.InventoryRepository
{
   public class MaterialReceivedRepository:Repository<Inventory_MaterialReceived>, IMaterialReceivedRepository
    {
        private readonly SCERPDBContext _context;
        public MaterialReceivedRepository(SCERPDBContext context) : base(context)
        {
            _context = context;
        }

       public DataTable GetMaterialReceivedDataTable(DateTime? fromDate, DateTime? toDate, string challanNo,string registerType, string compId)
       {
            SqlConnection connection = (SqlConnection)_context.Database.Connection;

            using (SqlCommand cmd = new SqlCommand("SPGetMaterialReceived"))
            {
                cmd.Connection = connection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = fromDate;
                cmd.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = toDate;
                cmd.Parameters.Add("@ChallanNo", SqlDbType.VarChar).Value = challanNo ?? "";
                cmd.Parameters.Add("@RegisterType", SqlDbType.VarChar).Value = registerType ?? "";
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
