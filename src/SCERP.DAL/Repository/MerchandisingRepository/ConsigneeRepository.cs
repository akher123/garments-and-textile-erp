using System;
using System.Linq;
using SCERP.DAL.IRepository.IMerchandisingRepository;
using SCERP.Model;

namespace SCERP.DAL.Repository.MerchandisingRepository
{
    public class ConsigneeRepository :Repository<OM_Consignee>, IConsigneeRepository 
    {
        public ConsigneeRepository(SCERPDBContext context) : base(context)
        {
        }

        public string GetNewConsigneeRefId(string compId)
        {
            var sqlQuery = String.Format(@"SELECT RIGHT('000'+ CAST( ISNULL(MAX(ConsigneeRefId),0)+1 as varchar(3) ),3) as ConsigneeRefId FROM OM_Consignee WHERE CompId='{0}'", compId);
            return Context.Database.SqlQuery<string>(sqlQuery).FirstOrDefault();
        }
    }
}
