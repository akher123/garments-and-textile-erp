using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.DAL.IRepository.IMerchandisingRepository;
using SCERP.Model.MerchandisingModel;

namespace SCERP.DAL.Repository.MerchandisingRepository
{
    public class ThreadConsumptionRepository : Repository<OM_ThreadConsumption>, IThreadConsumptionRepository
   {
        public ThreadConsumptionRepository(SCERPDBContext context) : base(context)
        {

        }

        public List<VwThreadConsumption> GetThreadConsumptionsByPaging(string compId,string searchString)
        {
            string cmd = String.Format(@"select * from VwThreadConsumption  where CompId='{0}' and (styleName+''+OrderName LIKE '%{1}%' OR '{1}'='null' )", compId, searchString);
            var consumptions = Context.Database.SqlQuery<VwThreadConsumption>(cmd);
            return consumptions.ToList();
        }

        public DataTable GetThreadConsumptionsReportDataTable(long threadConsumptionId)
        {
            string sqlCmd = String.Format(@"exec SpOmThreadConsumptionReport '{0}'", threadConsumptionId);
            return ExecuteQuery(sqlCmd);
        }
   }
}
