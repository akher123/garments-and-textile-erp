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
   public interface IMaterialReceivedManager
    {
        List<Inventory_MaterialReceived> GetMaterialReceivedByPaging(DateTime? fromDate, DateTime? toDate, string searchString, string registerType, string compId, int pageIndex, string sort, string sortdir, out int totalRecords);
        string GetMaterialReceivedRefId();
       Inventory_MaterialReceived GetMaterialReceivedId(long materialReceivedId, string compId);
       bool IsMaterialReceivedExist(Inventory_MaterialReceived model);
       int EditMaterialReceived(Inventory_MaterialReceived model);
       int SaveMaterialReceived(Inventory_MaterialReceived model);
       int DeleteMaterialReceived(long materialReceivedId, string compId); 
       DataTable GetMaterialReceivedDataTable(DateTime? fromDate, DateTime? toDate, string challanNo, string registerType,string compId);
        DataTable GetReceivedYarnByStyle(string orderStyleRefId);
    }
}
