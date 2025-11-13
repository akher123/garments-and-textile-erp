using System;
using System.Collections.Generic;
using System.Web.Mvc;
using SCERP.Model;
using SCERP.Model.MerchandisingModel;
using SCERP.Model.Planning;
using SCERP.Model.Production;

namespace SCERP.Web.Areas.Production.Models
{
    public class StanderdMinValueViewModel:PROD_StanderdMinValue
    {
        public string Key { get; set; }
        public string ProcessRefId { get; set; }
        public List<PROD_SubProcess> SubProcesses { get; set; }
        public Dictionary<string, VwStanderdMinValDetail> SmvDtls { get; set; }
        public List<PROD_StanderdMinValue> SmvcList { get; set; }
        public VwStanderdMinValDetail SmvDetail { get; set; }
    
        public IEnumerable<VBuyerOrder> BuyerOrders { get; set; }
        public List<VOMBuyOrdStyle> BuyOrdStyles { get; set; }
        public string OrderNo { get; set; }
        public StanderdMinValueViewModel()
        {
            SubProcesses=new List<PROD_SubProcess>();

            SmvDtls = new Dictionary<string, VwStanderdMinValDetail>();
            SmvDetail = new VwStanderdMinValDetail();
            BuyerOrders = new ArraySegment<VBuyerOrder>();
            BuyOrdStyles = new List<VOMBuyOrdStyle>();
            SmvcList=new List<PROD_StanderdMinValue>();
        }

        public IEnumerable<SelectListItem> ProcessesSeasonsSelectListItem
        {
            get
            {
                return new SelectList(SubProcesses, "SubProcessRefId", "SubProcessName");
            }
        }
    }
}