using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.CommercialModel;

namespace SCERP.DAL.IRepository.ICommercialRepository
{
    public interface IReceiveRepository : IRepository<CommReceive>
    {
        CommReceive GetReceiveById(int? id);
        List<CommReceive> GetAllReceives();
        List<CommReceive> GetAllReceivesByPaging(int startPage, int pageSize, out int totalRecords, CommReceive receive);
        List<CommReceive> GetReceiveBySearchKey(int searchByCountry, string searchByReceive);
    }
}
