using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Transactions;
using SCERP.BLL.IManager.IInventoryManager;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.Common;
using SCERP.DAL.IRepository.IInventoryRepository;
using SCERP.DAL.IRepository.IMerchandisingRepository;
using SCERP.Model.InventoryModel;

namespace SCERP.BLL.Manager.InventoryManager
{
    public class StyleShipmentManager : IStyleShipmentManager
    {
        private readonly IStyleShipmentRepository _styleShipmentRepository;
        private readonly IStyleShipmentDetail _styleShipmentDetail;
        private IOmBuyOrdStyleRepository _buyOrdStyleRepository;
        public StyleShipmentManager(IOmBuyOrdStyleRepository buyOrdStyleRepository,IStyleShipmentRepository styleShipmentRepository, IStyleShipmentDetail styleShipmentDetail)
        {
            _buyOrdStyleRepository = buyOrdStyleRepository;
            _styleShipmentRepository = styleShipmentRepository;
            _styleShipmentDetail = styleShipmentDetail;
        }

        public List<VwInventoryStyleShipment> GetStyleShipmentByPaging(int pageIndex, string sort, string sortdir, string buyerRefId, string searchKey, string compId, out int totalRecords)
        {
            var index = pageIndex;
            var pageSize = AppConfig.PageSize;
            var styleShipmentList = _styleShipmentRepository.GetStyleShipmentByPaging(buyerRefId, searchKey, compId);
            totalRecords = styleShipmentList.Count();
            var  styleShipments= styleShipmentList.OrderByDescending(r => r.StyleShipmentId).Skip(index * pageSize).Take(pageSize);
                return styleShipments.ToList();
        }

        public List<SpInventoryStyleShipment> GetStyleShipment(string orderStyleRefId, string compId,long styleShipmentId)
        {
            return _styleShipmentRepository.GetStyleShipment(orderStyleRefId, compId, styleShipmentId);
        }

        public int SaveStyleShipment(Inventory_StyleShipment model)
        {
            model.IsApproved = false;
            model.StyleShipmentRefId = GetNewStyleShipmentRefId();
            model.PrepairedBy = PortalContext.CurrentUser.UserId;
            return _styleShipmentRepository.Save(model);
        }

        public int EditStyleShipment(Inventory_StyleShipment model)
        {
            int edited = 0;
            using (var transaction = new TransactionScope())
            {
                edited+=_styleShipmentDetail.Delete(x => x.StyleShipmentId == model.StyleShipmentId && x.CompId==model.CompId);
                Inventory_StyleShipment styleShipment = _styleShipmentRepository.FindOne(x => x.StyleShipmentId == model.StyleShipmentId && x.CompId == model.CompId);
                styleShipment.Messrs = model.Messrs;
                styleShipment.Address = model.Address;
                styleShipment.InvoiceNo = model.InvoiceNo;
                styleShipment.InvoiceDate = model.InvoiceDate;
                styleShipment.ShipDate = model.ShipDate;
                styleShipment.DepoName = model.DepoName;
                styleShipment.Through = model.Through;
                styleShipment.ThroughCellNo = model.ThroughCellNo;
                styleShipment.VehicleNo = model.VehicleNo;
                styleShipment.DriverName = model.DriverName;
                styleShipment.DriverLicenceNo = model.DriverLicenceNo;
                styleShipment.DriverCellNo = model.DriverCellNo;
                styleShipment.DriverNid = model.DriverNid;
                styleShipment.ShipmentMode = model.ShipmentMode;
                styleShipment.LockNo = model.LockNo;
                styleShipment.Remarks = model.Remarks;
                styleShipment.BuyerRefId = model.BuyerRefId;
                styleShipment.OrderNo = model.OrderNo;
                styleShipment.OrderStyleRefId = model.OrderStyleRefId;
                edited = _styleShipmentRepository.Edit(styleShipment);
                var detail = model.Inventory_StyleShipmentDetail.Select(x =>
                {
                    x.StyleShipmentId = model.StyleShipmentId;
                    return x;
                });
                edited += _styleShipmentDetail.SaveList(detail.ToList());
                transaction.Complete();
            }
            return edited;
        }

