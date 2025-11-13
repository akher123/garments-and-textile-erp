using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.DAL.IRepository.IMerchandisingRepository;
using SCERP.Model;
using SCERP.Model.InventoryModel;

namespace SCERP.DAL.Repository.MerchandisingRepository
{
    public class OmColorRepository :Repository<OM_Color>, IOmColorRepository
    {
        public OmColorRepository(SCERPDBContext context) : base(context)
        {
        }
        public string GetNewOmColorRefId(string compId)
        {
            var sqlQuery = String.Format(@"select RIGHT('0000'+ CAST(ISNULL(max(ColorRefId),0)+1 as varchar ),4) as ColorRefId from OM_Color where CompId='{0}'", compId);
            return Context.Database.SqlQuery<string>(sqlQuery).FirstOrDefault();
        }

        public IQueryable<VwLot> GetLots(string compId, string typeId)
        {
           return Context.VwLots.Where(x => x.CompId == compId && x.TypeId == typeId);
        }

    
    }
}
