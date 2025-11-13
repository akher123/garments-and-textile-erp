using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.MarketingModel
{
    public partial class MarketingInquiry
    {
        public int InquiryId { get; set; }
        [Required]
        public int MarketingPersonId { get; set; }
       [Required]
        public Nullable<System.DateTime> InquiryDate { get; set; }
        [Required]
        public int InstituteId { get; set; }
        public string InquiryContactPerson { get; set; }
        public string Mobile { get; set; }
        public string Telephone { get; set; }
        public string Email { get; set; }
        public string FurtherContactType { get; set; }
        public string Remarks { get; set; }
        public bool IsActive { get; set; }
        [Required]
        public double? Amount { get; set; }
        [Required]
        public double? OthersAmount { get; set; }
         [Required]
        public string BillNo { get; set; }
        public virtual MarketingPerson MarketingPerson { get; set; }
        public virtual MarketingInstitute MarketingInstitute { get; set; }

    }
}
