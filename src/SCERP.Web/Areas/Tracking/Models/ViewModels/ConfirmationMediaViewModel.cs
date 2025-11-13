using System.Collections.Generic;
using SCERP.Model.TrackingModel;

namespace SCERP.Web.Areas.Tracking.Models.ViewModels
{
    public class ConfirmationMediaViewModel:TrackConfirmationMedia
    {
        public List<TrackConfirmationMedia> ConfirmationMediaList { get; set; }
        public ConfirmationMediaViewModel()
        {
            ConfirmationMediaList=new List<TrackConfirmationMedia>();
        }
    }
}