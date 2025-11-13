using System;
using System.Collections.Generic;
using SCERP.Model.Production;
namespace SCERP.DAL.IRepository.IProductionRepository
{
    public interface IFinishingProcessRepository:IRepository<PROD_FinishingProcess>
    {
        List<VwFinishingProcessDetail> GetFinishingProcessDetailByStyleColor(string compId, string orderStyleRefId, string colorRefId, long finishType);
        List<VwFinishingProcessDetail> GetFinishingProcessDetailByStyleColorStatus(string compId, string orderStyleRefId, string colorRefId, long finishType);
        List<VwFinishingProcess> GetSewingInputProcessByStyleColor(string compId, string orderStyleRefId, string colorRefId, int finishType);

        
        List<VwFinishingProcessDetail> GetPolyFinishingProcessDetailByStyleColor(string compId, string orderStyleRefId, string colorRefId, long finishingProcessId);
        List<VwFinishingProcess> GetFinishingProcess(DateTime? inputDate, int finishType, string compId);
    }
}
