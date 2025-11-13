using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using SCERP.Common;
using SCERP.DAL.IRepository.IHRMRepository;
using System.Linq;
using System;
using SCERP.Model;
using SCERP.Model.Custom;
using SCERP.Model.HRMModel;
using System.Data.SqlClient;

namespace SCERP.DAL.Repository.HRMRepository
{
    public class EmployeeRepository : Repository<Employee>, IEmployeeRepository
    {

        public EmployeeRepository(SCERPDBContext context)
            : base(context)
        {

        }

        public IQueryable<EmployeeViewModel> GetEmployees(int? employeeDepartment, int? employeeWorkGroup, int? employeeWorkShift, int? employeeGrade, int? employeeType, int? employeeDesignationId, string searchByEmployeeCardId, string searchByName, string searchByPhone, bool isNewSearch)
        {
            IQueryable<EmployeeViewModel> employees = null;
            return employees;
        }

        public int SaveEmployeeInfo(EmployeeViewModel employeeViewModel)
        {
            var saved = 0;

            try
            {
                Context.Employees.Add(employeeViewModel.Employee);
                Context.EmployeeCompanyInfos.Add(employeeViewModel.EmployeeCompanyInfo);
                Context.EmployeePresentAddresses.Add(employeeViewModel.EmployeePresentAddress);
                Context.EmployeePermanentAddresses.Add(employeeViewModel.EmployeePermanentAddress);
                saved = Context.SaveChanges();
            }
            catch (Exception)
            {
                saved = 0;
            }

            return saved;
        }

        public Employee GetEmployeeById(Guid employeeId)
        {
            Employee employee = null;
            try
            {
                employee = Context.Employees.Find(employeeId);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }

            return employee;
        }

        public int SaveEmployeeMandatoryInfo(EmployeeMandatoryInfoCustomModel employeeMandatoryInfoViewModel)
        {
            var saved = 0;

            try
            {
                Context.Employees.Add(employeeMandatoryInfoViewModel.Employee);
                Context.EmployeeCompanyInfos.Add(employeeMandatoryInfoViewModel.EmployeeCompanyInfo);
                Context.EmployeePresentAddresses.Add(employeeMandatoryInfoViewModel.EmployeePresentAddress);
                saved = Context.SaveChanges();
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }

            return saved;
        }

        public EmployeeMandatoryInfoCustomModel GetEmployeeMandatoryInfoByEmployeeId(Guid? employeeGuid, DateTime? effectiveDate)
        {
            var employeeMandatoryInfoCustomModel = new EmployeeMandatoryInfoCustomModel();

            try
            {
                var employee = Context.SPGetSpecificEmployeeActiveInfo(employeeGuid, effectiveDate).ToList().FirstOrDefault();

                if (employee != null)
                {
                    employeeMandatoryInfoCustomModel.Employee.EmployeeId = employee.EmployeeId;
                    employeeMandatoryInfoCustomModel.Employee.EmployeeCardId = employee.EmployeeCardId;
                    employeeMandatoryInfoCustomModel.Employee.Name = employee.Name;
                    employeeMandatoryInfoCustomModel.Employee.NameInBengali = employee.NameInBengali;
                    employeeMandatoryInfoCustomModel.Employee.MothersName = employee.MothersName;
                    employeeMandatoryInfoCustomModel.Employee.MothersNameInBengali = employee.MothersNameInBengali;
                    employeeMandatoryInfoCustomModel.Employee.FathersName = employee.FathersName;
                    employeeMandatoryInfoCustomModel.Employee.FathersNameInBengali = employee.FathersNameInBengali;
                    employeeMandatoryInfoCustomModel.Employee.ReligionId = employee.ReligionId;
                    employeeMandatoryInfoCustomModel.Employee.DateOfBirth = employee.DateOfBirth;
                    employeeMandatoryInfoCustomModel.Employee.GenderId = (byte) employee.GenderId;
                    employeeMandatoryInfoCustomModel.EmployeePresentAddress.MobilePhone = employee.MobilePhone;
                    employeeMandatoryInfoCustomModel.Employee.JoiningDate = employee.JoiningDate;
                    employeeMandatoryInfoCustomModel.Employee.ConfirmationDate = employee.ConfirmationDate;
                    employeeMandatoryInfoCustomModel.EmployeePresentAddress.MailingAddress = employee.MailingAddress;
                    employeeMandatoryInfoCustomModel.EmployeePresentAddress.MailingAddressInBengali = employee.MailingAddressInBengali;
                    employeeMandatoryInfoCustomModel.EmployeeCompanyId = employee.CompanyId;
                    employeeMandatoryInfoCustomModel.EmployeeBranchId = employee.BranchId;
                    employeeMandatoryInfoCustomModel.EmployeeBranchUnitId = employee.BranchUnitId;
                    employeeMandatoryInfoCustomModel.EmployeeBranchUnitDepartmentId = employee.BranchUnitDepartmentId;
                    employeeMandatoryInfoCustomModel.EmployeeTypeId = employee.EmployeeTypeId;
                    employeeMandatoryInfoCustomModel.EmployeeGradeId = employee.EmployeeGradeId;
                    employeeMandatoryInfoCustomModel.EmployeeDesignationId = employee.EmployeeDesignationId;
                    employeeMandatoryInfoCustomModel.IsEligibleForOvertime = employee.IsEligibleForOvertime;
                    employeeMandatoryInfoCustomModel.EmployeeCompanyInfo.FromDate = employee.EffectiveDate;
                    employeeMandatoryInfoCustomModel.Employee.PhotographPath = employee.PhotographPath;
                }
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }

            return employeeMandatoryInfoCustomModel;
        }

