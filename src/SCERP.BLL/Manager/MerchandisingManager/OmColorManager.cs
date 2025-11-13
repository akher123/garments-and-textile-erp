using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.Common;
using SCERP.DAL.IRepository.IMerchandisingRepository;
using SCERP.Model;
using SCERP.Model.InventoryModel;

namespace SCERP.BLL.Manager.MerchandisingManager
{
    public class OmColorManager : IOmColorManager
    {
        private readonly IOmColorRepository _colorRepository;
        private readonly string _compId;
        private readonly IBuyOrdStyleColorRepository _buyOrdStyleColorRepository;
        public OmColorManager(IOmColorRepository colorRepository, IBuyOrdStyleColorRepository buyOrdStyleColorRepository)
        {
            _compId = PortalContext.CurrentUser.CompId;
            _colorRepository = colorRepository;
            _buyOrdStyleColorRepository = buyOrdStyleColorRepository;
        }
        public object ColorAutoComplite(string serachString,string typeId)
        {
            return _colorRepository.Filter(x => x.CompId == _compId && (x.TypeId == typeId || String.IsNullOrEmpty(typeId)) &&  x.ColorName.Trim().Replace(" ", String.Empty).ToLower().Contains(serachString.Trim().Replace(" ", String.Empty).ToLower())).Take(10);
        }

