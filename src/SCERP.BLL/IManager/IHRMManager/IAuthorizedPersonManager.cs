using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.BLL.IManager.IHRMManager;
using SCERP.Common;
using SCERP.DAL;
using SCERP.DAL.IRepository.IHRMRepository;
using SCERP.DAL.Repository.HRMRepository;
using SCERP.Model;
using System.Linq;
using SCERP.Model.Custom;


namespace SCERP.BLL.IManager.IHRMManager
{
    public interface IAuthorizedPersonManager
    {
        List<AuthorizedPerson> GetAllAuthorizedPersonsByPaging(int startPage, int pageSize, AuthorizedPerson authorizedPerson,string searchByName, out int totalRecords);
          

        List<AuthorizedPerson> GetAllAuthorizedPersons();

        List<AuthorizedPerson> GetAuthorizedPersonsByAuthorizationType(int? typeId);

        AuthorizedPerson GetAuthorizedPersonById(int? id);

        int SaveAuthorizedPerson(AuthorizedPerson authorizedPerson);

        int EditAuthorizedPerson(AuthorizedPerson authorizedPerson);

        int DeleteAuthorizedPerson(int id);

        List<AuthorizedPerson> GetAllAuthorizedPersonsBySearchKey(string searchByName, int searchByTypeId);

        bool CheckExistingAuthorizedPerson(AuthorizedPerson authorizedPerson);

        List<SCERP.Model.AuthorizationType> GetAllAuthorizedType();

        List<Employee> GetAllEmployee();

        List<AuthorizedPerson> GetAllAuthorizedPersonBySearchKey(int authorizationTypeId, string searchByAuthorizedPerson);

        bool CheckUserIsStorePerson(int authorizationType, Guid? userId);
    }
}
