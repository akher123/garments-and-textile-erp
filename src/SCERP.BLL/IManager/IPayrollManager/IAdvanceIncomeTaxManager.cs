using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.PayrollModel;

namespace SCERP.BLL.IManager.IPayrollManager
{
    public interface IAdvanceIncomeTaxManager
    {
        List<AdvanceIncomeTax> GetAllAdvanceIncomeTaxsByPaging(AdvanceIncomeTax model, out int totalRecords);
        List<AdvanceIncomeTax> GetAllAdvanceIncomeTaxs();
        AdvanceIncomeTax  GetAdvanceIncomeTaxId(int advanceTaxId);
        int SaveAdvanceIncomeTax(AdvanceIncomeTax  model);
        int EditAdvanceIncomeTax(AdvanceIncomeTax  model);
        int DeleteAdvanceIncomeTax(int advanceTaxId);
        bool IsStyleAdvanceIncomeTaxExist(AdvanceIncomeTax  model);
        string GetNewAdvanceIncomeTaxRefId(string prifix);
    }
}
