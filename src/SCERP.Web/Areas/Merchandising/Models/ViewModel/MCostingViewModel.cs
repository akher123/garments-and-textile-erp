using SCERP.Model.MerchandisingModel;
using System.Collections.Generic;
using SCERP.Model.Production;

namespace SCERP.Web.Areas.Merchandising.Models.ViewModel
{
    public class MCostingViewModel : ProSearchModel<OM_Costing>
    {
        public int CostingId { get; set; }
        public List<OM_Costing> MCostings { get; set; }
        public OM_Costing Costing { get; set; }
        public MCostingViewModel()
        {
            MCostings = new List<OM_Costing>();
            Costing = new OM_Costing();
        }
    }
}