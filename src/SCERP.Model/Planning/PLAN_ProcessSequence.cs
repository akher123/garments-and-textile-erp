using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.Production;

namespace SCERP.Model.Planning
{
    public class PLAN_ProcessSequence : ProSearchModel<PLAN_ProcessSequence>
   {
        public long ProcessSequenceId { get; set; }
        public string CompId { get; set; }
        [Required(ErrorMessage = @"Select Style")]
        public string OrderStyleRefId { get; set; }
        [Required(ErrorMessage = @"Required Process Row")]
        public int? ProcessRow { get; set; }
         [Required(ErrorMessage = @"Select Process")]
        public string ProcessRefId { get; set; }

    }
}
