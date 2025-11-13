using System;
using System.Collections.Generic;
using System.Linq;
using SCERP.DAL.IRepository.IProductionRepository;
using SCERP.Model.Production;

namespace SCERP.DAL.Repository.ProductionRepository
{
    public class FinishingProcessRepository : Repository<PROD_FinishingProcess>,IFinishingProcessRepository
    {
        public FinishingProcessRepository(SCERPDBContext context) : base(context)
        {
        }
        public List<VwFinishingProcessDetail> GetFinishingProcessDetailByStyleColor(string compId, string orderStyleRefId, string colorRefId,long finishType)
        {
            string sqlQuery = string.Format("EXEC SpProdGetIronFinishingInput '{0}','{1}' , '{2}' ,'{3}'", compId, orderStyleRefId, colorRefId, finishType);
            return Context.Database.SqlQuery<VwFinishingProcessDetail>(sqlQuery).ToList();
        }
        public List<VwFinishingProcessDetail> GetFinishingProcessDetailByStyleColorStatus(string compId, string orderStyleRefId, string colorRefId, long finishType)
        {
            string sqlQuery = string.Format("EXEC SpProdGetIronPolyFinishingStatus '{0}','{1}' , '{2}' ,'{3}'", compId, orderStyleRefId, colorRefId, finishType);
            return Context.Database.SqlQuery<VwFinishingProcessDetail>(sqlQuery).ToList();
        }

        public List<VwFinishingProcess> GetSewingInputProcessByStyleColor(string compId, string orderStyleRefId, string colorRefId,int finishType)
        {

            return Context.VwFinishingProcesses.Where(x => x.CompId == compId && x.ColorRefId == colorRefId && x.OrderStyleRefId == orderStyleRefId && x.FType == finishType).OrderBy(x=>x.FinishingProcessId).ToList();
        }
        public List<VwFinishingProcessDetail> GetPolyFinishingProcessDetailByStyleColor(string compId, string orderStyleRefId, string colorRefId, long finishingProcessId)
        {
            string sqlQuery = string.Format("EXEC SpProdGetPolyFinishingInput '{0}','{1}' , '{2}' ,'{3}'", compId, orderStyleRefId, colorRefId, finishingProcessId);

            return Context.Database.SqlQuery<VwFinishingProcessDetail>(sqlQuery).ToList();
        }
        public List<VwFinishingProcess> GetFinishingProcess(DateTime? inputDate, int finishType, string compId)
        {
            return Context.VwFinishingProcesses.Where(x => x.CompId == compId &&x.InputDate==inputDate && x.FType == finishType).ToList();
        }
    }
}
