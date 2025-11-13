using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Transactions;
using SCERP.BLL.IManager.ICommercialManager;
using SCERP.Common;
using SCERP.DAL.IRepository.ICommercialRepository;
using SCERP.DAL.IRepository.IMerchandisingRepository;
using SCERP.Model;
using SCERP.Model.CommercialModel;
using SCERP.Model.CommonModel;
using SCERP.Model.InventoryModel;
using SCERP.Model.MerchandisingModel;

namespace SCERP.BLL.Manager.CommercialManager
{
    public class PurchaseOrderManager : IPurchaseOrderManager
    {
        private readonly IPurchaseOrderRepository _purchaseOrderRepository;
        private readonly IPurchaseOrderDetailRepository _purchaseOrderDetailRepository;
        private IOmBuyOrdStyleRepository _buyOrdStyleRepository;
        private IOmBuyerRepository _buyerRepository;

        public PurchaseOrderManager(IOmBuyerRepository buyerRepository, IOmBuyOrdStyleRepository buyOrdStyleRepository, IPurchaseOrderRepository purchaseOrderRepository, IPurchaseOrderDetailRepository purchaseOrderDetailRepository)
        {
            _buyerRepository = buyerRepository;
            _buyOrdStyleRepository = buyOrdStyleRepository;
            _purchaseOrderRepository = purchaseOrderRepository;
            _purchaseOrderDetailRepository = purchaseOrderDetailRepository;


        }

        public CommPurchaseOrder GetPurchaseOrderById(long purchaseOrderId)
        {
            string companyId = PortalContext.CurrentUser.CompId;
            CommPurchaseOrder purchaseOrder = _purchaseOrderRepository.FindOne(x => x.CompId == companyId && x.PurchaseOrderId == purchaseOrderId);
            purchaseOrder.CommPurchaseOrderDetail = _purchaseOrderDetailRepository.Filter(x => x.CompId == companyId && x.PurchaseOrderId == purchaseOrderId).ToList();
            return purchaseOrder;
        }

        public int DeletePurchseOrder(long purchaseOrderId)
        {
            int deleteIndex = 0;
            using (var transaction = new TransactionScope())
            {
                string companyId = PortalContext.CurrentUser.CompId;
                deleteIndex += _purchaseOrderDetailRepository.Delete(x => x.PurchaseOrderId == purchaseOrderId && x.CompId == companyId);
                deleteIndex +=
                    _purchaseOrderRepository.Delete(x => x.PurchaseOrderId == purchaseOrderId && x.CompId == companyId);
                transaction.Complete();
            }
            return deleteIndex;
        }

        public IEnumerable<VwCommPurchaseOrder> GetPurchaseOrderList(string pType, string orderStyleRefId, string orderNo)
        {
            string companyId = PortalContext.CurrentUser.CompId;
            List<VwCommPurchaseOrder> purchaseOrders = _purchaseOrderRepository.GetPurchaseOrderList(companyId,
                pType, orderStyleRefId, orderNo);


            return purchaseOrders;
        }

        public IEnumerable<VwPurchaseOrderDetail> GetAllAccessories(string orderStyleRefId, int supplierId)
        {
            string companyId = PortalContext.CurrentUser.CompId;
            const string accessoriesGroupCode = "03";
            var consumptionItems = _purchaseOrderRepository.GetConsumptionItems(companyId, orderStyleRefId, supplierId, accessoriesGroupCode);
            return consumptionItems;
        }

        public string GetNewPurchaseOrderRefId(string pType)
        {
            string companyId = PortalContext.CurrentUser.CompId;
            string purchaseOrderRefId = _purchaseOrderRepository.Filter(x => x.CompId == companyId && x.PType==pType).Max(x => x.PurchaseOrderRefId.Substring(2, 10));
            string newpurchaseOrderRefId = purchaseOrderRefId.IncrementOne().PadZero(8);
          
            if (pType=="A")
            {
                return "PO" + newpurchaseOrderRefId;
            }
            else
            {
                return "YO" + newpurchaseOrderRefId;
            }
           

        }

        public int SavePurchaseOrder(CommPurchaseOrder purchaseOrder)
        {
            purchaseOrder.CompId = PortalContext.CurrentUser.CompId;
            purchaseOrder.UserId = PortalContext.CurrentUser.UserId;
            return _purchaseOrderRepository.Save(purchaseOrder);
        }

