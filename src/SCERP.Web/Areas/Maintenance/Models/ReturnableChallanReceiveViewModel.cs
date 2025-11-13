using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SCERP.Model.Maintenance;

namespace SCERP.Web.Areas.Maintenance.Models
{
    public class ReturnableChallanReceiveViewModel : MaintenanceSearchModel<ReturnableChallanReceiveViewModel>
    {
        public DateTime ReceiveDate { get; set; }
        public int ReceiveQty { get; set; }
        public ReturnableChallanReceiveViewModel()
        {
           ReturnableChallan=new Maintenance_ReturnableChallan();
           ReturnableChallanDetail=new Maintenance_ReturnableChallanDetail();
           ReturnableChallanReceive=new Maintenance_ReturnableChallanReceive();
           ReturnableChallanReceives=new List<Maintenance_ReturnableChallanReceive>();
            VwReturnableChallanReceive=new VwReturnableChallanReceive();
            VwReturnableChallanReceives=new List<VwReturnableChallanReceive>();
            DetailReceiveViewModels=new List<DetailReceiveViewModel>();
            VwReceiveDetails=new List<VwReceiveDetail>();
        }
        public Maintenance_ReturnableChallan ReturnableChallan { get; set; }
        public Maintenance_ReturnableChallanDetail ReturnableChallanDetail { get; set; }
        public Maintenance_ReturnableChallanReceive ReturnableChallanReceive { get; set; }
        public List<Maintenance_ReturnableChallanReceive> ReturnableChallanReceives { get; set; } 
        public VwReturnableChallanReceive VwReturnableChallanReceive { get; set; }
        public List<VwReceiveDetail> VwReceiveDetails { get; set; }
        public List<VwReturnableChallanReceive> VwReturnableChallanReceives { get; set; }
        public List<DetailReceiveViewModel> DetailReceiveViewModels { get; set; } 
    }
}