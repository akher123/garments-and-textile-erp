using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace SCERP.Web.Areas.MIS.Models.ViewModel
{
    public class FabricStatusViewModel
    {
        public DataTable YDeliveryDtl { get; set; }
        public DataTable RollRcvDtl { get; set; }
        public DataTable DyeingDtl { get; set; }
        public DataTable FinishingDtl { get; set; }
    }
}