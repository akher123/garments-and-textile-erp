using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCERP.Model.InventoryModel;
using SCERP.Model.Production;
using SCERP.Web.Areas.Merchandising.Report.DATASOURCE;

namespace SCERP.Web.Areas.Inventory.Models.ViewModels
{
    public class MaterialReceivedViewModel:ProSearchModel<MaterialReceivedViewModel>
    {
        public string Key { get; set; }
        public MaterialReceivedViewModel()
        {
            MaterialReceived=new Inventory_MaterialReceived();
            MaterialReceivedList=new List<Inventory_MaterialReceived>();
            DataTable=new DataTable();
            MaterialReceivedDetailDictionary=new Dictionary<string, Inventory_MaterialReceivedDetail>();
            MaterialReceivedDetail=new Inventory_MaterialReceivedDetail();
        }
        public DataTable DataTable { get; set; }
        public Dictionary<string,Inventory_MaterialReceivedDetail> MaterialReceivedDetailDictionary { get; set; }
        public Inventory_MaterialReceived MaterialReceived { get; set; }
        public Inventory_MaterialReceivedDetail MaterialReceivedDetail { get; set; }
        public List<Inventory_MaterialReceived> MaterialReceivedList { get; set; }
        public IEnumerable<SelectListItem> BillStatusSelectListItem
        {
            get
            {
                return new SelectList(new[] { new { Text = "Pending", Value = "Pending" }, new { Text = "Done", Value = "Done" }}, "Value", "Text");
            }
        }
        public IEnumerable<SelectListItem> RegisterTypeListItem 
        {
            get
            {
                return new SelectList(new[] { new { Text = "General Register", Value = "GENERALREGISTER" }, new { Text = "Dyes Chemical Register", Value = "DYESCHEMICALREGISTER" }, new { Text = "Yarn Register", Value = "YARNREGISTER" }, new { Text = "Grey Fabric", Value = "GREYFABRIC" }, new { Text = "Finish Fabric", Value = "FINISHFABRIC" } }, "Value", "Text");
            }
        }
    }
}