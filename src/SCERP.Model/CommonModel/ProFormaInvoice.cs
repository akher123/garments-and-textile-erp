using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.CommonModel
{
   public class ProFormaInvoice
    {
        public int PiId { get; set; }
        public string CompId { get; set; }
        public string PiRefId { get; set; }
        [Required]
        public string PiNo { get; set; }
        public int SupplierId { get; set; }

        public DateTime? ReceivedDate { get; set; }
        public string BookingNo { get; set; }
        public DateTime? EndDate { get; set; }
        public string PType { get; set; }
        public string Remarks { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid? EditedBy { get; set; }
        public DateTime? EditedDate { get; set; }

        public virtual  Mrc_SupplierCompany Mrc_SupplierCompany { get; set; }
    }
}
