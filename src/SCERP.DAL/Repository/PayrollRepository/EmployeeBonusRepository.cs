using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Runtime.Remoting.Contexts;
using SCERP.Common;
using SCERP.DAL.IRepository.IPayrollRepository;
using SCERP.Model;
using System.Linq;
using SCERP.Model.Custom;
using System.Data.SqlClient;


namespace SCERP.DAL.Repository.PayrollRepository
{
    public class EmployeeBonusRepository : Repository<EmployeeBonus>, IEmployeeBonusRepository
    {
        public EmployeeBonusRepository(SCERPDBContext context)
            : base(context)
        {
        }

        public EmployeeBonus GetEmployeeBonusById(int? id)
        {
            return Context.EmployeeBonus.Where(x => x.EmployeeBonusId == id && x.IsActive).Include(x => x.Employee).FirstOrDefault();
        }

        public List<EmployeeBonusView> GetAllEmployeeBonusesByPaging(int startPage, int pageSize, out int totalRecords, EmployeeBonus employeeBonus, SearchFieldModel model)
        {
            List<EmployeeBonusView> employeeBonuses;

            try
            {
                var departmentIdParam = new SqlParameter { ParameterName = "BranchUnitDepartmentId", Value = model.SearchByBranchUnitDepartmentId };
                var sectionIdParam = new SqlParameter { ParameterName = "SectionId", Value = model.SearchByDepartmentSectionId };
                var lineIdParam = new SqlParameter { ParameterName = "LineId", Value = model.SearchByDepartmentLineId };
                var employeeCardIdParam = new SqlParameter { ParameterName = "employeeCardId", Value = model.SearchByEmployeeCardId ?? "0"  };
                var employeeTypeId = new SqlParameter { ParameterName = "EmployeeTypeId", Value = model.SearchByEmployeeTypeId };
                var fromDateParam = new SqlParameter { ParameterName = "fromDate", Value = model.StartDate };
                var toDateParam = new SqlParameter { ParameterName = "toDate", Value = model.EndDate };

                employeeBonuses = Context.Database.SqlQuery<EmployeeBonusView>("SPGetEmployeeBonus  @employeeCardId,@BranchUnitDepartmentId,@SectionId,@LineId,@EmployeeTypeId,@fromDate,@toDate",
                                                                                                employeeCardIdParam, departmentIdParam, sectionIdParam, lineIdParam,
                                                                                                employeeTypeId, fromDateParam, toDateParam).ToList<EmployeeBonusView>();

                totalRecords = employeeBonuses.Count();

                employeeBonuses = employeeBonuses.OrderByDescending(r => r.BonusDate).Skip(startPage * pageSize).Take(pageSize).ToList();

                //switch (employeeBonus.sort)
                //{
                //    case "Amount":

                //        switch (employeeBonus.sortdir)
                //        {
                //            case "DESC":
                //                employeeBonuses = employeeBonuses
                //                    .OrderByDescending(r => r.Amount)
                //                    .Skip(startPage * pageSize)
                //                    .Take(pageSize).ToList();
                //                break;

                //            default:
                //                employeeBonuses = employeeBonuses
                //                    .OrderBy(r => r.Amount)
                //                    .Skip(startPage * pageSize)
                //                    .Take(pageSize).ToList();
                //                break;
                //        }
                //        break;

                //    case "ReceivedDate":

                //        switch (employeeBonus.sortdir)
                //        {
                //            case "DESC":
                //                employeeBonuses = employeeBonuses
                //                    .OrderByDescending(r => r.EffectiveDate)
                //                    .Skip(startPage * pageSize)
                //                    .Take(pageSize).ToList();
                //                break;

                //            default:
                //                employeeBonuses = employeeBonuses
                //                    .OrderBy(r => r.EffectiveDate)
                //                    .Skip(startPage * pageSize)
                //                    .Take(pageSize).ToList();
                //                break;
                //        }
                //        break;

                  
                //    default:
                //        employeeBonuses = employeeBonuses.OrderBy(r => r.EffectiveDate).Skip(startPage * pageSize).Take(pageSize).ToList();
                //        break;
                //}
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }

            return employeeBonuses.ToList();
        }

