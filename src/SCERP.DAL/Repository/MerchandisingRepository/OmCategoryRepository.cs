using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.DAL.IRepository.IMerchandisingRepository;
using SCERP.Model;

namespace SCERP.DAL.Repository.MerchandisingRepository
{
    public class OmCategoryRepository : Repository<OM_Category>, IOmCategoryRepository
   {
        public OmCategoryRepository(SCERPDBContext context) : base(context)
        {
        }

        public string GetNewCategoryRefId(string compId)
        {
            var sqlQuery=String.Format(@"SELECT RIGHT('000'+ CAST( ISNULL(MAX(CatRefId),0)+1 as varchar(3) ),3) as CatRefId FROM OM_Category WHERE CompId='{0}'", compId);
            return Context.Database.SqlQuery<string>(sqlQuery).FirstOrDefault();
        }
   }
}
