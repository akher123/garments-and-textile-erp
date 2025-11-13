using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Transactions;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.Common;
using SCERP.DAL.IRepository;
using SCERP.DAL.IRepository.IMerchandisingRepository;
using SCERP.Model;
using SCERP.Model.MerchandisingModel;

namespace SCERP.BLL.Manager.MerchandisingManager
{
    public class BuyOrdShipManager : IBuyOrdShipManager
    {
        private readonly IBuyOrdShipDetailRepository _ordShipDetailRepository;
        private readonly IBuyOrdShipRepository _buyOrdShipRepository;
        private readonly IOmBuyOrdStyleRepository _ordStyleRepository;
        private readonly IBuyerOrderRepository _buyerOrderRepository;
        private readonly string _compId;
        private IRepository<VwOM_BuyOrdShip> _buyerOrderShipRepoistory;
        public BuyOrdShipManager(IBuyerOrderRepository buyerOrderRepository, IBuyOrdShipRepository buyOrdShipRepository, IBuyOrdShipDetailRepository ordShipDetailRepository, IOmBuyOrdStyleRepository ordStyleRepository, IRepository<VwOM_BuyOrdShip> buyerOrderShipRepoistory)
        {
            _buyerOrderRepository = buyerOrderRepository;
            _ordStyleRepository = ordStyleRepository;
            _buyerOrderShipRepoistory = buyerOrderShipRepoistory;
            _ordShipDetailRepository = ordShipDetailRepository;
            _compId = PortalContext.CurrentUser.CompId;
            _buyOrdShipRepository = buyOrdShipRepository;
        }

        public List<VwBuyOrdShip> GetBuyOrdShipPaging(string orderStyleRefId, out int totalRecords)
        {

            var buyerOrderShips = _buyOrdShipRepository.Filter(x => x.CompId == _compId && x.OrderStyleRefId == orderStyleRefId);
            IQueryable<VwBuyOrdShip> qabls = _buyOrdShipRepository.GetBuyOrdShips(orderStyleRefId,_compId);
            totalRecords = qabls.Count();
            return qabls.ToList();
        }

        public int DeleteBuyOrdShip(string orderShipRefId)
        {
            var deleteStatus = 0;
            var isProcessed = _ordShipDetailRepository.Filter(x => x.OrderShipRefId == orderShipRefId && x.CompId == _compId).ToList().Sum(x => x.Quantity);
            if (isProcessed > 0)
            {
                deleteStatus = -1;
            }
            else
            {
                deleteStatus = _buyOrdShipRepository.Delete(x => x.OrderShipRefId == orderShipRefId && x.CompId == _compId);
                deleteStatus += _ordShipDetailRepository.Delete(x => x.OrderShipRefId == orderShipRefId && x.CompId == _compId);
            }
            return deleteStatus;
        }
        public int EditBuyOrdShip(OM_BuyOrdShip model)
        {
            int effectedRows = 0;
            using (var transaction = new TransactionScope())
            {
                var orderShip = _buyOrdShipRepository.FindOne(x => x.OrderShipId == model.OrderShipId);
                orderShip.OrderShipId = model.OrderShipId;
                orderShip.OrderNo = model.OrderNo;
                orderShip.OrderShipRefId = model.OrderShipRefId;
                orderShip.OrderStyleRefId = model.OrderStyleRefId;
                orderShip.LotNo = model.LotNo;
                orderShip.ShipDate = model.ShipDate;
                orderShip.CountryId = model.CountryId;
                orderShip.PortOfLoadingRefId = model.PortOfLoadingRefId;
                orderShip.Quantity = model.Quantity;
                orderShip.ProductionQty = model.Quantity;
                orderShip.DespatchQty = model.DespatchQty;
                orderShip.ETD = model.ETD;
                orderShip.Remarks = model.Remarks;
                effectedRows += _buyOrdShipRepository.Edit(orderShip);
                effectedRows += UpdateExFactoryDate(orderShip.OrderStyleRefId);
                transaction.Complete();
            }

            return effectedRows;
        }

