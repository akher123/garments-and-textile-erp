using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;

using SCERP.Common;
using SCERP.DAL.IRepository.IProductionRepository;
using SCERP.Model.Production;

namespace SCERP.DAL.Repository.ProductionRepository
{
    public class SewingOutPutProcessRepository : Repository<PROD_SewingOutPutProcess>, ISewingOutPutProcessRepository
    {
        private readonly SCERPDBContext _context;
        public SewingOutPutProcessRepository(SCERPDBContext context)
            : base(context)
        {
            _context = context;
        }
        public List<VwSewingOutputProcess> GetSewingOutputProcessByStyleColor(string compId, string orderStyleRefId, string colorRefId, string orderShipRefId)
        {
            return Context.VwSewingOutputProcesses.Where(x => x.CompId == compId && x.OrderStyleRefId == orderStyleRefId && x.ColorRefId == colorRefId&&x.OrderShipRefId==orderShipRefId).ToList();
        }

        public List<VwSewingOutput> GetAllSewingOutputInfo(long sewingOutPutProcessId, string compId)
        {
            string sqlQuery = string.Format(@"EXEC SpProdSewingOutputProcessEdit {0},'{1}'", sewingOutPutProcessId, compId);
            return Context.Database.SqlQuery<VwSewingOutput>(sqlQuery).ToList();
        }

        public IQueryable<VwSewingOutputProcess> GetDailySewingOut(string compId, DateTime outputDate, int lineId)
        {
            return Context.VwSewingOutputProcesses.Where(x => x.OutputDate == outputDate && x.CompId == compId && (x.LineId == lineId || lineId == 0));
        }

        public IQueryable<VwSewingOutputProcess> GetDailySewingOutForReport(string compId, DateTime outputDate, int lineId)
        {
            return Context.VwSewingOutputProcesses.Where(x => x.OutputDate == outputDate && x.CompId == compId && (x.LineId == lineId || lineId == 0));
        }

        public DataTable GetSewingWIP(DateTime outputDate, int hourId, string compId)
        {
            SqlConnection connection = (SqlConnection)_context.Database.Connection;

            using (SqlCommand cmd = new SqlCommand("SpSewingWIP"))
            {
                cmd.Connection = connection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ViewDate", SqlDbType.DateTime).Value = outputDate;
                cmd.Parameters.Add("@HourId", SqlDbType.Int).Value = hourId;
                cmd.Parameters.Add("@CompId", SqlDbType.VarChar).Value = compId;
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    return dt;
                }
            }
        }

