using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;
using SCERP.Model.InventoryModel;

namespace SCERP.DAL.IRepository.IInventoryRepository
{
  public interface IStyleShipmentRepository:IRepository<Inventory_StyleShipment> 
    {
      List<SpInventoryStyleShipment> GetStyleShipment(string orderStyleRefId, string compId, long styleShipmentId);
      DataTable GetShipmentChallan(long processDeliveryId);
      IQueryable<VwInventoryStyleShipment> GetStyleShipmentByPaging(string buyerRefId, string searchKey, string compId);
      IQueryable<VwInventoryStyleShipment> GetApprovedStyleShipmentByPaging(string compId, bool isApproved);
      DataTable GetStockPostionDetail(string compId, string buyerRefId);
      DataTable GetMonthlyShipmentSummary(string compId, DateTime? fromDate, DateTime? toDate);
    }
}
