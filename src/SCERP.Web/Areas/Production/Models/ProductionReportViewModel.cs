using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using SCERP.Common;
using SCERP.Model;
using SCERP.Model.CommonModel;
using SCERP.Model.Production;

namespace SCERP.Web.Areas.Production.Models
{
    public class ProductionReportViewModel : SearchModel<ProductionReportViewModel>
    {
        public ProductionReportViewModel()
        {
            DataTable=new DataTable();
            CuttingBatch=new PROD_CuttingBatch();
            Buyers=new List<OM_Buyer>();
            OrderList = new List<object>();
            Styles = new List<object>();
            Components = new List<object>();
            Parties=new List<Party>();
            YearId= DateTime.Now.Year;
        }

        public ReportType RptType { get; set; }
        public int YearId { get; set; }
        public MonthEnum MonthId { get; set; }
        public DateTime? FilterDate { get; set; }
        public string ProcessRefId { get; set; }
        public long CuttingTagId { get; set; }
        public string Key { get; set; }
        public string SizeRefId { get; set; }
        public long PartyId { get; set; }
        public IEnumerable Components { get; set; }
        public IEnumerable Buyers { get; set; }
        public IEnumerable OrderList { get; set; }
        public IEnumerable Styles { get; set; }
        public List<Party> Parties { get; set; }
        public PROD_CuttingBatch CuttingBatch { get; set; }
        public DataTable DataTable { get; set; }
        public IEnumerable<SelectListItem> PartySelectListItem
        {
            get
            {
                return new SelectList(Parties, "PartyId", "Name");
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
                return new SelectList(Styles, "OrderStyleRefId", "StyleNo");
            }
        }
     

        public IEnumerable<SelectListItem> ComponentSelectListItem
        {
            get
            {
                return new SelectList(Components, "ComponentRefId", "ComponentName");
            }
        }


        public IEnumerable<SelectListItem> YearSelectListItem
        {
            get
            {
                return new SelectList(Enumerable.Range(2016, (DateTime.Now.Year - 2016) + 1));
            }
        }
      
    }
}