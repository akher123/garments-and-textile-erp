using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.CommercialModel;

namespace SCERP.BLL.IManager.ICommonManager
{
   public interface ISalesContactManager
    {
        List<CommSalseContact> GetSalesContacts(int lcId);
        List<CommSalseContact> GetAllSalesContacts();
        int EditSalseContact(CommSalseContact modelSalseContact);
        int SaveSalseContact(CommSalseContact modelSalseContact);
        CommSalseContact GetSalseContactById(int salseContactId);
        
        int DeleteSalesContact(int salseContactId);
    }
}
