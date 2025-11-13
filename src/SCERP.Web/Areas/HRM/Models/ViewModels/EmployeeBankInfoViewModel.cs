using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCERP.Model;

namespace SCERP.Web.Areas.HRM.Models.ViewModels
{

    public class EmployeeBankInfoViewModel:EmployeeBankInfo
    {
       public  EmployeeBankInfoViewModel()
        {
            EmployeeBankInfos=new List<EmployeeBankInfo>();
        }
        public List<EmployeeBankInfo> EmployeeBankInfos { get; set; }


        public List<BankAccountType> BankAccountTypes { get; set; }
        public List<SelectListItem> BankAccountTypeSelectListItem
        {
            get { return new SelectList(BankAccountTypes, "BankAccountTypeId", "AccountType").ToList(); }

        }
    }
}