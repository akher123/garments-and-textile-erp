using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;
using SCERP.Model.InventoryModel;

namespace SCERP.BLL.IManager.IInventoryManager
{
   public interface IStyleShipmentManager 
    {
       List<VwInventoryStyleShipment> GetStyleShipmentByPaging(int pageIndex, string sort, string sortdir, string buyerRefId, string searchKey, string compId, out int totalRecords);
       List<SpInventoryStyleShipment> GetStyleShipment(string orderStyleRefId, string compId, long styleShipmentId);
       int SaveStyleShipment(Inventory_StyleShipment model); 
       int EditStyleShipment(Inventory_StyleShipment model);
       string GetNewStyleShipmentRefId();
       Inventory_StyleShipment GetStyleShipmentById(long styleShipmentId);
       int ApprovedStyleShipment(long styleShipmentId, string compId);
       List<VwInventoryStyleShipment> GetApprovedStyleShipmentByPaging(int pageIndex, string sort, string sortdir, bool isApproved, string compId, out int totalRecords);
       DataTable GetShipmentChallan(long processDeliveryId);
       bool IsShipmentApproved(long styleShipmentId);
       List<VwInventoryStyleShipment> GetShipmentStyleRefIds(long styleShipmentId);
       int DeleteStyleShipmentById(long styleShipmentId);
       DataTable GetStockPostionDetail(string buyerRefId,string compId);
       DataTable GetMonthlyShipmentSummary(DateTime? fromDate, DateTime? toDate);
    }
}
