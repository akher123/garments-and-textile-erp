using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SCERP.Model;

namespace SCERP.Web.Areas.HRM.Models.ViewModels
{
    public class BankAccountTypeViewModel:BankAccountType
    {
        public List<BankAccountType> BankAccountTypes { get; set; }

        public BankAccountTypeViewModel()
        {
            BankAccountTypes=new List<BankAccountType>();
        }
        public string SearchKeyAccountType { get; set; }
    }
}