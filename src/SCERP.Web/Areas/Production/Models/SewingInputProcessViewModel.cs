using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCERP.Model;
using SCERP.Model.Production;

namespace SCERP.Web.Areas.Production.Models
{
    public class SewingInputProcessViewModel : ProSearchModel<SewingInputProcessViewModel>
    {

        public SewingInputProcessViewModel()
        {

            SewingInputProcess=new PROD_SewingInputProcess();
            SewingInputDetails = new List<VwSewingInputDetail>();
            VwSewingInputProcessDetails=new List<VwSewingInputProcessDetail>();
            PivotDictionary=new Dictionary<string, List<string>>();
            SewingInputProcessDetailDictionary=new Dictionary<string, PROD_SewingInputProcessDetail>();
            Buyers = new List<Object>();
            OrderList = new List<object>();
            StyleList = new List<object>();
            Colors = new List<object>();
            Machines=new List<Production_Machine>();
            VwSewingInputProcesses=new List<VwSewingInputProcess>();
            HourList=new List<PROD_Hour>();
            OrderShips = new List<object>();
        }


        public long? TotalInput { get; set; }
        public string InputTime { get; set; }
        public int? RowNo { get; set; }
        public List<VwSewingInputProcess> VwSewingInputProcesses { get; set; } 
        public PROD_SewingInputProcess SewingInputProcess { get; set; }
        public List<VwSewingInputDetail> SewingInputDetails { get; set; }
        public List<VwSewingInputProcessDetail> VwSewingInputProcessDetails { get; set; }
        public Dictionary<string, List<string>> PivotDictionary { get; set; }
        public Dictionary<string, PROD_SewingInputProcessDetail> SewingInputProcessDetailDictionary { get; set; }
        public IEnumerable Buyers { get; set; }
        public IEnumerable OrderList { get; set; }
        public IEnumerable StyleList { get; set; }
        public IEnumerable Colors { get; set; }
        public List<Production_Machine> Machines { get; set; }
        public List<PROD_Hour> HourList { get; set; }
        public IEnumerable OrderShips { get; set; }
        public IEnumerable<SelectListItem> MachineSelectListItem
        {
            get
            {
                return new SelectList(Machines, "MachineId", "Name");
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
                return new SelectList(StyleList, "OrderStyleRefId", "StyleName");
            }
        }
        public IEnumerable<SelectListItem> ColorSelectListItem
        {
            get
            {
                return new SelectList(Colors, "ColorRefId", "ColorName");
            }
        }
        public IEnumerable<SelectListItem> HourSelectListItem
        {
            get
            {
                return new SelectList(HourList, "HourId", "HourName");
            }
        }

        public IEnumerable<SelectListItem> OrderShipSelectListItem
        {
            get
            {
                return new SelectList(OrderShips, "OrderShipRefId", "CountryName");
            }
        }
    }
}