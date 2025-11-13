using System;
using System.Runtime.Remoting.Contexts;
using SCERP.Common;
using SCERP.DAL.IRepository.IHRMRepository;
using SCERP.Model;
using System.Linq;
using System.Collections.Generic;

namespace SCERP.DAL.Repository.HRMRepository
{
    public class ReligionRepository : Repository<Religion>, IReligionRepository
    {

        public ReligionRepository(SCERPDBContext context)
            : base(context)
        {

        }

        public Religion GetReligionById(int religionId)
        {
            return Context.Religions.FirstOrDefault(x => x.ReligionId == religionId);
        }

        public List<Religion> GetAllReligions()
        {
            return Context.Religions.Where(x => x.IsActive == true).OrderBy(x=>x.Name).ToList();
        }

        public List<Religion> GetAllReligionsByPaging(int startPage, int pageSize, out int totalRecords, Religion religion)
        {
            IQueryable<Religion> religions;

            try
            {
                string searchKey = religion.Name;


                religions = Context.Religions.Where(
                    x =>
                        x.IsActive &&
                        ((x.Name.Replace(" ", "")
                            .ToLower()
                            .Contains(searchKey.Replace(" ", "").ToLower())) || String.IsNullOrEmpty(searchKey)));

                totalRecords = religions.Count();

                switch (religion.sortdir)
                {
                    case "DESC":
                        religions = religions
                            .OrderBy(r => r.Name)
                            .Skip(startPage*pageSize)
                            .Take(pageSize);
                        break;

                    default:
                        religions = religions
                            .OrderByDescending(r => r.Name)
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

            return religions.ToList();
        }
    }
}
