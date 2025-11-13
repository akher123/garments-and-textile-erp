using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;

namespace SCERP.DAL.IRepository.IMerchandisingRepository
{
    public interface IMerchandiserRepository : IRepository<OM_Merchandiser>
    {
        string GetMerchandiserRefId(string compId);
        bool IsMerchandiser(Guid? userId);
    }
}
