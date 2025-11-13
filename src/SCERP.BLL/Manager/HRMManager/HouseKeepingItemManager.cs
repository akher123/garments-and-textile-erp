using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.BLL.IManager.IHRMManager;
using SCERP.Common;
using SCERP.DAL.IRepository;
using SCERP.Model.CommonModel;
using SCERP.Model.HRMModel;

namespace SCERP.BLL.Manager.HRMManager
{
    public class HouseKeepingItemManager : IHouseKeepingItemManager
    {
        private IRepository<HouseKeepingItem> _houseKeepingItemRepository;

        public HouseKeepingItemManager(IRepository<HouseKeepingItem> houseKeepingItemRepository)
        {
            _houseKeepingItemRepository = houseKeepingItemRepository;
        }
        public List<HouseKeepingItem> GetHouseKeepingItems(int pageIndex, string sort, string sortdir, string searchString, out int totalRecords)
        {
            var index = pageIndex;
            var pageSize = AppConfig.PageSize;
            var houseKeepings =
                _houseKeepingItemRepository.Filter(x => x.Name.Trim().Contains(searchString) || String.IsNullOrEmpty(searchString)
                                                 || x.HkItemRefId.Contains(searchString) || String.IsNullOrEmpty(searchString));
            totalRecords = houseKeepings.Count();
            switch (sort)
            {
                case "Name":
                    switch (sortdir)
                    {
                        case "DESC":
                            houseKeepings = houseKeepings
                                .OrderByDescending(r => r.Name)
                                .Skip(index * pageSize)
                                .Take(pageSize);

                            break;
                        default:
                            houseKeepings = houseKeepings
                                .OrderBy(r => r.Name)
                                .Skip(index * pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;

                default:
                    houseKeepings = houseKeepings
                        .OrderByDescending(r => r.HkItemRefId)
                        .Skip(index * pageSize)
                        .Take(pageSize);
                    break;
            }
            return houseKeepings.ToList();
        }

        public HouseKeepingItem GetHouseKeepingItemById(int houseKeepingItemId)
        {
            return _houseKeepingItemRepository.FindOne(x => x.HouseKeepingItemId == houseKeepingItemId);
        }

        public string GetNewRefId()
        {
            var refId = _houseKeepingItemRepository.All().Max(x => x.HkItemRefId) ?? "0";
            return refId.IncrementOne().PadZero(3);
        }

        public int EditHouseKeepingItem(HouseKeepingItem houseKeepingItem)
        {
            HouseKeepingItem model = _houseKeepingItemRepository.FindOne(x => x.HouseKeepingItemId == houseKeepingItem.HouseKeepingItemId);
            model.Name = houseKeepingItem.Name;
            model.Description = houseKeepingItem.Description;
            return _houseKeepingItemRepository.Edit(model);
        }

        public int SaveHouseKeepingItem(HouseKeepingItem houseKeepingItem)
        {
            return _houseKeepingItemRepository.Save(houseKeepingItem);
        }

        public int DeleteHouseKeepingItem(HouseKeepingItem hk)
        {
            return _houseKeepingItemRepository.DeleteOne(hk);
        }

        public List<HouseKeepingItem> GetHouseKeepingItems(string compId)
        {
            return _houseKeepingItemRepository.Filter(x => x.CompId == compId).ToList();
        }
    }
}
