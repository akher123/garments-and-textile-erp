using System;
using System.Collections.Generic;
using System.Data.Entity;
using SCERP.Common;
using SCERP.DAL.IRepository.IHRMRepository;
using SCERP.Model;
using System.Linq;

namespace SCERP.DAL.Repository.HRMRepository
{
    public class AuthorizedPersonRepository : Repository<AuthorizedPerson>, IAuthorizedPersonRepository
    {
        public AuthorizedPersonRepository(SCERPDBContext context)
            : base(context)
        {
        }

        //public List<Employee> GetAuthorizedPersonsByType(int? type)
        //{

        //    List<Guid> employeeId = Context.AuthorizedPersons.Where(p => p.IsActive == true)
        //            .Include(x => x.AuthorizationType)
        //            .Where(
        //                x => x.AuthorizationType.AuthorizationId == type).Select(p=>p.EmployeeId).ToList();

        //    return Context.Employees.Where(p => employeeId.Contains(p.EmployeeId)).ToList();
        //}

        public List<Employee> GetAuthorizedPersons(int processKeyId, int authorizationId)
        {

            List<Guid> employeeId = Context.AuthorizedPersons.Where(p => p.IsActive == true)
                    .Include(x => x.AuthorizationType)
                    .Where(
                        x => x.AuthorizationType.ProcessKeyId == processKeyId && x.AuthorizationType.AuthorizationId == authorizationId)
                              .Select(p => p.EmployeeId).ToList();

            return Context.Employees.Where(p => employeeId.Contains(p.EmployeeId)).ToList();
        }

        //public bool CheckAuthorizedPerson(Guid? employeeId, int authorizationId)
        //{
        //    if (Context.AuthorizedPersons != null)
        //    {
        //        var author = Context.AuthorizedPersons.Where(p => p.IsActive == true && p.EmployeeId == employeeId)
        //            .Include(x => x.AuthorizationType)
        //            .Where(
        //                x => x.AuthorizationType.AuthorizationId == authorizationId
        //                && x.AuthorizationType.ProcessKeyId== (int)SCERP.Common.ProcessKeyEnum.HRM_Leave); 
        //        if (author.Any())
        //            return true;
        //    }
        //    return false;

        //}

        public bool CheckAuthorizedPerson(Guid? employeeId, int processKeyId, int authorizationId)
        {
            if (Context.AuthorizedPersons != null)
            {
                var author = Context.AuthorizedPersons.Where(p => p.IsActive && p.EmployeeId == employeeId)
                    .Include(x => x.AuthorizationType)
                    .Where(
                        x => x.AuthorizationType.ProcessKeyId == processKeyId
                        && x.AuthorizationType.AuthorizationId == authorizationId);
                if (author.Any())
                    return true;
            }
            return false;

        }

        public AuthorizedPerson GetAuthorizedPersonById(int? id)
        {
            return Context.AuthorizedPersons.Include(x => x.Employee).FirstOrDefault(x => x.Id == id);
        }

        public override IQueryable<AuthorizedPerson> All()
        {
            return Context.AuthorizedPersons.Where(x => x.IsActive == true).OrderBy(r => r.Id);
        }

