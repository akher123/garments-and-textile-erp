using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.MerchandisingModel;

namespace SCERP.DAL.IRepository.IMerchandisingRepository
{
    public interface IThreadConsumptionRepository:IRepository<OM_ThreadConsumption>
    {
        List<VwThreadConsumption> GetThreadConsumptionsByPaging(string compId,string searchString);
        DataTable GetThreadConsumptionsReportDataTable(long threadConsumptionId);
    }
}
