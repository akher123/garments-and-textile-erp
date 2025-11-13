using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.DAL.IRepository.IMerchandisingRepository;
using SCERP.Model;

namespace SCERP.DAL.Repository.MerchandisingRepository
{
    public class OmSizeRepository :Repository<OM_Size>, IOmSizeRepository
    {
        public OmSizeRepository(SCERPDBContext context) : base(context)
        {
        }
        public string GetNewOmSizeRefId(string compId)
        {
            var sqlQuery = String.Format("SELECT RIGHT('0000'+ CAST( ISNULL(MAX(SizeRefId),0)+1 as varchar(4) ),4) as SizeRefId FROM OM_Size WHERE CompId='{0}'", compId);
            return Context.Database.SqlQuery<string>(sqlQuery).FirstOrDefault();
        }
    }
}
