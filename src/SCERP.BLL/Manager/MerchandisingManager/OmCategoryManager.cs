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
    public class OmCategoryManager : IOmCategoryManager
    {
        private readonly IOmCategoryRepository _categoryRepository;
        private readonly IOmBuyOrdStyleRepository _buyOrdStyleRepository;
        private readonly string _compId;
        public OmCategoryManager(IOmCategoryRepository categoryRepository, IOmBuyOrdStyleRepository buyOrdStyleRepository)
        {
            _categoryRepository = categoryRepository;
            _buyOrdStyleRepository = buyOrdStyleRepository;
           _compId = PortalContext.CurrentUser.CompId;
        }
        public List<OM_Category> GetCategories()
        {
            return _categoryRepository.Filter(x => x.CompId == _compId).ToList();
        }
        public List<OM_Category> GetCategoriesByPaging(OM_Category model, out int totalRecords)
        {
            var index = model.PageIndex;
            var pageSize = AppConfig.PageSize;
            var categories = _categoryRepository.Filter(x => x.CompId == _compId
                && ((x.CatName.Trim().Replace(" ", string.Empty).ToLower().Contains(model.SearchString.Trim().Replace(" ", string.Empty).ToLower()) || String.IsNullOrEmpty(model.SearchString))
                || (x.CatRefId.Trim().Replace(" ", string.Empty).ToLower().Contains(model.SearchString.Trim().Replace(" ", string.Empty).ToLower()) || String.IsNullOrEmpty(model.SearchString))));
            totalRecords = categories.Count();
            switch (model.sort)
            {
                case "CatName":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            categories = categories
                                .OrderByDescending(r => r.CatName)
                                .Skip(index * pageSize)
                                .Take(pageSize);

                            break;
                        default:
                            categories = categories
                                .OrderBy(r => r.CatName)
                                .Skip(index * pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;
                case "CatRefId":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            categories = categories
                                .OrderByDescending(r => r.CatRefId)
                                .Skip(index * pageSize)
                                .Take(pageSize);

                            break;
                        default:
                            categories = categories
                                .OrderBy(r => r.CatRefId)
                                .Skip(index * pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;
                default:
                    categories = categories
                        .OrderBy(r => r.CatRefId)
                        .Skip(index * pageSize)
                        .Take(pageSize);
                    break;
            }
            return categories.ToList();
        }

        public string GetNewCategoryRefId()
        {
            return _categoryRepository.GetNewCategoryRefId(_compId);
        }

        public OM_Category GetCategoryById(int catergoryId)
        {
            return _categoryRepository.FindOne(x => x.CatergoryId == catergoryId && x.CompId == _compId);
        }

        public int EditCatergory(OM_Category model)
        {
            var category = _categoryRepository.FindOne(x => x.CatergoryId == model.CatergoryId && x.CompId == _compId);
            category.CatName = model.CatName;
            return _categoryRepository.Edit(category);

        }

        public int SaveCatergory(OM_Category model)
        {
            model.CompId = _compId;
            model.CatRefId = _categoryRepository.GetNewCategoryRefId(_compId);
            return _categoryRepository.Save(model);
        }

        public int DeleteCategory(string catRefId)
        {
            var isUsesd = _buyOrdStyleRepository.Exists(x => x.CatIRefId == catRefId && x.CompId == _compId);
            var deleted = 0;
            if (isUsesd)
            {
                deleted = -1;
            }
            else
            {
                deleted = _categoryRepository.Delete(x => x.CatRefId == catRefId && x.CompId == _compId);
            }
            return deleted;
        }

        public bool CheckExistingCategory(OM_Category model)
        {
           return _categoryRepository.Exists(
                x =>
                    x.CompId == _compId && x.CatergoryId != model.CatergoryId &&
                    x.CatName.ToLower().Equals(model.CatName.ToLower()));
        }
    }
}