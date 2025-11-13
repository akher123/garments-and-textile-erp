using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.DAL.IRepository.IInventoryRepository;
using SCERP.Model;

namespace SCERP.DAL.Repository.InventoryRepository
{
    public class QualityCertificateDetailRepository : Repository<Inventory_QualityCertificateDetail>, IQualityCertificateDetailRepository
    {
        public QualityCertificateDetailRepository(SCERPDBContext context) : base(context)
        {
        }
    }
}
