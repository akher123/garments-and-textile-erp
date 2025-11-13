using SCERP.BLL.IManager.IHRMManager;
using SCERP.Common;
using SCERP.DAL;
using SCERP.DAL.IRepository.IHRMRepository;
using SCERP.DAL.Repository.HRMRepository;
using SCERP.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SCERP.BLL.Manager.HRMManager
{
    public class UnitManager : IUnitManager
    {
        private readonly IUnitRepository _unitRepository = null;
        private readonly IBranchUnitRepository _branchUnitRepository = null;

        public UnitManager(SCERPDBContext context)
        {
            _unitRepository = new UnitRepository(context);
            _branchUnitRepository = new BranchUnitRepository(context);
        }

        public List<Unit> GetAllUnits(int startPage, int pageSize, Unit unit, out int totalRecords)
        {
            List<Unit> units;
            try
            {
                units = _unitRepository.GetAllUnits(startPage, pageSize, unit, out totalRecords);
            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message);

            }
            return units;
        }

        public Unit GetUnitById(int unitId)
        {
            Unit unit;
            try
            {
                unit =
                    _unitRepository.FindOne(x => x.IsActive && x.UnitId == unitId);
            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message);
            }
            return unit;
        }
        public bool IsExistUnit(Unit model)
        {
            bool isExist;
            try
            {
                isExist = _unitRepository.Exists(x => x.IsActive == true
                    && x.UnitId != model.UnitId
                     && (x.Name.Replace("", " ").ToLower() == model.Name.Replace("", " ").ToLower()));
            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message);
            }
            return isExist;
        }

        public int EditUnit(Unit model)
        {
            var editIndex = 0;
            try
            {
                var unitObj = _unitRepository.FindOne(x => x.IsActive && x.UnitId == model.UnitId);
                unitObj.Name = model.Name;
                unitObj.NameInBengali = model.NameInBengali;
                unitObj.Description = model.Description;
                unitObj.EditedDate = DateTime.Now;
                unitObj.EditedBy = PortalContext.CurrentUser.UserId;
                editIndex = _unitRepository.Edit(unitObj);

            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message);
            }
            return editIndex;
        }

        public int SaveUnit(Unit model)
        {
            var saveIndex = 0;
            try
            {
                model.CreatedBy = PortalContext.CurrentUser.UserId;
                model.CreatedDate = DateTime.Now;
                model.IsActive = true;
                saveIndex = _unitRepository.Save(model);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
            return saveIndex;
        }

        public int DeleteUnit(int unitId)
        {

            var deleteIndex = 0;
            try
            {
                var unitObj = _unitRepository.FindOne(x => x.IsActive == true && x.UnitId == unitId);
                unitObj.IsActive = false;
                unitObj.EditedDate = DateTime.Now;
                unitObj.EditedBy = PortalContext.CurrentUser.UserId;
                deleteIndex = _unitRepository.Edit(unitObj);
            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message);
            }
            return deleteIndex;
        }

        public List<Unit> GetAllUnitsBySearchKey(Unit model)
        {
            List<Unit> units;
            try
            {
                units = _unitRepository.Filter(x => x.IsActive && ((x.Name.Replace(" ", "")
                        .ToLower().Contains(model.Name.Replace(" ", "")
                            .ToLower())) || String.IsNullOrEmpty(model.Name))).ToList();
            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message);
            }
            return units;
        }

        public List<Unit> GetAllUnits()
        {
            List<Unit> units = null;
            try
            {
                units = _unitRepository.Filter(x => x.IsActive).OrderBy(x => x.Name).ToList();
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return units;
        }

     
        public IEnumerable GetAllUnitsByCompanyId(int companyId)
        {
            IEnumerable units;
            try
            {              
                units = _branchUnitRepository.GetAllUnitsByCompanyId(companyId);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
            return units;
        }
    }
}
