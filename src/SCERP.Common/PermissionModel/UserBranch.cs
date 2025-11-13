using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Common.PermissionModel
{
    public sealed class UserBranch
    {
        public UserBranch()
        {
            this.Units = new HashSet<UserUnit>();
        }
        public int BranchId { get; set; }
        public string BranchName { get; set; }
        public int CompanyId { get; set; }

        public ICollection<UserUnit> Units { get; set; }


        public UserUnit Unit { get; set; }

    }
}
