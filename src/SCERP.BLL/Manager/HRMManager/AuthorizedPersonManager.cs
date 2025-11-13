using System;
using System.Collections.Generic;
using System.Linq;
using SCERP.BLL.IManager.IHRMManager;
using SCERP.Common;
using SCERP.DAL;
using SCERP.DAL.IRepository.IHRMRepository;
using SCERP.DAL.Repository.HRMRepository;
using SCERP.Model;

namespace SCERP.BLL.Manager.HRMManager
{
    public class AuthorizedPersonManager : BaseManager, IAuthorizedPersonManager
    {

        private readonly IAuthorizedPersonRepository _authorizedPersonRepository = null;

        public AuthorizedPersonManager(SCERPDBContext context)
        {
            this._authorizedPersonRepository = new AuthorizedPersonRepository(context);
        }

        public List<AuthorizedPerson> GetAllAuthorizedPersonsByPaging(int startPage, int pageSize, AuthorizedPerson authorizedPerson, string searchByName, out int totalRecords)
        {
            var authorizedPersons = new List<AuthorizedPerson>();
            try
            {

                authorizedPersons = _authorizedPersonRepository.GetAllAuthorizedPersonByPaging(startPage, pageSize, out totalRecords, authorizedPerson, searchByName).ToList();

            }
            catch (Exception exception)
            {
                totalRecords = 0;
                Errorlog.WriteLog(exception);
            }

            return authorizedPersons;
        }

        public List<AuthorizedPerson> GetAllAuthorizedPersonsBySearchKey(string searchByName, int searchByTypeId)
        {
            var authorizedPersons = new List<AuthorizedPerson>();

            try
            {
                authorizedPersons = _authorizedPersonRepository.GetAllAuthorizedPersonsBySearchKey(searchByName, searchByTypeId);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return authorizedPersons;
        }

        public AuthorizedPerson GetAuthorizedPersonById(int? id)
        {
            AuthorizedPerson authorizedPerson = null;
            try
            {
                authorizedPerson = _authorizedPersonRepository.GetAuthorizedPersonById(id);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                authorizedPerson = null;
            }

            return authorizedPerson;
        }

        public List<AuthorizedPerson> GetAllAuthorizedPersons()
        {
            var authorizedPersonList = new List<AuthorizedPerson>();

            try
            {
                authorizedPersonList = _authorizedPersonRepository.GetAllAuthorizedPerson();
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
               
            }

            return authorizedPersonList;
        }

        public int SaveAuthorizedPerson(AuthorizedPerson authorizedPerson)
        {
            var savedAuthorizedPerson = 0;
            try
            {
                var authorizedPersonOhj = new AuthorizedPerson()
                {
                    EmployeeId = authorizedPerson.EmployeeId,
                    AuthorizationTypeId = authorizedPerson.AuthorizationTypeId,
                    CreatedDate = DateTime.Now,
                    IsActive = true,
                    CreatedBy = PortalContext.CurrentUser.UserId,
                };

                savedAuthorizedPerson = _authorizedPersonRepository.Save(authorizedPersonOhj);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                savedAuthorizedPerson = 0;
            }

            return savedAuthorizedPerson;
        }

        public int EditAuthorizedPerson(AuthorizedPerson authorizedPerson)
        {
            var editedAuthorizedPerson = 0;
            try
            {
                var authorizedPersonObj = _authorizedPersonRepository.FindOne(x => x.Id == authorizedPerson.Id);
                authorizedPersonObj.AuthorizationTypeId = authorizedPerson.AuthorizationTypeId;
                authorizedPerson.EditedDate = DateTime.Now;
                authorizedPerson.EditedBy = PortalContext.CurrentUser.UserId;
                editedAuthorizedPerson = _authorizedPersonRepository.Edit(authorizedPersonObj);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                editedAuthorizedPerson = 0;
            }

            return editedAuthorizedPerson;
        }

        public int DeleteAuthorizedPerson(int id)
        {
            var deletedAuthorizedPerson = 0;
            var authorizedPerson = _authorizedPersonRepository.FindOne(x=>x.IsActive&&x.Id==id);
            try
            {
                
                authorizedPerson.EditedDate = DateTime.Now;
                authorizedPerson.EditedBy = PortalContext.CurrentUser.UserId;
                authorizedPerson.IsActive = false;
                deletedAuthorizedPerson = _authorizedPersonRepository.Edit(authorizedPerson);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                deletedAuthorizedPerson = 0;
            }

            return deletedAuthorizedPerson;
        }


        public bool CheckExistingAuthorizedPerson(AuthorizedPerson authorizedPerson)
        {
            bool isExist = false;
            try
            {
                isExist =
                    _authorizedPersonRepository.Exists(
                        x =>
                            x.IsActive&&x.Id!=authorizedPerson.Id&&
                            x.EmployeeId == authorizedPerson.EmployeeId &&
                            x.AuthorizationTypeId ==authorizedPerson.AuthorizationTypeId 
                            );
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return isExist;
        }

        public List<SCERP.Model.AuthorizationType> GetAllAuthorizedType()
        {

            return _authorizedPersonRepository.GetAllAuthorizedType();
        }

        public List<Employee> GetAllEmployee()
        {
            return _authorizedPersonRepository.GetAllEmployee();
        }

        public List<AuthorizedPerson> GetAllAuthorizedPersonBySearchKey(int authorizationTypeId, string searchByAuthorizedPerson)
        {
            var authorizedPersons = new List<AuthorizedPerson>();
            try
            {
                authorizedPersons = _authorizedPersonRepository. GetAllAuthorizedPersonBySearchKey( authorizationTypeId,  searchByAuthorizedPerson);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);

            }
            return authorizedPersons;
        }

        public bool CheckUserIsStorePerson(int authorizationType, Guid? userId)
        {
            return _authorizedPersonRepository.CheckUserIsStorePerson(authorizationType,userId);
        }


        public List<AuthorizedPerson> GetAuthorizedPersonsByAuthorizationType(int? typeId)
                                      
        {
           var authorizedPersons = new List<AuthorizedPerson>();
            try
            {
                authorizedPersons = _authorizedPersonRepository.GetAuthorizedPersonsByAuthorizationType(typeId);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);

            }
            return authorizedPersons;
        }
    }
}

