using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Common;
using SCERP.Model.InventoryModel;

namespace SCERP.BLL.IManager.IInventoryManager
{
    public interface IMaterialReceiveAgainstPoManager
    {
        List<Inventory_MaterialReceiveAgainstPo> GetReceiveAgainstPoByPaging
            (int pageIndex, string sort, string sortdir, DateTime? fromDate, DateTime? toDate, string searchString, out int totalRecords,int storeId, string[]  type);

        Inventory_MaterialReceiveAgainstPo GetReceiveAgainstPoByid(long materialReceiveAgstPoId);
        string GetNewRcvRefId();
        Dictionary<string, VwMaterialReceiveAgainstPoDetail> GetDictionary(long lecevedAgainstPoId);
        int SaveReceiveAgainstPo(Inventory_MaterialReceiveAgainstPo receiveAgainstPo);
        int DeteteReceiveAgainstPo(long materialReceiveAgstPoId ,int receiveActionType);
        List<VwMaterialReceiveAgainstPo> GetMrrSummaryReport(DateTime? fromDate, DateTime? toDate, string searchString);
        List<VwMaterialReceiveAgainstPoDetail> GetVwMaterialReceiveAgainstPoDetail
            (long materialReceiveAgstPoId);

        int UpdateQc(Inventory_MaterialReceiveAgainstPo receiveAgainstPo);
        int UpdateGrn(Inventory_MaterialReceiveAgainstPo receiveAgainstPo, int receiveActionType);
        string GetNewAcessRcvRefId();
        DataTable GetAccessoriesStatusDataTable(string modelOrderStyleRefId, string compId);
        DataTable GetAccessoriesRcvDetailStatus
            (DateTime? modelFromDate, DateTime? modelToDate,string compId);
    }
}
