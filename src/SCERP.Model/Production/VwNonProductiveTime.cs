using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.Production
{
    public class VwNonProductiveTime
    {
        public int NonProductiveTimeId { get; set; }
        public string BuyerRefId { get; set; }
        public string OrderNo { get; set; }
        public string OrderStyleRefId { get; set; }
        public long MachineId { get; set; }
        public string NptRefId { get; set; }
        public System.DateTime StartTime { get; set; }
        public Nullable<System.DateTime> EndTime { get; set; }
        public string Solution { get; set; }
        public string Supervisor { get; set; }
        public int DownTimeCategoryId { get; set; }
        public string CatergoryName { get; set; }
        public Nullable<System.DateTime> EntryDate { get; set; }
        public string BuyerName { get; set; }
        public string OrderName { get; set; }
        public string StyleName { get; set; }
        public string ResponsibleDepartment { get; set; }
        public string Remarks { get; set; }
        public string CompId { get; set; }
        public int Manpower { get; set; }
        public string MachineName { get; set; }
    }
}
