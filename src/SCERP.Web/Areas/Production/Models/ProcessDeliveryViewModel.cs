using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using SCERP.Model;
using SCERP.Model.CommonModel;
using SCERP.Model.Production;

namespace SCERP.Web.Areas.Production.Models
{
    public class ProcessDeliveryViewModel : ProSearchModel<ProcessDeliveryViewModel>
    {
        public ProcessDeliveryViewModel()
        {
            Deliveries = new List<VwProcessDelivery>();
            Delivery = new PROD_ProcessDelivery();
            CuttingBatch = new PROD_CuttingBatch();
            PartyList = new List<Party>();
            CuttingJobDictionary = new Dictionary<string, List<string>>();
            DeliveryDetails = new Dictionary<string, PROD_ProcessDeliveryDetail>();
           
            DoDictionary = new Dictionary<string, Dictionary<string, PROD_ProcessDeliveryDetail>>();
            Buyers = new List<OM_Buyer>();
            OrderList = new List<object>();
            Styles = new List<object>();
            Colors = new List<object>();
            Components = new List<OM_Component>();
            CuttiongProcesses=new List<PartyWiseCuttiongProcess>();
        }

        public int DeliveryType { get; set; }
        public IEnumerable Components { get; set; }
        public List<OM_Buyer> Buyers { get; set; }
        public IEnumerable OrderList { get; set; }
        public IEnumerable Styles { get; set; }
        public IEnumerable Colors { get; set; }
        public Dictionary<string, Dictionary<string, PROD_ProcessDeliveryDetail>> DoDictionary { get; set; }
   
        public List<PartyWiseCuttiongProcess> CuttiongProcesses { get; set; }
        public Dictionary<string, PROD_ProcessDeliveryDetail> DeliveryDetails { get; set; }
        public Dictionary<string, List<string>> CuttingJobDictionary { get; set; }
        public List<Party> PartyList { get; set; }
        public long CuttingTagId { get; set; }
        [Required(ErrorMessage = @"Required")]
        public string ColorRefId { get; set; }
          [Required(ErrorMessage = @"Required")]
        public string ComponentRefId { get; set; }
        public PROD_CuttingBatch CuttingBatch { get; set; }
        public PROD_ProcessDelivery Delivery { get; set; }
        public List<VwProcessDelivery> Deliveries { get; set; }
        public IEnumerable<SelectListItem> ComponentSelectListItem
        {
            get
            {
                return new SelectList(Components, "ComponentRefId", "ComponentName");
            }
        }
        public IEnumerable<SelectListItem> PartySelectListItem
        {
            get
            {
                return new SelectList(PartyList, "PartyId", "Name");
            }
        }
        public IEnumerable<SelectListItem> BuyerSelectListItem
        {
            get
            {
                return new SelectList(Buyers, "BuyerRefId", "BuyerName");
            }
        }
        public IEnumerable<SelectListItem> OrderSelectListItem
        {
            get
            {
                return new SelectList(OrderList, "OrderNo", "RefNo");
            }
        }
        public IEnumerable<SelectListItem> StylesSelectListItem
        {
            get
            {
                return new SelectList(Styles, "OrderStyleRefId", "StyleName");
            }
        }
        public IEnumerable<SelectListItem> ColorSelectListItem
        {
            get
            {
                return new SelectList(Colors, "ColorRefId", "ColorName");
            }
        }
    }
}