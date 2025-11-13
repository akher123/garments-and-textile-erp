using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SCERP.Common;

namespace SCERP.Model.Production
{


    public partial class PROD_SewingOutPutProcess
    {
        public PROD_SewingOutPutProcess()
        {
            this.PROD_SewingOutPutProcessDetail = new HashSet<PROD_SewingOutPutProcessDetail>();
        }
        public long SewingOutPutProcessId { get; set; }
        public string SewingOutPutProcessRefId { get; set; }
        [Required(ErrorMessage = CustomErrorMessage.RequiredErrorMessage)]
        public int LineId { get; set; }
        [Required(ErrorMessage = CustomErrorMessage.RequiredErrorMessage)]
        public string BuyerRefId { get; set; }
        [Required(ErrorMessage = CustomErrorMessage.RequiredErrorMessage)]
        public string OrderNo { get; set; }
        [Required(ErrorMessage = CustomErrorMessage.RequiredErrorMessage)]
        public string OrderStyleRefId { get; set; }
        [Required(ErrorMessage = CustomErrorMessage.RequiredErrorMessage)]
        public string ColorRefId { get; set; }
        public System.Guid PreparedBy { get; set; }
        public string CompId { get; set; }
        [Required(ErrorMessage = CustomErrorMessage.RequiredErrorMessage)]
        public int HourId { get; set; }
        [StringLength(20, ErrorMessage = "maximum Length 20 characters.")]
        public string Remarks { get; set; }
        [Required]
        public DateTime OutputDate { get; set; }
        public int ManPower	 { get; set; }
        public string BatchNo { get; set; }
        public string JobNo { get; set; }
        [Required(ErrorMessage = "Required")]
        public string OrderShipRefId { get; set; }
        public virtual PROD_Hour PROD_Hour { get; set; }
        public virtual PROD_SewingInputProcess PROD_SewingInputProcess { get; set; }
        public virtual ICollection<PROD_SewingOutPutProcessDetail> PROD_SewingOutPutProcessDetail { get; set; }
    }
}
