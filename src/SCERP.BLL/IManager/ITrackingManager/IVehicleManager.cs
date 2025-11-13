using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.TrackingModel;

namespace SCERP.BLL.IManager.ITrackingManager
{
   public interface IVehicleManager
    {
       List<TrackVehicle> GetAllVehicle();
       List<TrackVehicle> GetAllVehicleByPaging(TrackVehicle model, out int totalRecords);
       int EditVehicle(TrackVehicle model);
       int SaveVehicle(TrackVehicle model);
       TrackVehicle GetVehicleById(int vehicleId);
       int DeleteVehicle(long vehicleId);
       bool IsVehicleTypeExist(TrackVehicle model);
    }
}
