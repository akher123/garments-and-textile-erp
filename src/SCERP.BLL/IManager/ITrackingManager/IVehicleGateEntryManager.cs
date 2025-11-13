using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.TrackingModel;

namespace SCERP.BLL.IManager.ITrackingManager
{
   public interface IVehicleGateEntryManager
    {
       List<TrackVehicleGateEntry> GetVehicleGateEntriListByPaging(TrackVehicleGateEntry model, out int totalRecords);
       int SaveVehicleGateEntry(TrackVehicleGateEntry model);
       int EditVehicleGateEnty(TrackVehicleGateEntry model);
       TrackVehicleGateEntry GetVehicleGateEntryById(long vehicleGateEntryId);
       int DeleteVehicleGateEntry(long vehicleGateEntryId);
       bool IsVehicleGateEntryExist(TrackVehicleGateEntry model);
    }
}
