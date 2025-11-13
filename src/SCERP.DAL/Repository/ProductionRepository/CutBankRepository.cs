using System;
using System.Collections.Generic;
using System.Linq;
using SCERP.DAL.IRepository.IProductionRepository;
using SCERP.Model.Production;

namespace SCERP.DAL.Repository.ProductionRepository
{
    public class CutBankRepository : Repository<PROD_CutBank>, ICutBankRepository
    {
        public CutBankRepository(SCERPDBContext context)
            : base(context)
        {
        }

        public int UpdateCutBank(string compId, string orderStyleRefId)
        {
            string sqlQuery = String.Format(@"EXEC SpProdUpdateCuttBank '{0}','{1}'", orderStyleRefId, compId);
            return Context.Database.ExecuteSqlCommand(sqlQuery);
        }

        public List<VwCutBank> GetAllCutBank(string compId, string orderStyleRefId)
        {
            return Context.VwCutBanks.Where(x => x.CompId == compId && x.OrderStyleRefId == orderStyleRefId).ToList();
        }

        public List<VwSewingInputDetail> GetAllCutBankByStyleColor(string compId, string orderStyleRefId, string colorRefId, string orderShipRefId)
        {
          // return Context.VwSewingInputDetails.Where(x => x.CompId == compId && x.OrderStyleRefId == orderStyleRefId && x.ColorRefId == colorRefId).ToList();
            string sqlQueru = String.Format(@"select * from VwSewingInputDetail where OrderStyleRefId='" +
                                            orderStyleRefId + "' and ColorRefId='" + colorRefId +
                                            "' and CompId='" + compId + "' and OrderShipRefId='" + orderShipRefId + "' order by SizeRow");
            return Context.Database.SqlQuery<VwSewingInputDetail>(sqlQueru).ToList();
        }

        public List<VwSewingInputDetail> GetPivotDictionaryByStyle(string compId, string orderStyleRefId)
        {
            string sqlQueru = String.Format(@"select * from VwSewingInputDetailStatus where OrderStyleRefId='" +
                                            orderStyleRefId  +
                                            "' and CompId='" + compId +"' order by SizeRow");
            return Context.Database.SqlQuery<VwSewingInputDetail>(sqlQueru).ToList();
        }
    }
}
