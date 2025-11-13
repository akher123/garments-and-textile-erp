using System;
using System.Collections.Generic;
using System.Linq;
using SCERP.DAL.IRepository.IHRMRepository;
using SCERP.Model;

namespace SCERP.DAL.Repository.HRMRepository
{
    public class AuthorizationTypeRepository :Repository<AuthorizationType>, IAuthorizationTypeRepository
    {
        public AuthorizationTypeRepository(SCERPDBContext context) : base(context)
        {
        }

        public List<AuthorizationType> GetAuthorizationTypes(int startPage, int pageSize, AuthorizationType model, out int totalRecords)
        {
            IQueryable<AuthorizationType> authorizationTypes;
            try
            {
                authorizationTypes = Context.AuthorizationTypes
                    .Where(x => x.IsActive == true && ((x.TypeName.Replace(" ", "")
                        .ToLower().Contains(model.TypeName.Replace(" ", "")
                            .ToLower())) || String.IsNullOrEmpty(model.TypeName)));
                totalRecords = authorizationTypes.Count();
                switch (model.sortdir)
                {
                    case "DESC":
                        authorizationTypes = authorizationTypes.OrderByDescending(r => r.TypeName)
                            .Skip(startPage * pageSize)
                            .Take(pageSize);
                        break;
                    default:
                        authorizationTypes = authorizationTypes.OrderBy(r => r.TypeName)
                          .Skip(startPage * pageSize)
                          .Take(pageSize);
                        break;
                }
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
            return authorizationTypes.ToList();
        }


        public List<AuthorizationType> GetAuthorizationTypeByProcessKeyId(int processKeyId)
        {
            List<AuthorizationType> authorizationTypes;
            try
            {
                authorizationTypes = Context.AuthorizationTypes.Where(x => x.IsActive && x.ProcessKeyId == processKeyId).ToList();
            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message);
            }
            return authorizationTypes;
        }
    }
}
