using SCERP.Model.CommercialModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.BLL.IManager.ICommercialManager
{
    public interface ICommTNAManager
    {
        List<CommTNA> GetLCWiseTna(int LCNoRefId);
        int UpdateTna(string compId, int commTnaRowId, string key, string value);
        int AddRows(int rowNumber, int LCNoRefId, string compId);
        DataTable GetTnAReport(string orderStyleRefId, string compId);
        bool IsExistTnA(string copyOrderStyleRefId);
        bool TnACopyAndPast(string orderStyleRefId, string copyOrderStyleRefId);
        int RemoveRow(int rowNumber, int LCNoRefId, string compId);
        dynamic GetTnaStatus(string compId, string indicationKey, string buyerRefId, string orderNo, string orderStyleRefId);
        DataTable GetBuyerWiseTnaAlert(string compId);
        DataTable GetBuyerWiseActive(string compId, string buyerRefId, string alertType);

        dynamic GetLCWiseTna(string orderStyleRefId, string compId);
        int CreateTnaByActivityTemplate(string orderStyleRefId);
        int Delete(string orderStyleRefId, string compId);
        bool Exist(string orderStyleRefId, string compId);
    }
}
