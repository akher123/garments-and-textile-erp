using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.BLL.IManager.ICommonManager;
using SCERP.Common;
using SCERP.DAL;
using SCERP.DAL.IRepository.ICommonRepository;
using SCERP.DAL.Repository.CommonRepository;
using SCERP.Model;

namespace SCERP.BLL.Manager.CommonManager
{
    public class MeasurementUnitManager : IMeasurementUnitManager
    {
        private readonly IMeasurementUnitRepository _measurementUnitRepository = null;
        public MeasurementUnitManager(SCERPDBContext context)
        {
            _measurementUnitRepository=new MeasurementUnitRepository(context);
        }

        public List<MeasurementUnit> GetMeasurementUnits()
        {
            return _measurementUnitRepository.Filter(x => x.IsActive).ToList();
        }

        public List<MeasurementUnit> GetMeasurementUnitByPaging(MeasurementUnit model, out int totalRecords)
        {
            var index = model.PageIndex;
            var pageSize = AppConfig.PageSize;
            IQueryable<MeasurementUnit> measurementUnits = _measurementUnitRepository.GetMeasurementUnitByPaging(model);
            totalRecords = measurementUnits.Count();
            switch (model.sort)
            {
                case "UnitName":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            measurementUnits = measurementUnits
                                .OrderByDescending(r => r.UnitName)
                                .Skip(index * pageSize)
                                .Take(pageSize);

                            break;
                        default:
                            measurementUnits = measurementUnits
                                .OrderBy(r => r.UnitName)
                                .Skip(index * pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;
                default:
                    measurementUnits = measurementUnits
                               .OrderByDescending(r => r.UnitName)
                               .Skip(index * pageSize)
                               .Take(pageSize);
                    break;
            }
            return measurementUnits.ToList();
        }

        public MeasurementUnit GetMeasurementUnitById(int unitId)
        {
            return _measurementUnitRepository.FindOne(x => x.IsActive && x.UnitId == unitId);
        }

        public int EditMeasurementUnit(MeasurementUnit model)
        {
            var editIndex = 0;
            try
            {
                var measurementUnits = _measurementUnitRepository.FindOne(x => x.UnitId == model.UnitId);
                measurementUnits.UnitName = model.UnitName;
                measurementUnits.Description = model.Description;
                measurementUnits.EditedBy = PortalContext.CurrentUser.UserId;
                measurementUnits.EditedDate = DateTime.Now;
                measurementUnits.IsActive = true;
                editIndex = _measurementUnitRepository.Edit(measurementUnits);
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return editIndex;
        }

        public int SaveMeasurementUnit(MeasurementUnit model)
        {
            var saveIndex = 0;
            try
            {
                model.CreatedBy = PortalContext.CurrentUser.UserId;
                model.CreatedDate = DateTime.Now;
                model.IsActive = true;
                saveIndex = _measurementUnitRepository.Save(model);
            }
            catch (Exception exception)
            {

                throw exception;
            }
            return saveIndex;
        }

        public int DeleteMeasurementUnit(int unitId)
        {
            var deleteIndex = 0;
            try
            {
                var inventoryBrand = _measurementUnitRepository.FindOne(x => x.UnitId == unitId);
                inventoryBrand.EditedBy = PortalContext.CurrentUser.UserId;
                inventoryBrand.EditedDate = DateTime.Now;
                inventoryBrand.IsActive = false;
                deleteIndex = _measurementUnitRepository.Edit(inventoryBrand);
            }
            catch (Exception exception)
            {

                throw exception;
            }
            return deleteIndex;
        }

        public bool IsExistMeasurementUnit(MeasurementUnit model)
        {
            return _measurementUnitRepository.Exists(x => x.UnitName.ToLower().Equals(model.UnitName.ToLower()) && x.UnitId != model.UnitId);
        }
    }
}
