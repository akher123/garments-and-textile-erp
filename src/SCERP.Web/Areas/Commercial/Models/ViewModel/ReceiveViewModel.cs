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
    public class ReceiveViewModel : CommReceive
    {
        public ReceiveViewModel()
        {
            ReceiveDetail = new CommReceiveDetail();
            Receives = new List<CommReceive>();
            ReceiveDetails = new Dictionary<string, CommReceiveDetail>();
            IsSearch = true;
        }
        public string Key { get; set; }
        public string BbLcNo { get; set; }
        public CommReceiveDetail ReceiveDetail { get; set; }
        public List<CommReceive> Receives { get; set; }
        public Dictionary<string, CommReceiveDetail> ReceiveDetails { get; set; }
    }
}