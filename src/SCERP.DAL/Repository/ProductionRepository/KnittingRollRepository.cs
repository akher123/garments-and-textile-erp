using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using SCERP.DAL.IRepository.IProductionRepository;
using SCERP.Model.Production;

namespace SCERP.DAL.Repository.ProductionRepository
{
    public class KnittingRollRepository : Repository<PROD_KnittingRoll>, IKnittingRollRepository
   {
        private readonly SCERPDBContext _context;
        public KnittingRollRepository(SCERPDBContext context) : base(context)
        {
            _context = context;
        }

        public VwKnittingRoll GetKnittingRollById(long knittingRollId)
        {
            return Context.VwKnittingRolls.FirstOrDefault(x => x.KnittingRollId == knittingRollId);
        }

        public IQueryable<VwKnittingRoll> GetKnittingRolls(Expression<Func<VwKnittingRoll, bool>> predicate)
        {
            return Context.VwKnittingRolls.Where(predicate);
        }

        public DataTable MachineWiseKnitting(DateTime? rolldate, string kType, string compId)
        {
            string sqlQuery = String.Format(@"exec SpDailyMachineWiseKnitting '{0}','{1}','{2}'",compId,kType,rolldate );
            return ExecuteQuery(sqlQuery);
        }
        public DataTable GetDailyKnittingRollSummaryByDate(DateTime dateTime, string compId)
        {
            SqlConnection connection = (SqlConnection)_context.Database.Connection;
            using (SqlCommand cmd = new SqlCommand("SpProdPartyWiseDailyKnittingRoll"))
            {
                cmd.Connection = connection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@RollDate", SqlDbType.Date).Value = dateTime;
                cmd.Parameters.Add("@CompId", SqlDbType.VarChar).Value = compId;
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    return dt;
                }
            }
        }

        public List<VwKnittingRoll> AutocompliteKnittingRoll(string orderStyleRefId,  string compId)
        {
            string sqlQuery =String.Format( @"select KR.* from VwKnittingRoll as KR
                inner join PLAN_Program as PG on KR.ProgramId=PG.ProgramId
                where PG.OrderStyleRefId='{0}' and KR.CompId='{1}' order by KR.CharllRollNo", orderStyleRefId, compId);
           return Context.Database.SqlQuery<VwKnittingRoll>(sqlQuery).ToList();
        }
   }
}
