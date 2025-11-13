using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Common;

namespace SCERP.Model
{
    public class SalarySetup : SearchModel<SalarySetup>
    {
        public int Id { get; set; }

        [Required(ErrorMessage = CustomErrorMessage.RequiredErrorMessage)]
        public int EmployeeGradeId { get; set; }

        [Required(ErrorMessage = CustomErrorMessage.RequiredErrorMessage)]
        public decimal GrossSalary { get; set; }

        [Required(ErrorMessage = CustomErrorMessage.RequiredErrorMessage)]
        public decimal BasicSalary { get; set; }

        public decimal HouseRent { get; set; }
        public decimal MedicalAllowance { get; set; }
        public decimal Conveyance { get; set; }
        public decimal? FoodAllowance { get; set; }
        public decimal? EntertainmentAllowance { get; set; }
        public string Description { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? EditedDate { get; set; }
        public Guid? EditedBy { get; set; }

        [Required(ErrorMessage = CustomErrorMessage.RequiredErrorMessage)]
        [DataType(DataType.Date)]
        public DateTime? FromDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? ToDate { get; set; }
        [Required(ErrorMessage = CustomErrorMessage.RequiredErrorMessage)]

        public bool IsActive { get; set; }

        public virtual EmployeeGrade EmployeeGrade { get; set; }

    }
}
