using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using SCERP.DAL.IRepository.IHRMRepository;
using SCERP.Model;
using System.Data;
using System.Data.Entity;
using System.Collections.Generic;
using System;


namespace SCERP.DAL.Repository.HRMRepository
{
    public class EmploymentRepository : Repository<Employment>, IEmploymentRepository
    {
        public EmploymentRepository(SCERPDBContext context)
            : base(context)
        {

        }
    }
}
