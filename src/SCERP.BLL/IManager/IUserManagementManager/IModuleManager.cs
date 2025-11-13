using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.DAL.IRepository;
using SCERP.Model;
using SCERP.Model.CommonModel;

namespace SCERP.BLL.IManager.IUserManagementManager
{
    public interface IModuleManager
    {
        List<Module> GetModules();
        Module GetModuleByName(string moduleName);
        Module CheckExistingModule(int id, string moduleName);
        int SaveModule(Module module);
        int EditModule(Module module);
        Module GetModuleById(int id);
        int DeleteModule(Module module);

        List<Module> GetModulesByPaging(int pageIndex, string sort, string sortdir, string searchString,
            out int totalRecords);
    }
}
