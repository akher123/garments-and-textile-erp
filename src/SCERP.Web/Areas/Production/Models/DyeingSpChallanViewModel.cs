using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCERP.Model;
using SCERP.Model.CommonModel;
using SCERP.Model.Production;

namespace SCERP.Web.Areas.Production.Models
{
    public class DyeingSpChallanViewModel:ProSearchModel<DyeingSpChallanViewModel>
    {
        public string Key { get; set; }
        public DyeingSpChallanViewModel()
        {
            DyeingSpChallan=new PROD_DyeingSpChallan();
            DyeingSpChallans=new List<PROD_DyeingSpChallan>();
            Parties=new List<Party>();
            Batches=new List<Pro_Batch>();
            VDyeingSpChallanDetail=new VwProdDyeingSpChallanDetail();
            DyeingSpChallanDetailDictionary=new Dictionary<string, VwProdDyeingSpChallanDetail>();
            Items=new List<object>();
            GroupList=new List<PROD_GroupSubProcess>();

        }

        public bool IsActive { get; set; }
        public VwProdDyeingSpChallanDetail VDyeingSpChallanDetail { get; set; } 
        public PROD_DyeingSpChallan DyeingSpChallan { get; set; }
        public List<PROD_DyeingSpChallan> DyeingSpChallans { get; set; }
        public List<Party> Parties { get; set; }
        public List<Pro_Batch> Batches { get; set; }
        public IEnumerable Items { get; set; }
        public List<PROD_GroupSubProcess> GroupList { get; set; }
        public Dictionary<string, VwProdDyeingSpChallanDetail> DyeingSpChallanDetailDictionary { get; set; }
        public IEnumerable<SelectListItem> PartySelectListItem
        {
            get { return new SelectList(Parties, "PartyId", "Name"); }
        }

        public IEnumerable<SelectListItem> BatchSelectListItem
        {
            get { return new SelectList(Batches,"BatchId","BtRefNo"); }
        }

        public IEnumerable<SelectListItem> ItemSelectListItem
        {
            get { return new SelectList(Items, "BatchDetailId", "ItemName");}
        }

        public IEnumerable<SelectListItem> GroupSelectListItem
        {
            get { return new SelectList(GroupList, "GroupSubProcessId", "GroupName");}
        }
        public IEnumerable<SelectListItem> ApprovedSpSelectListItem
        {
            get
            {
                return new SelectList(new[] { new { Text = "Approved", Value = true }, new { Text = "Pending", Value = false } }, "Value", "Text");
            }
        }
    }
}