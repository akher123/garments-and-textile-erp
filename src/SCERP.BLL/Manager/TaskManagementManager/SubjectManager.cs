using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using SCERP.BLL.IManager.ITaskManagementManager;
using SCERP.Common;
using SCERP.DAL.IRepository.ITaskManagementRepository;
using SCERP.Model.TaskManagementModel;

namespace SCERP.BLL.Manager.TaskManagementManager
{
    public class SubjectManager : ISubjectManager
    {
        private readonly ISubjectRepository _subjectRepository;
        public SubjectManager(ISubjectRepository subjectRepository)
        {
            _subjectRepository = subjectRepository;
        }
        public List<TmSubject> GetALLSubjectByPaging(TmSubject model, out int totalRecords)
        {
            string compId = PortalContext.CurrentUser.CompId;
            var index = model.PageIndex;
            var pageSize = AppConfig.PageSize;
            var subjectList = _subjectRepository.Filter(x => x.CompId == compId).Include(x => x.TmModule).Where(x => (x.SubjectName.Trim().Contains(model.SearchString) || String.IsNullOrEmpty(model.SearchString))
                || (x.TmModule.ModuleName.Trim().Contains(model.SearchString) || String.IsNullOrEmpty(model.SearchString)));
           
            totalRecords = subjectList.Count();
            switch (model.sort)
            {
                case "TmModule.ModuleName":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            subjectList = subjectList
                                 .OrderByDescending(r => r.TmModule.ModuleName).ThenBy(r => r.TmModule.ModuleName)
                                .Skip(index * pageSize)
                                .Take(pageSize);

                            break;
                        default:
                            subjectList = subjectList
                                 .OrderBy(r => r.TmModule.ModuleName).ThenBy(r => r.TmModule.ModuleName)
                                .Skip(index * pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;

                case "SubjectName":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            subjectList = subjectList
                                 .OrderByDescending(r => r.SubjectName)
                                .Skip(index * pageSize)
                                .Take(pageSize);

                            break;
                        default:
                            subjectList = subjectList
                                 .OrderBy(r => r.SubjectName).ThenBy(r => r.SubjectName)
                                .Skip(index * pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;

                default:
                    subjectList = subjectList
                        .OrderByDescending(r => r.SubjectId)
                        .Skip(index * pageSize)
                        .Take(pageSize);
                    break;
            }
            return subjectList.ToList();
        }
        public int SaveSubject(TmSubject model)
        {
            model.CompId = PortalContext.CurrentUser.CompId;
            return _subjectRepository.Save(model);
        }
        public int EditSubject(TmSubject model)
        {
            TmSubject subject = _subjectRepository.FindOne(x => x.SubjectId == model.SubjectId);
            subject.ModuleId = model.ModuleId;
            subject.SubjectName = model.SubjectName;
            subject.Description = model.Description;
            return _subjectRepository.Edit(subject);
        }
        public TmSubject GetSubjectBySubjectId(int subjectId)
        {
            return _subjectRepository.FindOne(x => x.SubjectId == subjectId);
        }

        public int DeleteSubject(int subjectId)
        {
            return _subjectRepository.Delete(x => x.SubjectId == subjectId);
        }

        public string GetSubjectNameBySubjectId(int subjectId)
        {
            TmSubject subject = _subjectRepository.FindOne(x => x.SubjectId == subjectId);
            if (subject != null)
            {
                return subject.SubjectName;
            }
            else
            {
                throw new ArgumentNullException("Subject not found by SubjectId");
            }
        }

        public List<TmSubject> GetSubjectsByModelId(int moduleId)
        {
            string compId = PortalContext.CurrentUser.CompId;
            return _subjectRepository.Filter(x => x.ModuleId == moduleId && x.CompId == compId).OrderBy(y=>y.SubjectName).ToList();
        }

        public List<TmSubject> GetALLSubject()
        {
            return _subjectRepository.All().OrderBy(y=>y.SubjectName).ToList();
        }

        public bool IsSubjectExist(TmSubject model)
        {
            return _subjectRepository.Exists(x => x.ModuleId == model.ModuleId && x.SubjectName == model.SubjectName);
        }
    }
}
