using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using SCERP.Common;
using SCERP.DAL.IRepository.IUserManagementRepository;
using SCERP.Model;
using SCERP.Model.UserRightManagementModel;

namespace SCERP.DAL.Repository.UserManagementRepository
{
    public class ModuleFeatureRepository : Repository<ModuleFeature>, IModuleFeatureRepository
    {
        public ModuleFeatureRepository(SCERPDBContext context)
            : base(context)
        {
        }

        public ModuleFeature GetModuleFeatureById(int id)
        {
            return
                Context.ModuleFeatures.Where(x => x.Id == id && x.IsActive == true)
                    .Include(x => x.Module)
                    .FirstOrDefault();
        }

        public ModuleFeature CheckExistingModuleFeature(int id, int moduleId, string moduleFeatureName)
        {
            return Context.ModuleFeatures.FirstOrDefault(x => x.IsActive == true && x.Id != id && x.ModuleId == moduleId && x.FeatureName.Replace(" ", "").ToLower() == moduleFeatureName.Replace(" ", "").ToLower());
        }

        public ModuleFeature GetModuleFeatureByName(int moduleId, string featureName)
        {
            return Context.ModuleFeatures.FirstOrDefault(x => x.IsActive == true && x.ModuleId == moduleId && String.Equals(x.FeatureName.Replace(" ", "").ToLower(), featureName.Replace(" ", "").ToLower()));
        }

        public List<ModuleFeatureViewModel> GetMouduleFeatureByUser(string userNmae)
        {
            var moduleFeature = from modulefeature in Context.ModuleFeatures
                join userrole in Context.UserRoles on modulefeature.Id equals userrole.ModuleFeatureId
                    into xG
                from x1 in xG.DefaultIfEmpty()
                where x1.UserName == userNmae && modulefeature.IsActive == true && modulefeature.ShowInMenu == true
                orderby modulefeature.ParentFeatureId ascending, modulefeature.OrderId ascending
                select new ModuleFeatureViewModel
                {
                    ModuleFeature = modulefeature,
                    UserRole = x1
                };

            return moduleFeature.ToList();
        }

        public List<ModuleFeature> GetAllParentFeatureName()
        {
      //      var sqlQuery =from m in t.Employees join e1 in t.Employees on m.ManagerID equals e1.EmployeeID
      // select new NewModuleFeatureViewModel
      //{
      //  EmployeeID = m.EmployeeID,
      //  Name = m.Name,
      //  ManagerName = e1.Name,
      //  Designation = m.Designation,
      //  Phone = m.Phone,
      //  Address = m.Address
      // };

            return null;
        }

        public List<ModuleFeature> GetAllParentFearureName()
        {
            //var parentFeatureNameList = from a in Context.ModuleFeatures
            //                            join b in Context.Modules on a.ModuleId equals b.Id
            //                            join t in Context.ModuleFeatures on a.ParentFeatureId=t.Id
            //                            where a.IsActive == true 
            //                            select new ModuleFeature
            //                            {

            //                                EmployeeCompanyInfo = employeeCompanyInfo
            //                            };
            //return parentFeatureNameList;
            return null;
        }

        public IQueryable<VwModuleFeature> GetVwModuleFeatures(Expression<Func<VwModuleFeature, bool>> predicate)
        {
            return Context.VwModuleFeatures.Where(predicate);
        }
    }
}