        public IEnumerable<VwPurchaseOrderDetail> GetAllPurchaseOrderDetails(long purchaseOrderId)
        {
            string compId = PortalContext.CurrentUser.CompId;
            return _purchaseOrderDetailRepository.GetAllPurchaseOrderDetails(purchaseOrderId, compId);
        }

        public int EditPurchaseOrder(CommPurchaseOrder purchaseOrder)
        {
            int editedIndex = 0;
            using (var transaction = new TransactionScope())
            {
                string compId = PortalContext.CurrentUser.CompId;
                _purchaseOrderDetailRepository.Delete(x => x.PurchaseOrderId == purchaseOrder.PurchaseOrderId && x.CompId == compId);
                _purchaseOrderRepository.Delete(x => x.PurchaseOrderId == purchaseOrder.PurchaseOrderId && x.CompId == compId);
                purchaseOrder.PurchaseOrderId = 0;
                purchaseOrder.CompId = PortalContext.CurrentUser.CompId;
                purchaseOrder.UserId = PortalContext.CurrentUser.UserId;
                editedIndex = _purchaseOrderRepository.Save(purchaseOrder);
                transaction.Complete();
            }
            return editedIndex;
        }

        public List<Mrc_SupplierCompany> GetAssignedSuppliers(string orderStyleRefId)
        {
            string compId = PortalContext.CurrentUser.CompId;
            return _purchaseOrderRepository.GetAssignedSuppliers(orderStyleRefId, compId);
        }

        public IEnumerable<VwPurchaseOrderDetail> GetAllRateQuittedYarns(string orderStyleRefId, int supplierId,string PiRefId)
        {
            string companyId = PortalContext.CurrentUser.CompId;
       
            IEnumerable<VwPurchaseOrderDetail> consumptionItems = _purchaseOrderRepository.GetAllRateQuittedYarns(companyId, orderStyleRefId, supplierId, PiRefId);
            return consumptionItems;
        }

        public List<Mrc_SupplierCompany> GetQuitedYarnSupplier(string orderStyleRefId)
        {
            string compId = PortalContext.CurrentUser.CompId;
            return _purchaseOrderRepository.GetQuitedYarnSupplier(orderStyleRefId, compId);
        }

        public List<MaterialStatus> GetSyleWiseMaterialStatus(string orderStyleRefId)
        {
            string compId = PortalContext.CurrentUser.CompId;
            return _purchaseOrderRepository.GetSyleWiseMaterialStatus(orderStyleRefId, compId);
        }

        public List<CommPurchaseOrder> GetAllPurchaseOrders()
        {
            return _purchaseOrderRepository.GetAllPurchaseOrders();
        }

        public CommPurchaseOrder GetPurchaseOrderByPurchaseOrderNo(string purchaseOrderNo)
        {
            return _purchaseOrderRepository.GetPurchaseOrderByPurchaseOrderNo(purchaseOrderNo);
        }

        public List<CommPurchaseOrderDetail> GetPurchaseOrderDetails(string purchaseOrderNo)
        {
            return _purchaseOrderRepository.GetPurchaseOrderDetails(purchaseOrderNo);
        }

        public IEnumerable GetPurchaseOrderByType(string pType,string orderStyleRefId)
        {

            var pibookings = _purchaseOrderRepository.GetPurchaseOrderList(PortalContext.CurrentUser.CompId,pType, orderStyleRefId, "").
               Select(x => new
            {
                Value = x.PurchaseOrderRefId,
                Text ="Ref No :"+x.PurchaseOrderRefId+" Supplier :"+x.SupplierName
            }).ToList();
            return pibookings;
        }

        public Inventory_Booking GetPurchaseOrderByRefId(string piBookingRefId)
        {
          var porder=  _purchaseOrderRepository.FindOne(
                x => x.PurchaseOrderRefId == piBookingRefId  && x.CompId == PortalContext.CurrentUser.CompId);
          var orderStyle=  _buyOrdStyleRepository.GetVBuyerOrderStyle(PortalContext.CurrentUser.CompId).FirstOrDefault(x=>x.OrderStyleRefId==porder.OrderStyleRefId);
          var buyer=  _buyerRepository.FindOne(x => x.BuyerRefId == orderStyle.BuyerRefId&&x.CompId==PortalContext.CurrentUser.CompId);
            return new Inventory_Booking()
            {
                BookingId = porder.PurchaseOrderId,
                SupplierId = porder.SupplierId,
                BookingRefId = porder.PurchaseOrderRefId,
                StyleNo = orderStyle.StyleName,
                OrderNo = orderStyle.RefNo,
                OrderStyleRefId = porder.OrderStyleRefId,
                BuyerId = buyer.BuyerId,
                BookingDate = porder.PurchaseOrderDate.GetValueOrDefault()
            };
        }

