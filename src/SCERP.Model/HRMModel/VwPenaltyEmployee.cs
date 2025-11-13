using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SCERP.Model.HRMModel
{
       
    public partial class VwPenaltyEmployee
    {
        public int PenaltyId { get; set; }
        public System.Guid EmployeeId { get; set; }
        public string EmployeeCardId { get; set; }
        public string Penalty { get; set; }
        public string PenaltyType { get; set; }
        public System.DateTime PenaltyDate { get; set; }
        public string Reason { get; set; }
        public System.Guid ClaimerId { get; set; }
        public string EmployeeName { get; set; }
        public string ClaimerName { get; set; }
        public bool IsActive { get; set; }
    }
}
