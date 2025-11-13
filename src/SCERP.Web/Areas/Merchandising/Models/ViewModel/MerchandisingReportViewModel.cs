using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Web.Mvc;
using System.Web.Optimization;
using SCERP.Common;
using SCERP.Model;
using SCERP.Model.MerchandisingModel;

namespace SCERP.Web.Areas.Merchandising.Models.ViewModel
{
    public class MerchandisingReportViewModel 
    {
        public IEnumerable<OM_Merchandiser> Merchandisers { get; set; }
        public MerchandisingReportViewModel()
        {
            DataTable=new DataTable();
            Buyers=new List<OM_Buyer>();
            Merchandisers=new List<OM_Merchandiser>();
            Styles=new List<VOMBuyOrdStyle>();
            OrderList=new List<OM_BuyerOrder>();
        }
        [Required(ErrorMessage = CustomErrorMessage.RequiredErrorMessage)]
        public string OrderNo { get; set; }
            [Required(ErrorMessage = CustomErrorMessage.RequiredErrorMessage)]
        public string OrderStyleRefId { get; set; }
        public virtual string SearchString { get; set; }
        public DataTable DataTable { get; set; }
        [DataType(DataType.Date)]
        public DateTime? FromDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime? ToDate { get; set; }
        public bool IsShowReport { get; set; }
       [Required(ErrorMessage = CustomErrorMessage.RequiredErrorMessage)]
        public string BuyerRefId { get; set; }
        public string MerchandiserId { get; set; }
        public List<OM_Buyer> Buyers { get; set; }
        public IEnumerable Styles { get; set; }
        public IEnumerable OrderList { get; set; }
        public IEnumerable<SelectListItem> BuyerSelectListItem
        {
            get
            {
                return new SelectList(Buyers, "BuyerRefId", "BuyerName");
            }
        }
        public IEnumerable<SelectListItem> MerchandisngSelectListItem
        {
            get
            {
                return new SelectList(Merchandisers, "EmpId", "EmpName");
            }
        }
        [Required(ErrorMessage = "@Required!")]
        public DateTime RecevedDate { get; set; }
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
                return new SelectList(Styles, "OrderStyleRefId", "StyleName");
            }
        }
    }
}