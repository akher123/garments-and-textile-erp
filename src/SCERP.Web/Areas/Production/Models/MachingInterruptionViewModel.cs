using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using SCERP.Model.Production;

namespace SCERP.Web.Areas.Production.Models
{
    public class MachingInterruptionViewModel : ProSearchModel<MachingInterruptionViewModel>
    {
        public MachingInterruptionViewModel()
        {
          MachingInterruption=new PROD_MachingInterruption();
          MachingInterruptions = new List<SpProdMatchingInterruption>();
        }
        public PROD_MachingInterruption MachingInterruption { get; set; }
        public List<SpProdMatchingInterruption> MachingInterruptions { get; set; } 
    }
}