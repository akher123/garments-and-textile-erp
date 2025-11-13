using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.Planning
{
    public class PLAN_TNAReport
    {
        public int Id { get; set; }
        public string CompId { get; set; }
        public string OrderStyleRefId { get; set; }
        public Nullable<int> ActivityId { get; set; }
        public string BuyerName { get; set; }
        public string StyleName { get; set; }
        public string OrderNo { get; set; }
        public string OrderDate { get; set; }
        public string MerchandiserName { get; set; }
        public string ActivityName { get; set; }
        public Nullable<int> LeadTime { get; set; }
        public string PlannedStartDate { get; set; }
        public string PlannedEndDate { get; set; }
        public string ActualStartDate { get; set; }
        public string ActrualEndDate { get; set; }
        public Nullable<System.Guid> ResponsiblePerson { get; set; }
        public string ResponsiblePersonName { get; set; }
        public Nullable<int> NotifyBeforeDays { get; set; }
        public string Remarks { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.Guid> CreatedBy { get; set; }
        public Nullable<System.DateTime> EditedDate { get; set; }
        public Nullable<System.Guid> EditedBy { get; set; }
        public bool IsActive { get; set; }
        public string EmailAddress { get; set; }
        public int? StartStatus { get; set; }
        public int? EndStatus { get; set; }
    }
}