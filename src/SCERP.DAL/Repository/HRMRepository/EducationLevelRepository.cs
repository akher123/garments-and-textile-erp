using System;
using System.Runtime.Remoting.Contexts;
using SCERP.Common;
using SCERP.DAL.IRepository.IHRMRepository;
using SCERP.Model;
using System.Linq;
using System.Collections.Generic;

namespace SCERP.DAL.Repository.HRMRepository
{
    public class EducationLevelRepository : Repository<EducationLevel>, IEducationLevelRepository
    {

        public EducationLevelRepository(SCERPDBContext context)
            : base(context)
        {

        }

        public EducationLevel GetEducationLevelById(int? id)
        {
            return Context.EducationLevels.First(x => x.Id == id);
        }

        public override IQueryable<EducationLevel> All()
        {
            return Context.EducationLevels.Where(x => x.IsActive).OrderBy(x => x.Title);
        }

        public List<EducationLevel> GetAllEducationLevelsByPaging(int startPage, int pageSize, out int totalRecords, EducationLevel educationLevel)
        {
            IQueryable<EducationLevel> educationLevels;

            try
            {
                string searchKey = educationLevel.Title;
                educationLevels = Context.EducationLevels.Where(
                    x =>
                        x.IsActive &&
                        ((x.Title.Replace(" ", "")
                            .ToLower()
                            .Contains(searchKey.Replace(" ", "").ToLower())) || String.IsNullOrEmpty(searchKey)));
                totalRecords = educationLevels.Count();
                switch (educationLevel.sortdir)
                {
                    case "DESC":
                        educationLevels = educationLevels
                            .OrderByDescending(r => r.Title)
                            .Skip(startPage*pageSize)
                            .Take(pageSize);
                        break;
                    default:
                        educationLevels = educationLevels
                            .OrderBy(r => r.Title)
                            .Skip(startPage*pageSize)
                            .Take(pageSize);
                        break;
                }
            }
            catch (Exception exception)
            {
                totalRecords = 0;
                throw new Exception(exception.Message);
            }

            return educationLevels.ToList();
        }
    }
}
