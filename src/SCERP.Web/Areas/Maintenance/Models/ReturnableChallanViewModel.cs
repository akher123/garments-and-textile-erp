using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCERP.Model.Maintenance;

namespace SCERP.Web.Areas.Maintenance.Models
{
    public class ReturnableChallanViewModel : MaintenanceSearchModel<ReturnableChallanViewModel>
    {
        public int ChallanStatus { get; set; }
        public DateTime? DateFrom { set; get; }
        public DateTime? DateTo { set; get; }  
        public string Key { get; set; }
        public ReturnableChallanViewModel()
        {
           ReturnableChallan=new Maintenance_ReturnableChallan();
           ReturnableChallans = new List<Maintenance_ReturnableChallan>();
           ReturnableChallanDetail=new Maintenance_ReturnableChallanDetail();
           ReturnableChallanDetails=new List<Maintenance_ReturnableChallanDetail>();
           ReturnableChallanDictionary=new Dictionary<string, Maintenance_ReturnableChallanDetail>();
           VwReturnableChallans=new List<VwReturnableChallan>();
        }
        public Maintenance_ReturnableChallan ReturnableChallan { get; set; }
        public List<Maintenance_ReturnableChallan> ReturnableChallans { get; set; }
        public Maintenance_ReturnableChallanDetail ReturnableChallanDetail { get; set; }
        public List<Maintenance_ReturnableChallanDetail> ReturnableChallanDetails { get; set; }
        public Dictionary<string, Maintenance_ReturnableChallanDetail> ReturnableChallanDictionary { get; set; }
        public List<VwReturnableChallan> VwReturnableChallans { get; set; }
       
        public IEnumerable<SelectListItem> ApprovedReturnableChallanSelectListItem 
        {
            get
            {
                return new SelectList(new[] { new { Text = "Approved", Value = true }, new { Text = "Pending", Value = false } }, "Value", "Text");
            }
        }
        public IEnumerable<SelectListItem> ChallanStatusSelectListItem
        {
            get
            {
                return new SelectList(new[] { new { Text = "All", Value = "1" }, new { Text = "Pending", Value = "2" } }, "Value", "Text");
            }
        }

    }
}