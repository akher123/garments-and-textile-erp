using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.MerchandisingModel
{
    public partial class OM_SampleOrder
    {
        public OM_SampleOrder()
        {
            this.OM_SampleStyle = new HashSet<OM_SampleStyle>();
        }
        public int SampleOrderId { get; set; }
        [Required]
        public string RefId { get; set; }
        [Required]
        public int MerchandiserId { get; set; }
        [Required]
        public long BuyerId { get; set; }
        [Required]
        public string OrderNo { get; set; }
        public string ArticleNo { get; set; }
        [Required]
        public DateTime? OrderDate { get; set; }
        public int? OrderQty { get; set; }
        public string Season { get; set; }
        public string Agent { get; set; }
        public string Remarks { get; set; }
        [Required]
        public string CompId { get; set; }

        public Guid CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid? EditedBy { get; set; }
        public DateTime? EditedDate { get; set; }
        public bool IsApproved { get; set; }
        public Guid? ApprovedBy { get; set; }
        public DateTime ApprovedDate { get; set; }
        public virtual OM_Buyer OM_Buyer { get; set; }
        public virtual OM_Merchandiser OM_Merchandiser { get; set; }
        public virtual ICollection<OM_SampleStyle> OM_SampleStyle { get; set; }
    }
}
