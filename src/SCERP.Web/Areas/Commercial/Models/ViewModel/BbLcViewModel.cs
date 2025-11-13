using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCERP.Model;
using SCERP.Model.CommercialModel;
using SCERP.Common;

namespace SCERP.Web.Areas.Commercial.Models.ViewModel
{
    public class BbLcViewModel : CommBbLcInfo
    {
        public BbLcViewModel()
        {
            BbLcInfos = new List<CommBbLcInfo>();
            Banks = new List<CommBank>();
            CommBbLcItemDetails = new CommBbLcItemDetails();
            CommBbLcItemDetailsDct = new Dictionary<string, CommBbLcItemDetails>();
            InventoryGroupNames = new List<Inventory_Group>();
            PrintFormatStatuses = new List<PrintFormatType>();
            CommSalesContacts=new List<CommSalseContact>();
            IsSearch = true;
        }

        public List<CommSalseContact> CommSalesContacts { get; set; }
        public string Key { get; set; }

        public List<Inventory_Group> InventoryGroupNames { get; set; }
        public List<CommBank> Banks { get; set; }
        public CommBbLcItemDetails CommBbLcItemDetails { get; set; }
        public Dictionary<string, CommBbLcItemDetails> CommBbLcItemDetailsDct { get; set; }
        public IEnumerable Lcs { get; set; }

        public List<SelectListItem> BankSelectListItem
        {
            get { return new SelectList(Banks, "BankId", "BankName").ToList(); }
        }

        public List<SelectListItem> ItemGroupNameSelectListItem
        {
            get { return new SelectList(InventoryGroupNames, "GroupId", "GroupName").ToList(); }
        }

        public IEnumerable<System.Web.Mvc.SelectListItem> LcSelectListItem
        {
            get { return new SelectList(Lcs, "LcId", "LcNo"); }
        }

        public List<Mrc_SupplierCompany> Suppliers { get; set; }

        public List<SelectListItem> SupplierSelectListItem
        {
            get { return new SelectList(Suppliers, "SupplierCompanyId", "CompanyName").ToList(); }
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

        public List<CommBbLcInfo> BbLcInfos { get; set; }

        public int PrintFormatId { get; set; }

        public IEnumerable PrintFormatStatuses { get; set; }

        public IEnumerable<System.Web.Mvc.SelectListItem> PrintFormatStatusSelectListItems
        {
            get { return new SelectList(PrintFormatStatuses, "Id", "Name"); }
        }

        public IEnumerable<System.Web.Mvc.SelectListItem> BbLcTypeSelectListItems
        {
            get { return new SelectList(new[] { new { Text = "BBLC", Value = 1 }, new { Text = "FTT", Value = 2 }, new { Text = "FDD", Value = 3 } }, "Value", "Text"); }
        }

        public IEnumerable<System.Web.Mvc.SelectListItem> SalesContactSelectListItem
        {
            get { return new SelectList(CommSalesContacts, "SalseContactId", "LcNo"); }
        }
    }
}