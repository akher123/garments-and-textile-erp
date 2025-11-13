using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model
{
    public partial class BloodGroup:BaseModel
    {
        public BloodGroup()
        {
            this.Employee = new HashSet<Employee>();
        }
    
        public int Id { get; set; }

        public string GroupName { get; set; }
        public string Description { get; set; }
        public Nullable<DateTime> CDT { get; set; }
        public Nullable<Guid> CreatedBy { get; set; }
        public Nullable<DateTime> EDT { get; set; }
        public Nullable<Guid> EditedBy { get; set; }
        public bool IsActive { get; set; }

        public virtual ICollection<Employee> Employee { get; set; }
    }
}
