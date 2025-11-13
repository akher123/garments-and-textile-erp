using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.DAL.IRepository.IPlanningRepository;
using SCERP.Model.Planning;

namespace SCERP.DAL.Repository.Planning
{
    public class TNATemplateRepository : Repository<PLAN_TNA_Template>, ITNATemplateRepository
    {
        private readonly SCERPDBContext _context;

        public TNATemplateRepository(SCERPDBContext context)
            : base(context)
        {
            this._context = context;
        }
    }
}
