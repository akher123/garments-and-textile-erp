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
    public class VoucherToCostCentreViewModel : Acc_VoucherMaster
    {
        public VoucherToCostCentreViewModel()
        {
            CostCentres = new List<Acc_CostCentreMultiLayer>();
            GlAccounts = new List<Acc_GLAccounts>();
            VoucherToCostcentres = new List<Acc_VoucherToCostcentre>();
        }

        public List<Acc_VoucherToCostcentre> VoucherToCostcentres { get; set; }

        public string Key { get; set; }

        public string AccountName { get; set; }

        public List<Acc_CostCentreMultiLayer> CostCentres { get; set; }

        public List<Acc_GLAccounts> GlAccounts { get; set; }

        public decimal Amount { get; set; }

        public decimal TotalAmount { get; set; }

        public Nullable<int> AccountHeadId { get; set; }

        public IEnumerable<SelectListItem> CostCentreSelectListItem
        {
            get
            {
                return new SelectList(CostCentres, "Id", "ItemName");
            }
        }

        public IEnumerable<SelectListItem> GlAccountsSelectListItem
        {
            get
            {
                return new SelectList(GlAccounts, "AccountCode", "AccountName");
            }
        }
    }
}