using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;

namespace SCERP.DAL.IRepository.IMerchandisingRepository
{
    public interface IOmSizeRepository:IRepository<OM_Size>
    {
        string GetNewOmSizeRefId(string compId);
    }
}
