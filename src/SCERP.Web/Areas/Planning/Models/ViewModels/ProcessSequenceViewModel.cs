using System;
using System.Collections.Generic;
using System.Web.Mvc;
using SCERP.Model;
using SCERP.Model.MerchandisingModel;
using SCERP.Model.Planning;

namespace SCERP.Web.Areas.Planning.Models.ViewModels
{
    public class ProcessSequenceViewModel:PLAN_ProcessSequence
    {
        public List<VProcessSequence> ProcessSequences { get; set; }
        public IEnumerable<VBuyerOrder> BuyerOrders { get; set; }
        public List<VOMBuyOrdStyle> BuyOrdStyles { get; set; }
        public List<PLAN_Process> Processes { get; set; }
        public string OrderNo { get; set; }
        public ProcessSequenceViewModel()
        {
            Processes=new List<PLAN_Process>();
            ProcessSequences = new List<VProcessSequence>();
            BuyerOrders=new ArraySegment<VBuyerOrder>();
            BuyOrdStyles = new List<VOMBuyOrdStyle>();
        }

        public IEnumerable<SelectListItem> ProcessesSeasonsSelectListItem
        {
            get
            {
                return new SelectList(Processes, "ProcessRefId", "ProcessName");
            }
        }
    }
}