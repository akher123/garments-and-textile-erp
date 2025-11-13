using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using SCERP.DAL.IRepository.IProductionRepository;
using SCERP.Model.Production;

namespace SCERP.DAL.Repository.ProductionRepository
{
    public class ProductionReportRepository : IProductionReportRepository
    {
        private readonly SCERPDBContext _context;
        public ProductionReportRepository(SCERPDBContext context)
        {
            _context = context;
        }
        public DataTable GetSpProdCuttiongReportSummary(string compId, string buyerRefId, string orderNo, string orderStyleRefId, string componentRefId, DateTime? cuttDate)
        {

            using (SqlConnection connection = (SqlConnection)_context.Database.Connection)
            {
                using (SqlCommand cmd = new SqlCommand("SpProdCuttingProductionSummaryReport"))
                {
                    cmd.Connection = connection;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@BuyerRefId", SqlDbType.VarChar).Value = buyerRefId;
                    cmd.Parameters.Add("@OrderNo", SqlDbType.VarChar).Value = orderNo;
                    cmd.Parameters.Add("@OrderStyleRefId", SqlDbType.VarChar).Value = orderStyleRefId;
                    cmd.Parameters.Add("@ComponentRefId", SqlDbType.VarChar).Value = componentRefId ?? "001";
                    cmd.Parameters.Add("@CompId", SqlDbType.VarChar).Value = compId;
                    cmd.Parameters.Add("@Date", SqlDbType.Date).Value = cuttDate;
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        sda.Fill(dt);
                        return dt;
                    }
                }
            }
        }


        public DataTable GetSpProdStyleWiseTagCuttingReport(string compId, string buyerRefId, string orderNo, string orderStyleRefId,
            string componentRefId)
        {
            using (SqlConnection connection = (SqlConnection)_context.Database.Connection)
            {
                using (SqlCommand cmd = new SqlCommand("SpProdStyleWiseTagCuttingReport"))
                {
                    cmd.Connection = connection;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@BuyerRefId", SqlDbType.VarChar).Value = buyerRefId;
                    cmd.Parameters.Add("@OrderNo", SqlDbType.VarChar).Value = orderNo;
                    cmd.Parameters.Add("@OrderStyleRefId", SqlDbType.VarChar).Value = orderStyleRefId;
                    cmd.Parameters.Add("@ComponentRefId", SqlDbType.VarChar).Value = componentRefId;
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


        public DataTable GetSpProdCuttiongReportDetail(string compId, string buyerRefId, string orderNo, string orderStyleRefId,
            string componentRefId, object cutDate)
        {
            SqlConnection connection = (SqlConnection)_context.Database.Connection;

            using (SqlCommand cmd = new SqlCommand("SpProdCuttingProductionDetail"))
            {
                cmd.Connection = connection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@BuyerRefId", SqlDbType.VarChar).Value = buyerRefId;
                cmd.Parameters.Add("@OrderNo", SqlDbType.VarChar).Value = orderNo;
                cmd.Parameters.Add("@OrderStyleRefId", SqlDbType.VarChar).Value = orderStyleRefId;
                cmd.Parameters.Add("@ComponentRefId", SqlDbType.VarChar).Value = componentRefId;
                cmd.Parameters.Add("@CompId", SqlDbType.VarChar).Value = compId;
                cmd.Parameters.Add("@Date", SqlDbType.Date).Value = cutDate;
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    return dt;
                }
            }
        }

        public DataTable GetJobCuttingSummary(string compId, string orderStyleRefId, string componentRefId, string colorRefId)
        {
            var connection = (SqlConnection)_context.Database.Connection;
            using (SqlCommand cmd = new SqlCommand("SpProdCuttingJobCardSummary"))
            {
                cmd.Connection = connection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@OrderStyleRefId", SqlDbType.VarChar).Value = orderStyleRefId;
                cmd.Parameters.Add("@ComponentRefId", SqlDbType.VarChar).Value = componentRefId;
                cmd.Parameters.Add("@ColorRefId", SqlDbType.VarChar).Value = colorRefId;
                cmd.Parameters.Add("@CompId", SqlDbType.VarChar).Value = compId;
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    return dt;
                }

            }
        }

        public DataTable GetJobCuttingDetail(string compId, string orderStyleRefId, string componentRefId, string colorRefId)
        {
            SqlConnection connection = (SqlConnection)_context.Database.Connection;
            using (SqlCommand cmd = new SqlCommand("SpProdCuttingJobCardDetail"))
            {
                cmd.Connection = connection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@OrderStyleRefId", SqlDbType.VarChar).Value = orderStyleRefId;
                cmd.Parameters.Add("@ComponentRefId", SqlDbType.VarChar).Value = componentRefId;
                cmd.Parameters.Add("@ColorRefId", SqlDbType.VarChar).Value = colorRefId;
                cmd.Parameters.Add("@CompId", SqlDbType.VarChar).Value = compId;
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    return dt;
                }
            }
        }

