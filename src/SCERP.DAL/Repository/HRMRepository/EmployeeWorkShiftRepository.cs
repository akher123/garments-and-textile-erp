using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.Security.Cryptography.X509Certificates;
using SCERP.Common;
using SCERP.DAL.IRepository.IHRMRepository;
using SCERP.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using SCERP.Model.Custom;

namespace SCERP.DAL.Repository.HRMRepository
{
    public class EmployeeWorkShiftRepository : Repository<EmployeeWorkShift>, IEmployeeWorkShiftRepository
    {
        public EmployeeWorkShiftRepository(SCERPDBContext context)
            : base(context)
        {
        }


        public List<VEmployeeWorkShiftDetail> GetAllAssignedEmployeeWorkShift(int startPage, int pageSize, out int totalRecords, EmployeeWorkShift model,
            SearchFieldModel searchFieldModel)
        {
            IQueryable<VEmployeeWorkShiftDetail> employeeWorkShiftDetails ;
            totalRecords = 0;
            try
            {
                

                IEnumerable<int> companyIdList = PortalContext.CurrentUser.PermissionContext.CompanyList.Select(x => x.CompanyId);
                IEnumerable<int> branchIdList = PortalContext.CurrentUser.PermissionContext.BranchList.Select(x => x.BranchId);
                IEnumerable<int> branchUnitIdList = PortalContext.CurrentUser.PermissionContext.UnitList.Select(x => x.BranchUnitId);
                IEnumerable<int> branchUnitDepartmentIdList = PortalContext.CurrentUser.PermissionContext.DepartmentList.Select(x => x.BranchUnitDepartmentId);
                IEnumerable<int> employeeTypeList = PortalContext.CurrentUser.PermissionContext.EmployeeTypeList.Select(x => x.Id);


                employeeWorkShiftDetails =
                                      Context.VEmployeeWorkShiftDetails.Where(x => companyIdList.Contains(x.CompanyId) &&
                                      branchIdList.Contains(x.BranchId) &&
                                      branchUnitIdList.Contains(x.BranchUnitId) &&
                                      branchUnitDepartmentIdList.Contains(x.BranchUnitDepartmentId) &&
                                      employeeTypeList.Contains(x.EmployeeTypeId) &&
                                      (x.EmployeeCardId == searchFieldModel.SearchByEmployeeCardId || searchFieldModel.SearchByEmployeeCardId == null) &&
                                      (x.DepartmentSectionId == searchFieldModel.SearchByDepartmentSectionId || searchFieldModel.SearchByDepartmentSectionId == 0) &&
                                      (x.DepartmentLineId == searchFieldModel.SearchByDepartmentLineId || searchFieldModel.SearchByDepartmentLineId == 0) &&
                                      ((x.ShiftDate >= searchFieldModel.StartDate || searchFieldModel.StartDate == null) && 
                                      (x.ShiftDate <= searchFieldModel.EndDate || searchFieldModel.EndDate == null)));
                                    

                if (searchFieldModel.SearchByCompanyId > 0 || searchFieldModel.SearchByBranchId > 0 || searchFieldModel.SearchByBranchUnitId > 0 || searchFieldModel.SearchByBranchUnitDepartmentId > 0 || searchFieldModel.SearchByBranchUnitWorkShiftId > 0)
                {
                    employeeWorkShiftDetails =
                        employeeWorkShiftDetails.Where(x => (x.CompanyId == searchFieldModel.SearchByCompanyId || searchFieldModel.SearchByCompanyId == 0)
                        && (x.BranchId == searchFieldModel.SearchByBranchId || searchFieldModel.SearchByBranchId == 0)
                        && (x.BranchUnitId == searchFieldModel.SearchByBranchUnitId || searchFieldModel.SearchByBranchUnitId == 0)
                        && (x.BranchUnitDepartmentId == searchFieldModel.SearchByBranchUnitDepartmentId || searchFieldModel.SearchByBranchUnitDepartmentId == 0)
                        && (x.BranchUnitWorkShiftId == searchFieldModel.SearchByBranchUnitWorkShiftId || searchFieldModel.SearchByBranchUnitWorkShiftId == 0));
                }

                totalRecords = employeeWorkShiftDetails.Count();
                switch (model.sort)
                {
                    case "CompanyName":
                        switch (model.sortdir)
                        {
                            case "DESC":
                                employeeWorkShiftDetails = employeeWorkShiftDetails
                                    .OrderByDescending(r => r.CompanyName)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);
                                break;
                            default:
                                employeeWorkShiftDetails = employeeWorkShiftDetails
                                     .OrderBy(r => r.CompanyName)
                                     .Skip(startPage * pageSize)
                                     .Take(pageSize);
                                break;
                        }
                        break;
                    case "WorkShiftName":
                        switch (model.sortdir)
                        {
                            case "DESC":
                                employeeWorkShiftDetails = employeeWorkShiftDetails
                                    .OrderByDescending(r => r.WorkShiftName)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);
                                break;
                            default:
                                employeeWorkShiftDetails = employeeWorkShiftDetails
                                     .OrderBy(r => r.WorkShiftName)
                                     .Skip(startPage * pageSize)
                                     .Take(pageSize);
                                break;
                        }
                        break;

                    case "BranchName":
                        switch (model.sortdir)
                        {
                            case "DESC":
                                employeeWorkShiftDetails = employeeWorkShiftDetails
                                      .OrderByDescending(r => r.BranchName)
                                      .Skip(startPage * pageSize)
                                      .Take(pageSize);
                                break;
                            default:
                                employeeWorkShiftDetails = employeeWorkShiftDetails
                                     .OrderBy(r => r.BranchName)
                                     .Skip(startPage * pageSize)
                                     .Take(pageSize);
                                break;
                        }
                        break;
                    case "UnitName":
                        switch (model.sortdir)
                        {
                            case "DESC":
                                employeeWorkShiftDetails = employeeWorkShiftDetails
                                      .OrderByDescending(r => r.UnitName)
                                      .Skip(startPage * pageSize)
                                      .Take(pageSize);
                                break;
                            default:
                                employeeWorkShiftDetails = employeeWorkShiftDetails
                                      .OrderBy(r => r.UnitName)
                                      .Skip(startPage * pageSize)
                                      .Take(pageSize);
                                break;
                        }
                        break;
                    case "DepartmentName":
                        switch (model.sortdir)
                        {
                            case "DESC":
                                employeeWorkShiftDetails = employeeWorkShiftDetails
                                 .OrderByDescending(r => r.DepartmentName)
                                 .Skip(startPage * pageSize)
                                 .Take(pageSize);
                                break;
                            default:
                                employeeWorkShiftDetails = employeeWorkShiftDetails
                                     .OrderBy(r => r.DepartmentName)
                                     .Skip(startPage * pageSize)
                                     .Take(pageSize);
                                break;
                        }
                        break;
                    case "EmployeeName":
                        switch (model.sortdir)
                        {
                            case "DESC":
                                employeeWorkShiftDetails = employeeWorkShiftDetails
                                 .OrderByDescending(r => r.EmployeeName)
                                 .Skip(startPage * pageSize)
                                 .Take(pageSize);
                                break;
                            default:
                                employeeWorkShiftDetails = employeeWorkShiftDetails
                                     .OrderBy(r => r.EmployeeName)
                                     .Skip(startPage * pageSize)
                                     .Take(pageSize);
                                break;
                        }
                        break;
                    case "SectionName":
                        switch (model.sortdir)
                        {
                            case "DESC":
                                employeeWorkShiftDetails = employeeWorkShiftDetails
                                 .OrderByDescending(r => r.SectionName)
                                 .Skip(startPage * pageSize)
                                 .Take(pageSize);
                                break;
                            default:
                                employeeWorkShiftDetails = employeeWorkShiftDetails
                                     .OrderBy(r => r.EmployeeCardId)
                                     .Skip(startPage * pageSize)
                                     .Take(pageSize);
                                break;
                        }
                        break;
                    case "LneName":
                        switch (model.sortdir)
                        {
                            case "DESC":
                                employeeWorkShiftDetails = employeeWorkShiftDetails
                                 .OrderByDescending(r => r.LineName)
                                 .Skip(startPage * pageSize)
                                 .Take(pageSize);
                                break;
                            default:
                                employeeWorkShiftDetails = employeeWorkShiftDetails
                                     .OrderBy(r => r.LineName)
                                     .Skip(startPage * pageSize)
                                     .Take(pageSize);
                                break;
                        }
                        break;
                    case "ShiftDate":
                        switch (model.sortdir)
                        {
                            case "DESC":
                                employeeWorkShiftDetails = employeeWorkShiftDetails
                                 .OrderByDescending(r => r.ShiftDate)
                                 .Skip(startPage * pageSize)
                                 .Take(pageSize);
                                break;
                            default:
                                employeeWorkShiftDetails = employeeWorkShiftDetails
                                     .OrderBy(r => r.ShiftDate)
                                     .Skip(startPage * pageSize)
                                     .Take(pageSize);
                                break;
                        }
                        break;
                    default:
                        switch (model.sortdir)
                        {
                            case "DESC":
                                employeeWorkShiftDetails = employeeWorkShiftDetails
                                  .OrderByDescending(r => r.EmployeeCardId)
                                  .Skip(startPage * pageSize)
                                  .Take(pageSize);
                                break;
                            default:
                                employeeWorkShiftDetails = employeeWorkShiftDetails
                                 .OrderBy(r => r.EmployeeCardId)
                                 .Skip(startPage * pageSize)
                                 .Take(pageSize);
                                break;
                        }

                        break;
                }
            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message);
            }

            return employeeWorkShiftDetails.ToList();

        }

        public int SaveEmployeeWorkShifts(List<EmployeeWorkShift> employeeWorkShifts)
        {
            var saveChanges = 0;
            try
            {
                Context.EmployeeWorkShifts.AddRange(employeeWorkShifts);
                 saveChanges = Context.SaveChanges();
            }
            catch (Exception)
            {
                
                throw;
            }
            return saveChanges;
        }

        public EmployeeWorkShift GetEmployeeWorkshiftById(int employeeWorkShiftId)
        {
            EmployeeWorkShift employeeWorkShift;
            try
            {
              employeeWorkShift=  Context.EmployeeWorkShifts.Include(x => x.Employee)
                    .Include(x => x.BranchUnitWorkShift.BranchUnit.Branch.Company).Include(x=>x.BranchUnitWorkShift.BranchUnit.Unit)
                    .SingleOrDefault(x => x.EmployeeWorkShiftId == employeeWorkShiftId && x.Status && x.IsActive);
            }
            catch (Exception)
            {

                throw;
            }
            return employeeWorkShift;
        }

        public VEmployeeWorkShiftDetail GetEmployeeWorkshiftDetailById(int employeeWorkShiftId)
        {
            VEmployeeWorkShiftDetail employeeWorkShift;
            try
            {
                employeeWorkShift = Context.VEmployeeWorkShiftDetails.SingleOrDefault(x=>x.EmployeeWorkShiftId==employeeWorkShiftId);
            }
            catch (Exception)
            {

                throw;
            }
            return employeeWorkShift;
        }

        public List<VEmployeeWorkShiftDetail> GetEmployeeWorkShiftDetailBySearchKey(SearchFieldModel searchFieldModel)
        {
            IQueryable<VEmployeeWorkShiftDetail> employeeWorkShiftDetails;
            try
            {
                employeeWorkShiftDetails = Context.VEmployeeWorkShiftDetails
                    .Where(
                        x =>
                            (x.EmployeeCardId == searchFieldModel.SearchByEmployeeCardId ||
                             searchFieldModel.SearchByEmployeeCardId == null)
                            &&
                            (x.CompanyId == searchFieldModel.SearchByCompanyId ||
                             searchFieldModel.SearchByCompanyId == 0)
                            &&
                            (x.BranchId == searchFieldModel.SearchByBranchId || searchFieldModel.SearchByBranchId == 0)
                            &&
                            (x.BranchUnitId == searchFieldModel.SearchByBranchUnitId ||
                             searchFieldModel.SearchByBranchUnitId == 0)
                             &&
                              (x.BranchUnitDepartmentId == searchFieldModel.SearchByBranchUnitDepartmentId ||
                             searchFieldModel.SearchByBranchUnitDepartmentId == 0)
                            &&
                            (x.BranchUnitWorkShiftId == searchFieldModel.SearchByBranchUnitWorkShiftId ||
                             searchFieldModel.SearchByBranchUnitWorkShiftId == 0)
                            &&
                            (x.DepartmentSectionId == searchFieldModel.SearchByDepartmentSectionId ||
                             searchFieldModel.SearchByDepartmentSectionId == 0)
                            &&
                            (x.DepartmentLineId == searchFieldModel.SearchByDepartmentLineId ||
                             searchFieldModel.SearchByDepartmentLineId == 0)
                            && ((x.ShiftDate >= searchFieldModel.StartDate || searchFieldModel.StartDate == null)
                                && (x.ShiftDate <= searchFieldModel.EndDate || searchFieldModel.EndDate == null)));

          
            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message);
            }

            return employeeWorkShiftDetails.ToList();
        }

        public List<EmployeesForWorkShiftCustomModel> GetEmployeesForWorkShift(EmployeeWorkShift model, SearchFieldModel searchFieldModel)
        {
            var listEmployeesForWorkShift = new List<EmployeesForWorkShiftCustomModel>();

            try
            {
                var employeeCardIdParam = !String.IsNullOrEmpty(searchFieldModel.SearchByEmployeeCardId) ?
                                           new ObjectParameter("EmployeeCardId", searchFieldModel.SearchByEmployeeCardId) :
                                           new ObjectParameter("EmployeeCardId", typeof(string));

                var companyIdParam = new ObjectParameter("CompanyId", searchFieldModel.SearchByCompanyId);

                var branchIdParam = new ObjectParameter("BranchId", searchFieldModel.SearchByBranchId);

                var branchUnitIdParam = new ObjectParameter("BranchUnitId", searchFieldModel.SearchByBranchUnitId);

                var branchUnitDepartmentIdParam = (searchFieldModel.SearchByBranchUnitDepartmentId > 0) ?
                                      new ObjectParameter("BranchUnitDepartmentId", searchFieldModel.SearchByBranchUnitDepartmentId) :
                                     new ObjectParameter("BranchUnitDepartmentId", typeof(int));

                var sectionIdParam = (searchFieldModel.SearchBySectionId > 0) ?
                 new ObjectParameter("DepartmentSectionId", searchFieldModel.SearchBySectionId) :
                 new ObjectParameter("DepartmentSectionId", typeof(int));

                var lineIdParam = (searchFieldModel.SearchByLineId > 0) ?
                 new ObjectParameter("DepartmentLineId", searchFieldModel.SearchByLineId) :
                 new ObjectParameter("DepartmentLineId", typeof(int));

                var workGroupIdParam = (searchFieldModel.SearchByWorkGroupId > 0) ?
                                     new ObjectParameter("WorkGroupId", searchFieldModel.SearchByWorkGroupId) :
                                    new ObjectParameter("WorkGroupId", typeof(int));


                var branchUnitWorkShiftIdParam = (model.CheckingBranchUnitWorkShiftId > 0) ?
                                     new ObjectParameter("BranchUnitWorkShiftId", model.CheckingBranchUnitWorkShiftId) :
                                    new ObjectParameter("BranchUnitWorkShiftId", typeof(int));


                var employeeTypeIdParam = (searchFieldModel.SearchByEmployeeTypeId > 0) ?
                                     new ObjectParameter("EmployeeTypeId", searchFieldModel.SearchByEmployeeTypeId) :
                                    new ObjectParameter("EmployeeTypeId", typeof(int));



                var checkDateParam = (model.CheckingShiftDate !=null) ?
                                     new ObjectParameter("CheckDate", model.CheckingShiftDate) :
                                    new ObjectParameter("CheckDate", typeof(DateTime));


                var userNameParam = new ObjectParameter("UserName", PortalContext.CurrentUser.Name);


                var spEmployeesForWorkShift = Context.SPGetEmployeesForWorkShift(employeeCardIdParam, companyIdParam, branchIdParam,
                                                                        branchUnitIdParam, branchUnitDepartmentIdParam, sectionIdParam,
                                                                        lineIdParam, workGroupIdParam, branchUnitWorkShiftIdParam, employeeTypeIdParam,
                                                                        checkDateParam, userNameParam);


                foreach (var employeesForWorkShift in spEmployeesForWorkShift)
                {
                    var employees = new EmployeesForWorkShiftCustomModel
                    {
                        EmployeeId = employeesForWorkShift.EmployeeId,
                        EmployeeCardNo = employeesForWorkShift.EmployeeCardNo,
                        EmployeeName = employeesForWorkShift.EmployeeName,
                        Department = employeesForWorkShift.Department,
                        Section = employeesForWorkShift.Section,
                        Line = employeesForWorkShift.Line,
                        EmployeeType = employeesForWorkShift.EmployeeType,
                        Grade = employeesForWorkShift.Grade,
                        Designation = employeesForWorkShift.Designation,
                        JoiningDate = employeesForWorkShift.JoiningDate,
                        WorkGroup = employeesForWorkShift.WorkGroup,
                        WorkGroupAssignedDate = employeesForWorkShift.WorkGroupAssignedDate
                    };

                    listEmployeesForWorkShift.Add(employees);
                }
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }

            return listEmployeesForWorkShift;
        }

        public int SaveNewJoiningEmployeeWorkShift(Guid employeeId, DateTime? joiningDate, int branchUnitId)
        {
            try
            {
                
                var employeeIdParam = new SqlParameter { ParameterName = "EmployeeId", Value = employeeId };
                var joiningDateParam = new SqlParameter { ParameterName = "JoiningDate", Value = joiningDate };
                var branchUnitIdParam = new SqlParameter { ParameterName = "BranchUnitId", Value = branchUnitId };

                return Context.Database.SqlQuery<int>("spHrmSaveNewJoiningEmployeeWorkShift @EmployeeId, @JoiningDate, @BranchUnitId", employeeIdParam, joiningDateParam, branchUnitIdParam).ToList()[0]; ;
            }
            catch (Exception exception)
            {
                throw exception;
            }

        }

        public int UpdateWorkShiftQuick(List<int> workShiftList,int searchByBranchUnitWorkShiftId)
        {

            var saveChanges = 0;

            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {

            
                try
                {
               
                    foreach (var intem in workShiftList)
                    {
                        EmployeeWorkShift employeeWorkShift = Context.EmployeeWorkShifts.SingleOrDefault(x => x.EmployeeWorkShiftId == intem);
                        employeeWorkShift.BranchUnitWorkShiftId = searchByBranchUnitWorkShiftId;
                        saveChanges += Context.SaveChanges();

                    }


                    dbContextTransaction.Commit();


               }
               catch (Exception)
               {
                    dbContextTransaction.Rollback();
               }

            }
            return saveChanges;

        }
    }
}
