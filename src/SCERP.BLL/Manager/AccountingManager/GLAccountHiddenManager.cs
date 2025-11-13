using System;
using SCERP.BLL.IManager.IAccountingManager;
using SCERP.DAL;
using SCERP.DAL.IRepository.IAccountingRepository;
using SCERP.DAL.Repository.AccountingRepository;
using System.Linq;
using SCERP.Model;
using System.Collections.Generic;
using SCERP.Model.AccountingModel;


namespace SCERP.BLL.Manager.AccountingManager
{
    public class GLAccountHiddenManager : BaseManager, IGLAccountHiddenManager
    {

        private IGLAccountHiddenRepository GLAccountHiddenRepository = null;

        public GLAccountHiddenManager(SCERPDBContext context)
        {
            this.GLAccountHiddenRepository = new GLAccountHiddenRepository(context);
        }

        public List<Acc_GLAccounts_Hidden> GetAllGLAccountHiddens(int startPage, int pageSize, out int totalRecordsHidden)
        {
            return GLAccountHiddenRepository.GetAllGLAccountHiddens(startPage, pageSize, out totalRecordsHidden);
        }

        public List<Acc_GLAccounts_Hidden> GetAllGLAccountVisibles(int startPage, int pageSize, out int totalRecordsVisible)
        {
            return GLAccountHiddenRepository.GetAllGLAccountVisibles(startPage, pageSize, out totalRecordsVisible);
        }

        public Acc_GLAccounts_Hidden GetGLAccountHiddenById(int? id)
        {
            return GLAccountHiddenRepository.GetGLAccountHiddenById(id);
        }

        public string SaveGLAccountHidden(Acc_GLAccounts_Hidden glAccountHidden)
        {

            string savedStatus = "";

            decimal accountCode = Convert.ToDecimal(glAccountHidden.AccountName.Split('-').ElementAt(glAccountHidden.AccountName.Split('-').Count() - 1));

            try
            {
                savedStatus = GLAccountHiddenRepository.SaveGLHead(accountCode);
            }

            catch (Exception ex)
            {
                savedStatus = "Failed to Save !";
            }

            return savedStatus;
        }

        public void DeleteGLAccountHidden(Acc_GLAccounts_Hidden GLAccountHidden)
        {
            GLAccountHidden.IsActive = false;
            GLAccountHiddenRepository.Edit(GLAccountHidden);
        }

        public void MakeGLAccountHidden(Acc_GLAccounts_Hidden GLAccountHidden)
        {
            GLAccountHidden.IsActive = true;
            GLAccountHiddenRepository.Edit(GLAccountHidden);
        }

        public void MakeGLAccountVisible(Acc_GLAccounts_Hidden GLAccountHidden)
        {
            GLAccountHidden.IsActive = false;
            GLAccountHiddenRepository.Edit(GLAccountHidden);
        }

        public string SaveStatus(bool status)
        {
            return GLAccountHiddenRepository.SaveStatus(status);
        }

        public int GetStatus()
        {
            return GLAccountHiddenRepository.GetStatus();
        }
    }
}