        public int SaveBuyOrdShip(OM_BuyOrdShip model)
        {
            int effectedRows = 0;
            using (var transaction = new TransactionScope())
            {
                model.CompId = _compId;
                model.OrderShipRefId = _buyOrdShipRepository.GetNewOrderShipRefId(_compId);
                model.ShipDate = model.ETD;
                effectedRows += _buyOrdShipRepository.Save(model);
                effectedRows += UpdateExFactoryDate(model.OrderStyleRefId);
                transaction.Complete();
            }

            return effectedRows;
        }

        private int UpdateExFactoryDate(string orderStyleRefId)
        {
            DateTime? maxShipDate = _buyOrdShipRepository.Filter(x => x.CompId == _compId && x.OrderStyleRefId == orderStyleRefId).Max(x => x.ETD);
            DateTime? etdDate = _buyOrdShipRepository.Filter(x => x.CompId == _compId && x.OrderStyleRefId == orderStyleRefId).Max(x => x.ShipDate);
            var dispatchQty = _buyOrdShipRepository.Filter(x => x.CompId == _compId && x.OrderStyleRefId == orderStyleRefId).Sum(x => x.DespatchQty);
            OM_BuyOrdStyle ordStyle = _ordStyleRepository.FindOne(x => x.CompId == _compId && x.OrderStyleRefId == orderStyleRefId);
            ordStyle.EFD = maxShipDate;
            ordStyle.PSD = etdDate;
            ordStyle.despatchQty = dispatchQty;
            return _ordStyleRepository.Edit(ordStyle);
        }

        public string GetNewOrderShipRefId()
        {

            return _buyOrdShipRepository.GetNewOrderShipRefId(_compId);
        }

        public OM_BuyOrdShip GetBuyerById(int orderShipId)
        {
            return _buyOrdShipRepository.FindOne(x => x.OrderShipId == orderShipId && x.CompId == _compId);
        }

        public DataTable UpdateTempAssort(string orderStyleRefId)
        {
            var tAssortTable = new DataTable();
            try
            {
                using (var transaction = new TransactionScope())
                {
                    var orderShip = new OM_BuyOrdShip()
                    {
                        OrderStyleRefId = orderStyleRefId,
                        CompId = _compId
                    };
                    var ordStyleSizeView = _buyOrdShipRepository.GetBuyOrdStyleSize(orderShip);
                    if (ordStyleSizeView.Any())
                    {
                        tAssortTable = _buyOrdShipRepository.UpdateTempAssort(orderShip) ?? new DataTable();
                        foreach (var size in ordStyleSizeView)
                        {
                            tAssortTable.Columns["C" + size.SizeRow].ColumnName = size.SizeName.Replace(" ", String.Empty);
                        }
                        var tablelength = ordStyleSizeView.Count;
                        var numOfCols = tAssortTable.Columns.Count - 6;
                        for (var i = tablelength + 1; i < numOfCols; i++)
                        {
                            tAssortTable.Columns.Remove("C" + i);
                        }
                        tAssortTable.Columns.Remove("TempAssortId");
                        tAssortTable.Columns.Remove("CompId");
                        tAssortTable.Columns.Remove("ColorRow");
                        tAssortTable.Columns.Remove("UserId");
                        tAssortTable.Columns.Remove("ColorRefId");
                        var toInsert = tAssortTable.NewRow();
                        toInsert[0] = "Total:";
                        for (var i = 1; i < tAssortTable.Columns.Count; i++)
                        {
                            toInsert[i] = tAssortTable.Compute("sum([" + tAssortTable.Columns[i].ColumnName + "])", "").ToString();
                        }
                        tAssortTable.Rows.InsertAt(toInsert, tAssortTable.Rows.Count + 1);
                    }

                    transaction.Complete();
                }
            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message);
            }

            return tAssortTable;
        }


