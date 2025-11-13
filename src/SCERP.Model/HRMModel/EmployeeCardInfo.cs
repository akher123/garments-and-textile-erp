using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model
{
    public partial class EmployeeCardInfo
    {
        public int CardSerialId { get; set; }
        public int CompanyId { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Telephone { get; set; }
        public string Fax { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public bool IsBangla { get; set; }
        public bool IsActive { get; set; }

        public virtual Company Company { get; set; }
    }
}
