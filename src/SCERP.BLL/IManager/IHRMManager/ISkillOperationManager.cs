using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;
using SCERP.Model.Custom;

namespace SCERP.BLL.IManager.IHRMManager
{
   public interface ISkillOperationManager
    {
       List<SkillOperation> GetAllSkillOperationByPaging(int startPage, int pageSize, out int totalRecords, SearchFieldModel searchFieldModel, SkillOperation skillOperation);
       SkillOperation GetSkillOperationById(int skillOperationId);
       bool IsExistSkillOperation(SkillOperation skillOperation);
       int EditSkillOperation(SkillOperation skillOperation);
       int SaveSkillOperation(SkillOperation skillOperation);
       int DeleteSkillOperation(int skillOperationId);
       List<SkillOperation> GetAllSkillOperationManager(); 
    } 
}
   