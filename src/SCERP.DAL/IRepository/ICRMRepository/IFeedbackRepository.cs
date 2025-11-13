using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;
using SCERP.Model.CRMModel;

namespace SCERP.DAL.IRepository.ICRMRepository
{
    public interface IFeedbackRepository : IRepository<CRMFeedback>
    {
        CRMFeedback GetFeedbackById(int? id);

        List<CRMFeedback> GetAllFeedbacks();

        List<CRMFeedback> GetAllFeedbacksByPaging(int startPage, int pageSize, out int totalRecords, CRMFeedback feedback);

        List<CRMFeedback> GetAllFeedbacksBySearchKey(string searchKey);



        //**************** Feedback of all user **************

        List<SCERP.Model.Module> GetAllModuleNames();

        List<CRMFeedback> GetAllFeedbacksAllUserByPaging(int startPage, int pageSize, out int totalRecords, CRMFeedback feedback);
    }
}
