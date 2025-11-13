using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Common;
using SCERP.DAL.IRepository.IInventoryRepository;
using SCERP.Model.InventoryModel;

namespace SCERP.DAL.Repository.InventoryRepository
{
    public class RedyeingFabricIssueRepository :Repository<Inventory_RedyeingFabricIssue> ,IRedyeingFabricIssueRepository
    {
        public RedyeingFabricIssueRepository(SCERPDBContext context) : base(context)
        {
        }

        public List<VwRedyeingFabricIssueDetail> GetVwRedyeingFabricIssueDetailById(long redyeingFabricIssueId)
        {
            string sqlQuery = string.Format( @"select FID.*,(select ItemName from VwProdBatchDetail where BatchDetailId=FID.BatchDetailId) as ItemName,
                            (select BatchNo from Pro_Batch where BatchId=FID.BatchId) as BatchNo
                                         from Inventory_RedyeingFabricIssueDetail as FID
                                         where FID.RedyeingFabricIssueId='{0}'", redyeingFabricIssueId);
            return Context.Database.SqlQuery<VwRedyeingFabricIssueDetail>(sqlQuery).ToList();
        }

        public object GetRedyeingReceivedBatchAutocomplite(string compId, string searchString, long partyId)
        {
            object batchList = Context.Database.SqlQuery<SelectModel>(String.Format(
                @"select distinct B.BatchId as Value,  B.BatchId, B.BatchNo as Text from Inventory_ReDyeingFabricReceiveDetail as FID
            inner join Pro_Batch as B on FID.BatchId=B.BatchId
            where B.CompId='{0}' and B.PartyId='{1}' and B.BatchNo like'%{2}%'", compId, partyId, searchString)).ToList();
            return batchList;
        }

        public IEnumerable<dynamic> GetRedyeingReceivedBatchDetailByBatchId(long batchId)
        {
            string sqlQuery = @"select (SUM(RQty) -ISNULL((select SUM(Inventory_RedyeingFabricIssueDetail.ReprocessQty) from Inventory_RedyeingFabricIssueDetail  where BatchDetailId=rdfr.BatchDetailId),0)) as BalanceQty,
                            rdfr.BatchId, rdfr.BatchDetailId,(select 'Fabric Type :'+VwProdBatchDetail.ItemName+'  F/DIA :'+VwProdBatchDetail.FdiaSizeName from VwProdBatchDetail where BatchDetailId=rdfr.BatchDetailId) as ItemName
                            from Inventory_ReDyeingFabricReceiveDetail as rdfr
                            where rdfr.BatchId={0}
                            group by rdfr.BatchDetailId,rdfr.BatchId";
              DataTable redyeingIssueDetailTable= base.ExecuteQuery(String.Format(sqlQuery, batchId));

              return redyeingIssueDetailTable.Todynamic();
        }
    }
}
