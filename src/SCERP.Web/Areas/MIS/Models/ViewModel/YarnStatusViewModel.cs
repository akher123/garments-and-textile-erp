using SCERP.Model.MerchandisingModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace SCERP.Web.Areas.MIS.Models.ViewModel
{
    public class YarnStatusViewModel
    {

        public List<VYarnConsumption> YConDtl { get; set; }
        public List<VCompConsumptionDetail> VCompConsumptionDetails { get; set; }
        public decimal? TotalConsQty { get { return YConDtl.Sum(x => x.KQty); } }
        public DataTable YBookingDtl { get; set; }
        public DataTable YRcvDtl { get; set; }
        
    }
}