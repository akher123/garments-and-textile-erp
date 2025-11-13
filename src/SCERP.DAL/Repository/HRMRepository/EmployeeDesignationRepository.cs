using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using SCERP.Common;
using SCERP.DAL.IRepository.IHRMRepository;
using SCERP.Model;

namespace SCERP.DAL.Repository.HRMRepository
{
    public class EmployeeDesignationRepository : Repository<EmployeeDesignation>, IEmployeeDesignationRepository
    {
        public EmployeeDesignationRepository(SCERPDBContext context)
            : base(context)
        {
        }

        public EmployeeDesignation GetEmployeeDesignationById(int? id)
        {
            return Context.EmployeeDesignations.Find(id);
        }

        public override IQueryable<EmployeeDesignation> All()
        {
            return Context.EmployeeDesignations.Where(x => x.IsActive == true);
        }

        public IQueryable<EmployeeDesignation> GetEmployeeDesignationByEmployeeGrade(int? employeeGradeId)
        {
            return Context.EmployeeDesignations.Where(x => x.GradeId == employeeGradeId && x.IsActive).OrderBy(x=>x.Title);
        }


        public IQueryable<EmployeeDesignation> GetEmployeeDesignationByEmployeeType(int employeeTypeId)
        {
            return Context.EmployeeDesignations.Where(x => x.EmployeeTypeId == employeeTypeId && x.IsActive).OrderBy(x => x.Title);
        }



        public IEnumerable<EmployeeDesignationViewModel> GetRestEmployeeDesignations()
        {
            var employeeDesignationList = from employeeDesignations in Context.EmployeeDesignations
                                          where
                                              !
                                                  (from companyOrganograms in Context.CompanyOrganograms
                                                   select new
                                                   {
                                                       companyOrganograms.DesignationId
                                                   }).Contains(new { DesignationId = employeeDesignations.Id })
                                          select new EmployeeDesignationViewModel()
                                          {
                                              DesignationId = employeeDesignations.Id,
                                              Title = employeeDesignations.Title,
                                          };

            return employeeDesignationList;
        }
     
        public List<EmployeeDesignation> GetAllEmployeeDesignationsByPaging(int startPage, int pageSize, out int totalRecords, EmployeeDesignation employeeDesignation)
        {
            IQueryable<EmployeeDesignation> employeeDesignations = null;

            try
            {

                var searchByEmployeeType = employeeDesignation.EmployeeTypeId;
                var searchByEmployeeGrade = employeeDesignation.GradeId;
                var searchByEmployeeDesignation = employeeDesignation.Title;
                employeeDesignations = Context.EmployeeDesignations.Include(x=>x.EmployeeType).Include(x => x.EmployeeGrade)
                    .Where((x => x.IsActive
                                 &&
                                 ((x.Title.Replace(" ", "")
                                     .ToLower()
                                     .Contains(searchByEmployeeDesignation.Replace(" ", "").ToLower()))
                                  || String.IsNullOrEmpty(searchByEmployeeDesignation))
                                 && (x.EmployeeTypeId == searchByEmployeeType || searchByEmployeeType == 0)
                                 && (x.GradeId == searchByEmployeeGrade || searchByEmployeeGrade == 0)));
                totalRecords = employeeDesignations.Count();                  
         
                switch (employeeDesignation.sort)
                {
                    case "EmployeeType.Title":
                        switch (employeeDesignation.sortdir)
                        {
                            case "DESC":
                                employeeDesignations =
                                    employeeDesignations
                                        .OrderByDescending(r => r.EmployeeType.Title)
                                        .Skip(startPage*pageSize)
                                        .Take(pageSize);
                                break;
                            default:
                                employeeDesignations = employeeDesignations
                                    .OrderBy(r => r.EmployeeType.Title)
                                    .Skip(startPage*pageSize)
                                    .Take(pageSize);
                                break;
                        }
                        break;

                    case "EmployeeGrade.Name":
                        switch (employeeDesignation.sortdir)
                        {
                            case "DESC":
                                employeeDesignations = employeeDesignations
                                    .OrderByDescending(r => r.EmployeeGrade.Name)
                                    .Skip(startPage*pageSize)
                                    .Take(pageSize);
                                break;
                            default:
                                employeeDesignations = employeeDesignations
                                    .OrderBy(r => r.EmployeeGrade.Name)
                                    .Skip(startPage*pageSize)
                                    .Take(pageSize);
                                break;
                        }
                        break;

                    default:

                        switch (employeeDesignation.sortdir)
                        {
                            case "DESC":
                                employeeDesignations = employeeDesignations
                                    .OrderByDescending(r => r.Title)
                                    .Skip(startPage*pageSize)
                                    .Take(pageSize);
                                break;
                            default:
                                employeeDesignations = employeeDesignations
                                    .OrderBy(r => r.Title)
                                    .Skip(startPage*pageSize)
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

            return employeeDesignations.ToList();
        }

        public List<EmployeeDesignation> GetAllEmployeeDesignationsBySearchKey(int searchByEmployeeTypeId,
           int searchByEmployeeGradeId, string searchByEmployeeDesignationTitle)
        {
            List<EmployeeDesignation> employeeDesignations = null;

            try
            {
                employeeDesignations = Context.EmployeeDesignations
                                                       .Where((x => x.IsActive == true
                                                        && ((x.Title.Replace(" ", "").ToLower().Contains(searchByEmployeeDesignationTitle.Replace(" ", "").ToLower()))
                                                        || String.IsNullOrEmpty(searchByEmployeeDesignationTitle))
                                                        && (x.EmployeeType.Id == searchByEmployeeTypeId || searchByEmployeeTypeId == 0)
                                                        && (x.EmployeeGrade.Id == searchByEmployeeGradeId || searchByEmployeeGradeId == 0)))
                                                       .Include(x => x.EmployeeGrade)
                                                       .Include(x=>x.EmployeeType).ToList();
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return employeeDesignations;
        }

    }
}