        public IQueryable<EmployeeViewModel> GetEmployees(int? employeeDepartment, int? employeeType, int? employeeDesignationId, string searchByEmployeeCardId, string searchByName, bool searchAll)
        {
            var employees = from employee in Context.Employees
                join employeeCompanyInfo in Context.EmployeeCompanyInfos on employee.EmployeeId equals employeeCompanyInfo.EmployeeId
                where employee.IsActive == true && employeeCompanyInfo.IsActive == true
                select new EmployeeViewModel
                {
                    Employee = employee,
                    EmployeeCompanyInfo = employeeCompanyInfo
                };
            return employees;
        }

        public Employee GetEmployeeByCardId(string employeeCardId)
        {
            return Context.Employees.FirstOrDefault(x => (x.EmployeeCardId == employeeCardId
                                                          && x.IsActive == true
                                                          && x.Status == (int) StatusValue.Active));
        }

        public Employee GetAnyEmployeeByCardId(string employeeCardId)
        {
            return Context.Employees.FirstOrDefault(x => (x.EmployeeCardId == employeeCardId && x.IsActive == true));
        }

        public List<Employee> GetMerchandiser()
        {
            //return new List<Employee>();

            return Context.Employees.Where(p => p.Id < 10).ToList();

            //int type = Convert.ToInt32(Common.AuthorizationType.Merchandiser);

            //var merchandiser = from p in Context.Employees
            //    join q in Context.AuthorizedPersons on p.EmployeeId equals q.EmployeeId
            //    where q.AuthorizationTypeId == type
            //    select p;

            //return merchandiser.ToList();
        }

