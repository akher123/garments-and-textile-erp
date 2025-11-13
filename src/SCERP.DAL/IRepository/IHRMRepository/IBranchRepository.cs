using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;

namespace SCERP.DAL.IRepository.IHRMRepository
{
   public  interface IBranchRepository:IRepository<Branch>
    {
       Branch GetBranchById(int? id);
       List<Branch> GetAllBranchesByPaging(int startPage, int pageSize, out int totalRecords, Branch branch);
       List<Branch> GetAllBranchesBySearchKey(string searchByBranchName, int searchByCompanyName);
       List<Branch> GetAllBranchesByCompanyId(int companyId);
    }
}
