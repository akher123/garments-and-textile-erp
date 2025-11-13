using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.Production
{
    public class PROD_FinishingProcess
    {

        public PROD_FinishingProcess()
        {
            this.PROD_FinishingProcessDetail = new HashSet<PROD_FinishingProcessDetail>();
        }

        public long FinishingProcessId { get; set; }
        public string FinishingProcessRefId { get; set; }
        public string CompId { get; set; }
        public Nullable<System.DateTime> InputDate { get; set; }
        public int HourId { get; set; }
        public Nullable<System.Guid> PreparedBy { get; set; }
        public int FType { get; set; }
        [Required]
        public string BuyerRefId { get; set; }
        [Required]
        public string OrderNo { get; set; }
        [Required]
        public string OrderStyleRefId { get; set; }
        [Required]
        public string ColorRefId { get; set; }
        public string Remarks { get; set; }
        [Required]
        public string OrderShipRefId { get; set; }
        public virtual PROD_Hour PROD_Hour { get; set; }

        public virtual ICollection<PROD_FinishingProcessDetail> PROD_FinishingProcessDetail { get; set; }
    }
}
