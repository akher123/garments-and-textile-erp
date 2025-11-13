using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using SCERP.DAL.IRepository.IInventoryRepository;
using SCERP.Model.InventoryModel;

namespace SCERP.DAL.Repository.InventoryRepository
{
    public class StyleShipmentRepository : Repository<Inventory_StyleShipment>, IStyleShipmentRepository
    {
        public StyleShipmentRepository(SCERPDBContext context) : base(context)
        {
        }

        public List<SpInventoryStyleShipment> GetStyleShipment(string orderStyleRefId, string compId, long styleShipmentId)
        {
            
           var parameters=new[]
           {
               new SqlParameter()
               {
                   ParameterName = "OrderStyleRefId",
                   DbType = DbType.String,
                   Value = orderStyleRefId,

               }
               , 
               new SqlParameter()
               {
                  ParameterName = "StyleShipmentId",
                   DbType = DbType.Int64,
                   Value =styleShipmentId,
               }, 
                 new SqlParameter()
               {
                  ParameterName = "CompId",
                   DbType = DbType.String,
                   Value = compId,
               }, 
           };

           return Context.Database.SqlQuery<SpInventoryStyleShipment>("SpInventoryStyleShipment @OrderStyleRefId,@StyleShipmentId,@CompId", parameters).ToList();
          
        }

        public DataTable GetShipmentChallan(long styleShipmentId)
        {
            SqlConnection connection = (SqlConnection)Context.Database.Connection;
            using (SqlCommand cmd = new SqlCommand("SpInventoryStyleShipmentChallan"))
            {
                cmd.Connection = connection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@StyleShipmentId", SqlDbType.BigInt).Value = styleShipmentId;
           
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    return dt;
                }
            }
        }

        public IQueryable<VwInventoryStyleShipment> GetStyleShipmentByPaging(string buyerRefId, string searchKey, string compId)
        {
            return Context.VwInventoryStyleShipments.Where(x => x.CompId == compId &&(x.BuyerRefId == buyerRefId || buyerRefId == null)
                && ((x.InvoiceNo.Trim().Contains(searchKey) || String.IsNullOrEmpty(searchKey))
                || (x.RefNo.Trim().Contains(searchKey) || String.IsNullOrEmpty(searchKey))
                || (x.StyleName.Trim().Contains(searchKey) || String.IsNullOrEmpty(searchKey))));
        }

        public IQueryable<VwInventoryStyleShipment> GetApprovedStyleShipmentByPaging(string compId, bool isApproved)
        {
            return Context.VwInventoryStyleShipments.Where(x => x.CompId == compId && (x.IsApproved == isApproved || isApproved == null));
        }

        public DataTable GetStockPostionDetail(string compId, string buyerRefId)
        {
            SqlConnection connection = (SqlConnection)Context.Database.Connection;
            using (SqlCommand cmd = new SqlCommand("SpinventoryShipStatus"))
            {
                cmd.Connection = connection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@buyerRefId", SqlDbType.VarChar).Value = buyerRefId;
                cmd.Parameters.Add("@CompId", SqlDbType.VarChar).Value = compId;

                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    return dt;
                }
            }
        }

        public DataTable GetMonthlyShipmentSummary(string compId, DateTime? fromDate, DateTime? toDate)
        {
            SqlConnection connection = (SqlConnection)Context.Database.Connection;
            using (SqlCommand cmd = new SqlCommand("SpMonthlyShipmentSummary"))
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
