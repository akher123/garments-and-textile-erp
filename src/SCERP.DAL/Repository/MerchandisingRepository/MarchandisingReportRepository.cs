using System;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using SCERP.DAL.IRepository.IMerchandisingRepository;

namespace SCERP.DAL.Repository.MerchandisingRepository
{
    public class MarchandisingReportRepository : Repository<Object>, IMarchandisingReportRepository
    {

        public MarchandisingReportRepository(SCERPDBContext context)
            : base(context)
        {

        }

        public DataTable GetBuyerWiseOrderSummaryDataTable(DateTime? fromDate, DateTime? toDate, string buyerRefId, string companyId)
        {
                 const string nulValueResolver = "-1";
            SqlConnection connection = (SqlConnection) Context.Database.Connection;
            
                using (SqlCommand cmd = new SqlCommand("SPOmBuyerWiseOrderSummary"))
                {
                    cmd.Connection = connection;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = fromDate;
                    cmd.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = toDate;
                    cmd.Parameters.Add("@BuyerRefId", SqlDbType.VarChar).Value = buyerRefId ?? nulValueResolver;
                    cmd.Parameters.Add("@CompId", SqlDbType.VarChar).Value = companyId;
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        sda.Fill(dt);
                        return dt;
                    }
                
            }

        }

