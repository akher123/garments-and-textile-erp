using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.DAL.IRepository.IProductionRepository;
using SCERP.Model.Production;

namespace SCERP.DAL.Repository.ProductionRepository
{
    public class MachingInterruptionRepository : Repository<PROD_MachingInterruption>, IMachingInterruptionRepository
    {
        private readonly SCERPDBContext _context;
        public MachingInterruptionRepository(SCERPDBContext context) : base(context)
        {
            _context = context;
        }

        public List<SpProdMatchingInterruption> GetMachingInterruptionByDate(DateTime? interrupDate,string processRefId, string compId)
        {
            
            string sqlQuery = String.Format("exec [dbo].[SpProdMachingInterruption] @ProcessRefId='{0}',@InterrupDate='{1}',@CompId='{2}'", processRefId, interrupDate, compId);
            return _context.Database.SqlQuery<SpProdMatchingInterruption>(sqlQuery).ToList();
        }
   }
}