        public Dictionary<string, VwMaterialReceiveAgainstPoDetail> GetAllPurchaseOrderDetailsByRefId(string piBookingRefId)
        {
            string compId = PortalContext.CurrentUser.CompId;
            if (!String.IsNullOrEmpty(piBookingRefId))
            {
                var porder = _purchaseOrderRepository.FindOne(x => x.CompId == compId && x.PurchaseOrderRefId == piBookingRefId);
                var poDetails = _purchaseOrderDetailRepository.GetVwSpPoSheetDetails(porder.PurchaseOrderId);
                
                return poDetails.ToDictionary(x => Convert.ToString(x.PurchaseOrderDetailId), x => new VwMaterialReceiveAgainstPoDetail()
                {
                    ItemId = x.ItemId,
                    ItemName = x.ItemName,
                    ColorRefId = x.ColorRefId??"0000",
                    SizeName = x.SizeName,
                    ColorName = x.ColorName,
                    SizeRefId = x.SizeRefId??"0000",
                    GColorName = x.GColorName,
                    GSizeName = x.GSizeName,
                    GSizeRefId = x.GSizeRefId ?? "0000",
                    FColorRefId = x.GColorRefId ?? "0000",
                    ReceivedRate = x.xRate.GetValueOrDefault(),
                    //ReceivedQty= x.Quantity.GetValueOrDefault()-x.TotalRcvQty.GetValueOrDefault(),
                    TotalRcvQty= x.TotalRcvQty,
                    BalanceQty = x.Quantity.GetValueOrDefault()-x.TotalRcvQty.GetValueOrDefault(),
                    PurchaseOrderDetailId = x.PurchaseOrderDetailId,
                    UnitName = x.UnitName,
                    PurchaseOrderRefId = x.PurchaseOrderRefId
                });
            }
            return new Dictionary<string, VwMaterialReceiveAgainstPoDetail>();
        }

        public IEnumerable<VwCommPurchaseOrder> GetApprovalPurchaseOrderList(string pType,bool isApproved, string searchString,string buyerRefId,string orderNo,String orderStyleRefId)
        {
            string companyId = PortalContext.CurrentUser.CompId;
            List<VwCommPurchaseOrder> purchaseOrders = _purchaseOrderRepository.GetPurchaseOrders(companyId,
                isApproved, pType, searchString,buyerRefId,orderNo,orderStyleRefId);
     
            return purchaseOrders;
        }

        public bool IsBookingApproed(long purchaseOrderId)
        {
            var purchaseOrder = _purchaseOrderRepository.FindOne(x => x.CompId == PortalContext.CurrentUser.CompId && x.PurchaseOrderId == purchaseOrderId);
            purchaseOrder.IsApproved = purchaseOrder.IsApproved != true;
            purchaseOrder.ApprovedBy = purchaseOrder.IsApproved == true ? PortalContext.CurrentUser.UserId : null;
            return _purchaseOrderRepository.Edit(purchaseOrder)>0;
        }

        public Dictionary<string, VwMaterialReceiveAgainstPoDetail> GetYarnPurchaseOrderDetails(long piBookingRefId)
        {
            return _purchaseOrderDetailRepository.GetAllPurchaseOrderDetails(piBookingRefId,PortalContext.CurrentUser.CompId).ToDictionary(x => Convert.ToString(x.PurchaseOrderDetailId), x => new VwMaterialReceiveAgainstPoDetail()
            {
                ItemId = x.ItemId,
                ItemName = x.ItemName,
                ColorRefId ="0000",
                ColorName = "",
                SizeName = x.SizeName,
                FColorRefId = x.ColorRefId,
                FColorName = x.ColorName,
                SizeRefId = x.SizeRefId,
                ReceivedRate = x.xRate.GetValueOrDefault(),
                ReceivedQty = x.Quantity.GetValueOrDefault(),

            });
             
        }

