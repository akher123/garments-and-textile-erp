using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using SCERP.Common;
using SCERP.Model.TrackingModel;

namespace SCERP.Web.Areas.Tracking.Models.ViewModels
{
    public class VehicleGateEntryViewModel : TrackVehicleGateEntry
    {
         public List<TrackVehicleGateEntry> VehicleGateEntryList { get; set; }
        public TrackVehicleGateEntry VehicleGateEntry { get; set; }
        public List<TrackConfirmationMedia> ConfirmationMediaList { get; set; }
        public List<TrackVehicle> Vehicles { get; set; } 
        public string EmployeeName { get; set; }

        public VehicleGateEntryViewModel()
        {
           VehicleGateEntryList=new List<TrackVehicleGateEntry>();
            VehicleGateEntry=new TrackVehicleGateEntry();
            ConfirmationMediaList=new List<TrackConfirmationMedia>();
            Vehicles=new List<TrackVehicle>();
        }
        public IEnumerable<SelectListItem> CheckOutStatusSelectListItem
        {
            get
            {
                return new SelectList(GetXStatusList(), "XStatus", "Name");
            }

        }
        private IEnumerable GetXStatusList()
        {
            var xStatusList = (from CheckOutStatus s in Enum.GetValues(typeof(CheckOutStatus))
                               select new { XStatus = (int)s, Name = s.ToString() }).ToList();
            return xStatusList;
        }
        public IEnumerable<SelectListItem> ConfirmationMediaSelectListItem 
        {
            get
            {
                return new SelectList(ConfirmationMediaList, "ConfirmationMediaId", "ConfirmationMedia");
            }
        }

        public IEnumerable<SelectListItem> VehiclesSelectListItem
        {
            get
            {
                return new SelectList(Vehicles, "VehicleId", "VehicheType");
            }
        }
    }
}