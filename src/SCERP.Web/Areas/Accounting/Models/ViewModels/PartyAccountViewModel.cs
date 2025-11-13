using SCERP.Common;
using SCERP.Model.CommonModel;
using SCERP.Model.Production;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SCERP.Web.Areas.Accounting.Models.ViewModels
{
    public class PartyAccountViewModel:ProSearchModel<PartyAccountViewModel>
    {
        public VwParty Party { get; set; }
        public List<VwParty> Parties { get; set; }
        public PartyAccountViewModel()
        {
            Party = new VwParty();
            Parties = new List<VwParty>();

        }
   
        [Required]
        public PartyType PrType { get; set; }
        [Required]
        public int GlId { get; set; }
        public IEnumerable<SelectListItem> AccountTypeSelectListItem
        {
            get
            {
                return new SelectList(new[] { new { ID = PartyType.K, Name = "Knitting Payable" }, new { ID = PartyType.R, Name = "Knitting Receivable" }, new { ID = PartyType.D, Name = "Dyeing Receivable" }, new { ID = PartyType.P, Name = "Print Payable" }, new { ID = PartyType.E, Name = "Embroidery Payable" } }, "ID", "Name");
            }
        }

        public IEnumerable<SelectListItem> PTypeSelectListItem
        {
            get
            {
                return new SelectList(new[] { new { ID = "P", Name = "Knitting & Dyeing " }, new { ID = "F", Name = "Print & Embroidery" } }, "ID", "Name");
            }
        }
    }
}