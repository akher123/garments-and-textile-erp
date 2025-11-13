using System.Collections;
using System.Collections.Generic;
using System.Web.Mvc;
using iTextSharp.text;
using SCERP.Model;
using Document = SCERP.Model.CommonModel.Document;

namespace SCERP.Web.Areas.Common.Models.ViewModel
{
    public class DocumentViewModel:Document
    {
        public string BuyerRefId { get; set; }
        public string OrderNo { get; set; }
        public string OrderStyleRefId { get; set; }
        public bool IsSearch { get; set; }
       
        public DocumentViewModel()
        {
            Documents=new List<Document>();
            BuyerList = new List<OM_Buyer>();
            OrderList = new List<object>();
        }
        public List<Document> Documents { get; set; }
        public List<OM_Buyer> BuyerList { get; set; }
        public IEnumerable OrderList { get; set; }
        public IEnumerable<SelectListItem> BuyerSelectListItem
        {
            get
            {
                return new SelectList(BuyerList, "BuyerRefId", "BuyerName");
            }
        }
        public IEnumerable<SelectListItem> OrderSelectListItem
        {
            get
            {
                return new SelectList(OrderList, "OrderNo", "RefNo");
            }
        }
    }
}