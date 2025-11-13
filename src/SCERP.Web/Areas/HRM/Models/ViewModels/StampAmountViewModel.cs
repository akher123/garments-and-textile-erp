using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SCERP.Model;
using System.ComponentModel.DataAnnotations;

namespace SCERP.Web.Areas.HRM.Models.ViewModels
{
    public class StampAmountViewModel : StampAmount
    {
        public StampAmountViewModel()
        {
            StampAmounts = new List<StampAmount>();
            IsSearch = true;
        }

        public List<StampAmount> StampAmounts { get; set; }
        public decimal SearchByAmount { get; set; }

        [DataType(DataType.Date)]
        [Required(ErrorMessage = @"Required")]
        public Nullable<DateTime> SearchByFromDate { get; set; }

        [DataType(DataType.Date)]
        [Required(ErrorMessage = @"Required")]
        public Nullable<DateTime> SearchByToDate { get; set; }
    }
}