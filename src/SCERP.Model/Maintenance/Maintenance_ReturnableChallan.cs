using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SCERP.Model.Maintenance
{
    
    
    public partial class Maintenance_ReturnableChallan
    {
        public Maintenance_ReturnableChallan()
        {
            this.Maintenance_ReturnableChallanDetail = new HashSet<Maintenance_ReturnableChallanDetail>();
            this.Maintenance_ReturnableChallanReceiveMaster = new HashSet<Maintenance_ReturnableChallanReceiveMaster>();
        }

        public long ReturnableChallanId { get; set; }
        public string ReturnableChallanRefId { get; set; }
        [Required]
        public string Messrs { get; set; }
        public string Address { get; set; }
        [Required]
        public string RefferancePerson { get; set; }
        public string Designation { get; set; }
        public string Department { get; set; }
        public string EmployeeCardId { get; set; }
        [Required]
        public Nullable<System.DateTime> ChallanDate { get; set; }
        public string CompId { get; set; }
        public string Remarks { get; set; }
        public string Phone { get; set; }
        public string ChllanType { get; set; }
        public Nullable<bool> IsApproved { get; set; }
        public Nullable<System.Guid> ApprovedBy { get; set; }
        public Nullable<System.Guid> PreparedBy { get; set; }
        public virtual ICollection<Maintenance_ReturnableChallanDetail> Maintenance_ReturnableChallanDetail { get; set; }
        public virtual ICollection<Maintenance_ReturnableChallanReceiveMaster> Maintenance_ReturnableChallanReceiveMaster { get; set; }
    }
}
