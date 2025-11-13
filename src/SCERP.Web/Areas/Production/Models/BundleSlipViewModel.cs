using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCERP.Model;
using SCERP.Model.Production;

namespace SCERP.Web.Areas.Production.Models
{
    public class BundleSlipViewModel:SearchModel<BundleSlipViewModel>
    {
        public BundleSlipViewModel()
        {
         
           CuttingBatch=new PROD_CuttingBatch();
        }
        public PROD_CuttingBatch CuttingBatch { get; set; }
        public DataTable DataTable { get; set; }
      
    }
}