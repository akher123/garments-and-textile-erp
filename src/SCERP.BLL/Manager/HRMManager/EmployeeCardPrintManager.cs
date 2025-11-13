using System;
using System.Collections.Generic;
using SCERP.BLL.IManager.IHRMManager;
using SCERP.Common;
using SCERP.DAL;
using SCERP.DAL.IRepository.IHRMRepository;
using SCERP.DAL.Repository.HRMRepository;
using SCERP.Model;
using SCERP.Model.Custom;


namespace SCERP.BLL.Manager.HRMManager
{
    public class EmployeeCardPrintManager : BaseManager, IEmployeeCardPrintManager
    {
        private readonly IEmployeeCardPrintRepository _employeeCardPrintRepository = null;

        public EmployeeCardPrintManager(SCERPDBContext context)
        {
            _employeeCardPrintRepository = new EmployeeCardPrintRepository(context);
        }

        public List<EmployeeCardPrintModel> GetEmployeeIDCardInfoByPaging(int startPage, int pageSize, out int totalRecords, Employee model, SearchFieldModel searchFieldModel)
        {
            if (searchFieldModel.SearchLanguageId == (int) Enum.Parse(typeof (LanguageType), "Bangla"))
            {
                return _employeeCardPrintRepository.GetEmployeeIDCardInfoInBengaliByPaging(startPage, pageSize, out totalRecords, model, searchFieldModel);  
            }

            return _employeeCardPrintRepository.GetEmployeeIDCardInfoInEnglishByPaging(startPage, pageSize, out totalRecords, model, searchFieldModel);  
            
        }

        public List<EmployeeCardPrintModel> GetEmployeeIDCardInfo(List<Guid> employeeIdList, SearchFieldModel searchFieldModel)
        {
            if (searchFieldModel.SearchLanguageId == (int)Enum.Parse(typeof(LanguageType), "Bangla"))
            {
                return _employeeCardPrintRepository.GetEmployeeIDCardInfoInBengali(employeeIdList, searchFieldModel);
            }

            return _employeeCardPrintRepository.GetEmployeeIDCardInfoInEnglish(employeeIdList, searchFieldModel);

        }

        public List<EmployeeCardInfo> GetCardBackInfo(int companyId, int language, int noofCard)
        {
            return _employeeCardPrintRepository.GetCardBackInfo(companyId, language, noofCard);
        }
    }
}
