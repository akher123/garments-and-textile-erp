using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.DAL.IRepository.IMerchandisingRepository;
using SCERP.Model;

namespace SCERP.DAL.Repository.MerchandisingRepository
{
    public class PortOfLoadingRepository : Repository<OM_PortOfLoading>, IPortOfLoadingRepository
   {
        public PortOfLoadingRepository(SCERPDBContext context) : base(context)
        {

        }

        public string GetNewPortOfLoadingfId(string compId)
        {
            var sqlQuery =String.Format(@"SELECT RIGHT('00'+ CAST( ISNULL(MAX(PortOfLoadingRefId),0)+1 as varchar(2) ),2) as PortOfLoadingRefId FROM OM_PortOfLoading WHERE CompId='{0}'",compId);

            return Context.Database.SqlQuery<string>(sqlQuery).FirstOrDefault();
        }
   }
}
