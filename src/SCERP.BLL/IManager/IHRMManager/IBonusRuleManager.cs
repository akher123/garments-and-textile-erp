using SCERP.Common;
using SCERP.Model.PayrollModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.BLL.IManager.IHRMManager
{
    public interface IBonusRuleManager
    {
        List<BonusRule> GetBonusRulesByPaging(int pageIndex, string sort, string sortdir, out int totalRecords, string searchString);
        BonusRule GetBonusRuleById(int bonusRuleId, string compId);
        string GetNewBounusRuleRefId(string compId);
        bool IsBounusRoleExist(BonusRule bonusRule);
        int EditBousRule(BonusRule bonusRule);
        int SaveBousBole(BonusRule bonusRule);
        int DeleteBounusRole(BonusRule bonusRule);
        List<SelectModel> GetUnPorcessBonusRule(int companyId);
    }
}