        public Dictionary<string, VwMaterialReceiveAgainstPoDetail> GetAllPurchaseOrderDetailsByStyleRefId(string orderStyleRefId)
        {
           string compId = PortalContext.CurrentUser.CompId;
           if (!String.IsNullOrEmpty(orderStyleRefId))
            {

                var poDetails = _purchaseOrderDetailRepository.GetVwSpPoSheetDetailByStyle(orderStyleRefId);
                
                return poDetails.ToDictionary(x => Convert.ToString(x.PurchaseOrderDetailId), x => new VwMaterialReceiveAgainstPoDetail()
                {
                    ItemId = x.ItemId,
                    ItemName = x.ItemName,
                    ColorRefId = x.ColorRefId??"0000",
                    SizeName = x.SizeName,
                    ColorName = x.ColorName,
                    SizeRefId = x.SizeRefId??"0000",
                    GColorName = x.GColorName,
                    GSizeName = x.GSizeName,
                    GSizeRefId = x.GSizeRefId ?? "0000",
                    FColorRefId = x.GColorRefId ?? "0000",
                    ReceivedRate = x.xRate.GetValueOrDefault(),
                    //ReceivedQty= x.Quantity.GetValueOrDefault()-x.TotalRcvQty.GetValueOrDefault(),
                    TotalRcvQty= x.TotalRcvQty,
                    BalanceQty = x.Quantity.GetValueOrDefault()-x.TotalRcvQty.GetValueOrDefault(),
                    PurchaseOrderDetailId = x.PurchaseOrderDetailId,
                    UnitName = x.UnitName,
                    PurchaseOrderRefId = x.PurchaseOrderRefId
                });
            }
            return new Dictionary<string, VwMaterialReceiveAgainstPoDetail>();
        
        }

        public Dictionary<string, VwMaterialReceiveAgainstPoDetail> GetYarnPurchaseOrderDetailsByPiRefId(string piRefId)
        {
           
            string compId = PortalContext.CurrentUser.CompId;
            if (!String.IsNullOrEmpty(piRefId))
            {

                List<VwSpPoSheet> poDetails = _purchaseOrderDetailRepository.GetYarnPurchaseOrderDetailsByPiRefId(piRefId, PortalContext.CurrentUser.CompId);

                return poDetails.ToDictionary(x => Guid.NewGuid().ToString(), x => new VwMaterialReceiveAgainstPoDetail()
                {
                    ItemId = x.ItemId,
                    ItemName = x.ItemName,
                    ColorRefId = "0000",
                    ColorName = "",
                    SizeRefId = x.SizeRefId ?? "0000",
                    SizeName = x.SizeName,
                    FColorRefId = x.ColorRefId,
                    FColorName = x.ColorName,
                    GColorName = x.GColorName,
                    GSizeName = x.GSizeName,
                    GSizeRefId = x.GSizeRefId ?? "0000",
                    ReceivedRate = x.xRate.GetValueOrDefault(),
                    TotalRcvQty = x.TotalRcvQty,
                    TotalAmount=x.Quantity.GetValueOrDefault(),
                    BalanceQty = x.Quantity.GetValueOrDefault() - x.TotalRcvQty.GetValueOrDefault(),
                    UnitName = x.UnitName,
                    StyleName=x.StyleName,
                    OrderStyleRefId = x.OrderStyleRefId
                });
            }
            return new Dictionary<string, VwMaterialReceiveAgainstPoDetail>();
        }

        public List<ProFormaInvoice> GetApporovedBookingBySupplier(int supplierId, string compId)
        {
            return _purchaseOrderRepository.GetApporovedBookingBySupplier(supplierId, compId);
        }

        public DataTable GetYarBookingSummaryByStyle(string orderStyleRefId)
        {
            string sql = @"select 
                        ISNULL((select top(1) ItemName from Inventory_Item where ItemCode=POD.ItemCode),'Total :') AS Yarn,
                        (select ColorName from OM_Color where ColorRefId=POD.ColorRefId and CompId=POD.CompId) AS Color,
                        (select SizeName from OM_Size where SizeRefId=POD.SizeRefId and CompId=POD.CompId) AS [Count]
                        ,SUM(POD.Quantity) AS Quantity
                        from CommPurchaseOrder AS PO 
                        INNER JOIN CommPurchaseOrderDetail AS POD ON PO.PurchaseOrderRefId=POD.PurchaseOrderRefId
                        WHERE PO.OrderStyleRefId='{0}' and PO.PType='Y' 
                        GROUP BY GROUPING SETS ((POD.CompId,POD.SizeRefId,POD.ColorRefId,POD.ItemCode), ())";
            sql = string.Format(sql, orderStyleRefId);
            return _purchaseOrderRepository.ExecuteQuery(sql);
        }
    }
}
