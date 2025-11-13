using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using SCERP.DAL.IRepository.IHRMRepository;
using SCERP.Model;
using SCERP.Model.Custom;

namespace SCERP.DAL.Repository.HRMRepository
{   
    public class SkillOperationRepository : Repository<SkillOperation>, ISkillOperationRepository
    {
        public SkillOperationRepository(SCERPDBContext context) : base(context)
        {
        }

        public List<SkillOperation> GetAllSkillOperationByPaging(int startPage, int pageSize, out int totalRecords, SearchFieldModel searchFieldModel, SkillOperation model)
        {

            IQueryable<SkillOperation> skillOperations; 

            try
            {
                var searchBySkillSetDifficultyId = model.SkillSetDifficultyId;
                var searchBySkillSetCategoryId = model.CategoryId;


                var searchBySkillOperationName = model.Name;

                Expression<Func<SkillOperation, bool>> predicate = x => x.IsActive &&
                                                                        ((x.Name.Replace(" ", "")
                                                                            .ToLower()
                                                                            .Contains(
                                                                                searchBySkillOperationName.Replace(" ", "")
                                                                                    .ToLower())) ||
                                                                         String.IsNullOrEmpty(searchBySkillOperationName)) &&
                                                                        ((x.SkillSetDifficultyId ==
                                                                          searchBySkillSetDifficultyId ||
                                                                          searchBySkillSetDifficultyId == 0) &&
                                                                         (x.CategoryId ==
                                                                          searchBySkillSetCategoryId ||
                                                                          searchBySkillSetCategoryId == 0));

                skillOperations = Context.SkillOperations.Include(x=>x.SkillSetDifficulty).Include(x=>x.SkillSetCategory).Where(predicate);
                totalRecords = skillOperations.Count();
                switch (model.sort)
                {
                    case "SkillSetDifficulty.DifficultyName":

                        switch (model.sortdir)
                        {
                            case "DESC":
                                skillOperations = skillOperations
                                    .OrderByDescending(r => r.SkillSetDifficulty.DifficultyName).ThenBy(x => x.Name)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);

                                break;
                            default:
                                skillOperations = skillOperations
                                    .OrderBy(r => r.SkillSetDifficulty.DifficultyName).ThenBy(x => x.Name)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);

                                break;
                        }

                        break;

                    case "SkillSetCategory.CategoryName":

                        switch (model.sortdir)
                        {
                            case "DESC":
                                skillOperations = skillOperations
                                    .OrderByDescending(r => r.SkillSetCategory.CategoryName).ThenBy(x => x.Name)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);

                                break;
                            default:
                                skillOperations = skillOperations
                                    .OrderBy(r => r.SkillSetCategory.CategoryName).ThenBy(x => x.Name)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);

                                break;
                        }

                        break;

                    case "Name":

                        switch (model.sortdir)
                        {
                            case "DESC":
                                skillOperations = skillOperations
                                    .OrderByDescending(r => r.Name)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);

                                break;
                            default:
                                skillOperations = skillOperations
                                    .OrderBy(r => r.Name)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);

                                break;
                        }
                        break;
                    default:
                        skillOperations = skillOperations
                                      .OrderBy(r => r.SkillSetDifficulty.DifficultyName).ThenBy(x => x.SkillSetCategory.CategoryName).ThenBy(x => x.Name)
                                      .Skip(startPage * pageSize)
                                      .Take(pageSize);
                        break;
                }
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }

            return skillOperations.ToList();
        }
  
    }
}
