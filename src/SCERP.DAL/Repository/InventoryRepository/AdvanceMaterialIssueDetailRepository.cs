
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using SCERP.DAL.IRepository.IInventoryRepository;
using SCERP.Model.InventoryModel;

namespace SCERP.DAL.Repository.InventoryRepository
{
    public class AdvanceMaterialIssueDetailRepository:Repository<Inventory_AdvanceMaterialIssueDetail>,IAdvanceMaterialIssueDetailRepository
    {
        public AdvanceMaterialIssueDetailRepository(SCERPDBContext context) : base(context)
        {
        }
        public List<VwAdvanceMaterialIssueDetail> GetVwAdvanceMaterialssDtl(long advanceMaterialIssueId, string compId)
        {
            //return Context.VwAdvanceMaterialIssueDetails.Where(x => x.CompId == compId && x.AdvanceMaterialIssueId == advanceMaterialIssueId).ToList();
          return  Context.Database.SqlQuery<VwAdvanceMaterialIssueDetail>(
                String.Format(@"EXEC [dbo].[GeYarnDeliveryChallan] '{0}', '{1}'", compId, advanceMaterialIssueId)).ToList();
        }

        public List<VwAdvanceMaterialIssueDetail> GetDeliverdYarnDetail(string refId, string compId)
        {
            return Context.VwAdvanceMaterialIssueDetails.Where(x => x.CompId == compId && x.IRefId == refId).ToList();
        }

        public List<AccessoriesReceiveBalance> GetAccessoriesRcvSummary(string orderStyleRefId, string compId)
        {
       return  Context.Database
                .SqlQuery<AccessoriesReceiveBalance>("exec spInvAccessoriesRcvSummary '" + orderStyleRefId + "','" + compId + "'").ToList();
            
        }

        public List<AccessoriesReceiveBalance> GetAccessorisEditRcvDetails(long materialIssueAdvanceMaterialIssueId)
        {
            return Context.Database
                .SqlQuery<AccessoriesReceiveBalance>("exec spInAccessoriesEditRcvBalance '" + materialIssueAdvanceMaterialIssueId + "'").ToList();
        }

        public DataTable GetAccessoriesIssueChallanDataTable(long advanceMaterialIssueId)
        {
            SqlConnection connection = (SqlConnection)Context.Database.Connection;

            using (SqlCommand cmd = new SqlCommand("spInvAccessoriesIssueChallan"))
            {
                cmd.Connection = connection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@AdvanceMaterialIssueId", SqlDbType.BigInt).Value = advanceMaterialIssueId;
          
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    return dt;
                }
            }
        }

        public DataTable GetAccessoriesIssueDetailStatus(DateTime? fromDate, DateTime? toDate, string compid)
        {
            SqlConnection connection = (SqlConnection)Context.Database.Connection;

            using (SqlCommand cmd = new SqlCommand("InvSpAccessoriesIssueDetail"))
            {
                cmd.Connection = connection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = fromDate;
                cmd.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = toDate;
                cmd.Parameters.Add("@CompId", SqlDbType.VarChar).Value = compid;
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    return dt;
                }
            }
        }
    }
}
