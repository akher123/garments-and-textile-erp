
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SCERP.Model;

namespace SCERP.BLL.IManager.IHRMManager
{
    public interface ICompanyManager
    {
        List<Company> GetAllCompaniesByPaging(int startPage, int pageSize, out int totalRecords, Company company);

        List<Company> GetAllCompanies(string compId);

        IEnumerable GetAllPermittedCompanies(); // From Portal Context

        Company GetCompanyById(int? id);

        int SaveCompany(Company company);

        int EditCompany(Company company);

        int DeleteCompany(Company company);

        Company GetCompanyInfo();

        bool CheckExistingCompany(Company company);

        List<Company> GetAllCompaniesBySearchKey(string companyName);
        string GetCompanyDomaun(string compId);
        string GetNewCompanyRefId();
    }
}