        public List<EmployeeBonus> GetAllEmployeeBonuses()
        {
            return Context.EmployeeBonus.Where(x => x.IsActive == true).OrderBy(y => y.EffectiveDate).ToList();
        }

        public List<EmployeeBonus> GetEmployeeBonusBySearchKey(decimal searchByAmount, DateTime searchByFromDate, DateTime searchByToDate)
        {
            List<EmployeeBonus> employeeBonuses = null;

            try
            {
                employeeBonuses = Context.EmployeeBonus.Where(x => x.IsActive == true && x.Amount == searchByAmount && x.EffectiveDate >= searchByFromDate && x.EffectiveDate <= searchByToDate).ToList();
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return employeeBonuses;
        }


        public List<EmployeesForBonusCustomModel> GetEmployeesForBonus(EmployeeBonus model, SearchFieldModel searchFieldModel)
        {
            var listEmployeesForBonus = new List<EmployeesForBonusCustomModel>();

            try
            {
                var employeeCardIdParam = !String.IsNullOrEmpty(searchFieldModel.SearchByEmployeeCardId) ?
                                           new ObjectParameter("employeeCardId", searchFieldModel.SearchByEmployeeCardId) :
                                           new ObjectParameter("employeeCardId", typeof(string));

                var companyIdParam = new ObjectParameter("companyId", searchFieldModel.SearchByCompanyId);

                var branchIdParam = new ObjectParameter("branchId", searchFieldModel.SearchByBranchId);

                var branchUnitIdParam = new ObjectParameter("branchUnitId", searchFieldModel.SearchByBranchUnitId);

                var branchUnitDepartmentIdParam = (searchFieldModel.SearchByBranchUnitDepartmentId > 0) ?
                  new ObjectParameter("branchUnitDepartmentId", searchFieldModel.SearchByBranchUnitDepartmentId) :
                  new ObjectParameter("branchUnitDepartmentId", typeof(int));

                var sectionIdParam = (searchFieldModel.SearchBySectionId > 0) ?
                 new ObjectParameter("sectionId", searchFieldModel.SearchBySectionId) :
                 new ObjectParameter("sectionId", typeof(int));

                var lineIdParam = (searchFieldModel.SearchByLineId > 0) ?
                 new ObjectParameter("lineId", searchFieldModel.SearchByLineId) :
                 new ObjectParameter("lineId", typeof(int));


                var employeeTypeParam = new ObjectParameter("employeeTypeId", searchFieldModel.SearchByEmployeeTypeId);


                var effectiveDateParam = new ObjectParameter("EffectiveDate", model.EffectiveDate);
                    

                var spEmployeesForBonus = Context.SPGetEmployeesForBonus(employeeCardIdParam, companyIdParam, branchIdParam,
                                                                        branchUnitIdParam, branchUnitDepartmentIdParam, sectionIdParam,
                                                                        lineIdParam, employeeTypeParam, effectiveDateParam);


                foreach (var employeesForBonus in spEmployeesForBonus)
                {
                    var employees = new EmployeesForBonusCustomModel()
                    {
                        EmployeeId = employeesForBonus.EmployeeId,
                        EmployeeCardId = employeesForBonus.EmployeeCardId,
                        Department = employeesForBonus.Department,
                        Section = employeesForBonus.Section,
                        Line = employeesForBonus.Line,
                        Name = employeesForBonus.Name,
                        Designation = employeesForBonus.Designation,
                        Grade = employeesForBonus.Grade,
                        JoiningDate = employeesForBonus.JoiningDate,
                        BonusDate = employeesForBonus.BonusDate,
                        ServiceLength = employeesForBonus.ServiceLength,
                        BasicSalary = employeesForBonus.BasicSalary,
                        GrossSalary = employeesForBonus.GrossSalary,
                        BonusAmount = employeesForBonus.BonusAmount
                    };

                    listEmployeesForBonus.Add(employees);
                }
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }

            return listEmployeesForBonus;
        }


        public int SaveEmployeeBonus(List<EmployeeBonus> employeeBonuses)
        {
            var saveChanges = 0;
            try
            {
                Context.EmployeeBonus.AddRange(employeeBonuses);
                saveChanges = Context.SaveChanges();
            }
            catch (Exception exception)
            {
            }

            return saveChanges;
        }

    }
}
