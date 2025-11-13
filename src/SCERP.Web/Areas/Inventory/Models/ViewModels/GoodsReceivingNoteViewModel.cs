using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using SCERP.Common;
using SCERP.Model;

namespace SCERP.Web.Areas.Inventory.Models.ViewModels
{
    public class GoodsReceivingNoteViewModel : VGoodsReceivingNote
    {

        public string Description { get; set; }
        public List<VGoodsReceivingNote> VGoodsReceivingNotes{ get; set; }
        public List<VQualityCertificateDetail> VQualityCertificateDetails { get; set; }
        public GoodsReceivingNoteViewModel()
        {
            Branches = new List<object>();
            Companies = new List<object>();
            VQualityCertificateDetails=new List<VQualityCertificateDetail>();
            VGoodsReceivingNotes=new List<VGoodsReceivingNote>();
        }

        [DataType(DataType.Date)]
        [Required(ErrorMessage =CustomErrorMessage.RequiredErrorMessage)]
        public DateTime? TransactionDate { get; set; }
        public IEnumerable Branches { get; set; }
        public IEnumerable<SelectListItem> BranchSelectListItem
        {
            get { return new SelectList(Branches, "BranchId", "BranchName"); }

        }

        public IEnumerable<SelectListItem> GRNStatusSelectListItem
        {
            get { return new SelectList(new[] { new { IsSendToStoreLedger = true, Status = "Converted" }, new { IsSendToStoreLedger = false, Status = "Convert" } }, "IsSendToStoreLedger", "Status"); }

        }
        public IEnumerable<SelectListItem> GRNApprovedStatusSelectListItem
        {
            get { return new SelectList(new[] { new { Value = true,  Text= "Approved" }, new {Value = false, Text = "Panding" } }, "Value", "Text"); }

        }
        public IEnumerable Companies { get; set; }
        public IEnumerable<SelectListItem> CompanySelectListItem
        {
            get { return new SelectList(Companies, "CompanyId", "CompanyName"); }

        }

        public decimal GetTotalAmount()
        {
            decimal total = 0;
            if (VQualityCertificateDetails.Any())
            {
                total= VQualityCertificateDetails.Sum(x => x.Amount);
            }
            return total;
        }

        public string CurrencyName
        {
            get
            {
                if (VQualityCertificateDetails.Any())
                {
                    return VQualityCertificateDetails[0].CureencyName;
                }
                return "";

            }
        }
    }
}