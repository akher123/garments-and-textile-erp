using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Common;

namespace SCERP.Model.Production
{
    public class VProBatch : ProSearchModel<VProBatch>
    {
     
    

        public long BatchId { get; set; }
        public Nullable<int> BtType { get; set; }
       [Required(ErrorMessage = CustomErrorMessage.RequiredErrorMessage)]
        public string BatchNo { get; set; }
        public string BtRefNo { get; set; }
        public Nullable<decimal> BatchQty { get; set; }
        [Required(ErrorMessage = CustomErrorMessage.RequiredErrorMessage)]
     
        public Nullable<System.DateTime> BatchDate { get; set; }
        [Required(ErrorMessage = CustomErrorMessage.RequiredErrorMessage)]
        public int BatchStatus { get; set; }
        public string CompId { get; set; }
           [Required(ErrorMessage = CustomErrorMessage.RequiredErrorMessage)]
        public Nullable<int> ItemId { get; set; }
        public string ItemName { get; set; }
        public string ItemCode { get; set; }
        [Required(ErrorMessage = CustomErrorMessage.RequiredErrorMessage)]
        public string Gsm { get; set; }
           [Required(ErrorMessage = CustomErrorMessage.RequiredErrorMessage)]
        public string GrColorRefId { get; set; }
        [Required(ErrorMessage = CustomErrorMessage.RequiredErrorMessage)]
        public string GColorName { get; set; }
        public string GSizeRefId { get; set; }
        public string GSizeName { get; set; }
        public string FColorRefId { get; set; }
        public string FColorName { get; set; }
        public string FSizeRefId { get; set; }
        public string FSizeName { get; set; }
        public Nullable<decimal> CostRate { get; set; }
        [Required(ErrorMessage = CustomErrorMessage.RequiredErrorMessage)]
        public Nullable<decimal> BillRate { get; set; }
        [Required(ErrorMessage = CustomErrorMessage.RequiredErrorMessage)]
        public Nullable<decimal> ShadePerc { get; set; }
        [Required(ErrorMessage = CustomErrorMessage.RequiredErrorMessage)]
        public string OrderStyleRefId { get; set; }
        public string BuyerName { get; set; }
        public string OrderName { get; set; }
        public string StyleName { get; set; }
        [Required(ErrorMessage = CustomErrorMessage.RequiredErrorMessage)]
        public string BuyerRefId { get; set; }
        [Required(ErrorMessage = CustomErrorMessage.RequiredErrorMessage)]
        public string OrderNo { get; set; }
        public Nullable<int> ConsumptionGroupId { get; set; }
           [Required(ErrorMessage = CustomErrorMessage.RequiredErrorMessage)]
        public long PartyId { get; set; }
        public string PartyName { get; set; }
        public string PartyRefNo { get; set; }
        public string MachineName { get; set; }
           [Required(ErrorMessage = CustomErrorMessage.RequiredErrorMessage)]
        public int MachineId { get; set; }
        public Nullable<long> ColorId { get; set; }
        [Required(ErrorMessage = CustomErrorMessage.RequiredErrorMessage)]
        public DateTime? LoadingDateTime { get; set; }
        public DateTime? UnLoadingDateTime { get; set; }
        public string ColorRef { get; set; }
        public string ColorName { get; set; }
        public string ApprovedLdNo { get; set; }
        public string Remarks { get; set; }
        public string JobRefId { get; set; }
        public long ProgramId { get; set; }
        


    }
}
