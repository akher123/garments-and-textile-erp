using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model
{
    public class CompanyOrganogram
    {
        public List<CompanyOrganogram> CompanyOrganograms { get; set; }
        public CompanyOrganogram()
        {
            CompanyOrganograms = new List<CompanyOrganogram>();
        }

        public int Id { get; set; }
        public int DesignationId { get; set; }
        public Nullable<int> LevelId { get; set; }
        public Nullable<int> ParentDesignationId { get; set; }
        public Nullable<System.DateTime> CDT { get; set; }
        public Nullable<System.Guid> CreatedBy { get; set; }
        public Nullable<System.DateTime> EDT { get; set; }
        public Nullable<System.Guid> EditedBy { get; set; }
        public Nullable<bool> IsActive { get; set; }

        public virtual EmployeeDesignation EmployeeDesignation { get; set; }
    }
}
