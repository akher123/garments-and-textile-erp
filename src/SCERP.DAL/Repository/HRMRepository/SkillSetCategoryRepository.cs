using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.DAL.IRepository.IHRMRepository;
using SCERP.Model;

namespace SCERP.DAL.Repository.HRMRepository
{
    public class SkillSetCategoryRepository : Repository<SkillSetCategory>, ISkillSetCategoryRepository
    {
        public SkillSetCategoryRepository(SCERPDBContext context)
            : base(context)
        {
        }


        public SkillSetCategory GetSkillSetCategoryById(int? categoryId)
        {
            return Context.SkillSetCategories.FirstOrDefault(x => x.CategoryId == categoryId);
        }

        public List<SkillSetCategory> GetAllSkillSetCategory()
        {
            return Context.SkillSetCategories.Where(x => x.IsActive).OrderBy(x => x.CategoryName).ToList();
        }


        public List<SkillSetCategory> GetAllSkillSetCategoryByPaging(int startPage, int pageSize, out int totalRecords, SkillSetCategory skillsetCategory)
        {
            IQueryable<SkillSetCategory> skillSetCategories;

            try
            {
                string searchKey = skillsetCategory.CategoryName;
                skillSetCategories = Context.SkillSetCategories.Where(
                    x =>
                        x.IsActive == true &&
                        ((x.CategoryName.Replace(" ", "")
                            .ToLower()
                            .Contains(searchKey.Replace(" ", "").ToLower())) || String.IsNullOrEmpty(searchKey)));
                totalRecords = skillSetCategories.Count();

                switch (skillsetCategory.sort)
                {
                    case "CategoryName":
                        switch (skillsetCategory.sortdir)
                        {
                            case "DESC":
                                skillSetCategories = skillSetCategories
                                    .OrderByDescending(r => r.CategoryName)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);
                                break;
                            default:
                                skillSetCategories = skillSetCategories
                                   .OrderBy(r => r.CategoryName)
                                   .Skip(startPage * pageSize)
                                   .Take(pageSize);
                                break;
                        }
                        break;
                }

            }
            catch (Exception exception)
            {
                totalRecords = 0;
                throw new Exception(exception.Message);
            }

            return skillSetCategories.ToList();

        }

      
    }
}
