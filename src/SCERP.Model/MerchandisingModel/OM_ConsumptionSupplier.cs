using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.MerchandisingModel
{
    public partial class OM_ConsumptionSupplier
    {
       
        public int ConsumptionSupplierId { get; set; }
        public long ConsumptionId { get; set; }
           [Required(ErrorMessage = "Required!")]
        public int SupplierId { get; set; }
        [Range(1.0 ,double.MaxValue)]
        public double Quantity { get; set; }
        public double Percentage { get; set; }
        public double Rate { get; set; }
        public string Remarks { get; set; }
        public string CompId { get; set; }
        public virtual OM_Consumption OM_Consumption { get; set; }
        public virtual Mrc_SupplierCompany Mrc_SupplierCompany { get; set; }

    }
}
