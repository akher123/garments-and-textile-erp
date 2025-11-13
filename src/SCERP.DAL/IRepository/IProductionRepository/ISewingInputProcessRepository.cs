using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.Production;

namespace SCERP.DAL.IRepository.IProductionRepository
{
   public interface ISewingInputProcessRepository:IRepository<PROD_SewingInputProcess> 
    {
       List<VwSewingInputProcess> GetSewingInputProcessByStyleColor(string compId, string orderStyleRefId, string colorRefId,string
           orderShipRefId);
       List<VwSewingInputProcessDetail> GetAllSewingInputInfo(string compId, long sewingInputProcessId);
       IQueryable<VwSewingInputProcess> GetSewingInputByPaging(DateTime? inputDate, int lineId, string compId);
       List<VwSewingOutput> GetVwSewingInput(string compId, string orderStyleRefId, string colorRefId, string orderShipRefId);
       List<VwSewingOutput> GetVwSewingInput(string compId, string orderStyleRefId);
        bool IsSewingInputExist(PROD_SewingInputProcess model);
       List<VwSewingInputProcess> DailySweingInPut(DateTime date, string compId);
       VwSewingInputProcess GetInputByBundleId(string bundleId);
    }
}
