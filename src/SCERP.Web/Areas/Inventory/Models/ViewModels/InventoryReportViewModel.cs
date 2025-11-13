using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using SCERP.Common;
using SCERP.Model;
using SCERP.Model.InventoryModel;
using SCERP.Model.MerchandisingModel;

namespace SCERP.Web.Areas.Inventory.Models.ViewModels
{
    public class InventoryReportViewModel : SearchModel<InventoryReportViewModel>
    {
        public List<Inventory_Group> Groups { get; set; }
        public List<Inventory_SubGroup> SubGroups { get; set; }
        public List<Mrc_SupplierCompany> SupplierCompanies { get; set; }
        public List<OM_Buyer> Buyers { get; set; }
        public IEnumerable Styles { get; set; }
        public IEnumerable OrderList { get; set; }
      
        public InventoryReportViewModel()
        {
            Buyers=new List<OM_Buyer>();
           
            Styles=new List<VOMBuyOrdStyle>();
            OrderList=new List<OM_BuyerOrder>();
            Groups=new List<Inventory_Group>();
            SubGroups=new List<Inventory_SubGroup>();
            SupplierCompanies=new List<Mrc_SupplierCompany>();
        }
        [Required(ErrorMessage = CustomErrorMessage.RequiredErrorMessage)]
        public string ItemName { get; set; }
        public string ReportName { get; set; }
        [Required(ErrorMessage = CustomErrorMessage.RequiredErrorMessage)]
        [DataType(DataType.Date)]
        public DateTime? FromDate { get; set; }
        [DataType(DataType.Date)]
        [Required(ErrorMessage = CustomErrorMessage.RequiredErrorMessage)]
        public DateTime? ToDate { get; set; }
        public  StringBuilder ReprotUrl{ get; set; }
        [Required(ErrorMessage = CustomErrorMessage.RequiredErrorMessage)]
        public string ItemCode { get; set; }
        public int ItemId { get; set; }
        public int GroupId  { get; set; }
        public int SubGroupId { get; set; }
        public int SupplierId { get; set; }
        public string BuyerRefId { get; set; }
        public string OrderNo { get; set; }
        public string OrderStyleRefId { get; set; }
        public IEnumerable<SelectListItem> GroupSelectListItem
        {
            get
            {
                return new SelectList(Groups, "GroupId", "GroupName");
            }
        }
        public IEnumerable<SelectListItem> SubGroupSelectListItem
        {
            get
            {
                return new SelectList(SubGroups, "SubGroupId", "SubGroupName");
            }
        }
        public IEnumerable<SelectListItem>SupplierSelectListItem
        {
            get
            {
                return new SelectList(SupplierCompanies, "SupplierCompanyId", "CompanyName");
            }
        }

   
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
                return new SelectList(Styles, "OrderStyleRefId", "StyleName");
            }
        }
    }
}