using System.Collections.Generic;
using SCERP.Model;
namespace SCERP.Web.Areas.HRM.Models.ViewModels
{
    public class GenderViewModel:Gender
    {
        public List<Gender> Genders { get; set; }
        public string SearchByTitle { get; set; }
        public GenderViewModel()
        {
            Genders=new List<Gender>();
        }
    }
}