using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCERP.Model;
using SCERP.Model.Production;

namespace SCERP.Web.Areas.MIS.Models.ViewModel
{
    public class MisDashboardViewModel : ProSearchModel<MisDashboardViewModel>
    {
        public DataTable MerchadiserWiseOrderStyleDtable { get; set; }
        public DataTable BuyerWiseOrderStyleDtable { get; set; }
        public DataTable SummaryDataTable { get; set; }
        public DataTable BuyerOrderMasterDataTable { get; set; }
        public VwProductionForecast ProductionForecastCurrentMonth { get; set; }
        public VwProductionForecast ProductionForecastPreviousMonth{ get; set; }
        public int CurrentYear { get; set; }
        public DateTime FilterDate { get; set; }
        public string BuyerRefId { get; set; }
        public string ColoumnName { get; set; }
        public string OrderNo { get; set; }
        public IEnumerable OrderList { get; set; }
        public IEnumerable BuyerOrderStyles { get; set; }
        public string OrderStyleRefId { get; set; }
        public List<OM_Buyer> Buyers { get; set; }
        public List<OM_BuyerOrder> BuyerOrders { get; set; }
        public List<OM_Style> Styles { get; set; }
        public string ViewType { get; set; }

        public int YearId { get; set; }
        public MisDashboardViewModel()
        {
            SummaryDataTable=new DataTable();
            MerchadiserWiseOrderStyleDtable=new DataTable();
            BuyerWiseOrderStyleDtable=new DataTable();
            BuyerOrderMasterDataTable = new DataTable();
            ProductionForecastCurrentMonth = new VwProductionForecast();
            ProductionForecastPreviousMonth = new VwProductionForecast();
            Buyers = new List<OM_Buyer>();
            BuyerOrders=new List<OM_BuyerOrder>();
            Styles=new List<OM_Style>();
            OrderList = new List<object>();
            BuyerOrderStyles = new List<object>();
            this.YearId = DateTime.Now.Year;
        }
        public IEnumerable<SelectListItem> BuyerSelectListItem
        {
            get
            {
                return new SelectList(Buyers, "BuyerRefId", "BuyerName");
            }
        }

        

        public List<SelectListItem> BuyerOrderSelectListItem
        {
            get { return new SelectList(BuyerOrders, "OrderNo", "OrderNo").ToList(); }
        }


        public IEnumerable<SelectListItem> YearSelectListItem
        {
            get
            {
                return new SelectList(Enumerable.Range(2015, (DateTime.Now.Year - 2015) + 1));
            }
        }
        public List<SelectListItem> StyleSelectListItem
        {
            get { return new SelectList(Styles, "StylerefId", "StyleName").ToList(); }
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
                return new SelectList(BuyerOrderStyles, "OrderStyleRefId", "StyleName");
            }
        }

    }
}