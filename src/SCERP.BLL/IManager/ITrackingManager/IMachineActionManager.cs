using System.Collections.Generic;
using SCERP.Model.TrackingModel;

namespace SCERP.BLL.IManager.ITrackingManager
{
   public interface IMachineActionManager
    {
       List<TrackMachineAction> GetAllMachineAction();
       int EditMachineAction(TrackMachineAction model);
       int SaveMachineAction(TrackMachineAction model);
       TrackMachineAction GetMachineActionByMachineActionId(int machineActionId);
       int DeleteMachineAction(int machineActionId);
       List<TrackMachineAction> GetAllMachineActionByPaging(TrackMachineAction model, out int totalRecords);
       bool IsMachineActonExist(TrackMachineAction model);
    }
}
