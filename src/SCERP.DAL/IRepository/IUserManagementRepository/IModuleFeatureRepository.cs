using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;
using SCERP.Model.UserRightManagementModel;


namespace SCERP.DAL.IRepository.IUserManagementRepository
{
    public interface IModuleFeatureRepository : IRepository<ModuleFeature>
    {
       // List<ModuleFeature> GetModuleFeaturesByPaging(int startPage, int pageSize, out int totalRecords, ModuleFeature feature);
        ModuleFeature GetModuleFeatureById(int id);
        ModuleFeature GetModuleFeatureByName(int moduleId, string featureName);
        ModuleFeature CheckExistingModuleFeature(int id, int moduleId, string moduleFeatureName);
        List<ModuleFeatureViewModel> GetMouduleFeatureByUser(string userNmae);

        List<ModuleFeature> GetAllParentFeatureName();
        List<ModuleFeature> GetAllParentFearureName();
        IQueryable<VwModuleFeature> GetVwModuleFeatures(Expression<Func<VwModuleFeature,bool>>predicate );
    }
}
