using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCERP.Common;
using SCERP.Model;
using SCERP.Model.CommonModel;
using SCERP.Model.Planning;
using SCERP.Model.Production;

namespace SCERP.Web.Areas.Production.Models
{
    public class DyeingJobOrderViewModel : ProSearchModel<DyeingJobOrderViewModel>
    {
        public List<PROD_DyeingJobOrder> JobOrders { get; set; }
        public Dictionary<string,VwDyeingJobOrderDetail> Dictionary { get; set; }
        public PROD_DyeingJobOrder JobOrder { get; set; }
        public VwDyeingJobOrderDetail JobOrderDetail { get; set; }
        public List<Party> Parties { get; set; }
        public List<PLAN_Process> ProcessList { get; set; }
        public List<Dropdown> Dropdowns { get; set; }
        public List<OM_Buyer> BuyerList { get; set; }
        public IEnumerable OrderList { get; set; }
        public IEnumerable StyleList { get; set; }
        public string ChallanNo { get; set; }
        public DyeingJobOrderViewModel()
        {
            JobOrders=new List<PROD_DyeingJobOrder>();
            JobOrder=new PROD_DyeingJobOrder();
            Dictionary=new Dictionary<string, VwDyeingJobOrderDetail>();
            Parties=new List<Party>();
            BuyerList = new List<OM_Buyer>();
            OrderList = new List<object>();
            StyleList = new List<object>();
            ProcessList = new List<PLAN_Process>();
            JobOrderDetail =new VwDyeingJobOrderDetail();
            Dropdowns = new List<Dropdown>();


        }
        public IEnumerable<SelectListItem> ChallanNoSelectListItem
        {
            get
            {
                return new SelectList(Dropdowns, "Id", "Value");
            }
        }

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
        public IEnumerable<SelectListItem> StylesSelectListItem
        {
            get
            {
                return new SelectList(StyleList, "OrderStyleRefId", "StyleName");
            }
        }
        public IEnumerable<SelectListItem> JobTypeSelectListItem
        {
            get
            {
                return new SelectList(new[] { new { Value = "IN", Text = "INTERNAL" }, new { Value = "EX", Text = "EXTERNAL" } }, "Value", "Text");
            }
        }
        public IEnumerable<SelectListItem> PartySelectListItem
        {
            get
            {
                return new SelectList(Parties, "PartyId", "Name");
            }
        }
        public IEnumerable<SelectListItem>ProcessSelectListItem
        {
            get
            {
                return new SelectList(ProcessList, "ProcessRefId", "ProcessName");
            }
        }
    }
}