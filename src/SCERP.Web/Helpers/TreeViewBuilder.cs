using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SCERP.Model;
using SCERP.Web.Areas.UserManagement.Models.ViewModels;

namespace SCERP.Web.Helpers
{
    public class TreeViewBuilder
    {
        private List<ModuleFeature> moduleFeatures = new List<ModuleFeature>();
        public TreeViewBuilder(List<ModuleFeature> moduleFeatures)
        {
            this.moduleFeatures = moduleFeatures;
        }

        private List<ModuleFeature> GetZeroParentFeatureModuleFeatures()
        {
            return moduleFeatures.Where(x => x.ParentFeatureId == 0).ToList();
        }

        private bool GetNextModuleFeature(int moduleFeatureId)
        {
            return moduleFeatures.Any(x => x.ParentFeatureId == moduleFeatureId);
        }

        private List<ModuleFeatureTreeView> GetAllModulFeature(int moduleFeatureId)
        {
            List<ModuleFeatureTreeView> list = new List<ModuleFeatureTreeView>();
            List<ModuleFeature> features = moduleFeatures.Where(x => x.ParentFeatureId == moduleFeatureId).ToList();

            foreach (ModuleFeature moduleFeature in features)
            {
                list.Add(new ModuleFeatureTreeView
                {
                    ModuleFeatureId = moduleFeature.Id,
                    Id = moduleFeature.Id,
                    ParentFeatureId = moduleFeature.ParentFeatureId,
                    FeatureName = moduleFeature.FeatureName
                });
            }

            return list;
        }

    


        private List<ModuleFeature> GetModuls()
        {
            return moduleFeatures.GroupBy(x => x.Module).Select(x=>x.First()).ToList();
        } 

        private List<ModuleFeatureTreeView> GetTreeView()
        {
            List<ModuleFeatureTreeView> parentTreeViews = new List<ModuleFeatureTreeView>();
            List<ModuleFeature> zeroParentFeatureModuleFeatures = GetZeroParentFeatureModuleFeatures();
            ModuleFeatureTreeView  parent=new ModuleFeatureTreeView();
           
            foreach (ModuleFeature zeroParentFeatureModuleFeature in zeroParentFeatureModuleFeatures)
            {
               
                bool nextModuleFeature = GetNextModuleFeature(zeroParentFeatureModuleFeature.Id);

                if (nextModuleFeature)
                {
                    if (zeroParentFeatureModuleFeature.ParentFeatureId != null)
                       parent= new ModuleFeatureTreeView{
                            ModuleFeatureId = zeroParentFeatureModuleFeature.Id,
                            Id = zeroParentFeatureModuleFeature.Id,
                            ModuleId=zeroParentFeatureModuleFeature.ModuleId,
                            ParentFeatureId = zeroParentFeatureModuleFeature.ParentFeatureId,
                            FeatureName = zeroParentFeatureModuleFeature.FeatureName,
                            ChaildModuleFeatures = GetAllModulFeature(zeroParentFeatureModuleFeature.Id)
                        };
                    parentTreeViews.Add(parent);
                }
                else
                {
                    parentTreeViews.Add(new ModuleFeatureTreeView
                    {
                        ModuleFeatureId = zeroParentFeatureModuleFeature.Id,
                        Id = zeroParentFeatureModuleFeature.Id,
                        ParentFeatureId = zeroParentFeatureModuleFeature.ParentFeatureId,
                        FeatureName = zeroParentFeatureModuleFeature.FeatureName,
                        ModuleId=zeroParentFeatureModuleFeature.ModuleId,
                    });
                }

            }
            return parentTreeViews;
        }
      

        public List<ModuleFeatureTreeView> GetTreeViewWithModule()
        {

            var parentList=new List<ModuleFeatureTreeView>();
            List<ModuleFeature> features = GetModuls();
            foreach (ModuleFeature moduleFeature in features)
            {
               parentList.Add(new ModuleFeatureTreeView()
               {
                   ModuleFeatureId = moduleFeature.Id,
                   Id = moduleFeature.Id,
                   ParentFeatureId = moduleFeature.ParentFeatureId,
                   FeatureName = moduleFeature.Module.ModuleName,
                   ChaildModuleFeatures = GetAllModuleFeatureByModuleId(moduleFeature.Module.Id)
               });
            }
            return parentList;
        }


        private List<ModuleFeatureTreeView> GetAllModuleFeatureByModuleId(int mduleId)
        {
            return GetTreeView().Where(x => x.ModuleId == mduleId).ToList();
        }

        public List<ModuleFeatureTreeView> GetModuleFeatureTreeView()
        {
            return moduleFeatures.Select(moduleFeature => new ModuleFeatureTreeView
            {
                Id = moduleFeature.Id, ModuleFeatureId = moduleFeature.Id, ParentFeatureId = moduleFeature.ParentFeatureId, FeatureName = moduleFeature.FeatureName, Module = moduleFeature.Module,
            }).ToList();
        }
    }
}