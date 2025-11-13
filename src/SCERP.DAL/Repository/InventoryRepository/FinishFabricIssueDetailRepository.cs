using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using SCERP.Common;
using SCERP.DAL.IRepository;
using SCERP.DAL.IRepository.IInventoryRepository;
using SCERP.Model.InventoryModel;
using SCERP.Model.Production;

namespace SCERP.DAL.Repository.InventoryRepository
{
    public class FinishFabricIssueDetailRepository : Repository<Inventory_FinishFabricIssueDetail>, IFinishFabricIssueDetailRepository
    {
        public FinishFabricIssueDetailRepository(SCERPDBContext context)
            : base(context)
        {
        }

        public List<VwFinishFabricIssueDetail> GetFinishFabIssureDetails(long finishFabIssueId)
        {
            string sqlQuery = String.Format(@"select FID.*,(select ItemName from VwProdBatchDetail where BatchDetailId=FID.BatchDetailId) as ItemName,
                            (select BatchNo from Pro_Batch where BatchId=FID.BatchId) as BatchNo
                                         from Inventory_FinishFabricIssueDetail as FID
                                         where FinishFabricIssueId='{0}'", finishFabIssueId);
            return Context.Database.SqlQuery<VwFinishFabricIssueDetail>(sqlQuery).ToList();
        }

        public object GetReceivedBatchAutocomplite(string compId, string searchString, long partyId)
        {
            object batchList = Context.Database.SqlQuery<SelectModel>(String.Format(
                @"select distinct B.BatchId as Value,  B.BatchId, B.BatchNo as Text from Inventory_FinishFabricIssueDetail as FID
            inner join Pro_Batch as B on FID.BatchId=B.BatchId
            where B.CompId='{0}' and B.PartyId='{1}' and B.BatchNo like'%{2}%'", compId, partyId, searchString)).ToList();
            return batchList;
        }


        public object GetReceivedBatchAutoComplite(string searchString)
        {

            object batchList = Context.Database.SqlQuery<SelectModel>(String.Format(
                @"select top(20) B.BatchId as Value,  B.BatchId, B.BatchNo as Text 
                    from Pro_Batch AS B
                where B.BatchNo like'%{0}%' 
				and B.BatchId IN  (
				select distinct FID.BatchId from Inventory_FinishFabricIssue AS FI
				INNER JOIN Inventory_FinishFabricIssueDetail AS FID ON FI.FinishFabIssueId=FID.FinishFabricIssueId
				where FI.IsReceived=1)", searchString)).ToList();
            return batchList;
        }
        public IEnumerable GetReceivedBatchDetailByBatchId(long batchId)
        {
            List<VwProdBatchDetail> batchDetails = Context.Database.SqlQuery<VwProdBatchDetail>(String.Format(
                @" select * from VwProdBatchDetail where BatchDetailId in (select distinct BatchDetailId from Inventory_FinishFabricIssueDetail where BatchId={0})", batchId)).ToList();
            return batchDetails.Select(x => new
            {
                x.BatchDetailId,
                ItemName = "Fabric Type:" + x.ItemName + "  " + "GSM :" + x.GSM + " DIA :" + x.FdiaSizeName,
                x.Quantity
            }).ToList();
        }

        public bool IsExistReceivedBatch(string batchNo)
        {
              string sql = String.Format(@"select top(1)b.BatchNo from Inventory_FinishFabricIssue AS FI
				INNER JOIN Inventory_FinishFabricIssueDetail AS FID ON FI.FinishFabIssueId=FID.FinishFabricIssueId
				INNER JOIN Pro_Batch AS B ON FID.BatchId=B.BatchId
				where FI.IsReceived=1 and B.BatchNo='{0}'", batchNo);
               return Context.Database.SqlQuery<string>(sql).Any();

        }
    }
}
