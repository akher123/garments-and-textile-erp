using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Remoting.Contexts;
using System.Transactions;
using SCERP.BLL.IManager.IAccountingManager;
using SCERP.Common;
using SCERP.DAL;
using SCERP.DAL.IRepository;
using SCERP.DAL.IRepository.IAccountingRepository;
using SCERP.DAL.IRepository.IInventoryRepository;
using SCERP.DAL.IRepository.IProductionRepository;
using SCERP.DAL.Repository.AccountingRepository;
using SCERP.Model;
using SCERP.Model.AccountingModel;
using SCERP.Model.MerchandisingModel;

namespace SCERP.BLL.Manager.AccountingManager
{
    public class VoucherMasterManager : IVoucherMasterManager
    {
        private readonly IVoucherMasterRepository _voucherMasterRepository;
        private readonly IVoucherDetailRepository _voucherDetailRepository;
        private readonly IGreyIssueRepository greyIssueRepository;
        private readonly IFinishFabricIssueRepository finishFabricIssueRepository;
        private readonly IProcessReceiveRepository processReceiveRepository;

        private readonly Guid? _employeeGuidId = PortalContext.CurrentUser.UserId;

        private readonly IRepository<PROD_KnittingRollIssue> _knittinRollissueRepository;
        public VoucherMasterManager(IProcessReceiveRepository processReceiveRepository,IFinishFabricIssueRepository finishFabricIssueRepository,IGreyIssueRepository greyIssueRepository,IRepository<PROD_KnittingRollIssue> knittinRollissueRepository, VoucherMasterRepository voucherMasterRepository, IVoucherDetailRepository voucherDetailRepository)
        {
            _voucherMasterRepository = voucherMasterRepository;
            _voucherDetailRepository = voucherDetailRepository;
            _knittinRollissueRepository = knittinRollissueRepository;
            this.greyIssueRepository = greyIssueRepository;
            this.finishFabricIssueRepository = finishFabricIssueRepository;
            this.processReceiveRepository = processReceiveRepository;
    }

        public int SaveVoucher(Acc_VoucherMaster voucherMaster)
        {            
          

            var saveIndex = _voucherMasterRepository.Save(voucherMaster);

            var result = _voucherMasterRepository.IncreaseRefno(voucherMaster.VoucherType, voucherMaster.VoucherDate);

            return saveIndex;
        }

