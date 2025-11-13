using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using SCERP.BLL.IManager.IAccountingManager;
using SCERP.Common;
using SCERP.DAL.IRepository.IAccountingRepository;
using SCERP.Model;

namespace SCERP.BLL.Manager.AccountingManager
{
    public class ControlAccountManager : IControlAccountManager
    {
        private readonly IControlAccountRepository _controlAccountRepository = null;
        private readonly IGLAccountRepository _glAccountRepository = null;
        private readonly ICompanySectorRepository _companySectorRepository = null;
        private readonly IPermitedChartOfAccountRepository _permitedChartOfreChartOfAccountRepository = null;


        public ControlAccountManager(IControlAccountRepository controlAccountRepository,
            IGLAccountRepository glAccountRepository, ICompanySectorRepository companySectorRepository,
            IPermitedChartOfAccountRepository permitedChartOfreChartOfAccountRepository)
        {
            this._controlAccountRepository = controlAccountRepository;
            this._glAccountRepository = glAccountRepository;
            this._companySectorRepository = companySectorRepository;
            this._permitedChartOfreChartOfAccountRepository = permitedChartOfreChartOfAccountRepository;

        }

        public List<Acc_ControlAccounts> GetAllControlAccounts()
        {
            return _controlAccountRepository.Filter(x => x.IsActive == true).OrderBy(x => x.ParentCode).ToList();
        }

        public List<Acc_GLAccounts> GetAllGLAccounts(string searchKey)
        {
            List<Acc_GLAccounts> hiddenList = _glAccountRepository.GetHiddenGlAccounts();

            List<Acc_GLAccounts> list = new List<Acc_GLAccounts>();

            if (hiddenList.Count > 0)
            {
                list = _glAccountRepository.Filter(x => x.IsActive == true && (x.AccountName.ToLower().Replace(" ", String.Empty).Contains(searchKey.ToLower().Replace(" ", String.Empty)) || string.IsNullOrEmpty(searchKey))).ToList();

                list = (from p in list
                          where !(from e in hiddenList
                                  select e.AccountCode).Contains(p.AccountCode)
                          select p).ToList();

                return list;
            }

            return _glAccountRepository.Filter(x => x.IsActive == true && (x.AccountName.ToLower().Replace(" ", String.Empty).Contains(searchKey.ToLower().Replace(" ", String.Empty)) || string.IsNullOrEmpty(searchKey))).ToList();
        }

        public List<Acc_GLAccounts> GetGLAccountsByControlCode(string controlCode)
        {
            decimal controlcode = Convert.ToDecimal(controlCode);
            return _glAccountRepository.Filter(x => x.IsActive == true && x.ControlCode == controlcode).ToList();
        }

        public int GetMaxControlCode(int parentCode)
        {
            return _controlAccountRepository.GetMaxControlCode(parentCode);
        }

        public int SaveControlAccount(Acc_ControlAccounts accControlAccount)
        {
            int index = 0;
            try
            {
                bool isExist = _glAccountRepository.Exists(x => x.ControlCode == accControlAccount.ControlCode);

                using (var transaction = new TransactionScope())
                {
                    if (accControlAccount.ControlLevel != null && accControlAccount.ControlLevel == 4 && !isExist)
                    {
                        var glAccount = new Acc_GLAccounts()
                        {
                            ControlCode = accControlAccount.ControlCode,
                            AccountName = accControlAccount.ControlName,
                            AccountCode =
                                Convert.ToDecimal(
                                    Convert.ToString(Convert.ToInt64(accControlAccount.ControlCode) + "000")),
                            IsActive = true
                        };
                        index += _glAccountRepository.Save(glAccount);
                    }
                    index += _controlAccountRepository.Save(accControlAccount);
                    transaction.Complete();
                }
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                index = 0;
            }
            return index;
        }

