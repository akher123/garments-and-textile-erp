using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using SCERP.Model.Maintenance;

namespace SCERP.Web.Areas.Maintenance.Models
{
    public class ChallanReceiveMasterViewModel : MaintenanceSearchModel<ChallanReceiveMasterViewModel>
    {
        public long ReturnableChallanId { get; set; }
        public ChallanReceiveMasterViewModel()
        {
            ChallanReceiveMaster = new Maintenance_ReturnableChallanReceiveMaster();
            ReturnableChallan = new Maintenance_ReturnableChallan();
            ChallanReceiveMasterList = new List<Maintenance_ReturnableChallanReceiveMaster>();
            VwChallanReceiveMasters=new List<VwChallanReceiveMaster>();
            VwReturnableChallanReceives = new List<VwReturnableChallanReceive>();
            RChallanReceiveDictionary = new Dictionary<string, VwReturnableChallanReceive>();
            DataTable=new DataTable();
        }
        public DataTable DataTable { get; set; }
        public Maintenance_ReturnableChallanReceiveMaster ChallanReceiveMaster { get; set; }
        public Maintenance_ReturnableChallan ReturnableChallan { get; set; }
        public List<Maintenance_ReturnableChallanReceiveMaster> ChallanReceiveMasterList { get; set; } 
        public List<VwChallanReceiveMaster> VwChallanReceiveMasters { get; set; }
        public List<VwReturnableChallanReceive> VwReturnableChallanReceives { get; set; }
        public Dictionary<string, VwReturnableChallanReceive> RChallanReceiveDictionary { get; set; }
    }
}