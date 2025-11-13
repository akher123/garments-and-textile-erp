using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.UserRightManagementModel
{
    public class UserTnaResponsible
    {
        public int UserTnaResponsibleId { get; set; }
        public Guid EmployeeId { get; set; }
        public string Responsible { get; set; }
        public bool IsActive { get; set; }

    }
}