        public DataTable GetShipAssort(string orderShipRefId, string orderStyleRefId)
        {
            try
            {
                DataTable tAssortTable;
                using (var transaction = new TransactionScope())
                {
                    var orderShip = new OM_BuyOrdShip()
                    {
                        OrderStyleRefId = orderStyleRefId,
                        OrderShipRefId = orderShipRefId,
                        CompId = _compId
                    };

                    tAssortTable = _buyOrdShipRepository.UpdateTempAssort(orderShip);

                    var ordStyleSizeView = _buyOrdShipRepository.GetBuyOrdStyleSize(orderShip);
                    foreach (var size in ordStyleSizeView)
                    {
                        tAssortTable.Columns["C" + size.SizeRow].ColumnName = size.SizeName.Replace(" ", String.Empty);
                    }
                    var tablelength = ordStyleSizeView.Count;
                    var numOfCols = tAssortTable.Columns.Count - 6;
                    for (var i = tablelength + 1; i < numOfCols; i++)
                    {
                        tAssortTable.Columns.Remove("C" + i);
                    }
                    tAssortTable.Columns.Remove("TempAssortId");
                    tAssortTable.Columns.Remove("CompId");
                    tAssortTable.Columns.Remove("ColorRow");
                    tAssortTable.Columns.Remove("UserId");
                    tAssortTable.Columns.Remove("ColorRefId");
                    var toInsert = tAssortTable.NewRow();
                    toInsert[0] = "Total:";
                    for (var i = 1; i < tAssortTable.Columns.Count; i++)
                    {
                        toInsert[i] = tAssortTable.Compute("sum([" + tAssortTable.Columns[i].ColumnName + "])", "").ToString();
                    }
                    tAssortTable.Rows.InsertAt(toInsert, tAssortTable.Rows.Count + 1);
                    transaction.Complete();
                }
                return tAssortTable;
            }
            catch (Exception exception)
            {
           
                throw  new Exception("Style Wise size and Color not entry properly!");
            }
        
        }

        public string GetNewLotNo(string orderStyleRefId)
        {
            return _buyOrdShipRepository.GetNewLotNo(_compId, orderStyleRefId);
        }

        public bool CheckShipGreaterQty(string orderStyleRefId, decimal qty)
        {
            var styleQty = _ordStyleRepository.FindOne(x => x.OrderStyleRefId == orderStyleRefId && x.CompId == _compId).Quantity.GetValueOrDefault();
            //var totalShipQty = _buyOrdShipRepository.Filter(x => x.OrderStyleRefId == orderStyleRefId && x.CompId == _compId).ToList().Sum(x => x.Quantity.GetValueOrDefault()); //Commented by Golam Rabbi, 21.05.2016
            //return (styleQty >= totalShipQty + qty); //Commented by Golam Rabbi, 21.05.2016
            return (styleQty >= qty); //Added by Golam Rabbi, 21.05.2016
        }

        public int SaveShipmentOfOder(OM_BuyOrdShip buyOrdShip)
        {
            var updateIndex = 0;
            using (var transaction = new TransactionScope())
            {

                var orderShip = _buyOrdShipRepository.FindOne(x => x.OrderShipId == buyOrdShip.OrderShipId && x.CompId == _compId);
                orderShip.DespatchQty = buyOrdShip.DespatchQty;
                orderShip.DeliveryDate = buyOrdShip.DeliveryDate;
                orderShip.CINo = buyOrdShip.CINo;
                decimal? totaldespatchQty =
                    _buyOrdShipRepository.Filter(x => x.OrderStyleRefId == orderShip.OrderStyleRefId && x.CompId == _compId).Sum(x => x.DespatchQty);
                var orderStyle = _ordStyleRepository.FindOne(x => x.OrderStyleRefId == orderShip.OrderStyleRefId && x.CompId == _compId);
                orderStyle.despatchQty = totaldespatchQty;
                updateIndex += _ordStyleRepository.Edit(orderStyle);
                updateIndex += _buyOrdShipRepository.Edit(orderShip);
                var shipDetails =
                    _ordShipDetailRepository.Filter(
                        x => x.OrderShipRefId == orderShip.OrderShipRefId && x.CompId == _compId);
                foreach (var shipDetail in shipDetails)
                {
                    if (buyOrdShip.DespatchQty.HasValue && buyOrdShip.DespatchQty > 0)
                    {
                        shipDetail.ShQty = shipDetail.Quantity;
                    }
                    else
                    {
                        shipDetail.ShQty = 0;
                    }

                    updateIndex += _ordShipDetailRepository.Edit(shipDetail);
                }
                transaction.Complete();

            }
            return updateIndex;
        }

