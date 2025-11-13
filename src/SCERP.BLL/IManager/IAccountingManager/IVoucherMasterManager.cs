using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;
using SCERP.Model.AccountingModel;

namespace SCERP.BLL.IManager.IAccountingManager
{
    public interface IVoucherMasterManager
    {
        int SaveVoucher(Acc_VoucherMaster voucherMaster);
        List<VAccVoucherMaster> GetVoucherList(VoucherList voucherList, out int totalRecord);
        Acc_VoucherMaster GetAllVoucherMaster(long id);
        int EditVoucher(Acc_VoucherMaster voucherMaster);
        List<VAccVoucherMaster> GetVoucherSummary(VoucherList voucherList);
        bool IsVoucherRefExist(VoucherList model);
        int DeleteVoucher(int? id);
        int SaveVoucherToCostCentre(Acc_VoucherToCostcentre voucherToCostcentre);
        int DeleteVouchertoCostCentre(Acc_VoucherToCostcentre voucherToCostcentre);
        IQueryable<Acc_VoucherToCostcentre> GetVoucherToCostCentre(long Id);
        string GetAccountNameByCode(decimal accountCode);
        string GetVoucherNoByType(string type, DateTime voucherDate);
        string GetCurrencyById(int id);
        int ChangeGlHeadGroup(string sectorId, string glId, string glIdNew);
        Acc_Currency GetAllCurrency();
        int? GetCostCentreByEmployeeId(Guid? employeeId);
        int ChangeGlHeadByParent(string sectorId, string glHead, string glHeadParent);
        DataTable GetChequeReport(int id, string compId);
        int SaveIniteGrationVoucher(Acc_VoucherMaster voucherMaster);
    }
}
