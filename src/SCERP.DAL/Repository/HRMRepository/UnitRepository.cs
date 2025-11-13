using SCERP.DAL.IRepository.IHRMRepository;
using SCERP.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SCERP.DAL.Repository.HRMRepository
{
    public class UnitRepository :Repository<Unit>, IUnitRepository
    {
        public UnitRepository(SCERPDBContext context) : base(context)
        {
        }
        public List<Unit> GetAllUnits(int startPage, int pageSize, Unit unit, out int totalRecords)
        {
            IQueryable<Unit> units;
            try
            {
             
                units = Context.Units
                    .Where(x => x.IsActive == true && ((x.Name.Replace(" ", "")
                        .ToLower().Contains(unit.Name.Replace(" ", "")
                            .ToLower())) || String.IsNullOrEmpty(unit.Name)));
                totalRecords = units.Count();
                switch (unit.sortdir)
                {
                    case "DESC":
                        units = units
                            .OrderByDescending(r => r.Name)
                            .Skip(startPage*pageSize)
                            .Take(pageSize);
                        break;
                    default:
                        units = units
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

            return units.ToList();
        }       
    }
}
