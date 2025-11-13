using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;
using SCERP.Model;
using SCERP.Model.CommonModel;
using SCERP.Model.Production;

namespace SCERP.Web.Areas.Production.Models
{
    public class KnittingRollViewModel : ProSearchModel<KnittingRollViewModel>
    {
        public string BuyerRefId { get; set; }
        public string OrderStyleRefId { get; set; }
        public string OrderNo { get; set; }
        public double TotalQty { get; set; }
        public string KType { get; set; }
        public Dictionary<string,VwKnittingRoll> Dictionary { get; set; }
        public VwKnittingRoll KnittingRoll { get; set; }
        public List<VwKnittingRoll> KnittingRolls { get; set; }
        public List<Party> Parties { get; set; }
        public List<Production_Machine> Machines { get; set; }
        public bool IsContinue { get; set; }
        public List<VwProduction> Productions { get; set; }
        public DataTable DataTable { get; set; }
        public List<OM_Buyer> BuyerList { get; set; }
        public IEnumerable OrderList { get; set; }
        public IEnumerable StyleList { get; set; }
        public int NoRoll { get; set; }
        public KnittingRollViewModel()
        {
            RollDictionary=new Dictionary<string, VwKnittingRoll>();
            KnittingRolls = new List<VwKnittingRoll>();
            Parties=new List<Party>();
            Machines=new List<Production_Machine>();
            Productions=new List<VwProduction>();
            KnittingRoll=new VwKnittingRoll();
            DataTable=new DataTable();
            BuyerList=new List<OM_Buyer>();
            OrderList=new List<Object>();
            StyleList = new List<Object>();
            Dictionary = new Dictionary<string, VwKnittingRoll>();
        }

        public Dictionary<string, VwKnittingRoll> RollDictionary { get; set; }
        public IEnumerable<SelectListItem> PartySelectListItem
        {
            get { return new SelectList(Parties, "PartyId", "Name"); }
        }
        public IEnumerable<SelectListItem> MachineSelectListItem
        {
            get { return new SelectList(Machines, "MachineId", "Name"); }
        }
        public IEnumerable<SelectListItem> BuyerSelectListItem
        {
            get
            {
                return new SelectList(BuyerList, "BuyerRefId", "BuyerName");
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
                return new SelectList(StyleList, "OrderStyleRefId", "StyleName");
            }
        }
    }
}