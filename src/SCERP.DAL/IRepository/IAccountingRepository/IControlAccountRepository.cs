using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;

namespace SCERP.DAL.IRepository.IAccountingRepository
{
    public interface IControlAccountRepository:IRepository<Acc_ControlAccounts>
    {
        int GetMaxControlCode(int ParentCode);
        string GetControlNameByCode(string controlcode);
        int ControltoSubGroupChange(string SubGroupCode, string ControlCode);
        int GLtoControlChange(string GLCode, string ControlCode);   
        int GLtoSubGroupChange(string GLCode, string SubGroup);
    }
}