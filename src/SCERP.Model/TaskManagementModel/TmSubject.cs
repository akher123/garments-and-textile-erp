using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace SCERP.Model.TaskManagementModel
{


    public partial class TmSubject : TmSearchModel<TmSubject>
    {
   
        public TmSubject()
        {
            this.TmTask = new HashSet<TmTask>();
        }
        public int SubjectId { get; set; }
        [Required]
        public string SubjectName { get; set; }
        public int ModuleId { get; set; }
        public string Description { get; set; }
        public string CompId { get; set; }
        public virtual TmModule TmModule { get; set; }
        public virtual ICollection<TmTask> TmTask { get; set; }
    }
}
