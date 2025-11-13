using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCERP.Common;
using SCERP.Model;
using SCERP.Model.CommonModel;
using SCERP.Model.Production;

namespace SCERP.Web.Areas.Production.Models
{
    public class CuttingTagViewModel : ProSearchModel<CuttingTagViewModel>
    {
        public CuttingTagViewModel()
        {
            Buyers = new List<OM_Buyer>();
            StyleList = new List<object>();
            OrderList = new List<object>();
            Colors = new List<object>();
            CuttingTag=new PROD_CuttingTag();
            Components=new List<OM_Component>();
            CuttingSequences=new List<VwCuttingSequence>();
            CuttingSequence=new VwCuttingSequence();
            CuttingTags = new List<VwCuttingTag>();
            Parties=new List<Party>();
            CuttingTagSupplier=new PROD_CuttingTagSupplier();
        }
        public string ComponentName { get; set; }
        public List<Party> Parties { get; set; } 
        public PROD_CuttingTag CuttingTag { get; set; }
        public PROD_CuttingTagSupplier CuttingTagSupplier { get; set; }
        public List<VwCuttingTag> CuttingTags { get; set; } 
        public List<VwCuttingSequence> CuttingSequences { get; set; }
        public VwCuttingSequence CuttingSequence { get; set; }
        public List<OM_Component> Components { get; set; }
        public IEnumerable Buyers { get; set; }
        public IEnumerable OrderList { get; set; }
        public IEnumerable StyleList { get; set; }
        public IEnumerable Colors { get; set; }
        public IEnumerable<SelectListItem> BuyerSelectListItem
        {
            get
            {
                return new SelectList(Buyers, "BuyerRefId", "BuyerName");
            }
        }
        public IEnumerable<SelectListItem> OrderSelectListItem
        {
            get
            {
                return new SelectList(OrderList, "OrderNo", "RefNo");
            }
        }
        public IEnumerable<SelectListItem> StylesSelectListItem
        {
            get
            {
                return new SelectList(StyleList, "OrderStyleRefId", "StyleName");
            }
        }
        public IEnumerable<SelectListItem> ColorSelectListItem
        {
            get
            {
                return new SelectList(Colors, "ColorRefId", "ColorName");
            }
        }
        public IEnumerable<SelectListItem> ComponentSelectListItem
        {
            get
            {
                return new SelectList(Components, "ComponentRefId", "ComponentName");
            }
        }

        public IEnumerable<SelectListItem> PartyListSelectListItem

        {
            get
            {
                return new SelectList(Parties, "PartyId", "Name");
            }
        }
        public IEnumerable<SelectListItem> EmblishmentStatusSelectListItem
        {
            get
            {
                return new SelectList(EmblishmentStatusList(), "Value", "Text");
            }
        }
        private IEnumerable EmblishmentStatusList()
        {
           int print= CuttingTag.IsPrint ? 1 : 0;
           int embroidery = CuttingTag.IsEmbroidery ? 2 : 0;
            var emblishmentStatusList = (from EmblishmentStatus s in Enum.GetValues(typeof(EmblishmentStatus))
                                         select new { Value = (int)s, Text = s.ToString() }).Where(x=>x.Value==print||x.Value==embroidery).ToList();
            return emblishmentStatusList;
        }
    }
}