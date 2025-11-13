using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.TaskManagementModel;

namespace SCERP.BLL.IManager.ITaskManagementManager
{
   public interface ISubjectManager
    {
       List<TmSubject> GetALLSubjectByPaging(TmSubject model, out int totalRecords);
       int SaveSubject(TmSubject model);
       int EditSubject(TmSubject model);
       TmSubject GetSubjectBySubjectId(int subjectId);
       int DeleteSubject(int subjectId);
  
       List<TmSubject> GetSubjectsByModelId(int moduleId);
       List<TmSubject> GetALLSubject();
       bool IsSubjectExist(TmSubject model);
    }
}