        public string GetNewStyleShipmentRefId()
        {
            var maxShipmentRefId = _styleShipmentRepository.Filter(x => x.CompId == PortalContext.CurrentUser.CompId).Max(x => x.StyleShipmentRefId.Substring(4)) ?? "0";
            return "PFL/" + maxShipmentRefId.IncrementOne().PadZero(4);
        }

        public Inventory_StyleShipment GetStyleShipmentById(long styleShipmentId)
        {
            return _styleShipmentRepository.FindOne(x=>x.StyleShipmentId==styleShipmentId);
        }

        public int ApprovedStyleShipment(long styleShipmentId, string compId)
        {
            Inventory_StyleShipment styleShipment = _styleShipmentRepository.FindOne(x => x.CompId == compId && x.StyleShipmentId == styleShipmentId);
            styleShipment.IsApproved = styleShipment.IsApproved != true;
            styleShipment.ApprovedBy = styleShipment.IsApproved == true ? PortalContext.CurrentUser.UserId : null;
            styleShipment.ApprovedBy = PortalContext.CurrentUser.UserId;
            return _styleShipmentRepository.Edit(styleShipment);
        }

        public List<VwInventoryStyleShipment> GetApprovedStyleShipmentByPaging(int pageIndex, string sort, string sortdir, bool isApproved, string compId, out int totalRecords)
        {
            var index = pageIndex;
            var pageSize = AppConfig.PageSize;
            IQueryable<VwInventoryStyleShipment> styleShipmentList = _styleShipmentRepository.GetApprovedStyleShipmentByPaging(compId,isApproved);
            totalRecords = styleShipmentList.Count();
            switch (sort)
            {
                case "Messrs":
                    switch (sortdir)
                    {
                        case "DESC":
                            styleShipmentList = styleShipmentList
                                .OrderByDescending(r => r.StyleShipmentId)
                                .Skip(index * pageSize)
                                .Take(pageSize);

                            break;
                        default:
                            styleShipmentList = styleShipmentList
                                .OrderBy(r => r.StyleShipmentId)
                                .Skip(index * pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;

                default:
                    styleShipmentList = styleShipmentList
                        .OrderByDescending(r => r.StyleShipmentId)
                        .Skip(index * pageSize)
                        .Take(pageSize);
                    break;
            }
            return styleShipmentList.ToList();
        }

        public DataTable GetShipmentChallan(long processDeliveryId)
        {
           return _styleShipmentRepository.GetShipmentChallan(processDeliveryId);
        }

        public bool IsShipmentApproved(long styleShipmentId) 
        {
            return _styleShipmentRepository.Exists(x => x.StyleShipmentId == styleShipmentId && x.IsApproved);
        }

        public List<VwInventoryStyleShipment> GetShipmentStyleRefIds(long styleShipmentId)
        {
            List<VwInventoryStyleShipment> details = _styleShipmentDetail.GetShipmentStyleRefIds(styleShipmentId);
            return details;
        }

        public int DeleteStyleShipmentById(long styleShipmentId)
        {
            int deleted = 0;
            using (var transaction = new TransactionScope())
            {
                deleted += _styleShipmentDetail.Delete(x => x.StyleShipmentId == styleShipmentId);
                deleted += _styleShipmentRepository.Delete(x => x.StyleShipmentId == styleShipmentId);
                transaction.Complete();
            }
            return deleted;
        }

        public DataTable GetStockPostionDetail(string buyerRefId,string compId)
        {
            return _styleShipmentRepository.GetStockPostionDetail(compId, buyerRefId);
        }

        public DataTable GetMonthlyShipmentSummary(DateTime? fromDate, DateTime? toDate)
        {
            return _styleShipmentRepository.GetMonthlyShipmentSummary(PortalContext.CurrentUser.CompId, fromDate, toDate);
        }
    }
}
