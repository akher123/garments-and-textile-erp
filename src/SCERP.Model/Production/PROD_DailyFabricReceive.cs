using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.Production
{
    public class PROD_DailyFabricReceive
    {
        public long FabricReceiveId { get; set; }
        public string CompId { get; set; }
        public string OrderStyleRefId { get; set; }
        public string ConsRefId { get; set; }
        public string ColorRefId { get; set; }
        public string ComponentRefId { get; set; }
        [Required(ErrorMessage =@"Required!!")]
        public decimal FabricQty { get; set; }
         [Required(ErrorMessage = @"Required!!")]
        public System.DateTime ReceivedDate { get; set; }
        public Nullable<System.Guid> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.Guid> EditedBy { get; set; }
        public Nullable<System.DateTime> EditedDate { get; set; }
    }
}
