using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SCERP.Model;

namespace SCERP.Web.Areas.Merchandising.Models.ViewModel
{
    public class TempGroupViewModel
    {
       public string sort { get; set; }
       public string sortdir { get; set; }
        public int TotalRecords { get; set; }
        public int? page { get; set; }
        public int PageIndex
        {
            get
            {
                int index = 0;
                //  int pageSize = AppConfig.PageSize;
                if (page.HasValue && page.Value > 0)
                {
                    index = page.Value - 1;
                }
                return index;
            }
        }
        public TempGroupViewModel()
        {
            TempGroup=new OM_TempGroup();
            TempGroups=new List<OM_TempGroup>();
        }
        public OM_TempGroup TempGroup { get; set; }
        public List<OM_TempGroup> TempGroups { get; set; }
      
    }
}