using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Common;
using SCERP.DAL.IRepository.IInventoryRepository;
using SCERP.Model.InventoryModel;

namespace SCERP.DAL.Repository.InventoryRepository
{
    public class FinishFabStoreRepository :Repository<Inventory_FinishFabStore>, IFinishFabStoreRepository
    {
        public FinishFabStoreRepository(SCERPDBContext context) : base(context)
        {
        }

        public List<SpInvFinishFabStore> GetDetailChallanBy(long dyeingSpChallanId, long finishFabStoreId)
        {
            string sqlQuery = String.Format("exec SpInvFinishFabStore '{0}','{1}'", finishFabStoreId, dyeingSpChallanId);
            return Context.Database.SqlQuery<SpInvFinishFabStore>(sqlQuery).ToList();
        }

        public IEnumerable GetBatchDeailsById(long batchId)
        {
            string sqlQuery = @"select FD.BatchDetailId,BD.ItemName from Inventory_FinishFabDetailStore  as FD 
                             inner join VwProdBatchDetail  as BD on FD.BatchDetailId=BD.BatchDetailId
                                 where FD.BatchId='{0}' and FD.CompId='{1}'";
            string sqlCommand = string.Format(sqlQuery, batchId,PortalContext.CurrentUser.CompId);
            return Context.Database.SqlQuery<VwFinishFabricIssueDetail>(sqlCommand).ToList();
        }

        public object GetFinishFabIssueDetail(long batchDetailId)
        {
            string sqlQuery = @"select (SUM(RcvQty)-(select ISNULL(SUM(FDI.FabQty),0)  from Inventory_FinishFabricIssueDetail as FDI where FDI.BatchDetailId=FDS.BatchDetailId))  as FabQty ,(select ISNULL(SUM(FDI.FabQty),0)  from Inventory_FinishFabricIssueDetail as FDI where FDI.BatchDetailId=FDS.BatchDetailId) as IssueQty,
                              	(SUM(FDS.GreyWt)-(select ISNULL(SUM(FDI.GreyWt),0)  from Inventory_FinishFabricIssueDetail as FDI where FDI.BatchDetailId=FDS.BatchDetailId))  as GreyWt ,
                               (SUM(FDS.CcuffQty)-(select ISNULL(SUM(FDI.CcuffQty),0)  from Inventory_FinishFabricIssueDetail as FDI where FDI.BatchDetailId=FDS.BatchDetailId))  as CcuffQty 
                               from Inventory_FinishFabDetailStore as FDS
                               where FDS.BatchDetailId='{0}' and CompId='{1}'
                               group by FDS.BatchDetailId";
            string sqlCommand = string.Format(sqlQuery, batchDetailId, PortalContext.CurrentUser.CompId);
            List<VwFinishFabricIssueDetail>finishFabricIssues= Context.Database.SqlQuery<VwFinishFabricIssueDetail>(sqlCommand).ToList();
            return finishFabricIssues.Select(x=>new
            {
                x.FabQty,
                x.IssueQty,
                x.GreyWt,
                x.CcuffQty
            }).FirstOrDefault();
        }
    }
}
