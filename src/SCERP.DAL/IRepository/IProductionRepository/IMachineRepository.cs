using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using SCERP.Model;
using SCERP.Model.Production;

namespace SCERP.DAL.IRepository.IProductionRepository
{
    public interface IMachineRepository:IRepository<Production_Machine>
    {
        IQueryable<VMachine> GetVMachineList(Expression<Func<VMachine,bool>>predicat);
        string GetNewMachineRefId(string compId);
     
    }
}