        public DataTable GetSewingWIPDetail(DateTime outputDate, string compId)
        {
            SqlConnection connection = (SqlConnection)_context.Database.Connection;

            using (SqlCommand cmd = new SqlCommand("SpSewingWIPDetail"))
            {
                cmd.Connection = connection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ViewDate", SqlDbType.DateTime).Value = outputDate;
                cmd.Parameters.Add("@CompId", SqlDbType.VarChar).Value = compId;
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    return dt;
                }
            }
        }

        public DataTable GetHourlyProduction(DateTime productionDate,string compId)
        {


            SqlConnection connection = (SqlConnection)_context.Database.Connection;
           
            _context.Database.ExecuteSqlCommand(@"truncate table PROD_HourlyProductionReport");
            string sqlLineQuery = @"insert into PROD_HourlyProductionReport (LineId,DataType,CompId) select Production_Machine.MachineId,{0},'{1}' from Production_Machine
                                  inner join PROD_Processor on Production_Machine.ProcessorRefId=PROD_Processor.ProcessorRefId and Production_Machine.CompId=PROD_Processor.CompId
                                  where PROD_Processor.ProcessRefId='007' and Production_Machine.CompId='{1}' and  Production_Machine.IsActive=1";
            _context.Database.ExecuteSqlCommand(String.Format(sqlLineQuery,1,compId));
           
            string sqlqDetail = @"update PROD_HourlyProductionReport set {0}=ISNULL((select SUM(PROD_SewingOutPutProcessDetail.Quantity) from PROD_SewingOutPutProcess
                 inner join PROD_SewingOutPutProcessDetail on PROD_SewingOutPutProcess.SewingOutPutProcessId=PROD_SewingOutPutProcessDetail.SewingOutPutProcessId 
                 inner join PROD_Hour on PROD_SewingOutPutProcess.HourId=PROD_Hour.HourId and PROD_SewingOutPutProcess.CompId=PROD_Hour.CompId
                 where  PROD_Hour.CompId='"+compId+"' AND PROD_Hour.HourRefId='{1}' and Convert(date,PROD_SewingOutPutProcess.OutputDate)=Convert(date,'" + productionDate + "') and PROD_SewingOutPutProcess.LineId=PROD_HourlyProductionReport.LineId and PROD_HourlyProductionReport.DataType=1 and PROD_HourlyProductionReport.CompId=PROD_Hour.CompId),0)";
            string columName = "";
            string oprerator = "";
            for (int i = 1; i <= 14; i++)
            {
                string sufix = i.ToString().PadZero(2);
                string colum = "H" + sufix;
                oprerator = i<14 ? "+" : "";
                columName += colum + oprerator;
                string sqlQueryFinal = String.Format(sqlqDetail, colum, sufix);
                _context.Database.ExecuteSqlCommand(sqlQueryFinal);
            }
            _context.Database.ExecuteSqlCommand(string.Format(sqlLineQuery, 2,compId));
            const string totalQty = @"update PROD_HourlyProductionReport set TotalQty= (select sum({0}) from PROD_HourlyProductionReport as HP where PROD_HourlyProductionReport.LineId=HP.LineId and PROD_HourlyProductionReport.DataType=1)";
            _context.Database.ExecuteSqlCommand(String.Format(totalQty,columName));
            _context.Database.ExecuteSqlCommand(@"update PROD_HourlyProductionReport set TotalQty=(select top 1 TotalQty from PROD_HourlyProductionReport as HP  where HP.LineId=PROD_HourlyProductionReport.LineId and HP.DataType=1) where DataType=2");
            _context.Database.ExecuteSqlCommand(
                @"insert  into PROD_HourlyProductionReport (LineId,DataType,CompId) values(99,3,'"+compId+"')");
            string totalQtuUpdate = @"update PROD_HourlyProductionReport set {0}=(select SUM({0}) from PROD_HourlyProductionReport  where DataType=1) where DataType=3";
            _context.Database.ExecuteSqlCommand(string.Format(totalQtuUpdate, "TotalQty"));
            string dcolum = " ";
            for (int i = 1; i <= 14; i++)
            {
                string sufix = i.ToString().PadZero(2);
                string colum = "H" + sufix;
                if (i == 1)
                {
                   dcolum =dcolum + " H" + sufix; 
                }
                else
                {
                  dcolum =dcolum + " + H" + sufix;  
                }
                string sqlt1 = " update PROD_HourlyProductionReport SET  " + colum + " = isnull(( SELECT " + dcolum;
                string sqlCoumUpdate = @" " + sqlt1 + @" FROM PROD_HourlyProductionReport as aa WHERE LineId = PROD_HourlyProductionReport.LineId AND DataType = 1),0) where DataType = 2 ";
              
                _context.Database.ExecuteSqlCommand(String.Format(sqlCoumUpdate, colum));

                _context.Database.ExecuteSqlCommand(string.Format(totalQtuUpdate, colum));

            }

            using (SqlCommand cmd = new SqlCommand("SpHourlyProduction"))
            {
                cmd.Connection = connection;
            
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ViewDate", SqlDbType.DateTime).Value = productionDate;
              
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    return dt;
                }
            }
        }

        public VwProductionForecast ProductionForecast(DateTime currentDate, string compId)
        {
             string sqlQuery = String.Format("exec [dbo].[SpProductionForecast] @OutputDate='{0}'", currentDate);
             return _context.Database.SqlQuery<VwProductionForecast>(sqlQuery).FirstOrDefault();
           
        }

        public DataTable GetSewingWIPSummary(DateTime outputDate, string compId)
        {
            SqlConnection connection = (SqlConnection)_context.Database.Connection;

            using (SqlCommand cmd = new SqlCommand("SpSewingWIPSummary"))
            {
                cmd.Connection = connection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ViewDate", SqlDbType.DateTime).Value = outputDate;
                cmd.Parameters.Add("@CompId", SqlDbType.VarChar).Value = compId;
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    return dt;
                }
            }
        }

        public VwProductionForecast ProductionForecastLastMonth(DateTime addMonths, string compId)
        {
            string sqlQuery = String.Format("exec [dbo].[SpProductionForecastLastMonth] @OutputDate='{0}'", addMonths);
            return _context.Database.SqlQuery<VwProductionForecast>(sqlQuery).FirstOrDefault();
        }

        public string GetLastSwingOutputDateTime(string compId)
        {
            
            string sqlQuery = String.Format("exec [dbo].[spGetLastSewingOutputDateTime] @CompId='{0}'", compId);
            return _context.Database.SqlQuery<String>(sqlQuery).FirstOrDefault();
        }

        public int GetTotalProductionHours(DateTime outputDate, string compId)
        {
            string sqlQuery = String.Format("select SUM(ManPower) from PROD_SewingOutPutProcess where CAST(OutputDate as Date)=CAST('{0}' AS Date) and CompId='{1}'",outputDate, compId);
            return _context.Database.SqlQuery<int>(sqlQuery).FirstOrDefault();
        }
    }
}
