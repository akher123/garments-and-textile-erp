using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SCERP.Model;

namespace SCERP.Web.Areas.Merchandising.Models.ViewModel
{
    public class OrderTypeViewModel:OM_OrderType
    {
        public List<OM_OrderType> OrderTypes { get; set; }

        public OrderTypeViewModel()
        {
            OrderTypes=new List<OM_OrderType>();
        }
    }
}