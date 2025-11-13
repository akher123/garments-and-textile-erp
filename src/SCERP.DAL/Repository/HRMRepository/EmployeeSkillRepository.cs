using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.DAL.IRepository;
using SCERP.DAL.IRepository.IHRMRepository;
using SCERP.Model;
using SCERP.Model.Custom;
using System.Data.Entity;

namespace SCERP.DAL.Repository.HRMRepository
{
    public class EmployeeSkillRepository : Repository<EmployeeSkill>, IEmployeeSkillRepository
    {
        public EmployeeSkillRepository(SCERPDBContext context)
            : base(context)
        {

        }

        public List<VEmployeeSkillDetail> GetAllEmployeeSkillDetails(int startPage, int pageSize, EmployeeSkill model, SearchFieldModel searchFieldModel,
            out int totalRecords)
        {
            var employeeSkillDetails = GetEmployeeSkillBySearchKey(searchFieldModel).AsQueryable();
            totalRecords = employeeSkillDetails.Count();
            switch (model.sort)
            {
                case "EmployeeCardId":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            employeeSkillDetails = employeeSkillDetails
                                .OrderByDescending(r => r.EmployeeCardId)
                                .Skip(startPage * pageSize)
                                .Take(pageSize);
                            break;
                        default:
                            employeeSkillDetails = employeeSkillDetails
                            .OrderBy(r => r.EmployeeCardId)
                            .Skip(startPage * pageSize)
                            .Take(pageSize);
                            break;
                    }
                    break;
                case "DifficultyName":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            employeeSkillDetails = employeeSkillDetails
                             .OrderByDescending(r => r.DifficultyName)
                             .Skip(startPage * pageSize)
                             .Take(pageSize);
                            break;
                        default:
                            employeeSkillDetails = employeeSkillDetails
                             .OrderBy(r => r.DifficultyName)
                             .Skip(startPage * pageSize)
                             .Take(pageSize);
                            break;
                    }
                    break;
                case "CategoryName":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            employeeSkillDetails = employeeSkillDetails
                            .OrderByDescending(r => r.CategoryName)
                            .Skip(startPage * pageSize)
                            .Take(pageSize);
                            break;
                        default:
                            employeeSkillDetails = employeeSkillDetails
                            .OrderBy(r => r.CategoryName)
                            .Skip(startPage * pageSize)
                            .Take(pageSize);
                            break;
                    }
                    break;
                case "Name":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            employeeSkillDetails = employeeSkillDetails
                            .OrderByDescending(r => r.OperationName)
                            .Skip(startPage * pageSize)
                            .Take(pageSize);
                            break;
                        default:
                            employeeSkillDetails = employeeSkillDetails
                            .OrderBy(r => r.OperationName)
                            .Skip(startPage * pageSize)
                            .Take(pageSize);
                            break;
                    }
                    break;
                case "Efficiency":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            employeeSkillDetails = employeeSkillDetails
                            .OrderByDescending(r => r.Efficiency)
                            .Skip(startPage * pageSize)
                            .Take(pageSize);
                            break;
                        default:
                            employeeSkillDetails = employeeSkillDetails
                            .OrderBy(r => r.Efficiency)
                            .Skip(startPage * pageSize)
                            .Take(pageSize);
                            break;
                    }
                    break;
                default:
                    switch (model.sortdir)
                    {
                        case "DESC":
                            employeeSkillDetails = employeeSkillDetails
                           .OrderByDescending(r => r.EmployeeCardId)
                           .Skip(startPage * pageSize)
                           .Take(pageSize);
                            break;
                        default:
                            employeeSkillDetails = employeeSkillDetails
                        .OrderBy(r => r.EmployeeCardId)
                        .Skip(startPage * pageSize)
                        .Take(pageSize);
                            break;
                    }

                    break;
            }

            return employeeSkillDetails.ToList();
        }

        public EmployeeSkill GetEmployeeSkillById(int employeeSkillId)
        {
            EmployeeSkill employeeSkill =
            Context.EmployeeSkills.Include(x => x.Employee).FirstOrDefault(x => x.EmployeeSkillId == employeeSkillId);
            return employeeSkill;
        }

        //private List<VEmployeeSkillDetail> GetEmployeeSkillBySearchKey(SearchFieldModel searchFieldModel)
        //{
        //    var employeeSkillDetails = Context.VEmployeeSkillDetails.Where(
        //        x =>
        //            (x.EmployeeCardId == searchFieldModel.SearchByEmployeeCardId ||
        //             searchFieldModel.SearchByEmployeeCardId == null));
        //    return employeeSkillDetails.ToList();

        //}



        private List<VEmployeeSkillDetail> GetEmployeeSkillBySearchKey(SearchFieldModel searchFieldModel)
        {
            var employeeSkillDetails = Context.VEmployeeSkillDetails.Where(
                x =>
                    (x.EmployeeCardId == searchFieldModel.SearchByEmployeeCardId ||
                     searchFieldModel.SearchByEmployeeCardId == null)

                     && ((x.FromDate >= searchFieldModel.StartDate || searchFieldModel.StartDate == null)
                               && (x.ToDate <= searchFieldModel.EndDate || searchFieldModel.EndDate == null)))
                               .OrderBy(x => x.EmployeeCardId).ThenByDescending(x => x.FromDate);
            return employeeSkillDetails.ToList();
        }

    }
}
