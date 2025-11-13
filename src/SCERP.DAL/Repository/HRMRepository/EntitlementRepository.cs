using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.Remoting.Contexts;
using SCERP.Common;
using SCERP.DAL.IRepository.IHRMRepository;
using SCERP.Model;
using System.Linq;

namespace SCERP.DAL.Repository.HRMRepository
{
    public class EntitlementRepository : Repository<Entitlement>, IEntitlementRepository
    {
        public EntitlementRepository(SCERPDBContext context)
            : base(context)
        {

        }

        public Entitlement GetEntitlementById(int? id)
        {
            return FindOne(x => x.Id == id && x.IsActive == true);
        }

        public override IQueryable<Entitlement> All()
        {
            return Context.Entitlements.Where(x => x.IsActive == true);
        }



        public List<Entitlement> GetAllEducationLevelsByPaging(int startPage, int pageSize, out int totalRecords, Entitlement entitlement)
        {


            IQueryable<Entitlement> entitlements = null;

            try
            {
                var searchByTitle = entitlement.Title;
                entitlements = Context.Entitlements.Where(
                    x =>
                        x.IsActive &&
                        ((x.Title.Replace(" ", "")
                            .ToLower()
                            .Contains(searchByTitle.Replace(" ", "").ToLower())) || String.IsNullOrEmpty(searchByTitle)));
                totalRecords = entitlements.Count();
                           
                switch (entitlement.sort)
                {
                    case "Title":
                        switch (entitlement.sortdir)
                        {
                            case "DESC":
                                entitlements = entitlements
                                    .OrderByDescending(r => r.Title)
                                    .Skip(startPage*pageSize)
                                    .Take(pageSize);
                                break;
                            default:
                                entitlements = entitlements
                                    .OrderBy(r => r.Title)
                                    .Skip(startPage*pageSize)
                                    .Take(pageSize);
                                break;
                        }

                        break;
                        default:
                        entitlements = entitlements
                                 .OrderBy(r => r.Title)
                                 .Skip(startPage * pageSize)
                                 .Take(pageSize);
                        break;
                    
                }
            }
            catch (Exception exception)
            {
                totalRecords = 0;
                throw new Exception(exception.Message);
            }

            return entitlements.ToList();
        }

        public List<Entitlement> GetEntitlementBySearchKey(Entitlement entitlement)
        {
            List<Entitlement> entitlements;
            try
            {
                entitlements = Context.Entitlements.Where(
                                              x =>
                                                  x.IsActive == true &&
                                                  ((x.Title.Replace(" ", "")
                                                      .ToLower()
                                                      .Contains(entitlement.Title.Replace(" ", "")
                                                      .ToLower())) || String.IsNullOrEmpty(entitlement.Title))
                                                 )
                                              .OrderBy(r => r.Title)
                                              .ToList();
            }

            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }

            return entitlements;
        }


    }
}
