using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.DAL.IRepository.ICommonRepository;
using SCERP.Model;

namespace SCERP.DAL.Repository.CommonRepository
{
    public class MeasurementUnitRepository : Repository<MeasurementUnit>, IMeasurementUnitRepository 
    {
        public MeasurementUnitRepository(SCERPDBContext context) : base(context)
        {
        }

        public IQueryable<MeasurementUnit> GetMeasurementUnitByPaging(MeasurementUnit model)
        {
            return
             Context.MeasurementUnits.Where(
                 x => x.IsActive && (x.UnitName.ToLower().Contains(model.UnitName) || String.IsNullOrEmpty(model.UnitName)));
        }
    }
}