        public List<EmployeeInfoCustomModel> GetAllEmployeeInfoByPaging(int startPage, int pageSize, SearchFieldModel searchFieldModel, EmployeeInfoCustomModel model, out int totalRecords)
        {
            var employeeInfos = new List<EmployeeInfoCustomModel>();

            try
            {

                var companyIdParam = (searchFieldModel.SearchByCompanyId > 0) ?
                    new ObjectParameter("CompanyID", searchFieldModel.SearchByCompanyId) :
                    new ObjectParameter("CompanyID", typeof (int));

                var branchIdParam = (searchFieldModel.SearchByBranchId > 0) ?
                    new ObjectParameter("BranchID", searchFieldModel.SearchByBranchId) :
                    new ObjectParameter("BranchID", typeof (int));

                var branchUnitIdParam = (searchFieldModel.SearchByBranchUnitId > 0) ?
                    new ObjectParameter("BranchUnitID", searchFieldModel.SearchByBranchUnitId) :
                    new ObjectParameter("BranchUnitID", typeof (int));

                var branchUnitDepartmentIdParam = (searchFieldModel.SearchByBranchUnitDepartmentId > 0) ?
                    new ObjectParameter("BranchUnitDepartmentID", searchFieldModel.SearchByBranchUnitDepartmentId) :
                    new ObjectParameter("BranchUnitDepartmentID", typeof (int));

                var departmentSectionIdParam = (searchFieldModel.SearchByDepartmentSectionId > 0) ?
                    new ObjectParameter("DepartmentSectionId", searchFieldModel.SearchByDepartmentSectionId) :
                    new ObjectParameter("DepartmentSectionId", typeof (int));

                var departmentLineIdParam = (searchFieldModel.SearchByDepartmentLineId > 0) ?
                    new ObjectParameter("DepartmentLineId", searchFieldModel.SearchByDepartmentLineId) :
                    new ObjectParameter("DepartmentLineId", typeof (int));

                var employeeTypeIdParam = (searchFieldModel.SearchByEmployeeTypeId > 0) ?
                    new ObjectParameter("EmployeeTypeID", searchFieldModel.SearchByEmployeeTypeId) :
                    new ObjectParameter("EmployeeTypeID", typeof (int));

                var employeeGradeIdParam = (searchFieldModel.SearchByEmployeeGradeId > 0) ?
                    new ObjectParameter("EmployeeGradeID", searchFieldModel.SearchByEmployeeGradeId) :
                    new ObjectParameter("EmployeeGradeID", typeof (int));

                var employeeDesignationIdParam = (searchFieldModel.SearchByEmployeeDesignationId > 0) ?
                    new ObjectParameter("EmployeeDesignationID", searchFieldModel.SearchByEmployeeDesignationId) :
                    new ObjectParameter("EmployeeDesignationID", typeof (int));

                var genderIdParam = (searchFieldModel.SearchByEmployeeGenderId > 0) ?
                    new ObjectParameter("GenderId", searchFieldModel.SearchByEmployeeGenderId) :
                    new ObjectParameter("GenderId", typeof (int));

                var employeeCardIdParam = !String.IsNullOrEmpty(searchFieldModel.SearchByEmployeeCardId) ?
                    new ObjectParameter("EmployeeCardId", searchFieldModel.SearchByEmployeeCardId) :
                    new ObjectParameter("EmployeeCardId", typeof (string));

                var employeeNameParam = !String.IsNullOrEmpty(searchFieldModel.SearchByEmployeeName) ?
                    new ObjectParameter("EmployeeName", searchFieldModel.SearchByEmployeeName) :
                    new ObjectParameter("EmployeeName", typeof (string));


                var employeeMobileNumberParam = !String.IsNullOrEmpty(searchFieldModel.SearchByEmployeeMobileNo) ?
                    new ObjectParameter("EmployeeMobilePhone", searchFieldModel.SearchByEmployeeMobileNo) :
                    new ObjectParameter("EmployeeMobilePhone", typeof (string));

                var employeeStatusParam = (searchFieldModel.SearchByEmployeeStatus > 0) ?
                    new ObjectParameter("EmployeeStatus", searchFieldModel.SearchByEmployeeStatus) :
                    new ObjectParameter("EmployeeStatus", typeof (int));

                var userNameParam = !String.IsNullOrEmpty(PortalContext.CurrentUser.Name) ?
                    new ObjectParameter("UserName", PortalContext.CurrentUser.Name) :
                    new ObjectParameter("UserName", typeof (string));

                var fromDateParam = (searchFieldModel.StartDate.HasValue) ?
                    new ObjectParameter("FromDate", searchFieldModel.StartDate) :
                    new ObjectParameter("FromDate", typeof (DateTime));

                var startRowIndexParam = (startPage >= 0) ?
                    new ObjectParameter("StartRowIndex", startPage) :
                    new ObjectParameter("StartRowIndex", typeof (int));

                var maxRowsParam = (pageSize > 0) ?
                    new ObjectParameter("MaxRows", pageSize) :
                    new ObjectParameter("MaxRows", typeof (int));


                var sortFieldParam = new ObjectParameter("SortField", GetSortField(model.sort));

                var sortDirectionParam = new ObjectParameter("SortDiriection", Common.CustomPaging.GetSortDirection(model.sortdir));

                var employees = Context.SPGetAllEmployeeInfo(companyIdParam, branchIdParam,
                    branchUnitIdParam, branchUnitDepartmentIdParam,
                    departmentSectionIdParam, departmentLineIdParam,
                    employeeTypeIdParam, employeeGradeIdParam,
                    employeeDesignationIdParam,
                    genderIdParam, employeeCardIdParam,
                    employeeNameParam, employeeMobileNumberParam,
                    employeeStatusParam, userNameParam,
                    fromDateParam, startRowIndexParam, maxRowsParam,
                    sortFieldParam, sortDirectionParam, out totalRecords);

                int serialNo = startPage*pageSize + 1;

                foreach (var employeeInfoResult in employees)
                {
                    var employeeInfoCustomModel = new EmployeeInfoCustomModel();
                    if (employeeInfoResult.EmployeeId != null)
                        employeeInfoCustomModel.Employee.EmployeeId = (Guid) employeeInfoResult.EmployeeId;
                    employeeInfoCustomModel.Employee.EmployeeCardId = employeeInfoResult.EmployeeCardNo;
                    employeeInfoCustomModel.Employee.Name = employeeInfoResult.Name;
                    employeeInfoCustomModel.Company.Name = employeeInfoResult.Company;
                    employeeInfoCustomModel.Branch.Name = employeeInfoResult.Branch;
                    employeeInfoCustomModel.Unit.Name = employeeInfoResult.Unit;
                    employeeInfoCustomModel.Department.Name = employeeInfoResult.Department;
                    employeeInfoCustomModel.DepartmentSection.Section = new Section {Name = employeeInfoResult.Section};
                    employeeInfoCustomModel.DepartmentLine.Line = new Line {Name = employeeInfoResult.Line};
                    employeeInfoCustomModel.EmployeeDesignation.Title = employeeInfoResult.Designation;
                    employeeInfoCustomModel.Employee.Gender = new Gender {Title = employeeInfoResult.Gender};
                    employeeInfoCustomModel.EmployeePresentAddress.MobilePhone = employeeInfoResult.Mobile;
                    employeeInfoCustomModel.Employee.Status = employeeInfoResult.EmployeeStatus ?? 0;
                    employeeInfoCustomModel.SerialNo = serialNo;
                    employeeInfos.Add(employeeInfoCustomModel);
                    serialNo++;
                }

                var total = totalRecords;
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }

            return employeeInfos;
        }

