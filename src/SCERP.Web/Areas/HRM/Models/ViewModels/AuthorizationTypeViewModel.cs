using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SCERP.Model;

namespace SCERP.Web.Areas.HRM.Models.ViewModels
{
    public class AuthorizationTypeViewModel : AuthorizationType
    {
        public string SearchByTypeName { get; set; }
        public List<AuthorizationType> AuthorizationTypes { get; set; }
        public AuthorizationTypeViewModel()
        {
            AuthorizationTypes=new List<AuthorizationType>();
        }

    }
}