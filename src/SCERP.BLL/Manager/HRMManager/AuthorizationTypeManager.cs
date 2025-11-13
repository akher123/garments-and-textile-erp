using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.BLL.IManager.IHRMManager;
using SCERP.Common;
using SCERP.DAL;
using SCERP.DAL.IRepository.IHRMRepository;
using SCERP.DAL.Repository;
using SCERP.DAL.Repository.HRMRepository;
using SCERP.Model;
using AuthorizationType = SCERP.Model.AuthorizationType;

namespace SCERP.BLL.Manager.HRMManager
{
    public class AuthorizationTypeManager:IAuthorizationTypeManager
    {
        private readonly IAuthorizationTypeRepository _authorizationTypeRepository = null;
        public AuthorizationTypeManager(SCERPDBContext context)
        {
            _authorizationTypeRepository=new AuthorizationTypeRepository(context);
        }

        public List<SCERP.Model.AuthorizationType> GetAuthorizationTypes(int startPage, int pageSize, SCERP.Model.AuthorizationType model, out int totalRecords)
        {
            List<SCERP.Model.AuthorizationType> authorizationTypes;
            try
            {
                authorizationTypes = _authorizationTypeRepository.GetAuthorizationTypes(startPage, pageSize, model, out totalRecords);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
            return authorizationTypes;
        }

        public SCERP.Model.AuthorizationType GetAuthorizationTypeById(int authorizationTypeId)
        {
            SCERP.Model.AuthorizationType authorizationType;
            try
            {
                authorizationType =
                    _authorizationTypeRepository.FindOne(x => x.IsActive && x.Id == authorizationTypeId);
            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message);
            }
            return authorizationType;
        }

        public int EditAuthorizationType(SCERP.Model.AuthorizationType authorization)
        {
            var editIndex = 0;
            try
            {
                var authorizationObj = _authorizationTypeRepository.FindOne(x => x.IsActive && x.Id == authorization.Id);
                authorizationObj.ProcessKeyId = authorization.ProcessKeyId;
                authorizationObj.AuthorizationId = authorization.AuthorizationId;
                authorizationObj.TypeName = authorization.TypeName;
                authorizationObj.EditedDate = DateTime.Now;
                authorizationObj.EditedBy = PortalContext.CurrentUser.UserId;
                editIndex = _authorizationTypeRepository.Edit(authorizationObj);

            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message);
            }
            return editIndex;
        }

        public bool IsExistauthorization(SCERP.Model.AuthorizationType authorization)
        {
            bool isExist;
            try
            {
                isExist = _authorizationTypeRepository.Exists(x => x.IsActive == true
                    && x.Id != authorization.Id
                    && x.ProcessKeyId!=authorization.ProcessKeyId
                    && (x.TypeName.Replace("", " ").ToLower() == authorization.TypeName.Replace("", " ").ToLower()));
            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message);
            }
            return isExist;
        }

        public int DeleteAuthorizationType(int authorizationTypeId)
        {
            var deleteIndex = 0;
            try
            {
                var authorizationObj = _authorizationTypeRepository.FindOne(x => x.IsActive && x.Id == authorizationTypeId);
                authorizationObj.IsActive = false;
                authorizationObj.EditedDate = DateTime.Now;
                authorizationObj.EditedBy = PortalContext.CurrentUser.UserId;
                deleteIndex = _authorizationTypeRepository.Edit(authorizationObj);
            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message);
            }
            return deleteIndex;
        }

        public int SaveAuthorizationType(SCERP.Model.AuthorizationType authorization)
        {
            var saveIndex = 0;
            try
            {
                authorization.CreatedBy = PortalContext.CurrentUser.UserId;
                authorization.CreatedDate = DateTime.Now;
                authorization.IsActive = true;
                saveIndex = _authorizationTypeRepository.Save(authorization);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
            return saveIndex;
        }


        public List<AuthorizationType> GetAuthorizationTypeByProcessKeyId(int processKeyId)
        {
            List<AuthorizationType> authorizationTypes;
            try
            {
                authorizationTypes = _authorizationTypeRepository.GetAuthorizationTypeByProcessKeyId(processKeyId);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
            return authorizationTypes;
        }
    }
}
