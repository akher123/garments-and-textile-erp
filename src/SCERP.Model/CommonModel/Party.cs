using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.InventoryModel;
using SCERP.Model.Production;

namespace SCERP.Model.CommonModel
{

    public partial class Party:SearchModel<Party>
    {
        public Party()
        {
            this.Pro_Batch = new HashSet<Pro_Batch>();
            this.PROD_ProcessDelivery=new HashSet<PROD_ProcessDelivery>();
            this.PROD_CuttingTagSupplier=new HashSet<PROD_CuttingTagSupplier>();
            this.PROD_ProcessReceive = new HashSet<PROD_ProcessReceive>();
            this.PLAN_Program = new HashSet<PLAN_Program>();
            this.PROD_KnittingRoll = new HashSet<PROD_KnittingRoll>();
            this.PROD_DyeingSpChallan=new HashSet<PROD_DyeingSpChallan>();
            this.Inventory_FinishFabricIssue = new HashSet<Inventory_FinishFabricIssue>();
            this.PROD_DyeingJobOrder = new HashSet<PROD_DyeingJobOrder>();
            this.Inventory_ReDyeingFabricReceive = new HashSet<Inventory_ReDyeingFabricReceive>();
            this.Inventory_RedyeingFabricIssue = new HashSet<Inventory_RedyeingFabricIssue>();
            this.Inventory_RejectYarnIssue = new HashSet<Inventory_RejectYarnIssue>();
            this.Inventory_GreyIssue = new HashSet<Inventory_GreyIssue>();
            this.OM_EmbWorkOrder = new HashSet<OM_EmbWorkOrder>();

        }
       [Required(ErrorMessage = @"Required")]
        public string Name { get; set; }
       [Required(ErrorMessage = @"Required")]
        public string PartyRefNo { get; set; }
        public long PartyId { get; set; }
        public string CompId { get; set; }
       [Required(ErrorMessage = @"Required")]
        public string Address { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string ContactPersonName { get; set; }
        public string ContactPhone { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? EditedDate { get; set; }
        public Guid? EditedBy { get; set; }
        public bool IsActive { get; set; }
        public string PType { get; set; }
        public int? DglId { get; set; }
        public int? KglId { get; set; }
        public int? KRglId { get; set; }

        public int? PGlId { get; set; }
        public int? EmGlId { get; set; }
        public ICollection<OM_EmbWorkOrder> OM_EmbWorkOrder { get; set; }
        public ICollection<PLAN_Program> PLAN_Program { get; set; }
        public virtual ICollection<Pro_Batch> Pro_Batch { get; set; }
        public virtual ICollection<PROD_CuttingTagSupplier> PROD_CuttingTagSupplier { get; set; }
        public virtual ICollection<PROD_ProcessDelivery> PROD_ProcessDelivery { get; set; }
        public virtual ICollection<PROD_ProcessReceive> PROD_ProcessReceive { get; set; }
        public virtual ICollection<PROD_DyeingJobOrder> PROD_DyeingJobOrder { get; set; }
        public virtual ICollection<PROD_KnittingRoll> PROD_KnittingRoll { get; set; }
        public virtual ICollection<PROD_DyeingSpChallan> PROD_DyeingSpChallan { get; set; }
        public virtual ICollection<Inventory_FinishFabricIssue> Inventory_FinishFabricIssue { get; set; }
        public virtual ICollection<Inventory_ReDyeingFabricReceive> Inventory_ReDyeingFabricReceive { get; set; }
        public virtual ICollection<Inventory_RedyeingFabricIssue> Inventory_RedyeingFabricIssue { get; set; }
        public virtual ICollection<Inventory_RejectYarnIssue> Inventory_RejectYarnIssue { get; set; }
        public virtual ICollection<Inventory_GreyIssue> Inventory_GreyIssue { get; set; }
        
        
    }
}