        public int SaveControlAccountTransfer(Acc_ControlAccounts accControlAccount)
        {
            int index = 0;

            try
            {
                using (var transaction = new TransactionScope())
                {
                    index += _controlAccountRepository.Save(accControlAccount);
                    transaction.Complete();
                }
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                index = 0;
            }
            return index;
        }

        public decimal GetMaxGlControlCode(int newParentCode)
        {
            return _glAccountRepository.GetMaxGlControlCode(newParentCode);
        }

        public Acc_ControlAccounts GetControlAccountsById(int id)
        {
            return _controlAccountRepository.FindOne(x => x.Id == id);
        }

        public Acc_GLAccounts GetGLAccountsById(int id)
        {
            return _glAccountRepository.FindOne(x => x.Id == id);
        }

        public int EditControlAccount(Acc_ControlAccounts model)
        {
            int edit = 0;
            try
            {
                var accControlAccounts = _controlAccountRepository.FindOne(x => x.Id == model.Id);
                accControlAccounts.ControlCode = model.ControlCode;
                accControlAccounts.ControlLevel = model.ControlLevel;
                accControlAccounts.ControlName = model.ControlName;
                accControlAccounts.ParentCode = model.ParentCode;
                accControlAccounts.SortOrder = model.SortOrder;
                accControlAccounts.IsActive = model.IsActive;
                if (accControlAccounts.ControlLevel == 4)
                {
                    var glAccountCode = accControlAccounts.ControlCode * 1000;

                    var glAccounts = _glAccountRepository.FindOne(x => x.AccountCode == glAccountCode);

                    if (glAccounts != null)
                    {
                        glAccounts.AccountName = model.ControlName;
                        edit += _glAccountRepository.Edit(glAccounts);
                    }
                }
                edit += _controlAccountRepository.Edit(accControlAccounts);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                edit = 0;
            }

            return edit;
        }

        public int SaveglAccount(Acc_GLAccounts glAccount)
        {
            int index = 0;
            try
            {
                var accountCode = Convert.ToDecimal(Convert.ToString(Convert.ToInt64(glAccount.ControlCode) + "000"));
                var exists = _glAccountRepository.Exists(x => x.AccountCode == accountCode);
                if (exists)
                {
                    var oldGlAccount =
                        _glAccountRepository.FindOne(x => x.ControlCode == glAccount.ControlCode &&
                                                          x.AccountCode == accountCode);

                    oldGlAccount.AccountCode = glAccount.AccountCode;
                    oldGlAccount.ControlCode = glAccount.ControlCode;
                    oldGlAccount.AccountName = glAccount.AccountName;
                    index = _glAccountRepository.Edit(oldGlAccount);
                }
                else
                {
                    index = _glAccountRepository.Save(glAccount);
                }

            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                index = 0;
            }
            return index;
        }

        public int EditglAccount(Acc_GLAccounts model)
        {
            var edit = 0;
            try
            {
                var accGlAccounts = _glAccountRepository.FindOne(x => x.Id == model.Id);

                accGlAccounts.AccountCode = model.AccountCode;
                accGlAccounts.AccountName = model.AccountName;
                accGlAccounts.AccountType = model.AccountType;
                accGlAccounts.BalanceType = model.BalanceType;
                accGlAccounts.IsActive = model.IsActive;
                accGlAccounts.ControlCode = model.ControlCode;
                edit = _glAccountRepository.Edit(accGlAccounts);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                edit = 0;
            }
            return edit;
        }

        public int DeleteControlAccount(int id)
        {
            int edit = 0;
            try
            {
                Acc_ControlAccounts accControlAccounts = _controlAccountRepository.FindOne(x => x.Id == id);
                accControlAccounts.IsActive = false;
                edit = _controlAccountRepository.Edit(accControlAccounts);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                edit = 0;
            }
            return edit;
        }

        public int DeleteGLAccount(int id)
        {
            int edit = 0;
            try
            {
                var accGlAccounts = _glAccountRepository.FindOne(x => x.Id == id);
                accGlAccounts.IsActive = false;
                edit = _glAccountRepository.Edit(accGlAccounts);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                edit = 0;
            }
            return edit;
        }

