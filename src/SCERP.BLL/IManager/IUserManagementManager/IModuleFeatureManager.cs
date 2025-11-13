using System.Collections;
using SCERP.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.UserRightManagementModel;

namespace SCERP.BLL.IManager.IUserManagementManager
{
    public interface IModuleFeatureManager
    {
        List<ModuleFeature> GetAllModuleFeatures();

        int SaveModuleFeature(ModuleFeature moduleFeature);

        int EditModuleFeature(ModuleFeature moduleFeature);

        ModuleFeature GetModuleFeatureById(int id);

        bool CheckExistingModuleFeature(int id, int moduleId, string moduleFeatureName);

        ModuleFeature GetModuleFeatureByName(int moduleId, string featureName);

        List<ModuleFeature> GetModuleFeatures(string compId, bool isSystemUser, string userName);
        
        List<ModuleFeatureViewModel> GetModuleFeaturesByUser(string userName);
        int DeleteModuleFeature(int id); 
        List<ModuleFeature> GetFeaturesByModule(int moduleId);
        int GetMaxOrderIdByParentFeatureId(int parentFeatureId);
        List<VwModuleFeature> GetModuleFeaturesByPaging(VwModuleFeature model, out int totalRecords);

        List<ModuleFeature> GetAllParentFeatureName();
        List<ModuleFeature> GetParentFeatures(int ParentFeatureId);
        List<ModuleFeature> GetAllParentFearureName(); 
    }
}
