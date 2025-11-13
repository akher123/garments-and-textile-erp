using SCERP.BLL.IManager.ICRMManager;
using SCERP.BLL.IManager.IHRMManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Common;
using SCERP.DAL;
using SCERP.DAL.IRepository.ICRMRepository;
using SCERP.DAL.Repository.CRMRepository;
using SCERP.Model;
using SCERP.Model.CRMModel;

namespace SCERP.BLL.Manager.CRMManager
{
    public class FeedbackManager : BaseManager, IFeedbackManager
    {
        private readonly IFeedbackRepository _feedbackRepository = null;

        public FeedbackManager(SCERPDBContext context)
        {
            this._feedbackRepository = new FeedbackRepository(context);
        }


        public List<CRMFeedback> GetAllFeedbacksByPaging(int startPage, int pageSize, out int totalRecords, CRMFeedback feedback)
        {
            var feedbacks = new List<CRMFeedback>();

            try
            {
                feedbacks = _feedbackRepository.GetAllFeedbacksByPaging(startPage, pageSize, out totalRecords, feedback).ToList();

            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                totalRecords = 0;
            }

            return feedbacks;
        }

        public List<CRMFeedback> GetAllFeedbacks()
        {
            var feedbacks = new List<CRMFeedback>();

            try
            {
                feedbacks = _feedbackRepository.GetAllFeedbacks();
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return feedbacks;
        }

        public CRMFeedback GetFeedbackById(int? id)
        {
            var feedback = new CRMFeedback();

            try
            {
                feedback = _feedbackRepository.GetFeedbackById(id);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);

            }

            return feedback;
        }

        public List<CRMFeedback> GetFeedbackBySearchKey(string searchKey)
        {
            var feedbacks = new List<CRMFeedback>();

            try
            {
                feedbacks = _feedbackRepository.Filter(x => x.Subject.Replace(" ", "").ToLower().Contains(searchKey.Replace(" ", "").ToLower()) || String.IsNullOrEmpty(searchKey)).OrderBy(x => x.Subject).ToList();
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return feedbacks;
        }

        public bool CheckExistingFeedback(CRMFeedback feedback)
        {
            bool isExist = false;

            try
            {
                isExist = _feedbackRepository.Exists(x => x.IsActive && x.Id != feedback.Id && x.Subject.Replace(" ", "").ToLower().Equals(feedback.Subject.Replace(" ", "").ToLower()));
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return isExist;
        }

        public int SaveFeedback(CRMFeedback feedback)
        {
            var savedFeedback = 0;

            try
            {
                feedback.IsActive = true;              
                feedback.CreatedBy = PortalContext.CurrentUser.UserId;
                savedFeedback = _feedbackRepository.Save(feedback);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                savedFeedback = 0;
            }
            return savedFeedback;
        }

        public int EditFeedback(CRMFeedback feedback)
        {
            var edit = 0;

            try
            {
                feedback.EditedDate = DateTime.Now;
                feedback.EditedBy = PortalContext.CurrentUser.UserId;
                edit = _feedbackRepository.Edit(feedback);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return edit;
        }

        public int DeleteFeedback(CRMFeedback feedback)
        {
            var deleted = 0;

            try
            {
                feedback.IsActive = false;
                deleted = _feedbackRepository.Edit(feedback);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return deleted;
        }


        //**************** Feedback of all user **************

        public List<SCERP.Model.Module> GetAllModuleNames()
        {
            return _feedbackRepository.GetAllModuleNames();
        }



        public List<CRMFeedback> GetAllFeedbacksAllUserByPaging(int startPage, int pageSize, out int totalRecords, CRMFeedback feedback)
        {
            var feedbacks = new List<CRMFeedback>();

            try
            {
                feedbacks = _feedbackRepository.GetAllFeedbacksAllUserByPaging(startPage, pageSize, out totalRecords, feedback).ToList();
            }

            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                totalRecords = 0;
            }

            return feedbacks;
        }
    }
}
