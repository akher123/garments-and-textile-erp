using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.MarketingModel
{
    public partial class MarketingInstitute
    {
        public MarketingInstitute()
        {
            MarketingInquiry=new HashSet<MarketingInquiry>();
        }
        public int InstituteId { get; set; }

        [Required]
        public string InstituteName { get; set; }
        public string District { get; set; }
        public string Address { get; set; }
        public string DecisionMaker { get; set; }
        public string Designation { get; set; }
        public string Mobile { get; set; }
        public string Telephone { get; set; }
        public string Email { get; set; }
        public string WebSite { get; set; }
        public bool IsAvailable { get; set; }
        public string Remarks { get; set; }
        public Nullable<int> StatusId { get; set; }
        public Nullable<System.DateTime> ClientEntryDate { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public HashSet<MarketingInquiry> MarketingInquiry { get; set; }
    }
}
