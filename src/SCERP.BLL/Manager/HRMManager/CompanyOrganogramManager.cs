using System;
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
    public class CompanyOrganogramManager : BaseManager, ICompanyOrganogramManager
    {

        private readonly ICompanyOrganogramRepository _companyOrganogramRepository = null;

        public CompanyOrganogramManager(SCERPDBContext context)
        {
            _companyOrganogramRepository = new CompanyOrganogramRepository(context);
        }

        public List<CompanyOrganogram> GetAllDesignations()
        {
            var designations = new List<CompanyOrganogram>();

            try
            {
                designations = _companyOrganogramRepository.GetAllDesignations();
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
             
            }

            return designations;
        }

        public CompanyOrganogram GetTopDesignation()
        {
            CompanyOrganogram topDsignation = null;

            try
            {
                topDsignation = _companyOrganogramRepository.GetTopDesignation();
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                topDsignation = null;
            }

            return topDsignation;
        }

        public void SetChildren(CompanyOrganogram designation, List<CompanyOrganogram> designationList)
        {
            try
            {
                _companyOrganogramRepository.SetChildren(designation, designationList);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
        }


        public int? SaveHierarchy(CompanyOrganogram companyOrganogram)
        {
            int? saved = 0;

            try
            {
                saved = _companyOrganogramRepository.Save(companyOrganogram);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                saved = 0;
            }

            return saved;
        }

        public int? EditHierarchy(CompanyOrganogram companyOrganogram)
        {
            int? saved = null;

            try
            {
                saved = _companyOrganogramRepository.Edit(companyOrganogram);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                saved = null;
            }

            return saved;
        }


        public CompanyOrganogram GetHierarchyByEmployeeDesignation(int designationID)
        {
            CompanyOrganogram organogram = null;
            try
            {
                organogram = _companyOrganogramRepository.GetHierarchyByEmployeeDesignation(designationID);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                organogram = null;
            }
            return organogram;

        }
    }
}
