using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.Common;
using SCERP.DAL;
using SCERP.DAL.IRepository.IMerchandisingRepository;
using SCERP.DAL.Repository.MerchandisingRepository;
using SCERP.Model;
using SCERP.Model.MerchandisingModel;

namespace SCERP.BLL.Manager.MerchandisingManager
{
    public class BuyOrdShipDetailManager : IBuyOrdShipDetailManager
    {
        private readonly IBuyOrdShipDetailRepository _buyOrdShipDetailRepository;
        private readonly IOmBuyOrdStyleRepository _buyOrdStyleRepository;
        private readonly IBuyOrdShipRepository _buyOrdShipRepository;
        private readonly IBuyOrdStyleColorRepository _styleColorRepository;
        private readonly IBuyOrdStyleSizeRepository _styleSizeRepository;
        private readonly string _compId;
        public BuyOrdShipDetailManager(IBuyOrdShipRepository buyOrdShipRepository, IOmBuyOrdStyleRepository buyOrdStyleRepository, IBuyOrdShipDetailRepository buyOrdShipDetailRepository, IBuyOrdStyleColorRepository styleColorRepository, IBuyOrdStyleSizeRepository styleSizeRepository)
        {
            _buyOrdStyleRepository = buyOrdStyleRepository;
            _buyOrdShipRepository = buyOrdShipRepository;
            _styleSizeRepository = styleSizeRepository;
            _styleColorRepository = styleColorRepository;
            _compId = PortalContext.CurrentUser.CompId;
            _buyOrdShipDetailRepository = buyOrdShipDetailRepository;
        }
        public int SaveBuyOrdShipDetail(OM_BuyOrdShipDetail model, string orderStyleRefId)
        {
            model.QuantityP = model.Quantity;

            int saveIndex = 0;
           var styleColor=  _styleColorRepository.FindOne(
                x => x.ColorRefId == model.ColorRefId && x.OrderStyleRefId == orderStyleRefId && x.CompId == _compId);
           var styleSize= _styleSizeRepository.FindOne(
                x => x.SizeRefId == model.SizeRefId && x.OrderStyleRefId == orderStyleRefId && x.CompId == _compId);
            model.SizeRow = styleSize.SizeRow;
            model.ColorRow = styleColor.ColorRow;
            model.OrderShipRefId = model.OrderShipRefId.Replace(" ", String.Empty);
            model.CompId = _compId;
            bool exist =
                _buyOrdShipDetailRepository.Exists(
                    x =>
                        x.ColorRefId == model.ColorRefId && x.SizeRefId == model.SizeRefId &&
                        x.ColorRow == model.ColorRow && x.SizeRow == model.SizeRow &&
                        x.OrderShipRefId == model.OrderShipRefId&&x.CompId==_compId);
            if (!exist)
            {
                 saveIndex= _buyOrdShipDetailRepository.Save(model);
            }
            else
            {
               var shipdetail= _buyOrdShipDetailRepository.FindOne(
                    x =>
                        x.ColorRefId == model.ColorRefId && x.SizeRefId == model.SizeRefId &&
                        x.ColorRow == model.ColorRow && x.SizeRow == model.SizeRow &&
                        x.OrderShipRefId == model.OrderShipRefId&&x.CompId==_compId);
                shipdetail.Quantity = model.Quantity;
                shipdetail.QuantityP = model.Quantity;
                saveIndex = _buyOrdShipDetailRepository.Edit(shipdetail);

            }
            return saveIndex;
        }

        public int UpdateBuyOrdShipDetail(OM_BuyOrdShipDetail model, string orderStyleRefId)
        {
            using (var transaction = new TransactionScope())
            {

                int updateIndex = 0;
                model.QuantityP = model.Quantity;


                var styleColor = _styleColorRepository.FindOne(
                    x => x.ColorRefId == model.ColorRefId && x.OrderStyleRefId == orderStyleRefId && x.CompId == _compId);
                var styleSize = _styleSizeRepository.FindOne(
                    x => x.SizeRefId == model.SizeRefId && x.OrderStyleRefId == orderStyleRefId && x.CompId == _compId);
                model.SizeRow = styleSize.SizeRow;
                model.ColorRow = styleColor.ColorRow;
                model.OrderShipRefId = model.OrderShipRefId.Replace(" ", String.Empty);
                model.CompId = _compId;
                bool exist =
                    _buyOrdShipDetailRepository.Exists(
                        x =>
                            x.ColorRefId == model.ColorRefId && x.SizeRefId == model.SizeRefId &&
                            x.ColorRow == model.ColorRow && x.SizeRow == model.SizeRow &&
                            x.OrderShipRefId == model.OrderShipRefId && x.CompId == _compId);
                if (!exist)
                {
                    updateIndex += _buyOrdShipDetailRepository.Save(model);
                }
                else
                {
                    var shipdetail = _buyOrdShipDetailRepository.FindOne(
                        x =>
                            x.ColorRefId == model.ColorRefId && x.SizeRefId == model.SizeRefId &&
                            x.ColorRow == model.ColorRow && x.SizeRow == model.SizeRow &&
                            x.OrderShipRefId == model.OrderShipRefId && x.CompId == _compId);
                    shipdetail.ShQty = model.Quantity;
                    updateIndex += _buyOrdShipDetailRepository.Edit(shipdetail);

                    var totalshQty = _buyOrdShipDetailRepository.Filter(
                        x => x.CompId == _compId && x.OrderShipRefId == model.OrderShipRefId)
                        .Sum(x => x.ShQty);

                    var orderShip =
                        _buyOrdShipRepository.FindOne(
                            x => x.CompId == _compId && x.OrderShipRefId == model.OrderShipRefId);
                    orderShip.DespatchQty = totalshQty;
                    updateIndex += _buyOrdShipRepository.Edit(orderShip);
                    var totalDespQty =
                        _buyOrdStyleRepository.FindOne(x => x.CompId == _compId && x.OrderStyleRefId == orderStyleRefId);
                    totalDespQty.despatchQty =
                        _buyOrdShipRepository.Filter(x => x.CompId == _compId && x.OrderStyleRefId == orderStyleRefId)
                            .Sum(x => x.DespatchQty);
                    updateIndex += _buyOrdStyleRepository.Edit(totalDespQty);
                    transaction.Complete();

                }
                return updateIndex;

            }
        }

      
    }
}
