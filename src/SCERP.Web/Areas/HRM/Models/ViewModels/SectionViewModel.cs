using System.Collections.Generic;
using System.Web.Mvc;
using SCERP.Model;

namespace SCERP.Web.Areas.HRM.Models.ViewModels
{
    public class SectionViewModel : Section
    {
        public SectionViewModel()
        {
            SectionList = new List<Section>();
            Branches = new List<Branch>();
            Departments = new List<Department>();
            IsSearch = true;
        }
        public int SearchDepartmentId { get; set; }
      
        public List<Department> Departments { get; set; }
        public List<Section> SectionList { get; set; }
        public List<Branch> Branches { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> DepartmentSelectListItem
        {
            get { return new SelectList(Departments, "Id", "Title"); }
        }

        public string SearchKey
        {
            get;
            set;
        }
     
    }
}