        public DataTable GetPartDesignSummary(string compId, string buyerRefId, string orderNo, string orderStyleRefId,
            string componentRefId)
        {
            using (SqlConnection connection = (SqlConnection)_context.Database.Connection)
            {
                using (SqlCommand cmd = new SqlCommand("SpProdPartSummary"))
                {
                    cmd.Connection = connection;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@BuyerRefId", SqlDbType.VarChar).Value = buyerRefId;
                    cmd.Parameters.Add("@OrderNo", SqlDbType.VarChar).Value = orderNo;
                    cmd.Parameters.Add("@OrderStyleRefId", SqlDbType.VarChar).Value = orderStyleRefId;
                    cmd.Parameters.Add("@ComponentRefId", SqlDbType.VarChar).Value = componentRefId;
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

        public List<VwProdBundleSlip> GetBundleSlip(string cuttingBatchRefId, string compId)
        {

            string sqlQuery = String.Format("exec [dbo].[SPProdBundleSlip] @CuttingBatchRefId='{0}',@CompId='{1}'", cuttingBatchRefId, compId);
            return _context.Database.SqlQuery<VwProdBundleSlip>(sqlQuery).ToList();
        }

        public DataTable GetSpBundleChar(string cuttingBatchRefId, string compId)
        {
            SqlConnection connection = (SqlConnection)_context.Database.Connection;

            using (SqlCommand cmd = new SqlCommand("SpProdBundelChart"))
            {
                cmd.Connection = connection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CuttingBatchRefId", SqlDbType.VarChar).Value = cuttingBatchRefId;
                cmd.Parameters.Add("@CompId", SqlDbType.VarChar).Value = compId;
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    return dt;
                }
            }

        }

        public DataTable GetSpBundle(string cuttingBatchRefId, string compId)
        {
            SqlConnection connection = (SqlConnection)_context.Database.Connection;

            using (SqlCommand cmd = new SqlCommand("SpProdBundel"))
            {
                cmd.Connection = connection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CuttingBatchRefId", SqlDbType.VarChar).Value = cuttingBatchRefId;
                cmd.Parameters.Add("@CompId", SqlDbType.VarChar).Value = compId;
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    return dt;
                }
            }
        }

        public DataTable GetProcessDelivery(long processDeliveryId, string compId)
        {
            SqlConnection connection = (SqlConnection)_context.Database.Connection;
            using (SqlCommand cmd = new SqlCommand("SpProdProcessDelivery"))
            {
                cmd.Connection = connection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ProcessDeliveryId", SqlDbType.VarChar).Value = processDeliveryId;
                cmd.Parameters.Add("@CompId", SqlDbType.VarChar).Value = compId;
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    return dt;
                }
            }
        }

