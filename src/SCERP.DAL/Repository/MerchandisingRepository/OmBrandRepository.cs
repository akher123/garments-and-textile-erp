using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.DAL.IRepository.IMerchandisingRepository;
using SCERP.Model;

namespace SCERP.DAL.Repository.MerchandisingRepository
{
    public class OmBrandRepository :Repository<OM_Brand>, IOmBrandRepository
    {
        public OmBrandRepository(SCERPDBContext context) : base(context)
        {
        }

        public string GetNewBrandRefId(string compId)
        {
            var sqlQuery = String.Format(@"SELECT RIGHT('000'+ CAST( ISNULL(MAX(CAST(BrandRefId as int )),0)+1 as varchar(3)),3)  as BrandRefId FROM OM_Brand WHERE CompId='{0}'", compId);
            return Context.Database.SqlQuery<string>(sqlQuery).FirstOrDefault();
        }
    }
}
