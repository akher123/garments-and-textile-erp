using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.Production;

namespace SCERP.DAL.IRepository.IProductionRepository
{
   public interface ICutBankRepository:IRepository<PROD_CutBank>
   {
       int UpdateCutBank(string compId, string orderStyleRefId);
       List<VwCutBank> GetAllCutBank(string compId,string orderStyleRefId);
       List<VwSewingInputDetail> GetAllCutBankByStyleColor(string compId, string orderStyleRefId, string colorRefId, string orderShipRefId);
        List<VwSewingInputDetail> GetPivotDictionaryByStyle(string compId, string orderStyleRefId);
    }
}
