using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using SCERP.DAL.IRepository.IHRMRepository;
using SCERP.Model;
using SCERP.Model.Custom;
using System.Data.Entity;

namespace SCERP.DAL.Repository.HRMRepository
{
    public class EfficiencyRateRepository : Repository<EfficiencyRate>, IEfficiencyRateRepository
    {
        public EfficiencyRateRepository(SCERPDBContext context)
            : base(context)
        {

        }

        public List<EfficiencyRate> GetAllEfficiencyByPaging(int startPage, int pageSize, out int totalRecords, SearchFieldModel searchFieldModel, EfficiencyRate model)
        {
            IQueryable<EfficiencyRate> efficiencyRates;
            try
            {
                var searchBySkillOperationId = model.SkillOperationId;
                var searchByEfficiencyRate = model.Rate;

                Expression<Func<EfficiencyRate, bool>> predicate = x => x.IsActive &&
                                                                        ((x.Rate.Replace(" ", "")
                                                                            .ToLower()
                                                                            .Contains(
                                                                                searchByEfficiencyRate.Replace(" ", "")
                                                                                    .ToLower())) ||
                                                                         String.IsNullOrEmpty(searchByEfficiencyRate)) &&
                                                                        ((x.SkillOperationId ==
                                                                          searchBySkillOperationId ||
                                                                          searchBySkillOperationId == 0));

                efficiencyRates = Context.EfficiencyRates.Include(x => x.SkillOperation).Where(predicate);
                totalRecords = efficiencyRates.Count();
                switch (model.sort)
                {
                    case "SkillOperation.Name":

                        switch (model.sortdir)
                        {
                            case "DESC":
                                efficiencyRates = efficiencyRates
                                    .OrderByDescending(r => r.SkillOperation.Name).ThenBy(x => x.Rate)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);

                                break;
                            default:
                                efficiencyRates = efficiencyRates
                                    .OrderBy(r => r.SkillOperation.Name).ThenBy(x => x.Rate)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);

                                break;
                        }

                        break;

                    case "Rate":

                        switch (model.sortdir)
                        {
                            case "DESC":
                                efficiencyRates = efficiencyRates
                                    .OrderByDescending(r => r.Rate)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);

                                break;
                            default:
                                efficiencyRates = efficiencyRates
                                    .OrderBy(r => r.Rate)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);

                                break;
                        }
                        break;
                    default:
                        efficiencyRates = efficiencyRates
                                      .OrderBy(r => r.SkillOperation.Name).ThenBy(x => x.Rate)
                                      .Skip(startPage * pageSize)
                                      .Take(pageSize);
                        break;
                }
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }

            return efficiencyRates.ToList();
        }



    }
}

