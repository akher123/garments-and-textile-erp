using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.BLL.IManager.IAccountingManager;
using SCERP.DAL;
using SCERP.DAL.IRepository.IAccountingRepository;
using SCERP.DAL.Repository.AccountingRepository;

namespace SCERP.BLL.Manager.AccountingManager
{
    public class GLAccountManager : IGLAccountManager
    {
        private IGLAccountRepository _glAccountRepository = null;
        public GLAccountManager(IGLAccountRepository glAccountRepository)
        {
            _glAccountRepository = glAccountRepository;
        }

        public object GetAutocompliteGLAccount(string accountName)
        {
            var glAccList =
                _glAccountRepository.Filter(
                    x =>
                        x.IsActive == true &&
                        x.AccountName.ToLower().Replace(" ", "").Contains(accountName.ToLower().Replace(" ", ""))).Select(x=>new
                        {
                            x.AccountCode,
                            x.AccountName,x.Id
                        }).Take(15).OrderBy(x=>x.AccountName).ToList();

            var hiddenList  = _glAccountRepository.GetHiddenGlAccounts();

            foreach (var qs in hiddenList)
            {
                glAccList = glAccList.Where(x => x.AccountCode != qs.AccountCode).ToList();
            }

            return glAccList;
        }
    }
}
