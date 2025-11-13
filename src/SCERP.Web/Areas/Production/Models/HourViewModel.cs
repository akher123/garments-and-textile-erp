using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SCERP.Model.Production;

namespace SCERP.Web.Areas.Production.Models
{
    public class HourViewModel : ProSearchModel<PROD_Hour>
    {
        public HourViewModel()
        {
            Hour=new PROD_Hour();
            Hours=new List<PROD_Hour>();
        }
       public PROD_Hour Hour { get; set; }
       public List<PROD_Hour> Hours { get; set; }  
    }
}