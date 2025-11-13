using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.BLL.IManager.IHRMManager;
using SCERP.Common;
using SCERP.DAL;
using SCERP.DAL.IRepository.IHRMRepository;
using SCERP.DAL.Repository.HRMRepository;
using SCERP.Model;
using SCERP.Model.Custom;

namespace SCERP.BLL.Manager.HRMManager
{
    public class BranchUnitManager : IBranchUnitManager
    {
        private readonly IBranchUnitRepository _branchUnitRepository = null;
        public BranchUnitManager(SCERPDBContext context)
        {
            _branchUnitRepository = new BranchUnitRepository(context);
        }
        public List<BranchUnit> GetAllBranchUnit(int startPage, int pageSize, out int totalRecords,
            SearchFieldModel searchFieldModel, BranchUnit model)
        {
            List<BranchUnit> branchUnits;
            try
            {
                branchUnits = _branchUnitRepository.GetAllBranchUnit(startPage, pageSize, out totalRecords, searchFieldModel, model);
            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message);
            }
            return branchUnits;
        }

        public BranchUnit GetBranchUnitById(int branchUnitId)
        {
            BranchUnit branchUnit;
            try
            {
                branchUnit =
                    _branchUnitRepository.FindOne(x => x.IsActive && x.BranchUnitId == branchUnitId);
            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message);
            }
            return branchUnit;
        }

        public bool IsExistBranchUnit(BranchUnit model)
        {
            bool isExist;
            try
            {
                isExist = _branchUnitRepository.Exists(x => x.IsActive == true && x.BranchUnitId != model.BranchUnitId
                    && x.UnitId == model.UnitId && x.BranchId == model.BranchId);
            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message);
            }
            return isExist;
        }

        public int EditBranchUnit(BranchUnit model)
        {
            var editIndex = 0;
            try
            {
                var branchUnitObj = _branchUnitRepository.FindOne(x => x.IsActive && x.BranchUnitId == model.BranchUnitId);
                branchUnitObj.BranchUnitId = model.BranchUnitId;
                branchUnitObj.BranchId = model.BranchId;
                branchUnitObj.UnitId = model.UnitId;
                branchUnitObj.EditedDate = DateTime.Now;
                branchUnitObj.EditedBy = PortalContext.CurrentUser.UserId;
                editIndex = _branchUnitRepository.Edit(branchUnitObj);

            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message);
            }
            return editIndex;
        }

        public int SaveBranchUnit(BranchUnit model)
        {
            var saveIndex = 0;
            try
            {

                model.CreatedBy = PortalContext.CurrentUser.UserId;
                model.CreatedDate = DateTime.Now;
                model.IsActive = true;
                saveIndex = _branchUnitRepository.Save(model);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
            return saveIndex;
        }

        public int DeleteBranchUnit(int branchUnitId)
        {
            var deleteIndex = 0;
            try
            {
                var branchUnitObj = _branchUnitRepository.FindOne(x => x.IsActive == true && x.BranchUnitId == branchUnitId);
                branchUnitObj.IsActive = false;
                branchUnitObj.EditedDate = DateTime.Now;
                branchUnitObj.EditedBy = PortalContext.CurrentUser.UserId;
                deleteIndex = _branchUnitRepository.Edit(branchUnitObj);
            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message);
            }
            return deleteIndex;
        }

        public List<BranchUnit> GetAllBranchUnitBySearchKey(SearchFieldModel searchFieldModel)
        {
            List<BranchUnit> branchUnits;
            try
            {
                branchUnits = _branchUnitRepository.Filter(
                                     x =>
                                         x.IsActive && (x.UnitId == searchFieldModel.SearchByUnitId || searchFieldModel.SearchByUnitId == 0) &&
                                         (x.BranchId == searchFieldModel.SearchByBranchId || searchFieldModel.SearchByBranchId == 0)
                                         && (x.Branch.CompanyId == searchFieldModel.SearchByCompanyId || searchFieldModel.SearchByCompanyId == 0))
                                         .Include(x => x.Branch)
                                         .Include(x => x.Branch.Company)
                                         .Include(x => x.Unit)
                                         .OrderBy(r => r.Unit.Name)
                                         .ToList();
            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message);
            }
            return branchUnits;
        }

        public IEnumerable GetBranchUnitByBranchId(int searchByBranchId)
        {
            IEnumerable branchUnits;
            try
            {
                branchUnits =
                  _branchUnitRepository.Filter(x => x.IsActive && x.BranchId == searchByBranchId).Include(x => x.Unit).Select(x => new
                    {
                        BranchUnitId = x.BranchUnitId,
                        UnitName = x.Unit.Name
                    }).OrderBy(x => x.UnitName).ToList();
            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message);
            }
            return branchUnits;
        }

        public IEnumerable GetAllPermittedBranchUnitsByBranchId(int branchId) //From portal context
        {

            List<Common.PermissionModel.UserUnit> units;
            try
            {
                units = PortalContext.CurrentUser.PermissionContext.UnitList.Where(x => x.BranchId == branchId).ToList();
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }

            return units;

        }

        public List<Common.PermissionModel.UserUnit> GetAllPermittedBranchUnitIdByBranchId(int branchId) 
        {

            List<Common.PermissionModel.UserUnit> units;
            try
            {
                units = PortalContext.CurrentUser.PermissionContext.UnitList.Where(x => x.BranchId == branchId).ToList();
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }

            return units;

        }
    }
}
