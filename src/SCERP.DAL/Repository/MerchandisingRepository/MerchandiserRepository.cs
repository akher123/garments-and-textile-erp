using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.DAL.IRepository.IMerchandisingRepository;
using SCERP.Model;

namespace SCERP.DAL.Repository.MerchandisingRepository
{
   public class MerchandiserRepository :Repository<OM_Merchandiser>, IMerchandiserRepository
    {
        public MerchandiserRepository(SCERPDBContext context) : base(context)
        {
        }

       public string GetMerchandiserRefId(string compId)
       {
           var sqlQuery = String.Format(@"SELECT RIGHT('0000'+ CAST( ISNULL(MAX(EmpId),0)+1 as varchar(4) ),4) as EmpId FROM OM_Merchandiser WHERE CompId='{0}'", compId);
           return Context.Database.SqlQuery<string>(sqlQuery).FirstOrDefault();
       }

       public bool IsMerchandiser(Guid? userId)
       {
           return Context.Database.SqlQuery<int>(string.Format(@"exec SPCheckMerchandiser '{0}'",userId)).FirstOrDefault()>0;
       }
    }
}
