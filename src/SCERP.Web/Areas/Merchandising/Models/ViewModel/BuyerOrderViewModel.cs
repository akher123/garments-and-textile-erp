using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;
using SCERP.Model;
using SCERP.Model.MerchandisingModel;

namespace SCERP.Web.Areas.Merchandising.Models.ViewModel
{
    public class BuyerOrderViewModel : VBuyerOrder
    {
        public OrderSheet OrderSheet { get; set; }
        public IEnumerable<VBuyerOrder> BuyerOrders { get; set; }
        public IEnumerable<VOM_BuyOrdStyle> BuyerOrderStyle { get; set; }
        public IEnumerable<VBuyerOrder> BuyerOrdersLc { get; set; }
        public IEnumerable<OM_Buyer> OmBuyers { get; set; }
        public IEnumerable<OM_Agent> OmAgents { get; set; }
        public IEnumerable<OM_Agent> OmSheAgents { get; set; }
        public IEnumerable<OM_Merchandiser> OmMerchandisers { get; set; }
        public IEnumerable<OM_Consignee> OmConsignees { get; set; }
        public IEnumerable<OM_OrderType> OmOrderTypes { get; set; }
        public IEnumerable<OM_PaymentTerm> OmPaymentTerms { get; set; }
        public IEnumerable<OM_Season> OmSeasons { get; set; }
        public DataTable OrderDetailsDataTable { get; set; }
        public string OrderStyleRefId { get; set; }
        public BuyerOrderViewModel()
        {
            BuyerOrders = new List<VBuyerOrder>();
            BuyerOrdersLc = new List<VBuyerOrder>();
            OmBuyers = new List<OM_Buyer>();
            OmAgents = new List<OM_Agent>();
            OmMerchandisers = new List<OM_Merchandiser>();
            OmConsignees = new List<OM_Consignee>();
            OmOrderTypes = new List<OM_OrderType>();
            OmPaymentTerms = new List<OM_PaymentTerm>();
            OmSeasons = new List<OM_Season>();
            OmSheAgents = new List<OM_Agent>();
            OrderSheet=new OrderSheet();
            OrderDetailsDataTable = new DataTable();
            BuyerOrderStyle = new List<VOM_BuyOrdStyle>();
        }

        public IEnumerable<SelectListItem> BuyerSelectListItem
        {
            get
            {
                return new SelectList(OmBuyers, "BuyerRefId", "BuyerName");
            }
        }
        public IEnumerable<SelectListItem> SheAgentSelectListItem
        {
            get
            {
                return new SelectList(OmSheAgents, "AgentRefId", "AgentName");
            }
        }
        public IEnumerable<SelectListItem> AgentsSelectListItem
        {
            get
            {
                return new SelectList(OmAgents, "AgentRefId", "AgentName");
            }
        }
        public IEnumerable<SelectListItem> MerchandisersSelectListItem
        {
            get
            {
                return new SelectList(OmMerchandisers, "EmpId", "EmpName");
            }
        }

        public IEnumerable<SelectListItem> ConsigneesSelectListItem
        {
            get
            {
                return new SelectList(OmConsignees, "ConsigneeRefId", "ConsigneeName");
            }
        }

        public IEnumerable<SelectListItem> OTypeSelectListItem
        {
            get
            {
                return new SelectList(OmOrderTypes, "OTypeRefId", "OTypeName");
            }
        }
        public IEnumerable<SelectListItem> PayTermSelectListItem
        {
            get
            {
                return new SelectList(OmPaymentTerms, "PayTermRefId", "PayTerm");
            }
        }
        public IEnumerable<SelectListItem> SeasonSelectListItem
        {
            get
            {
                return new SelectList(OmSeasons, "SeasonRefId", "SeasonName");
            }
        }

        public IEnumerable<SelectListItem> ShipModeSelectListItem
        {
            get
            {
                return new SelectList(new[]{"SEA","AIR"});
            }
        }

        public IEnumerable<SelectListItem> SubContactSelectListItem
        {
            get
            {
                return new SelectList(new[] { new { Id = "N", Value = "CONFIRM ORDER" }, new { Id = "Y", Value = "SUBCONTACT" }, new { Id = "S", Value = "SAMPLE ORDER" } }, "Id", "Value");
            }
        }
        public IEnumerable<SelectListItem> StatusSelectListItem
        {
            get
            {
                return new SelectList(new[] { new { Closed = "O", Value = "OPEN" }, new { Closed = "C", Value = "CLOSED" } }, "Closed", "Value");
            }
        }

        public IEnumerable<SelectListItem> OrderStatusSelectListItem
        {
            get
            {
                return new SelectList(new[] { new { SystemRefId = "Grey", Value = "ALL OK" }, new { SystemRefId = "Green", Value = "Confirm" }, new { SystemRefId = "Red", Value = "50% OK" }, new { SystemRefId = "Yellow", Value = "(70-80)% OK" } }, "SystemRefId", "Value");
            }
        }
    }
}