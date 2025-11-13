using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using SCERP.Common;
using SCERP.Model.MerchandisingModel;

namespace SCERP.BLL.IManager.IPlanningManager
{
   public interface ITimeAndActionManager
    {
       DataTable GetStyleWiseTna(string orderNo, string orderStyleRefId, string buyerRefId, string compId, string searchKey, string activitySearchKey, DateTime? date);
       int UpdateTna(string compId, int tnaRowId, string key, string value);
       int AddRows(int rowNumber, string orderStyleRefId, string compId);
       DataTable GetTnAReport(string orderStyleRefId, string compId);
       bool IsExistTnA(string copyOrderStyleRefId);
       bool TnACopyAndPast(string orderStyleRefId, string copyOrderStyleRefId);
       int RemoveRow(int rowNumber, string orderStyleRefId, string compId);
       dynamic GetTnaStatus(string compId,string indicationKey,string buyerRefId,string orderNo,string orderStyleRefId);
       DataTable GetBuyerWiseTnaAlert(string compId);
       DataTable GetBuyerWiseActive(string compId, string buyerRefId, string alertType);

       dynamic GetStyleWiseTna(string orderStyleRefId, string compId);
       int CreateTnaByActivityTemplate(string orderStyleRefId);
       int Delete(string orderStyleRefId, string compId);
       bool Exist(string orderStyleRefId, string compId);
       DataTable GetHorizontalTna(string orderNo, string orderStyleRefId, string buyerRefId, string compId);
       List<OM_TnaActivity> GetTaActivityList();
       IEnumerable<string> GetTnaRespobslibles
           (string compId);

       List<string> GetTnaActivity(string searchString);
       List<string> GetTnaResponsible(string searchString);

       int CreateTnaByBuyerTemplate(string compId, string buyerRefId, int defaultTemplate, string orderStyleRefId,
           DateTime orderDate);
        int UpdateActualStartDate(string shortName, DateTime actualStartDate, string orderStyleRefId,string compId);
        int UpdateActualEndDate(string bULKCUTTING, DateTime? endDate, string orderStyleRefId, string compId);
        DataTable GetTnaActivityLog(long id,string keyName);
    }
}
