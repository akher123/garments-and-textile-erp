using System;
using System.Runtime.Remoting.Contexts;
using SCERP.Common;
using SCERP.DAL.IRepository.IHRMRepository;
using SCERP.Model;
using System.Linq;
using System.Collections.Generic;

namespace SCERP.DAL.Repository.HRMRepository
{
    public class MaritalStatusRepository : Repository<MaritalState>, IMaritalStatusRepository
    {

        public MaritalStatusRepository(SCERPDBContext context)
            : base(context)
        {

        }

        public MaritalState GetMaritalStatusById(int maritalStateId)
        {
            return Context.MaritalStates.FirstOrDefault(x => x.MaritalStateId == maritalStateId);
        }

        public List<MaritalState> GetAllMaritalStatuses()
        {
            try
            {
                return Context.MaritalStates.Where(x => x.IsActive).OrderBy(x => x.Title).ToList();
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
            
        }

        public List<MaritalState> GetAllMaritalStatusesByPaging(int startPage, int pageSize, out int totalRecords, MaritalState maritalState)
        {
            IQueryable<MaritalState> maritalStatuses;

            try
            {
                string searchKey = maritalState.Title;
           
                maritalStatuses = Context.MaritalStates.Where(x => x.IsActive == true &&
                                                 ((x.Title.Replace(" ", "")
                                                     .ToLower()
                                                     .Contains(searchKey.Replace(" ", "").ToLower())) ||
                                                  String.IsNullOrEmpty(searchKey)));
                totalRecords = maritalStatuses.Count();
                switch (maritalState.sortdir)
                {
                    case "DESC":
                        maritalStatuses = maritalStatuses
                            .OrderByDescending(r => r.Title)
                            .Skip(startPage*pageSize)
                            .Take(pageSize);
                        break;

                    default:
                        maritalStatuses = maritalStatuses
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

            return maritalStatuses.ToList();
        }
    }
}
