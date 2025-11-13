using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.InventoryModel
{
   public class Inventory_FabricReturn
    {
        public long FabricReturnId { get; set; }
        public long ProgramId { get; set; }
        [Required]
        public string ReturnChallanNo { get; set; }
              [Required]
        public System.DateTime ReturnDate { get; set; }
         [Required]
        public double FabQty { get; set; }
    
          public int? QtyInPcs { get; set; }
        public double ReturnYarnQty { get; set; }
        public double WstYarnQty { get; set; }
        public string Remarks { get; set; }
        public Nullable<System.Guid> ReceivedBy { get; set; }
        public string CompId { get; set; }
        public long? ProgramDetailId { get; set; }
        [Required]
        public string ProcessRefId { get; set; }
        public virtual PLAN_Program PLAN_Program { get; set; }
    }
}
