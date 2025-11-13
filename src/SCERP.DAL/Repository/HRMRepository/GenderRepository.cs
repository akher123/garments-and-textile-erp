using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Runtime.Remoting.Contexts;
using SCERP.Common;
using SCERP.DAL.IRepository.IHRMRepository;
using SCERP.Model;
using System.Linq;


namespace SCERP.DAL.Repository.HRMRepository
{
    public class GenderRepository : Repository<Gender>, IGenderRepository
    {
        public GenderRepository(SCERPDBContext context)
            : base(context)
        {
        }


        public List<Gender> GetAllGenders()
        {
            return Context.Genders.Where(x => x.IsActive == true).OrderBy(y => y.Title).ToList();
        }

        public List<Gender> GetGenders(int startPage, int pageSize, Gender gender, out int totalRecords)
        {
            IQueryable<Gender> genders;
            try
            {
                genders = Context.Genders
                    .Where(x => x.IsActive == true && ((x.Title.Replace(" ", "")
                        .ToLower().Contains(gender.Title.Replace(" ", "")
                            .ToLower())) || String.IsNullOrEmpty(gender.Title)));
                totalRecords = genders.Count();  
                switch (gender.sortdir)
                {
                    case "DESC":
                        genders = genders.OrderByDescending(r => r.Title)
                            .Skip(startPage*pageSize)
                            .Take(pageSize);   
                        break;
                    default:
                        genders = genders.OrderBy(r => r.Title)
                          .Skip(startPage * pageSize)
                          .Take(pageSize); 
                        break;
                }
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
            return genders.ToList();
        }
    }
}
