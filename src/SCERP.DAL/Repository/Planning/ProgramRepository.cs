using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using SCERP.Common;
using SCERP.DAL.IRepository.IPayrollRepository;
using SCERP.Model;
using SCERP.Model.InventoryModel;
using SCERP.Model.Planning;

namespace SCERP.DAL.Repository.Planning
{
    public class ProgramRepository : Repository<PLAN_Program>, IProgramRepository
    {
        public ProgramRepository(SCERPDBContext context) : base(context)
        {
        }

        public string GetNewProgramRefId(string compId)
        {

            var sqlQuery = String.Format("Select  substring(MAX(ProgramRefId),8,10 )from PLAN_Program where CompId='{0}'",
                compId);
            var issueReceiveNo =
              Context.Database.SqlQuery<string>(sqlQuery)
                  .SingleOrDefault() ?? "0";
            var maxNumericValue = Convert.ToInt32(issueReceiveNo);
            var irNo = "PP" + GetRefNumber(maxNumericValue, 8); // PLF/ORD
            return irNo;
        }

        public IQueryable<VwProgram> GetVwProgramList(Expression<Func<VwProgram, bool>> wherExpression)
        {
            return Context.VwPrograms.Where(wherExpression);
        }

        public List<VwAssignedProgram> GetVwAssignedProgramList(string searchString, string sewingProcessRefId, string compId)
        {
            return
                Context.VwAssignedPrograms.Where(x => x.ProcessRefId == sewingProcessRefId && (x.RefNo == searchString || string.IsNullOrEmpty(searchString)) && (x.StyleName == searchString || string.IsNullOrEmpty(searchString)) && x.CompId == compId && x.xStatus == "O")
                    .ToList();
        }

        public VwAssignedProgram GetAssignedProgram(string programRefId, string orderStyleRefId, string compId)
        {
            return
                Context.VwAssignedPrograms.FirstOrDefault(
                    x => x.CompId == compId && x.ProgramRefId == programRefId && x.OrderStyleRefId == orderStyleRefId);
        }

        public IQueryable<VwProgram> GetApprovedKnittingProgramList(string processRefId, string compId, bool isApproved)
        {
            return Context.VwPrograms.Where(x => x.CompId == compId && x.ProcessRefId == processRefId && (x.IsApproved == isApproved || x.IsApproved == isApproved));
        }

        public List<VwProgram> GetKnittingProgramStatus(DateTime? fromDate, DateTime? toDate, string processRefId, long PartyId, string searchString, string compId)
        {
            return Context.VwPrograms.Where(x => x.CompId == compId && x.ProcessRefId == processRefId
                && (((x.PrgDate >= fromDate || fromDate == null) && (x.PrgDate <= toDate || toDate == null)) && (x.PartyId == PartyId || PartyId == 0)) && (x.ProgramRefId.Contains(searchString) || string.IsNullOrEmpty(searchString))).ToList();
        }

        public DataTable GetPartyWiseKnittingBalance(string processorRefId, string compId, long partyId)
        {
            string spString = string.Format(@"exec SpProdPartyWiseKnittingStatus '{0}','{1}','{2}'", compId, processorRefId, partyId);
            return ExecuteQuery(spString);

        }

        private string GetRefNumber(int maxNumericValue, int length)
        {
            var refNumber = Convert.ToString(maxNumericValue + 1);
            while (refNumber.Length != length)
            {
                refNumber = "0" + refNumber;
            }
            return refNumber;
        }

        public List<Dropdown> GetConumptionFabrics(string orderStyleRefId, string colorRefId, string compId)
        {
            string sql = @"select distinct ItemCode AS Id, ItemName as [Value] from VCompConsumptionDetail
                        where OrderStyleRefId='{0}' and PColorRefId='{1}' and CompId='{2}'";
            return Context.Database.SqlQuery<Dropdown>(string.Format(sql,orderStyleRefId,colorRefId,compId)).ToList();
        }

        public List<Dropdown> GetFabricColorNameByStyle(string orderStyleRefId, string compId)
        {
            string sql = @"select distinct PColorRefId AS Id, PColorName as [Value] from VCompConsumptionDetail 
                        where OrderStyleRefId='{0}' and CompId='{1}'";
            return Context.Database.SqlQuery<Dropdown>(string.Format(sql, orderStyleRefId,compId)).ToList();
        }

        public List<ProgramYarnRetur> GetProgramYarnReturn(long programId)
        {
            string sql = @" [dbo].[sel_ProgramYarnReturn] @ProgramId={0}";
            return Context.Database.SqlQuery<ProgramYarnRetur>(string.Format(sql, programId)).ToList();
        }
    }
}
