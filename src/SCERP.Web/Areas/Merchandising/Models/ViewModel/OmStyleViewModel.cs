using System.Collections.Generic;
using SCERP.Model.MerchandisingModel;

namespace SCERP.Web.Areas.Merchandising.Models.ViewModel
{
    public class OmStyleViewModel : VStyle
    {
        public List<VStyle> VStyles { get; set; }
        public bool IsExist { get; set; }
        public OmStyleViewModel()
        {
            VStyles=new List<VStyle>();
        }

    }
}