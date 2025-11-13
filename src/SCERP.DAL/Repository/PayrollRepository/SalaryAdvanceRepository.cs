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
    public class SalaryAdvanceRepository : Repository<SalaryAdvance>, ISalaryAdvanceRepository
    {
        public SalaryAdvanceRepository(SCERPDBContext context)
            : base(context)
        {
        }

        public SalaryAdvance GetSalaryAdvanceById(int? id)
        {
            return Context.SalaryAdvance.Where(x => x.SalaryAdvanceId == id).Include(x=>x.Employee).FirstOrDefault();
        }

        public List<SalaryAdvanceView> GetAllSalaryAdvancesByPaging(int startPage, int pageSize, out int totalRecords, SalaryAdvance salaryAdvance, SearchFieldModel model)
        {            
            List<SalaryAdvanceView> salaryAdvances;

            try
            {           
                var departmentIdParam = new SqlParameter { ParameterName = "departmentId", Value = model.SearchByBranchUnitDepartmentId };
                var sectionIdParam = new SqlParameter { ParameterName = "sectionId", Value = model.SearchByDepartmentSectionId};
                var lineIdParam = new SqlParameter { ParameterName = "lineId", Value = model.SearchByDepartmentLineId};
                var employeeCardIdParam = new SqlParameter { ParameterName = "employeeCardId", Value = model.SearchByEmployeeCardId ?? "0" };
                var employeeTypeId = new SqlParameter { ParameterName = "EmployeeTypeId", Value = model.SearchByEmployeeTypeId};
                var fromDateParam = new SqlParameter { ParameterName = "fromDate", Value = model.StartDate };
                var toDateParam = new SqlParameter { ParameterName = "toDate", Value = model.EndDate };

                salaryAdvances = Context.Database.SqlQuery<SalaryAdvanceView>("SPSalaryAdvance  @departmentId, @sectionId, @lineId, @employeeCardId, @EmployeeTypeId, @fromDate, @toDate",
                                                                                                departmentIdParam, sectionIdParam, lineIdParam, employeeCardIdParam, 
                                                                                                employeeTypeId, fromDateParam, toDateParam).ToList<SalaryAdvanceView>();
                    
                totalRecords = salaryAdvances.Count();
                     
                switch (salaryAdvance.sort)
                {
                    case "Amount":

                        switch (salaryAdvance.sortdir)
                        {
                            case "DESC":
                                salaryAdvances = salaryAdvances
                                    .OrderByDescending(r => r.Amount)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize).ToList();
                                break;

                            default:
                                salaryAdvances = salaryAdvances
                                    .OrderBy(r => r.Amount)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize).ToList();
                                break;
                        }
                        break;

                    case "ReceivedDate":

                        switch (salaryAdvance.sortdir)
                        {
                            case "DESC":
                                salaryAdvances = salaryAdvances
                                    .OrderByDescending(r => r.ReceivedDate)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize).ToList();
                                break;

                            default:
                                salaryAdvances = salaryAdvances
                                    .OrderBy(r => r.ReceivedDate)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize).ToList();
                                break;
                        }
                        break;

                    //case "EmployeeId":

                    //    switch (salaryAdvance.sortdir)
                    //    {
                    //        case "DESC":
                    //            salaryAdvances = salaryAdvances
                    //                .OrderByDescending(r => r.EmployeeId)
                    //                .Skip(startPage * pageSize)
                    //                .Take(pageSize).ToList();
                    //            break;

                    //        default:
                    //            salaryAdvances = salaryAdvances
                    //                .OrderBy(r => r.EmployeeId)
                    //                .Skip(startPage * pageSize)
                    //                .Take(pageSize).ToList();
                    //            break;
                    //    }
                    //    break;


                    default:
                        salaryAdvances = salaryAdvances.OrderBy(r => r.ReceivedDate).Skip(startPage * pageSize).Take(pageSize).ToList();
                        break;
                }
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }

            return salaryAdvances.ToList();
        }

        public List<SalaryAdvance> GetAllSalaryAdvances()
        {
            return Context.SalaryAdvance.Where(x => x.IsActive == true).OrderBy(y => y.ReceivedDate).ToList();
        }

        public List<SalaryAdvance> GetSalaryAdvanceBySearchKey(decimal searchByAmount, DateTime searchByFromDate, DateTime searchByToDate)
        {
            List<SalaryAdvance> salaryAdvances = null;

            try
            {
                //  SalaryAdvances = Context.SalaryAdvance.Where(x => x.IsActive == true && x.Amount == searchByAmount && x.FromDate <= searchByFromDate && x.ToDate >= searchByToDate).ToList();
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return salaryAdvances;
        }


        public List<EmployeesForAdvanceSalaryCustomModel> GetEmployeesForAdvanceSalary(SalaryAdvance model, SearchFieldModel searchFieldModel)
        {
            var listEmployeesForAdvanceSalary = new List<EmployeesForAdvanceSalaryCustomModel>();

            try
            {
                var employeeCardIdParam = !String.IsNullOrEmpty(searchFieldModel.SearchByEmployeeCardId) ?
                                           new ObjectParameter("employeeCardId", searchFieldModel.SearchByEmployeeCardId) :
                                           new ObjectParameter("employeeCardId", typeof(string));

                var companyIdParam = new ObjectParameter("companyId", searchFieldModel.SearchByCompanyId);

                var branchIdParam = new ObjectParameter("branchId", searchFieldModel.SearchByBranchId);

                var branchUnitIdParam = new ObjectParameter("branchUnitId", searchFieldModel.SearchByBranchUnitId);

                var branchUnitDepartmentIdParam = new ObjectParameter("branchUnitDepartmentId", searchFieldModel.SearchByBranchUnitDepartmentId);


                var sectionIdParam = (searchFieldModel.SearchBySectionId > 0) ?
                 new ObjectParameter("sectionId", searchFieldModel.SearchBySectionId) :
                 new ObjectParameter("sectionId", typeof(int));

                var lineIdParam = (searchFieldModel.SearchByLineId > 0) ?
                 new ObjectParameter("lineId", searchFieldModel.SearchByLineId) :
                 new ObjectParameter("lineId", typeof(int));

                var percentageParam = new ObjectParameter("Percentage", model.Percentage);

                var employeeTypeParam = new ObjectParameter("employeeTypeId", searchFieldModel.SearchByEmployeeTypeId);

                var receivedDateParam = new ObjectParameter("ReceivedDate", model.ReceivedDate);


                var spEmployeesForAdvanceSalary = Context.SPGetEmployeesForAdvanceSalary(employeeCardIdParam, companyIdParam, branchIdParam,
                                                                        branchUnitIdParam, branchUnitDepartmentIdParam, sectionIdParam,
                                                                        lineIdParam, percentageParam, employeeTypeParam, receivedDateParam);


                foreach (var employeesForAdvanceSalary in spEmployeesForAdvanceSalary)
                {
                    var employees = new EmployeesForAdvanceSalaryCustomModel
                    {
                        EmployeeId = employeesForAdvanceSalary.EmployeeId,
                        EmployeeCardId = employeesForAdvanceSalary.EmployeeCardId,
                        Department = employeesForAdvanceSalary.Department,
                        Section = employeesForAdvanceSalary.Section,
                        Line = employeesForAdvanceSalary.Line,
                        Name = employeesForAdvanceSalary.Name,
                        Designation = employeesForAdvanceSalary.Designation,
                        Grade = employeesForAdvanceSalary.Grade,
                        JoiningDate = employeesForAdvanceSalary.JoiningDate,
                        BasicSalary = employeesForAdvanceSalary.BasicSalary,
                        GrossSalary = employeesForAdvanceSalary.GrossSalary,
                        Amount = employeesForAdvanceSalary.Amount
                    };

                    listEmployeesForAdvanceSalary.Add(employees);
                }
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }

            return listEmployeesForAdvanceSalary;
        }


        public int SaveEmployeeSalaryAdvance(List<SalaryAdvance> salaryAdvances)
        {
            var saveChanges = 0;
            try
            {
                Context.SalaryAdvance.AddRange(salaryAdvances);
                saveChanges = Context.SaveChanges();
            }
            catch (Exception exception)
            {
            }

            return saveChanges;
        }

    }
}
