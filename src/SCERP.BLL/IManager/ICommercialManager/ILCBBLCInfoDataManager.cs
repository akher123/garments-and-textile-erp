using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.BLL.IManager.ICommercialManager
{
    public interface ILCBBLCInfoDataManager
    {
        dynamic GetStyleWiseTna(string orderStyleRefId, string compId);
        int UpdateTna(string compId, int tnaRowId, string key, string value);
        int AddRows(int rowNumber, string orderStyleRefId, string compId);
        DataTable GetTnAReport(string orderStyleRefId, string compId);
        bool IsExistTnA(string copyOrderStyleRefId);
        bool TnACopyAndPast(string orderStyleRefId, string copyOrderStyleRefId);
        int RemoveRow(int rowNumber, string orderStyleRefId, string compId);
        dynamic GetTnaStatus(string compId, string indicationKey, string buyerRefId, string orderNo, string orderStyleRefId);
        DataTable GetBuyerWiseTnaAlert(string compId);
        DataTable GetBuyerWiseActive(string compId, string buyerRefId, string alertType);
    }
}
