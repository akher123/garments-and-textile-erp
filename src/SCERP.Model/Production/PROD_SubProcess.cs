using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SCERP.Model.Production
{
    public class PROD_SubProcess : ProSearchModel<PROD_SubProcess>
    {
        public long SubProcessId { get; set; }
        public string SubProcessRefId { get; set; }
        public string ProcessRefId { get; set; }
        public string SubProcessName{ get; set; }
        [Required(ErrorMessage = @"SubProcess  Missing!")]
        [Remote("CheckExistingSubProcess", "SubProcess", HttpMethod = "POST", AdditionalFields = "SubProcessId,SubProcessRefId", ErrorMessage = @"SubProcess already exists!")]
        public string CompId { get; set; }

    }
}
