using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using SCERP.Common;
using SCERP.DAL.IRepository.IHRMRepository;
using SCERP.Model;
using SCERP.Model.Custom;

namespace SCERP.DAL.Repository.HRMRepository
{
    public class EmployeeDailyAttendanceRepository : Repository<EmployeeDailyAttendance>, IEmployeeDailyAttendanceRepository
    {
        public EmployeeDailyAttendanceRepository(SCERPDBContext context)
            : base(context)
        {
        }

        public List<VEmployeeDailyAttendanceDetail> GetEmployeeDailyAttendanceByPaging(int startPage, int pageSize, out int totalRecords, EmployeeDailyAttendance model, SearchFieldModel searchFieldModel)           
        {
            var fromDate = searchFieldModel.StartDate;
            var toDate = searchFieldModel.EndDate;
            if (searchFieldModel.StartDate.HasValue && searchFieldModel.EndDate.HasValue)
            {
                fromDate = fromDate.GetValueOrDefault();
                toDate = toDate.GetValueOrDefault().AddDays(1); //Do not remove AddDays(1)
            }
            IEnumerable<int> companyIdList = PortalContext.CurrentUser.PermissionContext.CompanyList.Select(x => x.CompanyId);
            IEnumerable<int> branchIdList = PortalContext.CurrentUser.PermissionContext.BranchList.Select(x => x.BranchId);
            IEnumerable<int> branchUnitIdList = PortalContext.CurrentUser.PermissionContext.UnitList.Select(x => x.BranchUnitId);
            IEnumerable<int> branchUnitDepartmentIdList = PortalContext.CurrentUser.PermissionContext.DepartmentList.Select(x => x.BranchUnitDepartmentId);
            IEnumerable<int> employeeTypeList = PortalContext.CurrentUser.PermissionContext.EmployeeTypeList.Select(x => x.Id);


            var employeeDailyAttendanceDetails =
                Context.VEmployeeDailyAttendanceDetails.Where(x => companyIdList.Contains(x.CompanyId) &&
                                                                   branchIdList.Contains(x.BranchId) &&
                                                                   branchUnitIdList.Contains(x.BranchUnitId) &&
                                                                   branchUnitDepartmentIdList.Contains(x.BranchUnitDepartmentId) &&
                                                                   employeeTypeList.Contains(x.EmployeeTypeId) &&
                                                                   (x.EmployeeCardId == searchFieldModel.SearchByEmployeeCardId || searchFieldModel.SearchByEmployeeCardId == null) &&
                                                                   (x.DepartmentSectionId == searchFieldModel.SearchByDepartmentSectionId || searchFieldModel.SearchByDepartmentSectionId == 0) &&
                                                                   (x.DepartmentLineId == searchFieldModel.SearchByDepartmentLineId || searchFieldModel.SearchByDepartmentLineId == 0) &&
                                                                   ((x.TransactionDateTime >= fromDate || fromDate == null) && (x.TransactionDateTime <= toDate || toDate == null)) &&
                                                                   (x.Status == searchFieldModel.SearchByEmployeeStatus || searchFieldModel.SearchByEmployeeStatus == 0)
                    );

            if (searchFieldModel.SearchByCompanyId > 0 || searchFieldModel.SearchByBranchId > 0 || searchFieldModel.SearchByBranchUnitId > 0 || searchFieldModel.SearchByBranchUnitDepartmentId > 0)
            {
                employeeDailyAttendanceDetails =
                    employeeDailyAttendanceDetails.Where(x => (x.CompanyId == searchFieldModel.SearchByCompanyId || searchFieldModel.SearchByCompanyId == 0)
                                                              && (x.BranchId == searchFieldModel.SearchByBranchId || searchFieldModel.SearchByBranchId == 0)
                                                              && (x.BranchUnitId == searchFieldModel.SearchByBranchUnitId || searchFieldModel.SearchByBranchUnitId == 0)
                                                              && (x.BranchUnitDepartmentId == searchFieldModel.SearchByBranchUnitDepartmentId || searchFieldModel.SearchByBranchUnitDepartmentId == 0));
            }


            totalRecords = employeeDailyAttendanceDetails.Count();
            switch (model.sort)
            {
                case "EmployeeName":
                    switch (model.sortdir)
                    {

                        case "DESC":
                            employeeDailyAttendanceDetails = employeeDailyAttendanceDetails.OrderByDescending(
                                x => x.EmployeeName).ThenBy(x => x.TransactionDateTime)
                                .Skip(startPage*pageSize)
                                .Take(pageSize);
                            break;
                        default:
                            employeeDailyAttendanceDetails = employeeDailyAttendanceDetails.OrderBy(
                                x => x.EmployeeName).ThenBy(x => x.TransactionDateTime)
                                .Skip(startPage*pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;
                case "EmployeeCardId":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            employeeDailyAttendanceDetails = employeeDailyAttendanceDetails
                                .OrderByDescending(r => r.EmployeeCardId).ThenBy(x => x.TransactionDateTime)
                                .Skip(startPage*pageSize)
                                .Take(pageSize);
                            break;
                        default:
                            employeeDailyAttendanceDetails = employeeDailyAttendanceDetails
                                .OrderBy(r => r.EmployeeCardId).ThenBy(x => x.TransactionDateTime)
                                .Skip(startPage*pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;
                case "Designation":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            employeeDailyAttendanceDetails = employeeDailyAttendanceDetails
                                .OrderByDescending(r => r.Designation).ThenBy(x => x.TransactionDateTime)
                                .Skip(startPage*pageSize)
                                .Take(pageSize);
                            break;
                        default:
                            employeeDailyAttendanceDetails = employeeDailyAttendanceDetails
                                .OrderBy(r => r.Designation).ThenBy(x => x.TransactionDateTime)
                                .Skip(startPage*pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;
                case "CompanyName":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            employeeDailyAttendanceDetails = employeeDailyAttendanceDetails
                                .OrderByDescending(r => r.CompanyName).ThenBy(x => x.TransactionDateTime)
                                .Skip(startPage*pageSize)
                                .Take(pageSize);
                            break;
                        default:
                            employeeDailyAttendanceDetails = employeeDailyAttendanceDetails
                                .OrderBy(r => r.CompanyName).ThenBy(x => x.TransactionDateTime)
                                .Skip(startPage*pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;
                case "BranchName":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            employeeDailyAttendanceDetails = employeeDailyAttendanceDetails
                                .OrderByDescending(r => r.BranchName).ThenBy(x => x.TransactionDateTime)
                                .Skip(startPage*pageSize)
                                .Take(pageSize);
                            break;
                        default:
                            employeeDailyAttendanceDetails = employeeDailyAttendanceDetails
                                .OrderBy(r => r.BranchName).ThenBy(x => x.TransactionDateTime)
                                .Skip(startPage*pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;
                case "UnitName":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            employeeDailyAttendanceDetails = employeeDailyAttendanceDetails
                                .OrderByDescending(r => r.UnitName).ThenBy(x => x.TransactionDateTime)
                                .Skip(startPage*pageSize)
                                .Take(pageSize);
                            break;
                        default:
                            employeeDailyAttendanceDetails = employeeDailyAttendanceDetails
                                .OrderBy(r => r.UnitName).ThenBy(x => x.TransactionDateTime)
                                .Skip(startPage*pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;
                case "DepartmentName":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            employeeDailyAttendanceDetails = employeeDailyAttendanceDetails
                                .OrderByDescending(r => r.DepartmentName).ThenBy(x => x.TransactionDateTime)
                                .Skip(startPage*pageSize)
                                .Take(pageSize);
                            break;
                        default:
                            employeeDailyAttendanceDetails = employeeDailyAttendanceDetails
                                .OrderBy(r => r.DepartmentName).ThenBy(x => x.TransactionDateTime)
                                .Skip(startPage*pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;
                case "SectionName":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            employeeDailyAttendanceDetails = employeeDailyAttendanceDetails
                                .OrderByDescending(r => r.SectionName).ThenBy(x => x.TransactionDateTime)
                                .Skip(startPage*pageSize)
                                .Take(pageSize);
                            break;
                        default:
                            employeeDailyAttendanceDetails = employeeDailyAttendanceDetails
                                .OrderBy(r => r.SectionName).ThenBy(x => x.TransactionDateTime)
                                .Skip(startPage*pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;
                case "LineName":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            employeeDailyAttendanceDetails = employeeDailyAttendanceDetails
                                .OrderByDescending(r => r.LineName).ThenBy(x => x.TransactionDateTime)
                                .Skip(startPage*pageSize)
                                .Take(pageSize);
                            break;
                        default:
                            employeeDailyAttendanceDetails = employeeDailyAttendanceDetails
                                .OrderBy(r => r.LineName).ThenBy(x => x.TransactionDateTime)
                                .Skip(startPage*pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;

                default:
                    employeeDailyAttendanceDetails = employeeDailyAttendanceDetails
                        .OrderBy(r => r.EmployeeCardId).ThenBy(x => x.TransactionDateTime)
                        .Skip(startPage*pageSize)
                        .Take(pageSize);
                    break;
            }
            return employeeDailyAttendanceDetails.ToList();
        }

        public List<VEmployeeCompanyInfoDetail> GetEmployes(int startPage, int pageSize, out int totalRecords,
            EmployeeDailyAttendance model, SearchFieldModel searchFieldModel)
        {

            IQueryable<VEmployeeCompanyInfoDetail> employeeCompanyInfoDetails;
            try
            {
                IEnumerable<int> companyIdList = PortalContext.CurrentUser.PermissionContext.CompanyList.Select(x => x.CompanyId);
                IEnumerable<int> branchIdList = PortalContext.CurrentUser.PermissionContext.BranchList.Select(x => x.BranchId);
                IEnumerable<int> branchUnitIdList = PortalContext.CurrentUser.PermissionContext.UnitList.Select(x => x.BranchUnitId);
                IEnumerable<int> branchUnitDepartmentIdList = PortalContext.CurrentUser.PermissionContext.DepartmentList.Select(x => x.BranchUnitDepartmentId);
                IEnumerable<int> employeeTypeList = PortalContext.CurrentUser.PermissionContext.EmployeeTypeList.Select(x => x.Id);


                employeeCompanyInfoDetails =
                    Context.VEmployeeCompanyInfoDetails.Where(x => companyIdList.Contains(x.CompanyId) && 
                                                                   branchIdList.Contains(x.BranchId) &&
                                                                   branchUnitIdList.Contains(x.BranchUnitId) &&
                                                                   branchUnitDepartmentIdList.Contains(x.BranchUnitDepartmentId) &&
                                                                   employeeTypeList.Contains(x.EmployeeTypeId) &&
                                                                   (x.EmployeeCardId == searchFieldModel.SearchByEmployeeCardId || searchFieldModel.SearchByEmployeeCardId == null) &&
                                                                   (x.DepartmentSectionId == searchFieldModel.SearchByDepartmentSectionId || searchFieldModel.SearchByDepartmentSectionId == 0) &&
                                                                   (x.DepartmentLineId == searchFieldModel.SearchByDepartmentLineId || searchFieldModel.SearchByDepartmentLineId == 0)
                                                                   
                        );

                employeeCompanyInfoDetails = employeeCompanyInfoDetails.Where(p => p.Status == 1);  // Added to filter Quit Employee

                if (searchFieldModel.SearchByCompanyId > 0 || searchFieldModel.SearchByBranchId > 0 || searchFieldModel.SearchByBranchUnitId > 0 || searchFieldModel.SearchByBranchUnitDepartmentId > 0)
                {
                    employeeCompanyInfoDetails =
                        employeeCompanyInfoDetails.Where(x => (x.CompanyId == searchFieldModel.SearchByCompanyId || searchFieldModel.SearchByCompanyId == 0)
                                                              && (x.BranchId == searchFieldModel.SearchByBranchId || searchFieldModel.SearchByBranchId == 0)
                                                              && (x.BranchUnitId == searchFieldModel.SearchByBranchUnitId || searchFieldModel.SearchByBranchUnitId == 0)
                                                              && (x.BranchUnitDepartmentId == searchFieldModel.SearchByBranchUnitDepartmentId || searchFieldModel.SearchByBranchUnitDepartmentId == 0));
                }

                totalRecords = employeeCompanyInfoDetails.Count();
                switch (model.sort)
                {
                    case "EmployeeName":
                        switch (model.sortdir)
                        {
                            case "DESC":
                                employeeCompanyInfoDetails = employeeCompanyInfoDetails
                                    .OrderByDescending(r => r.EmployeeName);
                                break;
                            default:
                                employeeCompanyInfoDetails = employeeCompanyInfoDetails
                                    .OrderBy(r => r.EmployeeName);
                                break;
                        }
                        break;
                    case "EmployeeCardId":
                        switch (model.sortdir)
                        {
                            case "DESC":
                                employeeCompanyInfoDetails = employeeCompanyInfoDetails
                                    .OrderByDescending(r => r.EmployeeCardId);
                                break;
                            default:
                                employeeCompanyInfoDetails = employeeCompanyInfoDetails
                                    .OrderBy(r => r.EmployeeCardId);
                                break;
                        }
                        break;
                    case "Designation":
                        switch (model.sortdir)
                        {
                            case "DESC":
                                employeeCompanyInfoDetails = employeeCompanyInfoDetails
                                    .OrderByDescending(r => r.Designation);
                                break;
                            default:
                                employeeCompanyInfoDetails = employeeCompanyInfoDetails
                                    .OrderBy(r => r.Designation);
                                break;
                        }
                        break;
                    case "CompanyName":
                        switch (model.sortdir)
                        {
                            case "DESC":
                                employeeCompanyInfoDetails = employeeCompanyInfoDetails
                                    .OrderByDescending(r => r.CompanyName);
                                break;
                            default:
                                employeeCompanyInfoDetails = employeeCompanyInfoDetails
                                    .OrderBy(r => r.CompanyName);
                                break;
                        }
                        break;
                    case "BranchName":
                        switch (model.sortdir)
                        {
                            case "DESC":
                                employeeCompanyInfoDetails = employeeCompanyInfoDetails
                                    .OrderByDescending(r => r.BranchName);
                                break;
                            default:
                                employeeCompanyInfoDetails = employeeCompanyInfoDetails
                                    .OrderBy(r => r.BranchName);
                                break;
                        }
                        break;
                    case "UnitName":
                        switch (model.sortdir)
                        {
                            case "DESC":
                                employeeCompanyInfoDetails = employeeCompanyInfoDetails
                                    .OrderByDescending(r => r.UnitName);
                                break;
                            default:
                                employeeCompanyInfoDetails = employeeCompanyInfoDetails
                                    .OrderBy(r => r.UnitName);
                                break;
                        }
                        break;
                    case "DepartmentName":
                        switch (model.sortdir)
                        {
                            case "DESC":
                                employeeCompanyInfoDetails = employeeCompanyInfoDetails
                                    .OrderByDescending(r => r.DepartmentName);
                                break;
                            default:
                                employeeCompanyInfoDetails = employeeCompanyInfoDetails
                                    .OrderBy(r => r.DepartmentName);
                                break;
                        }
                        break;
                    case "SectionName":
                        switch (model.sortdir)
                        {
                            case "DESC":
                                employeeCompanyInfoDetails = employeeCompanyInfoDetails
                                    .OrderByDescending(r => r.SectionName);
                                break;
                            default:
                                employeeCompanyInfoDetails = employeeCompanyInfoDetails
                                    .OrderBy(r => r.SectionName);
                                break;
                        }
                        break;
                    case "LineName":
                        switch (model.sortdir)
                        {
                            case "DESC":
                                employeeCompanyInfoDetails = employeeCompanyInfoDetails
                                    .OrderByDescending(r => r.LineName);
                                break;
                            default:
                                employeeCompanyInfoDetails = employeeCompanyInfoDetails
                                    .OrderBy(r => r.LineName);
                                break;
                        }
                        break;

                    default:
                        switch (model.sortdir)
                        {
                            case "DESC":
                                employeeCompanyInfoDetails = employeeCompanyInfoDetails
                                    .OrderByDescending(r => r.EmployeeCardId);
                                break;
                            default:
                                employeeCompanyInfoDetails = employeeCompanyInfoDetails
                                    .OrderBy(r => r.EmployeeCardId);
                                break;
                        }

                        break;
                }

            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message);
            }
            return employeeCompanyInfoDetails.ToList();
        }

        public EmployeeDailyAttendance GetEmployeeDailyAttendance(int employeeDailyAttendanceId)
        {
            EmployeeDailyAttendance employeeDailyAttendance;
            try
            {
                employeeDailyAttendance =
                    Context.EmployeeDailyAttendances.Include(x => x.Employee).SingleOrDefault(x => x.IsActive && x.Id == employeeDailyAttendanceId);
            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message, exception.InnerException);
            }
            return employeeDailyAttendance;
        }

        public bool ImportMachineAttendanceData(object[] parameters)
        {


            bool importStatus = false;

            try
            {
                var status =
                    Context.Database.SqlQuery<int>("SPGetAttendanceDataFromMachine @FromDate, @ToDate", parameters)
                        .ToList()
                        .FirstOrDefault();

                if (status > 0)
                    importStatus = true;

            }
            catch (SqlException exception)
            {
                throw new Exception(exception.Message + "/n" + "Data not import from link server");
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
            return importStatus;
        }

        public int SaveEmployeeDailyAttendances(List<EmployeeDailyAttendance> employeeDailyAttendances)
        {
            var saveChanges = 0;
            try
            {
                Context.EmployeeDailyAttendances.AddRange(employeeDailyAttendances);
                saveChanges = Context.SaveChanges();
            }
            catch (Exception exception)
            {
            }
            return saveChanges;
        }


        public int ProcessEmployeeInOut(SearchFieldModel searchFieldModel)
        {
            var processedResult = 0;
            string storeProcedureName = "";

            var connectionString = Context.Database.Connection.ConnectionString;
            SqlConnection conn = new SqlConnection(connectionString);

            if (searchFieldModel.ProcessType == "InOut")
                storeProcedureName = "dbo.SPProcessEmployeeInOut2";
            else if (searchFieldModel.ProcessType == "OT")
                storeProcedureName = "dbo.SPProcessEmployeeInOutOT";

            using (conn)
            {
                SqlCommand cmd = new SqlCommand(storeProcedureName, conn);
                cmd.CommandType = CommandType.StoredProcedure;

                DateTime? fromDate = new DateTime(1900, 01, 01);
                if (searchFieldModel.EffectiveFromDate != null)
                    fromDate = searchFieldModel.EffectiveFromDate;

                SqlParameter fromDateParam = cmd.Parameters.AddWithValue("@FromDate", fromDate);
                SqlParameter userNameParam = cmd.Parameters.AddWithValue("@UserName", PortalContext.CurrentUser.Name);

                fromDateParam.SqlDbType = SqlDbType.DateTime;
                userNameParam.SqlDbType = SqlDbType.NVarChar;

                conn.Open();
                cmd.CommandTimeout = 3600000;
                processedResult = Convert.ToInt16(cmd.ExecuteScalar());
                conn.Close();
            }

            return processedResult;
        }
    }
}
