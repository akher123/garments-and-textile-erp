using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using SCERP.BLL.IManager.IHRMManager;
using SCERP.Common;
using SCERP.DAL;
using SCERP.DAL.IRepository.IHRMRepository;
using SCERP.DAL.Repository.HRMRepository;
using SCERP.Model;
using SCERP.Model.Custom;

namespace SCERP.BLL.Manager.HRMManager
{
   public class SkillOperationManager:BaseManager, ISkillOperationManager
    {

        private readonly ISkillOperationRepository _skillOperationRepository = null;

        public SkillOperationManager(SCERPDBContext context)
        {
            _skillOperationRepository = new SkillOperationRepository(context);
        }


       public List<SkillOperation> GetAllSkillOperationByPaging(int startPage, int pageSize, out int totalRecords, SearchFieldModel searchFieldModel, SkillOperation model)
       {
           var skillOperations = new List<SkillOperation>();
           try
           {
               skillOperations = _skillOperationRepository.GetAllSkillOperationByPaging(startPage, pageSize, out totalRecords, searchFieldModel,model).ToList();

           }
           catch (Exception exception)
           {
               Errorlog.WriteLog(exception);
               totalRecords = 0;
           }

           return skillOperations;
       }

       public SkillOperation GetSkillOperationById(int skillOperationId)
       {

           var skillOperation = new SkillOperation();
           try
           {
               skillOperation =
                   _skillOperationRepository.FindOne(x => x.IsActive && x.SkillOperationId == skillOperationId);

           }
           catch (Exception exception)
           {
               throw new Exception(exception.Message);
           }
           return skillOperation;
       }

       public bool IsExistSkillOperation(SkillOperation skillOperation)
       {
           bool isExist = false;
           try
           {
             

                isExist =
                   _skillOperationRepository.Exists(
                       x =>
                           x.IsActive&&
                           x.SkillOperationId != skillOperation.SkillOperationId &&
                           x.SkillSetDifficultyId == skillOperation.SkillSetDifficultyId &&
                           x.CategoryId == skillOperation.CategoryId &&
                           x.Name.Replace(" ", "").ToLower().Equals(skillOperation.Name.Replace(" ", "").ToLower()));
           }
           catch (Exception exception)
           {
               throw new Exception(exception.InnerException.Message);
           }
           return isExist;
       }


       public int EditSkillOperation(SkillOperation skillOperation)
       {
           var editedSkillSetOperation = 0; 
           try
           {
               skillOperation.EditedDate = DateTime.Now;
               skillOperation.EditedBy = PortalContext.CurrentUser.UserId;
               editedSkillSetOperation = _skillOperationRepository.Edit(skillOperation);
           }
           catch (Exception exception)
           {
               throw new Exception(exception.InnerException.Message);
           }

           return editedSkillSetOperation;
       }



       public int SaveSkillOperation(SkillOperation skillOperation)
       {
           var savedskilloperation = 0;
           try
           {
               skillOperation.CreatedDate = DateTime.Now;
               skillOperation.CreatedBy = PortalContext.CurrentUser.UserId;
               skillOperation.IsActive = true;

               savedskilloperation = _skillOperationRepository.Save(skillOperation);
           }
           catch (Exception exception)
           {
               throw new Exception(exception.InnerException.Message);
           }

           return savedskilloperation;
       }




       public int DeleteSkillOperation(int skillOperationId)
       {
           var deleteIndex = 0;
           try
           {
               var skilloperationObj = GetSkillOperationById(skillOperationId);
               skilloperationObj.IsActive = false;
               skilloperationObj.CreatedDate = DateTime.Now;
               skilloperationObj.CreatedBy = PortalContext.CurrentUser.UserId;
               skilloperationObj.EditedDate = DateTime.Now;
               skilloperationObj.EditedBy = PortalContext.CurrentUser.UserId;
               deleteIndex = _skillOperationRepository.Edit(skilloperationObj);

           }
           catch (Exception exception)
           {

               throw new Exception(exception.Message);
           }
           return deleteIndex;
       }

       public List<SkillOperation> GetAllSkillOperationManager()
       {
           List<SkillOperation> skillOperations = null;

           try
           {

               skillOperations = _skillOperationRepository.Filter(x => x.IsActive).OrderBy(x => x.Name).ToList();
           }
           catch (Exception exception)
           {
               Errorlog.WriteLog(exception);
           }

           return skillOperations;

       }
    }
}
