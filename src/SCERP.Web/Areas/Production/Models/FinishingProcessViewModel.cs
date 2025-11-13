using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using SCERP.Model;
using SCERP.Model.Production;

namespace SCERP.Web.Areas.Production.Models
{
    public class FinishingProcessViewModel : ProSearchModel<FinishingProcessViewModel>
    {
        public FinishingProcessViewModel()
        {
            VwFinishingProcessDetails = new List<VwFinishingProcessDetail>();
            FinishingProcess = new PROD_FinishingProcess();
            PivotDictionary = new Dictionary<string, List<string>>();
            FinishingProcessDetailDictionary = new Dictionary<string, PROD_FinishingProcessDetail>();
            Buyers = new List<Object>();
            OrderList = new List<object>();
            StyleList = new List<object>();
            Colors = new List<object>();
            HourList = new List<PROD_Hour>();
            VwFinishingProcess = new List<VwFinishingProcess>();
            DataTable = new DataTable();
            OrderShips = new List<object>();
        }

        public bool IsReload { get; set; }
        public DataTable DataTable { get; set; }
        public long? TotalQty { get; set; }
        public int? RowNo { get; set; }
        public List<VwFinishingProcess> VwFinishingProcess { get; set; }
        public PROD_FinishingProcess    FinishingProcess{ get; set; }
        public List<VwFinishingProcessDetail> VwFinishingProcessDetails { get; set; }
        public Dictionary<string, List<string>> PivotDictionary { get; set; }
        public Dictionary<string, PROD_FinishingProcessDetail> FinishingProcessDetailDictionary { get; set; }
        public IEnumerable Buyers { get; set; }
        public IEnumerable OrderList { get; set; }
        public IEnumerable StyleList { get; set; }
        public IEnumerable Colors { get; set; }

        public int Total
        {
            get
            {
                return VwFinishingProcess.Sum(x => x.InputQuantity.GetValueOrDefault());
            }
        }

        public List<PROD_Hour> HourList { get; set; }
     
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