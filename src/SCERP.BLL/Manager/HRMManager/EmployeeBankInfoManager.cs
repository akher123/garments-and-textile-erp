using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using SCERP.BLL.IManager.IHRMManager;
using SCERP.Common;
using SCERP.DAL.IRepository.IHRMRepository;
using SCERP.DAL.Repository.HRMRepository;
using SCERP.Model;
using SCERP.DAL;

namespace SCERP.BLL.Manager.HRMManager
{
    public class EmployeeBankInfoManager : BaseManager, IEmployeeBankInfoManager
    {
        private readonly IEmployeeBankInfoRepository _employeeBankInfoRepository = null;
        private readonly IBankAccountTypeRepository _bankAccountTypeRepository = null;

        public EmployeeBankInfoManager(SCERPDBContext context)
        {
            _employeeBankInfoRepository = new EmployeeBankInfoRepository(context);
            _bankAccountTypeRepository = new BankAccountTypeRepository(context);
        }

        public List<EmployeeBankInfo> GetEmployeeBankInfoByEmployeeId(Guid employeeId)
        {
            IQueryable<EmployeeBankInfo> employeeBankInfos;
            try
            {
                employeeBankInfos = _employeeBankInfoRepository
                                    .All()
                                    .Include(x=>x.BankAccountType)
                                    .Where(x => x.IsActive == true && x.EmployeeId == employeeId)
                                    .OrderByDescending(x=>x.Id);
            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message);
            }
            return employeeBankInfos.ToList();
        }

        public EmployeeBankInfo GetEmployeeBankInfoById(int id)
        {
            EmployeeBankInfo employeeBankInfo;

            try
            {
                employeeBankInfo = _employeeBankInfoRepository.FindOne(x => x.Id == id);
            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message);
            }
            return employeeBankInfo;
        }

        public EmployeeBankInfo GetEmployeeBankInfoById(Guid? employeeId, int? id)
        {
            EmployeeBankInfo employeeBankInfo;

            try
            {
                employeeBankInfo = _employeeBankInfoRepository.FindOne(x => x.EmployeeId == employeeId && x.Id == id);
            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message);
            }
            return employeeBankInfo;
        }

        public List<BankAccountType> GetAllBankAccountTypes()
        {
            List<BankAccountType> bankAccountTypes;
            try
            {
                bankAccountTypes = _bankAccountTypeRepository.Filter(x => x.IsActive).OrderBy(x => x.AccountType).ToList();
            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message);
            }
            return bankAccountTypes;
        }

        public int EditEmployeeBankInfo(EmployeeBankInfo employeeBankInfo)
        {
            var edit = 0;

            try
            {
                employeeBankInfo.EditedBy = PortalContext.CurrentUser.UserId;
                employeeBankInfo.EditedDate = DateTime.Now;
                edit = _employeeBankInfoRepository.Edit(employeeBankInfo);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }

            return edit;
        }

        public int SaveEmployeeBankInfo(EmployeeBankInfo employeeBankInfo)
        {
            int save = 0;
            try
            {
                employeeBankInfo.CreatedBy = PortalContext.CurrentUser.UserId;
                employeeBankInfo.CreatedDate = DateTime.Now;
                employeeBankInfo.IsActive = true;
                save = _employeeBankInfoRepository.Save(employeeBankInfo);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }

            return save;
        }


        public int DeleteEmployeeBankInfo(EmployeeBankInfo employeeBankInfo)
        {
            var delete = 0;
            try
            {
                employeeBankInfo.EditedBy = PortalContext.CurrentUser.UserId;
                employeeBankInfo.EditedDate = DateTime.Now;
                employeeBankInfo.IsActive = false;
                delete = _employeeBankInfoRepository.Edit(employeeBankInfo);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }

            return delete;

        }

        public BankAccountType GeEmployeeBankAccountTypeById(int? id)
        {
            return _bankAccountTypeRepository.GetById(id);
        }

        public EmployeeBankInfo GetLatestEmployeeBankInfoByEmployeeGuidId(Guid employeeId)
        {
            EmployeeBankInfo employeeBankInfo = null;
            try
            {
                employeeBankInfo = _employeeBankInfoRepository.GetLatestEmployeeBankInfoByEmployeeGuidId(employeeId);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
            return employeeBankInfo;
        }

        public int UpdateEmployeeBankInfoDate(EmployeeBankInfo employeeBankInfo)
        {
            var updated = 0;
            try
            {
                employeeBankInfo.EditedBy = PortalContext.CurrentUser.UserId;
                employeeBankInfo.EditedDate = DateTime.Now;
                updated = _employeeBankInfoRepository.Edit(employeeBankInfo);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
            return updated;
        }

    }
}
