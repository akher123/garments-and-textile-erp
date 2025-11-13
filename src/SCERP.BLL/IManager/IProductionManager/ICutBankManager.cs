using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.Production;

namespace SCERP.BLL.IManager.IProductionManager
{
   public interface ICutBankManager 
    {
       int UpdateCutBank(string orderStyleRefId);
       List<VwCutBank> GetAllCutBank(string orderStyleRefId);
       List<VwSewingInputDetail> GetAllCutBankByStyleColor(string orderStyleRefId, string colorRefId, string orderShipRefId);
        Dictionary<string, Dictionary<string, List<string>>>  GetPivotDictionaryByStyle(string orderStyleRefId);
    }
}
