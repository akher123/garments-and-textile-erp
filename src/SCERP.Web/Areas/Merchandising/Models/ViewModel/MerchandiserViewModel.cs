using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SCERP.Model;

namespace SCERP.Web.Areas.Merchandising.Models.ViewModel
{
    public class MerchandiserViewModel:OM_Merchandiser
    {
        public List<OM_Merchandiser> Merchandisers { get; set; }
        public MerchandiserViewModel()
        {
            Merchandisers=new List<OM_Merchandiser>();
        }

       
    }
}