using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.DAL.IRepository.IMerchandisingRepository;
using SCERP.Model;

namespace SCERP.DAL.Repository.MerchandisingRepository
{
    public class CostDefinationRepository : Repository<OM_CostDefination>, ICostDefinationRepository
    {
        public CostDefinationRepository(SCERPDBContext context) : base(context)
        {
        }

        public string GetNewCostRefId(string compId)
        {
            var sqlQuery = String.Format("SELECT Convert(varchar(4), max(Convert(int,CostRefId))+1 ) AS CostRefId FROM OM_CostDefination WHERE CompId='{0}'", compId);
            return Context.Database.SqlQuery<string>(sqlQuery).FirstOrDefault();
        }
    }
}
