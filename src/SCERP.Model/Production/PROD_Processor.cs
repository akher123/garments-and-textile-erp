using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SCERP.Model.Production
{
    public class PROD_Processor : ProSearchModel<PROD_Processor>
   {
        public int ProcessorId { get; set; }
         [Required(ErrorMessage = @"Processor  Missing!")]
        public string ProcessRefId { get; set; }
        public string CompId { get; set; }
        [Required(ErrorMessage = @"Processor  Missing!")]
        public string ProcessorRefId { get; set; }
        [Required(ErrorMessage = @"Processor  Missing!")]
        [Remote("CheckExistingProcessor", "Processor", HttpMethod = "POST", AdditionalFields = "ProcessorId,ProcessRefId", ErrorMessage = @"Processor already exists!")]
        public string ProcessorName { get; set; }
    }
}
