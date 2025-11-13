using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Common;

namespace SCERP.Model.AccountingModel
{
    public class Acc_StylePayment:SearchModel<Acc_StylePayment>
    {
        public int StylePaymnetId { get; set; }
       
        public string StylePaymentRefId { get; set; }
        [Required(ErrorMessage = CustomErrorMessage.RequiredErrorMessage)]
        public Nullable<System.DateTime> PayDate { get; set; }
        [Required(ErrorMessage = CustomErrorMessage.RequiredErrorMessage)]
        public string BuyerRefId { get; set; }
        [Required(ErrorMessage = CustomErrorMessage.RequiredErrorMessage)]
        public string OrderNo { get; set; }
        [Required(ErrorMessage = CustomErrorMessage.RequiredErrorMessage)]
        public string OrderStyleRefId { get; set; }
        [Required(ErrorMessage = CustomErrorMessage.RequiredErrorMessage)]
        public string CostGroup { get; set; }
        [Required(ErrorMessage = CustomErrorMessage.RequiredErrorMessage)]
        public double PayAount { get; set; }
        [Required(ErrorMessage = CustomErrorMessage.RequiredErrorMessage)]
        public string CompId { get; set; }
        public string Remarks { get; set; }
    }
}
