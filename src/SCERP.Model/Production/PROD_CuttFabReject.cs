using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.Production
{
    public class PROD_CuttFabReject
    {
        public int CuttFabRejectId { get; set; }
        public string CompId { get; set; }
        [Required]
        public long BatchId { get; set; }
        [Required]
        public long BatchDetailId { get; set; }
        [Required]
        public double RejectWit { get; set; }
        [Required]
        public double CuttingWit { get; set; }
        public string ChallanNo { get; set; }
        public string Remarks { get; set; }
        public DateTime EntryDate { get; set; }

    }
}
