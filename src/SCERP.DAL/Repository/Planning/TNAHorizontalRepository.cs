using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.SqlClient;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.DAL.IRepository.IPlanningRepository;
using SCERP.Model;
using SCERP.Model.Planning;
using SCERP.Common;
using System.Data;

namespace SCERP.DAL.Repository.Planning
{
    public class TNAHorizontalRepository : Repository<PLAN_TNA>, ITNAHorizontalRepository
    {
        private readonly SCERPDBContext _context;
        private SqlConnection _connection;
        private readonly string companyId;

        public TNAHorizontalRepository(SCERPDBContext context)
            : base(context)
        {
            this._context = context;
            _connection = (SqlConnection) _context.Database.Connection;
            this.companyId = PortalContext.CurrentUser.CompId;
        }

        public int SaveTnaHorizontal(PLAN_TNA Tna)
        {
            var editedTna = 0;
            var table = new DataTable();

            PLAN_Activity activity = Context.PLAN_Activity.Find(Tna.ActivityId);

            var start = "";
            var end = "";

            string plannedStartDate = Tna.PlannedStartDate == null ? "NULL" : "'" + Tna.PlannedStartDate + "'";
            string plannedEndDate = Tna.PlannedEndDate == null ? "NULL" : "'" + Tna.PlannedEndDate + "'";
            string actualStartDate = Tna.ActualStartDate == null ? "NULL" : "'" + Tna.ActualStartDate + "'";
            string actrualEndDate = Tna.ActrualEndDate == null ? "NULL" : "'" + Tna.ActrualEndDate + "'";

            string cmdText = @"UPDATE OM_BuyOrdStyle";
            var orderStyleRefId = Tna.OrderStyleRefId;

            if (activity.StartField.Trim().Length > 0 && activity.EndField.Trim().Length == 0)
            {
                start = activity.StartField;
                cmdText += " SET " + start + "=" + plannedStartDate;
                cmdText += ", Act" + start + "=" + actualStartDate;
            }

            else if (activity.StartField.Trim().Length == 0 && activity.EndField.Trim().Length > 0)
            {
                end = activity.EndField;
                cmdText += " SET " + end + "=" + plannedStartDate;
                cmdText += ", Act" + end + "=" + actualStartDate;
            }

            else if (activity.StartField.Trim().Length > 0 && activity.EndField.Trim().Length > 0)
            {
                start = activity.StartField;
                cmdText += " SET " + start + "=" + plannedStartDate;
                cmdText += ", Act" + start + "=" + actualStartDate;

                end = activity.EndField;
                cmdText += ", " + end + "=" + plannedEndDate;
                cmdText += ", Act" + end + "=" + actrualEndDate;
            }

            cmdText += " WHERE OM_BuyOrdStyle.OrderStyleRefId = '" + orderStyleRefId + "'";

            if (_connection != null && _connection.State == ConnectionState.Closed)
            {
                _connection.Open();
                var adapter = new SqlDataAdapter(cmdText, _connection);
                adapter.Fill(table);
                _connection.Close();
                editedTna = 1;
            }
            return editedTna;
        }

        public int DeleteTnaHorizontal(PLAN_TNA Tna)
        {
            var editedTna = 0;
            var table = new DataTable();

            PLAN_Activity activity = Context.PLAN_Activity.Find(Tna.ActivityId);

            var start = "";
            var end = "";

            string plannedStartDate = "NULL";
            string plannedEndDate = "NULL";
            string actualStartDate = "NULL";
            string actrualEndDate = "NULL";

            string cmdText = @"UPDATE OM_BuyOrdStyle";
            var orderStyleRefId = Tna.OrderStyleRefId;

            if (activity.StartField.Trim().Length > 0 && activity.EndField.Trim().Length == 0)
            {
                start = activity.StartField;
                cmdText += " SET " + start + "=" + plannedStartDate;
                cmdText += ", Act" + start + "=" + actualStartDate;
            }

            else if (activity.StartField.Trim().Length == 0 && activity.EndField.Trim().Length > 0)
            {
                end = activity.EndField;
                cmdText += " SET " + end + "=" + plannedStartDate;
                cmdText += ", Act" + end + "=" + actualStartDate;
            }

            else if (activity.StartField.Trim().Length > 0 && activity.EndField.Trim().Length > 0)
            {
                start = activity.StartField;
                cmdText += " SET " + start + "=" + plannedStartDate;
                cmdText += ", Act" + start + "=" + actualStartDate;

                end = activity.EndField;
                cmdText += ", " + end + "=" + plannedEndDate;
                cmdText += ", Act" + end + "=" + actrualEndDate;
            }

            cmdText += " WHERE OM_BuyOrdStyle.OrderStyleRefId = '" + orderStyleRefId + "'";

            if (_connection != null && _connection.State == ConnectionState.Closed)
            {
                _connection.Open();
                var adapter = new SqlDataAdapter(cmdText, _connection);
                adapter.Fill(table);
                editedTna = 1;
            }
            return editedTna;
        }

        public PLAN_TNA GetTnaByRefActivity(string buyerOrderRef, int activityId)
        {
            var tna = new PLAN_TNA();
            tna = _context.PLAN_TNA.FirstOrDefault(p => p.OrderStyleRefId == buyerOrderRef && p.ActivityId == activityId && p.IsActive);
            return tna;
        }

