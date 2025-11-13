using System;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using SCERP.DAL.IRepository.IHRMRepository;
using SCERP.Model;
using System.Data;
using System.Data.Entity;
using System.Collections.Generic;

namespace SCERP.DAL.Repository.HRMRepository
{
    public class EmployeeDocumentRepository : Repository<EmployeeDocument>, IEmployeeDocumentRepository
    {
        public EmployeeDocumentRepository(SCERPDBContext context)
            : base(context)
        {
        }

        public EmployeeDocument GetEmployeeDocumentById(int? id)
        {
            return Context.EmployeeDocuments.Find(id);
        }

        public List<EmployeeDocument> GetAllEmployeeDocumentsByEmployeeGuidId(Guid employeeGuidId)
        {
            var employeeDocument = Context.EmployeeDocuments.Where(r => r.IsActive == true && r.EmployeeId == employeeGuidId).OrderByDescending(x => x.CreatedDate).ToList();
            return employeeDocument;
        }

        public IQueryable<Employee> GetEmployee()
        {
            return Context.Employees.Where(p => p.IsActive);
        }

        public IQueryable<EmployeeDocument> GetEmployeeDocument()
        {
            return Context.EmployeeDocuments.Where(p => p.IsActive).OrderBy(x=>x.Title);
        }

        public EmployeeDocument GetEmployeeDocument(int id)
        {
            var employeeDocument = Context.EmployeeDocuments.SingleOrDefault(p => p.Id == id);
            return employeeDocument;
        }
    }
}
