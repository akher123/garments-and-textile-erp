using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.DAL.IRepository;
using SCERP.Model;

namespace SCERP.BLL.IManager.IAccountingManager
{
    public interface IGLAccountManager
    {
        object GetAutocompliteGLAccount(string accountName);
    }
}
