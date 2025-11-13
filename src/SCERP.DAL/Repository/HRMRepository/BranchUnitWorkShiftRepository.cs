using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using SCERP.Common;
using SCERP.DAL.IRepository.IHRMRepository;
using SCERP.Model;
using SCERP.Model.Custom;

namespace SCERP.DAL.Repository.HRMRepository
{
    public class BranchUnitWorkShiftRepository : Repository<BranchUnitWorkShift>, IBranchUnitWorkShiftRepository
    {
        public BranchUnitWorkShiftRepository(SCERPDBContext context)
            : base(context)
        {
        }

        public List<BranchUnitWorkShift> GetBranchUnitWorkShiftsBuPaging(int startPage, int pageSize, out int totalRecords,
            SearchFieldModel searchFieldModel, BranchUnitWorkShift model)
        {
            IQueryable<BranchUnitWorkShift> branchUnitWorkShifts;
            try
            {
                branchUnitWorkShifts =
                    Context.BranchUnitWorkShifts.
                        Include(x => x.BranchUnit.Unit)
                        .Include(x => x.BranchUnit.Branch.Company)
                        .Include(x => x.WorkShift)
                        .Where(
                            x =>
                                x.IsActive &&
                                (x.BranchUnit.Branch.CompanyId == searchFieldModel.SearchByCompanyId || searchFieldModel.SearchByCompanyId == 0)
                                &&
                                (x.BranchUnit.BranchId == searchFieldModel.SearchByBranchId || searchFieldModel.SearchByBranchId == 0)
                                &&
                                (x.BranchUnitId == searchFieldModel.SearchByBranchUnitId || searchFieldModel.SearchByBranchUnitId == 0) && (x.WorkShiftId == searchFieldModel.SearchByWorkShiftId || searchFieldModel.SearchByWorkShiftId == 0));
                totalRecords = branchUnitWorkShifts.Count();
                switch (model.sort)
                {
                    case "BranchUnit.Branch.Company.Name":
                        switch (model.sortdir)
                        {
                            case "DESC":
                                branchUnitWorkShifts = branchUnitWorkShifts
                                    .OrderByDescending(r => r.BranchUnit.Branch.Company.Name)
                                    .Skip(startPage*pageSize)
                                    .Take(pageSize);
                                break;
                            default:
                                branchUnitWorkShifts = branchUnitWorkShifts
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
                                branchUnitWorkShifts = branchUnitWorkShifts
                                    .OrderByDescending(r => r.BranchUnit.Branch.Name)
                                    .Skip(startPage*pageSize)
                                    .Take(pageSize);
                                break;
                            default:
                                branchUnitWorkShifts = branchUnitWorkShifts
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
                                branchUnitWorkShifts = branchUnitWorkShifts
                                    .OrderByDescending(r => r.BranchUnit.Unit.Name)
                                    .Skip(startPage*pageSize)
                                    .Take(pageSize);
                                break;
                            default:
                                branchUnitWorkShifts = branchUnitWorkShifts
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
                                branchUnitWorkShifts = branchUnitWorkShifts
                                    .OrderByDescending(r => r.FromDate)
                                    .Skip(startPage*pageSize)
                                    .Take(pageSize);
                                break;
                            default:
                                branchUnitWorkShifts = branchUnitWorkShifts
                                    .OrderByDescending(r => r.FromDate)
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
            return branchUnitWorkShifts.ToList();
        }

        public BranchUnitWorkShift GetBranchUnitDepartmentById(int branchUnitWorkShiftId)
        {
            BranchUnitWorkShift branchUnitWorkShift;
            try
            {
                branchUnitWorkShift =
                    Context.BranchUnitWorkShifts.Include(x => x.BranchUnit.Branch.Company)
                        .Include(x => x.BranchUnit.Unit)
                        .SingleOrDefault(x => x.BranchUnitWorkShiftId == branchUnitWorkShiftId);
            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message, exception.InnerException);
            }
            return branchUnitWorkShift;
        }

        public List<BranchUnitWorkShift> GetBranchUnitWorkShiftBySearchKey(SearchFieldModel searchFieldModel)
        {
            IQueryable<BranchUnitWorkShift> branchUnitWorkShifts;
            try
            {
                branchUnitWorkShifts =
                    Context.BranchUnitWorkShifts.
                        Include(x => x.BranchUnit.Unit)
                        .Include(x => x.BranchUnit.Branch.Company)
                        .Include(x => x.WorkShift)
                        .Where(
                            x =>
                                x.IsActive &&
                                (x.BranchUnit.Branch.CompanyId == searchFieldModel.SearchByCompanyId)
                                &&
                                (x.BranchUnit.BranchId == searchFieldModel.SearchByBranchId)
                                &&
                                (x.BranchUnitId == searchFieldModel.SearchByBranchUnitId) && (x.WorkShiftId == searchFieldModel.SearchByWorkShiftId || searchFieldModel.SearchByWorkShiftId == 0));

            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message);
            }
            return branchUnitWorkShifts.ToList();
        }

        public List<WorkShift> GetBranchUnitWorkShiftByBrancUnitId(int searchByBranchUnitId)
        {
            List<WorkShift> workShifts;
            try
            {
                workShifts = Context.BranchUnitWorkShifts.Include(x => x.WorkShift).Where(x => x.Status == (Int16) StatusValue.Active && x.IsActive && x.BranchUnitId == searchByBranchUnitId).OrderByDescending(x=>x.FromDate).ToList()
                    .Select(x =>
                        new WorkShift()
                        {
                            DisplayMember = x.WorkShift.NameDetail + " - (" + x.FromDate.Value.ToString("dd/MM/yyyy") + " - " + (x.ToDate == null ? "" : x.ToDate.Value.ToString("dd/MM/yyyy")) + ")",
                            ValueMember = x.BranchUnitWorkShiftId
                        }).OrderByDescending(x => x.DisplayMember).ToList();
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message, exception.InnerException);
            }
            return workShifts.Take(10).ToList();
        }
    }
}
