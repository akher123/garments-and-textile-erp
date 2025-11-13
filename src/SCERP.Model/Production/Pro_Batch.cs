using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Common;
using SCERP.Model.CommonModel;
using SCERP.Model.InventoryModel;

namespace SCERP.Model.Production
{
    public  class Pro_Batch : ProSearchModel<Pro_Batch>
    {
        public Pro_Batch()
        {
            this.PROD_BatchDetail = new HashSet<PROD_BatchDetail>();
            this.Inventory_FinishFabDetailStore = new HashSet<Inventory_FinishFabDetailStore>();
            this.PROD_DyeingSpChallanDetail=new HashSet<PROD_DyeingSpChallanDetail>();
        }

        public long BatchId { get; set; }
        [Required(ErrorMessage =CustomErrorMessage.RequiredErrorMessage)]
        public string BatchNo { get; set; }
        public int BtType { get; set; }
        public string BtRefNo { get; set; }
        [Required(ErrorMessage = CustomErrorMessage.RequiredErrorMessage)]
        [DataType(DataType.Date)]
        public DateTime? BatchDate { get; set; }
          [Required(ErrorMessage = CustomErrorMessage.RequiredErrorMessage)]
        public long PartyId { get; set; }
          [Required(ErrorMessage = CustomErrorMessage.RequiredErrorMessage)]
        public int MachineId { get; set; }
          [Required(ErrorMessage = CustomErrorMessage.RequiredErrorMessage)]
        public decimal? BatchQty { get; set; }
          [Required(ErrorMessage = CustomErrorMessage.RequiredErrorMessage)]
        public long ColorId { get; set; }
        public int BatchStatus { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? EditedDate { get; set; }
        public Guid? EditedBy { get; set; }
        public bool IsActive { get; set; }
        public string CompId { get; set; }
        public Nullable<int> ItemId { get; set; }
        public string FColorRefId { get; set; }
        public int? GroupSubProcessId { get; set; }
        public string GSizeRefId { get; set; }
        public string FSizeRefId { get; set; }
        public Nullable<decimal> ShadePerc { get; set; }
        public Nullable<decimal> CostRate { get; set; }
        public Nullable<decimal> BillRate { get; set; }
  
        public Nullable<int> ConsumptionGroupId { get; set; }
        public string BuyerRefId { get; set; }
        public string OrderNo { get; set; }
        public string OrderStyleRefId { get; set; }
        [Required(ErrorMessage = CustomErrorMessage.RequiredErrorMessage)]
        public string GrColorRefId { get; set; }
        public string ApprovedLdNo { get; set; }
        public DateTime? LoadingDateTime { get; set; }
        public DateTime? UnLoadingDateTime { get; set; }
        public string Gsm { get; set; }
        public string Remarks { get; set; }
        [Required(ErrorMessage = CustomErrorMessage.RequiredErrorMessage)]
        public string JobRefId { get; set; }
        public virtual Color Color { get; set; }
        public virtual Party Party { get; set; }
        public virtual Production_Machine Production_Machine { get; set; }
        public virtual ICollection<PROD_BatchDetail> PROD_BatchDetail { get; set; }
        public virtual ICollection<PROD_DyeingSpChallanDetail> PROD_DyeingSpChallanDetail { get; set; }
        public virtual ICollection<Inventory_FinishFabDetailStore> Inventory_FinishFabDetailStore { get; set; }
        public virtual ICollection<PROD_BatchRoll> PROD_BatchRoll { get; set; }
    }
}
