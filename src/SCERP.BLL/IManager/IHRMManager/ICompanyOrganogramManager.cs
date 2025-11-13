using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;
using System.Linq;

namespace SCERP.BLL.IManager.IHRMManager
{
    public interface ICompanyOrganogramManager
    {
        List<CompanyOrganogram> GetAllDesignations();

        CompanyOrganogram GetTopDesignation();

        void SetChildren(CompanyOrganogram designation, List<CompanyOrganogram> designationList);

        int? SaveHierarchy(CompanyOrganogram companyOrganogram);

        CompanyOrganogram GetHierarchyByEmployeeDesignation(int designationID);

        int? EditHierarchy(CompanyOrganogram companyOrganogram);
    }
}
