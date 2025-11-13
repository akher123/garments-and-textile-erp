using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Common;

namespace SCERP.Model
{
    public class EmployeeDesignationViewModel
    {
        [Required(ErrorMessage = CustomErrorMessage.RequiredErrorMessage)]
        public int DesignationId { get; set; }

        public string Title { get; set; }

        public int SupervisorDesignationId { get; set; }

       [Required(ErrorMessage = CustomErrorMessage.RequiredErrorMessage)]
        public int EmployeeLevelId { get; set; }
    }
}
