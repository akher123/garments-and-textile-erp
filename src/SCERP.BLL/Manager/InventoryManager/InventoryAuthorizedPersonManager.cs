using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using SCERP.BLL.IManager.IInventoryManager;
using SCERP.Common;
using SCERP.DAL;
using SCERP.DAL.IRepository.IHRMRepository;
using SCERP.DAL.IRepository.IInventoryRepository;
using SCERP.DAL.Repository.HRMRepository;
using SCERP.DAL.Repository.InventoryRepository;
using SCERP.Model;

namespace SCERP.BLL.Manager.InventoryManager
{
    public class InventoryAuthorizedPersonManager : IInventoryAuthorizedPersonManager
    {
        private readonly IInventoryAuthorizedPersonRepository _inventoryAuthorizedPersonRepository;
        private IEmployeeCompanyInfoRepository _employeeCompanyInfoRepository;
        public InventoryAuthorizedPersonManager(SCERPDBContext context)
        {
            _inventoryAuthorizedPersonRepository = new InventoryAuthorizedPersonRepository(context);
            _employeeCompanyInfoRepository = new EmployeeCompanyInfoRepository(context);
        }

        public List<Inventory_AuthorizedPerson> GetInventoryAuthorizedPersonsByPaging(out int totalRecords, Inventory_AuthorizedPerson model)
        {
            var index = model.PageIndex;
            var pageSize = AppConfig.PageSize;
            var authorizedPersons =
                _inventoryAuthorizedPersonRepository.Filter(x => x.IsActive).Include(x => x.Employee)
                .Where(x => (x.ProcessId == model.ProcessId || model.ProcessId == 0)
                    && ((x.ProcessTypeId == model.ProcessTypeId || model.ProcessTypeId == 0)));
            totalRecords = authorizedPersons.Count();
            switch (model.sort)
            {
                case "Employee.Name":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            authorizedPersons = authorizedPersons
                                .OrderByDescending(r => r.Employee.Name)
                                .Skip(index * pageSize)
                                .Take(pageSize);

                            break;
                        default:
                            authorizedPersons = authorizedPersons
                           .OrderBy(r => r.Employee.Name)
                           .Skip(index * pageSize)
                           .Take(pageSize);
                            break;
                    }
                    break;
                case "ProcessName":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            authorizedPersons = authorizedPersons
                             .OrderByDescending(r => r.ProcessName)
                             .Skip(index * pageSize)
                             .Take(pageSize);
                            break;
                        default:
                            authorizedPersons = authorizedPersons
                            .OrderByDescending(r => r.ProcessName)
                            .Skip(index * pageSize)
                            .Take(pageSize);
                            break;
                    }

                    break;
                case "ProcessTypeName":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            authorizedPersons = authorizedPersons
                             .OrderByDescending(r => r.ProcessTypeName)
                             .Skip(index * pageSize)
                             .Take(pageSize);
                            break;
                        default:
                            authorizedPersons = authorizedPersons
                            .OrderByDescending(r => r.ProcessTypeName)
                            .Skip(index * pageSize)
                            .Take(pageSize);
                            break;
                    }

