using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using SCERP.Common;
using SCERP.Model;
using SCERP.Model.InventoryModel;
using SCERP.Model.Planning;

namespace SCERP.DAL.IRepository.IPayrollRepository
{
    public interface IProgramRepository:IRepository<PLAN_Program> 
    {
        string GetNewProgramRefId(string compId);
        IQueryable<VwProgram> GetVwProgramList(Expression<Func<VwProgram,bool>>wherExpression );
        List<VwAssignedProgram> GetVwAssignedProgramList(string searchString, string sewingProcessRefId, string compId);
        VwAssignedProgram GetAssignedProgram(string programRefId, string orderStyleRefId,string compId);
        IQueryable<VwProgram> GetApprovedKnittingProgramList(string processRefId, string compId, bool isApproved);
        List<VwProgram> GetKnittingProgramStatus(DateTime? fromDate, DateTime? toDate,string processRefId, long partyId, string searchString, string compId);
        DataTable GetPartyWiseKnittingBalance(string processorRefId, string compId, long partyId);
        List<Dropdown> GetConumptionFabrics(string orderStyleRefId, string colorRefId, string compId);
        List<Dropdown> GetFabricColorNameByStyle(string orderStyleRefId, string compId);
        List<ProgramYarnRetur> GetProgramYarnReturn(long programId);
    }
}
