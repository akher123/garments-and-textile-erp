using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Common;
using SCERP.DAL.IRepository.IHRMRepository;
using SCERP.Model;

namespace SCERP.DAL.Repository.HRMRepository
{
   public class SkillSetDifficultyRepository : Repository<SkillSetDifficulty>, ISkillSetDifficultyRepository
    {
        public SkillSetDifficultyRepository(SCERPDBContext context) : base(context)
        {
        }


        public SkillSetDifficulty GetSkillSetDifficultyById(int? skillSetDifficultyId)
        {
            return Context.SkillSetDifficulties.FirstOrDefault(x => x.SkillSetDifficultyId == skillSetDifficultyId);
        }

       public List<SkillSetDifficulty> GetAllSkillSetDifficulty()
       {
           return Context.SkillSetDifficulties.Where(x => x.IsActive).OrderBy(x => x.DifficultyName).ToList();
       }



       public List<SkillSetDifficulty> GetAllEmployeeSkillBySearchKey(string searchKey)
       {
           List<SkillSetDifficulty> difficulties = null;

           try
           {
               difficulties = Context.SkillSetDifficulties.Where(
                   x =>
                       x.IsActive == true &&
                       ((x.DifficultyName.Replace(" ", "")
                           .ToLower()
                           .Contains(searchKey.Replace(" ", "").ToLower())) || String.IsNullOrEmpty(searchKey))).ToList();
           }
           catch (Exception exception)
           {
               Errorlog.WriteLog(exception);
           }
           return difficulties; 
           
       }









       public List<SkillSetDifficulty> GetAllSkillSetDifficultyByPaging(int startPage, int pageSize, out int totalRecords,SkillSetDifficulty skillSetDifficulty)
       {

           IQueryable<SkillSetDifficulty> skillSetDifficulties;

           try
           {
               string searchKey = skillSetDifficulty.DifficultyName;
               skillSetDifficulties = Context.SkillSetDifficulties.Where(
                   x =>
                       x.IsActive == true &&
                       ((x.DifficultyName.Replace(" ", "")
                           .ToLower()
                           .Contains(searchKey.Replace(" ", "").ToLower())) || String.IsNullOrEmpty(searchKey)));
               totalRecords = skillSetDifficulties.Count();

               switch (skillSetDifficulty.sort)
               {
                   case "DifficultyName":
                       switch (skillSetDifficulty.sortdir)
                       {
                           case "DESC":
                               skillSetDifficulties = skillSetDifficulties
                                   .OrderByDescending(r => r.DifficultyName)
                                   .Skip(startPage * pageSize)
                                   .Take(pageSize);
                               break;
                           default:
                               skillSetDifficulties = skillSetDifficulties
                                  .OrderBy(r => r.DifficultyName)
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
           return skillSetDifficulties.ToList();
           
       }

      
    }
}
