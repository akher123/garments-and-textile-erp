using System.Collections.Generic;
using SCERP.Model;
using SCERP.Model.TrackingModel;

namespace SCERP.Web.Areas.Tracking.Models.ViewModels
{
    public class VehicleViewModel:TrackVehicle
    {
        public VehicleViewModel()
        {
            Vehicles=new List<TrackVehicle>();
        }
        public List<TrackVehicle> Vehicles { get; set; }
    }
}