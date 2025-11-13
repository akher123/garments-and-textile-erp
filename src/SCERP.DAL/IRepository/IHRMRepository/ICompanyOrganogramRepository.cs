using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;

namespace SCERP.DAL.IRepository.IHRMRepository
{
    public interface ICompanyOrganogramRepository:IRepository<CompanyOrganogram>
    {
        List<CompanyOrganogram> GetAllDesignations();
        CompanyOrganogram GetTopDesignation();
        void SetChildren(CompanyOrganogram designation, List<CompanyOrganogram> designationList);
        CompanyOrganogram GetHierarchyByEmployeeDesignation(int designationID);

    }
}
