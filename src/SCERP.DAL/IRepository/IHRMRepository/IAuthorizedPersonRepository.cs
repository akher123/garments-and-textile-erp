using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;

namespace SCERP.DAL.IRepository.IHRMRepository
{
    public interface IAuthorizedPersonRepository : IRepository<AuthorizedPerson>
    {
        //List<Employee> GetAuthorizedPersonsByType(int? type);

        List<Employee> GetAuthorizedPersons(int processKeyId, int authorizationId);

        //bool CheckAuthorizedPerson(Guid? employeeId);
        //bool CheckAuthorizedPerson(Guid? employeeId, int authorizationId);

        bool CheckAuthorizedPerson(Guid? employeeId,int processKeyId, int authorizationId);

        //bool CheckLeaveApprovalPerson(Guid? employeeId);

        AuthorizedPerson GetAuthorizedPersonById(int? id);

        List<AuthorizedPerson> GetAllAuthorizedPersonsBySearchKey(string searchByName, int searchByTypeId);

        List<AuthorizedPerson> GetAllAuthorizedPersonByPaging(int startPage, int pageSize, out int totalRecords, AuthorizedPerson authorizedPerson, string searchByName);

        List<AuthorizedPerson> GetAllAuthorizedPerson();

        List<AuthorizedPerson> GetAuthorizedPersonsByAuthorizationType(int? typeId);

        List<AuthorizationType> GetAllAuthorizedType();
        List<Employee> GetAllEmployee();
        List<AuthorizedPerson> GetAllAuthorizedPersonBySearchKey(int authorizationTypeId, string searchByAuthorizedPerson);
        AuthorizedPerson GetAuthorizedPersonByEmployeeId(Guid? userId, int inventoryRequisition);
        bool CheckUserIsStorePerson(int requisitionPreparation, Guid? userId);
    }
}