        private int? GetSortField(string sortExpression)
        {

            switch (sortExpression)
            {
                case "Employee.EmployeeCardId":
                    return 1;
                    break;
                case "Employee.Name":
                    return 2;
                    break;
                case "Employee.Gender.Title":
                    return 3;
                    break;
                case "Company.Name":
                    return 4;
                    break;
                case "Branch.Name":
                    return 5;
                    break;
                case "Unit.Name":
                    return 6;
                    break;
                case "Department.Name":
                    return 7;
                    break;
                case "DepartmentSection.Section.Name":
                    return 8;
                    break;
                case "DepartmentLine.Line.Name":
                    return 9;
                    break;
                case "EmployeeDesignation.Title":
                    return 10;
                    break;
                case "Employee.Status":
                    return 11;
                    break;

                default:
                    return 1;
                    break;

            }
        }

        public bool CheckExistingEmployeeCardNumber(Employee employee)
        {

            try
            {
                var existingEmployee = Context.Employees.FirstOrDefault(x => (x.EmployeeCardId.Trim().Replace(" ", "") == employee.EmployeeCardId.Trim().Replace(" ", "")
                                                                              && x.IsActive == true));

                if (existingEmployee != null && !String.IsNullOrEmpty(existingEmployee.EmployeeCardId)) return true;

            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }

            return false;
        }

        public List<SkillMatrixMachineType> GetAllMachineTypes()
        {
            var machineTypes = Context.SkillMatrixMachineTypes.Where(p => p.IsActive).ToList();
            return machineTypes;
        }

        public List<SkillMatrixDetailProcess> GetSkillMatrixDetailByMachineTypeId(int? machineTypeId, Guid employeeId)
        {
            if (machineTypeId == 0)
                machineTypeId = -1;
            var machineTypeParam = new SqlParameter {ParameterName = "MachineTypeId", Value = machineTypeId};

            var employeeIdParam = new SqlParameter {ParameterName = "EmployeeId", Value = employeeId};

            return Context.Database.SqlQuery<SkillMatrixDetailProcess>("SPGetSkillMatrixDetailProcessName @MachineTypeId, @EmployeeId", machineTypeParam, employeeIdParam).ToList();
        }

