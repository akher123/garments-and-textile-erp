
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using SCERP.DAL.IRepository.IProductionRepository;
using SCERP.Model;
using SCERP.Model.Production;

namespace SCERP.DAL.Repository.ProductionRepository
{
    public class MachineRepository :Repository<Production_Machine>, IMachineRepository
    {
        public MachineRepository(SCERPDBContext context) : base(context)
        {
        }

        public IQueryable<VMachine> GetVMachineList(Expression<Func<VMachine, bool>> predicat)
        {
            return Context.VMachines.Where(predicat);
        }

        public string GetNewMachineRefId(string compId)
        {

            var sqlQuery =
                String.Format(
                    "SELECT RIGHT('000'+ CONVERT(VARCHAR(3),ISNULL(max(MachineRefId),00)+1),3)  AS MachineRefId FROM Production_Machine WHERE CompId='{0}'",
                    compId);
            return Context.Database.SqlQuery<string>(sqlQuery).FirstOrDefault();
        }

       
    }
}