                    break;
                default:
                    authorizedPersons = authorizedPersons
                                  .OrderBy(r => r.ProcessTypeId)
                                  .Skip(index * pageSize)
                                  .Take(pageSize);
                    break;
            }
        
            return authorizedPersons.ToList();
        }

        public Inventory_AuthorizedPerson GetAuthorizedPersonById(int authorizedPersonId)
        {
            return _inventoryAuthorizedPersonRepository.Filter(x => x.IsActive).Include(x => x.Employee).SingleOrDefault(x => x.AuthorizedPersonId == authorizedPersonId);
        }

        public VEmployeeCompanyInfoDetail GetEmployeeByEmployeeCardId(string employeeCardId)
        {
            return _employeeCompanyInfoRepository.GetEmployeeCompanyInfoByEmployeeCardId(employeeCardId);
        }

        public int SaveInventoryAuthorizedPerson(Inventory_AuthorizedPerson model)
        {
            int saveIndex = 0;
            try
            {
                var authorizedPerson = new Inventory_AuthorizedPerson
                {
                    ProcessId = model.ProcessId,
                    EmployeeCardId = model.EmployeeCardId,
                    ProcessName = model.ProcessName,
                    ProcessTypeName = model.ProcessTypeName,
                    ProcessTypeId = model.ProcessTypeId,
                    EmployeeId = model.EmployeeId,
                    CreatedBy = PortalContext.CurrentUser.UserId,
                    CreatedDate = DateTime.Now,
                    IsActive = true
                };
                saveIndex = _inventoryAuthorizedPersonRepository.Save(authorizedPerson);
            }
            catch (Exception exception)
            {

                throw;
            }
            return saveIndex;
        }

        public int EditInventoryAuthorizedPerson(Inventory_AuthorizedPerson model)
        {
            var editIndex = 0;
            try
            {
                var authorizedPerson =
                    _inventoryAuthorizedPersonRepository.FindOne(x => x.AuthorizedPersonId == model.AuthorizedPersonId);
                authorizedPerson.ProcessId = model.ProcessId;
                authorizedPerson.ProcessName = model.ProcessName;
                authorizedPerson.EmployeeCardId = model.EmployeeCardId;
                authorizedPerson.ProcessTypeName = model.ProcessTypeName;
                authorizedPerson.ProcessTypeId = model.ProcessTypeId;
                authorizedPerson.EmployeeId = model.EmployeeId;
                authorizedPerson.EditedBy = PortalContext.CurrentUser.UserId;
                authorizedPerson.EditedDate = DateTime.Now;
                model.IsActive = true;
                editIndex = _inventoryAuthorizedPersonRepository.Edit(authorizedPerson);
            }
            catch (Exception exception)
            {

                throw;
            }
            return editIndex;
        }

        public int DeleteInventoryAuthorizedPerson(int authorizedPersonId)
        {
            var edit = 0;
            try
            {
                var authorizedPerson =
                     _inventoryAuthorizedPersonRepository.FindOne(x => x.AuthorizedPersonId == authorizedPersonId);

                authorizedPerson.EditedBy = PortalContext.CurrentUser.UserId;
                authorizedPerson.EditedDate = DateTime.Now;
                authorizedPerson.IsActive = false;
                edit = _inventoryAuthorizedPersonRepository.Edit(authorizedPerson);

            }
            catch (Exception exception)
            {

                throw exception;
            }
            return edit;
        }

        public bool CheckUserIsStorePerson(int processTypeId, int processId, Guid? userId)
        {
           return _inventoryAuthorizedPersonRepository.CheckUserIsStorePerson(processTypeId, processId, userId);
        
        }

        public List<Inventory_AuthorizedPerson> GetAuthorizedPersonsByProcessTypeId(int processTypeId,int processId)
        {
            return
                _inventoryAuthorizedPersonRepository.Filter(x => x.IsActive && x.ProcessTypeId == processTypeId && x.ProcessId == processId).Include(x => x.Employee).Where(x => x.Employee.Status == 1)
                    .ToList();
        }

        public IEnumerable GetAuthorizedPersons(int processTypeId, int processId)
        {
            return _inventoryAuthorizedPersonRepository.Filter(x => x.IsActive && x.ProcessTypeId == processTypeId&&x.ProcessId==processId).Include(x => x.Employee).Where(x => x.Employee.Status == 1).Select(x=>new {Name=x.Employee.Name,EmployeeId=x.EmployeeId});
               
        }

        public int FindSoterRequisiotionProcessId(int storePurchaseRequisition, Guid? userId)
        {
            var processId = 0;
            var inventoryAuthorizedPerson = _inventoryAuthorizedPersonRepository.FindOne(
                x =>
                    x.ProcessTypeId == (int) InventoryProcessType.StorePurchaseRequisition && x.EmployeeId == userId&&x.IsActive);
            if (inventoryAuthorizedPerson!=null)
            {
                processId = inventoryAuthorizedPerson.ProcessId;
            }
            return processId;
        }

        public bool IsExistAuthorizedPerson(Inventory_AuthorizedPerson model)
        {
            return
                _inventoryAuthorizedPersonRepository.Exists(
                    x =>x.IsActive&&x.AuthorizedPersonId!=model.AuthorizedPersonId&&x.EmployeeCardId == model.EmployeeCardId && x.ProcessId == model.ProcessId && x.ProcessTypeId == model.ProcessTypeId&&x.IsActive);
        }
    }
}
