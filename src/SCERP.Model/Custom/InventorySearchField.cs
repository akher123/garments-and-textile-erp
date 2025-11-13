using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.Custom
{
    public class InventorySearchField 
    {
       public int SearcbByGroupId{ get; set; }
       public int SearchBySubGroupId { get; set; }
            [Required(AllowEmptyStrings = true)]
       public int SearchByBranchId { get; set; }
       public int SearchByItemId { get; set; }
        [Required(AllowEmptyStrings = true)]
        public int SearchByCompanyId { get; set; }
           [DataType(DataType.Date)]
        public DateTime? FromDate { get; set; }
           [DataType(DataType.Date)]
        public DateTime? ToDate { get; set; }

    }
}
