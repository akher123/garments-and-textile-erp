using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCERP.Common;
using SCERP.Model;
using SCERP.Model.InventoryModel;

namespace SCERP.Web.Areas.Inventory.Models.ViewModels
{
    public class LoanGivenViewModel : VLoanGiven
    {
        public List<VLoanGiven> LoanGivens { get; set; }
        public VMaterialIssueDetail IssueDetail { get; set; }
        public Dictionary<string, VMaterialIssueDetail> MaterialIssueDetails { get; set; }
        public string Key { get; set; }
        [Required(ErrorMessage = CustomErrorMessage.RequiredErrorMessage)]
        public string LoanRequisitionNo { get; set; }

        public string OriginSerarcKey { get; set; }
        public LoanGivenViewModel()
        {
            LoanGivens = new List<VLoanGiven>();
            MaterialIssueDetails = new Dictionary<string, VMaterialIssueDetail>();
            IssueDetail = new VMaterialIssueDetail();
            InventoryBrands = new List<Inventory_Brand>();
            Countries = new List<Country>();
            InventorySizes = new List<Inventory_Size>();
            Suppliers = new List<Mrc_SupplierCompany>();
        }
        public List<Inventory_Brand> InventoryBrands { get; set; }
        public IEnumerable<SelectListItem> BrandSelectListItem
        {
            get
            {
                return new SelectList(InventoryBrands, "BrandId", "Name");
            }
        }

        public List<Country> Countries { get; set; }
        public IEnumerable<SelectListItem> OriginSelectListItem
        {
            get
            {
                return new SelectList(Countries, "Id", "CountryName");
            }
        }

        public List<Inventory_Size> InventorySizes { get; set; }
        public IEnumerable<SelectListItem> SizeSelectListItem
        {
            get
            {
                return new SelectList(InventorySizes, "SizeId", "Title");
            }
        }


        public List<Mrc_SupplierCompany> Suppliers { get; set; }
        public IEnumerable<SelectListItem> SupplierSelectListItem
        {
            get { return new SelectList(Suppliers, "SupplierCompanyId", "CompanyName"); }

        }


    }


}