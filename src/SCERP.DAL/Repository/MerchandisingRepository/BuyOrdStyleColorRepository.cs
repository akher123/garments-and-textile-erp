using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.DAL.IRepository.IMerchandisingRepository;
using SCERP.Model;
using SCERP.Model.MerchandisingModel;

namespace SCERP.DAL.Repository.MerchandisingRepository
{
    public class BuyOrdStyleColorRepository :Repository<OM_BuyOrdStyleColor>, IBuyOrdStyleColorRepository
    {
        public BuyOrdStyleColorRepository(SCERPDBContext context) : base(context)
        {
        }

        public List<VBuyOrdStyleColor> GetBuyOrdStyleColor(string orderStyleRefId,string compId)
        {

            var sqlQurty = String.Format(@"select * from VBuyOrdStyleColor where OrderStyleRefId='{0}' and CompId='{1}'", orderStyleRefId, compId);
            return Context.Database.SqlQuery<VBuyOrdStyleColor>(sqlQurty).ToList();
        }

        public VBuyOrdStyleColor GetBuyOrdStyleColorById(long id)
        {
            var sqlQurty = String.Format(@"select * from VBuyOrdStyleColor where OrderStyleColorId='{0}'", id);
            return Context.Database.SqlQuery<VBuyOrdStyleColor>(sqlQurty).SingleOrDefault();
        }

        public VBuyOrdStyleSize GetBuyOrdStyleSizeById(long id)
        {
            var sqlQurty = String.Format(@"select * from VBuyOrdStyleSize where OrderStyleSizeId='{0}'", id);
            return Context.Database.SqlQuery<VBuyOrdStyleSize>(sqlQurty).SingleOrDefault();
        }

        public IEnumerable GetSizeByOrderStyleRefId(string orderStyleRefId, string compId)
        {
            var sqlQurty = String.Format(@"select * from VBuyOrdStyleSize where OrderStyleRefId='{0}' and CompId='{1}'", orderStyleRefId, compId);
            return Context.Database.SqlQuery<VBuyOrdStyleSize>(sqlQurty).ToList();
        }

        public int UpdateBuyOrdStyleColor(long orderStyleColorId,string colorRefId)
        {
            string sql = string.Format("exec spUpdateStyleColor @OrderStyleColorId={0},@ToColorRefId='{1}'", orderStyleColorId, colorRefId);
            return Context.Database.ExecuteSqlCommand(sql);
        }
    }
}
