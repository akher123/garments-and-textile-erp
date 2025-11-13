using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCERP.Model;
using SCERP.Model.CommercialModel;
using System.Collections;

namespace SCERP.Web.Areas.Commercial.Models.ViewModel
{
    public class BankAdviceViewModel : CommBankAdvice
    {
        public BankAdviceViewModel()
        {
            Exports = new List<CommExport>();
            BankAdvice = new CommBankAdvice();
            BankAdvices = new List<CommAccHead>();
            BankAdviceFixeds = new List<CommAccHead>();
            IsSearch = true;
        }

        public string Key { get; set; }    
        public List<CommExport> Exports { get; set; }
        public CommBankAdvice BankAdvice { get; set; }
        public List<CommAccHead> BankAdvices { get; set; }
        public List<CommAccHead> BankAdviceFixeds { get; set; }
    }
}