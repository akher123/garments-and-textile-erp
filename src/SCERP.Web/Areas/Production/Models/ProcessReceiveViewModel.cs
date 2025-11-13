using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCERP.Model.CommonModel;
using SCERP.Model.Production;

namespace SCERP.Web.Areas.Production.Models
{
    public class ProcessReceiveViewModel : ProSearchModel<ProcessReceiveViewModel>
    {
        public Dictionary<string, Dictionary<string, PROD_ProcessReceiveDetail>> DoDictionary { get; set; }
        public Dictionary<string, List<string>> Dictionary { get; set; }
        public Dictionary<string, PROD_ProcessReceiveDetail> ReceiveDictionary { get; set; }
        public PROD_ProcessReceiveDetail ReceiveDetail { get; set; }
        public List<PROD_ProcessReceive> Receives { get; set; }
        public PROD_ProcessReceive Receive { get; set; }
        public List<Party> Parties { get; set; }
        public List<SpPodProcessReceiveBalance> ReceiveBalances { get; set; }
        public string CuttingBatchRefId { get; set; }
        public string TagName { get; set; }
        public ProcessReceiveViewModel()
        {
            DoDictionary=new Dictionary<string, Dictionary<string, PROD_ProcessReceiveDetail>>();
            ReceiveDictionary=new Dictionary<string, PROD_ProcessReceiveDetail>();
            Dictionary=new Dictionary<string, List<string>>();
            ReceiveDetail=new PROD_ProcessReceiveDetail();
            Receives=new List<PROD_ProcessReceive>();
            Receive=new PROD_ProcessReceive();
            Parties=new List<Party>();
            ReceiveBalances=new List<SpPodProcessReceiveBalance>();
          
        }
        public IEnumerable<SelectListItem> PartySelectListItem
        {
            get
            {
                return new SelectList(Parties, "PartyId", "Name");
            }
        }
    }
}