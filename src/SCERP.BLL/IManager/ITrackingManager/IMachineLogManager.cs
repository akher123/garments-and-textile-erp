using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.TrackingModel;

namespace SCERP.BLL.IManager.ITrackingManager
{
   public interface IMachineLogManager
    {
       List<TrackMachineLog> GetAllMachineLogByPaging(TrackMachineLog model, out int totalRecords);
       int SaveMachineLog(TrackMachineLog model);
       int EditMachineLog(TrackMachineLog model);
       TrackMachineLog GetMachineLogByMachineLogId(long machineLogId);
       int DeleteMachineLog(long machineLogId);
       string GetNewMachineLogRefId();
       bool IsMachineLogExist(TrackMachineLog model);
    }
}
