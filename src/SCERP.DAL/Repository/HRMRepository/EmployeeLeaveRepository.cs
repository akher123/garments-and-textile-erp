using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using SCERP.DAL.IRepository.IHRMRepository;
using SCERP.Model;
using System.Linq;
using SCERP.Common;
using System.Data;
using System.Data.SqlClient;
using SCERP.Model.HRMModel;
using SCERP.Model.Custom;

namespace SCERP.DAL.Repository.HRMRepository
{
    public class EmployeeLeaveRepository : Repository<EmployeeLeave>, IEmployeeLeaveRepository
    {
        private SqlConnection _connection;
        private readonly SCERPDBContext _context;

        public EmployeeLeaveRepository(SCERPDBContext context)
            : base(context)
        {
            _context = context;
            _connection = (SqlConnection)_context.Database.Connection;
        }

        public EmployeeLeave GetEmployeeLeaveById(int? id)
        {
            return Context.EmployeeLeaves.First(x => x.Id == id);
        }

        public override IQueryable<EmployeeLeave> All()
        {
            return Context.EmployeeLeaves.Where(x => x.IsActive == true).OrderBy(r => r.Id);
        }

        public List<EmployeeLeave> GetAllEmployeeLeavesBySearchKey(string employeeCardId, DateTime fromDate, DateTime toDate)
        {
            List<EmployeeLeave> EmployeeLeaves = null;

            try
            {
                EmployeeLeaves = Context.EmployeeLeaves.Where(
                    x =>
                        x.IsActive == true &&
                        ((x.Employee.EmployeeCardId.Replace(" ", "")
                            .ToLower()
                            .Contains(employeeCardId.Replace(" ", "").ToLower())) || String.IsNullOrEmpty(employeeCardId))
                        && (x.RecommendedFromDate >= fromDate || x.RecommendedToDate <= toDate)).Include(x => x.Employee).ToList();
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return EmployeeLeaves;
        }

        public List<EmployeeLeave> GetAllEmployeeLeavesByPaging(int startPage, int pageSize, out int totalRecords, EmployeeLeave employeeLeave)
        {

            IQueryable<EmployeeLeave> employeeLeaves = null;

            try
            {
                Guid employeeId = employeeLeave.EmployeeId;
                DateTime? recommendedFromDate = employeeLeave.RecommendedFromDate;
                DateTime? recommendedToDate = employeeLeave.RecommendedToDate;
                DateTime? appliedFromDate = employeeLeave.AppliedFromDate;
                DateTime? appliedToDate = employeeLeave.AppliedToDate;

                employeeLeaves = Context.EmployeeLeaves.Include(p => p.Employee).Include(p => p.LeaveType)
                    .Where(x => x.IsActive && (x.EmployeeId == employeeId || employeeId == Guid.Empty) && x.Employee.IsActive == true
                                && ((x.RecommendedFromDate >= recommendedFromDate || recommendedFromDate == null) || (x.AppliedFromDate >= appliedFromDate || appliedFromDate == null))
                                && ((x.RecommendedToDate <= recommendedToDate || recommendedToDate == null) || (x.AppliedToDate <= appliedToDate || appliedToDate == null)));

                totalRecords = employeeLeaves.Count();

                switch (employeeLeave.sort)
                {
                    case "Employee.Name":

                        switch (employeeLeave.sortdir)
                        {
                            case "DESC":
                                employeeLeaves = employeeLeaves
                                    .OrderByDescending(r => r.Employee.Name)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);

                                break;
                            default:
                                employeeLeaves = employeeLeaves
                                    .OrderBy(r => r.Employee.Name)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);

                                break;
                        }
                        break;

                    case "Employee.EmployeeCardId":

                        switch (employeeLeave.sortdir)
                        {
                            case "DESC":
                                employeeLeaves = employeeLeaves
                                    .OrderByDescending(r => r.Employee.EmployeeCardId)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);

                                break;
                            default:
                                employeeLeaves = employeeLeaves
                                    .OrderBy(r => r.Employee.EmployeeCardId)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);

                                break;
                        }
                        break;

                    case "LeaveType.Title":

                        switch (employeeLeave.sortdir)
                        {
                            case "DESC":
                                employeeLeaves = employeeLeaves
                                    .OrderByDescending(r => r.LeaveType.Title)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);

                                break;
                            default:
                                employeeLeaves = employeeLeaves
                                    .OrderBy(r => r.LeaveType.Title)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);

