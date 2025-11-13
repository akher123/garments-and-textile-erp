using System;
using System.Collections.Generic;
using System.Linq;
using SCERP.BLL.IManager.ITrackingManager;
using SCERP.Common;
using SCERP.DAL.IRepository.ITrackingRepository;
using SCERP.Model.TrackingModel;

namespace SCERP.BLL.Manager.TrackingManager
{
    public class VehicleManager : IVehicleManager
    {
        private IVehicleRepository _vehicleRepository;
        private IVehicleGateEntryRepository _vehicleGateEntryRepository;
        public VehicleManager(IVehicleRepository vehicleRepository, IVehicleGateEntryRepository vehicleGateEntryRepository)
        {
            _vehicleRepository = vehicleRepository;
            _vehicleGateEntryRepository = vehicleGateEntryRepository;
        }

        public List<TrackVehicle> GetAllVehicleByPaging(TrackVehicle model, out int totalRecords)
        {
            var index = model.PageIndex;
            var pageSize = AppConfig.PageSize;
            var vehicleList =
                _vehicleRepository.Filter(
                    x =>x.IsActive==true && (x.VehicheType.Trim().Contains(model.SearchString) || String.IsNullOrEmpty(model.SearchString)));
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
        public List<TrackVehicle> GetAllVehicle()
        {
            return _vehicleRepository.All().OrderBy(y => y.VehicheType).ToList();
        }
        public bool IsVehicleTypeExist(TrackVehicle model)
        {
            return _vehicleRepository.Exists(
             x =>
                 x.CompanyId == PortalContext.CurrentUser.CompId && x.IsActive == true && x.VehicleId != model.VehicleId && x.VehicheType == model.VehicheType);
        }
        public int EditVehicle(TrackVehicle model)
        {
            var vehicle = _vehicleRepository.FindOne(x => x.IsActive == true && x.VehicleId == model.VehicleId);
            vehicle.VehicheType = model.VehicheType;
            vehicle.Remarks = model.Remarks;
            vehicle.EditedBy = PortalContext.CurrentUser.UserId;
            vehicle.EditedDate = DateTime.Now;
            return _vehicleRepository.Edit(vehicle);
        }
        public int SaveVehicle(TrackVehicle model)
        {
            model.CompanyId = PortalContext.CurrentUser.CompId;
            model.CreatedBy = PortalContext.CurrentUser.UserId;
            model.CreatedDate = DateTime.Now;
            model.IsActive = true;
            return _vehicleRepository.Save(model);
        }
        public int DeleteVehicle(long vehicleId)
        {
            var deleted = 0;
            if (_vehicleGateEntryRepository.Exists(x => x.IsActive == true && x.VehicleId == vehicleId))
            {
                deleted = -1;// This vehicle Id used by another table
            }
            else
            {
                var vehicle = _vehicleRepository.FindOne(x => x.IsActive == true && x.VehicleId == vehicleId);
                vehicle.IsActive = false;
                vehicle.EditedBy = PortalContext.CurrentUser.UserId;
                vehicle.EditedDate = DateTime.Now;
                return _vehicleRepository.Edit(vehicle);
            }
            return deleted;
        }
        public TrackVehicle GetVehicleById(int vehicleId)
        {
            return _vehicleRepository.FindOne(x => x.IsActive == true && x.VehicleId == vehicleId);
        }
    }
}
