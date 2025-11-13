using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Common.PermissionModel
{
    public sealed class UserCompany
    {
        public UserCompany()
        {
            this.Branches = new HashSet<UserBranch>();
        }


        public int CompanyId { get; set; }
        public string CompanyName { get; set; }

        public ICollection<UserBranch> Branches { get; set; }

        public UserBranch Branch { get; set; }

    }
}
