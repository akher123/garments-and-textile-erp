using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SCERP.Model.MerchandisingModel;
using SCERP.Model.Production;

namespace SCERP.Web.Areas.Merchandising.Models.ViewModel
{
    public class SampleStyleViewModel:ProSearchModel<OM_SampleStyle>
    {
        public SampleStyleViewModel()
        {
            SampleStyle=new OM_SampleStyle();
            SampleStyles=new List<OM_SampleStyle>();
        }

        public int SampleOrderId { get; set; }
        public List<OM_SampleStyle> SampleStyles { get; set; }
        public OM_SampleStyle SampleStyle { get; set; }
    }
}