        public DataTable GetProcessReceiveDetail(string processReceiveRefId, string compId)
        {
            SqlConnection connection = (SqlConnection)_context.Database.Connection;

            using (SqlCommand cmd = new SqlCommand("SPProdProcessReceiveDetail"))
            {
                cmd.Connection = connection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ProcessReceiveRefId", SqlDbType.VarChar).Value = processReceiveRefId;
                cmd.Parameters.Add("@CompId", SqlDbType.VarChar).Value = compId;
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    return dt;
                }
            }
        }

        public DataTable GetfactoryDataTable(string orderStyleRefId, string colorRefId, long cuttingTagId, string compId,
            string processRefId)
        {

            SqlConnection connection = (SqlConnection)_context.Database.Connection;

            using (SqlCommand cmd = new SqlCommand("SpProdTagSupplyer"))
            {
                cmd.Connection = connection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@OrderStyleRefId", SqlDbType.VarChar).Value = orderStyleRefId;
                cmd.Parameters.Add("@ColorRefId", SqlDbType.VarChar).Value = colorRefId;
                cmd.Parameters.Add("@CuttingTagId", SqlDbType.BigInt).Value = cuttingTagId;
                cmd.Parameters.Add("@CompId", SqlDbType.VarChar).Value = compId;
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    return dt;
                }
            }
        }

        public DataTable GetProcessReceiveDataTable(string orderStyleRefId, string colorRefId, long cuttingTagId, string compId,
            string processRefId)
        {
            SqlConnection connection = (SqlConnection)_context.Database.Connection;

            using (SqlCommand cmd = new SqlCommand("SpProdProcessReceive"))
            {
                cmd.Connection = connection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@OrderStyleRefId", SqlDbType.VarChar).Value = orderStyleRefId;
                cmd.Parameters.Add("@ColorRefId", SqlDbType.VarChar).Value = colorRefId;
                cmd.Parameters.Add("@CuttingTagId", SqlDbType.BigInt).Value = cuttingTagId;
                cmd.Parameters.Add("@ProcessRefId", SqlDbType.VarChar).Value = processRefId;
                cmd.Parameters.Add("@CompId", SqlDbType.VarChar).Value = compId;
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    return dt;
                }
            }
        }

        public DataTable GetProcessDeliveryDatable(string orderStyleRefId, string colorRefId, long cuttingTagId, string compId,
            string processRefId)
        {

            SqlConnection connection = (SqlConnection)_context.Database.Connection;

            using (SqlCommand cmd = new SqlCommand("SpProdProcessDeliveryReport"))
            {
                cmd.Connection = connection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@OrderStyleRefId", SqlDbType.VarChar).Value = orderStyleRefId;
                cmd.Parameters.Add("@ColorRefId", SqlDbType.VarChar).Value = colorRefId;
                cmd.Parameters.Add("@CuttingTagId", SqlDbType.BigInt).Value = cuttingTagId;
                cmd.Parameters.Add("@ProcessRefId", SqlDbType.VarChar).Value = processRefId;
                cmd.Parameters.Add("@CompId", SqlDbType.VarChar).Value = compId;

                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    return dt;
                }
            }
        }
        public DataTable GetFactoryStyleWiseBalanceReport(string compId, long partyId, string orederStyleRefId, string processRefId)
        {

            SqlConnection connection = (SqlConnection)_context.Database.Connection;
            using (SqlCommand cmd = new SqlCommand("SpProdReceiveBalanceReport"))
            {
                cmd.Connection = connection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@OrderStyleRefId", SqlDbType.VarChar).Value = orederStyleRefId;
                cmd.Parameters.Add("@ProcessRefId", SqlDbType.VarChar).Value = processRefId;
                cmd.Parameters.Add("@CompId", SqlDbType.VarChar).Value = compId;

                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    return dt;
                }
            }
        }


        public DataTable GetMinimumSendReceive(string compId, long partyId, string orderStyleRefId, string processRefId)
        {
            string sqlQuery = String.Format(@"select  C.ColorName,SUM(V.OrderQty) as OrderQty, SUM(V.Quantity) as Quantity ,SUM(V.RejectQty) as RejectQty,SUM(V.ReceiveQty)as ReceiveQty  
                                 from VwProdSizeWizeMinimumSendRecive  as V
                                 inner join OM_Color as C on V.ColorRefId=C.ColorRefId and V.CompId=C.CompId
                                 where V.OrderStyleRefId='{0}'  and V.CompId='{1}' and V.ProcessRefId='{2}' and V.PartyId='{3}'
                                 group by C.ColorName",orderStyleRefId,compId,processRefId,partyId);
            SqlConnection connection = (SqlConnection)_context.Database.Connection;
           
            using (SqlCommand cmd = new SqlCommand(sqlQuery))
            {
                cmd.Connection = connection;
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    return dt;
                }
            }

        }

        public DataTable GetPrintEmbroideryBalanceSummaryt(string compId, long partyId, string orderStyleRefId)
        {
            SqlConnection connection = (SqlConnection)_context.Database.Connection;
            using (SqlCommand cmd = new SqlCommand("SpProdPrintEmbroieryBalanceSummary"))
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

        public DataTable GetProcessDeliveryDetailReportData(string compId, string orderStyleRefId, string processRefId)
        {
            SqlConnection connection = (SqlConnection)_context.Database.Connection;
            using (SqlCommand cmd = new SqlCommand("SpProdProcessDeliveryDetailReport"))
            {
                cmd.Connection = connection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompId", SqlDbType.VarChar).Value = compId;
                cmd.Parameters.Add("@OrderStyleRefId", SqlDbType.VarChar).Value = orderStyleRefId;
                cmd.Parameters.Add("@ProcessRefId", SqlDbType.VarChar).Value = processRefId;
             
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    return dt;
                }
            }
        }

        public DataTable GetReceiveDetailReportData(string compId, string orderStyleRefId, string processRefId)
        {
            SqlConnection connection = (SqlConnection)_context.Database.Connection;
            using (SqlCommand cmd = new SqlCommand("SpProdProcessReceiveDetailReport"))
            {
                cmd.Connection = connection;
                cmd.CommandType = CommandType.StoredProcedure;
           
                cmd.Parameters.Add("@ProcessRefId", SqlDbType.VarChar).Value = processRefId;
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

        public DataTable GetCuttBankData(string compId, string orderStyleRefId)
        {
            SqlConnection connection = (SqlConnection)_context.Database.Connection;
            using (SqlCommand cmd = new SqlCommand("SpProdCutBankReport"))
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

        public DataTable GetSweingInputReport(string compId, long sewingInputProcessId)
        {
            SqlConnection connection = (SqlConnection)_context.Database.Connection;
            using (SqlCommand cmd = new SqlCommand("SpProdSewingInpurtReport"))
            {
                cmd.Connection = connection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompId", SqlDbType.VarChar).Value = compId;
                cmd.Parameters.Add("@SewingInputProcessId", SqlDbType.VarChar).Value = sewingInputProcessId;
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    return dt;
                }
            }
        }

        public DataTable GetHourlyProductionDataTable(string compId, DateTime outputDate)
        {
            SqlConnection connection = (SqlConnection)_context.Database.Connection;
            using (SqlCommand cmd = new SqlCommand("SpProdHourlyProductionReprot"))
            {
                cmd.Connection = connection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompId", SqlDbType.VarChar).Value = compId;
                cmd.Parameters.Add("@OutputDate", SqlDbType.DateTime).Value = outputDate;
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    return dt;
                }
            }
        }

        public DataTable GetSizeLineWiseSewingOutputSummary(string compId, string orderStyleRefId, string colorRefId)
        {
            SqlConnection connection = (SqlConnection)_context.Database.Connection;

            using (SqlCommand cmd = new SqlCommand("SpSizeAndLineWizeOutputSumamaryReport"))
            {
                cmd.Connection = connection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@OrderStyleRefId", SqlDbType.VarChar).Value = orderStyleRefId;
                cmd.Parameters.Add("@ColorRefId", SqlDbType.VarChar).Value = colorRefId;
                cmd.Parameters.Add("@CompId", SqlDbType.VarChar).Value = compId;

                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    return dt;
                }
            }
        }

        public DataTable GetSizeLineWiseSewingOutputDetail(string compId, string orderStyleRefId, string colorRefId)
        {

            SqlConnection connection = (SqlConnection)_context.Database.Connection;

            using (SqlCommand cmd = new SqlCommand("SpSizeAndLineWizeOutputDetailReport"))
            {
                cmd.Connection = connection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@OrderStyleRefId", SqlDbType.VarChar).Value = orderStyleRefId;
                cmd.Parameters.Add("@ColorRefId", SqlDbType.VarChar).Value = colorRefId;
                cmd.Parameters.Add("@CompId", SqlDbType.VarChar).Value = compId;

                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    return dt;
                }
            }
        }

      

        public DataTable GetSizeLineWiseSewingInputDetail(string compId, string orderStyleRefId, string colorRefId)
        {

            SqlConnection connection = (SqlConnection)_context.Database.Connection;

            using (SqlCommand cmd = new SqlCommand("SpProdLineAndSizeWizeSewingInputDetailReport"))
            {
                cmd.Connection = connection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@OrderStyleRefId", SqlDbType.VarChar).Value = orderStyleRefId;
                cmd.Parameters.Add("@ColorRefId", SqlDbType.VarChar).Value = colorRefId;
                cmd.Parameters.Add("@CompId", SqlDbType.VarChar).Value = compId;

                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    return dt;
                }
            }
        }

        public DataTable GetLineStatus(string compId, DateTime outputDate)
        {
            SqlConnection connection = (SqlConnection)_context.Database.Connection;
            using (SqlCommand cmd = new SqlCommand("SpProdLineStatusReport"))
            {
                cmd.Connection = connection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompId", SqlDbType.VarChar).Value = compId;
                cmd.Parameters.Add("@OutputDate", SqlDbType.DateTime).Value = outputDate;
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    return dt;
                }
            }
        }

        public DataTable GetManMachineUtiliztiont(DateTime outputDate)
        {
            SqlConnection connection = (SqlConnection)_context.Database.Connection;
            using (SqlCommand cmd = new SqlCommand("SpHrmManMachineUtilization"))
            {
                cmd.Connection = connection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@EffectiveDate", SqlDbType.DateTime).Value = outputDate;
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    return dt;
                }
            }
        }

        public DataTable GetDailyProductionStatus(string compId, DateTime findDate)
        {
            SqlConnection connection = (SqlConnection)_context.Database.Connection;
            using (SqlCommand cmd = new SqlCommand("SpProdDailyProductionStatus"))
            {
                cmd.Connection = connection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompId", SqlDbType.VarChar).Value = compId;
                cmd.Parameters.Add("@FilterDate", SqlDbType.DateTime).Value = findDate;
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    return dt;
                }
            }
        }

        public DataTable GetDailyProductionStatusSummary(string compId, DateTime getValueOrDefault)
        {
            SqlConnection connection = (SqlConnection)_context.Database.Connection;
            using (SqlCommand cmd = new SqlCommand("SpDailyProductionStatusSumary"))
            {
                cmd.Connection = connection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompId", SqlDbType.VarChar).Value = compId;
                cmd.Parameters.Add("@FilterDate", SqlDbType.DateTime).Value = getValueOrDefault;
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    return dt;
                }
            }
        }

        public DataTable GetMonthlyProductionStatus(string compId, int yearId, int monthId)
        {
            SqlConnection connection = (SqlConnection)_context.Database.Connection;
            using (SqlCommand cmd = new SqlCommand("SpProdMonthlyProductionStatus"))
            {
                cmd.Connection = connection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompId", SqlDbType.VarChar).Value = compId;
                cmd.Parameters.Add("@YarId", SqlDbType.Int).Value = yearId;
                cmd.Parameters.Add("@Month", SqlDbType.Int).Value = monthId;
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    return dt;
                }
            }
        }

        public DataTable GetMonthlySewingProductionStatus(string compId, int yearId, int monthId)
        {
            SqlConnection connection = (SqlConnection)_context.Database.Connection;
            using (SqlCommand cmd = new SqlCommand("SpProdMonthlySewingProductionStatus"))
            {
                cmd.Connection = connection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompId", SqlDbType.VarChar).Value = compId;
                cmd.Parameters.Add("@YearId", SqlDbType.Int).Value = yearId;
                cmd.Parameters.Add("@MonthId", SqlDbType.Int).Value = monthId;
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    return dt;
                }
            }
        }

        public DataTable GetDailyPrintEmbStatus(string compId, DateTime? filterDate)
        {
            SqlConnection connection = (SqlConnection)_context.Database.Connection;
            using (SqlCommand cmd = new SqlCommand("SpProdDailyPrintEmbSendRcvStatus"))
            {
                cmd.Connection = connection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompId", SqlDbType.VarChar).Value = compId;
                cmd.Parameters.Add("@FilterDate", SqlDbType.DateTime).Value = filterDate;
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    return dt;
                }
            }
        }

        public DataTable GetMonthlyDayWiseSewingProductionStatus(string compId, int yearId, int monthId)
        {
            SqlConnection connection = (SqlConnection)_context.Database.Connection;
            using (SqlCommand cmd = new SqlCommand("SpProdMonthlySewingProduction"))
            {
                cmd.Connection = connection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompId", SqlDbType.VarChar).Value = compId;
                cmd.Parameters.Add("@YearId", SqlDbType.Int).Value = yearId;
                cmd.Parameters.Add("@MonthId", SqlDbType.Int).Value = monthId;
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    return dt;
                }
            }
        }

        public DataTable GetMonthlyCuttingStatus(DateTime fromDate, DateTime toDate, string compId)
        {
            SqlConnection connection = (SqlConnection)_context.Database.Connection;
            using (SqlCommand cmd = new SqlCommand("SpProdMonthlyCuttingStatusReport"))
            {
                cmd.Connection = connection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompId", SqlDbType.VarChar).Value = compId;
                cmd.Parameters.Add("@FromDate", SqlDbType.Date).Value = fromDate;
                cmd.Parameters.Add("@ToDate", SqlDbType.Date).Value = toDate;
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    return dt;
                }
            }
        }

        public DataTable GetBatchDetail(long batchId, string compId)
        {
            SqlConnection connection = (SqlConnection)_context.Database.Connection;
            using (SqlCommand cmd = new SqlCommand("SpProdBatchReport"))
            {
                cmd.Connection = connection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@BatchId", SqlDbType.BigInt).Value = batchId;
                cmd.Parameters.Add("@CompId", SqlDbType.VarChar).Value = compId;
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    return dt;
                }
            }
        }

        public DataTable GetKnittingProductionDetailStatus(string orderStyleRefId, string compId)
        {
            SqlConnection connection = (SqlConnection)_context.Database.Connection;
            using (SqlCommand cmd = new SqlCommand("SpKnittingProduction"))
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

        public DataTable GetDyeingSpChallan(long dyeingSpChallanId)
        {
            SqlConnection connection = (SqlConnection)_context.Database.Connection;
            using (SqlCommand cmd = new SqlCommand("SpDyeingSpChallan"))
            {
                cmd.Connection = connection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@DyeingSpChallanId", SqlDbType.BigInt).Value = dyeingSpChallanId;
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    return dt;
                }
            }
        }

        public DataTable GetKnittingRollDeliveryChallan(int knittingRollIssueId)
        {
            SqlConnection connection = (SqlConnection)_context.Database.Connection;
            using (SqlCommand cmd = new SqlCommand("spProdKnittingRollDeliveryChallan"))
            {
                cmd.Connection = connection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@KnittingRollIssueId", SqlDbType.BigInt).Value = knittingRollIssueId;
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    return dt;
                }
            }
        }

        public DataTable GetMontyPlanningVsProduction(int yearId, int monthId)
        {
            SqlConnection connection = (SqlConnection)_context.Database.Connection;
            using (SqlCommand cmd = new SqlCommand("SpMonthlyPlanningVsProductionReport"))
            {
                cmd.Connection = connection;
                cmd.CommandType = CommandType.StoredProcedure;
               
                cmd.Parameters.Add("@yearId", SqlDbType.Int).Value = yearId;
                cmd.Parameters.Add("@monthId", SqlDbType.Int).Value = monthId;
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    return dt;
                }
            }
        }

        public DataTable GetMontyLossTimeSummaryReport(int yearId, int monthId)
        {
            SqlConnection connection = (SqlConnection)_context.Database.Connection;
            using (SqlCommand cmd = new SqlCommand("SpProdMonthlyLossTimeSummary"))
            {
                cmd.Connection = connection;
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@YearId", SqlDbType.Int).Value = yearId;
                cmd.Parameters.Add("@MonthId", SqlDbType.Int).Value = monthId;
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    return dt;
                }
            }
        }

        public DataTable GetUnitWiseHourlyProduction(string compId, DateTime outputDate)
        {
            SqlConnection connection = (SqlConnection)_context.Database.Connection;
            using (SqlCommand cmd = new SqlCommand("spUnitWiseHourlySewingProduction"))
            {
                cmd.Connection = connection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompId", SqlDbType.VarChar).Value = compId;
                cmd.Parameters.Add("@FilterDate", SqlDbType.DateTime).Value = outputDate;
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    return dt;
                }
            }
        }

        public DataTable GetSewingUnitProductionForecasting(string compId, DateTime outputDate)
        {
            SqlConnection connection = (SqlConnection)_context.Database.Connection;
            using (SqlCommand cmd = new SqlCommand("spSewingUnitProductionForecasting"))
            {
                cmd.Connection = connection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompId", SqlDbType.VarChar).Value = compId;
                cmd.Parameters.Add("@FilterDate", SqlDbType.DateTime).Value = outputDate;
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    return dt;
                }
            }
        }

        public DataTable GetTargetVProduction(int modelYearId, int modelMonthId)
        {
            SqlConnection connection = (SqlConnection)_context.Database.Connection;
            using (SqlCommand cmd = new SqlCommand("SpProdSpLine"))
            {
                cmd.Connection = connection;
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@YearId", SqlDbType.Int).Value = modelYearId;
                cmd.Parameters.Add("@MonthId", SqlDbType.Int).Value = modelMonthId;
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    return dt;
                }
            }
        }

        public DataTable GetDalilyLineWiseTargetVsProduction(string compId, DateTime outputDate)
        {
            SqlConnection connection = (SqlConnection)_context.Database.Connection;
            using (SqlCommand cmd = new SqlCommand("SpProdDalilyLineWiseTargetVsProduction"))
            {
                cmd.Connection = connection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompId", SqlDbType.VarChar).Value = compId;
                cmd.Parameters.Add("@FilterDate", SqlDbType.DateTime).Value = outputDate;
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    return dt;
                }
            }
        }

        public DataTable GetKnittingRollDeliveryChallanSummary(int knittingRollIssueId)
        {
            SqlConnection connection = (SqlConnection)_context.Database.Connection;
            using (SqlCommand cmd = new SqlCommand("spRollDeliveryChallanSummary"))
            {
                cmd.Connection = connection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@KnittingRollIssueId", SqlDbType.BigInt).Value = knittingRollIssueId;
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    return dt;
                }
            }
        }

        public DataTable GetInKnitProgramDataTable(int knittingRollIssueId)
        {
            SqlConnection connection = (SqlConnection)_context.Database.Connection;
            string sqlQutery =@"select * from VProgramDetail where  MType='I' and PrgramRefId=(select ProgramRefId from PROD_KnittingRollIssue where KnittingRollIssueId=@KnittingRollIssueId)";
            using (SqlCommand cmd = new SqlCommand(sqlQutery))
            {
                cmd.Connection = connection;
                cmd.Parameters.Add("@KnittingRollIssueId", SqlDbType.BigInt).Value = knittingRollIssueId;
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    return dt;
                }
            }
        }

        public DataTable GetMontyStyleWiseProduction(int modelYearId, int modelMonthId)
        {

            SqlConnection connection = (SqlConnection)_context.Database.Connection;
            using (SqlCommand cmd = new SqlCommand("spMonthlyWiseProductions"))
            {
                cmd.Connection = connection;
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@YearId", SqlDbType.Int).Value = modelYearId;
                cmd.Parameters.Add("@MonthId", SqlDbType.Int).Value = modelMonthId;
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    return dt;
                }
            }
        }

        public DataTable DailyProductionStatus(DateTime addDays)
        {
            SqlConnection connection = (SqlConnection)_context.Database.Connection;
            using (SqlCommand cmd = new SqlCommand("spDailyProductionSummary"))
            {
                cmd.Connection = connection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ReportDate", SqlDbType.DateTime).Value = addDays;
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    return dt;
                }
            }
        }

        public DataTable GetDyeingProfitabilyAnalysis(int yearId)
        {
            SqlConnection connection = (SqlConnection)_context.Database.Connection;
            using (SqlCommand cmd = new SqlCommand("spMisDyeingProfitabilyAnalysis"))
            {
                cmd.Connection = connection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@YearId", SqlDbType.Int).Value = yearId;
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    return dt;
                }
            }
        }

     

        public DataTable GetDailyProductionCapacity(DateTime filterDate)
        {
            SqlConnection connection = (SqlConnection)_context.Database.Connection;
            using (SqlCommand cmd = new SqlCommand("spDailyPructivityAnalysis"))
            {
                cmd.Connection = connection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@filterDate", SqlDbType.DateTime).Value = filterDate;
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    return dt;
                }
            }
        }

        public DataTable GetPrintEmprocessStatus(string orderNo, string orderStyleRefId, string compId)
        {
            SqlConnection connection = (SqlConnection)_context.Database.Connection;
            using (SqlCommand cmd = new SqlCommand("spProdStyleWisePrintEmbStatus"))
            {
                cmd.Connection = connection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@orderNo", SqlDbType.VarChar).Value = orderNo;
                cmd.Parameters.Add("@CompId", SqlDbType.VarChar).Value = compId;
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    return dt;
                }
            }
        }
        public DataTable GetKnittingRollDeliveryPartyChallan(int knittingRollIssueId)
        {
            SqlConnection connection = (SqlConnection)_context.Database.Connection;
            using (SqlCommand cmd = new SqlCommand("spKnittingRollDeliveryParty"))
            {
                cmd.Connection = connection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@KnittingRollIssueId", SqlDbType.BigInt).Value = knittingRollIssueId;
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    return dt;
                }
            }
        }

        public DataTable GetMonthlyOtherCuttingProduction(int yearId, int monthId)
        {
            SqlConnection connection = (SqlConnection)_context.Database.Connection;
            using (SqlCommand cmd = new SqlCommand("spGetMonthlyOtherCuttingProduction"))
            {
                cmd.Connection = connection;
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@YearId", SqlDbType.Int).Value = yearId;
                cmd.Parameters.Add("@MonthId", SqlDbType.Int).Value = monthId;
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    return dt;
                }
            }
        }

        public DataTable GetOrderClosingStatus(string buyerRefId, string orderNo, string orderStyleRefId)
        {
          
            SqlConnection connection = (SqlConnection)_context.Database.Connection;
            using (SqlCommand cmd = new SqlCommand("spOrderClosingStatus"))
            {
                cmd.Connection = connection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@buyerRefId", SqlDbType.VarChar).Value = buyerRefId;
                cmd.Parameters.Add("@orderNo", SqlDbType.VarChar).Value = orderNo;
                cmd.Parameters.Add("@orderStyleRefId", SqlDbType.VarChar).Value = orderStyleRefId;
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
