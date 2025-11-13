using System.Collections;
using System.Data.Entity;
using SCERP.BLL.IManager.IHRMManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Common;
using SCERP.DAL;
using SCERP.DAL.IRepository.IHRMRepository;
using SCERP.DAL.Repository.HRMRepository;
using SCERP.Model;
using SCERP.Model.Custom;

namespace SCERP.BLL.Manager.HRMManager
{
    public class HeadOfDepartmentManager : IHeadOfDepartmentManager
    {
        private readonly IBranchUnitDepartmentRepository _branchUnitDepartmentRepository;
        public HeadOfDepartmentManager(SCERPDBContext context)
        {
            _branchUnitDepartmentRepository = new BranchUnitDepartmentRepository(context);
        }



        //public List<BranchUnitDepartment> GetAllBranchUnitDepartment(int startPage, int pageSize, out int totalRecords, SearchFieldModel searchFieldModel,
        //    BranchUnitDepartment model)
        //{
        //    List<BranchUnitDepartment> branchUnitDepartments;
        //    try
        //    {
        //        branchUnitDepartments = _branchUnitDepartmentRepository.GetAllBranchUnitDepartment(startPage, pageSize, out totalRecords, searchFieldModel, model);
        //    }
        //    catch (Exception exception)
        //    {

        //        throw new Exception(exception.Message);
        //    }
        //    return branchUnitDepartments;
        //}

        //public BranchUnitDepartment GetBranchUnitDepartmentById(int branchUnitDepartmentId)
        //{
        //    BranchUnitDepartment branchUnitDepartment;
        //    try
        //    {

        //        branchUnitDepartment = _branchUnitDepartmentRepository.GetBranchUnitDepartmentById(branchUnitDepartmentId);
        //    }
        //    catch (Exception exception)
        //    {

        //        throw new Exception(exception.Message);
        //    }
        //    return branchUnitDepartment;
        //}

        //public int EditranchUnitDepartment(BranchUnitDepartment model)
        //{
        //    var editIndex = 0;
        //    try
        //    {
        //        var branchUnitDepartmenttObj = _branchUnitDepartmentRepository.FindOne(x => x.IsActive && x.BranchUnitDepartmentId == model.BranchUnitDepartmentId);
        //        branchUnitDepartmenttObj.BranchUnitId = model.BranchUnitId;
        //        branchUnitDepartmenttObj.UnitDepartmentId = model.UnitDepartmentId;
        //        branchUnitDepartmenttObj.EditedDate = DateTime.Now;
        //        branchUnitDepartmenttObj.EditedBy = PortalContext.CurrentUser.UserId;
        //        editIndex = _branchUnitDepartmentRepository.Edit(branchUnitDepartmenttObj);

        //    }
        //    catch (Exception exception)
        //    {

        //        throw new Exception(exception.Message);
        //    }
        //    return editIndex;
        //}

        //public int SaveBranchUnitDepartment(BranchUnitDepartment model)
        //{
        //    var saveIndex = 0;
        //    try
        //    {
        //        model.CreatedBy = PortalContext.CurrentUser.UserId;
        //        model.CreatedDate = DateTime.Now;
        //        model.IsActive = true;
        //        saveIndex = _branchUnitDepartmentRepository.Save(model);
        //    }
        //    catch (Exception exception)
        //    {
        //        throw new Exception(exception.Message);
        //    }
        //    return saveIndex;
        //}

        //public bool IsExistBranchUnitDepartment(BranchUnitDepartment model)
        //{
        //    bool isExist;
        //    try
        //    {
        //        isExist = _branchUnitDepartmentRepository.Exists(x => x.IsActive && x.BranchUnitDepartmentId != model.BranchUnitDepartmentId
        //            && x.UnitDepartmentId == model.UnitDepartmentId && x.BranchUnitId == model.BranchUnitId);
        //    }
        //    catch (Exception exception)
        //    {

        //        throw new Exception(exception.Message);
        //    }
        //    return isExist;
        //}

        //public int BranchUnitDepartment(int branchUnitDepatmentId)
        //{
        //    var deleteIndex = 0;
        //    try
        //    {
        //        var brUnitDeptObj = _branchUnitDepartmentRepository.FindOne(x => x.IsActive == true && x.BranchUnitDepartmentId == branchUnitDepatmentId);
        //        brUnitDeptObj.IsActive = false;
        //        brUnitDeptObj.EditedDate = DateTime.Now;
        //        brUnitDeptObj.EditedBy = PortalContext.CurrentUser.UserId;
        //        deleteIndex = _branchUnitDepartmentRepository.Edit(brUnitDeptObj);
        //    }
        //    catch (Exception exception)
        //    {

        //        throw new Exception(exception.Message);
        //    }
        //    return deleteIndex;
        //}

        //public List<BranchUnitDepartment> GetBranchUnitDepartmentBySearchKey(SearchFieldModel searchFieldModel)
        //{
        //    List<BranchUnitDepartment> branchUnitDepartments;
        //    try
        //    {
        //        branchUnitDepartments = _branchUnitDepartmentRepository.GetBranchUnitDepartmentBySearchKey(searchFieldModel);
        //    }
        //    catch (Exception exception)
        //    {

        //        throw new Exception(exception.Message);
        //    }
        //    return branchUnitDepartments;
        //}

        //public List<Unit> GetUnitsByBranchId(int searchByBranchId)
        //{
        //    List<Unit> units;
        //    try
        //    {
        //        units = _branchUnitDepartmentRepository.GetUnitsByBranchId(searchByBranchId);
        //    }
        //    catch (Exception exception)
        //    {

        //        throw new Exception(exception.Message);

        //    }
        //    return units;
        //}

        //public List<Department> GetDepartmentsByBranchId(int searchByUnitId)
        //{
        //    List<Department> departments = null;
        //    try
        //    {
        //        departments = _branchUnitDepartmentRepository.GetDepartmentsByBranchId(searchByUnitId);
        //    }
        //    catch (Exception exception)
        //    {
        //        Errorlog.WriteLog(exception);

        //    }

        //    return departments;
        //}

        //public IEnumerable GetBranchUnitDepartmentsByBranchUnitId(int branchUnitId)
        //{
        //    IEnumerable branchUnitDepartments;
        //    try
        //    {
        //        branchUnitDepartments =
        //          _branchUnitDepartmentRepository.Filter(x => x.IsActive && x.BranchUnitId == branchUnitId).Include(x => x.UnitDepartment.Department).Select(x => new
        //          {
        //              BranchUnitDepartmentId = x.BranchUnitDepartmentId,
        //              DepartmentName = x.UnitDepartment.Department.Name
        //          }).OrderBy(x=>x.DepartmentName).ToList();

        //    }
        //    catch (Exception exception)
        //    {

        //        throw new Exception(exception.Message);
        //    }
        //    return branchUnitDepartments;
        //}


        //public IEnumerable GetAllPermittedBranchUnitDepartmentsByBranchUnitId(int branchUnitId)
        //{
        //    IEnumerable branchUnitDepartments;
        //    try
        //    {
        //        branchUnitDepartments = PortalContext.CurrentUser.PermissionContext.DepartmentList.Where(x => x.BranchUnitId == branchUnitId).ToList(); ;
        //    }
        //    catch (Exception exception)
        //    {
        //        throw new Exception(exception.Message);
        //    }
        //    return branchUnitDepartments;
        //}
    }
}
