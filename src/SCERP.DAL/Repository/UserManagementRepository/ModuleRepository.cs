using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Common;
using SCERP.DAL.IRepository.IUserManagementRepository;
using SCERP.Model;

namespace SCERP.DAL.Repository.UserManagementRepository
{
    public class ModuleRepository : Repository<Module>, IModuleRepository
    {
        public ModuleRepository(SCERPDBContext context)
            : base(context)
        {

        }

        public List<Module> GetModules()
        {
            return Context.Modules.Where(x => x.IsActive == true).OrderBy(y => y.ModuleName).ToList();
        }


        public Module GetModuleById(int id)
        {
            return Context.Modules.FirstOrDefault(x => x.IsActive == true && x.Id == id);
        }

        public Module CheckExistingModule(int id, string moduleName)
        {
            return Context.Modules.FirstOrDefault(x =>
                x.IsActive == true && x.Id != id &&
                x.ModuleName.Replace(" ", "").ToLower() == moduleName.Replace(" ", "").ToLower());
        }

        public Module GetModuleByName(string moduleName)
        {
            return Context.Modules.FirstOrDefault(x =>
                x.IsActive == true && String.Equals(x.ModuleName.Replace(" ", "").ToLower(),
                    moduleName.Replace(" ", "").ToLower()));
        }

        public List<Module> GetModulesByPaging(int pageIndex, string sort, string sortdir, string searchString, out int totalRecords)
        {
            var index = pageIndex;
            var pageSize = AppConfig.PageSize;
            IQueryable<Module> modules = Context.Modules.Where(x => x.IsActive == true && (
                                                                  (x.ModuleName.Contains(searchString) || String.IsNullOrEmpty(searchString))));

            totalRecords = modules.Count();
            switch (sort)
            {
                case "ModuleName":
                    switch (sortdir)
                    {
                        case "DESC":
                            modules = modules
                                .OrderByDescending(r => r.ModuleName)
                                .Skip(index * pageSize)
                                .Take(pageSize);

                            break;
                        default:
                            modules = modules
                                .OrderBy(r => r.ModuleName)
                                .Skip(index * pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;

                case "Description":
                    switch (sortdir)
                    {
                        case "DESC":
                            modules = modules
                                .OrderByDescending(r => r.Description)
                                .Skip(index * pageSize)
                                .Take(pageSize);

                            break;
                        default:
                            modules = modules
                                .OrderBy(r => r.Description)
                                .Skip(index * pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;
             
                default:
                    modules = modules
                        .OrderByDescending(r => r.ModuleName)
                        .Skip(index * pageSize)
                        .Take(pageSize);
                    break;
            }
            return modules.ToList();
        }


      
}
}
