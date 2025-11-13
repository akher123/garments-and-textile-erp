using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Common.PermissionModel
{
    public class UserDepartment
    {
        public UserDepartment()
        {
        }

        public int BranchUnitDepartmentId { get; set; }

        public int BranchUnitId { get; set; }

        public int DepartmentId { get; set; }

        public string DepartmentName { get; set; }

        //public ICollection<UserUnit> Units { get; set; }
    }
}
