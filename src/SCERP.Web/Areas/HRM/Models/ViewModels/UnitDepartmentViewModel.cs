using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using SCERP.Model;

namespace SCERP.Web.Areas.HRM.Models.ViewModels
{
    public class UnitDepartmentViewModel : UnitDepartment
    {
        public UnitDepartmentViewModel()
        {
            Units = new List<Unit>();
            Departments = new List<Department>();
            UnitDepartments = new List<UnitDepartment>();
            IsSearch = true;
        }

        public List<Unit> Units { get; set; }
        public List<UnitDepartment> UnitDepartments { get; set; }
        public List<Department> Departments { get; set; }
        [Required(ErrorMessage = @"Required")]
        public int SearchByUnitId { get; set; }
        [Required(ErrorMessage = @"Required")]
        public int SearchByDepartmentId{ get; set; }



        public List<SelectListItem> UnitSelectListItem
        {
            get { return new SelectList(Units, "UnitId", "Name").ToList(); }

        }
        public List<SelectListItem> DepartmentSelectListItem
        {
            get { return new SelectList(Departments, "Id", "Name").ToList(); }
        }
       
    }
}