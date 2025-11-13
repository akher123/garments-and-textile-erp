using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Common;
using SCERP.Model;
using SCERP.Model.InventoryModel;
using SCERP.Model.Planning;
using SCERP.Model.Production;

namespace SCERP.BLL.IManager.IPlanningManager
{
    public interface IProgramManager
    {
        string GetNewProgramRefId(string prefix, string processRefId);
        List<PLAN_Program> GetPrograms(string orderStyleRefId);
        int SaveProgram(PLAN_Program model, List<VProgramDetail> inPutProgramDetails);
        List<VProgramDetail> GetInPutProgramDetails(long programId);
        List<VProgramDetail> GetOutPutProgramDetails(long programId);
        PLAN_Program GetProgramById(long programId);
        bool IsExistProgram(string orderStyleRefId, string processRefId);
        int EditProgram(PLAN_Program program, List<VProgramDetail> inPutProgramDetails);
        List<VwProgram> GetVwProgramsByPaging(string processRefId, out int totalRecords);
        decimal DeleteProgram(string programRefId, string orderStyleRefId);
        List<VwAssignedProgram> GetVwPrograms(string searchString);
        IEnumerable GetProgramCollarCuffAutocomplite(string serachString, string compId);
        VwAssignedProgram GetAssignedProgram(string programRefId, string orderStyleRefId);
        List<PLAN_Program> GetProgramsByPatins(string searchString, out int totalRecords);
        int  SaveProgram(PLAN_Program program);
        int EditProgram(PLAN_Program program);
        List<VwProgram> GetVwProgramsPaging(string searchString, string processRefId, int pageIndex, DateTime? fromDate, DateTime? toDate, out int totalRecords);
        int DeleteProgramById(long programId);
        List<VwProgram> GetApprovedKnittingProgramByPaging(string processRefId, int pageIndex, string sort, string sortdir, bool isApproved, string compId, out int totalRecords);
        int ApprovedKnittingProgram(long programId, string compId);
        List<VwProgram> GetVProgramById(long programId);
        List<VwProgram> GetKnittingProgramStatus(DateTime? fromDate, DateTime? toDate,string processRefId, long partyId, string searchString, string compId);
        IEnumerable GetProgramAutocomplite(string serachString, long partyId, string compId, string processRefId);

        DataTable GetPartyWiseKnittingBalance(string processorRefId, string compId, long partyId);

        int LockedProgram   (long programId, string compId);
        List<VwProgram> GetLokedProgramByPaging(string knitting, int pageIndex, string sort, string sortdir, bool locked, string compId, out int totalRecords);
        bool ProgramIsLoked(long programId);

        List<PLAN_Program> GetProgramsByProcessType(string processRefId,string compId);
        VwProgram GetProgramByProgramRefId(string piBookingRefId, string compId);


        IEnumerable GetProgramAutocomplite(string serachString, string p1, string p2);

        IEnumerable GetProgramAutocomplite(string serachString, string p);
        IEnumerable GetProgramAllAutocomplite(string serachString, string compId);
        List<Dropdown> GetConumptionFabrics(string orderStyleRefId,string colorRefId,string compId);
        List<Dropdown> GetFabricColorNameByStyle(string orderStyleRefId,string compId);
        int UpdateProgramRate(List<VProgramDetail> values);
        List<ProgramYarnRetur> GetProgramYarnReturn(long programId);
    }
}
