using System;
using SCERP.BLL.IManager.IHRMManager;
using SCERP.DAL;
using SCERP.DAL.IRepository.IHRMRepository;
using SCERP.DAL.Repository.HRMRepository;
using System.Linq;
using SCERP.Model;
using System.Collections.Generic;

namespace SCERP.BLL.Manager.HRMManager
{
    public class BloodGroupManager : BaseManager, IBloodGroupManager
    {
        private readonly IBloodGroupRepository _bloodGroupRepository = null;

        public BloodGroupManager(SCERPDBContext context)
        {
            _bloodGroupRepository = new BloodGroupRepository(context);
        }

        public List<BloodGroup> GetAllBloodGroups()
        {
            try
            {
                return _bloodGroupRepository.GetAllBloodGroups();
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
            
        }

        public BloodGroup GetBloodGroupById(int? id)
        {
            return _bloodGroupRepository.FindOne(p => p.Id == id);
        }
    }
}
