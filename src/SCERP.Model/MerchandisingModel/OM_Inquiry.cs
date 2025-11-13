using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.MerchandisingModel
{
   public class OM_Inquiry
    {
       public int InquiryId { get; set; }
       public string InquiryRef { get; set; }
       public string BuyerName { get; set; }
       public string Season { get; set; }
       public string DesignRef { get; set; }
       public string Designer { get; set; }
       public string Photo { get; set; }
       public string Description { get; set; }
       public string FabricationGsm { get; set; }
       public string Colour { get; set; }
       public string SampleType { get; set; }
       public string SampleSize { get; set; }
       public DateTime? RecvedDate { get; set; }
       public DateTime? SubmissionDate { get; set; }
       public double? PriceUSD { get; set; }
       public string Remarks { get; set; }
        public string Merchandiser { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? EditedDate { get; set; }
        public Guid? CreatedBy { get; set; }
        public Guid? EditedBy { get; set; }
        public string CompId { get; set; }
        public bool IsActive { get; set; }
    }
}
