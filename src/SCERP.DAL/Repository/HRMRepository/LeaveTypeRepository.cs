using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Contexts;
using SCERP.DAL.IRepository.IHRMRepository;
using SCERP.Model;
using System.Linq;

namespace SCERP.DAL.Repository.HRMRepository
{
    public class LeaveTypeRepository : Repository<LeaveType>, ILeaveTypeRepository
    {

        public LeaveTypeRepository(SCERPDBContext context)
            : base(context)
        {

        }

        public LeaveType GetLeaveTypeById(int? id)
        {
            return Context.LeaveTypes.Find(id);
        }

        public List<LeaveType> GetAllLeaveTypes(int startPage, int pageSize, out int totalRecords, LeaveType leaveType)
        {
            IQueryable<LeaveType> leaveTypes = null;

            try
            {
                var searchByTitle = leaveType.Title;

                leaveTypes = Context.LeaveTypes.Where(x => x.IsActive == true &&
                                                 ((x.Title.Replace(" ", "")
                                                     .ToLower()
                                                     .Contains(searchByTitle.Replace(" ", "").ToLower())) ||
                                                  String.IsNullOrEmpty(searchByTitle)));
                totalRecords = leaveTypes.Count();
                switch (leaveType.sort)
                {
                    case "Title":
                        switch (leaveType.sortdir)
                        {
                            case "DESC":
                                leaveTypes = leaveTypes
                                  .OrderByDescending(r => r.Title)
                                  .Skip(startPage * pageSize)
                                  .Take(pageSize);
                                break;
                            default:
                                leaveTypes = leaveTypes
                              .OrderBy(r => r.Title)
                              .Skip(startPage * pageSize)
                              .Take(pageSize); ;
                                break;
                        }
                        break;
                    default:
                        leaveTypes = leaveTypes
                      .OrderBy(r => r.Title)
                      .Skip(startPage * pageSize)
                      .Take(pageSize); ;
                        break;
                }
            }
            catch (Exception exception)
            {
                totalRecords = 0;
                throw new Exception(exception.Message);
            }

            return leaveTypes.ToList();
        }

        public List<LeaveType> GetLeaveTypeBySearchKey(LeaveType leaveType)
        {
            List<LeaveType> leaveTypes;
            try
            {
                leaveTypes = Context.LeaveTypes.Where(
                                             x =>
                                                 x.IsActive == true &&
                                                 ((x.Title.Replace(" ", "")
                                                     .ToLower()
                                                     .Contains(leaveType.Title.Replace(" ", "")
                                                     .ToLower())) || String.IsNullOrEmpty(leaveType.Title))
                                                )
                                             .OrderBy(r => r.Title)
                                             .ToList();
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
            return leaveTypes;
        }

        public override IQueryable<LeaveType> All()
        {
            return Context.LeaveTypes.Where(x => x.IsActive == true);
        }


    }
}
