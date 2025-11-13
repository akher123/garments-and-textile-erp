using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Common;
using SCERP.DAL.IRepository.IProductionRepository;
using SCERP.Model.Production;

namespace SCERP.DAL.Repository.ProductionRepository
{
    public class DyeingJobOrderRepository :Repository<PROD_DyeingJobOrder>, IDyeingJobOrderRepository
    {
        public DyeingJobOrderRepository(SCERPDBContext context) : base(context)
        {
        }

    

        public List<VwDyeingJobOrderDetail> GetDyeingJobOrderDetails(long dyeingJobOrderId)
        {
            return
                Context.Database.SqlQuery<VwDyeingJobOrderDetail>(
                    "select * from VwDyeingJobOrderDetail where DyeingJobOrderId='" + dyeingJobOrderId + "' ").ToList();
        }
        public List<Dropdown> GetDyeingJobOrderByPartyId(long partyId)
        {
            string sql = @"select JO.JobRefId AS Id, 'Order No :'+ Jo.WorkOrderNo +' Party :'+p.Name AS Value from PROD_DyeingJobOrder AS JO
                             INNER JOIN Party AS P ON JO.PartyId = P.PartyId
                             where P.PartyId = '{0}'";
            return Context.Database.SqlQuery<Dropdown>(string.Format(sql, partyId)).ToList();
        }

        public List<Dropdown> GetKnittingRollIssueChallan(string orderStyleRefId)
        {
            string sql = @"select distinct IssueRefNo AS Id,IssueRefNo AS [Value] from PROD_KnittingRollIssue
                           where OrderStyleRefId= '{0}' and IssueRefNo not  IN ( select WorkOrderNo from PROD_DyeingJobOrder where OrderStyleRefId='{0}')";
            return Context.Database.SqlQuery<Dropdown>(string.Format(sql, orderStyleRefId)).ToList();
        }
    }
}
