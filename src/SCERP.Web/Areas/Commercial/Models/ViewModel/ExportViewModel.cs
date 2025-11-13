using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Antlr.Runtime.Misc;
using SCERP.Model;
using SCERP.Model.CommercialModel;
using SCERP.Model.Production;

namespace SCERP.Web.Areas.Commercial.Models.ViewModel
{
    public class ExportViewModel : ProSearchModel<CommExport>
    {
        public ExportViewModel()
        {
            CommExport = new CommExport();
            CommLcInfos = new List<COMMLcInfo>();
            CommSalesContacts = new List<CommSalseContact>();
            CommExports = new List<CommExport>();
            CommExportDetail = new Dictionary<string, CommExportDetail>();
            ExportDetail = new CommExportDetail();

            CommPackingListDetail = new Dictionary<string, CommPackingListDetail>();
            PackingListDetail = new CommPackingListDetail();

            BuyerOrders = new List<OM_BuyerOrder>();
            BuyOrdStyles = new List<OM_BuyOrdStyle>();
        }

        public IEnumerable<System.Web.Mvc.SelectListItem> LcSelectListItem
        {
            get { return new SelectList(CommLcInfos, "LcId", "LcNo"); }
        }
        public IEnumerable<System.Web.Mvc.SelectListItem> SalesContactSelectListItem
        {
            get { return new SelectList(CommSalesContacts, "SalseContactId", "LcNo"); }
        }
        public IEnumerable<SelectListItem> BuyerOrderSelectListItem
        {
            get
            {
                return new SelectList(BuyerOrders, "OrderNo", "RefNo");
            }
        }

        public IEnumerable<SelectListItem> BuyerOrderStyleSelectListItem
        {
            get
            {
                return new SelectList(BuyOrdStyles, "OrderStyleRefId", "StyleRefId");
            }
        }

        public List<OM_BuyerOrder> BuyerOrders { get; set; }

        public List<OM_BuyOrdStyle> BuyOrdStyles { get; set; }

        public int? CartonQuantity { get; set; }
        public int? ItemQuantity { get; set; }
        public decimal? Rate { get; set; }
        public long ExportId { get; set; }

        //public string Country { get; set; }
        //public string Color { get; set; }
        //public string Size { get; set; }


        public CommExport CommExport { get; set; }
        public List<COMMLcInfo> CommLcInfos { get; set; }
        public List<CommSalseContact> CommSalesContacts { get; set; }
        public List<CommExport> CommExports { get; set; }
        public CommExportDetail ExportDetail { get; set; }
        public Dictionary<string, CommExportDetail> CommExportDetail { get; set; }

        public CommPackingListDetail PackingListDetail { get; set; }
        public Dictionary<string, CommPackingListDetail> CommPackingListDetail { get; set; }
    }
}