using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.Production;

namespace SCERP.Model.UserRightManagementModel
{
    public class VwModuleFeature: ProSearchModel<Branch>
    {
        public int Id { get; set; }
        public int ModuleId { get; set; }
        public string ModuleName { get; set; }
        public Nullable<int> ParentFeatureId { get; set; }
        public string FeatureName { get; set; }
        public string NavURL { get; set; }
        public Nullable<bool> ShowInMenu { get; set; }
        public Nullable<int> OrderId { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public string ParentFeatureName { get; set; }
    }
}