        public List<Acc_CompanySector> GetAllCompanySector()
        {
            return _companySectorRepository.All().ToList();
        }

        public int SavePermitedChartOfAccounts(List<Acc_PermitedChartOfAccount> paOfAccounts)
        {
            int saveList = 0;

            List<Acc_PermitedChartOfAccount> chartOfAccounts = paOfAccounts.ToList();

            try
            {
                List<Acc_PermitedChartOfAccount> existsPermitedChartOfAccounts =
                    GetPermitedChartOfAccount(paOfAccounts[0].SectorId);
                if (existsPermitedChartOfAccounts == null)
                {
                    return _permitedChartOfreChartOfAccountRepository.SaveList(paOfAccounts);
                }
                foreach (var existsCoA in existsPermitedChartOfAccounts)
                {
                    if (paOfAccounts.Exists(x => x.SectorId == existsCoA.SectorId &&
                                                 x.ControlCode == existsCoA.ControlCode &&
                                                 x.ControlLevel == existsCoA.ControlLevel))
                    {
                        chartOfAccounts.RemoveAll(
                            x =>
                                x.SectorId == existsCoA.SectorId && x.ControlCode == existsCoA.ControlCode &&
                                x.ControlLevel == existsCoA.ControlLevel);
                    }
                    else
                    {
                        Acc_PermitedChartOfAccount aCoA = existsCoA;
                        saveList = _permitedChartOfreChartOfAccountRepository.Delete(
                                       x => x.Id == aCoA.Id && x.SectorId == aCoA.SectorId &&
                                            x.ControlCode == aCoA.ControlCode && x.ControlLevel == aCoA.ControlLevel) ==
                                   0
                            ? 1
                            : 0;
                    }
                }
                if (chartOfAccounts.Count > 0)
                {
                    saveList = _permitedChartOfreChartOfAccountRepository.SaveList(chartOfAccounts);
                }
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                saveList = 0;
            }

            return saveList;
        }

        public List<Acc_PermitedChartOfAccount> GetPermitedChartOfAccount(int companySectorId)
        {
            return _permitedChartOfreChartOfAccountRepository.GetPermitedChartOfAccount(companySectorId);
        }

        public bool CheckExistVoucherByGLId(int controlLevel, int id, ref string message)
        {
            return _permitedChartOfreChartOfAccountRepository.CheckExistVoucherByGLId(controlLevel, id, ref message);
        }

        public bool CheckExistingName(Acc_GLAccounts glAccount)
        {
            return _permitedChartOfreChartOfAccountRepository.CheckExistingName(glAccount);
        }

        public bool CheckExistingName(Acc_ControlAccounts controlAccounts)
        {
            return _permitedChartOfreChartOfAccountRepository.CheckExistingName(controlAccounts);
        }

        public int GetGLAccountIdByCode(string accountCode)
        {
            return _permitedChartOfreChartOfAccountRepository.GetGLAccountIdByCode(accountCode);
        }

        public int GetControlIdByCode(string controlCode)
        {
            return _permitedChartOfreChartOfAccountRepository.GetControlIdByCode(controlCode);
        }

        public string GetControlNameByCode(string controlcode)
        {
            return _controlAccountRepository.GetControlNameByCode(controlcode);
        }

        public int ControltoSubGroupChange(string SubGroupCode, string ControlCode)
        {
            return _controlAccountRepository.ControltoSubGroupChange(SubGroupCode, ControlCode);
        }
        public int GLtoControlChange(string GLCode, string ControlCode)
        {
            return _controlAccountRepository.GLtoControlChange(GLCode, ControlCode);
        }
        public int GLtoSubGroupChange(string GLCode, string SubGroup)
        {
            return _controlAccountRepository.GLtoSubGroupChange(GLCode, SubGroup);
        }
    }
}
