using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.HRMModel
{
    public class HrmMaternityPayment : SearchModel<HrmMaternityPayment>
    {
        public int MaternityPaymentId { get; set; }
        public System.Guid EmployeeId { get; set; }

        [DataType(DataType.Date)]
        public Nullable<System.DateTime> LeaveDayStart { get; set; }

        [DataType(DataType.Date)]
        public Nullable<System.DateTime> LeaveDayEnd { get; set; }

        [DataType(DataType.Date)]
        public Nullable<System.DateTime> FirstPaymentDate { get; set; }

        public Nullable<decimal> FirstPaymentAmount { get; set; }

        [DataType(DataType.Date)]
        public Nullable<System.DateTime> SecondPaymentDate { get; set; }

        public Nullable<decimal> SecondPaymentAmount { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.Guid> CreatedBy { get; set; }
        public Nullable<System.DateTime> EditedDate { get; set; }
        public Nullable<System.Guid> EditedBy { get; set; }
        public bool IsActive { get; set; }
        public string CompId { get; set; }
    }
}
