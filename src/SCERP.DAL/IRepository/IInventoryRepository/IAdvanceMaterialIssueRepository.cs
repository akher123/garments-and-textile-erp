using System;
using System.Collections.Generic;
using System.Linq;
using SCERP.Model.InventoryModel;

namespace SCERP.DAL.IRepository.IInventoryRepository
{
  public  interface IAdvanceMaterialIssueRepository:IRepository<Inventory_AdvanceMaterialIssue>
  {
      IQueryable<VwAdvanceMaterialIssue> GetAdvanceMaterialIssue(string compId, DateTime? fromDate, DateTime? toDate, string searchString, int storeId);
      VwAdvanceMaterialIssue GetVwAdvanceMaterialIssueById(long advanceMaterialIssueId, string compId);
        List<ProgramYarnWithdrow> GetProgramYarnWithdrow(string programRefId, string compId);
    }
}
