using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.DAL.IRepository.IMerchandisingRepository;
using SCERP.Model;

namespace SCERP.DAL.Repository.MerchandisingRepository
{
    public class OrderTypeRepository :Repository<OM_OrderType>, IOrderTypeRepository
    {
        public OrderTypeRepository(SCERPDBContext context) : base(context)
        {
        }

        public string GetNewOTypeRefId()
        {
            var sqlQuery = String.Format(@"SELECT RIGHT('00'+ CAST( ISNULL(MAX(OTypeRefId),0)+1 as varchar(2) ),2) as OTypeRefId FROM OM_OrderType");
            return Context.Database.SqlQuery<string>(sqlQuery).FirstOrDefault();
        }
    }
}
