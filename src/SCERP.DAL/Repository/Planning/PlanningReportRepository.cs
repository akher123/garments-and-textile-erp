using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Common;
using SCERP.DAL.IRepository.IPlanningRepository;
using SCERP.Model.Planning;

namespace SCERP.DAL.Repository.Planning
{
    public class PlanningReportRepository : IPlanningReportRepository
    {
        private readonly SCERPDBContext _context;
        private readonly string _companyId;

        public PlanningReportRepository(SCERPDBContext context)
        {
            this._context = context;
            this._companyId = PortalContext.CurrentUser.CompId;
        }

        public List<PLAN_TNAReport> GetTNAReportData(string buyerRefId, string orderNo, string orderStyleRefId,Guid userId)
        {
            object[] parms = {
                new SqlParameter("BuyerRefId", buyerRefId),
                new SqlParameter("OrderNo", orderNo??""),
                new SqlParameter("OrderStyleRefId", orderStyleRefId??""),
                new SqlParameter("CompanyId", _companyId),
                new SqlParameter("UserId", userId)
            };
            var tnaReports = _context.Database.SqlQuery<PLAN_TNAReport>("SPPlanTNAReport @BuyerRefId,@OrderNo,  @OrderStyleRefId, @CompanyId,@UserId", parms).ToList();
            return tnaReports;
        }

        public List<PLAN_TNAReport> GetTnaResponsePersonReportData(string person)
        {
            List<PLAN_TNAReport> tnaReports;
            tnaReports = _context.Database.SqlQuery<PLAN_TNAReport>("SPPlanTNAResponsible @PersonName, @CompanyId", new SqlParameter("PersonName", person), new SqlParameter("CompanyId", _companyId)).ToList();
            return tnaReports;
        }

        public List<PLAN_TNAHorizontal> GetTnaGroupUpdateReport(PLAN_TNAHorizontal tnaHorizonal)
        {
            List<PLAN_TNAHorizontal> tnaHorizon;
            List<PLAN_TNAHorizontal> resultList = new List<PLAN_TNAHorizontal>();
            List<PLAN_TNAHorizontal> dynamicValue;

            var seasonRefId = tnaHorizonal.SeasonRefId == "" ? "0".PadLeft(2, '0') : tnaHorizonal.SeasonRefId;
            var buyerRefId = tnaHorizonal.BuyerName.PadLeft(3, '0');
            var merchandiserRefId = tnaHorizonal.MerchandiserName.PadLeft(4, '0');
            var orderStyleRefId = "0000000";
            var columnName = tnaHorizonal.OrderStyleRefId;
            DateTime? fromDate = tnaHorizonal.FromDate;
            DateTime? toDate = tnaHorizonal.ToDate;

            tnaHorizon = _context.Database.SqlQuery<PLAN_TNAHorizontal>("SPGETTNAHorizontal @SeasonRefId, @BuyerRefId, @MerchandiserRefId, @OrderStyleRefId, @CompanyId", new SqlParameter("SeasonRefId", seasonRefId), new SqlParameter("BuyerRefId", buyerRefId), new SqlParameter("MerchandiserRefId", merchandiserRefId), new SqlParameter("OrderStyleRefId", orderStyleRefId), new SqlParameter("CompanyId", _companyId)).ToList();
            dynamicValue = _context.Database.SqlQuery<PLAN_TNAHorizontal>("SPPlanDynamicTnaColumn @ColumnName, @CompanyId", new SqlParameter("ColumnName", columnName), new SqlParameter("CompanyId", _companyId)).ToList();

            foreach (var p in tnaHorizon)
            {
                foreach (var c in dynamicValue)
                {
                    if (p.OrderStyleRefId == c.OrderStyleRefId && c.YBD >= fromDate && c.YBD <= toDate)
                    {
                        p.YBD = c.YBD;
                        resultList.Add(p);
                    }
                }
            }
            return resultList.OrderBy(p=>p.YBD).ToList();
        }

     
        public DataTable GetStyleWiseSmv(string compId)
        {
            using (SqlConnection connection = (SqlConnection)_context.Database.Connection)
            {
                using (SqlCommand cmd = new SqlCommand("SpPlanStyleWiseSmv"))
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
        }

        public DataTable GetMachineCapacity(string compId,string processRefId)
        {
            using (SqlConnection connection = (SqlConnection)_context.Database.Connection)
            {
                using (SqlCommand cmd = new SqlCommand("SpPlanMachineCapacity"))
                {
                    cmd.Connection = connection;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompId", SqlDbType.VarChar).Value = compId;
                    cmd.Parameters.Add("@ProcessRefId", SqlDbType.VarChar).Value = processRefId;
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        sda.Fill(dt);
                        return dt;
                    }
                }
            }
        }

        public DataTable GetStyleWiseSmvDetal(string compId, DateTime? fromDate, DateTime? toDate)
        {
            using (SqlConnection connection = (SqlConnection)_context.Database.Connection)
            {
                using (SqlCommand cmd = new SqlCommand("SpPlanStyleWiseSmvDetail"))
                {
                    cmd.Connection = connection;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompId", SqlDbType.VarChar).Value = compId;
                    cmd.Parameters.Add("@FromDate", SqlDbType.VarChar).Value = fromDate;
                    cmd.Parameters.Add("@ToDate", SqlDbType.VarChar).Value = toDate;
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        sda.Fill(dt);
                        return dt;
                    }
                }
            }
        }

        public DataTable GetSweingOrderPlanStatus(string compId)
        {

            using (SqlConnection connection = (SqlConnection)_context.Database.Connection)
            {
                using (SqlCommand cmd = new SqlCommand("spSweingOrderPlanStatus"))
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
        }
    }
}
