using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCERP.Model;
using SCERP.Model.InventoryModel;
using SCERP.Model.Planning;
using SCERP.Model.Production;

namespace SCERP.Web.Areas.Inventory.Models.ViewModels
{
    public class FabricReturnViewModel : ProSearchModel<FabricReturnViewModel>
    {
        public List<VwProgram> Programs { get; set; }
        public Inventory_FabricReturn FabricReturn { get; set; }
        public List<Inventory_FabricReturn> FabricReturns { get; set; }
        public List<VProgramDetail> ProgramDetails { get; set; }
          [Required]
        public long? ProgramDetailId { get; set; }
        public FabricReturnViewModel()
        {
            Programs=new List<VwProgram>();
            FabricReturn=new Inventory_FabricReturn();
            FabricReturns=new List<Inventory_FabricReturn>();
            ProgramDetails=new List<VProgramDetail>();
        }

        public IEnumerable<SelectListItem> ProgramDetailsSeletedItem
        {
            get
            {
                var rogramDetails = ProgramDetails.Select(x => new
                {
                   Text= x.ComponentName+"--"+"Color :"+x.ColorName+"--"+"F.Size :"+x.FinishSizeName+"--M/C Gauge :"+x.GSM+"--Pcs :"+x.NoOfCone,
                   Value = x.ProgramDetailId
                });
                return new SelectList(rogramDetails, "Value", "Text");
            }
        }
    }
}