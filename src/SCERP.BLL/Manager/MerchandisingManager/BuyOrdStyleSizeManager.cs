using System.Collections.Generic;
using System.Linq;
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
    public class BuyOrdStyleSizeManager : IBuyOrdStyleSizeManager
    {
        private readonly IBuyOrdStyleSizeRepository _ordStyleSizeRepository;
        private readonly IBuyOrdStyleColorRepository _ordStyleColorRepository;
        private readonly string _compId;
        public BuyOrdStyleSizeManager(IBuyOrdStyleSizeRepository ordStyleSizeRepository, IBuyOrdStyleColorRepository ordStyleColorRepository)
        {
            _compId = PortalContext.CurrentUser.CompId;

            _ordStyleSizeRepository = ordStyleSizeRepository;
            _ordStyleColorRepository = ordStyleColorRepository;
        }

        public List<VBuyOrdStyleSize> GetBuyOrdStyleSize(string orderStyleRefId)
        {


            return _ordStyleSizeRepository.GetBuyOrdStyleSize(orderStyleRefId, _compId);
        }

        public int SaveBuyOrdStyleSize(OM_BuyOrdStyleSize ordSize)
        {
            var saveIndex = 0;
            bool exist =
                _ordStyleSizeRepository.Exists(x => x.OrderStyleRefId == ordSize.OrderStyleRefId&&x.SizeRefId==ordSize.SizeRefId && x.CompId == _compId);
            if (!exist)
            {
                    ordSize.CompId = _compId;
            ordSize.SizeRow =
                (_ordStyleSizeRepository.Filter(x => x.OrderStyleRefId == ordSize.OrderStyleRefId&&x.CompId==_compId).Max(x => x.SizeRow) ?? 0) + 1;
             saveIndex = _ordStyleSizeRepository.Save(ordSize);
            }
            else
            {
                var ostyleSize = _ordStyleSizeRepository.FindOne(x => x.OrderStyleRefId == ordSize.OrderStyleRefId && x.SizeRefId == ordSize.SizeRefId && x.CompId == _compId);
            ostyleSize.SizeRefId = ordSize.SizeRefId;
               saveIndex=_ordStyleSizeRepository.Edit(ostyleSize);
            }
        
            return saveIndex;
        }

        public int DelteBuyOrdStyleSize(OM_BuyOrdStyleSize size)
        {
            var updateIndex = 0;
            using (var transaction = new TransactionScope())
            {
                var orderStlieSizeList =
                    _ordStyleSizeRepository.Filter(
                        x => x.OrderStyleRefId == size.OrderStyleRefId && x.CompId == _compId && x.OrderStyleSizeId != size.OrderStyleSizeId)
                        .OrderBy(x => x.SizeRow)
                        .ToList();
                _ordStyleSizeRepository.Delete(x => x.OrderStyleRefId == size.OrderStyleRefId && x.CompId == _compId);
                foreach (var omBuyOrdStyleSize in orderStlieSizeList)
                {
                    omBuyOrdStyleSize.OrderStyleSizeId = 0;
                    updateIndex += SaveBuyOrdStyleSize(omBuyOrdStyleSize);
                }
                transaction.Complete();
            }
            return updateIndex;
        }
        public int DelteBuyOrdStyleColor(OM_BuyOrdStyleColor color)
        {
            var updateIndex = 0;

            using (var transaction = new TransactionScope())
            {
                var orderStlieColorList =
           _ordStyleColorRepository.Filter(x => x.OrderStyleRefId == color.OrderStyleRefId && x.CompId == _compId && x.OrderStyleColorId != color.OrderStyleColorId).OrderBy(x => x.ColorRow).ToList();
                var index = _ordStyleColorRepository.Delete(x => x.OrderStyleRefId == color.OrderStyleRefId && x.CompId == _compId);
                foreach (var omBuyOrdStyleColor in orderStlieColorList)
                {
                    omBuyOrdStyleColor.OrderStyleColorId = 0;
                    updateIndex += SaveBuyOrdStyleColor(omBuyOrdStyleColor);
                }
                transaction.Complete();
            }
            return updateIndex;
        }
        public int EditBuyOrdStyleSize(OM_BuyOrdStyleSize size)
        {
            //var ostyleSize = _ordStyleSizeRepository.FindOne(x => x.OrderStyleSizeId == size.OrderStyleSizeId && x.CompId == _compId);
            //ostyleSize.SizeRefId = size.SizeRefId;
            return _ordStyleSizeRepository.UpdateBuyOrdStyleSize(size.OrderStyleSizeId, size.SizeRefId);
        }

        public int EditBuyOrdStyleColor(OM_BuyOrdStyleColor color)
        {
            //var ostyleColor = _ordStyleColorRepository.FindOne(x => x.OrderStyleColorId == color.OrderStyleColorId && x.CompId == _compId);
            //ostyleColor.ColorRefId = color.ColorRefId;
            //return _ordStyleColorRepository.Edit(ostyleColor);
        
            return _ordStyleColorRepository.UpdateBuyOrdStyleColor(color.OrderStyleColorId, color.ColorRefId);
        }

        public int SaveBuyOrdStyleColor(OM_BuyOrdStyleColor color)
        {
            var saveIndex = 0;
            var exist =
                _ordStyleColorRepository.Exists(
                    x => x.OrderStyleRefId == color.OrderStyleRefId && x.ColorRefId == color.ColorRefId && x.CompId == _compId);
            if (!exist)
            {
                color.CompId = _compId;
                color.ColorRow =
                    (_ordStyleColorRepository.Filter(x => x.OrderStyleRefId == color.OrderStyleRefId && x.CompId == _compId).Max(x => x.ColorRow) ?? 0) + 1;
                saveIndex = _ordStyleColorRepository.Save(color);
            }
            else
            {
                var ostyleColor = _ordStyleColorRepository.FindOne(x => x.OrderStyleRefId == color.OrderStyleRefId && x.ColorRefId == color.ColorRefId && x.CompId == _compId);
                ostyleColor.ColorRefId = color.ColorRefId;
                saveIndex = _ordStyleColorRepository.Edit(ostyleColor);
            }

            return saveIndex;
        }

        public List<VBuyOrdStyleColor> GetBuyOrdStyleColor(string orderStyleRefId)
        {
            return _ordStyleColorRepository.GetBuyOrdStyleColor(orderStyleRefId, _compId);
        }

        public VBuyOrdStyleSize GetBuyOrdStyleSizeById(long id)
        {
            return _ordStyleColorRepository.GetBuyOrdStyleSizeById(id);
        }
    }
}
