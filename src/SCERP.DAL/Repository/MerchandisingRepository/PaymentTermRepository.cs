using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.DAL.IRepository.IMerchandisingRepository;
using SCERP.Model;

namespace SCERP.DAL.Repository.MerchandisingRepository
{
    public class PaymentTermRepository :Repository<OM_PaymentTerm>, IPaymentTermRepository
    {
        public PaymentTermRepository(SCERPDBContext context) : base(context)
        {
        }

        public string GetPayTermRef(string compId)
        {
            var sqlQuery = String.Format(@"SELECT RIGHT('00'+ CAST( ISNULL(MAX(PayTermRefId),0)+1 as varchar(2) ),2) as PayTermRefId FROM OM_PaymentTerm WHERE CompId='{0}'", compId);
            return Context.Database.SqlQuery<string>(sqlQuery).FirstOrDefault();
        }
    }
}