        public List<OM_Color> GetOmColorByPaging(OM_Color model, out int totalRecords)
        {
            var index = model.PageIndex;
            var pageSize = AppConfig.PageSize;
            var colorList = _colorRepository.Filter(x => x.CompId == _compId&&x.TypeId==model.TypeId && ((x.ColorName.Trim().Replace(" ", string.Empty).ToLower().Contains(model.SearchString.Trim().Replace(" ", string.Empty).ToLower()) || String.IsNullOrEmpty(model.SearchString))
                || (x.ColorRefId.Trim().Replace(" ", string.Empty).ToLower().Contains(model.SearchString.Trim().Replace(" ", string.Empty).ToLower()) || String.IsNullOrEmpty(model.SearchString))
                || (x.ColorCode.Trim().Replace(" ", string.Empty).ToLower().Contains(model.SearchString.Trim().Replace(" ", string.Empty).ToLower()) || String.IsNullOrEmpty(model.SearchString))));
            totalRecords = colorList.Count();
            switch (model.sort)
            {
                case "ColorName":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            colorList = colorList
                                .OrderByDescending(r => r.ColorName)
                                .Skip(index * pageSize)
                                .Take(pageSize);

                            break;
                        default:
                            colorList = colorList
                                .OrderBy(r => r.ColorName)
                                .Skip(index * pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;
                case "ColorRefId":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            colorList = colorList
                                .OrderByDescending(r => r.ColorRefId)
                                .Skip(index * pageSize)
                                .Take(pageSize);

                            break;
                        default:
                            colorList = colorList
                                .OrderBy(r => r.ColorRefId)
                                .Skip(index * pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;
                default:
                    colorList = colorList
                        .OrderByDescending(r => r.ColorRefId)
                        .Skip(index * pageSize)
                        .Take(pageSize);
                    break;
            }
            return colorList.ToList();
        }

        public OM_Color GetOmColorById(int colorId)
        {
            return _colorRepository.FindOne(x => x.ColorId == colorId && x.CompId == _compId);
        }

        public string GetNewOmColorRefId()
        {
            return _colorRepository.GetNewOmColorRefId(_compId);
        }

        public int EditOmColor(OM_Color model)
        {
            var color = _colorRepository.FindOne(x => x.ColorId == model.ColorId && x.CompId == _compId);
            color.ColorName = model.ColorName;
            color.ColorCode = model.ColorCode;
            color.TypeId = model.TypeId;
            return _colorRepository.Edit(color);
        }

        public int DeleteOmColor(string colorRefId)
        {
            bool isUsesd = _buyOrdStyleColorRepository.Exists(x => x.ColorRefId == colorRefId && x.CompId == _compId);
            var deleted = 0;
            if (isUsesd)
            {
                deleted = -1;
            }
            else
            {
                deleted = _colorRepository.Delete(x => x.ColorRefId == colorRefId && x.CompId == _compId);
            }
            return deleted;
        }

        public int SaveOmColor(OM_Color model)
        {
            bool isExist=   _colorRepository.Exists(x => x.ColorName.Trim().Replace(" ", "").ToLower() == model.ColorName.Trim().Replace(" ", "").ToLower()&&x.TypeId==model.TypeId);
            if (!isExist)
            {
                model.CompId = _compId;
                model.ColorRefId = _colorRepository.GetNewOmColorRefId(_compId);
                return _colorRepository.Save(model);
            }
            else
            {
                throw new Exception(model.ColorName + " Color name already Exist");
            }
          
        }
        public bool CheckExistingColor(OM_Color color)
        {
          return  _colorRepository.Exists(x => x.ColorId != color.ColorId && x.CompId == _compId && x.ColorName.ToLower() == color.ColorName.ToLower());
        }

        public List<OM_Color> GetOmColors()
        {
            return _colorRepository.Filter(x => x.CompId == _compId).ToList();
        }

        public List<VwLot> GetLotByPaging(OM_Color model, out int totalRecords)
        {
      
            var index = model.PageIndex;
            var pageSize = AppConfig.PageSize;
            IQueryable<VwLot> colorList = _colorRepository.GetLots(PortalContext.CurrentUser.CompId, model.TypeId);
            colorList = colorList.Where(x => ((x.ColorName.Trim().Replace(" ", string.Empty).ToLower().Contains(model.SearchString.Trim().Replace(" ", string.Empty).ToLower()) || String.IsNullOrEmpty(model.SearchString))
                || (x.ColorRefId.Trim().Replace(" ", string.Empty).ToLower().Contains(model.SearchString.Trim().Replace(" ", string.Empty).ToLower()) || String.IsNullOrEmpty(model.SearchString))
                || (x.ColorCode.Trim().Replace(" ", string.Empty).ToLower().Contains(model.SearchString.Trim().Replace(" ", string.Empty).ToLower()) || String.IsNullOrEmpty(model.SearchString))));
            totalRecords = colorList.Count();
            switch (model.sort)
            {
                case "ColorName":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            colorList = colorList
                                .OrderByDescending(r => r.ColorName)
                                .Skip(index * pageSize)
                                .Take(pageSize);

                            break;
                        default:
                            colorList = colorList
                                .OrderBy(r => r.ColorName)
                                .Skip(index * pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;
                case "ColorRefId":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            colorList = colorList
                                .OrderByDescending(r => r.ColorRefId)
                                .Skip(index * pageSize)
                                .Take(pageSize);

                            break;
                        default:
                            colorList = colorList
                                .OrderBy(r => r.ColorRefId)
                                .Skip(index * pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;
                default:
                    colorList = colorList
                        .OrderByDescending(r => r.ColorRefId)
                        .Skip(index * pageSize)
                        .Take(pageSize);
                    break;
            }
            return colorList.ToList();
        }

        public object LotAutocomplite(string serachString, string typeId)
        {
            IQueryable<VwLot> lotList = _colorRepository.GetLots(PortalContext.CurrentUser.CompId, typeId);
            lotList = lotList.Where(x => x.ColorName.ToLower().Contains(serachString.ToLower()));
            return lotList;
        }



        public object AutoCompleteColor(string searchString)
        {
            return _colorRepository.Filter(x => x.ColorName.Trim().Replace(" ", String.Empty).StartsWith(searchString.Replace(" ", String.Empty))).Take(10).OrderBy(x => x.ColorName);
        }

        public int SaveLot(OM_Color lot)
        {
            bool isExist = _colorRepository.Exists(x => x.ColorName.Trim().Replace(" ", "").ToLower() == lot.ColorName.Trim().Replace(" ", "").ToLower() && x.TypeId == lot.TypeId && x.ColorCode==lot.ColorCode);
            if (!isExist)
            {
                lot.CompId = _compId;
                lot.ColorRefId = _colorRepository.GetNewOmColorRefId(_compId);
                return _colorRepository.Save(lot);
            }
            else
            {
                throw new Exception(lot.ColorName + "  lot number already Exist for this brand");
            }
        }

        public int EditLot(OM_Color lot)
        {
            bool isExist = _colorRepository.Exists(x=> x.ColorId != lot.ColorId&& x.ColorName.Trim().Replace(" ", "").ToLower() == lot.ColorName.Trim().Replace(" ", "").ToLower() && x.TypeId == lot.TypeId && x.ColorCode == lot.ColorCode);
            if (!isExist)
            {
                var color = _colorRepository.FindOne(x => x.ColorId == lot.ColorId && x.CompId == _compId);
                color.ColorName = lot.ColorName;
                color.ColorCode = lot.ColorCode;
                color.TypeId = lot.TypeId;
                return _colorRepository.Edit(color);
            }
            else
            {
                throw new Exception(lot.ColorName + "  lot number already Exist");
            }
          
        }

        public object GetLotDetails(string lotId)
        {
            string sql = @"select 
                FColorRefId,
                SizeRefId,
                ItemId,
                (select ItemCode from Inventory_Item where ItemId=P.ItemId) as ItemCode,
                (select ColorName from OM_Color where ColorRefId=P.FColorRefId) as FColorName,
                (select SizeName  from OM_Size where SizeRefId=P.SizeRefId) as SizeName,
                (select ItemName from Inventory_Item where ItemId=P.ItemId) as ItemName,
                (select SUM(TR.Quantity)-ISNULL((
				 select SUM(Inventory_StockRegister.Quantity) from Inventory_StockRegister where Inventory_StockRegister.ColorRefId=TR.ColorRefId and Inventory_StockRegister.TransactionType=2),0)
				 from Inventory_StockRegister AS TR where TR.ColorRefId='{0}' and TR.TransactionType=1
				 group by TR.ColorRefId) AS SQty
			    from  (select distinct top 1 ItemId,FColorRefId,SizeRefId from Inventory_MaterialReceiveAgainstPoDetail as MR where ColorRefId='{0}') as P";
            DataTable table= _colorRepository.ExecuteQuery(string.Format(sql, lotId));
            return table.Todynamic().FirstOrDefault();

        }
    }
}
