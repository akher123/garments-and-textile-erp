using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SCERP.BLL.IManager.IHRMManager;
using SCERP.Common;
using SCERP.DAL.IRepository.IHRMRepository;
using SCERP.DAL.Repository.HRMRepository;
using SCERP.Model;
using SCERP.DAL;

namespace SCERP.BLL.Manager.HRMManager
{
    public class CompanyManager : BaseManager, ICompanyManager
    {
        private readonly ICompanyRepository _companyRepository = null;

        public CompanyManager(SCERPDBContext context)
        {
            _companyRepository = new CompanyRepository(context);
        }
     

        public List<Company> GetAllCompaniesByPaging(int startPage, int pageSize, out int totalRecords, Company company)
        {
            var companies = new List<Company>();
            try
            {
                companies = _companyRepository.GetAllCompaniesByPaging(startPage, pageSize, out totalRecords, company).ToList();
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                totalRecords = 0;
            }

            return companies;
        }


        public List<Company> GetAllCompanies()
        {
            var companies=new List<Company>();
            try
            {
                companies = _companyRepository.Filter(x => x.IsActive).OrderBy(x => x.Name).ToList();
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
             
            }

            return companies;
        }


        public IEnumerable GetAllPermittedCompanies() // From portal context
        {

            IEnumerable companies=new List<Object>();
            try
            {              
                companies = PortalContext.CurrentUser.PermissionContext.CompanyList;

            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }

            return companies;
        }


        public Company GetCompanyById(int? id)
        {
            Company company;
            try
            {
                company = _companyRepository.GetCompanyById(id);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                company = null;
            }
            return company;
        }

       
        public bool CheckExistingCompany(Company company)
        {
            var isExist = false;
            try
            {
                isExist = _companyRepository
                    .Exists(x =>x.IsActive && x.Id != company.Id && x.Name.Replace(" ", "")
                    .ToLower().Equals(company.Name.Replace(" ", "").ToLower()));
            }
            catch (Exception excption)
            {
                Errorlog.WriteLog(excption);
            }
            return isExist;
        }

     
        public int SaveCompany(Company company)
        {
            int saveCompany;

            try
            {
                company.CreatedDate = DateTime.Now;
                company.CreatedBy = PortalContext.CurrentUser.UserId;
                saveCompany = _companyRepository.Save(company);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                saveCompany = 0;
            }

            return saveCompany;
        }


        public int EditCompany(Company company)
        {
            int edit;
            try
            {
                company.EditedDate = DateTime.Now;
                company.EditedBy = PortalContext.CurrentUser.UserId;
                edit = _companyRepository.Edit(company);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                edit = 0;
            }
            return edit;
        }


        public int DeleteCompany(Company company)
        {
            company.IsActive = false;
            int edit;
            try
            {
                edit = _companyRepository.Edit(company);
            }
            catch (Exception exception)
            {
                edit = 0;
                Errorlog.WriteLog(exception);
                
            }
            return edit;
           
        }

        public Company GetCompanyInfo()
        {
            return _companyRepository.All().First(x => x.IsActive == true);
        }

        public List<Company> GetAllCompaniesBySearchKey(string companyName)
        {
            var companies = new List<Company>();

            try
            {
                companies = _companyRepository.GetAllCompaniesBySearchKey(companyName);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return companies;
        }

        public List<Company> GetAllCompanies(string compId)
        {
            var companies = new List<Company>();
            try
            {
                if (PortalContext.CurrentUser.IsSystemUser)
                {
                    companies = _companyRepository.Filter(x => x.IsActive).OrderBy(x => x.Name).ToList();
                }
                else
                {
                    companies = _companyRepository.Filter(x => x.IsActive && x.CompanyRefId == compId).OrderBy(x => x.Name).ToList();
                }

            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);

            }

            return companies;
        }

        public string GetCompanyDomaun(string compId)
        {
            var company = _companyRepository.FindOne(x => x.IsActive == true && x.CompanyRefId == compId) ?? new Company();
            return company.DomainName;
        }

        public string GetNewCompanyRefId()
        {
            var maxId = _companyRepository.All().Max(x => x.CompanyRefId);
            return maxId.IncrementOne().PadZero(3);
        }
    }
}
