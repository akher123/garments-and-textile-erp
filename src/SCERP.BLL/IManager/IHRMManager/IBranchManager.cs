
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SCERP.Model;

namespace SCERP.BLL.IManager.IHRMManager
{
    public interface IBranchManager
    {
        List<Branch> GetAllBranchesByPaging(int startPage, int pageSize, Branch branch, out int totalRecords);

        List<Branch> GetAllBranches();

        Branch GetBranchById(int? id);

        int SaveBranch(Branch branch);

        int EditBranch(Branch branch);

        int DeleteBranch(Branch branch);

        bool CheckExistingBranch(Branch branch);

        List<Branch> GetAllBranchesBySearchKey(string searchByBranchName, int searchByCompanyName);

        List<Branch> GetAllBranchesByCompanyId(int id);

        IEnumerable GetAllPermittedBranchesByCompanyId(int companyId);

    }
}
