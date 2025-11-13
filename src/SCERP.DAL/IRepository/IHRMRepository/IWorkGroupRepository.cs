using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;
using SCERP.Model.Custom;

namespace SCERP.DAL.IRepository.IHRMRepository
{
    public interface IWorkGroupRepository : IRepository<WorkGroup>
    {
        List<WorkGroup> GetAllWorkGroupsByPaging(int startPage, int pageSize,SearchFieldModel searchFieldModel,WorkGroup model, out int totalRecords);
        WorkGroup GetWorkGroupById(int workGroupId);
    }
}
