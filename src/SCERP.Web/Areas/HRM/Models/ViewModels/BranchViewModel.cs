using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SCERP.Model;

namespace SCERP.Web.Areas.HRM.Models.ViewModels
{
    public class BranchViewModel : Branch
    {

        public BranchViewModel()
        {
            Branches = new List<Branch>();
            IsSearch = true;
        }

        public List<Branch> Branches { get; set; }

        public string SearchByBranchName
        {
            get;
            set;
        }

        public int SearchByCompany
        {
            get;
            set;
        }
    }
}