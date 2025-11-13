using SCERP.BLL.IManager.ITrackingManager;
using SCERP.Model.HRMModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.BLL.Manager.TrackingManager
{
    public class HouseKeepingItemManager : IHouseKeepingItemManager
    {


        public int DeleteHouseKeepingItem(int reportId)
        {
            throw new NotImplementedException();
        }

        public int EditHouseKeepingItem(HouseKeepingItem model)
        {
            throw new NotImplementedException();
        }

        public List<HouseKeepingItem> GetAllHouseKeepingItem()
        {
            throw new NotImplementedException();
        }

        public List<HouseKeepingItem> GetAllHouseKeepingItemPaging(HouseKeepingItem model, out int totalRecords)
        {
            var index = model.PageIndex;
            var pageSize = AppConfig.PageSize;
            var vehicleList =
                _vehicleRepository.Filter(
                    x => x.IsActive == true && (x.VehicheType.Trim().Contains(model.SearchString) || String.IsNullOrEmpty(model.SearchString)));
            totalRecords = vehicleList.Count();
            switch (model.sort)
            {
                case "VehicheType":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            vehicleList = vehicleList
                                 .OrderByDescending(r => r.VehicheType)
                                .Skip(index * pageSize)
                                .Take(pageSize);

                            break;
                        default:
                            vehicleList = vehicleList
                                 .OrderBy(r => r.VehicheType).ThenBy(r => r.VehicheType)
                                .Skip(index * pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;

                default:
                    vehicleList = vehicleList
                        .OrderByDescending(r => r.VehicleId)
                        .Skip(index * pageSize)
                        .Take(pageSize);
                    break;
            }
            return vehicleList.ToList();
        }

        public HouseKeepingItem GetHouseKeepingItemById(int itemId)
        {
            throw new NotImplementedException();
        }

        public bool IsHouseKeepingItemExist(HouseKeepingItem model)
        {
            throw new NotImplementedException();
        }

        public int SaveHouseKeepingItem(HouseKeepingItem model)
        {
            throw new NotImplementedException();
        }
    }
}
