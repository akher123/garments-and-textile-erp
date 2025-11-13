using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using SCERP.DAL.IRepository.IMerchandisingRepository;
using SCERP.Model;
using SCERP.Model.MerchandisingModel;

namespace SCERP.DAL.Repository.MerchandisingRepository
{
    public class ConsumptionDetailRepository : Repository<OM_ConsumptionDetail>, IConsumptionDetailRepository
    {
        public ConsumptionDetailRepository(SCERPDBContext context)
            : base(context)
        {
        }

        public List<VConsumptionDetail> GetVConsumptionDetails(string consRefId, string compId)
        {
            return Context.VConsumptionDetails.Where(x => x.ConsRefId == consRefId && x.CompId == compId).OrderBy(x=>x.GColorRefId).OrderBy(x=>x.SizeRow).ToList();
        }

        public DataTable GetGColorList(string orderStyleRefId, string compId)
        {
            var sqlQuirey = String.Format("select distinct (Select Top(1) ColorName  from OM_Color where SD.ColorRefId=ColorRefId and CompId=SD.CompId) as ColorName,ColorRefId  from VBuyOrdShipDetail as SD where SD.OrderStyleRefId='{0}' and SD.CompId='{1}'", orderStyleRefId, compId);
            return ExecuteQuery(sqlQuirey);
        }

        public DataTable GetGSizeList(string orderStyleRefId, string compId)
        {
            var sqlQuirey =
                String.Format(
                    "select distinct (Select Top(1) SizeName from OM_Size where SD.SizeRefId=SizeRefId and SD.CompId=CompId) as SizeName,SizeRefId from VBuyOrdShipDetail as SD where SD.OrderStyleRefId='{0}' and CompId='{1}'",
                    orderStyleRefId, compId);
            return ExecuteQuery(sqlQuirey);
        }

        public DataTable GetVConsumptionDetailsByStyleRefId(string orderStyleRefId, string compId)
        {

            SqlConnection connection = (SqlConnection)Context.Database.Connection;

            using (SqlCommand cmd = new SqlCommand("SpMrcFabricConsumptionDetail"))
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

        public DataTable GetAccessoriesConsumptionDetail(string orderStyleRefId, string compId)
        {

            SqlConnection connection = (SqlConnection)Context.Database.Connection;

            using (SqlCommand cmd = new SqlCommand("SpMrcAccessoriesConsumptionDetail"))
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

        public List<SPOrderStyleDetailForBOM> GetOrderStyleDetailForBOM(string orderStyleRefId, string compId)
        {
            var sqParams = new object[]
            {
                new SqlParameter {ParameterName = "CompId", Value = compId},
                new SqlParameter {ParameterName = "OrderStyleRefId", Value = orderStyleRefId},

            };
            return Context.Database.SqlQuery<SPOrderStyleDetailForBOM>("SpOrderStyleDetailForBOM @CompId ,@OrderStyleRefId", sqParams).ToList();
        }

        public DataTable GetAccessoriesConsumptionDetailByOrder(string orderNo, string compId)
        {
            SqlConnection connection = (SqlConnection)Context.Database.Connection;

            using (SqlCommand cmd = new SqlCommand("SpMrcOrderWiseAccessoriesConsumptionDetail"))
            {
                cmd.Connection = connection;
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@OrderNo", SqlDbType.VarChar).Value = orderNo;
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
