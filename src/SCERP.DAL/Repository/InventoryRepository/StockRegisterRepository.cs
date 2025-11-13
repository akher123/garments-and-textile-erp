using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using SCERP.DAL.IRepository.IInventoryRepository;
using SCERP.Model.InventoryModel;

namespace SCERP.DAL.Repository.InventoryRepository
{
    public class StockRegisterRepository : Repository<Inventory_StockRegister>, IStockRegisterRepository
   {
        public StockRegisterRepository(SCERPDBContext context) : base(context)
        {
        }

        public List<VwStockPosition> GetStockPostion(DateTime? fromDate, DateTime? toDate, int groupId, int subGroupId)
        {
            var sqParams = new object[]
            {
                  new SqlParameter {ParameterName = "SubGroupId", Value = subGroupId},
                 new SqlParameter {ParameterName = "GroupId", Value = groupId},
                new SqlParameter {ParameterName = "FromDate", Value = fromDate},
                new SqlParameter {ParameterName = "ToDate", Value = toDate},

            };
            return Context.Database.SqlQuery<VwStockPosition>("InvSpAdvanceReportStock @SubGroupId, @GroupId ,@FromDate,@ToDate", sqParams).ToList();
          
        }

        public List<VwStockPosition> GetStockPostionDetail(DateTime? fromDate, DateTime? toDate, int groupId, int subGroupId)
        {
            var sqParams = new object[]
            {
                  new SqlParameter {ParameterName = "SubGroupId", Value = subGroupId},
                 new SqlParameter {ParameterName = "GroupId", Value = groupId},
                new SqlParameter {ParameterName = "FromDate", Value = fromDate},
                new SqlParameter {ParameterName = "ToDate", Value = toDate},

            };
            return Context.Database.SqlQuery<VwStockPosition>("InvSpAdvanceReportStockDetail @SubGroupId, @GroupId ,@FromDate,@ToDate", sqParams).ToList();
          
        }

       public List<VwStockPosition> GetDyedYarnStockPostionDetail(DateTime? fromDate, DateTime? toDate, int groupId, int subGroupId)
       {
           var sqParams = new object[]
           {
               new SqlParameter {ParameterName = "SubGroupId", Value = subGroupId},
               new SqlParameter {ParameterName = "GroupId", Value = groupId},
               new SqlParameter {ParameterName = "FromDate", Value = fromDate},
               new SqlParameter {ParameterName = "ToDate", Value = toDate},

           };
           return Context.Database.SqlQuery<VwStockPosition>("InvSpDyedYarnStockDetail @SubGroupId, @GroupId ,@FromDate,@ToDate", sqParams).ToList();
          
       }

        public List<VwStockPosition> GetBuyerWiseStockPostionDetail(DateTime? fromDate, DateTime? toDate)
        {
            var sqParams = new object[]
            {
              
                new SqlParameter {ParameterName = "FromDate", Value = fromDate},
                new SqlParameter {ParameterName = "ToDate", Value = toDate},

            };
            return Context.Database.SqlQuery<VwStockPosition>("InvBuyerWiseYarnStockDetail @FromDate,@ToDate", sqParams).ToList();
          
        }
   }
}
