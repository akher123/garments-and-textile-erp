using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;
using SCERP.Model.CommonModel;

namespace SCERP.DAL.IRepository.ICommonRepository
{
    public interface ICustomSqlQuaryRepository:IRepository<CustomSqlQuary>
    {
        List<VUserReport> GetUserReport(string userName);
    }
}
