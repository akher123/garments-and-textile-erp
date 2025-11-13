using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Common.PermissionModel
{
    public sealed class UserUnit
    {
        public UserUnit()
        {
            this.Departments = new HashSet<UserDepartment>();
            this.Department = new UserDepartment();
        }

        public int BranchUnitId { get; set; }

        public int BranchId { get; set; }

        public int UnitId { get; set; }

        public string UnitName { get; set; }

        public ICollection<UserDepartment> Departments { get; set; }

        public UserDepartment Department { get; set; }
    
    }
}
