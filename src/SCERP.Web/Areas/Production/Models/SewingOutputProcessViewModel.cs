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
    public class SewingOutputProcessViewModel : ProSearchModel<SewingInputProcessViewModel>
    {
        public SewingOutputProcessViewModel()
        {
            VwSewingOutputs=new List<VwSewingOutput>();
            SewingOutputDetails = new List<VwSewingOutputDetail>();
            SewingOutPutProcess = new PROD_SewingOutPutProcess();
            PivotDictionary=new Dictionary<string, List<string>>();
            SewingOutputProcessDetailDictionary = new Dictionary<string, PROD_SewingOutPutProcessDetail>();
            Buyers = new List<Object>();
            OrderList = new List<object>();
            StyleList = new List<object>();
            Colors = new List<object>();
            Machines=new List<Production_Machine>();
            HourList=new List<PROD_Hour>();
            VwSewingOutputProcesses = new List<VwSewingOutputProcess>();
            DataTable = new DataTable(); 
            OrderShips = new List<object>();
        }

        public bool IsReload { get; set; }
        public DataTable DataTable { get; set; }
        public long? TotalQty { get; set; }
         public int? RowNo { get; set; }
         public List<VwSewingOutputProcess> VwSewingOutputProcesses { get; set; } 
        public PROD_SewingOutPutProcess SewingOutPutProcess { get; set; }
        public List<VwSewingOutput> VwSewingOutputs { get; set; }
        public List<VwSewingOutputDetail> SewingOutputDetails { get; set; }
        public Dictionary<string, List<string>> PivotDictionary { get; set; }
        public Dictionary<string, PROD_SewingOutPutProcessDetail> SewingOutputProcessDetailDictionary { get; set; } 
        public IEnumerable Buyers { get; set; }
        public IEnumerable OrderList { get; set; }
        public IEnumerable StyleList { get; set; }
        public IEnumerable Colors { get; set; }
        public List<Production_Machine> Machines { get; set; }
        public List<PROD_Hour> HourList { get; set; } 
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
        public IEnumerable OrderShips { get; set; }
        public IEnumerable<SelectListItem> OrderShipSelectListItem
        {
            get
            {
                return new SelectList(OrderShips, "OrderShipRefId", "CountryName");
            }
        }
    }
}
    