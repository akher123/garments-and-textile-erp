using System;
using System.ComponentModel.DataAnnotations;
using SCERP.Common;

namespace SCERP.Model.AccountingModel
{
   public partial class Acc_Currency
    {
        public int CurrencyId { get; set; }
        public string FirstCurName { get; set; }
        public decimal FirstCurValue { get; set; }
        public string FirstCurSymbol { get; set; }
        public string SecendCurName { get; set; }
        public decimal SecendCurValue { get; set; }
        public string SecendCurSymbol { get; set; }

        public string ThirdCurName { get; set; }
        public decimal ThirdCurValue { get; set; }
        public string ThirdCurSymbol { get; set; }
        [DataType(DataType.Date)]
        [Required(ErrorMessage = CustomErrorMessage.RequiredErrorMessage)]
        public DateTime? CurDate { get; set; }
        public bool ActiveStatus { get; set; }
       
    }
}