        public DataTable GetConfirmedOrderStatus(string companyId)
        {
            SqlConnection connection = (SqlConnection) Context.Database.Connection;
            
                using (SqlCommand cmd = new SqlCommand("SPOmConfirmedOderStatus"))
                {
                    cmd.Connection = connection;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompId", SqlDbType.VarChar).Value = companyId;
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        sda.Fill(dt);
                        return dt;
                    }
                
            }
        }

        public DataTable GetDetailOrderStatus(string companyId)
        {
            SqlConnection connection = (SqlConnection) Context.Database.Connection;
            
                using (SqlCommand cmd = new SqlCommand("SPOmDetailOderStatus"))
                {
                    cmd.Connection = connection;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompId", SqlDbType.VarChar).Value = companyId;
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        sda.Fill(dt);
                        return dt;
                    }
                
            }
        }

        public DataTable GetProductionStatus(string companyId, DateTime? fromDate, DateTime? toDate)
        {
            SqlConnection connection = (SqlConnection) Context.Database.Connection;
            
                using (SqlCommand cmd = new SqlCommand("SpOmProductionStatus"))
                {
                    cmd.Connection = connection;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = fromDate;
                    cmd.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = toDate;
                    cmd.Parameters.Add("@CompId", SqlDbType.VarChar).Value = companyId;
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        sda.Fill(dt);
                        return dt;
                    }
                
            }
        }

        public DataTable GetShipmentStatus(string seasonRefId, string merchandiserId, string buyerRefId, string companyId)
        {
            string nullValueResolver = "-1";
            SqlConnection connection = (SqlConnection) Context.Database.Connection;
            
                using (SqlCommand cmd = new SqlCommand("SpOmShipmentStatus"))
                {
                    cmd.Connection = connection;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompId", SqlDbType.VarChar).Value = companyId;
                    cmd.Parameters.Add("@BuyerRefId", SqlDbType.VarChar).Value = buyerRefId ?? nullValueResolver;
                    cmd.Parameters.Add("@MerchandiserId", SqlDbType.VarChar).Value = merchandiserId ?? nullValueResolver;
                    cmd.Parameters.Add("@SeasonRefId", SqlDbType.VarChar).Value = seasonRefId ?? nullValueResolver;
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        sda.Fill(dt);
                        return dt;
                    }
                
            }
        }

        public DataTable GetPandingConsumptionDataTable(string merchandiserId, string companyId)
        {
            string nullValueResolver = "-1";
            SqlConnection connection = (SqlConnection)Context.Database.Connection;

            using (SqlCommand cmd = new SqlCommand("SpOmPandingConsumptionReport"))
            {
                cmd.Connection = connection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompId", SqlDbType.VarChar).Value = companyId;
                cmd.Parameters.Add("@MerchandiserId", SqlDbType.VarChar).Value = merchandiserId ?? nullValueResolver;
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    return dt;
                }

            }
        }

        public DataTable GetPoSheet(long purchaseOrderId, string companyId)
        {
           
            SqlConnection connection = (SqlConnection)Context.Database.Connection;
            using (SqlCommand cmd = new SqlCommand("SpPoSheet"))
            {
                cmd.Connection = connection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompId", SqlDbType.VarChar).Value = companyId;
                cmd.Parameters.Add("@PurchaseOrderId", SqlDbType.VarChar).Value = purchaseOrderId;
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    return dt;
                }
              
            }


        }

        public DataTable GetFabricWorkOrderSheet(string orderStyleRefId, string companyId)
        {
            SqlConnection connection = (SqlConnection)Context.Database.Connection;
            using (SqlCommand cmd = new SqlCommand("SpOmFabricWorkOrderSheet"))
            {
                cmd.Connection = connection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@OrderStyleRefId", SqlDbType.VarChar).Value = orderStyleRefId;
                cmd.Parameters.Add("@CompId", SqlDbType.VarChar).Value = companyId;
            
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    return dt;
                }

            }
        }

        public DataTable GetFabricWorkOrderDetailSheet(string orderStyleRefId, string companyId)
        {
            SqlConnection connection = (SqlConnection)Context.Database.Connection;
            using (SqlCommand cmd = new SqlCommand("SpOmFabricWorkOrderDetailReport"))
            {
                cmd.Connection = connection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@OrderStyleRefId", SqlDbType.VarChar).Value = orderStyleRefId;
                cmd.Parameters.Add("@CompId", SqlDbType.VarChar).Value = companyId;

                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    return dt;
                }

            }
        }

        public DataTable GetBulkFabricOrderSheet(int fabricOrderId, string companyId)
        {
            SqlConnection connection = (SqlConnection)Context.Database.Connection;
            using (SqlCommand cmd = new SqlCommand("SpOmBulkFabricOrderReport"))
            {
                cmd.Connection = connection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@FabricOrderId", SqlDbType.VarChar).Value = fabricOrderId;
                cmd.Parameters.Add("@CompId", SqlDbType.VarChar).Value = companyId;

                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    return dt;
                }

            }
        }

        public DataTable GetRunningOrderStatus(string companyId)
        {
            SqlConnection connection = (SqlConnection)Context.Database.Connection;
            using (SqlCommand cmd = new SqlCommand("SpOmRunningOrderSumamry"))
            {
                cmd.Connection = connection;
                cmd.CommandType = CommandType.StoredProcedure;
         
                cmd.Parameters.Add("@CompId", SqlDbType.VarChar).Value = companyId;

                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    return dt;
                }

            }
        }

        public DataTable GetSeasonWiseOrderSummary(DateTime? fromDate, DateTime? todate, string companyId)
        {
            SqlConnection connection = (SqlConnection)Context.Database.Connection;
            using (SqlCommand cmd = new SqlCommand("SpOmSeasonWiseOrderSummaryReport"))
            {
                cmd.Connection = connection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompId", SqlDbType.VarChar).Value = companyId;
                cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = fromDate;
                cmd.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = todate;
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    return dt;
                }

            }
        }

        public int SendMailExecut()
        {
            string sqlQuery =
                @"EXEC msdb.dbo.sp_send_dbmail @profile_name = 'softcode', 
                @recipients = 'akher.ice07@gmail.com',@copy_recipients = 'shahanur2000bd@hotmail.com;sayeedseb@gmail.com',
                @body = 'Test',   @subject = 'Dyeing Production ', @body_format='HTML'; ";
           return Context.Database.ExecuteSqlCommand(sqlQuery);
        }

        public DataTable GetYarnWorkOrderSheet(long purchaseOrderId, string companyId)
        {
            SqlConnection connection = (SqlConnection)Context.Database.Connection;
            using (SqlCommand cmd = new SqlCommand("SpOmYarnWorkOrderReport"))
            {
                cmd.Connection = connection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompId", SqlDbType.VarChar).Value = companyId;
                cmd.Parameters.Add("@PurchaseOrderId", SqlDbType.VarChar).Value = purchaseOrderId;
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    return dt;
                }

            }
        }

        public DataTable GetBulkYarnBooking(long bulkBookingId, string companyId)
        {

            SqlConnection connection = (SqlConnection)Context.Database.Connection;
            using (SqlCommand cmd = new SqlCommand("SpOmBulkYarnBooking"))
            {
                cmd.Connection = connection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompId", SqlDbType.VarChar).Value = companyId;
                cmd.Parameters.Add("@BulkBookingId", SqlDbType.BigInt).Value = bulkBookingId;
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    return dt;
                }

            }
        }

        public DataTable GetOrderShipmentSummary(DateTime? fromDate, DateTime? toDate, Guid? userId, string companyId)
        {
            SqlConnection connection = (SqlConnection)Context.Database.Connection;
            using (SqlCommand cmd = new SqlCommand("BuyerOrderShipSummary"))
            {
                cmd.Connection = connection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompId", SqlDbType.VarChar).Value = companyId;
                cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = fromDate;
                cmd.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = toDate;
                cmd.Parameters.Add("@EmployeeId", SqlDbType.UniqueIdentifier).Value = userId;
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    return dt;
                }

            }
        }

        public DataTable GetRunningOrderOrderStatus(string compId)
        {
            SqlConnection connection = (SqlConnection)Context.Database.Connection;
            using (SqlCommand cmd = new SqlCommand("SpOMRunningOrderStatus"))
            {
                cmd.Connection = connection;
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@CompId", SqlDbType.VarChar).Value = compId;

                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    return dt;
                }

            }
        }

        public DataTable GetPreCostSheet(string orderStyleRefId, string compId)
        {
            SqlConnection connection = (SqlConnection)Context.Database.Connection;
            using (SqlCommand cmd = new SqlCommand("SpOMCostSheet"))
            {
                cmd.Connection = connection;
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@CompId", SqlDbType.VarChar).Value = compId;
                cmd.Parameters.Add("@OrderStyleRefId", SqlDbType.VarChar).Value = orderStyleRefId;
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    return dt;
                }

            }
        }

        public DataTable GetStyleWiseProduction(string orderStyleRefId, string compId)
        {
            SqlConnection connection = (SqlConnection)Context.Database.Connection;
            using (SqlCommand cmd = new SqlCommand("SpProdStypeWiseProductionStatus"))
            {
                cmd.Connection = connection;
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@CompId", SqlDbType.VarChar).Value = compId;
                cmd.Parameters.Add("@OrderStyleRefId", SqlDbType.VarChar).Value = orderStyleRefId;
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    return dt;
                }

            }
        }

        public DataTable GetCollarCuffBulkFabricOrderSheet(int fabricOrderId, string companyId)
        {
            SqlConnection connection = (SqlConnection)Context.Database.Connection;
            using (SqlCommand cmd = new SqlCommand("SpOmCollarCuffBulkFabricOrderReport"))
            {
                cmd.Connection = connection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@FabricOrderId", SqlDbType.VarChar).Value = fabricOrderId;
                cmd.Parameters.Add("@CompId", SqlDbType.VarChar).Value = companyId;

                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    return dt;
                }

            }
        }

        public DataTable GetShipmentAlert(string buyerRefId, string orderNo, string orderStyleRefId, string compId)
        {
            SqlConnection connection = (SqlConnection)Context.Database.Connection;
          
            using (SqlCommand cmd = new SqlCommand("SpReportShipmentAlert"))
            {
                cmd.Connection = connection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@buyerRefId", SqlDbType.VarChar).Value = buyerRefId ?? "";
                cmd.Parameters.Add("@orderNo", SqlDbType.VarChar).Value = orderNo ?? "";
                cmd.Parameters.Add("@orderStyleRefId", SqlDbType.VarChar).Value = orderStyleRefId ?? "";
                cmd.Parameters.Add("@compId", SqlDbType.VarChar).Value = compId;
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    return dt;
                }

            }
        }

        public string GetInWord(decimal sumTotal)
        {
               SqlConnection connection = (SqlConnection)Context.Database.Connection;

            try
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("SELECT  dbo.fnNumberToWords(" + sumTotal + ")");

                cmd.Connection = connection;
             

                return cmd.ExecuteScalar().ToString();
            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message);
            }
            finally
            {
                if (connection.State==ConnectionState.Open)
                {
                    connection.Close();
                }
            }
               
            
        }
    }
}
