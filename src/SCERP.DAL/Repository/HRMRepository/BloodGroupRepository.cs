using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using SCERP.DAL.IRepository.IHRMRepository;
using SCERP.Model;


namespace SCERP.DAL.Repository.HRMRepository
{
    public class BloodGroupRepository:Repository<BloodGroup>, IBloodGroupRepository
    {
        public BloodGroupRepository(SCERPDBContext context)
            : base(context)
        {

        }

        public List<BloodGroup> GetAllBloodGroups()
        {
            try
            {
                return Context.BloodGroups.Where(x => x.IsActive).OrderBy(x=>x.GroupName).ToList();
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
            
        }
    }
}
