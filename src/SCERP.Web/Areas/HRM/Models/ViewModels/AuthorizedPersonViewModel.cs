using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCERP.Common;
using SCERP.Model;

namespace SCERP.Web.Areas.HRM.Models.ViewModels
{
    public class AuthorizedPersonViewModel : AuthorizedPerson
    {
        public List<AuthorizedPerson> AuthorizedPersons { get; set; }
        public VEmployeeCompanyInfoDetail EmployeeCompanyInfo { get; set; }
        public AuthorizedPersonViewModel()
        {
            EmployeeCompanyInfo = new VEmployeeCompanyInfoDetail();
            AuthorizedPersons = new List<AuthorizedPerson>();
            AuthorizationTypes = new List<SCERP.Model.AuthorizationType>();
           
        }
        public IEnumerable<System.Web.Mvc.SelectListItem> AuthorizationTypeSelectListItem
        {
            get { return new SelectList(AuthorizationTypes, "Id","TypeName"); }
        }
        public List<SCERP.Model.AuthorizationType> AuthorizationTypes { get; set; }
        [Required]
        public string SearchByAuthorizedPerson{get; set;}
        [Required]
        public int SearchByAuthorizationTypeId { get; set; }
        [Display(Name = @"Employee Id")]
        [Required(ErrorMessage = CustomErrorMessage.RequiredErrorMessage)]
        public string EmployeeCardId {  get; set;}
   
    }
}