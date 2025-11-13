
using System;
using System.Linq;
using SCERP.Model;
using System.Collections.Generic;

namespace SCERP.BLL.IManager.IHRMManager
{
    public interface IEmployeeDocumentManager
    {

        List<EmployeeDocument> GetAllEmployeeDocumentsByEmployeeGuidId(Guid employeeGuidId);

        EmployeeDocument GetEmployeeDocumentById(int? id);

        int SaveEmployeeDocument(EmployeeDocument aEmployeeDocument);

        void DeleteEmployeeDocument(EmployeeDocument employeeDocument);

        IQueryable<Employee> GetEmployee();


        IQueryable<EmployeeDocument> GetEmployeeDocument();

        EmployeeDocument GetEmployeeDocument(int id);


        int EditEmployeeDocument(EmployeeDocument document);
    }
}