        public SkillMatrixPointTable GetAllSkillPoint(Guid employeeId)
        {
            return Context.SkillMatrixPointTables.SingleOrDefault(p => p.EmployeeId == employeeId && p.IsActive);
        }

        public string SaveSkillMatrix(List<string> values, Guid employeeId, string employeeCardId)
        {

            var performance = Convert.ToDouble(values[0].Split('-').ElementAt(4).Trim());
            var attitude = Convert.ToDouble(values[0].Split('-').ElementAt(5).Trim());
            var machineTypeId = Convert.ToInt32(values[0].Split('-').ElementAt(6).Trim());

            SkillMatrix skillMatrix = Context.SkillMatrices.FirstOrDefault(p => p.EmployeeId == employeeId && p.IsActive);

            if (skillMatrix != null)
            {
                skillMatrix.Performance = performance;
                skillMatrix.Attitude = attitude;
                Context.SaveChanges();
            }

            else
            {
                SkillMatrix skillMat = new SkillMatrix();

                skillMat.EmployeeId = employeeId;
                skillMat.EmployeeCardId = employeeCardId;
                skillMat.Performance = performance;
                skillMat.Attitude = attitude;
                skillMat.CreatedDate = DateTime.Now;
                skillMat.CreatedBy = PortalContext.CurrentUser.UserId;
                skillMat.IsActive = true;

                Context.SkillMatrices.Add(skillMat);
                Context.SaveChanges();
            }

            skillMatrix = Context.SkillMatrices.FirstOrDefault(p => p.EmployeeId == employeeId && p.IsActive);

            List<SkillMatrixDetail> skillMatrixDetailList = new List<SkillMatrixDetail>();
            skillMatrixDetailList = Context.SkillMatrixDetails.Where(p => p.SkillMatrixId == skillMatrix.SkillMatrixId).ToList();

            foreach (var skill in skillMatrixDetailList)
            {
                Context.SkillMatrixDetails.Remove(skill);
                Context.SaveChanges();
            }

            foreach (var t in values)
            {
                var processId = Convert.ToInt32(t.Split('-').ElementAt(0).Trim());
                var processSmv = t.Split('-').ElementAt(1).Trim() == "" ? 0 : Convert.ToDouble(t.Split('-').ElementAt(1).Trim());
                var processGrade = t.Split('-').ElementAt(2).Trim();
                var averageCycle = t.Split('-').ElementAt(3).Trim() == "" ? 0 : Convert.ToDouble(t.Split('-').ElementAt(3).Trim());

                SkillMatrixDetail skillMatrixDetail = new SkillMatrixDetail();

                skillMatrixDetail.SkillMatrixId = skillMatrix.SkillMatrixId;
                skillMatrixDetail.MachineTypeId = machineTypeId;
                skillMatrixDetail.ProcessId = processId;
                skillMatrixDetail.ProcessSmv = processSmv;
                skillMatrixDetail.ProcessGrade = processGrade;
                skillMatrixDetail.AverageCycle = averageCycle;
                skillMatrixDetail.CreatedDate = DateTime.Now;
                skillMatrixDetail.CreatedBy = PortalContext.CurrentUser.UserId;
                skillMatrixDetail.IsActive = true;

                Context.SkillMatrixDetails.Add(skillMatrixDetail);
                Context.SaveChanges();
            }
            return "Data Saved Successfully !";
        }

        public int SaveSkillMatrixPoint(SkillMatrixPointTable model)
        {
            SkillMatrixPointTable pointTable = Context.SkillMatrixPointTables.FirstOrDefault(p => p.EmployeeId == model.EmployeeId);

            if (pointTable != null)
            {
                Context.SkillMatrixPointTables.Remove(pointTable);
                Context.SaveChanges();
            }

            model.CreatedDate = DateTime.Now;
            model.CreatedBy = PortalContext.CurrentUser.UserId;
            model.IsActive = true;
            
            Context.SkillMatrixPointTables.Add(model);
            Context.SaveChanges();
            return 1;
        }
    }
}