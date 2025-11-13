using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.CommonModel;

namespace SCERP.BLL.IManager.ICommonManager
{
   public interface IGatePassManager
    {
       List<GatePass> GetGatePassByPaging
           (string typeId,string searchString, string compId, int pageIndex, string sort, string sortdir, out int totalRecords);

       GatePass GetGatePassById(int gatePassId);
       List<GatePassDetail> GetGatepassDetailById(int gatePassId);
       string GetGatePassRefId(string compId,string typeId);
       int EditGatePass(GatePass gatePass);
       int DeleteGatePass(int gatePassId);
       int SaveGatePass(GatePass gatePass);

       DataTable GatePassReport(int gatePassId);
       int ApprovedGatePass(int gatePassId);
        List<GatePass> GetGateSamplePass(string typeId, string searchString, string compId);
    }
}
