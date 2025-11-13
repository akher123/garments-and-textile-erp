using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCERP.Common;
using SCERP.Model;
using SCERP.Model.MerchandisingModel;

namespace SCERP.Web.Areas.Merchandising.Models.ViewModel
{
    public class BuyerTnaTemplateViewModel : SearchModel<BuyerTnaTemplateViewModel>
    {
        public BuyerTnaTemplateViewModel()
        {
            TemplateTypes=new List<TemplateType>();
            Buyers=new List<OM_Buyer>();
            Templates = new Dictionary<string, BuyerTnaTemplateModel>();
        }

        public Dictionary<string, BuyerTnaTemplateModel> Templates { get; set; }   
        public List<OM_Buyer> Buyers { get; set; }
        [Required]
        public string BuyerRefId { get; set; }
        [Required]
        public int TemplateTypeId { get; set; }
        public IEnumerable TemplateTypes { get; set; }

        public IEnumerable<System.Web.Mvc.SelectListItem> TemplateTypeStatusSelectListItems
        {
            get { return new SelectList(TemplateTypes, "Id", "Name"); }
        }
        public List<SelectListItem> BuyerSelectListItem
        {
            get { return new SelectList(Buyers, "BuyerRefId", "BuyerName").ToList(); }
        }

    }
}