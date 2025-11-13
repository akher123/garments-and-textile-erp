using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;

namespace SCERP.DAL.IRepository.IHRMRepository
{
    public interface ICompanyRepository:IRepository<Company>
    {
        Company GetCompanyById(int? id);      
        List<Company> GetAllCompaniesByPaging(int startPage, int pageSize, out int totalRecords, Company company);
        List<Company> GetAllCompaniesBySearchKey(string searchByCompanyName);
    }
}
