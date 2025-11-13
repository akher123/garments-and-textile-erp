using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCERP.Model;
using SCERP.Model.CommercialModel;
using System.Collections;

namespace SCERP.Web.Areas.Commercial.Models.ViewModel
{
    public class LcViewModel : COMMLcInfo
    {
        public LcViewModel()
        {
            LcInfos = new List<COMMLcInfo>();
            IsSearch = true;
            CommLcInfo = new COMMLcInfo();
            Buyers = new List<OM_Buyer>();
            Banks = new List<CommBank>();
            LcTypes = new List<object>();
            PartialShip = new List<object>();
            VwCommLcInfos = new List<VwCommLcInfo>();
            GroupLcs = new List<COMMLcInfo>();
        }

        public List<VwCommLcInfo> VwCommLcInfos { get; set; }
        public COMMLcInfo CommLcInfo { get; set; }
        public List<OM_Buyer> Buyers { get; set; }
        public List<COMMLcInfo> GroupLcs { get; set; }
        public List<CommBank> Banks { get; set; }
        public List<SelectListItem> CashIntensiveCompletedStatusSelectedItem
        {
            get { return new SelectList(new[] { "Completed", "Pending" }).ToList(); }
        }
        public IEnumerable<System.Web.Mvc.SelectListItem> LcGroupSelectListItem
        {
            get { return new SelectList(GroupLcs, "LcId", "LcNo"); }
        }


        public List<SelectListItem> CashIntensiveStatusSelectedItem
        {
            get { return new SelectList(new[] {"OK", "Pending", "U/P"}).ToList(); }
        }

        public List<SelectListItem> BuyerSelectListItem
        {
            get { return new SelectList(Buyers, "BuyerId", "BuyerName").ToList(); }
        }

        public List<SelectListItem> BankSelectListItem
        {
            get { return new SelectList(Banks, "BankId", "BankName").ToList(); }
        }

        public IEnumerable LcTypes { get; set; }

        public IEnumerable<System.Web.Mvc.SelectListItem> LcTypeSelectListItem
        {
            get { return new SelectList(LcTypes, "Id", "Name"); }
        }

        public IEnumerable PartialShip { get; set; }

        public IEnumerable<System.Web.Mvc.SelectListItem> PartialShipmentSelectListItem
        {
            get { return new SelectList(PartialShip, "Id", "Name"); }
        }

        public List<COMMLcInfo> LcInfos { get; set; }

        public string SearchKey { get; set; }

        public IEnumerable SearchType { get; set; }

        public IEnumerable<System.Web.Mvc.SelectListItem> SearchTypeSelectListItem
        {
            get { return new SelectList(SearchType, "Id", "Name"); }
        }
        public IEnumerable<SelectListItem> StatusSelectListItem
        {
            get
            {
                return new SelectList(new[] { new { Closed = "O", Value = "OPEN" }, new { Closed = "C", Value = "CLOSED" } }, "Closed", "Value");
            }
        }
    }
}