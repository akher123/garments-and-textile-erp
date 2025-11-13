using SCERP.Model;
using SCERP.Model.CommercialModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SCERP.Web.Areas.Commercial.Models.ViewModel
{
    public class ImportViewModel : CommImport
    {

        public ImportViewModel()
        {
           
            CommLcInfos = new List<COMMLcInfo>();
            CommSalesContacts = new List<CommSalseContact>();
            CommImport = new CommImport();
            CommImports = new List<CommImport>();
            

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

       
        //public int? ItemQuantity { get; set; }
        //public decimal? Rate { get; set; }
        //public long ImportId { get; set; }
        
        public CommImport CommImport { get; set; }
        public List<CommImport> CommImports { get; set; }
        public List<COMMLcInfo> CommLcInfos { get; set; }
        public List<CommSalseContact> CommSalesContacts { get; set; }
        
    }

}
