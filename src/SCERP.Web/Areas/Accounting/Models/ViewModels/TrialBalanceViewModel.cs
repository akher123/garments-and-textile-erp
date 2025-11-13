using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCERP.Model;
using System.Collections;

namespace SCERP.Web.Areas.Accounting.Models.ViewModels
{
    public class TrialBalanceViewModel
    {
        public List<TrialBalanceViewModel> TrialBalance { get; set; }
        public int TotalRecords { get; set; }
        public string ClasslCode { get; set; }
        public string AccountCode { get; set; }
        public string Particulars { get; set; }
        public decimal OpeningBalance { get; set; }
        public decimal TotalDebit { get; set; }
        public decimal TotalCredit { get; set; }
        public decimal ClosingBalance { get; set; }
        public decimal CurrentYear { get; set; }
        public decimal PrevYear { get; set; }
        public string ClasslHead { get; set; }
        public string GrouplHead { get; set; }
        public string SubGrouplHead { get; set; }
        public string SubGroupCode { get; set; }
        public string ControlHead { get; set; }
        public string GlHead { get; set; }
        public int OpeningOption { get; set; }
        public string CompanyName { get; set; }
        public string ReportTitle { get; set; }
        public string SectorName { get; set; }
        public string GLHeadName { get; set; }
        public string DateBetween { get; set; }
        public string Others1 { get; set; }
        public string Others2 { get; set; }
        public decimal Others1_CurrentYear { get; set; }
        public decimal Others2_CurrentYear { get; set; }
        public decimal Others1_PrevYear { get; set; }
        public decimal Others2_PrevYear { get; set; }
        public decimal RateOfDep { get; set; }
        public decimal Depreciation { get; set; }
        public int SectorId { get; set; }
        public int CostCentresMultilayers { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string Note { get; set; }      
    }
}