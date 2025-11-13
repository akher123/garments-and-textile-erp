using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.DAL.IRepository.IMerchandisingRepository;
using SCERP.Model;

namespace SCERP.DAL.Repository.MerchandisingRepository
{
    public class ComponentRepository :Repository<OM_Component>, IComponentRepository
    {
        public ComponentRepository(SCERPDBContext context) : base(context)
        {
        }

        public string GetComponentRefId(string compId)
        {
            var sqlQuery = String.Format(@"SELECT RIGHT('000'+ CAST( ISNULL(MAX(ComponentRefId),0)+1 as varchar(3) ),3) as ComponentRefId FROM OM_Component WHERE CompId='{0}'",compId);
            return Context.Database.SqlQuery<string>(sqlQuery).FirstOrDefault();
        }
    }
}
