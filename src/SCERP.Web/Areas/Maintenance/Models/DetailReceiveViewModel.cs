using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SCERP.Model.Maintenance;

namespace SCERP.Web.Areas.Maintenance.Models
{
    public class DetailReceiveViewModel
    {
        public VwReturnableChallanReceive VwRturnableChallanReceive { get; set; } 
        public List<Maintenance_ReturnableChallanReceive> ReturnableChallanReceives { get; set; }
    }
}