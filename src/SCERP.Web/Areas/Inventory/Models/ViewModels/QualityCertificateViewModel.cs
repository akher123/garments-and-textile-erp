using System.Collections;
using System.Collections.Generic;
using System.Web.Mvc;
using iTextSharp.text;
using SCERP.Model;


namespace SCERP.Web.Areas.Inventory.Models.ViewModels
{
    public class QualityCertificateViewModel : VQualityCertificate
    {
        public List<VQualityCertificate> QualityCertificates { get; set; }
        public List<Inventory_ItemStoreDetail> InventoryItemStoreDetails { get; set; }
        public List<VQualityCertificateDetail> VQualityCertificateDetails { get; set; }

        public QualityCertificateViewModel()
        {
            VQualityCertificateDetails = new List<VQualityCertificateDetail>();
            InventoryItemStoreDetails=new List<Inventory_ItemStoreDetail>();
            QualityCertificates = new List<VQualityCertificate>();
            Branches = new List<object>();
            Companies = new List<object>();
          
        }

        public IEnumerable<SelectListItem> IsGrnConvertedSelectListItem
       {
           get { return new SelectList(new[] { new { IsGrnConverted = false, Status = "Convert" }, new { IsGrnConverted = true, Status = "Converted" }, }, "IsGrnConverted", "Status"); }

        }

        public IEnumerable Branches { get; set; }
        public IEnumerable<SelectListItem> BranchSelectListItem
        {
            get { return new SelectList(Branches, "BranchId", "BranchName"); }

        }

        public IEnumerable Companies { get; set; }
        public IEnumerable<SelectListItem> CompanySelectListItem
        {
            get { return new SelectList(Companies, "CompanyId", "CompanyName"); }

        }
        public List<Mrc_SupplierCompany> Suppliers { get; set; }
        public IEnumerable<SelectListItem> SupplierSelectListItem
        {
            get { return new SelectList(Suppliers, "SupplierCompanyId", "CompanyName"); }

        }
    }
}