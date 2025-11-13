using SCERP.Common;
using SCERP.DAL.IRepository.ICRMRepository;
using SCERP.DAL.IRepository.IHRMRepository;
using SCERP.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using SCERP.Model.CRMModel;

namespace SCERP.DAL.Repository.CRMRepository
{
    public class FeedbackRepository : Repository<CRMFeedback>, SCERP.DAL.IRepository.ICRMRepository.IFeedbackRepository
    {
        public FeedbackRepository(SCERPDBContext context)
            : base(context)
        {

        }

        public CRMFeedback GetFeedbackById(int? id)
        {
            return Context.CRMFeedbacks.Find(id);
        }

        public List<CRMFeedback> GetAllFeedbacksByPaging(int startPage, int pageSize, out int totalRecords, CRMFeedback feedback)
        {
            IQueryable<CRMFeedback> feedbacks;          

            try
            {                
                string searchKey = feedback.Subject;

                feedbacks = Context.CRMFeedbacks.Where(x => x.IsActive && x.CreatedBy == PortalContext.CurrentUser.UserId && ((x.Subject.Replace(" ", "").ToLower().Contains(searchKey.Replace(" ", "").ToLower()))
                                                                                                                              || (x.Description.Replace(" ", "").ToLower().Contains(searchKey.Replace(" ", "").ToLower()))
                                                                                                                              || String.IsNullOrEmpty(searchKey))
                    );

                DateTime? fromDate = feedback.FromDate;
                DateTime? toDate = null;

                if (feedback.ToDate != null)
                {
                    toDate = feedback.ToDate.Value.AddDays(1);
                }
                if (fromDate != null && toDate != null)
                    feedbacks = feedbacks.Where(p => p.CreatedDate >= fromDate && p.CreatedDate <= toDate);

                totalRecords = feedbacks.Count();

                switch (feedback.sort)
                {
                    case "Subject":
                        switch (feedback.sortdir)
                        {
                            case "DESC":
                                feedbacks = feedbacks
                                    .OrderByDescending(r => r.Subject)
                                    .Skip(startPage*pageSize)
                                    .Take(pageSize);
                                break;
                            default:
                                feedbacks = feedbacks
                                    .OrderBy(r => r.Subject)
                                    .Skip(startPage*pageSize)
                                    .Take(pageSize);
                                break;
                        }
                        break;
                    default:
                        feedbacks = feedbacks
                            .OrderByDescending(r => r.Id)
                            .Skip(startPage*pageSize)
                            .Take(pageSize);
                        break;
                }
            }
            catch (Exception exception)
            {
                totalRecords = 0;
                throw new Exception(exception.Message);
            }

            return feedbacks.ToList();
        }

        public List<CRMFeedback> GetAllFeedbacksBySearchKey(string searchKey)
        {
            List<CRMFeedback> feedbacks = null;

            try
            {
                feedbacks = !String.IsNullOrEmpty(searchKey) ? Context.CRMFeedbacks.Where(x => x.IsActive == true && x.Subject.ToLower().Contains(searchKey.ToLower())).ToList() : GetAllFeedbacks();
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return feedbacks;
        }

        public List<CRMFeedback> GetAllFeedbacks()
        {
            return Context.CRMFeedbacks.Where(x => x.IsActive).OrderBy(x => x.Id).ToList();
        }



        //**************** Feedback of all user **************

        public List<SCERP.Model.Module> GetAllModuleNames()
        {
            List<SCERP.Model.Module> modules = null;

            try
            {
                modules = Context.Modules.Where(p => p.IsActive == true).ToList();
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return modules;
        }

        public List<CRMFeedback> GetAllFeedbacksAllUserByPaging(int startPage, int pageSize, out int totalRecords, CRMFeedback feedback)
        {
            IQueryable<CRMFeedback> feedbacks;

            try
            {
                string searchKey = feedback.Subject;

                feedbacks = Context.CRMFeedbacks.Where(x => x.IsActive && ((x.Subject.Replace(" ", "").ToLower().Contains(searchKey.Replace(" ", "").ToLower()))
                                                                           || (x.Description.Replace(" ", "").ToLower().Contains(searchKey.Replace(" ", "").ToLower()))
                                                                           || String.IsNullOrEmpty(searchKey))
                    );

                DateTime? fromDate = feedback.FromDate;
                DateTime? toDate = null;

                if (feedback.ToDate != null)
                {
                    toDate = feedback.ToDate.Value.AddDays(1);
                }
                if (fromDate != null && toDate != null)
                    feedbacks = feedbacks.Where(p => p.CreatedDate >= fromDate && p.CreatedDate <= toDate);

                totalRecords = feedbacks.Count();

                switch (feedback.sort)
                {
                    case "Subject":
                        switch (feedback.sortdir)
                        {
                            case "DESC":
                                feedbacks = feedbacks
                                    .OrderByDescending(r => r.Subject)
                                    .Skip(startPage*pageSize)
                                    .Take(pageSize);
                                break;
                            default:
                                feedbacks = feedbacks
                                    .OrderBy(r => r.Subject)
                                    .Skip(startPage*pageSize)
                                    .Take(pageSize);
                                break;
                        }
                        break;
                    default:
                        feedbacks = feedbacks
                            .OrderByDescending(r => r.Id)
                            .Skip(startPage*pageSize)
                            .Take(pageSize);
                        break;
                }
            }
            catch (Exception exception)
            {
                totalRecords = 0;
                throw new Exception(exception.Message);
            }

            return feedbacks.ToList();
        }
    }
}
