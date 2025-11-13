using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.TrackingModel;

namespace SCERP.BLL.IManager.ITrackingManager
{
   public interface IConfirmationMediaManager
    { 
       List<TrackConfirmationMedia> GetAllConfirmationMediaList();
       List<TrackConfirmationMedia> GetAllConfirmationMediaListByPaging(TrackConfirmationMedia model, out int totalRecords);
       int SaveConfirmationMedia(TrackConfirmationMedia model);
       int EditConfirmationMedia(TrackConfirmationMedia model);
       TrackConfirmationMedia GetConfirmationMediaById(int confirmationMediaId);
       int DeleteConfirmationMedia(long confirmationMediaId);
       bool IsConfirmationMediaExist(TrackConfirmationMedia model);
    }
}
