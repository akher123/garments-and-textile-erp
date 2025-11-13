using SCERP.Common;
using SCERP.DAL.IRepository.ICommercialRepository;
using SCERP.Model.CommercialModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.DAL.Repository.CommercialRepository
{
    public class ImportDetailsRepository : Repository<CommImportDetails>, IImportDetailsRepository
    {
        private readonly string _companyId;
        public ImportDetailsRepository(SCERPDBContext context) : base(context)
        {
            this._companyId = PortalContext.CurrentUser.CompId;
        }

    }
}
