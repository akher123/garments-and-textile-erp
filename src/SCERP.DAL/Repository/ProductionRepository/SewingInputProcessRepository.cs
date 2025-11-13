using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using SCERP.Common;
using SCERP.DAL.IRepository.IProductionRepository;
using SCERP.Model.Production;

namespace SCERP.DAL.Repository.ProductionRepository
{
    public class SewingInputProcessRepository : Repository<PROD_SewingInputProcess>,ISewingInputProcessRepository
    {
        public SewingInputProcessRepository(SCERPDBContext context) : base(context)
        {
        }

        public List<VwSewingInputProcess> GetSewingInputProcessByStyleColor(string compId, string orderStyleRefId, string colorRefId,string orderShipRefId)
        {
            return Context.VwSewingInputProcess.Where(x => x.CompId == compId && x.OrderStyleRefId == orderStyleRefId && x.ColorRefId == colorRefId&&x.OrderShipRefId==orderShipRefId).ToList();
        }

        public List<VwSewingInputProcessDetail> GetAllSewingInputInfo(string compId, long sewingInputProcessId)
        {
            string sqlQuery =string.Format(@"EXEC SpProdSewingIputProcessEdit {0},'{1}'",sewingInputProcessId,compId);
             return Context.Database.SqlQuery<VwSewingInputProcessDetail>(sqlQuery).ToList();

            // return Context.VwSewingInputProcessDetails.Where(x => x.CompId == compId && x.SewingInputProcessId==sewingInputProcessId).OrderBy(x=>x.SizeRow).ToList();
        }

        public IQueryable<VwSewingInputProcess> GetSewingInputByPaging(DateTime? inputDate, int lineId, string compId)
        {
            return Context.VwSewingInputProcess.Where( x => x.CompId == compId&&x.OnlyDate==inputDate && (x.LineId == lineId || lineId == 0));
        }

        public List<VwSewingOutput> GetVwSewingInput(string compId, string orderStyleRefId, string colorRefId, string orderShipRefId)
        {
            string sqlQuery = String.Format(@"select * from VwSewingFindInput where ColorRefId='{0}' and OrderStyleRefId='{1}' and CompId='{2}' and OrderShipRefId='{3}' ", colorRefId, orderStyleRefId, compId,orderShipRefId);
            return Context.Database.SqlQuery<VwSewingOutput>(sqlQuery).ToList();
        }
        public List<VwSewingOutput> GetVwSewingInput(string compId, string orderStyleRefId)
        {
            string sqlQuery = String.Format(@"select * from VwSewingFindInputStatus where OrderStyleRefId='{0}' and CompId='{1}' ",  orderStyleRefId, compId);
            return Context.Database.SqlQuery<VwSewingOutput>(sqlQuery).ToList();
        }
        public bool IsSewingInputExist(PROD_SewingInputProcess model)
        {
            var isExist = Context.VwSewingInputProcess.Any(x => x.CompId == model.CompId && x.OnlyDate == model.InputDate && x.OrderStyleRefId == model.OrderStyleRefId && x.ColorRefId == model.ColorRefId && x.LineId == model.LineId && x.HourId == model.HourId && x.SewingInputProcessId != model.SewingInputProcessId);
            return isExist;
        }

        public List<VwSewingInputProcess> DailySweingInPut(DateTime date, string compId)
        {
            return  Context.VwSewingInputProcess.Where(x => x.CompId == compId && x.OnlyDate == date).ToList();
        }

        public VwSewingInputProcess GetInputByBundleId(string bundleId)
        {
            return
                Context.VwSewingInputProcess.FirstOrDefault(
                    x => x.CompId == PortalContext.CurrentUser.CompId && x.JobNo == bundleId);
        }
    }
}
