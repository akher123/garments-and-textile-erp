using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using SCERP.DAL.IRepository.ICommercialRepository;
using SCERP.Model;
using SCERP.Model.CommercialModel;
using SCERP.Model.MerchandisingModel;
using SCERP.Common;
using SCERP.Model.CommonModel;

namespace SCERP.DAL.Repository.CommercialRepository
{
    public class PurchaseOrderRepository : Repository<CommPurchaseOrder>, IPurchaseOrderRepository
    {
        private readonly string _companyId;

        public PurchaseOrderRepository(SCERPDBContext context) : base(context)
        {
            this._companyId = PortalContext.CurrentUser.CompId;
        }

        public List<VwPurchaseOrderDetail> GetConsumptionItems(string companyId, string orderStyleRefId, int supplierId, string accessoriesGroupCode)
        {
            var sqParams = new object[]
            {
                new SqlParameter {ParameterName = "CompId", Value = companyId},
                new SqlParameter {ParameterName = "OrderStyleRefId", Value = orderStyleRefId},
                new SqlParameter {ParameterName = "SupplierId", Value = supplierId},
                new SqlParameter {ParameterName = "GroupCode", Value = accessoriesGroupCode}
            };

            return Context.Database.SqlQuery<VwPurchaseOrderDetail>("SpOMConsumtionItem @CompId ,@OrderStyleRefId ,@SupplierId,@GroupCode", sqParams).ToList();
        }

        public List<Mrc_SupplierCompany> GetAssignedSuppliers(string orderStyleRefId, string compId)
        {
            var sqParams = new object[]
            {
                new SqlParameter {ParameterName = "CompId", Value = compId},
                new SqlParameter {ParameterName = "OrderStyleRefId", Value = orderStyleRefId},

            };
            return Context.Database.SqlQuery<Mrc_SupplierCompany>("SpOmGetAssignedSupplier @CompId ,@OrderStyleRefId", sqParams).ToList();
        }

        public List<VwCommPurchaseOrder> GetPurchaseOrderList(string companyId, string pType, string orderStyleRefId, string orderNo)
        {
            return Context.Database.SqlQuery<VwCommPurchaseOrder>("select * from  VwCommPurchaseOrder").ToList().Where(x => x.CompId == companyId && x.PType == pType && (x.OrderStyleRefId == orderStyleRefId || String.IsNullOrEmpty(orderStyleRefId))).OrderByDescending(x => x.PurchaseOrderRefId).ToList();
        }

        public IEnumerable<VwPurchaseOrderDetail> GetAllRateQuittedYarns(string companyId, string orderStyleRefId, int supplierId, string piRefId)
        {
          
            var yarnConsumptions = Context.VYarnConsumptions.Where(x => x.CompId == companyId && x.OrderStyleRefId == orderStyleRefId && x.SupplierId == supplierId&&x.PiRefId==piRefId).ToList();
            return yarnConsumptions.Select(ycons => new VwPurchaseOrderDetail()
            {
                ColorRefId = ycons.KColorRefId,
                GColorRefId = ycons.GrColorRefId,
                ColorName = ycons.KColorName,
                SizeRefId = ycons.KSizeRefId,
                SizeName = ycons.KSizeName,
                ItemCode = ycons.ItemCode,
                ItemName = ycons.ItemName,
                UnitName = ycons.UnitName,
                xRate = ycons.Rate,
                Quantity = ycons.KQty,
                SupplierId = ycons.SupplierId,
                SupplierName = ycons.SupplierName
            }).ToList();
        }

        public List<Mrc_SupplierCompany> GetQuitedYarnSupplier(string orderStyleRefId, string compId)
        {
            var sqParams = new object[]
            {
                new SqlParameter {ParameterName = "CompId", Value = compId},
                new SqlParameter {ParameterName = "OrderStyleRefId", Value = orderStyleRefId},

            };
            return Context.Database.SqlQuery<Mrc_SupplierCompany>("SpOmGetQuitedYarnSupplier @CompId ,@OrderStyleRefId", sqParams).ToList();
        }