        public List<AuthorizedPerson> GetAllAuthorizedPersonsBySearchKey(string searchByName, int searchByTypeId)
        {
            List<AuthorizedPerson> authorizedPersons = null;

            try
            {
                authorizedPersons = Context.AuthorizedPersons.Include(x => x.Employee).Include(x => x.AuthorizationType).Where(x => x.IsActive == true && ((x.Employee.Name.Replace(" ", "").ToLower()
                                   .Contains(searchByName.Replace(" ", "").ToLower())) || String.IsNullOrEmpty(searchByName))
                                && (x.AuthorizationTypeId == searchByTypeId || searchByTypeId == 0)).ToList();
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return authorizedPersons;
        }

        public List<AuthorizedPerson> GetAllAuthorizedPersonByPaging(int startPage, int pageSize, out int totalRecords, AuthorizedPerson authorizedPerson, string searchByName)
        {
            IQueryable<AuthorizedPerson> authorizedPersons = null;

            try
            {
                authorizedPersons = Context.AuthorizedPersons
                    .Include(x => x.Employee)
                    .Where(x => x.IsActive == true
                        && ((x.Employee.Name.Replace(" ", "")
                    .ToLower()
                     .Contains(searchByName.Replace(" ", "").ToLower()))
                     || String.IsNullOrEmpty(searchByName))
                     && (x.AuthorizationTypeId == authorizedPerson.AuthorizationTypeId
                     || authorizedPerson.AuthorizationTypeId == 0));

                totalRecords = authorizedPersons.Count();
                switch (authorizedPerson.sort)
                {
                    case "Employee.Name":
                        switch (authorizedPerson.sortdir)
                        {
                            case "DESC":
                                authorizedPersons = authorizedPersons
                                    .OrderByDescending(r => r.Employee.Name)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);

                                break;
                            default:
                                authorizedPersons = authorizedPersons
                               .OrderBy(r => r.Employee.Name)
                               .Skip(startPage * pageSize)
                               .Take(pageSize);
                                break;
                        }
                        break;
                    default:

                        switch (authorizedPerson.sortdir)
                        {
                            case "DESC":
                                authorizedPersons = authorizedPersons
                                 .OrderByDescending(r => r.AuthorizationType.TypeName)
                                 .Skip(startPage * pageSize)
                                 .Take(pageSize);
                                break;
                            default:
                                authorizedPersons = authorizedPersons
                                .OrderByDescending(r => r.AuthorizationType.TypeName)
                                .Skip(startPage * pageSize)
                                .Take(pageSize);
                                break;
                        }

                        break;
                }
            }
            catch (Exception exception)
            {
                totalRecords = 0;
                throw new Exception(exception.Message);
            }

            return authorizedPersons.ToList();
        }

        public List<AuthorizedPerson> GetAllAuthorizedPerson()
        {
            var authorizedPersonList = Context.AuthorizedPersons.Where(x => x.IsActive).OrderBy(r => r.Id).ToList();
            return authorizedPersonList;
        }

        public List<AuthorizedPerson> GetAuthorizedPersonsByAuthorizationType(int? typeId)
        {
            var authorizedPersonList = Context.AuthorizedPersons.Where(p => p.IsActive == true)
               
                .Include(y => y.Employee)
                .Where(x => x.AuthorizationTypeId == typeId).ToList();

            return authorizedPersonList;
        }

        public List<SCERP.Model.AuthorizationType> GetAllAuthorizedType()
        {
            return Context.AuthorizationTypes.Where(p => p.IsActive).OrderBy(x => x.TypeName).ToList();
        }

        public List<Employee> GetAllEmployee()
        {
            return Context.Employees.Where(p => p.IsActive).ToList();
        }

        public List<AuthorizedPerson> GetAllAuthorizedPersonBySearchKey(int authorizationTypeId, string searchByAuthorizedPerson)
        {
            IQueryable<AuthorizedPerson> authorizedPersons;

            try
            {
                authorizedPersons = Context.AuthorizedPersons.Include(x => x.Employee).Include(x => x.AuthorizationType).Where(x => x.IsActive == true &&
                                                 ((x.Employee.Name.Replace(" ", "")
                                                     .ToLower()
                                                     .Contains(searchByAuthorizedPerson.Replace(" ", "").ToLower())) || String.IsNullOrEmpty(searchByAuthorizedPerson)) && (x.AuthorizationTypeId == authorizationTypeId || authorizationTypeId == 0));

            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message);
            }

            return authorizedPersons.ToList();
        }

        public AuthorizedPerson GetAuthorizedPersonByEmployeeId(Guid? userId, int inventoryRequisition)
        {
          return  Context.AuthorizedPersons.Include(x => x.AuthorizationType)
                .FirstOrDefault(x => x.EmployeeId == userId && x.AuthorizationType.ProcessKeyId == inventoryRequisition);
        }

        public bool CheckUserIsStorePerson(int authorizationType, Guid? userId)
        {
            return
                Context.AuthorizedPersons.Any(x => x.AuthorizationTypeId == authorizationType && x.EmployeeId == userId);

        }
    }
}
