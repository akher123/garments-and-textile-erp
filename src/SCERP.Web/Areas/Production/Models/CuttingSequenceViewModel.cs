using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iTextSharp.text.pdf.crypto;
using SCERP.Model;
using SCERP.Model.Production;

namespace SCERP.Web.Areas.Production.Models
{
    public class CuttingSequenceViewModel : ProSearchModel<CuttingSequenceViewModel>
    {
        public CuttingSequenceViewModel()
        {
            CuttingSequence = new VwCuttingSequence();
            Buyers=new List<OM_Buyer>();
            Styles=new List<OM_Style>();
            StyleList = new List<object>();
            OrderList = new List<object>();
            Colors = new List<object>();
            Components=new List<OM_Component>();
            CuttingSequenceDictionary = new Dictionary<string, VwCuttingSequence>();
        }
        public Dictionary<string, VwCuttingSequence> CuttingSequenceDictionary { get; set; }
        public VwCuttingSequence CuttingSequence { get; set; }
        public IEnumerable Buyers { get; set; }
        public IEnumerable OrderList { get; set; }
        public List<OM_Style> Styles { get; set; }
        public IEnumerable StyleList { get; set; }
        public IEnumerable Colors { get; set; }
        public OM_Color Color { get; set; }
        public List<OM_Component> Components { get; set; }
        
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
    }
}