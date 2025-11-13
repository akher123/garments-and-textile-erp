using SCERP.DAL.IRepository.IHRMRepository;
using SCERP.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SCERP.DAL.Repository.HRMRepository
{
    public class SectionRepository : Repository<Section>, ISectionRepository
    {
        public SectionRepository(SCERPDBContext context)
            : base(context)
        {

        }
        public List<Section> GetAllSections(int startPage, int pageSize, Section model, out int totalRecords)
        {
            IQueryable<Section> sections;
            try
            {
                

                sections = Context.Sections
                       .Where(x => x.IsActive == true && ((x.Name.Replace(" ", "")
                           .ToLower().Contains(model.Name.Replace(" ", "")
                           .ToLower())) || String.IsNullOrEmpty(model.Name))) ;
                totalRecords = sections.Count();
                switch (model.sortdir)
                {
                    case "DESC":
                        sections = sections
                            .OrderByDescending(r => r.Name)
                            .Skip(startPage*pageSize)
                            .Take(pageSize);
                        break;
                    default:
                        sections = sections
                            .OrderBy(r => r.Name)
                            .Skip(startPage*pageSize)
                            .Take(pageSize);
                        break;
                }              
            }            
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }

            return sections.ToList();
        }
    }
}
