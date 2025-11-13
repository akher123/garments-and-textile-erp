using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.DAL.IRepository.IAccountingRepository;
using SCERP.Model;

namespace SCERP.DAL.Repository.AccountingRepository
{
    public class PermitedChartOfAccountRepository : Repository<Acc_PermitedChartOfAccount>, IPermitedChartOfAccountRepository
    {
        public PermitedChartOfAccountRepository(SCERPDBContext context) : base(context)
        {

        }

        public List<Acc_PermitedChartOfAccount> GetPermitedChartOfAccount(int companySectorId)
        {
            return Filter(x => x.SectorId == companySectorId && x.IsActive).ToList();
        }

        public bool CheckExistVoucherByGLId(int controlLevel, int id, ref string message)
        {
            List<Acc_VoucherDetail> result;

            if (controlLevel == 5)
            {
                result = Context.Acc_VoucherDetail.Where(p => p.GLID == id).ToList();

                if (result.Any())
                {
                    message = "Can not delete, there are vouchers exist with this Ledger !";
                    return true;
                }
            }

            else if (controlLevel == 4)
            {
                var accControlAccounts = Context.Acc_ControlAccounts.FirstOrDefault(p => p.Id == id);

                if (accControlAccounts != null)
                {
                    var controlcode = accControlAccounts.ControlCode;

                    var temp = Context.Acc_GLAccounts.Where(p => p.ControlCode == controlcode);

                    if (temp.Any())
                    {
                        message = "Can not delete, GL code exists for this control code !";
                        return true;
                    }
                }
            }

            else
            {
                var accControlAccounts = Context.Acc_ControlAccounts.FirstOrDefault(p => p.Id == id);

                if (accControlAccounts != null)
                {
                    var controlcode = accControlAccounts.ControlCode;

                    var temp = Context.Acc_ControlAccounts.Where(p => p.ParentCode == controlcode);

                    if (temp.Any())
                    {
                        message = "Can not delete, control code exists for this code !";
                        return true;
                    }
                }
            }
            return false;
        }

        public bool CheckExistingName(Acc_GLAccounts glAccount)
        {
            if (glAccount.AccountCode.ToString().Length > 7)
            {
                var temp = Context.Acc_GLAccounts.Where(p => p.AccountCode != glAccount.AccountCode);

                foreach (var t in temp)
                {
                    if (t.AccountName.Trim().ToLower() == glAccount.AccountName.Trim().ToLower())
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool CheckExistingName(Acc_ControlAccounts controlAccounts)
        {
            var temp = Context.Acc_ControlAccounts.Where(p => p.ParentCode == controlAccounts.ParentCode && p.ControlCode != controlAccounts.ControlCode);

            foreach (var t in temp)
            {
                if (t.ControlName.Trim().ToLower() == controlAccounts.ControlName.Trim().ToLower())
                {
                    return true;
                }
            }
            return false;
        }

        public int GetGLAccountIdByCode(string accountCode)
        {
            decimal accCode = 0;

            if (!string.IsNullOrEmpty(accountCode))
            {
                accCode = Convert.ToDecimal(accountCode);
            }

            Acc_GLAccounts gl = Context.Acc_GLAccounts.FirstOrDefault(p => p.AccountCode == accCode);

            if (gl != null)
                return gl.Id;
            else
                return 0;
        }

        public int GetControlIdByCode(string controlCode)
        {
            decimal controlcode = 0;

            if (!string.IsNullOrEmpty(controlCode))
                controlcode = Convert.ToDecimal(controlCode);

            Acc_ControlAccounts control = Context.Acc_ControlAccounts.FirstOrDefault(p => p.ControlCode == controlcode);

            if (control != null) return control.Id;
            else return 0;
        }
    }
}
