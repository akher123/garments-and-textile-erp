using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Data.Entity;
using SCERP.DAL.IRepository.IInventoryRepository;
using SCERP.Model.InventoryModel;

namespace SCERP.DAL.Repository.InventoryRepository
{
    public class GreyIssueRepository :Repository<Inventory_GreyIssue>, IGreyIssueRepository
    {
        public GreyIssueRepository(SCERPDBContext context) : base(context)
        {
        }

        public List<KnittingOrderDelivery> GetKnittingOrderDelivery(int programId, long greyIssueId)
        {
            return Context.Database.SqlQuery<KnittingOrderDelivery>("spGetKnittingOrderDelivery {0},{1}", programId, greyIssueId).ToList();
        }

        public int DeleteGreyIssue(long greyIssueId)
        {
            var parent = Context.Inventory_GreyIssue.Include(x =>x.Inventory_GreyIssueDetail).SingleOrDefault(p => p.GreyIssueId == greyIssueId);
            foreach (var child in parent.Inventory_GreyIssueDetail.ToList())
            {
                Context.Inventory_GreyIssueDetail.Remove(child);
            }
            Context.Inventory_GreyIssue.Remove(parent);
           return Context.SaveChanges();
        }
        public DataTable GetGeryIssuePartyChallan(long greyIssueId)
        {
            string sql = string.Format("exec [spGetGreyIssueChallan] {0}", greyIssueId);
            return base.ExecuteQuery(sql);
        }
    }
}
