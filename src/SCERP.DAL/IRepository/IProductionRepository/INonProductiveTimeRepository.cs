using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.Production;

namespace SCERP.DAL.IRepository.IProductionRepository
{
    public interface INonProductiveTimeRepository:IRepository<PROD_NonProductiveTime>
    {
        List<VwNonProductiveTime> GetNpts(DateTime? fromDate, string compId);
        List<VwNonProductiveTime> GetDateWiseNpts(DateTime? fromDate, DateTime? toDate, string compId);
    }
}
