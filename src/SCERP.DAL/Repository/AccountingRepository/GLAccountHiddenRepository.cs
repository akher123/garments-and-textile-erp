using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using SCERP.DAL.IRepository.IAccountingRepository;
using SCERP.Model;
using SCERP.Model.AccountingModel;


namespace SCERP.DAL.Repository.AccountingRepository
{
    public class GLAccountHiddenRepository : Repository<Acc_GLAccounts_Hidden>, IGLAccountHiddenRepository
    {
        public GLAccountHiddenRepository(SCERPDBContext context)
            : base(context)
        {

        }

        public override IQueryable<Acc_GLAccounts_Hidden> All()
        {
            return Context.Acc_GLAccounts_Hiddens.Where(x => x.IsActive == true);
        }

        public Acc_GLAccounts_Hidden GetGLAccountHiddenById(int? id)
        {
            return Context.Acc_GLAccounts_Hiddens.Find(id);
        }

        public List<Acc_GLAccounts_Hidden> GetAllGLAccountHiddens(int startPage, int pageSize, out int totalRecordsHidden)
        {
            var GLAccountHidden = Context.Acc_GLAccounts_Hiddens.Where(p => p.IsActive == true).ToList();

            totalRecordsHidden = GLAccountHidden.Count();

            GLAccountHidden = GLAccountHidden.Skip(startPage * pageSize).Take(pageSize).ToList();

            return GLAccountHidden;
        }

        public List<Acc_GLAccounts_Hidden> GetAllGLAccountVisibles(int startPage, int pageSize, out int totalRecordsVisible)
        {
            var GLAccountVisible = Context.Acc_GLAccounts_Hiddens.Where(p => p.IsActive == false).ToList();

            totalRecordsVisible = GLAccountVisible.Count();

            GLAccountVisible = GLAccountVisible.Skip(startPage * pageSize).Take(pageSize).ToList();

            return GLAccountVisible;
        }

        public IQueryable<Acc_GLAccounts_Hidden> GetAllGLAccountHiddens()
        {
            var GLAccountHidden = Context.Acc_GLAccounts_Hiddens.Where(r => r.IsActive == false);
            return GLAccountHidden;
        }

        public string SaveGLHead(decimal accountCode)
        {
            var accountHidden = Context.Acc_GLAccounts_Hiddens.Where(p => p.AccountCode == accountCode);

            if (accountHidden.Count() > 0)
            {
                return "Account Head already Exists !";
            }

            var account = Context.Acc_GLAccounts.Where(p => p.AccountCode == accountCode).SingleOrDefault();

            Acc_GLAccounts_Hidden accHidden = new Acc_GLAccounts_Hidden();
            accHidden.ControlCode = account.ControlCode;
            accHidden.AccountCode = account.AccountCode;
            accHidden.AccountName = account.AccountName;
            accHidden.IsActive = true;

            Context.Acc_GLAccounts_Hiddens.Add(accHidden);
            Context.SaveChanges();

            return "Successfully Saved !";
        }

        public string SaveStatus(bool status)
        {
            var hiddenStatus = from p in Context.Acc_GLAccounts_Hidden_Statuses
                               where p.Id == 1
                               select p;

            foreach (var t in hiddenStatus)
            {
                t.Status = status;
            }

            int result = Context.SaveChanges();
            if (result == 1)
                return "Successfully Saved !";
            else
                return "Failed to save data !";
        }

        public int GetStatus()
        {
            bool result = (from p in Context.Acc_GLAccounts_Hidden_Statuses
                           where p.Id == 1
                           select p).SingleOrDefault().Status;
            if (result)
                return 1;
            else
                return 2;
        }
    }
}