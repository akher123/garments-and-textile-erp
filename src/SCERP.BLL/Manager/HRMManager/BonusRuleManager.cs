using SCERP.BLL.IManager.IHRMManager;
using SCERP.Common;
using SCERP.DAL.IRepository;
using SCERP.Model.PayrollModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.BLL.Manager.HRMManager
{
    public class BonusRuleManager : IBonusRuleManager
    {
        private readonly IRepository<BonusRule> bonusRuleRepository;
        public BonusRuleManager(IRepository<BonusRule> bonusRuleRepository)
        {
            this.bonusRuleRepository = bonusRuleRepository;
        }
        public List<BonusRule> GetBonusRulesByPaging(int pageIndex, string sort, string sortdir, out int totalRecords, string searchString)
        {
            var index = pageIndex;
            var pageSize = AppConfig.PageSize;
            var bonusRules =
                bonusRuleRepository.Filter(x => x.Title.Trim().Contains(searchString) || String.IsNullOrEmpty(searchString)
                             || x.BonusRoleRefId.Contains(searchString) || String.IsNullOrEmpty(searchString));
            totalRecords = bonusRules.Count();
            switch (sort)
            {
                case "Title":
                    switch (sortdir)
                    {
                        case "DESC":
                            bonusRules = bonusRules
                                 .OrderByDescending(r => r.Title)
                                .Skip(index * pageSize)
                                .Take(pageSize);

                            break;
                        default:
                            bonusRules = bonusRules
                                 .OrderBy(r => r.BonusRuleId)
                                .Skip(index * pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;

                default:
                    bonusRules = bonusRules
                        .OrderByDescending(r => r.BonusRuleId)
                        .Skip(index * pageSize)
                        .Take(pageSize);
                    break;
            }
            return bonusRules.ToList();
        }

        public string GetNewBounusRuleRefId(string compId)
        {
            var maxRefId = bonusRuleRepository.Filter(x => x.CompId == compId).Max(x => x.BonusRoleRefId)??"0";
            return maxRefId.IncrementOne().PadZero(3);
        }
     

        public BonusRule GetBonusRuleById(int bonusRuleId, string compId)
        {
            return bonusRuleRepository.FindOne(x => x.CompId == compId && x.BonusRuleId == bonusRuleId);
        }
        public bool IsBounusRoleExist(BonusRule model)
        {
            return bonusRuleRepository.Exists( x => x.CompId == PortalContext.CurrentUser.CompId && x.BonusRuleId != model.BonusRuleId && x.Title == model.Title&&x.MoreThanOneYear==model.MoreThanOneYear&&x.MoreThanSixLessOneYear==model.MoreThanSixLessOneYear&&x.EffectiveDate==model.EffectiveDate);
        }
        public int EditBousRule(BonusRule model)
        {
            var bonusRule = bonusRuleRepository.FindOne(x => x.CompId == PortalContext.CurrentUser.CompId && x.BonusRuleId == model.BonusRuleId);
            bonusRule.BonusRoleRefId = bonusRule.BonusRoleRefId;
            bonusRule.Title = bonusRule.Title;
            bonusRule.Remarks = bonusRule.Remarks;
            bonusRule.EffectiveDate = bonusRule.EffectiveDate;
            bonusRule.MoreThanOneYear = bonusRule.MoreThanOneYear;
            bonusRule.MoreThanSixLessOneYear = bonusRule.MoreThanSixLessOneYear;
            bonusRule.EditedBy = PortalContext.CurrentUser.UserId;
            bonusRule.EditedDate = DateTime.Now;
            return bonusRuleRepository.Edit(bonusRule);
        }
        public int SaveBousBole(BonusRule model)
        {
            var compId = PortalContext.CurrentUser.CompId;
            model.BonusRoleRefId = GetNewBounusRuleRefId(compId);
            model.CompId = compId;
            model.IsProcessed = "N";
            model.CreatedBy = PortalContext.CurrentUser.UserId.GetValueOrDefault();
            model.CreatedDate = DateTime.Now;
            return bonusRuleRepository.Save(model);
        }

        public int DeleteBounusRole(BonusRule bonusRule)
        {
            return bonusRuleRepository.DeleteOne(bonusRule);
        }

        public List<SelectModel> GetUnPorcessBonusRule(int companyId)
        {
            var bonusRules = bonusRuleRepository.Filter(x => x.CompId == PortalContext.CurrentUser.CompId && x.IsProcessed == "N");
            return bonusRules.Select(x => new SelectModel() { Text = x.Title, Value = x.BonusRuleId }).ToList();
        }
    }
}
