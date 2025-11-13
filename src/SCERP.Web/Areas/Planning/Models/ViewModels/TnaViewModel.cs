using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCERP.Model;
using SCERP.Model.Planning;

namespace SCERP.Web.Areas.Planning.Models.ViewModels
{
    public class TnaViewModel : PLAN_TNA
    {
        public TnaViewModel()
        {
            tna = new List<PLAN_TNA>();
            TnaReports = new List<PLAN_TNAReport>();
            IsSearch = true;
            OrderList = new List<object>();
            BuyerOrderStyles = new List<object>();
        }
        public IEnumerable OrderList { get; set; }
        public IEnumerable BuyerOrderStyles { get; set; }
        public List<PLAN_TNA> tna { get; set; }

        public List<PLAN_TNAReport> TnaReports { get; set; }

        public List<OM_Buyer> Buyers { get; set; }

        public string BuyerRefId { get; set; }
        public string OrderNo { get; set; }
        
        public List<SelectListItem> BuyerSelectListItem
        {
            get { return new SelectList(Buyers, "BuyerRefId", "BuyerName").ToList(); }
        }

        public List<OM_BuyerOrder> BuyerOrders { get; set; }

        public List<SelectListItem> BuyerOrderSelectListItem
        {
            get { return new SelectList(BuyerOrders, "OrderNo", "OrderNo").ToList(); }
        }

        public List<OM_Style> Styles { get; set; }

        public List<SelectListItem> StyleSelectListItem
        {
            get { return new SelectList(Styles, "StylerefId", "StyleName").ToList(); }
        }

        public List<PLAN_Activity> Activities { get; set; }

        public List<SelectListItem> ActivitySelectListItem
        {
            get { return new SelectList(Activities, "Id", "ActivityName").ToList(); }
        }

        public List<PLAN_ResponsiblePerson> ResponsiblePersons { get; set; }

        public List<SelectListItem> ResponsiblePersonSelectListItem
        {
            get { return new SelectList(ResponsiblePersons, "ResponsiblePersonId", "ResponsiblePersonName").ToList(); }
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
        public int SearchByBuyerId
        {
            get;
            set;
        }

        public string SearchByBuyerOrderId
        {
            get;
            set;
        }

        public int SearchByactivityId
        {
            get;
            set;
        }

        public string SearchByStyleId
        {
            get;
            set;
        }
    }
}