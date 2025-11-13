using System;
using System.Collections.Generic;

using SCERP.Model;

namespace SCERP.Web.Areas.UserManagement.Models.ViewModels
{
    public class ModuleFeatureTreeView : ModuleFeature
    {
        public int ModuleFeatureId { get; set; }
        public ICollection<ModuleFeatureTreeView> ChaildModuleFeatures { get; set; }
        public ModuleFeatureTreeView()
        {
            ChaildModuleFeatures=new HashSet<ModuleFeatureTreeView>();
        }
    }
}