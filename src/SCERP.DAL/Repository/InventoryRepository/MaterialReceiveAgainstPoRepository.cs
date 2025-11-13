using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Common;
using SCERP.DAL.IRepository.IInventoryRepository;
using SCERP.Model.InventoryModel;

namespace SCERP.DAL.Repository.InventoryRepository
{
    public class MaterialReceiveAgainstPoRepository : Repository<Inventory_MaterialReceiveAgainstPo>, IMaterialReceiveAgainstPoRepository
    {
        public MaterialReceiveAgainstPoRepository(SCERPDBContext context) : base(context)
        {
        }

        public List<VwMaterialReceiveAgainstPoDetail> VwMaterialReceiveAgainstPoDetail(long lecevedAgainstPoId)
        {
            string compId = PortalContext.CurrentUser.CompId;
            string sqlQuery =
                string.Format(
                    " select * from VwMaterialReceiveAgainstPoDetail where CompId='{0}' and MaterialReceiveAgstPoId='{1}' ",
                    compId, lecevedAgainstPoId);
            return Context.Database.SqlQuery<VwMaterialReceiveAgainstPoDetail>(sqlQuery).ToList();
        }

        public List<VwMaterialReceiveAgainstPo> GetMrrSummaryReport(string compId, DateTime? fromDate, DateTime? toDate, string searchString)
        {
           var vwMaterialReceiveAgainstPo= Context.VwMaterialReceiveAgainstPos.Where(x => x.CompId == compId
                                                           &&
                                                           ((x.MRRNo.Trim()
                                                               .ToLower()
                                                               .Contains(searchString.Trim().ToLower()) ||
                                                             String.IsNullOrEmpty(searchString))
                                                            ||
                                                            (x.InvoiceNo.Trim()
                                                                .ToLower()
                                                                .Contains(searchString.Trim().ToLower()) ||
                                                             String.IsNullOrEmpty(searchString))
                                                            ||
                                                            (x.PoNo.Trim()
                                                                .ToLower()
                                                                .Contains(searchString.Trim().ToLower()) ||
                                                             String.IsNullOrEmpty(searchString))
                                                            ||
                                                            (x.RefNo.Trim()
                                                                .ToLower()
                                                                .Contains(searchString.Trim().ToLower()) ||
                                                             String.IsNullOrEmpty(searchString))
                                                            ||
                                                            (x.CompanyName.Trim()
                                                                .ToLower()
                                                                .Contains(searchString.Trim().ToLower()) ||
                                                             String.IsNullOrEmpty(searchString)))
                                                           &&
                                                           ((x.MRRDate >= fromDate || fromDate == null) &&
                                                            (x.MRRDate <= toDate || toDate == null))).ToList();

            return vwMaterialReceiveAgainstPo;
        }

        public DataTable GetAccessoriesStatusDataTable(string orderStyleRefId, string compId)
        {
            SqlConnection connection = (SqlConnection)Context.Database.Connection;

            using (SqlCommand cmd = new SqlCommand("spInvAccessoriesStatus"))
            {
                cmd.Connection = connection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@OrderStyleRefId", SqlDbType.VarChar).Value = orderStyleRefId;
                cmd.Parameters.Add("@CompId", SqlDbType.VarChar).Value = compId;
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    return dt;
                }
            }
        }

        public DataTable GetAccessoriesRcvDetailStatus(DateTime? fromDate, DateTime? toDate, string compId)
        {
            SqlConnection connection = (SqlConnection)Context.Database.Connection;

            using (SqlCommand cmd = new SqlCommand("InvSpAccessoriesRecvDetailStatus"))
            {
                cmd.Connection = connection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = fromDate;
                cmd.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = toDate;
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
