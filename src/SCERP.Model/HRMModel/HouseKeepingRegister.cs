using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.HRMModel
{
    public partial class HouseKeepingRegister
    {
        public int HouseKeepingRegisterId { get; set; }
        public int HouseKeepingItemId { get; set; }
        [Required]
        public Nullable<double> Quantity { get; set; }
        [Required]
        public Nullable<double> Rate { get; set; }
        [Required]
        public System.Guid EmployeeId { get; set; }
        [Required]
        public Nullable<System.DateTime> IusseDate { get; set; }
        public string Remarks { get; set; }
        public double ReturnQty { get; set; }
        public Nullable<System.DateTime> ReturnDate { get; set; }
        public string CompId { get; set; }

        public virtual HouseKeepingItem HouseKeepingItem { get; set; }
        public virtual Employee Employee { get; set; }
    }
}
