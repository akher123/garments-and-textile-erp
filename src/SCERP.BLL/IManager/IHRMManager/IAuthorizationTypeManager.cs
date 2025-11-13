using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.DAL.IRepository;
using SCERP.Model;

namespace SCERP.BLL.IManager.IHRMManager
{
    public interface IAuthorizationTypeManager 
    {
        List<AuthorizationType> GetAuthorizationTypes(int startPage, int pageSize, AuthorizationType model, out int totalRecords);
        AuthorizationType GetAuthorizationTypeById
            (int authorizationTypeId);

        int EditAuthorizationType(AuthorizationType authorization);
        bool IsExistauthorization(AuthorizationType authorization);
        int DeleteAuthorizationType(int authorizationTypeId);
        int SaveAuthorizationType(AuthorizationType authorization);
        List<AuthorizationType> GetAuthorizationTypeByProcessKeyId(int processKeyId);
    }
}
