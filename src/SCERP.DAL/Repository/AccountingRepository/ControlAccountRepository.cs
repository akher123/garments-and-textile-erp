using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.DAL.IRepository.IAccountingRepository;
using SCERP.Model;
using System.Data.SqlClient;

namespace SCERP.DAL.Repository.AccountingRepository
{
    public class ControlAccountRepository : Repository<Acc_ControlAccounts>, IControlAccountRepository
    {
        private SCERPDBContext context;

        public ControlAccountRepository(SCERPDBContext context) : base(context)
        {
            this.context = context;
        }

        public int GetMaxControlCode(int parentCode)
        {
            decimal max = context.Database.SqlQuery<decimal>(
                "select isnull(max(ControlCode),0)+1 as ControlCode from Acc_ControlAccounts where ParentCode=" +
                parentCode + "").SingleOrDefault();
            return Convert.ToInt32(max);
        }

        public string GetControlNameByCode(string controlcode)
        {
            decimal controlCode = 0;
            if (!string.IsNullOrEmpty(controlcode))
                controlCode = Convert.ToDecimal(controlcode);

            var accControlAccounts = context.Acc_ControlAccounts.FirstOrDefault(p => p.ControlCode == controlCode);
            if (accControlAccounts != null) return accControlAccounts.ControlName;
            else
                return "Not Found";
        }

        public int ControltoSubGroupChange(string SubGroupCode, string ControlCode)
        {
            var SubGroupCodeParam = new SqlParameter { ParameterName = "SubGroupCode", Value = Convert.ToDecimal(SubGroupCode.Trim()) };
            var ControlCodeParam = new SqlParameter { ParameterName = "ControlCode", Value = Convert.ToDecimal(ControlCode.Trim()) };

            List<int> result = Context.Database.SqlQuery<int>("ControltoSubGroupChange @SubGroupCode, @ControlCode", SubGroupCodeParam, ControlCodeParam).ToList();
            return 1;
        }

        public int GLtoControlChange(string GLCode, string ControlCode)
        {
            var GLCodeParam = new SqlParameter { ParameterName = "GLCode", Value = Convert.ToDecimal(GLCode.Trim()) };
            var ControlCodeParam = new SqlParameter { ParameterName = "ControlCode", Value = Convert.ToDecimal(ControlCode.Trim()) };

            List<int> result = Context.Database.SqlQuery<int>("GLtoControlChange @GLCode, @ControlCode", GLCodeParam, ControlCodeParam).ToList();
            return 1;
        }

        public int GLtoSubGroupChange(string GLCode, string SubGroup)
        {
            var GLCodeParam = new SqlParameter { ParameterName = "GLCode", Value = Convert.ToDecimal(GLCode.Trim()) };
            var SubGroupCodeParam = new SqlParameter { ParameterName = "SubGroupCode", Value = Convert.ToDecimal(SubGroup.Trim()) };

            List<int> result = Context.Database.SqlQuery<int>("GLtoSubGroupChange @GLCode, @SubGroupCode", GLCodeParam, SubGroupCodeParam).ToList();
            return 1;
        }
    }
}
