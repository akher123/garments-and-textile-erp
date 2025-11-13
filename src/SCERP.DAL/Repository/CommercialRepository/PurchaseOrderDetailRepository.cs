using System;
using System.Collections.Generic;
using System.Linq;
using SCERP.DAL.IRepository.ICommercialRepository;
using SCERP.Model.CommercialModel;
using SCERP.Model.InventoryModel;

namespace SCERP.DAL.Repository.CommercialRepository
{
    public class PurchaseOrderDetailRepository : Repository<CommPurchaseOrderDetail>, IPurchaseOrderDetailRepository
    {
        public PurchaseOrderDetailRepository(SCERPDBContext context) : base(context)
        {
        }
        public IEnumerable<VwPurchaseOrderDetail> GetAllPurchaseOrderDetails(long purchaseOrderId,string compId)
        {
         return   Context.VwPurchaseOrderDetail.Where(x => x.PurchaseOrderId == purchaseOrderId && x.CompId == compId);
        }

        public List<VwSpPoSheet> GetVwSpPoSheetDetails(long purchaseOrderId)
        {
            string sqQuery = String.Format("select* from VwSpPoSheet where PurchaseOrderId='{0}'", purchaseOrderId);
            return Context.Database.SqlQuery<VwSpPoSheet>(sqQuery).OrderBy(x=>x.ItemName).ThenBy(x=>x.ColorRefId).ThenBy(x=>x.SizeRefId).ThenBy(x=>x.GColorName).ThenBy(x=>x.GSizeName).ToList();
        }

        public List<VwSpPoSheet> GetVwSpPoSheetDetailByStyle(string orderStyleRefId)
        {
            string sqQuery = String.Format("select* from VwSpPoSheet where OrderStyleRefId='{0}'", orderStyleRefId);
            return Context.Database.SqlQuery<VwSpPoSheet>(sqQuery).OrderBy(x => x.ItemName).ThenBy(x => x.ColorRefId).ThenBy(x => x.SizeRefId).ThenBy(x => x.GColorName).ThenBy(x => x.GSizeName).ToList();
        }

        public List<VwSpPoSheet> GetYarnPurchaseOrderDetailsByPiRefId(string piRefId,string compId)
        {
           
            string sqQuery = String.Format("exec spGetYarnBookingByPiRefId @CompId='{0}',@PiRfId='{1}'", compId, piRefId);
            return Context.Database.SqlQuery<VwSpPoSheet>(sqQuery).OrderBy(x => x.ItemName).ToList();
        }
    }
}
