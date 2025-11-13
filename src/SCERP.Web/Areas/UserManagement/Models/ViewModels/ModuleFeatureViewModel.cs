using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SCERP.Model;
using System.Web.Mvc;

namespace SCERP.Web.Areas.UserManagement.Models.ViewModels
{
    public class ModuleFeatureViewModel : ModuleFeature
    {
        public List<Module> Modules { get; set; }
        public List<ModuleFeature> ModuleFeatures { get; set; }
        public List<ModuleFeature> Features { get; set; }
        public List<ModuleFeature> ParentFeatureNames { get; set; }
        public IEnumerable ModuleNames { get; set; }
        public int SearchByModuleName { get; set; }
        public string SearchByFeatureNames { get; set; }
        public ModuleFeatureViewModel()
        {
            Modules = new List<Module>();
            ModuleFeatures = new List<ModuleFeature>();
            IsSearch = true;
           
            Features = new List<ModuleFeature>();
        }
        public IEnumerable<SelectListItem> ModuleNameSelectListItem
        {
            get { return new SelectList(ModuleNames, "Id", "ModuleName").ToList(); }
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