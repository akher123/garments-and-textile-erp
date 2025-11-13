using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;


namespace SCERP.DAL.IRepository.IHRMRepository
{
    public interface IEmployeeDocumentRepository : IRepository<EmployeeDocument>
    {
        EmployeeDocument GetEmployeeDocumentById(int? id);
        List<EmployeeDocument> GetAllEmployeeDocumentsByEmployeeGuidId(Guid employeeGuidId);
        IQueryable<Employee> GetEmployee();
        EmployeeDocument GetEmployeeDocument(int id);
        IQueryable<EmployeeDocument> GetEmployeeDocument();
    }
}
