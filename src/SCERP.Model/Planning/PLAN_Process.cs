using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SCERP.Model.Planning
{
    public partial class PLAN_Process : SearchModel<PLAN_Process>
    {
        public PLAN_Process()
        {
            this.PLAN_ProcessTemplate = new HashSet<PLAN_ProcessTemplate>();
        }
        public int ProcessId { get; set; }
        public string CompId { get; set; }
        public string ProcessRefId { get; set; }
        [Required(ErrorMessage = @"Code  Missing!")]
        public string ProcessCode { get; set; }
        [Required(ErrorMessage = @"Process  Missing!")]
        [Remote("CheckExistingProcess", "Process", HttpMethod = "POST", AdditionalFields = "ProcessId", ErrorMessage = @"Process already exists!")]
        public string ProcessName { get; set; }
        [Required(ErrorMessage = @"BufferDay  Missing!")]
        public Nullable<int> BufferDay { get; set; }
        public Nullable<bool> IsRelative { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.Guid> CreatedBy { get; set; }
        public Nullable<System.DateTime> EditedDate { get; set; }
        public Nullable<System.Guid> EditedBy { get; set; }
        public bool IsActive { get; set; }

        public virtual ICollection<PLAN_ProcessTemplate> PLAN_ProcessTemplate { get; set; }
    }
}
