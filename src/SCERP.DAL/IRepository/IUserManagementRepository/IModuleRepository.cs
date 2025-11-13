using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;

namespace SCERP.DAL.IRepository.IUserManagementRepository
{
    public interface IModuleRepository : IRepository<Module>
    {
        List<Module> GetModules();
        Module GetModuleById(int id);
        Module GetModuleByName(string moduleName);
        Module CheckExistingModule(int id, string moduleName);

       List<Module> GetModulesByPaging(int pageIndex, string sort, string sortdir, string searchString,
            out int totalRecords);
    }
}
