using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.DAL.IRepository.IProductionRepository;
using SCERP.Model.Production;

namespace SCERP.DAL.Repository.ProductionRepository
{
    public class NonProductiveTimeRepository :Repository<PROD_NonProductiveTime>, INonProductiveTimeRepository
    {
        public NonProductiveTimeRepository(SCERPDBContext context) : base(context)
        {
        }

        public List<VwNonProductiveTime> GetNpts(DateTime? fromDate, string compId)
        {
            string sqlQuery =
                @"select * from VwNonProductiveTime where Convert(date,EntryDate)=Convert(date,@FromDate) and CompId=@CompId";
            object[] parameters = new object[]
            {
                new SqlParameter("@FromDate", fromDate),
                new SqlParameter("@CompId", compId),
            };
            return Context.Database.SqlQuery<VwNonProductiveTime>(sqlQuery, parameters).ToList();
        }

        public List<VwNonProductiveTime> GetDateWiseNpts(DateTime? fromDate, DateTime? toDate, string compId)
        {
            string sqlQuery =
             @"select * from VwNonProductiveTime where Convert(date,EntryDate)>=Convert(date,@FromDate) and Convert(date,EntryDate)<=Convert(date,@ToDate)  and CompId=@CompId";
            object[] parameters = new object[]
            {
                new SqlParameter("@FromDate", fromDate),
                 new SqlParameter("@ToDate", toDate),
                new SqlParameter("@CompId", compId),
            };
            return Context.Database.SqlQuery<VwNonProductiveTime>(sqlQuery, parameters).ToList();
        }
    }
}
