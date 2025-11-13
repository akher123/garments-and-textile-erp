using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Contexts;

using SCERP.DAL.IRepository.IHRMRepository;
using System.Linq;
using SCERP.Model;

namespace SCERP.DAL.Repository.HRMRepository
{
    public class SkillSetRepository : Repository<SkillSet>, ISkillSetRepository
    {


        public SkillSetRepository(SCERPDBContext context)
            : base(context)
        {

        }

        public SkillSet GetSkillSetById(int? id)
        {
            return Context.SkillSets.First(x => x.Id == id);
        }

        public List<SkillSet> GetAllSkillSetsByPaging(int startPage, int pageSize, out int totalRecords, SkillSet skillSet)
        {
            List<SkillSet> skillsets = null;
            try
            {
                var searchByTitle = skillSet.Title;

                totalRecords = Context.SkillSets.Count(x => x.IsActive == true &&
                                                 ((x.Title.Replace(" ", "")
                                                     .ToLower()
                                                     .Contains(searchByTitle.Replace(" ", "").ToLower())) ||
                                                  String.IsNullOrEmpty(searchByTitle)));

                switch (skillSet.sortdir)
                {
                    case "DESC":
                        skillsets = Context.SkillSets.Where(
                          x =>
                              x.IsActive == true &&
                              ((x.Title.Replace(" ", "")
                                  .ToLower()
                                  .Contains(searchByTitle.Replace(" ", "").ToLower())) || String.IsNullOrEmpty(searchByTitle)))
                          .OrderByDescending(r => r.Title)
                          .Skip(startPage * pageSize)
                          .Take(pageSize)
                          .ToList();
                        break;


                    default:
                        skillsets = Context.SkillSets.Where(
                              x =>
                                  x.IsActive == true &&
                                  ((x.Title.Replace(" ", "")
                                      .ToLower()
                                      .Contains(searchByTitle.Replace(" ", "").ToLower())) || String.IsNullOrEmpty(searchByTitle)))
                              .OrderBy(r => r.Title)
                              .Skip(startPage * pageSize)
                              .Take(pageSize)
                              .ToList();
                        break;
                }
            }
            catch (Exception exception)
            {
                totalRecords = 0;
                throw new Exception(exception.Message);
            }

            return skillsets;
        }

        public bool SkillSetIsExist(SkillSet skillSet)
        {
            return Filter(x => x.Id != skillSet.Id).Any(x => x.Title.Replace(" ", "").ToLower().Equals(skillSet.Title.Replace(" ", "").ToLower()) && x.IsActive);
        }

        public override IQueryable<SkillSet> All()
        {
            return Context.SkillSets.Where(x => x.IsActive == true);
        }

    }
}
