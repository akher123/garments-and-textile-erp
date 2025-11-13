using System;
using System.Collections.Generic;
using System.Linq;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.Common;
using SCERP.DAL;
using SCERP.DAL.IRepository.IMerchandisingRepository;
using SCERP.DAL.Repository.MerchandisingRepository;
using SCERP.Model;

namespace SCERP.BLL.Manager.MerchandisingManager
{
    public class OmSizeManager : IOmSizeManager
    {
        private readonly IOmSizeRepository _sizeRepository;
        private readonly string _compId;
        private readonly IBuyOrdStyleSizeRepository _buyOrdStyleSizeRepository;
        public OmSizeManager(IOmSizeRepository sizeRepository, IBuyOrdStyleSizeRepository buyOrdStyleSizeRepository)
        {
            _sizeRepository = sizeRepository;
            _compId = PortalContext.CurrentUser.CompId;
            _buyOrdStyleSizeRepository = buyOrdStyleSizeRepository;
        }

        public object SizeAutoComplite(string serachString, string typeId)
        {
            return
                _sizeRepository.Filter(
                    x => x.CompId == _compId && (x.TypeId==typeId||String.IsNullOrEmpty(typeId))&&
                        x.SizeName.Trim()
                            .Replace(" ", String.Empty)
                            .ToLower()
                            .Contains(serachString.Trim().Replace(" ", String.Empty).ToLower())).Take(10).ToList();
        }

        public List<OM_Size> GetOmSizesByPaging(OM_Size model, out int totalRecords)
        {
            var index = model.PageIndex;
            var pageSize = AppConfig.PageSize;
            var sizeList = _sizeRepository.Filter(x => x.CompId == _compId && (x.TypeId == model.TypeId || String.IsNullOrEmpty(model.TypeId))
                && ((x.SizeName.Trim().Replace(" ", string.Empty).ToLower().Contains(model.SearchString.Trim().Replace(" ", string.Empty).ToLower()) || String.IsNullOrEmpty(model.SearchString))
                || (x.SizeRefId.Trim().Replace(" ", string.Empty).ToLower().Contains(model.SearchString.Trim().Replace(" ", string.Empty).ToLower()) || String.IsNullOrEmpty(model.SearchString))
               || (x.ItemType.Trim().Replace(" ", string.Empty).ToLower().Contains(model.SearchString.Trim().Replace(" ", string.Empty).ToLower()) || String.IsNullOrEmpty(model.SearchString))
                ));
            totalRecords = sizeList.Count();
            switch (model.sort)
            {
                case "SizeName":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            sizeList = sizeList
                                .OrderByDescending(r => r.SizeName)
                                .Skip(index * pageSize)
                                .Take(pageSize);

                            break;
                        default:
                            sizeList = sizeList
                                .OrderBy(r => r.SizeName)
                                .Skip(index * pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;
                case "SizeRefId":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            sizeList = sizeList
                                .OrderByDescending(r => r.SizeRefId)
                                .Skip(index * pageSize)
                                .Take(pageSize);

                            break;
                        default:
                            sizeList = sizeList
                                .OrderBy(r => r.SizeRefId)
                                .Skip(index * pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;
                default:
                    sizeList = sizeList
                        .OrderByDescending(r => r.SizeRefId)
                        .Skip(index * pageSize)
                        .Take(pageSize);
                    break;
            }
            return sizeList.ToList();
        }

        public OM_Size GetOmSizeById(int sizeId)
        {
            return _sizeRepository.FindOne(x => x.SizeId == sizeId && x.CompId == _compId);
        }

        public string GetNewOmSizeRefId()
        {
            return _sizeRepository.GetNewOmSizeRefId(_compId);
        }

        public int EditOmSize(OM_Size model)
        {
            var sizeItem = _sizeRepository.FindOne(x => x.SizeId == model.SizeId && x.CompId == _compId);
            sizeItem.SizeName = model.SizeName;
            sizeItem.TypeId = model.TypeId;

            sizeItem.ItemType = model.ItemType;
            return _sizeRepository.Edit(sizeItem);
        }

        public int SaveOmSize(OM_Size model)
        {

            bool isExist = _sizeRepository.Exists(x=>x.CompId==_compId&&
              x.SizeName.Trim().Replace(" ", "").ToLower() == model.SizeName.Trim().Replace(" ", "").ToLower());
            if (!isExist)
            {
                model.CompId = _compId;
                model.SizeRefId = _sizeRepository.GetNewOmSizeRefId(_compId);
                return _sizeRepository.Save(model);
            }
            else
            {
                throw new Exception(model.SizeName + " Size name already Exist");
            }
        }

        public int DeleteOmSize(string sizeRefId)
        {
            bool isUsesd = _buyOrdStyleSizeRepository.Exists(x => x.SizeRefId == sizeRefId && x.CompId == _compId);
            var deleted = 0;
            if (isUsesd)
            {
                deleted = -1;
            }
            else
            {
                deleted = _sizeRepository.Delete(x => x.SizeRefId == sizeRefId && x.CompId == _compId);
            }
            return deleted;
        }

        public bool CheckSizeExist(OM_Size size)
        {
            var exist = _sizeRepository.Exists(
             x =>
                 x.CompId == _compId && x.SizeId != size.SizeId && x.SizeName == size.SizeName &&
                 x.TypeId == size.TypeId);
            return exist;
        }

        public List<OM_Size> GetOmSizes()
        {
            return _sizeRepository.Filter(x => x.CompId == _compId).ToList();
        }
    }
}
