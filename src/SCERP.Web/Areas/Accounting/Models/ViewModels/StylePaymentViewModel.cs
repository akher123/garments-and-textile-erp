using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCERP.Common;
using SCERP.Model;
using SCERP.Model.AccountingModel;

namespace SCERP.Web.Areas.Accounting.Models.ViewModels
{
    public class StylePaymentViewModel : Acc_StylePayment
    {
        public StylePaymentViewModel()
        {

           StylePayments =new List<Acc_StylePayment>();
           StylePayment = new Acc_StylePayment();
           VwStylePayment=new VStylePayment();
           Buyers = new List<OM_Buyer>();
           BuyerOrders = new List<OM_BuyerOrder>();
           Styles = new List<OM_Style>();
           OrderList = new List<object>();
           BuyerOrderStyles = new List<object>();
           BuyerOrderMasterDataTable = new DataTable();
        }
        
        public List<Acc_StylePayment> StylePayments { get; set; } 
        public Acc_StylePayment StylePayment { get; set; }
        public VStylePayment VwStylePayment { get; set; }
        public List<VStylePayment> VwStylePayments { get; set; } 
        public List<OM_Buyer> Buyers { get; set; }
        public List<OM_BuyerOrder> BuyerOrders { get; set; }
        public List<OM_Style> Styles { get; set; }
        public IEnumerable OrderList { get; set; }
        public IEnumerable BuyerOrderStyles { get; set; }
        public DataTable BuyerOrderMasterDataTable { get; set; }
        public List<Dropdown> CostGroups
        {
            get
            {
                return new List<Dropdown>()
                {
                    new Dropdown() {Id = "FAB", Value = "FABRIC"},
                    new Dropdown() {Id = "ACC", Value = "ACCESSORIES"},
                    new Dropdown() {Id = "EMB", Value = "EMBELLISHMENT"},
                    new Dropdown() {Id = "OTC", Value = "OTHER COST"},
                };
            }
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

        public IEnumerable<SelectListItem> CostGroupsSelectListItem
        {

            get
            {

                return new SelectList(CostGroups, "Id", "Value");
            }
        }
    }
}