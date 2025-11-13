using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;
using SCERP.Model.AccountingModel;

namespace SCERP.DAL.IRepository.IAccountingRepository
{
    public interface IGLAccountHiddenRepository : IRepository<Acc_GLAccounts_Hidden>
    {
        Acc_GLAccounts_Hidden GetGLAccountHiddenById(int? id);
        List<Acc_GLAccounts_Hidden> GetAllGLAccountHiddens(int startPage, int pageSize, out int totalRecordsHidden);
        List<Acc_GLAccounts_Hidden> GetAllGLAccountVisibles(int startPage, int pageSize, out int totalRecordsVisible);
        IQueryable<Acc_GLAccounts_Hidden> GetAllGLAccountHiddens();
        string SaveGLHead(decimal accountCode);
        string SaveStatus(bool status);
        int GetStatus();
    }
}