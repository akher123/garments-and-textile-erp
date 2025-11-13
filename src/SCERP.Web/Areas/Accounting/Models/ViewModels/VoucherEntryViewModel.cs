using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCERP.Common;
using SCERP.Model;
using SCERP.Model.AccountingModel;

namespace SCERP.Web.Areas.Accounting.Models.ViewModels
{
    public class VoucherEntryViewModel : Acc_VoucherMaster
    {
        public VoucherEntryViewModel()
        {
            VoucherList = new VoucherList();
            VoucherLists = new List<VAccVoucherMaster>();
            VoucherDetail = new Acc_VoucherDetail();
            CostCentres = new List<Acc_CostCentre>();
            CostCentresMultilayers = new List<Acc_CostCentreMultiLayer>();
            CompanySectors = new List<Acc_CompanySector>();
            VoucherDetails = new Dictionary<string, Acc_VoucherDetail>();
        }
        public bool IsPartial { get; set; }
        public List<VAccVoucherMaster> VoucherLists { get; set; }

        public string Key { get; set; }

        public VoucherList VoucherList { get; set; }

        public string AccountName { get; set; }

        public string CostCentreName { get; set; }

        public List<Acc_CostCentre> CostCentres { get; set; }

        public List<Acc_CostCentreMultiLayer> CostCentresMultilayers { get; set; }

        public Acc_VoucherDetail VoucherDetail { get; set; }

        public decimal TotalDebitAmount { get; set; }

        public decimal TotalCreditAmount { get; set; }

        public decimal TotalAmount { get; set; }

        [Required(ErrorMessage = CustomErrorMessage.RequiredErrorMessage)]
        public DateTime? VoucherDate { get; set; }

        public Dictionary<string, Acc_VoucherDetail> VoucherDetails { get; set; }

        public List<Acc_CompanySector> CompanySectors { get; set; }

        public object[] VoucherTypes
        {
            get
            {
                string userName = PortalContext.CurrentUser.Name;

                if (userName == "rony")
                {
                    return new object[]
                   {
                        new {VoucherTypeId = "JV", Value = "Journal Voucher"},
                        new {VoucherTypeId = "BP", Value = "Bank Payment"}
                   };
                }

                else
                    return new object[]
                    {
                        new {VoucherTypeId = "JV", Value = "Journal Voucher"},
                        new {VoucherTypeId = "CP", Value = "Cash Payment"},
                        new {VoucherTypeId = "CR", Value = "Cash Receipt"},
                        new {VoucherTypeId = "BP", Value = "Bank Payment"},
                        new {VoucherTypeId = "BR", Value = "Bank Receipt"},
                        new {VoucherTypeId = "CV", Value = "Contra Voucher"},
                        new {VoucherTypeId = "IV", Value = "Integration Voucher"}


                    };
            }
        }

        public IEnumerable<SelectListItem> VoucherTypeSelectListItem
        {
            get
            {
                return new SelectList(VoucherTypes, "VoucherTypeId", "Value", VoucherType);
            }
        }

        public IEnumerable<SelectListItem> CompanySectorSelectListItem
        {
            get
            {
                return new SelectList(CompanySectors, "Id", "SectorName");
            }
        }

        public IEnumerable<SelectListItem> CostCentreSelectListItem
        {
            get
            {
                return new SelectList(CostCentres, "Id", "CostCentreName");
            }
        }

        public IEnumerable<SelectListItem> CostCentreMultilayersSelectListItem
        {
            get
            {
                return new SelectList(CostCentresMultilayers, "Id", "ItemName");
            }
        }

        public string GetVoucherFullName(string type)
        {
            return new[]
            {
                new {VoucherTypeId = "JV", Value = "Journal Voucher"},
                new {VoucherTypeId = "CP", Value = "Cash Payment"},
                new {VoucherTypeId = "CR", Value = "Cash Receipt"},
                new {VoucherTypeId = "BP", Value = "Bank Payment"},
                new {VoucherTypeId = "BR", Value = "Bank Receipt"},
                new {VoucherTypeId = "CV", Value = "Contra Voucher"}
            }.First(x => x.VoucherTypeId == type).Value;
        }
    }
}