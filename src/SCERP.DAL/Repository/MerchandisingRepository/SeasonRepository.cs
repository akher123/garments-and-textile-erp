using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.DAL.IRepository.IMerchandisingRepository;
using SCERP.Model;

namespace SCERP.DAL.Repository.MerchandisingRepository
{
    public class SeasonRepository :Repository<OM_Season>, ISeasonRepository
    {
        public SeasonRepository(SCERPDBContext context) : base(context)
        {
        }

        public string GetNewSeasonRefId(string compId)
        {
            var sqlQuery = String.Format(@"SELECT RIGHT('00'+ CAST( ISNULL(MAX(SeasonRefId),0)+1 as varchar(2) ),2) as SeasonRefId FROM OM_Season WHERE CompId='{0}'", compId);
            return Context.Database.SqlQuery<string>(sqlQuery).FirstOrDefault();
        }
    }
}
