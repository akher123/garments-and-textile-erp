using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.AccountingModel;


namespace SCERP.BLL.IManager.IAccountingManager
{
    public interface IStylePaymentManager
    {
        List<VStylePayment> GetAllStylePaymentByPaging(Acc_StylePayment model, out int totalRecords);
        List<Acc_StylePayment> GetAllStylePayments();
        Acc_StylePayment GetStylePaymentsByStylePaymentsId(int stylePaymentId);
        int SaveStylePayment(Acc_StylePayment model);
        int EditStylePayment(Acc_StylePayment model);
        int DeleteStylePayment(int stylePaymnetId);
        bool IsStylePaymentExist(Acc_StylePayment model);
        string GetNewStylePaymentRefId(string prifix);
    }
}
