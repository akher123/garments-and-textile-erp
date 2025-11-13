using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCERP.Model;
using SCERP.Model.UserRightManagementModel;


namespace SCERP.Web.Areas.UserManagement.Models.ViewModels
{
    public class NewModuleFeatureViewModel : VwModuleFeature
    {
        public List<Module> Modules { get; set; } 
        public List<ModuleFeature> ModuleFeatures{ get; set; }
        public List<VwModuleFeature> Features { get; set; }
        public List<ModuleFeature> ParentFeatureNames { get; set; }
      
        
        public NewModuleFeatureViewModel()
        {
            ModuleFeatures = new List<ModuleFeature>();
            Modules = new List<Module>();
            Features = new List<VwModuleFeature>();
            ParentFeatureNames=new List<ModuleFeature>();
        }

        public IEnumerable<SelectListItem> ModuleSelectListItem
        {
            get { return new SelectList(Modules, "Id", "ModuleName").ToList(); }
        }
        public IEnumerable<SelectListItem> ModuleFeatureSelectListItem 
        {
            get { return new SelectList(ModuleFeatures, "Id", "FeatureName").ToList(); }
        }
    } 
}