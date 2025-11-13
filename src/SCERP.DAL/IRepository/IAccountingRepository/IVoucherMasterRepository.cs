using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;
using SCERP.Model.AccountingModel;

namespace SCERP.DAL.IRepository.IAccountingRepository
{
    public interface IVoucherMasterRepository : IRepository<Acc_VoucherMaster>
    {
        IQueryable<VAccVoucherMaster> GetVoucherList(Expression<Func<VAccVoucherMaster, bool>> predicate);
        int SaveVoucherToCostCentre(Acc_VoucherToCostcentre voucherToCostcentre);
        int DeleteVouchertoCostCentre(Acc_VoucherToCostcentre voucherToCostcentre);
        IQueryable<Acc_VoucherToCostcentre> GetVoucherToCostCentre(long Id);
        string GetAccountNameByCode(decimal accountCode);
        string GetVoucherNoByType(string type, DateTime voucherDate);
        string GetCurrencyById(int id);
        int ChangeGlHeadGroup(string sectorId, string glId, string glIdNew);
        Acc_Currency GetCurrency();
        decimal GetConversionValueByVoucherId(long id);
        int? GetCostCentreByEmployeeId(Guid? employeeId);
        int ChangeGlHeadByParent(string sectorId, string glHead, string glHeadParent);
        int IncreaseRefno(string type, DateTime voucherDate);
    }
}
