using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.BLL.IManager.ITaskManagementManager;
using SCERP.Common;
using SCERP.DAL.IRepository.ITaskManagementRepository;
using SCERP.Model.TaskManagementModel;

namespace SCERP.BLL.Manager.TaskManagementManager
{
    public class AssigneeManager : IAssigneeManager
    {
        private readonly IAssigneeRepository _assigneeRepository;
        public AssigneeManager(IAssigneeRepository assigneeRepository)
        {
            _assigneeRepository = assigneeRepository;
        }

        public List<TmAssignee> GetAllAssigneeByPaging(TmAssignee model, out int totalRecords)
        {
            int index = model.PageIndex;
            int pageSize = AppConfig.PageSize;
            var assigneeList =
                _assigneeRepository.Filter(
                    x => (x.Assignee.Contains(model.SearchString) || String.IsNullOrEmpty(model.SearchString)));
            totalRecords = assigneeList.Count();
            switch (model.sort)
            {
                case "Assignee":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            assigneeList = assigneeList
                                 .OrderByDescending(r => r.Assignee)
                                .Skip(index * pageSize)
                                .Take(pageSize);

                            break;
                        default:
                            assigneeList = assigneeList
                                 .OrderBy(r => r.Assignee)
                                .Skip(index * pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;

                default:
                    assigneeList = assigneeList
                        .OrderByDescending(r => r.AssigneeId)
                        .Skip(index * pageSize)
                        .Take(pageSize);
                    break;
            }
            return assigneeList.ToList();
        }
        public List<TmAssignee> GetAllAssignee()
        {
            return _assigneeRepository.All().ToList();
        }
        public int EditAssignee(TmAssignee model)
        {
            TmAssignee assignee =
                _assigneeRepository.FindOne(x => x.AssigneeId == model.AssigneeId);

            assignee.Assignee = model.Assignee;
            return _assigneeRepository.Edit(assignee);
        }

        public int SaveAssignee(TmAssignee model)
        {
            model.CompId = PortalContext.CurrentUser.CompId;
            return _assigneeRepository.Save(model);
        }

        public int DeleleAssignee(int assigneeId)
        {
            return _assigneeRepository.Delete(x => x.AssigneeId == assigneeId);
        }
        public TmAssignee GetAssigneeByAssigneeId(int assigneeId)
        {
            return _assigneeRepository.FindOne(x => x.AssigneeId == assigneeId);
        }
        public bool IsAssigneeExist(TmAssignee model)
        {
            return _assigneeRepository.Exists(x => x.Assignee == model.Assignee);
        }
    }
}
