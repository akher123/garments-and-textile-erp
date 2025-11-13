using System;
using System.Collections.Generic;
using System.Linq;
using SCERP.DAL.IRepository.IInventoryRepository;
using SCERP.Model.InventoryModel;

namespace SCERP.DAL.Repository.InventoryRepository
{
    public class AdvanceMaterialIssueRepository :Repository<Inventory_AdvanceMaterialIssue>, IAdvanceMaterialIssueRepository
    {
        public AdvanceMaterialIssueRepository(SCERPDBContext context) : base(context)
        {
        }

        public IQueryable<VwAdvanceMaterialIssue> GetAdvanceMaterialIssue(string companyId, DateTime? fromDate, DateTime? toDate, string searchString, int iType)
        {
           var advanceMaterialIssues= Context.VwAdvanceMaterialIssues.Where(x => x.CompId == companyId&&x.IType==iType
                                                       &&
                                                       ((x.IRefId.Trim()
                                                           .ToLower()
                                                           .Contains(searchString.Trim().ToLower()) ||
                                                        String.IsNullOrEmpty(searchString))
                                                        ||
                                                        (x.ProgramRefId.Trim()
                                                           .ToLower()
                                                           .Contains(searchString.Trim().ToLower()) ||
                                                        String.IsNullOrEmpty(searchString))
                                                       ||
                                                       (x.StyleNo.Trim()
                                                           .ToLower()
                                                           .Contains(searchString.Trim().ToLower()) ||
                                                        String.IsNullOrEmpty(searchString))
                                                       ||
                                                       (x.BuyerName.Trim()
                                                           .ToLower()
                                                           .Contains(searchString.Trim().ToLower()) ||
                                                        String.IsNullOrEmpty(searchString))
                                                       ||
                                                       (x.IRNoteNo.Trim()
                                                           .ToLower()
                                                           .Contains(searchString.Trim().ToLower()) ||
                                                        String.IsNullOrEmpty(searchString)))
                                                       &&
                                                       ((x.IRNoteDate >= fromDate || fromDate == null) &&
                                                        (x.IRNoteDate <= toDate || toDate == null)));
            return advanceMaterialIssues;
        }

        public VwAdvanceMaterialIssue GetVwAdvanceMaterialIssueById(long advanceMaterialIssueId, string compId)
        {
            return  Context.VwAdvanceMaterialIssues.FirstOrDefault(x => x.CompId == compId && x.AdvanceMaterialIssueId == advanceMaterialIssueId);
        }
        public List<ProgramYarnWithdrow> GetProgramYarnWithdrow(string programRefId, string compId)
        {
            string sql = String.Format("exec sel_ProgramYarnWithdrow @CompId='{0}',@ProgramRefId='{1}'", compId, programRefId);
            return Context.Database.SqlQuery<ProgramYarnWithdrow>(sql).ToList();
        }

    }
}
