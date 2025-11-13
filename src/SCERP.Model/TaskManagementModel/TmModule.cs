using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SCERP.Model.TaskManagementModel
{
        public partial class TmModule : TmSearchModel<TmModule>
        {
            public TmModule()
            {
                this.TmSubject = new HashSet<TmSubject>();
                this.TmTask = new HashSet<TmTask>();
            }

            public int ModuleId { get; set; }
            [Required]
            public string ModuleName { get; set; }
            public string CompId { get; set; }

            public virtual ICollection<TmSubject> TmSubject { get; set; }
            public virtual ICollection<TmTask> TmTask { get; set; }
        }
 }