        public DataTable GetTotalShipAssort(string orderShipRefId, string orderStyleRefId)
        {
            DataTable tAssortTable;
            using (var transaction = new TransactionScope())
            {
                var orderShip = new OM_BuyOrdShip()
                {
                    OrderStyleRefId = orderStyleRefId,
                    OrderShipRefId = orderShipRefId,
                    CompId = _compId
                };

                tAssortTable = _buyOrdShipRepository.UpdatedShipmentTotalTempAssort(orderShip);

                var ordStyleSizeView = _buyOrdShipRepository.GetBuyOrdStyleSize(orderShip);
                foreach (var size in ordStyleSizeView)
                {
                    tAssortTable.Columns["C" + size.SizeRow].ColumnName = size.SizeName.Replace(" ", String.Empty);
                }
                var tablelength = ordStyleSizeView.Count;
                var numOfCols = tAssortTable.Columns.Count - 6;
                for (var i = tablelength + 1; i < numOfCols; i++)
                {
                    tAssortTable.Columns.Remove("C" + i);
                }
                tAssortTable.Columns.Remove("TempAssortId");
                tAssortTable.Columns.Remove("CompId");
                tAssortTable.Columns.Remove("ColorRow");
                tAssortTable.Columns.Remove("UserId");
                tAssortTable.Columns.Remove("ColorRefId");
                var toInsert = tAssortTable.NewRow();
                toInsert[0] = "Total:";
                for (var i = 1; i < tAssortTable.Columns.Count; i++)
                {
                    toInsert[i] = tAssortTable.Compute("sum([" + tAssortTable.Columns[i].ColumnName + "])", "").ToString();
                }
                tAssortTable.Rows.InsertAt(toInsert, tAssortTable.Rows.Count + 1);
                transaction.Complete();
            }
            return tAssortTable;
        }


        public OrderSheet GetOrderSheetDetail(string orderNo)
        {
            var orderSheet = new OrderSheet
            {
                VBuyerOrder =
                    _buyerOrderRepository.GetBuyerOrderViews(x => x.CompId == _compId)
                        .FirstOrDefault(x => x.OrderNo == orderNo) ?? new VBuyerOrder()
            };

            if (orderSheet.VBuyerOrder != null)
            {
                var vomBuyOrdStyles =
                    _ordStyleRepository.GetBuyerOrderStyle(
                        x => x.CompId == _compId && x.OrderNo == orderSheet.VBuyerOrder.OrderNo).ToList() ??
                    new List<VOMBuyOrdStyle>();
                foreach (var ordStyle in vomBuyOrdStyles)
                {
                    var styleShipList =
                        _buyerOrderShipRepoistory.Filter(
                            x => x.OrderStyleRefId == ordStyle.OrderStyleRefId & x.CompId == ordStyle.CompId);
                    var orderStyle = new OrderStyle();
                    orderStyle.BuyOrdStyle = ordStyle;
                    foreach (var ship in styleShipList)
                    {

                        orderStyle.Shipments.Add(new Shipment()
                        {
                            OrdShip = ship,
                            ShipTable = GetShipAssort(ship.OrderShipRefId, ordStyle.OrderStyleRefId)
                        });
                    }

                    orderSheet.OrderStyles.Add(ordStyle.OrderStyleRefId, orderStyle);
                }
            }

            return orderSheet;
        }

        public IEnumerable GetStyleWiseShipment(string orderStyleRefId, string compId)
        {

            var shipments = _buyerOrderShipRepoistory.Filter(x => x.CompId == compId && x.OrderStyleRefId == orderStyleRefId).OrderBy(x=>x.ShipDate).ToList()
                    .Select(x => new 
                    {
                      x.OrderShipRefId,
                      CountryName= x.CountryName+" Ship Date :"+String.Format("{0:dd/MM/yyyy}",x.ShipDate)
                    }).ToList();
            return shipments;
        }

        public Dictionary<VwBuyOrdShip, DataTable> GetBuyOrdShipByeStyle(string orderStyleRefId)
        {
            IQueryable<VwBuyOrdShip> qabls = _buyOrdShipRepository.GetBuyOrdShips(orderStyleRefId, _compId);
            Dictionary<VwBuyOrdShip, DataTable> keyValuePairs = new Dictionary<VwBuyOrdShip, DataTable>();
            foreach (var item in qabls)
            {
                DataTable table = GetShipAssort(item.OrderShipRefId, orderStyleRefId);
                keyValuePairs.Add(item, table);
            }
            return keyValuePairs;
        }
    }
}

