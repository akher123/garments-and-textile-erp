using System.Linq;
using System.Runtime.Remoting.Contexts;
using SCERP.DAL.IRepository.IHRMRepository;
using SCERP.Model;
using System.Data.Entity;
using System.Collections.Generic;
using System;
using SCERP.Model.Custom;

namespace SCERP.DAL.Repository.HRMRepository
{
    public class WorkGroupRepository : Repository<WorkGroup>, IWorkGroupRepository
    {
        public WorkGroupRepository(SCERPDBContext context)
            : base(context)
        {
        }

        public List<WorkGroup> GetAllWorkGroupsByPaging(int startPage, int pageSize, SearchFieldModel searchFieldModel, WorkGroup model, out int totalRecords)
        {
            IQueryable<WorkGroup> workGroups ;

            try
            {
                workGroups = Context.WorkGroups
                    .Include(x => x.BranchUnit)
                    .Include(x => x.BranchUnit.Branch)
                    .Include(x => x.BranchUnit.Branch.Company)
                    .Include(x => x.BranchUnit.Unit)
                    .Where(x => x.IsActive
                                &&
                                (x.BranchUnit.Branch.Company.Id == searchFieldModel.SearchByCompanyId ||
                                 searchFieldModel.SearchByCompanyId == 0) &&
                                (x.BranchUnit.Branch.Id == searchFieldModel.SearchByBranchId ||
                                 searchFieldModel.SearchByBranchId == 0) &&
                                (x.BranchUnit.BranchUnitId == searchFieldModel.SearchByBranchUnitId ||
                                 searchFieldModel.SearchByBranchUnitId == 0) &&
                                (((x.Name.Replace(" ", "").ToLower()).Contains(
                                    searchFieldModel.SearchByWorkGroupName.Replace(" ", "").ToLower()))
                                 || string.IsNullOrEmpty(searchFieldModel.SearchByWorkGroupName)));
                                                       

                totalRecords = workGroups.Count();


                switch (model.sort)
                {
                    case "BranchUnit.Branch.Company.Name":

                        switch (model.sortdir)
                        {
                            case "DESC":
                                workGroups = workGroups
                                    .OrderByDescending(r => r.BranchUnit.Branch.Company.Name)
                                    .Skip(startPage*pageSize)
                                    .Take(pageSize);
                                break;
                            default:
                                workGroups = workGroups
                                    .OrderBy(r => r.BranchUnit.Branch.Company.Name)
                                    .Skip(startPage*pageSize)
                                    .Take(pageSize);

                                break;
                        }
                        break;

                    case "BranchUnit.Branch.Name":

                        switch (model.sortdir)
                        {
                            case "DESC":
                                workGroups = workGroups
                                    .OrderByDescending(r => r.BranchUnit.Branch.Name)
                                    .Skip(startPage*pageSize)
                                    .Take(pageSize);
                                break;
                            default:
                                workGroups = workGroups
                                    .OrderBy(r => r.BranchUnit.Branch.Name)
                                    .Skip(startPage*pageSize)
                                    .Take(pageSize);
                                break;
                        }
                        break;

                    case "BranchUnit.Unit.Name":

                        switch (model.sortdir)
                        {
                            case "DESC":
                                workGroups = workGroups
                                    .OrderByDescending(r => r.BranchUnit.Unit.Name)
                                    .Skip(startPage*pageSize)
                                    .Take(pageSize);
                                break;
                            default:
                                workGroups = workGroups
                                    .OrderBy(r => r.BranchUnit.Unit.Name)
                                    .Skip(startPage*pageSize)
                                    .Take(pageSize);
                                break;
                        }
                        break;

                    default:
                        switch (model.sortdir)
                        {
                            case "DESC":
                                workGroups = workGroups
                                    .OrderByDescending(r => r.Name)
                                    .Skip(startPage*pageSize)
                                    .Take(pageSize);
                                break;
                            default:
                                workGroups = workGroups
                                    .OrderBy(r => r.Name)
                                    .Skip(startPage*pageSize)
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

            return workGroups.ToList();
        }

        public WorkGroup GetWorkGroupById(int workGroupId)
        {
            WorkGroup workGroup = null;
            try
            {
                workGroup =
                    Context.WorkGroups.Where(x => x.IsActive && x.WorkGroupId == workGroupId)
                        .Include(x => x.BranchUnit.Branch.Company)
                        .Include(x => x.BranchUnit.Unit)
                        .FirstOrDefault();
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
            return workGroup;
        }
    }
}
