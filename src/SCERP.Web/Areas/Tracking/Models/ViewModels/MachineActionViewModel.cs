using System.Collections.Generic;
using System.Web.Mvc;
using SCERP.Model.TrackingModel;

namespace SCERP.Web.Areas.Tracking.Models.ViewModels
{
    public class MachineActionViewModel:TrackMachineAction
    {
        public List<TrackMachineAction> MachineActions { get; set; }
        
        public MachineActionViewModel()
        {
           MachineActions=new List<TrackMachineAction>(); 
        }
        
    }
}