        public List<MaterialStatus> GetSyleWiseMaterialStatus(string orderStyleRefId, string compId)
        {
            var sqParams = new object[]
            {
                new SqlParameter {ParameterName = "CompId", Value = compId},
                new SqlParameter {ParameterName = "OrderStyleRefId", Value = orderStyleRefId},

            };
            return Context.Database.SqlQuery<MaterialStatus>("SpOmMaterialStatus @CompId ,@OrderStyleRefId", sqParams).ToList();
        }

        public List<CommPurchaseOrder> GetAllPurchaseOrders()
        {
            return Context.CommPurchaseOrders.ToList();
        }

        public CommPurchaseOrder GetPurchaseOrderByPurchaseOrderNo(string purchaseOrderNo)
        {
            return Context.CommPurchaseOrders.FirstOrDefault(p => p.PurchaseOrderNo.Trim().ToLower() == purchaseOrderNo.Trim().ToLower());
        }

        public List<CommPurchaseOrderDetail> GetPurchaseOrderDetails(string purchaseOrderNo)
        {
            List<CommPurchaseOrderDetail> detail = new List<CommPurchaseOrderDetail>();
            CommPurchaseOrder temp = Context.CommPurchaseOrders.SingleOrDefault(p => p.PurchaseOrderNo.Trim().ToLower() == purchaseOrderNo.Trim().ToLower() && p.CompId == _companyId);

            if (temp != null)
            {
                detail = Context.CommPurchaseOrderDetails.Where(p => p.PurchaseOrderId == temp.PurchaseOrderId && p.CompId == _companyId).ToList();
            }
            return detail;
        }

        public List<VwCommPurchaseOrder> GetApprovalPurchaseOrderList(string companyId, bool isApproved, string pType, string searchString)
        {
            string sqQuery = String.Format(@"select * from  VwCommPurchaseOrder where PType='{0}' and IsApproved='{1}' and MerchandiserId IN(select MerchandiserRefId from UserMerchandiser where EmployeeId='{2}')  order by PurchaseOrderRefId",
                    pType, isApproved, PortalContext.CurrentUser.UserId);
            return Context.Database.SqlQuery<VwCommPurchaseOrder>(sqQuery).ToList();
        }

        public List<VwCommPurchaseOrder> GetPurchaseOrders(string companyId, bool isApproved, string pType, string searchString, string buyerRefId, string orderNo, String orderStyleRefId)
        {

            var approvedPurchaseOrders = Context.Database.SqlQuery<VwCommPurchaseOrder>("select * from  VwCommPurchaseOrder where  MerchandiserId in (select MerchandiserRefId from UserMerchandiser where EmployeeId='" + PortalContext.CurrentUser.UserId + "')");
            return approvedPurchaseOrders.Where(
                  x => x.IsApproved == isApproved && x.PType == pType && (x.BuyerRefId == buyerRefId || string.IsNullOrEmpty(buyerRefId)) && (x.OrderNo == orderNo || String.IsNullOrEmpty(orderNo)) && (x.OrderStyleRefId == orderStyleRefId || orderStyleRefId == null) && (x.PurchaseOrderRefId == searchString || searchString == null)).OrderByDescending(x => x.PurchaseOrderRefId).ToList();


        }

        public List<ProFormaInvoice> GetApporovedBookingBySupplier(int supplierId, string compId)
        {
            string sql = string.Format(@"select distinct PIN.* from CommPurchaseOrder AS PO
		           inner join ProFormaInvoice AS PIN ON PO.PurchaseOrderNo=PIN.PiRefId
		           where PO.IsApproved=1 AND PO.SupplierId='{1}' and PO.CompId='{0}'",  compId, supplierId);
            return Context.Database.SqlQuery<ProFormaInvoice>(sql).ToList();
        }
    }
}