using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using SCERP.DAL.IRepository.IMerchandisingRepository;
using SCERP.Model;
using SCERP.Model.MerchandisingModel;

namespace SCERP.DAL.Repository.MerchandisingRepository
{
    public class OmStyleRepository :Repository<OM_Style> ,IOmStyleRepository
    {
        public OmStyleRepository(SCERPDBContext context) : base(context)
        {
        }

        public IQueryable<VStyle> GetVStyles(Expression<Func<VStyle, bool>> predicate)
        {
            return Context.VStyles.Where(predicate);
        }

        public string GetNewStyleRefId(string compId)
        {
            var sqlQuery = String.Format(@"SELECT RIGHT('0000'+ CAST( ISNULL(MAX(StylerefId),0)+1 as varchar(4) ),4) as StylerefId FROM OM_Style WHERE CompId='{0}'",compId);
            return Context.Database.SqlQuery<string>(sqlQuery).FirstOrDefault();
        }
    }
}
