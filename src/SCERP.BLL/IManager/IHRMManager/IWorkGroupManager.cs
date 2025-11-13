using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;
using SCERP.Model.Custom;


namespace SCERP.BLL.IManager.IHRMManager
{
    public interface IWorkGroupManager
    {
        List<WorkGroup> GetAllWorkGroupsByPaging(int startPage, int pageSize,
            SearchFieldModel searchFieldModel, WorkGroup model, out int totalRecords);
        WorkGroup GetWorkGroupById(int workGroupId);
        bool CheckExistingWorkGroup(WorkGroup workGroup);
        int EditWorkGroup(WorkGroup workGroup);
        int SaveWorkGroup(WorkGroup workGroup);
        int DeleteWorkGroup(int workGroupId);
        List<WorkGroup> GetAllWorkGroupsBySearchKey(int companyId, int branchId, int unitId, string workGroupName);
        List<WorkGroup> GetAllWorkGroups();
        List<WorkGroup> GetWorkGroupsByBranchUnitId(int? branchUnitId);
    }
}