        public List<VAccVoucherMaster> GetVoucherList(VoucherList model, out int totalRecord)
        {
            var index = model.PageIndex;
            var pageSize = AppConfig.PageSize;

            int currencyId = 0;
            var sectorId = GetCostCentreByEmployeeId(_employeeGuidId);

            if (!string.IsNullOrEmpty(model.VoucherRefNo) && Enum.IsDefined(typeof (CurrencyType), model.VoucherRefNo.ToUpper() ?? "abc"))
            {
                currencyId = (int) Enum.Parse(typeof (CurrencyType), model.VoucherRefNo.ToUpper());
            }

            Expression<Func<VAccVoucherMaster, bool>> predicate;

            predicate = x => (
                ((x.VoucherRefNo.Trim().Contains(model.VoucherRefNo) || String.IsNullOrEmpty(model.VoucherRefNo.Trim()))
                 || (x.VoucherType.Trim().ToLower().Contains(model.VoucherRefNo.ToLower()) || String.IsNullOrEmpty(model.VoucherRefNo.Trim().ToLower()))
                 || (SqlFunctions.StringConvert(x.Amount).Contains(model.VoucherRefNo.Trim()) || String.IsNullOrEmpty(model.VoucherRefNo.Trim()))
                 || (SqlFunctions.StringConvert((double?) x.VoucherNo).Contains(model.VoucherRefNo.Trim()) || String.IsNullOrEmpty(model.VoucherRefNo.Trim()))
                 || (x.ActiveCurrencyId == currencyId)
                    )
                && ((x.Date >= model.FromDate || model.FromDate == null) && (x.Date <= model.ToDate || model.ToDate == null))
                && x.SectorId == sectorId
                );

            var dbRawSqlQuery = _voucherMasterRepository.GetVoucherList(predicate);

            totalRecord = dbRawSqlQuery.Count();

            switch (model.sort)
            {
                case "VoucherNo":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            dbRawSqlQuery = dbRawSqlQuery
                                .OrderByDescending(r => r.VoucherRefNo)
                                .Skip(index*pageSize)
                                .Take(pageSize);

                            break;
                        default:
                            dbRawSqlQuery = dbRawSqlQuery
                                .OrderBy(r => r.VoucherRefNo)
                                .Skip(index*pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;
                case "VoucherType":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            dbRawSqlQuery = dbRawSqlQuery
                                .OrderByDescending(r => r.VoucherType)
                                .Skip(index*pageSize)
                                .Take(pageSize);

                            break;
                        default:
                            dbRawSqlQuery = dbRawSqlQuery
                                .OrderBy(r => r.VoucherType)
                                .Skip(index*pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;
                case "Amount":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            dbRawSqlQuery = dbRawSqlQuery
                                .OrderByDescending(r => r.Amount)
                                .Skip(index*pageSize)
                                .Take(pageSize);

                            break;
                        default:
                            dbRawSqlQuery = dbRawSqlQuery
                                .OrderBy(r => r.Amount)
                                .Skip(index*pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;
                default:
                    dbRawSqlQuery = dbRawSqlQuery
                        .OrderByDescending(r => r.Date).ThenByDescending(x => x.VoucherNo)
                        .Skip(index*pageSize)
                        .Take(pageSize);
                    break;
            }

            foreach (var t in dbRawSqlQuery)
            {
                t.Amount = Math.Round(t.Amount/_voucherMasterRepository.GetConversionValueByVoucherId(t.Id), 2);
            }

            return dbRawSqlQuery.ToList();
        }


        public Acc_VoucherMaster GetAllVoucherMaster(long id)
        {
            var voucherMaster = _voucherMasterRepository.FindOne(x => x.IsActive == true && x.Id == id);
            voucherMaster.Acc_VoucherDetail = _voucherDetailRepository.Filter(x => x.RefId == id).Include(x => x.Acc_GLAccounts).ToList();
            return voucherMaster;
        }

        public int EditVoucher(Acc_VoucherMaster voucherMaster)
        {
            var editIndex = 0;
            using (var transaction = new TransactionScope())
            {
                var voucherMasterObject = _voucherMasterRepository.FindOne(x => x.IsActive == true && x.Id == voucherMaster.Id);
                voucherMasterObject.Acc_VoucherDetail = _voucherDetailRepository.Filter(x => x.RefId == voucherMaster.Id).ToList();
                _voucherMasterRepository.Delete(voucherMasterObject);
                editIndex = _voucherMasterRepository.Save(voucherMaster);
                transaction.Complete();
            }
            return editIndex;
        }

        public List<VAccVoucherMaster> GetVoucherSummary(VoucherList model)
        {
            Expression<Func<VAccVoucherMaster, bool>> predicate;
            predicate = x => (
                ((x.VoucherRefNo.Trim().Contains(model.VoucherRefNo) || String.IsNullOrEmpty(model.VoucherRefNo.Trim().ToLower()))
                 || (x.VoucherType.Trim().Contains(model.VoucherRefNo) || String.IsNullOrEmpty(model.VoucherRefNo.Trim().ToLower()))
                 || (SqlFunctions.StringConvert(x.Amount).Contains(model.VoucherRefNo.Trim()) || String.IsNullOrEmpty(model.VoucherRefNo.Trim().ToLower()))
                 || (SqlFunctions.StringConvert((double?) x.VoucherNo).Contains(model.VoucherRefNo.Trim()) || String.IsNullOrEmpty(model.VoucherRefNo.Trim().ToLower())))
                && ((x.Date >= model.FromDate || model.FromDate == null) && (x.Date <= model.ToDate || model.ToDate == null)));
            return _voucherMasterRepository.GetVoucherList(predicate).ToList();
        }

        public bool IsVoucherRefExist(VoucherList model)
        {
            return _voucherMasterRepository.Exists(x => x.VoucherRefNo == model.VoucherRefNo && x.Id != model.Id);
        }

        public int DeleteVoucher(int? id)
        {
            var deleteIndx = 0;
            using (var transaction = new TransactionScope())
            {
                var vm = _voucherMasterRepository.FindOne(x => x.Id == id);
                vm.Acc_VoucherDetail = _voucherDetailRepository.Filter(x => x.RefId == id).ToList();
                deleteIndx = _voucherMasterRepository.DeleteOne(vm);
                transaction.Complete();
            }

            return deleteIndx;
        }

        public int SaveVoucherToCostCentre(Acc_VoucherToCostcentre voucherToCostcentre)
        {
            return _voucherMasterRepository.SaveVoucherToCostCentre(voucherToCostcentre);
        }

        public int DeleteVouchertoCostCentre(Acc_VoucherToCostcentre voucherToCostcentre)
        {
            return _voucherMasterRepository.DeleteVouchertoCostCentre(voucherToCostcentre);
        }

        public IQueryable<Acc_VoucherToCostcentre> GetVoucherToCostCentre(long Id)
        {
            return _voucherMasterRepository.GetVoucherToCostCentre(Id);
        }

        public string GetAccountNameByCode(decimal accountCode)
        {
            return _voucherMasterRepository.GetAccountNameByCode(accountCode);
        }

        public string GetVoucherNoByType(string type, DateTime voucherDate)
        {
            return _voucherMasterRepository.GetVoucherNoByType(type, voucherDate);
        }

        public int ChangeGlHeadGroup(string sectorId, string glId, string glIdNew)
        {
            return _voucherMasterRepository.ChangeGlHeadGroup(sectorId, glId, glIdNew);
        }

        public int ChangeGlHeadByParent(string sectorId, string glHead, string glHeadParent)
        {
            return _voucherMasterRepository.ChangeGlHeadByParent(sectorId, glHead, glHeadParent);            
        }

        public DataTable GetChequeReport(int id, string compId)
        {
           return _voucherMasterRepository.ExecuteQuery(String.Format("exec SpGetBankPaymentCheque '{0}'", id));
        }

        public string GetCurrencyById(int id)
        {
            return _voucherMasterRepository.GetCurrencyById(id);
        }
        public Acc_Currency GetAllCurrency()
        {
            return _voucherMasterRepository.GetCurrency();
        }

        public int? GetCostCentreByEmployeeId(Guid? employeeId)
        {
            return _voucherMasterRepository.GetCostCentreByEmployeeId(employeeId);
        }
     
       public int SaveIniteGrationVoucher(Acc_VoucherMaster voucherMaster)
        {
            var saveIndex = 0;
            using (var trnsaction=new TransactionScope())
            {
                saveIndex = _voucherMasterRepository.Save(voucherMaster);
                if (voucherMaster.IntType== (int)BillTable.PROD_KnittingRollIssue)
                {
                    var knittingRollChallan = _knittinRollissueRepository.FindOne(x => x.IssueRefNo == voucherMaster.IntRefId);
                    knittingRollChallan.VoucherMasterId = voucherMaster.Id;
                    knittingRollChallan.Posted = POSTED.Y.ToString();
                    _knittinRollissueRepository.Edit(knittingRollChallan);
                }
                else if (voucherMaster.IntType == (int)BillTable.Inventory_GreyIssue)
                {
                    var greyIssue = greyIssueRepository.FindOne(x => x.RefId == voucherMaster.IntRefId);
                    greyIssue.VoucherMasterId = voucherMaster.Id;
                    greyIssue.Posted = POSTED.Y.ToString();
                    greyIssueRepository.Edit(greyIssue);
                }
                else if (voucherMaster.IntType == (int)BillTable.Inventory_FinishFabricIssue)
                {
                    var fabIssue = finishFabricIssueRepository.FindOne(x => x.FinishFabIssureRefId == voucherMaster.IntRefId);
                    fabIssue.VoucherMasterId = voucherMaster.Id;
                    fabIssue.Posted = POSTED.Y.ToString();
                    finishFabricIssueRepository.Edit(fabIssue);
                }
                else if (voucherMaster.IntType == (int)BillTable.PROD_ProcessReceive)
                {
                    var printEmb = processReceiveRepository.FindOne(x => x.RefNo == voucherMaster.IntRefId);
                    printEmb.VoucherMasterId = voucherMaster.Id;
                    printEmb.Posted = POSTED.Y.ToString();
                    processReceiveRepository.Edit(printEmb);
                }
                trnsaction.Complete();
            }

            return saveIndex;
        }
    }
}
