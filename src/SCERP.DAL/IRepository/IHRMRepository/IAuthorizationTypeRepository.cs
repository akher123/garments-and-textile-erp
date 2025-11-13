using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;

namespace SCERP.DAL.IRepository.IHRMRepository
{
    public interface IAuthorizationTypeRepository:IRepository<AuthorizationType>
    {
        List<AuthorizationType> GetAuthorizationTypes(int startPage, int pageSize, AuthorizationType model, out int totalRecords);
        List<AuthorizationType> GetAuthorizationTypeByProcessKeyId(int processKeyId);
    }
}
