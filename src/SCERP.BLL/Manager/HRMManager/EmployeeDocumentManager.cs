using System;
using SCERP.BLL.IManager.IHRMManager;
using SCERP.Common;
using SCERP.DAL;
using SCERP.DAL.IRepository.IHRMRepository;
using SCERP.DAL.Repository.HRMRepository;
using System.Linq;
using SCERP.Model;
using System.Collections.Generic;

namespace SCERP.BLL.Manager.HRMManager
{
    public class EmployeeDocumentManager : BaseManager, IEmployeeDocumentManager
    {
        private readonly IEmployeeDocumentRepository _employeeDocumentRepository = null;
     
        public List<EmployeeDocument> GetAllEmployeeDocumentsByEmployeeGuidId(Guid employeeGuidId)
        {

            return _employeeDocumentRepository.GetAllEmployeeDocumentsByEmployeeGuidId(employeeGuidId);
        }

        public EmployeeDocument GetEmployeeDocumentById(int? id)
        {
            return _employeeDocumentRepository.GetEmployeeDocumentById(id);
        }

        public int SaveEmployeeDocument(EmployeeDocument aEmployeeDocument)
        {

            var savedEmployeeDocument = 0;

            try
            {
                aEmployeeDocument.EditedBy = PortalContext.CurrentUser.UserId;
                aEmployeeDocument.EditedDate = DateTime.Now;
                aEmployeeDocument.IsActive = true;
                savedEmployeeDocument = _employeeDocumentRepository.Save(aEmployeeDocument);
            }
            catch (Exception ex)
            {
               throw  new Exception(ex.Message);
            }

            return savedEmployeeDocument;
        }

        public void DeleteEmployeeDocument(EmployeeDocument employeeDocument)
        {

            employeeDocument.IsActive = false;
            _employeeDocumentRepository.Edit(employeeDocument);
        }


        public EmployeeDocumentManager(SCERPDBContext context)
        {
            this._employeeDocumentRepository = new EmployeeDocumentRepository(context);
        }

        public IQueryable<Employee> GetEmployee()
        {
            return _employeeDocumentRepository.GetEmployee();
        }

        public IQueryable<EmployeeDocument> GetEmployeeDocument()
        {
            IQueryable<EmployeeDocument> doc = _employeeDocumentRepository.GetEmployeeDocument();

            foreach (var t in doc)
            {
                t.Path = t.Path.Split('-').ElementAt(5);
            }
            return doc;
        }

        public EmployeeDocument GetEmployeeDocument(int id)
        {
            return _employeeDocumentRepository.GetEmployeeDocument(id);
        }

        public int EditEmployeeDocument(EmployeeDocument document)
        {
            var savedEmployeeDocument = 0;

            try
            {
               
                savedEmployeeDocument = _employeeDocumentRepository.Edit(document);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return savedEmployeeDocument;
        }
    }
}