        public List<PLAN_TNAHorizontal> GetAllTnaHorizontalByPaging(int startPage, int pageSize, out int totalRecords, PLAN_TNAHorizontal tnaHorizonal)
        {
            var tnaHorizon = new List<PLAN_TNAHorizontal>();

            var seasonRefId = tnaHorizonal.SeasonRefId ?? "0".PadLeft(2, '0');
            var buyerRefId = tnaHorizonal.BuyerName.PadLeft(3, '0');
            var merchandiserRefId = tnaHorizonal.MerchandiserName.PadLeft(4, '0');
            var orderStyleRefId = tnaHorizonal.OrderStyleRefId ?? "0".PadLeft(7, '0');
            var companyId = tnaHorizonal.CompId;

            tnaHorizon = _context.Database.SqlQuery<PLAN_TNAHorizontal>("SPGETTNAHorizontal @SeasonRefId, @BuyerRefId, @MerchandiserRefId, @OrderStyleRefId, @CompanyId", new SqlParameter("SeasonRefId", seasonRefId), new SqlParameter("BuyerRefId", buyerRefId), new SqlParameter("MerchandiserRefId", merchandiserRefId), new SqlParameter("OrderStyleRefId", orderStyleRefId), new SqlParameter("CompanyId", companyId)).ToList();
            totalRecords = tnaHorizon.Count;

            switch (tnaHorizonal.sort)
            {
                case "Id":

                    switch (tnaHorizonal.sortdir)
                    {
                        case "DESC":
                            tnaHorizon = tnaHorizon
                                .OrderByDescending(r => r.BuyerName).ThenBy(x => x.BuyerName)
                                .Skip(startPage*pageSize)
                                .Take(pageSize).ToList();
                            break;
                        default:
                            tnaHorizon = tnaHorizon
                                .OrderBy(r => r.BuyerName).ThenBy(x => x.BuyerName)
                                .Skip(startPage*pageSize)
                                .Take(pageSize).ToList();
                            break;
                    }
                    break;

                default:
                    tnaHorizon = tnaHorizon
                        .OrderByDescending(r => r.OrderDate).ThenBy(x => x.BuyerName)
                        .Skip(startPage*pageSize)
                        .Take(pageSize).ToList();
                    break;
            }
            return tnaHorizon.ToList();
        }

        public List<PLAN_TNAHorizontal> GetAllTnaUpdateHorizontalByPaging(int startPage, int pageSize, out int totalRecords, PLAN_TNAHorizontal tnaHorizonal)
        {
            List<PLAN_TNAHorizontal> tnaHorizon;
            List<PLAN_TNAHorizontal> resultList = new List<PLAN_TNAHorizontal>();
            List<PLAN_TNAHorizontal> dynamicValue;

            var seasonRefId = tnaHorizonal.SeasonRefId ?? "0".PadLeft(2, '0');
            var buyerRefId = tnaHorizonal.BuyerName.PadLeft(3, '0');
            var merchandiserRefId = tnaHorizonal.MerchandiserName.PadLeft(4, '0');
            var orderStyleRefId = "0000000";
            var columnName = tnaHorizonal.PI;
            DateTime? fromDate = tnaHorizonal.FromDate;
            DateTime? toDate = tnaHorizonal.ToDate;

            tnaHorizon = _context.Database.SqlQuery<PLAN_TNAHorizontal>("SPGETTNAHorizontal @SeasonRefId, @BuyerRefId, @MerchandiserRefId, @OrderStyleRefId, @CompanyId", new SqlParameter("SeasonRefId", seasonRefId), new SqlParameter("BuyerRefId", buyerRefId), new SqlParameter("MerchandiserRefId", merchandiserRefId), new SqlParameter("OrderStyleRefId", orderStyleRefId), new SqlParameter("CompanyId", companyId)).ToList();
            dynamicValue = _context.Database.SqlQuery<PLAN_TNAHorizontal>("SPPlanDynamicTnaColumn @ColumnName, @CompanyId", new SqlParameter("ColumnName", columnName), new SqlParameter("CompanyId", companyId)).ToList();

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
            totalRecords = resultList.Count;

            switch (tnaHorizonal.sort)
            {
                case "Id":

                    switch (tnaHorizonal.sortdir)
                    {
                        case "DESC":
                            resultList = resultList
                                .OrderByDescending(r => r.BuyerName).ThenBy(x => x.BuyerName)
                                .Skip(startPage*pageSize)
                                .Take(pageSize).ToList();
                            break;
                        default:
                            resultList = resultList
                                .OrderBy(r => r.BuyerName).ThenBy(x => x.BuyerName)
                                .Skip(startPage*pageSize)
                                .Take(pageSize).ToList();
                            break;
                    }
                    break;

                default:
                    resultList = resultList
                        .OrderByDescending(r => r.OrderDate).ThenBy(x => x.BuyerName)
                        .Skip(startPage*pageSize)
                        .Take(pageSize).ToList();
                    break;
            }
            return resultList.OrderBy(p=>p.YBD).ToList();
        }
    }
}