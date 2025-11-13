using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace SCERP.Common.PermissionModel
{
    public class PermissionContext
    {

        public PermissionContext()
        {
            this.CompanyList = new List<UserCompany>();
            this.BranchList = new List<UserBranch>();
            this.UnitList = new List<UserUnit>();
            this.DepartmentList = new List<UserDepartment>();
        }

        public List<UserCompany> CompanyList { get; set; }

        public List<UserBranch> BranchList { get; set; }

        public List<UserUnit> UnitList { get; set; }

        public List<UserDepartment> DepartmentList { get; set; }

        public List<UserEmployeeType> EmployeeTypeList { get; set; }

        public int[] CompanyArray { get; set; }
    }

}
