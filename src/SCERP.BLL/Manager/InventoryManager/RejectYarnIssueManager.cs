
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using SCERP.BLL.IManager.IInventoryManager;
using SCERP.Common;
using SCERP.DAL.IRepository.IInventoryRepository;
using SCERP.Model.InventoryModel;

namespace SCERP.BLL.Manager.InventoryManager
{
    public class RejectYarnIssueManager : IRejectYarnIssueManager
    {
        private readonly IRejectYarnIssueRepository _rejectYarnIssueRepository;
        public RejectYarnIssueManager(IRejectYarnIssueRepository rejectYarnIssueRepository)
        {
            _rejectYarnIssueRepository = rejectYarnIssueRepository;
        }

        public List<Inventory_RejectYarnIssue> GetRejectYarns(string searchString, int pageIndex, string sort, string sortdir, out int totalRecord)
        {
            var pageSize = AppConfig.PageSize;
            IQueryable<Inventory_RejectYarnIssue> rejectYarnIssues = _rejectYarnIssueRepository.GetWithInclude(x => x.RejectYarnIssueId > 0, "Party");
            totalRecord = rejectYarnIssues.Count();
            rejectYarnIssues = rejectYarnIssues
                  .OrderByDescending(r => r.RefId)
                  .Skip(pageIndex * pageSize)
                  .Take(pageSize);
          
            return rejectYarnIssues.ToList();
        }

        public Inventory_RejectYarnIssue GetRejectYarnById(int rejectYarnIssueId)
        {
          return  _rejectYarnIssueRepository.FindOne(x => x.RejectYarnIssueId ==rejectYarnIssueId)??new Inventory_RejectYarnIssue();
        }

        public List<SpRejectYarnDetail> GetRejectYarnDetailById(int rejectYarnIssueId)
        {
            return _rejectYarnIssueRepository.GetRejectYarnDetailById(rejectYarnIssueId);
        }

        public int SaveRejectYarn(Inventory_RejectYarnIssue rejectYarnIssue)
        {
            rejectYarnIssue.IssueType = "S";
            return _rejectYarnIssueRepository.Save(rejectYarnIssue);
        }

        public string GetNewId()
        {
            var refIdDesgit = _rejectYarnIssueRepository.All().Max(x=>x.RefId) ?? "0";
            return refIdDesgit.IncrementOne().PadZero(4);
        }

        public int EditRejectYarn(Inventory_RejectYarnIssue rejectYarnIssue)
        {

            return _rejectYarnIssueRepository.Save(rejectYarnIssue);
        }

        public DataTable GetRejectYarnReport(int rejectYarnIssueId)
        {
            return _rejectYarnIssueRepository.ExecuteQuery(String.Format("exec spRejectYarnIssureReport {0}",rejectYarnIssueId));
        }
    }
}
