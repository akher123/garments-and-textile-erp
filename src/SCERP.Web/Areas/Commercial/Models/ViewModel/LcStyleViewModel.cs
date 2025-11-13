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
    public class LcStyleViewModel : COMMLcStyle
    {
        public LcStyleViewModel()
        {
            LcStyles = new List<COMMLcStyle>();
            vWLcStyles = new List<VwCommLcStyle>();
            IsSearch = true;
        }

        public string LcNo { get; set; }
        public List<COMMLcStyle> LcStyles { get; set; }
        public List<VwCommLcStyle> vWLcStyles { get; set; }
    }
}