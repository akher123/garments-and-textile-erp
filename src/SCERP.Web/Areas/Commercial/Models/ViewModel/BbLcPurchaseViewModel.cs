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
    public class BbLcPurchaseViewModel : CommBbLcPurchaseCommon
    {
        public BbLcPurchaseViewModel()
        {
            CommBbLcPurchaseCommons = new List<CommBbLcPurchaseCommon>();
            VWCommBbLcPurchase = new List<VwCommBbLcPurchase>();
            VwBbLcPurchaseCommon = new List<VwBbLcPurchaseCommon>();
            IsSearch = true;
        }

        public string LcNo { get; set; }
        public List<CommBbLcPurchaseCommon> CommBbLcPurchaseCommons { get; set; }
        public List<VwCommBbLcPurchase> VWCommBbLcPurchase { get; set; }
        public List<VwBbLcPurchaseCommon> VwBbLcPurchaseCommon { get; set; }
    }
}