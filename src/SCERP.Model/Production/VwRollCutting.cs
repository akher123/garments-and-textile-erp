using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Common;

namespace SCERP.Model.Production
{
    public class VwRollCutting
    {
        public long RollCuttingId { get; set; }
        public string CompId { get; set; }
        public string CuttingBatchRefId { get; set; }
        public long CuttingBatchId { get; set; }
        public string RollNo { get; set; }
        [Required(ErrorMessage = CustomErrorMessage.CommonErrorMessage)]
        public string ColorRefId { get; set; }
        public string BatchNo { get; set; }
        public Nullable<int> Quantity { get; set; }
        public Nullable<int> RollSart { get; set; }
        public Nullable<int> RollEnd { get; set; }
        public string Combo { get; set; }
        public string RollName { get; set; }
        public string ColorName { get; set; }

    }
}
