using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.DAL.IRepository.IMerchandisingRepository;
using SCERP.Model;
using SCERP.Model.MerchandisingModel;

namespace SCERP.DAL.Repository.MerchandisingRepository
{
    public class BuyOrdStyleSizeRepository :Repository<OM_BuyOrdStyleSize>, IBuyOrdStyleSizeRepository
    {
        public BuyOrdStyleSizeRepository(SCERPDBContext context) : base(context)
        {
        }

        public List<VBuyOrdStyleSize> GetBuyOrdStyleSize(string orderStyleRefId,string compId)
        {
            var sqlQuery = String.Format(@"select * from VBuyOrdStyleSize where OrderStyleRefId='{0}' and CompId='{1}'", orderStyleRefId, compId);
            return Context.Database.SqlQuery<VBuyOrdStyleSize>(sqlQuery).ToList();
        }

        public int UpdateBuyOrdStyleSize(long orderStyleSizeId, string sizeRefId)
        {
            string sql = string.Format("EXEC [dbo].[spUpdateStyleSize] @OrderStyleSizeId={0},@ToSizeRefId='{1}'", orderStyleSizeId, sizeRefId);
            return Context.Database.ExecuteSqlCommand(sql);
        }
    }
}