                                break;
                        }
                        break;

                    case "AppliedFromDate":

                        switch (employeeLeave.sortdir)
                        {
                            case "DESC":
                                employeeLeaves = employeeLeaves
                                    .OrderByDescending(r => r.AppliedFromDate)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);

                                break;
                            default:
                                employeeLeaves = employeeLeaves
                                    .OrderBy(r => r.AppliedFromDate)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);

                                break;
                        }
                        break;

                    case "AppliedToDate":

                        switch (employeeLeave.sortdir)
                        {
                            case "DESC":
                                employeeLeaves = employeeLeaves
                                    .OrderByDescending(r => r.AppliedToDate)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);

                                break;
                            default:
                                employeeLeaves = employeeLeaves
                                    .OrderBy(r => r.AppliedToDate)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);

                                break;
                        }
                        break;

                    case "AppliedTotalDays":

                        switch (employeeLeave.sortdir)
                        {
                            case "DESC":
                                employeeLeaves = employeeLeaves
                                    .OrderByDescending(r => r.AppliedTotalDays)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);

                                break;
                            default:
                                employeeLeaves = employeeLeaves
                                    .OrderBy(r => r.AppliedTotalDays)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);

                                break;
                        }
                        break;

                    case "RecommendedFromDate":

                        switch (employeeLeave.sortdir)
                        {
                            case "DESC":
                                employeeLeaves = employeeLeaves
                                    .OrderByDescending(r => r.RecommendedFromDate)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);

                                break;
                            default:
                                employeeLeaves = employeeLeaves
                                    .OrderBy(r => r.RecommendedFromDate)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);

                                break;
                        }
                        break;

                    case "RecommendedToDate":

                        switch (employeeLeave.sortdir)
                        {
                            case "DESC":
                                employeeLeaves = employeeLeaves
                                    .OrderByDescending(r => r.RecommendedToDate)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);

                                break;
                            default:
                                employeeLeaves = employeeLeaves
                                    .OrderBy(r => r.RecommendedToDate)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);

                                break;
                        }
                        break;

                    case "RecommendedTotalDays":

                        switch (employeeLeave.sortdir)
                        {
                            case "DESC":
                                employeeLeaves = employeeLeaves
                                    .OrderByDescending(r => r.RecommendedTotalDays)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);

                                break;
                            default:
                                employeeLeaves = employeeLeaves
                                    .OrderBy(r => r.RecommendedTotalDays)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);

                                break;
                        }
                        break;

                    case "RecommendationStatus":

                        switch (employeeLeave.sortdir)
                        {
                            case "DESC":
                                employeeLeaves = employeeLeaves
                                    .OrderByDescending(r => r.RecommendationStatus)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);

                                break;
                            default:
                                employeeLeaves = employeeLeaves
                                    .OrderBy(r => r.RecommendationStatus)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);

                                break;
                        }
                        break;

                    case "ApprovedFromDate":

                        switch (employeeLeave.sortdir)
                        {
                            case "DESC":
                                employeeLeaves = employeeLeaves
                                    .OrderByDescending(r => r.ApprovedFromDate)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);

                                break;
                            default:
                                employeeLeaves = employeeLeaves
                                    .OrderBy(r => r.ApprovedFromDate)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);

                                break;
                        }
                        break;

                    case "ApprovedToDate":

                        switch (employeeLeave.sortdir)
                        {
                            case "DESC":
                                employeeLeaves = employeeLeaves
                                    .OrderByDescending(r => r.ApprovedToDate)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);

                                break;
                            default:
                                employeeLeaves = employeeLeaves
                                    .OrderBy(r => r.ApprovedToDate)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);

                                break;
                        }
                        break;

                    case "ApprovedTotalDays":

                        switch (employeeLeave.sortdir)
                        {
                            case "DESC":
                                employeeLeaves = employeeLeaves
                                    .OrderByDescending(r => r.ApprovedTotalDays)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);

                                break;
                            default:
                                employeeLeaves = employeeLeaves
                                    .OrderBy(r => r.ApprovedTotalDays)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);

                                break;
                        }
                        break;

                    case "ApprovalStatus":

                        switch (employeeLeave.sortdir)
                        {
                            case "DESC":
                                employeeLeaves = employeeLeaves
                                    .OrderByDescending(r => r.ApprovalStatus)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);

                                break;
                            default:
                                employeeLeaves = employeeLeaves
                                    .OrderBy(r => r.ApprovalStatus)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);

                                break;
                        }
                        break;

                    default:

                        switch (employeeLeave.sortdir)
                        {
                            case "ASC":
                                employeeLeaves = employeeLeaves
                                    .OrderBy(r => r.AppliedFromDate)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);

                                break;
                            default:
                                employeeLeaves = employeeLeaves
                                    .OrderByDescending(r => r.AppliedFromDate)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);

                                break;
                        }
                        break;
                }
            }
            catch (Exception exception)
            {
                totalRecords = 0;
                throw new Exception(exception.Message);
            }

            return employeeLeaves.ToList();
        }

        public List<EmployeeLeave> GetAllRecommendedEmployeeLeavesByPaging(int startPage, int pageSize, out int totalRecords, EmployeeLeave employeeLeave)
        {
            int recommandedStatusId = Convert.ToInt32(LeaveRecommendation.Recommended);


            IQueryable<EmployeeLeave> employeeLeaves = null;

            try
            {
                Guid employeeId = employeeLeave.EmployeeId;
                DateTime? appliedFromDate = employeeLeave.AppliedFromDate;
                DateTime? appliedToDate = employeeLeave.AppliedToDate;
                DateTime? recommendedFromDate = employeeLeave.RecommendedFromDate;
                DateTime? recommendedToDate = employeeLeave.RecommendedToDate;
                DateTime? approvedFromDate = employeeLeave.ApprovedFromDate;
                DateTime? approvedToDate = employeeLeave.ApprovedToDate;
                Guid? approvalPersonId = employeeLeave.ApprovalPerson;

                employeeLeaves = Context.EmployeeLeaves.Include(p => p.Employee).Include(p => p.LeaveType)
                    .Where(x => x.IsActive
                          && (x.EmployeeId == employeeId || employeeId == Guid.Empty)
                          && (x.AppliedFromDate >= appliedFromDate || appliedFromDate == null)
                          && (x.AppliedToDate <= appliedToDate || appliedToDate == null)
                          && (x.RecommendedFromDate >= recommendedFromDate || recommendedFromDate == null)
                          && (x.RecommendedToDate <= recommendedToDate || recommendedToDate == null)
                          && (x.ApprovedFromDate >= approvedFromDate || approvedFromDate == null)
                          && (x.ApprovedToDate <= approvedToDate || approvedToDate == null)
                          && (x.ApprovalPerson == approvalPersonId)
                          && (x.RecommendationStatus == recommandedStatusId)
                          );

                totalRecords = employeeLeaves.Count();

                switch (employeeLeave.sort)
                {
                    case "Employee.Name":

                        switch (employeeLeave.sortdir)
                        {
                            case "DESC":
                                employeeLeaves = employeeLeaves
                                    .OrderByDescending(r => r.Employee.Name)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);

                                break;
                            default:
                                employeeLeaves = employeeLeaves
                                    .OrderBy(r => r.Employee.Name)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);

                                break;
                        }
                        break;

                    case "Employee.EmployeeCardId":

                        switch (employeeLeave.sortdir)
                        {
                            case "DESC":
                                employeeLeaves = employeeLeaves
                                    .OrderByDescending(r => r.Employee.EmployeeCardId)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);

                                break;
                            default:
                                employeeLeaves = employeeLeaves
                                    .OrderBy(r => r.Employee.EmployeeCardId)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);

                                break;
                        }
                        break;

                    case "LeaveType.Title":

                        switch (employeeLeave.sortdir)
                        {
                            case "DESC":
                                employeeLeaves = employeeLeaves
                                    .OrderByDescending(r => r.LeaveType.Title)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);

                                break;
                            default:
                                employeeLeaves = employeeLeaves
                                    .OrderBy(r => r.LeaveType.Title)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);

                                break;
                        }
                        break;

                    case "AppliedFromDate":

                        switch (employeeLeave.sortdir)
                        {
                            case "DESC":
                                employeeLeaves = employeeLeaves
                                    .OrderByDescending(r => r.AppliedFromDate)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);

                                break;
                            default:
                                employeeLeaves = employeeLeaves
                                    .OrderBy(r => r.AppliedFromDate)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);

                                break;
                        }
                        break;

                    case "AppliedToDate":

                        switch (employeeLeave.sortdir)
                        {
                            case "DESC":
                                employeeLeaves = employeeLeaves
                                    .OrderByDescending(r => r.AppliedToDate)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);

                                break;
                            default:
                                employeeLeaves = employeeLeaves
                                    .OrderBy(r => r.AppliedToDate)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);

                                break;
                        }
                        break;

                    case "AppliedTotalDays":

                        switch (employeeLeave.sortdir)
                        {
                            case "DESC":
                                employeeLeaves = employeeLeaves
                                    .OrderByDescending(r => r.AppliedTotalDays)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);

                                break;
                            default:
                                employeeLeaves = employeeLeaves
                                    .OrderBy(r => r.AppliedTotalDays)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);

                                break;
                        }
                        break;

                    case "RecommendedFromDate":

                        switch (employeeLeave.sortdir)
                        {
                            case "DESC":
                                employeeLeaves = employeeLeaves
                                    .OrderByDescending(r => r.RecommendedFromDate)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);

                                break;
                            default:
                                employeeLeaves = employeeLeaves
                                    .OrderBy(r => r.RecommendedFromDate)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);

                                break;
                        }
                        break;

                    case "RecommendedToDate":

                        switch (employeeLeave.sortdir)
                        {
                            case "DESC":
                                employeeLeaves = employeeLeaves
                                    .OrderByDescending(r => r.RecommendedToDate)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);

                                break;
                            default:
                                employeeLeaves = employeeLeaves
                                    .OrderBy(r => r.RecommendedToDate)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);

                                break;
                        }
                        break;

                    case "RecommendedTotalDays":

                        switch (employeeLeave.sortdir)
                        {
                            case "DESC":
                                employeeLeaves = employeeLeaves
                                    .OrderByDescending(r => r.RecommendedTotalDays)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);

                                break;
                            default:
                                employeeLeaves = employeeLeaves
                                    .OrderBy(r => r.RecommendedTotalDays)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);

                                break;
                        }
                        break;

                    case "RecommendationStatus":

                        switch (employeeLeave.sortdir)
                        {
                            case "DESC":
                                employeeLeaves = employeeLeaves
                                    .OrderByDescending(r => r.RecommendationStatus)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);

                                break;
                            default:
                                employeeLeaves = employeeLeaves
                                    .OrderBy(r => r.RecommendationStatus)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);

                                break;
                        }
                        break;

                    case "ApprovedFromDate":

                        switch (employeeLeave.sortdir)
                        {
                            case "DESC":
                                employeeLeaves = employeeLeaves
                                    .OrderByDescending(r => r.ApprovedFromDate)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);

                                break;
                            default:
                                employeeLeaves = employeeLeaves
                                    .OrderBy(r => r.ApprovedFromDate)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);

                                break;
                        }
                        break;

                    case "ApprovedToDate":

                        switch (employeeLeave.sortdir)
                        {
                            case "DESC":
                                employeeLeaves = employeeLeaves
                                    .OrderByDescending(r => r.ApprovedToDate)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);

                                break;
                            default:
                                employeeLeaves = employeeLeaves
                                    .OrderBy(r => r.ApprovedToDate)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);

                                break;
                        }
                        break;

                    case "ApprovedTotalDays":

                        switch (employeeLeave.sortdir)
                        {
                            case "DESC":
                                employeeLeaves = employeeLeaves
                                    .OrderByDescending(r => r.ApprovedTotalDays)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);

                                break;
                            default:
                                employeeLeaves = employeeLeaves
                                    .OrderBy(r => r.ApprovedTotalDays)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);

                                break;
                        }
                        break;

                    case "ApprovalStatus":

                        switch (employeeLeave.sortdir)
                        {
                            case "DESC":
                                employeeLeaves = employeeLeaves
                                    .OrderByDescending(r => r.ApprovalStatus)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);

                                break;
                            default:
                                employeeLeaves = employeeLeaves
                                    .OrderBy(r => r.ApprovalStatus)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);

                                break;
                        }
                        break;

                    default:

                        switch (employeeLeave.sortdir)
                        {
                            case "ASC":
                                employeeLeaves = employeeLeaves
                                    .OrderBy(r => r.AppliedFromDate)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);

                                break;
                            default:
                                employeeLeaves = employeeLeaves
                                    .OrderByDescending(r => r.AppliedFromDate)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);

                                break;
                        }
                        break;
                }
            }
            catch (Exception exception)
            {
                totalRecords = 0;
                throw new Exception(exception.Message);
            }

            return employeeLeaves.ToList();
        }

        public List<EmployeeLeave> GetAllAppliedEmployeeLeavesByPaging(int startPage, int pageSize, out int totalRecords, EmployeeLeave employeeLeave)
        {

            IQueryable<EmployeeLeave> employeeLeaves = null;

            try
            {
                Guid employeeId = employeeLeave.EmployeeId;
                DateTime? appliedFromDate = employeeLeave.AppliedFromDate;
                DateTime? appliedToDate = employeeLeave.AppliedToDate;
                DateTime? recommendedFromDate = employeeLeave.RecommendedFromDate;
                DateTime? recommendedToDate = employeeLeave.RecommendedToDate;
                DateTime? approvedFromDate = employeeLeave.ApprovedFromDate;
                DateTime? approvedToDate = employeeLeave.ApprovedToDate;
                Guid? recommendedPersonId = employeeLeave.RecommendationPerson;

                employeeLeaves = Context.EmployeeLeaves.Include(p => p.Employee).Include(p => p.LeaveType)
                    .Where(x => x.IsActive
                          && (x.EmployeeId == employeeId || employeeId == Guid.Empty)
                          && (x.AppliedFromDate >= appliedFromDate || appliedFromDate == null)
                          && (x.AppliedToDate <= appliedToDate || appliedToDate == null)
                          && (x.RecommendedFromDate >= recommendedFromDate || recommendedFromDate == null)
                          && (x.RecommendedToDate <= recommendedToDate || recommendedToDate == null)
                          && (x.ApprovedFromDate >= approvedFromDate || approvedFromDate == null)
                          && (x.ApprovedToDate <= approvedToDate || approvedToDate == null)
                          && (x.RecommendationPerson == recommendedPersonId || recommendedPersonId == null)
                          );

                totalRecords = employeeLeaves.Count();

                switch (employeeLeave.sort)
                {
                    case "Employee.Name":

                        switch (employeeLeave.sortdir)
                        {
                            case "DESC":
                                employeeLeaves = employeeLeaves
                                    .OrderByDescending(r => r.Employee.Name)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);

                                break;
                            default:
                                employeeLeaves = employeeLeaves
                                    .OrderBy(r => r.Employee.Name)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);

                                break;
                        }
                        break;

                    case "Employee.EmployeeCardId":

                        switch (employeeLeave.sortdir)
                        {
                            case "DESC":
                                employeeLeaves = employeeLeaves
                                    .OrderByDescending(r => r.Employee.EmployeeCardId)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);

                                break;
                            default:
                                employeeLeaves = employeeLeaves
                                    .OrderBy(r => r.Employee.EmployeeCardId)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);

                                break;
                        }
                        break;

                    case "LeaveType.Title":

                        switch (employeeLeave.sortdir)
                        {
                            case "DESC":
                                employeeLeaves = employeeLeaves
                                    .OrderByDescending(r => r.LeaveType.Title)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);

                                break;
                            default:
                                employeeLeaves = employeeLeaves
                                    .OrderBy(r => r.LeaveType.Title)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);

                                break;
                        }
                        break;

                    case "AppliedFromDate":

                        switch (employeeLeave.sortdir)
                        {
                            case "DESC":
                                employeeLeaves = employeeLeaves
                                    .OrderByDescending(r => r.AppliedFromDate)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);

                                break;
                            default:
                                employeeLeaves = employeeLeaves
                                    .OrderBy(r => r.AppliedFromDate)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);

                                break;
                        }
                        break;

                    case "AppliedToDate":

                        switch (employeeLeave.sortdir)
                        {
                            case "DESC":
                                employeeLeaves = employeeLeaves
                                    .OrderByDescending(r => r.AppliedToDate)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);

                                break;
                            default:
                                employeeLeaves = employeeLeaves
                                    .OrderBy(r => r.AppliedToDate)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);

                                break;
                        }
                        break;

                    case "AppliedTotalDays":

                        switch (employeeLeave.sortdir)
                        {
                            case "DESC":
                                employeeLeaves = employeeLeaves
                                    .OrderByDescending(r => r.AppliedTotalDays)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);

                                break;
                            default:
                                employeeLeaves = employeeLeaves
                                    .OrderBy(r => r.AppliedTotalDays)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);

                                break;
                        }
                        break;

                    case "RecommendedFromDate":

                        switch (employeeLeave.sortdir)
                        {
                            case "DESC":
                                employeeLeaves = employeeLeaves
                                    .OrderByDescending(r => r.RecommendedFromDate)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);

                                break;
                            default:
                                employeeLeaves = employeeLeaves
                                    .OrderBy(r => r.RecommendedFromDate)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);

                                break;
                        }
                        break;

                    case "RecommendedToDate":

                        switch (employeeLeave.sortdir)
                        {
                            case "DESC":
                                employeeLeaves = employeeLeaves
                                    .OrderByDescending(r => r.RecommendedToDate)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);

                                break;
                            default:
                                employeeLeaves = employeeLeaves
                                    .OrderBy(r => r.RecommendedToDate)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);

                                break;
                        }
                        break;

                    case "RecommendedTotalDays":

                        switch (employeeLeave.sortdir)
                        {
                            case "DESC":
                                employeeLeaves = employeeLeaves
                                    .OrderByDescending(r => r.RecommendedTotalDays)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);

                                break;
                            default:
                                employeeLeaves = employeeLeaves
                                    .OrderBy(r => r.RecommendedTotalDays)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);

                                break;
                        }
                        break;

                    case "RecommendationStatus":

                        switch (employeeLeave.sortdir)
                        {
                            case "DESC":
                                employeeLeaves = employeeLeaves
                                    .OrderByDescending(r => r.RecommendationStatus)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);

                                break;
                            default:
                                employeeLeaves = employeeLeaves
                                    .OrderBy(r => r.RecommendationStatus)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);

                                break;
                        }
                        break;

                    case "ApprovedFromDate":

                        switch (employeeLeave.sortdir)
                        {
                            case "DESC":
                                employeeLeaves = employeeLeaves
                                    .OrderByDescending(r => r.ApprovedFromDate)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);

                                break;
                            default:
                                employeeLeaves = employeeLeaves
                                    .OrderBy(r => r.ApprovedFromDate)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);

                                break;
                        }
                        break;

                    case "ApprovedToDate":

                        switch (employeeLeave.sortdir)
                        {
                            case "DESC":
                                employeeLeaves = employeeLeaves
                                    .OrderByDescending(r => r.ApprovedToDate)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);

                                break;
                            default:
                                employeeLeaves = employeeLeaves
                                    .OrderBy(r => r.ApprovedToDate)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);

                                break;
                        }
                        break;

                    case "ApprovedTotalDays":

                        switch (employeeLeave.sortdir)
                        {
                            case "DESC":
                                employeeLeaves = employeeLeaves
                                    .OrderByDescending(r => r.ApprovedTotalDays)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);

                                break;
                            default:
                                employeeLeaves = employeeLeaves
                                    .OrderBy(r => r.ApprovedTotalDays)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);

                                break;
                        }
                        break;

                    case "ApprovalStatus":

                        switch (employeeLeave.sortdir)
                        {
                            case "DESC":
                                employeeLeaves = employeeLeaves
                                    .OrderByDescending(r => r.ApprovalStatus)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);

                                break;
                            default:
                                employeeLeaves = employeeLeaves
                                    .OrderBy(r => r.ApprovalStatus)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);

                                break;
                        }
                        break;

                    default:

                        switch (employeeLeave.sortdir)
                        {
                            case "ASC":
                                employeeLeaves = employeeLeaves
                                    .OrderBy(r => r.AppliedFromDate)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);

                                break;
                            default:
                                employeeLeaves = employeeLeaves
                                    .OrderByDescending(r => r.AppliedFromDate)
                                    .Skip(startPage * pageSize)
                                    .Take(pageSize);

                                break;
                        }
                        break;
                }
            }
            catch (Exception exception)
            {
                totalRecords = 0;
                throw new Exception(exception.Message);
            }

            return employeeLeaves.ToList();
        }

        public List<EmployeeLeave> GetAllEmployeeLeaves()
        {
            var employeeLeaveList = Context.EmployeeLeaves.Where(x => x.IsActive == true).OrderBy(r => r.Id).ToList();
            return employeeLeaveList;
        }

        public IQueryable<LeaveType> GetAllLeaveType()
        {
            return Context.LeaveTypes.Where(x => x.IsActive == true);
        }

        public List<string> GetEmployeeData(string employeeCardId)
        {
            var employeeData = new List<string>();

            if (Context.Employees.Count(p => p.EmployeeCardId == employeeCardId && p.IsActive) != 1)
            {
                return null;
            }

            var singleOrDefault = Context.Employees.SingleOrDefault(p => p.EmployeeCardId == employeeCardId && p.IsActive);
            if (singleOrDefault == null) return null;
            var employeeId = singleOrDefault.EmployeeId;

            var table = new DataTable();
            const string cmdText = @"SPEmployeeInfo";
            SqlCommand command = new SqlCommand(cmdText, _connection);

            command.Parameters.AddWithValue("@employeeId", employeeId);
            command.CommandType = CommandType.StoredProcedure;

            try
            {
                if (_connection != null && _connection.State == ConnectionState.Closed)
                {
                    _connection.Open();
                    var adapter = new SqlDataAdapter(command);
                    adapter.Fill(table);

                    DataRow emp = table.Rows[0];

                    if (emp != null)
                    {
                        employeeData.Add(emp.Field<string>("Name"));
                        employeeData.Add(emp.Field<string>("CompanyName"));
                        employeeData.Add(emp.Field<string>("Branch"));
                        employeeData.Add(emp.Field<string>("Unit"));
                        employeeData.Add(emp.Field<string>("Department"));
                        employeeData.Add(emp.Field<string>("Designation"));

                        employeeData.Add(emp.Field<string>("Grade"));
                        employeeData.Add(emp.Field<string>("Line"));
                        employeeData.Add(emp.Field<string>("Section"));
                        employeeData.Add(emp.Field<string>("EmployeeType"));

                        employeeData.Add(emp.Field<string>("PreMailingAddress"));
                        employeeData.Add(emp.Field<string>("MobilePhone"));
                        employeeData.Add(emp.Field<string>("JoiningDate"));
                    }
                }
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                table = null;
            }
            finally
            {
                if (_connection != null && _connection.State == ConnectionState.Open) _connection.Close();
            }

            return employeeData;
        }

        public List<EmployeeLeaveData> GetEmployeeLeaveData(string employeeCardId, int year)
        {
            if (Context.Employees.Count(p => p.EmployeeCardId == employeeCardId && p.IsActive) != 1)
                return null;

            var singleOrDefault = Context.Employees.SingleOrDefault(p => p.EmployeeCardId == employeeCardId && p.IsActive);
            if (singleOrDefault == null) return null;
            var employeeId = singleOrDefault.EmployeeId;

            var employeeDetail = Context.EmployeeCompanyInfos.Where(x => x.EmployeeId == employeeId && x.FromDate <= DateTime.Now)
                .OrderByDescending(x => x.FromDate)
                .FirstOrDefault();

            if (employeeDetail == null) return null;


            var branchUnitId = 0;
            var branchUnitDepartment = Context.BranchUnitDepartments.SingleOrDefault(p => p.BranchUnitDepartmentId == employeeDetail.BranchUnitDepartmentId);
            if (branchUnitDepartment != null)
            {
                branchUnitId = branchUnitDepartment.BranchUnitId;
            }

            var employeeTypeId = Context.EmployeeCompanyInfos.Include(p => p.EmployeeDesignation)
                                .OrderByDescending(p => p.FromDate)
                                .First(p => p.EmployeeId == employeeId && p.FromDate <= DateTime.Now && p.EmployeeDesignation.IsActive == true && p.IsActive == true)
                                .EmployeeDesignation.EmployeeTypeId;


            var employeeIdParam = new SqlParameter { ParameterName = "EmployeeId", Value = employeeId };
            var employeeCardIdParam = new SqlParameter { ParameterName = "EmployeeCardId", Value = employeeCardId };
            var yearParam = new SqlParameter { ParameterName = "Year", Value = year };
            var branchUnitIdParam = new SqlParameter { ParameterName = "@BranchUnitId", Value = branchUnitId };
            var employeeTypeIdParam = new SqlParameter { ParameterName = "EmployeeTypeId", Value = employeeTypeId };

            return Context.Database.SqlQuery<EmployeeLeaveData>("HRMSPGetIndividualLeaveHistoryForSpecificYear @EmployeeId, @EmployeeCardId, @Year, @BranchUnitId,@EmployeeTypeId",
                                                                 employeeIdParam, employeeCardIdParam, yearParam, branchUnitIdParam, employeeTypeIdParam).ToList();

        }

        public bool CheckLeaveValidity(Guid employeeId, string employeeCardId, int year, int leaveTypeId, int appliedTotalDays)
        {
            try
            {
                var employeeDetail = Context.EmployeeCompanyInfos.Where(x => x.EmployeeId == employeeId && x.FromDate <= DateTime.Now)
                    .OrderByDescending(x => x.FromDate)
                    .FirstOrDefault();

                if (employeeDetail == null) return false;


                var branchUnitId = 0;
                var branchUnitDepartment = Context.BranchUnitDepartments.SingleOrDefault(p => p.BranchUnitDepartmentId == employeeDetail.BranchUnitDepartmentId);
                if (branchUnitDepartment != null)
                {
                    branchUnitId = branchUnitDepartment.BranchUnitId;
                }

                var employeeTypeId = Context.EmployeeCompanyInfos.Include(p => p.EmployeeDesignation)
                                    .OrderByDescending(p => p.FromDate)
                                    .First(p => p.EmployeeId == employeeId && p.FromDate <= DateTime.Now && p.EmployeeDesignation.IsActive == true && p.IsActive == true)
                                    .EmployeeDesignation.EmployeeTypeId;


                var employeeIdParam = new SqlParameter { ParameterName = "EmployeeId", Value = employeeId };
                var employeeCardIdParam = new SqlParameter { ParameterName = "EmployeeCardId", Value = employeeCardId };
                var yearParam = new SqlParameter { ParameterName = "Year", Value = year };
                var branchUnitIdParam = new SqlParameter { ParameterName = "BranchUnitId", Value = branchUnitId };
                var employeeTypeIdParam = new SqlParameter { ParameterName = "EmployeeTypeId", Value = employeeTypeId };
                var leaveTypeIdParam = new SqlParameter { ParameterName = "LeaveTypeId", Value = leaveTypeId };

                int? availableDays = 0;

                var employeeLeaveData = Context.Database.SqlQuery<EmployeeLeaveData>("HRMSPGetEmployeeAvailableLeaveOfSpecificLeaveType @EmployeeId, @EmployeeCardId, @Year, @BranchUnitId, @EmployeeTypeId, @LeaveTypeId",
                    employeeIdParam, employeeCardIdParam, yearParam, branchUnitIdParam, employeeTypeIdParam, leaveTypeIdParam).SingleOrDefault();

                if (employeeLeaveData != null)
                    availableDays = employeeLeaveData.Available;

                if (appliedTotalDays > availableDays)
                    return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }

        public int SaveEmployeeLeave(EmployeeLeave employeeLeave)
        {
            try
            {
                Guid employeeId = employeeLeave.EmployeeId;
                if (employeeId == null) return 0;
                var employeeIdParam = new SqlParameter { ParameterName = "EmployeeId", Value = employeeId };

                string employeeCardId = employeeLeave.EmployeeCardId;
                if (String.IsNullOrEmpty(employeeCardId)) return 0;
                var employeeCardIdParam = new SqlParameter { ParameterName = "EmployeeCardId", Value = employeeCardId };

                int? leaveTypeId = employeeLeave.LeaveTypeId;
                if (leaveTypeId == null || leaveTypeId <= 0) return 0;
                var leaveTypeIdParam = new SqlParameter { ParameterName = "LeaveTypeId", Value = leaveTypeId };

                DateTime? submitDate = employeeLeave.SubmitDate;
                if (submitDate == null) return 0;
                var submitDateParam = new SqlParameter { ParameterName = "SubmitDate", Value = submitDate };

                DateTime? appliedFromDate = employeeLeave.AppliedFromDate;
                if (appliedFromDate == null) return 0;
                var appliedFromDateParam = new SqlParameter { ParameterName = "AppliedFromDate", Value = appliedFromDate };

                DateTime? appliedToDate = employeeLeave.AppliedToDate;
                if (appliedToDate == null) return 0;
                var appliedToDateParam = new SqlParameter { ParameterName = "AppliedToDate", Value = appliedToDate };

                int? appliedTotalDays = employeeLeave.AppliedTotalDays;
                if (appliedTotalDays == null || appliedTotalDays <= 0) return 0;
                var appliedTotalDaysParam = new SqlParameter { ParameterName = "AppliedTotalDays", Value = appliedTotalDays };

                string leavePurpose = employeeLeave.LeavePurpose;
                if (String.IsNullOrEmpty(leavePurpose)) return 0;
                var leavePurposeParam = new SqlParameter { ParameterName = "LeavePurpose", Value = leavePurpose };

                string emergencyPhoneNo = employeeLeave.EmergencyPhoneNo;
                if (String.IsNullOrEmpty(emergencyPhoneNo)) return 0;
                var emergencyPhoneNoParam = new SqlParameter { ParameterName = "EmergencyPhoneNo", Value = emergencyPhoneNo };

                string addressDuringLeave = employeeLeave.AddressDuringLeave;
                if (String.IsNullOrEmpty(addressDuringLeave)) return 0;
                var addressDuringLeaveParam = new SqlParameter { ParameterName = "AddressDuringLeave", Value = addressDuringLeave };

                DateTime? recommendedFromDate = new DateTime(1900, 01, 01);
                if (employeeLeave.RecommendedFromDate != null)
                    recommendedFromDate = employeeLeave.RecommendedFromDate;
                var recommendedFromDateParam = new SqlParameter { ParameterName = "RecommendedFromDate", Value = recommendedFromDate };

                DateTime? recommendedToDate = new DateTime(1900, 01, 01);
                if (employeeLeave.RecommendedToDate != null)
                    recommendedToDate = employeeLeave.RecommendedToDate;
                var recommendedToDateParam = new SqlParameter { ParameterName = "RecommendedToDate", Value = recommendedToDate };

                int? recommendedTotalDays = -1;
                if (employeeLeave.RecommendedTotalDays > 0)
                    recommendedTotalDays = employeeLeave.RecommendedTotalDays;
                var recommendedTotalDaysParam = new SqlParameter { ParameterName = "RecommendedTotalDays", Value = recommendedTotalDays };

                int? recommendationStatus = -1;
                if (employeeLeave.RecommendationStatus > 0)
                    recommendationStatus = employeeLeave.RecommendationStatus;
                var recommendationStatusParam = new SqlParameter { ParameterName = "RecommendationStatus", Value = recommendationStatus };

                DateTime? recommendationStatusDate = new DateTime(1900, 01, 01);
                if (employeeLeave.RecommendationStatusDate != null)
                    recommendationStatusDate = employeeLeave.RecommendationStatusDate;
                var recommendationStatusDateParam = new SqlParameter { ParameterName = "RecommendationStatusDate", Value = recommendationStatusDate };

                Guid? recommendationPerson = Guid.Empty;
                if (employeeLeave.RecommendationPerson != null)
                    recommendationPerson = employeeLeave.RecommendationPerson;
                var recommendationPersonParam = new SqlParameter { ParameterName = "RecommendationPerson", Value = recommendationPerson };

                string recommendationComment = string.Empty;
                if (!String.IsNullOrEmpty(employeeLeave.RecommendationComment))
                    recommendationComment = employeeLeave.RecommendationComment;
                var recommendationCommentParam = new SqlParameter { ParameterName = "RecommendationComment", Value = recommendationComment };

                DateTime? approvedFromDate = new DateTime(1900, 01, 01);
                if (employeeLeave.ApprovedFromDate != null)
                    approvedFromDate = employeeLeave.ApprovedFromDate;
                var approvedFromDateParam = new SqlParameter { ParameterName = "ApprovedFromDate", Value = approvedFromDate };

                DateTime? approvedToDate = new DateTime(1900, 01, 01);
                if (employeeLeave.ApprovedToDate != null)
                    approvedToDate = employeeLeave.ApprovedToDate;
                var approvedToDateParam = new SqlParameter { ParameterName = "ApprovedToDate", Value = approvedToDate };

                int? approvedTotalDays = -1;
                if (employeeLeave.ApprovedTotalDays > 0)
                    approvedTotalDays = employeeLeave.ApprovedTotalDays;
                var approvedTotalDaysParam = new SqlParameter { ParameterName = "ApprovedTotalDays", Value = approvedTotalDays };

                int? approvalStatus = -1;
                if (employeeLeave.ApprovalStatus > 0)
                    approvalStatus = employeeLeave.ApprovalStatus;
                var approvalStatusParam = new SqlParameter { ParameterName = "ApprovalStatus", Value = approvalStatus };

                DateTime? approvalStatusDate = new DateTime(1900, 01, 01);
                if (employeeLeave.ApprovalStatusDate != null)
                    approvalStatusDate = employeeLeave.ApprovalStatusDate;
                var approvalStatusDateParam = new SqlParameter { ParameterName = "ApprovalStatusDate", Value = approvalStatusDate };

                Guid? approvalPerson = Guid.Empty;
                if (employeeLeave.ApprovalPerson != null)
                    approvalPerson = employeeLeave.ApprovalPerson;
                var approvalPersonParam = new SqlParameter { ParameterName = "ApprovalPerson", Value = approvalPerson };

                string approvalComment = string.Empty;
                if (!String.IsNullOrEmpty(employeeLeave.ApprovalComment))
                    approvalComment = employeeLeave.ApprovalComment;
                var approvalCommentParam = new SqlParameter { ParameterName = "ApprovalComment", Value = approvalComment };

                DateTime? consumedFromDate = new DateTime(1900, 01, 01);
                if (employeeLeave.ConsumedFromDate != null)
                    consumedFromDate = employeeLeave.ConsumedFromDate;
                var consumedFromDateParam = new SqlParameter { ParameterName = "ConsumedFromDate", Value = consumedFromDate };

                DateTime? consumedToDate = new DateTime(1900, 01, 01);
                if (employeeLeave.ConsumedToDate != null)
                    consumedToDate = employeeLeave.ConsumedToDate;
                var consumedToDateParam = new SqlParameter { ParameterName = "ConsumedToDate", Value = consumedToDate };

                int? consumedTotalDays = -1;
                if (employeeLeave.ConsumedTotalDays > 0)
                    consumedTotalDays = employeeLeave.ConsumedTotalDays;
                var consumedTotalDaysParam = new SqlParameter { ParameterName = "ConsumedTotalDays", Value = consumedTotalDays };

                int? joinedBeforeDays = 0;
                if (employeeLeave.JoinedBeforeDays > 0)
                    joinedBeforeDays = employeeLeave.JoinedBeforeDays;
                var joinedBeforeDaysParam = new SqlParameter { ParameterName = "JoinedBeforeDays", Value = joinedBeforeDays };

                DateTime? resumeDate = new DateTime(1900, 01, 01);
                if (employeeLeave.ResumeDate != null)
                    resumeDate = employeeLeave.ResumeDate;
                var resumeDateParam = new SqlParameter { ParameterName = "ResumeDate", Value = resumeDate };

                DateTime? createdDate = new DateTime(1900, 01, 01);
                if (employeeLeave.CreatedDate != null)
                    createdDate = employeeLeave.CreatedDate;
                var createdDateParam = new SqlParameter { ParameterName = "CreatedDate", Value = createdDate };

                Guid? createdBy = Guid.Empty;
                if (employeeLeave.CreatedBy != null)
                    createdBy = employeeLeave.CreatedBy;
                var createdByParam = new SqlParameter { ParameterName = "CreatedBy", Value = createdBy };

                bool isActive = false;
                if (employeeLeave.IsActive)
                    isActive = employeeLeave.IsActive;
                var isActiveParam = new SqlParameter { ParameterName = "IsActive", Value = isActive };

                var employeeLeaveSaveStatus = Context.Database.SqlQuery<int>("HRMSPSaveEmployeeLeave  @EmployeeId, @EmployeeCardId, @LeaveTypeId, @SubmitDate, " +
                    "@AppliedFromDate, @AppliedToDate, @AppliedTotalDays, @LeavePurpose, @EmergencyPhoneNo, @AddressDuringLeave, @RecommendedFromDate, " +
                    "@RecommendedToDate, @RecommendedTotalDays, @RecommendationStatus, @RecommendationStatusDate, @RecommendationPerson, " +
                    "@RecommendationComment, @ApprovedFromDate, @ApprovedToDate, @ApprovedTotalDays, @ApprovalStatus, @ApprovalStatusDate, @ApprovalPerson, " +
                    "@ApprovalComment, @ConsumedFromDate, @ConsumedToDate, @ConsumedTotalDays, @JoinedBeforeDays, @ResumeDate, @CreatedDate, " +
                    "@CreatedBy, @IsActive", employeeIdParam, employeeCardIdParam, leaveTypeIdParam, submitDateParam, appliedFromDateParam, appliedToDateParam,
                    appliedTotalDaysParam, leavePurposeParam, emergencyPhoneNoParam, addressDuringLeaveParam, recommendedFromDateParam,
                    recommendedToDateParam, recommendedTotalDaysParam, recommendationStatusParam, recommendationStatusDateParam, recommendationPersonParam,
                    recommendationCommentParam, approvedFromDateParam, approvedToDateParam, approvedTotalDaysParam, approvalStatusParam, approvalStatusDateParam, approvalPersonParam,
                    approvalCommentParam, consumedFromDateParam, consumedToDateParam, consumedTotalDaysParam, joinedBeforeDaysParam, resumeDateParam, createdDateParam,
                    createdByParam, isActiveParam).ToList()[0];

                return employeeLeaveSaveStatus;


            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }

        public int UpdateEmployeeLeave(EmployeeLeave employeeLeave)
        {
            try
            {
                int? employeeLeaveId = -1;
                if (employeeLeave.Id > 0)
                    employeeLeaveId = employeeLeave.Id;
                else
                    return 0;
                var employeeLeaveIdParam = new SqlParameter { ParameterName = "EmployeeLeaveId", Value = employeeLeaveId };

                Guid employeeId = employeeLeave.EmployeeId;
                if (employeeId == null) return 0;
                var employeeIdParam = new SqlParameter { ParameterName = "EmployeeId", Value = employeeId };

                string employeeCardId = employeeLeave.EmployeeCardId;
                if (String.IsNullOrEmpty(employeeCardId)) return 0;
                var employeeCardIdParam = new SqlParameter { ParameterName = "EmployeeCardId", Value = employeeCardId };

                int? leaveTypeId = employeeLeave.LeaveTypeId;
                if (leaveTypeId == null || leaveTypeId <= 0) return 0;
                var leaveTypeIdParam = new SqlParameter { ParameterName = "LeaveTypeId", Value = leaveTypeId };

                DateTime? submitDate = employeeLeave.SubmitDate;
                if (submitDate == null) return 0;
                var submitDateParam = new SqlParameter { ParameterName = "SubmitDate", Value = submitDate };

                DateTime? appliedFromDate = employeeLeave.AppliedFromDate;
                if (appliedFromDate == null) return 0;
                var appliedFromDateParam = new SqlParameter { ParameterName = "AppliedFromDate", Value = appliedFromDate };

                DateTime? appliedToDate = employeeLeave.AppliedToDate;
                if (appliedToDate == null) return 0;
                var appliedToDateParam = new SqlParameter { ParameterName = "AppliedToDate", Value = appliedToDate };

                int? appliedTotalDays = employeeLeave.AppliedTotalDays;
                if (appliedTotalDays == null || appliedTotalDays <= 0) return 0;
                var appliedTotalDaysParam = new SqlParameter { ParameterName = "AppliedTotalDays", Value = appliedTotalDays };

                string leavePurpose = employeeLeave.LeavePurpose;
                if (String.IsNullOrEmpty(leavePurpose)) return 0;
                var leavePurposeParam = new SqlParameter { ParameterName = "LeavePurpose", Value = leavePurpose };

                string emergencyPhoneNo = employeeLeave.EmergencyPhoneNo;
                if (String.IsNullOrEmpty(emergencyPhoneNo)) return 0;
                var emergencyPhoneNoParam = new SqlParameter { ParameterName = "EmergencyPhoneNo", Value = emergencyPhoneNo };

                string addressDuringLeave = employeeLeave.AddressDuringLeave;
                if (String.IsNullOrEmpty(addressDuringLeave)) return 0;
                var addressDuringLeaveParam = new SqlParameter { ParameterName = "AddressDuringLeave", Value = addressDuringLeave };

                DateTime? recommendedFromDate = new DateTime(1900, 01, 01);
                if (employeeLeave.RecommendedFromDate != null)
                    recommendedFromDate = employeeLeave.RecommendedFromDate;
                var recommendedFromDateParam = new SqlParameter { ParameterName = "RecommendedFromDate", Value = recommendedFromDate };

                DateTime? recommendedToDate = new DateTime(1900, 01, 01);
                if (employeeLeave.RecommendedToDate != null)
                    recommendedToDate = employeeLeave.RecommendedToDate;
                var recommendedToDateParam = new SqlParameter { ParameterName = "RecommendedToDate", Value = recommendedToDate };

                int? recommendedTotalDays = -1;
                if (employeeLeave.RecommendedTotalDays > 0)
                    recommendedTotalDays = employeeLeave.RecommendedTotalDays;
                var recommendedTotalDaysParam = new SqlParameter { ParameterName = "RecommendedTotalDays", Value = recommendedTotalDays };

                int? recommendationStatus = -1;
                if (employeeLeave.RecommendationStatus > 0)
                    recommendationStatus = employeeLeave.RecommendationStatus;
                var recommendationStatusParam = new SqlParameter { ParameterName = "RecommendationStatus", Value = recommendationStatus };

                DateTime? recommendationStatusDate = new DateTime(1900, 01, 01);
                if (employeeLeave.RecommendationStatusDate != null)
                    recommendationStatusDate = employeeLeave.RecommendationStatusDate;
                var recommendationStatusDateParam = new SqlParameter { ParameterName = "RecommendationStatusDate", Value = recommendationStatusDate };

                Guid? recommendationPerson = Guid.Empty;
                if (employeeLeave.RecommendationPerson != null)
                    recommendationPerson = employeeLeave.RecommendationPerson;
                var recommendationPersonParam = new SqlParameter { ParameterName = "RecommendationPerson", Value = recommendationPerson };

                string recommendationComment = string.Empty;
                if (!String.IsNullOrEmpty(employeeLeave.RecommendationComment))
                    recommendationComment = employeeLeave.RecommendationComment;
                var recommendationCommentParam = new SqlParameter { ParameterName = "RecommendationComment", Value = recommendationComment };

                DateTime? approvedFromDate = new DateTime(1900, 01, 01);
                if (employeeLeave.ApprovedFromDate != null)
                    approvedFromDate = employeeLeave.ApprovedFromDate;
                var approvedFromDateParam = new SqlParameter { ParameterName = "ApprovedFromDate", Value = approvedFromDate };

                DateTime? approvedToDate = new DateTime(1900, 01, 01);
                if (employeeLeave.ApprovedToDate != null)
                    approvedToDate = employeeLeave.ApprovedToDate;
                var approvedToDateParam = new SqlParameter { ParameterName = "ApprovedToDate", Value = approvedToDate };

                int? approvedTotalDays = -1;
                if (employeeLeave.ApprovedTotalDays > 0)
                    approvedTotalDays = employeeLeave.ApprovedTotalDays;
                var approvedTotalDaysParam = new SqlParameter { ParameterName = "ApprovedTotalDays", Value = approvedTotalDays };

                int? approvalStatus = -1;
                if (employeeLeave.ApprovalStatus > 0)
                    approvalStatus = employeeLeave.ApprovalStatus;
                var approvalStatusParam = new SqlParameter { ParameterName = "ApprovalStatus", Value = approvalStatus };

                DateTime? approvalStatusDate = new DateTime(1900, 01, 01);
                if (employeeLeave.ApprovalStatusDate != null)
                    approvalStatusDate = employeeLeave.ApprovalStatusDate;
                var approvalStatusDateParam = new SqlParameter { ParameterName = "ApprovalStatusDate", Value = approvalStatusDate };

                Guid? approvalPerson = Guid.Empty;
                if (employeeLeave.ApprovalPerson != null)
                    approvalPerson = employeeLeave.ApprovalPerson;
                var approvalPersonParam = new SqlParameter { ParameterName = "ApprovalPerson", Value = approvalPerson };

                string approvalComment = string.Empty;
                if (!String.IsNullOrEmpty(employeeLeave.ApprovalComment))
                    approvalComment = employeeLeave.ApprovalComment;
                var approvalCommentParam = new SqlParameter { ParameterName = "ApprovalComment", Value = approvalComment };

                DateTime? consumedFromDate = new DateTime(1900, 01, 01);
                if (employeeLeave.ConsumedFromDate != null)
                    consumedFromDate = employeeLeave.ConsumedFromDate;
                var consumedFromDateParam = new SqlParameter { ParameterName = "ConsumedFromDate", Value = consumedFromDate };

                DateTime? consumedToDate = new DateTime(1900, 01, 01);
                if (employeeLeave.ConsumedToDate != null)
                    consumedToDate = employeeLeave.ConsumedToDate;
                var consumedToDateParam = new SqlParameter { ParameterName = "ConsumedToDate", Value = consumedToDate };

                int? consumedTotalDays = -1;
                if (employeeLeave.ConsumedTotalDays > 0)
                    consumedTotalDays = employeeLeave.ConsumedTotalDays;
                var consumedTotalDaysParam = new SqlParameter { ParameterName = "ConsumedTotalDays", Value = consumedTotalDays };

                int? joinedBeforeDays = 0;
                if (employeeLeave.JoinedBeforeDays > 0)
                    joinedBeforeDays = employeeLeave.JoinedBeforeDays;
                var joinedBeforeDaysParam = new SqlParameter { ParameterName = "JoinedBeforeDays", Value = joinedBeforeDays };

                DateTime? resumeDate = new DateTime(1900, 01, 01);
                if (employeeLeave.ResumeDate != null)
                    resumeDate = employeeLeave.ResumeDate;
                var resumeDateParam = new SqlParameter { ParameterName = "ResumeDate", Value = resumeDate };

                DateTime? createdDate = new DateTime(1900, 01, 01);
                if (employeeLeave.CreatedDate != null)
                    createdDate = employeeLeave.CreatedDate;
                var createdDateParam = new SqlParameter { ParameterName = "CreatedDate", Value = createdDate };

                Guid? createdBy = Guid.Empty;
                if (employeeLeave.CreatedBy != null)
                    createdBy = employeeLeave.CreatedBy;
                var createdByParam = new SqlParameter { ParameterName = "CreatedBy", Value = createdBy };

                DateTime? editedDate = new DateTime(1900, 01, 01);
                if (employeeLeave.EditedDate != null)
                    editedDate = employeeLeave.EditedDate;
                var editedDateParam = new SqlParameter { ParameterName = "EditedDate", Value = editedDate };

                Guid? editedBy = Guid.Empty;
                if (employeeLeave.EditedBy != null)
                    editedBy = employeeLeave.EditedBy;
                var editedByParam = new SqlParameter { ParameterName = "EditedBy", Value = editedBy };

                bool isActive = false;
                if (employeeLeave.IsActive)
                    isActive = employeeLeave.IsActive;
                var isActiveParam = new SqlParameter { ParameterName = "IsActive", Value = isActive };

                var employeeLeaveUpdateStatus = Context.Database.SqlQuery<int>("HRMSPUpdateEmployeeLeave  @EmployeeLeaveId, @EmployeeId, " +
                                                                         "@EmployeeCardId, @LeaveTypeId, @SubmitDate, " +
                                                                         "@AppliedFromDate, @AppliedToDate, @AppliedTotalDays, @LeavePurpose, @EmergencyPhoneNo, " +
                                                                         "@AddressDuringLeave, @RecommendedFromDate, @RecommendedToDate, " +
                                                                         "@RecommendedTotalDays, @RecommendationStatus, @RecommendationStatusDate, " +
                                                                         "@RecommendationPerson, @RecommendationComment, @ApprovedFromDate, " +
                                                                         "@ApprovedToDate, @ApprovedTotalDays, @ApprovalStatus, @ApprovalStatusDate, " +
                                                                         "@ApprovalPerson, @ApprovalComment, @ConsumedFromDate, @ConsumedToDate, " +
                                                                         "@ConsumedTotalDays, @JoinedBeforeDays, @ResumeDate, @CreatedDate, @CreatedBy, @EditedDate, @EditedBy, @IsActive",
                                                                          employeeLeaveIdParam, employeeIdParam, employeeCardIdParam, leaveTypeIdParam, submitDateParam,
                                                                          appliedFromDateParam, appliedToDateParam, appliedTotalDaysParam, leavePurposeParam,
                                                                          emergencyPhoneNoParam, addressDuringLeaveParam, recommendedFromDateParam, recommendedToDateParam,
                                                                          recommendedTotalDaysParam, recommendationStatusParam, recommendationStatusDateParam,
                                                                          recommendationPersonParam, recommendationCommentParam, approvedFromDateParam,
                                                                          approvedToDateParam, approvedTotalDaysParam, approvalStatusParam, approvalStatusDateParam,
                                                                          approvalPersonParam, approvalCommentParam, consumedFromDateParam, consumedToDateParam,
                                                                          consumedTotalDaysParam, joinedBeforeDaysParam, resumeDateParam, createdDateParam, createdByParam,
                                                                          editedDateParam, editedByParam, isActiveParam)
                                                                         .ToList()[0];

                return employeeLeaveUpdateStatus;


            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }

        public int DeleteEmployeeLeave(EmployeeLeave employeeLeave)
        {
            try
            {
                int? employeeLeaveId = -1;
                if (employeeLeave.Id > 0)
                    employeeLeaveId = employeeLeave.Id;
                else
                    return 0;
                var employeeLeaveIdParam = new SqlParameter { ParameterName = "EmployeeLeaveId", Value = employeeLeaveId };


                Guid employeeId = employeeLeave.EmployeeId;
                if (employeeId == null) return 0;
                var employeeIdParam = new SqlParameter { ParameterName = "EmployeeId", Value = employeeId };

                string employeeCardId = employeeLeave.EmployeeCardId;
                if (String.IsNullOrEmpty(employeeCardId)) return 0;
                var employeeCardIdParam = new SqlParameter { ParameterName = "EmployeeCardId", Value = employeeCardId };


                DateTime? editedDate = new DateTime(1900, 01, 01);
                if (employeeLeave.EditedDate != null)
                    editedDate = employeeLeave.EditedDate;
                var editedDateParam = new SqlParameter { ParameterName = "EditedDate", Value = editedDate };

                Guid? editedBy = Guid.Empty;
                if (employeeLeave.EditedBy != Guid.Empty)
                    editedBy = employeeLeave.CreatedBy;
                var editedByParam = new SqlParameter { ParameterName = "EditedBy", Value = editedBy };

                bool isActive = false;
                if (employeeLeave.IsActive)
                    isActive = employeeLeave.IsActive;
                var isActiveParam = new SqlParameter { ParameterName = "IsActive", Value = isActive };

                var employeeLeaveUpdateStatus = Context.Database.SqlQuery<int>("HRMSPDeleteEmployeeLeave  @EmployeeLeaveId, @EmployeeId, " +
                                                                         "@EmployeeCardId, " +
                                                                         "@EditedDate, @EditedBy, @IsActive",
                                                                          employeeLeaveIdParam, employeeIdParam, employeeCardIdParam,
                                                                          editedDateParam, editedByParam, isActiveParam)
                                                                         .ToList()[0];

                return employeeLeaveUpdateStatus;


            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }

        public int SaveIndividualLeaveHistoryForSpecificYear(Guid employeeId, string employeeCardId, int year, int branchUnitId, int employeeTypeId)
        {
            try
            {
                var employeeIdParam = new SqlParameter { ParameterName = "EmployeeId", Value = employeeId };
                var employeeCardIdParam = new SqlParameter { ParameterName = "EmployeeCardId", Value = employeeCardId };
                var yearParam = new SqlParameter { ParameterName = "Year", Value = year };
                var branchUnitIdParam = new SqlParameter { ParameterName = "BranchUnitId", Value = branchUnitId };
                var employeeTypeIdParam = new SqlParameter { ParameterName = "EmployeeTypeId", Value = employeeTypeId };

                return Context.Database.SqlQuery<int>("HRMSPSaveIndividualLeaveHistoryForSpecificYear @EmployeeId, @EmployeeCardId, @Year, @BranchUnitId, @EmployeeTypeId",
                                                                     employeeIdParam, employeeCardIdParam, yearParam, branchUnitIdParam, employeeTypeIdParam).ToList()[0];
            }
            catch (Exception exception)
            {
                throw exception;
            }

        }

        public bool CheckEmployeeLeaveExistence(EmployeeLeave employeeLeave)
        {
            Guid employeeId = employeeLeave.EmployeeId;
            var employeeIdParam = new SqlParameter { ParameterName = "EmployeeId", Value = employeeId };

            DateTime? consumedFromDate = new DateTime(1900, 01, 01);
            if (employeeLeave.ConsumedFromDate != null)
                consumedFromDate = employeeLeave.ConsumedFromDate;
            var consumedFromDateParam = new SqlParameter { ParameterName = "ConsumedFromDate", Value = consumedFromDate };

            DateTime? consumedToDate = new DateTime(1900, 01, 01);
            if (employeeLeave.ConsumedToDate != null)
                consumedToDate = employeeLeave.ConsumedToDate;
            var consumedToDateParam = new SqlParameter { ParameterName = "ConsumedToDate", Value = consumedToDate };


            var isEmployeeLeaveExistence = Context.Database.SqlQuery<int>("spHrmCheckEmployeeLeaveExistence @EmployeeId, " +
                                                                     "@ConsumedFromDate, @ConsumedToDate",
                                                                      employeeIdParam, consumedFromDateParam, consumedToDateParam)
                                                                     .ToList()[0];


            return isEmployeeLeaveExistence > 0;

        }

        public List<EmployeeLeaveHistoryIndividual> GetEmployeeLeaveSummaryIndividual(Guid employeeId, DateTime date)
        {

            var employeeIdParam = new SqlParameter { ParameterName = "EmployeeId", Value = employeeId };

            if (date == null)
                date = new DateTime(1900, 01, 01);
            var dateParam = new SqlParameter { ParameterName = "UpToDate", Value = date };

            return Context.Database.SqlQuery<EmployeeLeaveHistoryIndividual>("SPEmployeeLeaveSummaryIndividual @EmployeeId, @UpToDate", employeeIdParam, dateParam).ToList();
        }

        public List<EmployeeSalaryIndividual> GetEmployeeSalarySummaryIndividual(Guid employeeId, DateTime date)
        {

            var employeeIdParam = new SqlParameter { ParameterName = "EmployeeId", Value = employeeId };

            if (date == null)
                date = new DateTime(1900, 01, 01);
            var dateParam = new SqlParameter { ParameterName = "UpToDate", Value = date };

            return Context.Database.SqlQuery<EmployeeSalaryIndividual>("SPEmployeeSalaryIndividual @EmployeeId, @UpToDate", employeeIdParam, dateParam).ToList();
        }

        public List<EmployeeAttendanceIndividual> GetEmployeeAttendanceIndividual(Guid employeeId, DateTime date)
        {

            var employeeIdParam = new SqlParameter { ParameterName = "EmployeeId", Value = employeeId };

            if (date == null)
                date = new DateTime(1900, 01, 01);
            var dateParam = new SqlParameter { ParameterName = "UpToDate", Value = date };

            return Context.Database.SqlQuery<EmployeeAttendanceIndividual>("SPEmployeeAttendanceIndividual @EmployeeId, @UpToDate", employeeIdParam, dateParam).ToList();
        }

        public List<EmployeePenaltyIndividual> GetEmployeePenaltyIndividual(Guid employeeId, DateTime date)
        {

            var employeeIdParam = new SqlParameter { ParameterName = "EmployeeId", Value = employeeId };

            if (date == null)
                date = new DateTime(1900, 01, 01);
            var dateParam = new SqlParameter { ParameterName = "UpToDate", Value = date };

            return Context.Database.SqlQuery<EmployeePenaltyIndividual>("SPEmployeePenaltyIndividual @EmployeeId, @UpToDate", employeeIdParam, dateParam).ToList();
        }

        public List<EmployeeBasicInfo> GetEmployeeBasicInfo(string EmployeeCardId, DateTime date)
        {
            var employeeCardIdParam = new SqlParameter { ParameterName = "EmployeeCardId", Value = EmployeeCardId };

            if (date == null)
                date = new DateTime(1900, 01, 01);
            var dateParam = new SqlParameter { ParameterName = "Date", Value = date };

            return Context.Database.SqlQuery<EmployeeBasicInfo>("SPEmployeeFollowUp @EmployeeCardId, @Date", employeeCardIdParam, dateParam).ToList();
        }
    }
}