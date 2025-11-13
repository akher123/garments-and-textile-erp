using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.DAL.IRepository.ICommercialRepository;
using SCERP.Model;
using SCERP.Model.CommercialModel;
using SCERP.Common;

namespace SCERP.DAL.Repository.CommercialRepository
{
    public class ExportDetailRepository : Repository<CommExportDetail>, IExportDetailRepository
    {
        public ExportDetailRepository(SCERPDBContext context)
            : base(context)
        {

        }
    }
}
