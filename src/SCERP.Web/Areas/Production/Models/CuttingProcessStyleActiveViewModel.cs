using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls.Expressions;
using iTextSharp.text;
using Microsoft.Ajax.Utilities;
using SCERP.Model;
using SCERP.Model.Production;


namespace SCERP.Web.Areas.Production.Models
{
    public class CuttingProcessStyleActiveViewModel : SearchModel<CuttingProcessStyleActiveViewModel>
  {
      public CuttingProcessStyleActiveViewModel()
      {
          CuttingProcessStyleActive=new PROD_CuttingProcessStyleActive();
          CuttingProcessStyleActiveList=new List<PROD_CuttingProcessStyleActive>();
          Buyers = new List<OM_Buyer>();
          OrderList = new List<object>();
          Styles = new List<object>(); 
          VwCuttingProcessStyleActiveList=new List<VwCuttingProcessStyleActive>();
          VwCuttingProcessStyleActive=new VwCuttingProcessStyleActive();
          BuyerList = new List<Object>();
      }
      public IEnumerable BuyerList { get; set; }
      public VwCuttingProcessStyleActive VwCuttingProcessStyleActive { get; set; }
      public List<VwCuttingProcessStyleActive> VwCuttingProcessStyleActiveList { get; set; } 
      public PROD_CuttingProcessStyleActive CuttingProcessStyleActive { get; set; }
      public List<PROD_CuttingProcessStyleActive> CuttingProcessStyleActiveList { get; set; }
      public List<OM_Buyer> Buyers { get; set; }
      public IEnumerable OrderList { get; set; }
      public IEnumerable Styles { get; set; }
      public IEnumerable<SelectListItem> BuyerListSelectListItem
      {
          get
          {
              return new SelectList(BuyerList,"BuyerRefId", "BuyerName");
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