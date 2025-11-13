using System;
using System.Collections.Generic;
using SCERP.DAL.IRepository.ITaskManagementRepository;
using SCERP.Model.TaskManagementModel;

namespace SCERP.DAL.Repository.TaskManagementRepository
{
    public class SubjectRepository : Repository<TmSubject>, ISubjectRepository
    {
        public SubjectRepository(SCERPDBContext context) : base(context)
        {
        }
    }
}
