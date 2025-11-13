using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using SCERP.BLL.IManager.IUserManagementManager;
using SCERP.Common;
using SCERP.DAL;
using SCERP.DAL.IRepository.IUserManagementRepository;
using SCERP.DAL.Repository.UserManagementRepository;
using SCERP.Model;
using SCERP.Model.UserRightManagementModel;

namespace SCERP.BLL.Manager.UserManagementManager
{
    public class ModuleFeatureManager : BaseManager, IModuleFeatureManager
    {

        private readonly IModuleFeatureRepository _moduleFeatureRepository = null;
        private readonly IUserRoleRepository userRoleRepository;
        public ModuleFeatureManager(SCERPDBContext context)
        {
            _moduleFeatureRepository = new ModuleFeatureRepository(context);
            this.userRoleRepository = new UserRoleRepository(context); ;
        }


        public List<VwModuleFeature> GetModuleFeaturesByPaging(VwModuleFeature model, out int totalRecords)
        {
            var index = model.PageIndex;
            var pageSize = AppConfig.PageSize;
            IQueryable<VwModuleFeature> vWmoduleFeatures = null;
            if (model.ParentFeatureId != null && model.ModuleId != 0 && model.SearchString != null)
            {
                vWmoduleFeatures = _moduleFeatureRepository.GetVwModuleFeatures(x => x.IsActive == true
                                   && ((x.ModuleId == model.ModuleId) && (x.ParentFeatureId != 0) && (x.ParentFeatureId == model.ParentFeatureId) && (x.FeatureName.Trim().Contains(model.SearchString.Trim()))));
            }
            else if (model.ParentFeatureId != null && model.ModuleId != 0 )
            {
                vWmoduleFeatures = _moduleFeatureRepository.GetVwModuleFeatures(x => x.IsActive == true
                                    && ((x.ModuleId == model.ModuleId || x.ModuleId == 0) &&(x.ParentFeatureId !=0) && (x.ParentFeatureId == model.ParentFeatureId || x.ParentFeatureId == 0)));
            }
             else if(model.ModuleId !=0)
            {
                vWmoduleFeatures = _moduleFeatureRepository.GetVwModuleFeatures(x => x.IsActive == true
                                    && (x.FeatureName.Trim().Contains(model.SearchString.Trim()) || String.IsNullOrEmpty(model.SearchString.Trim().ToLower())
                                    && (x.ModuleId == model.ModuleId || model.ModuleId == 0)
                                    && (x.ParentFeatureId != 0)
                                    && (x.ParentFeatureId == model.Id || model.Id == 0)));
            }
             else
             {
                 vWmoduleFeatures = _moduleFeatureRepository.GetVwModuleFeatures(x => x.IsActive == true
                                   && (x.FeatureName.Trim().Contains(model.SearchString.Trim()) || String.IsNullOrEmpty(model.SearchString.Trim().ToLower())));
             }
            totalRecords = vWmoduleFeatures.Count();
            switch (model.sort)
            {
                case "ModuleName":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            vWmoduleFeatures = vWmoduleFeatures
                                 .OrderByDescending(r => r.ModuleName).ThenByDescending(x => x.OrderId)
                                .Skip(index * pageSize)
                                .Take(pageSize);

                            break;
                        default:
                            vWmoduleFeatures = vWmoduleFeatures
                                 .OrderBy(r => r.ModuleName).ThenBy(x=>x.OrderId)
                                .Skip(index * pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;
                    case "FeatureName":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            vWmoduleFeatures = vWmoduleFeatures
                                 .OrderByDescending(r => r.FeatureName).ThenByDescending(x=>x.OrderId)
                                .Skip(index * pageSize)
                                .Take(pageSize);

                            break;
                        default:
                            vWmoduleFeatures = vWmoduleFeatures
                                 .OrderBy(r => r.FeatureName).ThenBy(x=>x.OrderId)
                                .Skip(index * pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;
                default:
                    vWmoduleFeatures = vWmoduleFeatures
                        .OrderByDescending(r => r.Id)
                        .Skip(index * pageSize)
                        .Take(pageSize);
                    break;
            }
            return vWmoduleFeatures.ToList();
        }

        public List<ModuleFeature> GetAllParentFeatureName()
        {
            return _moduleFeatureRepository.GetAllParentFeatureName();
        }

        public List<ModuleFeature> GetParentFeatures(int parentModuleId)
        {
            return _moduleFeatureRepository.Filter(x => x.IsActive == true && x.ParentFeatureId == parentModuleId).ToList();
        }

        public List<ModuleFeature> GetAllParentFearureName()
        {
            return _moduleFeatureRepository.GetAllParentFearureName();
        }

        public List<ModuleFeature> GetAllModuleFeatures()
        {
            List<ModuleFeature> moduleFeatures = null;

            try
            {
                moduleFeatures = _moduleFeatureRepository.All().Include(x => x.Module).Where(x => x.IsActive == true).OrderBy(y => y.Module.ModuleName).ToList();
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                moduleFeatures = null;
            }

            return moduleFeatures;
        }
        public List<ModuleFeature> GetModuleFeatures(string compId, bool isSystemUser,string userName)
        {
            if (!isSystemUser)
            {
                List<int> moduelfeatures = userRoleRepository.Filter(x => x.CompId == compId&&x.UserName==userName).Select(x => x.ModuleFeatureId).Distinct().ToList();
                return _moduleFeatureRepository.All().Where(x => x.IsActive == true && moduelfeatures.Contains(x.Id)).OrderBy(z => z.ParentFeatureId).ToList();
            }
            else 
            {
                return _moduleFeatureRepository.All().Where(x => x.IsActive == true).OrderBy(z => z.ParentFeatureId).ToList();
            }

        }
        public List<ModuleFeatureViewModel> GetModuleFeaturesByUser(string userName)
        {
            return _moduleFeatureRepository.GetMouduleFeatureByUser(userName);
        }

        public int DeleteModuleFeature(int id)
        {
            var moduleFeature = _moduleFeatureRepository.FindOne(x => x.Id == id && x.IsActive == true);
            moduleFeature.IsActive = false;
            return _moduleFeatureRepository.Edit(moduleFeature);
        }

        public ModuleFeature GetModuleFeatureById(int id)
        {
            ModuleFeature moduleFeature = null;

            try
            {
                moduleFeature = _moduleFeatureRepository.GetModuleFeatureById(id);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                moduleFeature = null;
            }
            return moduleFeature;
        }

        public bool CheckExistingModuleFeature(int id, int moduleId, string moduleFeatureName)
        {
            return _moduleFeatureRepository.Exists(x => x.IsActive == true && x.Id != id && x.ModuleId == moduleId && x.FeatureName.Replace(" ", "").ToLower() == moduleFeatureName.Replace(" ", "").ToLower());

        }

        public ModuleFeature GetModuleFeatureByName(int moduleId, string featureName)
        {
            ModuleFeature moduleFeature = null;

            try
            {
                moduleFeature = _moduleFeatureRepository.GetModuleFeatureByName(moduleId, featureName);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                moduleFeature = null;
            }
            return moduleFeature; 
        }


        public int SaveModuleFeature(ModuleFeature moduleFeature)
        {
            if (moduleFeature.ParentFeatureId == null)
            {
                moduleFeature.ParentFeatureId = 0;
            }
            moduleFeature.CDT = DateTime.Now;
            moduleFeature.CreatedBy = PortalContext.CurrentUser.UserId;
            moduleFeature.IsActive = true;
            return _moduleFeatureRepository.Save(moduleFeature);
          
        }

        public int EditModuleFeature(ModuleFeature model)
        {

            var moduleFeature = _moduleFeatureRepository.FindOne(x=>x.Id==model.Id&&x.IsActive==true);
            moduleFeature.ModuleId = model.ModuleId;
            moduleFeature.FeatureName = model.FeatureName;
            moduleFeature.ParentFeatureId = model.ParentFeatureId ?? 0;
            moduleFeature.OrderId = model.OrderId;
            moduleFeature.NavURL = model.NavURL;
            moduleFeature.ShowInMenu = model.ShowInMenu;
            moduleFeature.EDT = DateTime.Now;
            moduleFeature.CreatedBy = PortalContext.CurrentUser.UserId;
            moduleFeature.IsActive = true;
            return _moduleFeatureRepository.Edit(moduleFeature);
        }
        public List<ModuleFeature> GetFeaturesByModule(int moduleId)
        {
            return _moduleFeatureRepository.Filter(x => x.IsActive==true && x.ModuleId == moduleId).OrderBy(y=>y.FeatureName).ToList();
        }

        public int GetMaxOrderIdByParentFeatureId(int parentFeatureId)
        {
            var orderId = _moduleFeatureRepository.Filter(x => x.ParentFeatureId == parentFeatureId).ToList().Max(x => (int?)x.OrderId) ?? 0;
            return orderId+1;

        }
    }
}
