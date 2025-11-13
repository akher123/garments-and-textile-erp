using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCERP.Common;
using SCERP.Model.TrackingModel;

namespace SCERP.Web.Areas.Tracking.Models.ViewModels
{
    public class VisitorGateEntryViewModel : TrackVisitorGateEntry
    {
        public string EditStatus { get; set; }
        //public List<TrackVisitorGateEntry> VisitorGateEntryList { get; set; }
        public TrackVisitorGateEntry VisitorGateEntry { get; set; }
        public List<TrackConfirmationMedia> ConfirmationMediaList { get; set; }
        public string EmployeeName { get; set; }
        
        public VisitorGateEntryViewModel()
        {
            //VisitorGateEntryList=new List<TrackVisitorGateEntry>();
            VisitorGateEntry=new TrackVisitorGateEntry();
            ConfirmationMediaList=new List<TrackConfirmationMedia>();
            
        }

        public IEnumerable<SelectListItem> CheckOutStatusSelectListItem
        {
            get
            {
                return new SelectList(GetXStatusList(), "XStatus", "Name");
            }

        }
        public IEnumerable<SelectListItem> ConfirmationMediaListSelectListItem
        {
            get
            {
                return new SelectList(ConfirmationMediaList, "ConfirmationMediaId", "ConfirmationMedia");
            }
        }
        private IEnumerable GetXStatusList()
        {
            var xStatusList = (from CheckOutStatus s in Enum.GetValues(typeof(CheckOutStatus))
                               select new { XStatus = (int)s, Name = s.ToString() }).ToList();
            return xStatusList;
        }

    }
}