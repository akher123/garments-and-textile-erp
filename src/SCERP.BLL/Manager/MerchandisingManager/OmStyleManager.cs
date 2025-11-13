using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.Common;
using SCERP.DAL;
using SCERP.DAL.IRepository.IInventoryRepository;
using SCERP.DAL.IRepository.IMerchandisingRepository;
using SCERP.DAL.Repository.InventoryRepository;
using SCERP.DAL.Repository.MerchandisingRepository;
using SCERP.Model;
using SCERP.Model.MerchandisingModel;

namespace SCERP.BLL.Manager.MerchandisingManager
{
    public class OmStyleManager : IOmStyleManager
    {
        private readonly IInventoryItemRepository _itemRepository;
        private readonly IOmStyleRepository _styleRepository;
        private readonly IOmBuyOrdStyleRepository _buyOrdStyleRepository;
        private readonly string _compId;
        public OmStyleManager(IInventoryItemRepository itemRepository, IOmStyleRepository styleRepository, IOmBuyOrdStyleRepository buyOrdStyleRepository)
        {
            _buyOrdStyleRepository = buyOrdStyleRepository;
            _compId = PortalContext.CurrentUser.CompId;
            _styleRepository = styleRepository;
            _itemRepository = itemRepository;
        }

        public List<OM_Style> GetAllStyles()
        {
            return _styleRepository.Filter(x => x.CompID == _compId).OrderBy(x => x.StyleName).ToList();
        }

        public List<VStyle> GetStylePaging(OM_Style model, out int totalRecords)
        {
            var index = model.PageIndex;
            var pageSize = AppConfig.PageSize;
            var vStyles = _styleRepository.GetVStyles(x=>x.CompID==_compId
                && ((x.StyleName.Trim().Replace(" ", string.Empty).ToLower().Contains(model.SearchString.Trim().Replace(" ", string.Empty).ToLower()) || String.IsNullOrEmpty(model.SearchString))
                || (x.ItemName.Trim().Replace(" ", string.Empty).ToLower().Contains(model.SearchString.Trim().Replace(" ", string.Empty).ToLower()) || String.IsNullOrEmpty(model.SearchString))
                || (x.StylerefId.Trim().Replace(" ", string.Empty).ToLower().Contains(model.SearchString.Trim().Replace(" ", string.Empty).ToLower()) || String.IsNullOrEmpty(model.SearchString))));
            totalRecords = vStyles.Count();
            switch (model.sort)
            {
                case "StyleName":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            vStyles = vStyles
                                .OrderByDescending(r => r.StyleName)
                                .Skip(index * pageSize)
                                .Take(pageSize);

                            break;
                        default:
                            vStyles = vStyles
                                .OrderBy(r => r.StyleName)
                                .Skip(index * pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;
                case "ItemName":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            vStyles = vStyles
                                .OrderByDescending(r => r.ItemName)
                                .Skip(index * pageSize)
                                .Take(pageSize);

                            break;
                        default:
                            vStyles = vStyles
                                .OrderBy(r => r.ItemName)
                                .Skip(index * pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;
                case "StylerefId":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            vStyles = vStyles
                                .OrderByDescending(r => r.StylerefId)
                                .Skip(index * pageSize)
                                .Take(pageSize);

                            break;
                        default:
                            vStyles = vStyles
                                .OrderBy(r => r.StylerefId)
                                .Skip(index * pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;
                default:
                    vStyles = vStyles
                        .OrderByDescending(r => r.StylerefId)
                        .Skip(index * pageSize)
                        .Take(pageSize);
                    break;
            }
            return vStyles.ToList();
        }

        public VStyle GetVStyleById(long styleId)
        {
            var vStyle= _styleRepository.GetVStyles(x => x.CompID == _compId ).FirstOrDefault(x=> x.StyleId == styleId);
            return vStyle;
        }

        public string GetNewStyleRefId()
        {
            return _styleRepository.GetNewStyleRefId(_compId);
        }

        public int EditStyle(OM_Style model)
        {
            var style = _styleRepository.FindOne(x => x.StyleId == model.StyleId && x.CompID == _compId);
            style.StyleName = model.StyleName;
            style.ItemId = model.ItemId;
            return _styleRepository.Edit(style);
        }

        public int DeleteStyle(string stylerefId)
        {
            var isUsesd = _buyOrdStyleRepository.Exists(x => x.StyleRefId == stylerefId && x.CompId == _compId);
            var deleted = 0;
            if (isUsesd)
            {
                deleted = -1;
            }
            else
            {
                deleted = _styleRepository.Delete(x => x.StylerefId == stylerefId && x.CompID == _compId);
            }
            return deleted;
        }
        public int SaveStyle(OM_Style model)
        {
            model.CompID= _compId;
            model.StylerefId = _styleRepository.GetNewStyleRefId(_compId);
            return _styleRepository.Save(model);
        }

        public object GetItemForStyle(string searchKey)
        {
            return _itemRepository.Filter(x => x.ItemCode.Substring(0, 2) == "03" || x.ItemCode.Substring(0, 2) == "10" || x.ItemCode.Substring(0, 2) == "05" || x.ItemCode.Substring(0, 2) == "09")
                .Where(
                    x =>
                        x.IsActive &&
                        x.ItemName.Trim()
                            .Replace(" ", String.Empty)
                            .ToLower()
                            .Contains(searchKey.Trim().Replace(" ", String.Empty).ToLower()))
                .OrderBy(x => x.ItemName).Select(x => new
                {
                    ItemId = x.ItemId,
                    ItemName = x.ItemName,
                    ItemCode = x.ItemCode
                }).Take(10);


        }

        public bool CheckExistingStyle(OM_Style style)
        {
            return _styleRepository.GetVStyles(x => x.CompID == _compId).Any(x=>x.StyleId!=style.StyleId&&x.ItemId==style.ItemId&&x.StyleName==style.StyleName);
        }
    }
}
