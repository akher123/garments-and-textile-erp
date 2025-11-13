using System;
using System.Collections;
using System.Linq;
using SCERP.Common;
using SCERP.Model;
using SCERP.Model.Custom;
using System.Collections.Generic;
using System.Web.Mvc;

namespace SCERP.Web.Areas.Planning.Models.ViewModels
{
    public class ProductionLineStatusViewModel : PLAN_ProductionLineStatus
    {
        public ProductionLineStatusViewModel()
        {
            ProductionLineStatuses = new List<PLAN_ProductionLineStatus>();  
            SearchFieldModel = new SearchFieldModel();           
            ProductionLines = new List<object>();
            IsSearch = true;
        }

        public List<PLAN_ProductionLineStatus> ProductionLineStatuses { get; set; }
        public SearchFieldModel SearchFieldModel { get; set; }

        public IEnumerable ProductionLines { get; set; }
        public IEnumerable<SelectListItem> ProductionLineSelectListItem
        {
            get { return new SelectList(ProductionLines, "ProductionLineId", "Name"); }

        }
    }
}