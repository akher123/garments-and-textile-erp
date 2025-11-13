
using System;
using System.Collections.Generic;
using System.Linq;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.Common;
using SCERP.DAL.IRepository.IMerchandisingRepository;
using SCERP.Model;

namespace SCERP.BLL.Manager.MerchandisingManager
{
    public class OmBrandManager : IOmBrandManager
    {
        private readonly IOmBrandRepository _brandRepository;
        private readonly string _compId;
        public OmBrandManager(IOmBrandRepository brandRepository)
        {
            _brandRepository = brandRepository;
            _compId = PortalContext.CurrentUser.CompId;
        }

        public List<OM_Brand> GetBrands()
        {
            return _brandRepository.Filter(x => x.CompId == _compId).OrderBy(x => x.BrandName).ToList();
        }

        public List<OM_Brand> GetOmBrandsByPaging(OM_Brand model, out int totalRecords)
        {

            var index = model.PageIndex;
            var pageSize = AppConfig.PageSize;
            var brndList = _brandRepository.Filter(x => x.CompId == _compId
                && ((x.BrandName.Trim().Replace(" ", string.Empty).ToLower().Contains(model.SearchString.Trim().Replace(" ", string.Empty).ToLower()) || String.IsNullOrEmpty(model.SearchString))
                || (x.BrandRefId.Trim().Replace(" ", string.Empty).ToLower().Contains(model.SearchString.Trim().Replace(" ", string.Empty).ToLower()) || String.IsNullOrEmpty(model.SearchString))));
            totalRecords = brndList.Count();
            switch (model.sort)
            {
                case "BrandName":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            brndList = brndList
                                .OrderByDescending(r => r.BrandName)
                                .Skip(index * pageSize)
                                .Take(pageSize);

                            break;
                        default:
                            brndList = brndList
                                .OrderBy(r => r.BrandName)
                                .Skip(index * pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;
                case "BrandRefId":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            brndList = brndList
                                .OrderByDescending(r => r.BrandRefId)
                                .Skip(index * pageSize)
                                .Take(pageSize);

                            break;
                        default:
                            brndList = brndList
                                .OrderBy(r => r.BrandRefId)
                                .Skip(index * pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;
                default:
                    brndList = brndList
                        .OrderByDescending(r => r.BrandRefId)
                        .Skip(index * pageSize)
                        .Take(pageSize);
                    break;
            }
            return brndList.ToList();
        }

        public OM_Brand GetOmBrandById(int brandId)
        {
            return _brandRepository.FindOne(x => x.BrandId == brandId && x.CompId == _compId);
        }

        public string GetNewBrandRefId()
        {
            return _brandRepository.GetNewBrandRefId(_compId);
        }

        public int EditOmBrand(OM_Brand model)
        {
            var brand = _brandRepository.FindOne(x => x.BrandId == model.BrandId && x.CompId == _compId);
            brand.BrandName = model.BrandName;
            return _brandRepository.Edit(brand);
        }

        public int SaveOmBrand(OM_Brand model)
        {
            model.CompId = _compId;
            model.BrandRefId = _brandRepository.GetNewBrandRefId(_compId);
            return _brandRepository.Save(model);
        }

        public int DeleteOmBrand(string brandRefId)
        {

            int deleted = _brandRepository.Delete(x => x.BrandRefId == brandRefId && x.CompId == _compId);
            return deleted;
        }

        public bool CheckExistingBrand(OM_Brand model)
        {
           return _brandRepository.Exists( x => x.BrandId != model.BrandId && x.CompId == _compId &&x.BrandName.Trim().ToLower().Equals(model.BrandName.Trim().ToLower()));
        }
    }
}
