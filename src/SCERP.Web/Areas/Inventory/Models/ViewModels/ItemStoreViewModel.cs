using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using iTextSharp.text;
using SCERP.Common;
using SCERP.Model;
using SCERP.Model.Custom;

namespace SCERP.Web.Areas.Inventory.Models.ViewModels
{
    public class ItemStoreViewModel : Inventory_ItemStore
    {
        public string Key { get; set; }

        [Required(ErrorMessage = CustomErrorMessage.RequiredErrorMessage)]
        public int StorePurchaseRequisitionId { get; set; }
        public int CompanyId { get; set; }
        [Required(ErrorMessage = "Required")]
        public string RequisitorName { get; set; }
        public List<VInventoryItemStore> InventoryItemStores { get; set; }
        public List<Inventory_Size> InventorySizes { get; set; }
        public List<Inventory_Brand> InventoryBrands { get; set; }
        public List<Country> InventoryOrigins { get; set; }
        public Dictionary<string, VItemReceiveDetail> InventoryItemStoreDetails { get; set; }

        public InventorySearchField InventorySearchField { get; set; }
        public List<Currency> Currencies { get; set; }
 
        public int LastRowIndex { get; set; }
        public ItemStoreViewModel()
        {
            InventoryItemStores = new List<VInventoryItemStore>();
            InventoryItemStoreDetails = new Dictionary<string, VItemReceiveDetail>();
            InventorySearchField = new InventorySearchField();
            Suppliers = new List<Mrc_SupplierCompany>();
            AuthorizedPersons = new List<Inventory_AuthorizedPerson>();
            Currencies = new List<Currency>();
            InventoryBrands = new List<Inventory_Brand>();
            InventorySizes = new List<Inventory_Size>();
            InventoryOrigins = new List<Country>();
        }

        public IEnumerable<SelectListItem> QcStatusSelectListItem
        {
            get
            {
                return new SelectList(from QCPassStatus s in Enum.GetValues(typeof(QCPassStatus))
                                      select new { QCStatus = (byte)s, Name = s.ToString() }, "QCStatus", "Name");
            }

        }
        public IEnumerable<SelectListItem> ReceiveTypeSelectListItem
        {
            get
            {
                return new SelectList(Enum.GetValues(typeof(MaterialReceiveType)).Cast<MaterialReceiveType>().Select(x => new
                {
                    RTypeId = Convert.ToInt16(x),
                    Name = x
                }), "RTypeId", "Name");
            }

        }
        public List<Inventory_AuthorizedPerson> AuthorizedPersons { get; set; }
        public IEnumerable<SelectListItem> AuthorizedPersonSelectListItem
        {
            get { return new SelectList(AuthorizedPersons, "EmployeeId", "Employee.Name"); }

        }
        public decimal GrandTotal { get { return InventoryItemStoreDetails.Sum(x => x.Value.UnitPrice * x.Value.ReceivedQuantity); } }
        public List<Mrc_SupplierCompany> Suppliers { get; set; }
        public IEnumerable<SelectListItem> SupplierSelectListItem
        {
            get { return new SelectList(Suppliers, "SupplierCompanyId", "CompanyName"); }

        }

        public IEnumerable<SelectListItem> SuppliedStatusSelectListItem
        {
            get { return new SelectList(new[] { new { SuppliedStatus = "R", Text = "Ref.Person" }, new { SuppliedStatus = "P", Text = "Party" } }, "SuppliedStatus", "Text"); }

        }



    }
}