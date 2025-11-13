using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using SCERP.Model;
using SCERP.Model.Production;

namespace SCERP.Web.Areas.Production.Models
{
    public class DailyFabricReceiveViewModel : ProSearchModel<PROD_DailyFabricReceive>
    {
        [Required(ErrorMessage = @"Required")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}",
               ApplyFormatInEditMode = true)]
        public DateTime ReceivedDate { get; set; }
        public List<OM_Buyer> OmBuyers { get; set; }
        public List<PROD_DailyFabricReceive> DailyFabricReceives { get; set; }
        public PROD_DailyFabricReceive DailyFabricReceive { get; set; }
        public VwReceivedFabricProductionSummary FabricProductionSummary { get; set; }
        public List<SpProdDailyFabricReceive> ReceivedFabricProductionSummaries { get; set; }
        public string OrderNo { get; set; }
        public string StyleName { get; set; }
        public string BuyerRefId { get; set; }
        public string ConsRefId { get; set; }
        public string ColorRefId { get; set; }
        public string ComponentRefId { get; set; }
        public string OrderStyleRefId { get; set; }
        public DailyFabricReceiveViewModel()
        {
            FabricProductionSummary=new VwReceivedFabricProductionSummary();
            OmBuyers=new List<OM_Buyer>();
            DailyFabricReceives=new List<PROD_DailyFabricReceive>();
            DailyFabricReceive=new PROD_DailyFabricReceive();
            ReceivedFabricProductionSummaries = new List<SpProdDailyFabricReceive>();
        }
        public IEnumerable<SelectListItem> BuyerSelectListItem
        {
            get
            {
                return new SelectList(OmBuyers, "BuyerRefId", "BuyerName");
            }
        }
    }
}