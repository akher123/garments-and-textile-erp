using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;

namespace SCERP.BLL.IManager.IMerchandisingManager
{
    public interface IPaymentTermManager
    {
        List<OM_PaymentTerm> GetPaymentTerms();
        List<OM_PaymentTerm> GetPaymentTermByPaging(OM_PaymentTerm model, out int totalRecords);
        OM_PaymentTerm GetPaymentTermById(int payentTermId);
        string GetPayTermRef();
        int EditPaymentTerm(OM_PaymentTerm model);
        int SavePaymentTerm(OM_PaymentTerm model);
        int DeletePaymentTerm(string payTermRefId);
        bool CheckExistingPaymentTerm(OM_PaymentTerm model);
    }
}
