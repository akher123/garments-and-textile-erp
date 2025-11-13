using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;
using SCERP.Model.CRMModel;

namespace SCERP.BLL.IManager.ICRMManager
{
    public interface IFeedbackManager
    {
        List<CRMFeedback> GetAllFeedbacksByPaging(int startPage, int pageSize, out int totalRecords, CRMFeedback feedback);

        List<CRMFeedback> GetAllFeedbacks();

        CRMFeedback GetFeedbackById(int? id);

        int SaveFeedback(CRMFeedback feedback);

        int EditFeedback(CRMFeedback feedback);

        int DeleteFeedback(CRMFeedback feedback);

        bool CheckExistingFeedback(CRMFeedback feedback);

        List<CRMFeedback> GetFeedbackBySearchKey(string searchKey);


        //**************** Feedback of all user **************

        List<SCERP.Model.Module> GetAllModuleNames();
        List<CRMFeedback> GetAllFeedbacksAllUserByPaging(int startPage, int pageSize, out int totalRecords, CRMFeedback feedback);
    }
}
