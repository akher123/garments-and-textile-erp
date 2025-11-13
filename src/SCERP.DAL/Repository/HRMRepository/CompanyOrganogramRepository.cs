using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using SCERP.DAL.IRepository.IHRMRepository;
using SCERP.Model;

namespace SCERP.DAL.Repository.HRMRepository
{
    public class CompanyOrganogramRepository : Repository<CompanyOrganogram>, ICompanyOrganogramRepository
    {
        public CompanyOrganogramRepository(SCERPDBContext context)
            : base(context)
        {
        }

        public List<CompanyOrganogram> GetAllDesignations()
        {
            return Context.CompanyOrganograms.Where(x => x.IsActive == true).Include(p => p.EmployeeDesignation).ToList();
        }

        public CompanyOrganogram GetTopDesignation()
        {
            return Context.CompanyOrganograms.FirstOrDefault(x => x.ParentDesignationId == null && x.IsActive == true);
        }

        public void SetChildren(CompanyOrganogram designation, List<CompanyOrganogram> designationList)
        {
            var designations = designationList.Where(x => x.ParentDesignationId == designation.DesignationId && x.IsActive == true).ToList();
            if (!designations.Any()) return;
            foreach (var singledesignation in designations)
            {
                SetChildren(singledesignation, designationList);
                designation.CompanyOrganograms.Add(singledesignation);
            }
        }

        public CompanyOrganogram GetHierarchyByEmployeeDesignation(int designationID)
        {
            return Context.CompanyOrganograms.FirstOrDefault(x => x.IsActive == true && x.DesignationId == designationID);
        }

    }
}
