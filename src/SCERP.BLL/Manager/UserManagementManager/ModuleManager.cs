using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using SCERP.BLL.IManager.IUserManagementManager;
using SCERP.Common;
using SCERP.DAL;
using SCERP.DAL.IRepository.IUserManagementRepository;
using SCERP.DAL.Repository.UserManagementRepository;
using SCERP.Model;
using SCERP.Model.CommonModel;

namespace SCERP.BLL.Manager.UserManagementManager
{
    public class ModuleManager : BaseManager, IModuleManager
    {

        public IModuleRepository _moduleRepository { get; set; }

        public ModuleManager(SCERPDBContext context)
        {
            _moduleRepository = new ModuleRepository(context);
        }

        public List<Module> GetModules()
        {
            List<Module> modules = null;
            try
            {
                modules = _moduleRepository.GetModules();
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                modules = null;
            }
            return modules;
        }


        public List<Module> GetModulesByPaging(int pageIndex, string sort, string sortdir, string searchString, out int totalRecords)
        {
            List<Module> modules = null;

            try
            {
                modules = _moduleRepository.GetModulesByPaging(pageIndex, sort, sortdir, searchString, out totalRecords);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                totalRecords = 0;
            }

            return modules;
        }
       

        public Module GetModuleById(int id)
        {
            Module module = null;

            try
            {
                module = _moduleRepository.GetModuleById(id);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                module = null;
            }
            return module;
        }

        public Module CheckExistingModule(int id, string moduleName)
        {
            Module module = null;

            try
            {
                module = _moduleRepository.CheckExistingModule(id, moduleName);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                module = null;
            }
            return module;
        }

        public Module GetModuleByName(string moduleName)
        {
            Module module = null;
            try
            {
                module = _moduleRepository.GetModuleByName(moduleName);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                module = null;
            }
            return module;
        }

        public int SaveModule(Module module)
        {
            var savedModule = 0;
            try
            {
                module.CDT = DateTime.Now;
                module.CreatedBy = PortalContext.CurrentUser.UserId;
                module.IsActive = true;
                savedModule = _moduleRepository.Save(module);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                savedModule = 0;
            }

            return savedModule;
        }

        public int EditModule(Module module)
        {
            var editedModule = 0;
            try
            {
                module.EDT = DateTime.Now;
                module.EditedBy = PortalContext.CurrentUser.UserId;
                editedModule = _moduleRepository.Edit(module);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                editedModule = 0;
            }

            return editedModule;
        }

        public int DeleteModule(Module module)
        {
           
            var deletedModule = 0;
            try
            {
                module.EDT = DateTime.Now;
                module.EditedBy = PortalContext.CurrentUser.UserId;
                module.IsActive = false;

                deletedModule = _moduleRepository.Edit(module);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                deletedModule = 0;
            }

            return deletedModule;
        }


    }
}
