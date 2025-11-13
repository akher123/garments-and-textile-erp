using SCERP.BLL.IManager.ICommercialManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.CommercialModel;
using SCERP.DAL.IRepository.ICommercialRepository;
using SCERP.Common;

namespace SCERP.BLL.Manager.CommercialManager
{
    public class CashLcManager : ICashLcManager
    {
        private readonly ICashLcRepository _cashLcRepository;
        private readonly string _compId;
        public CashLcManager(ICashLcRepository cashLcRepository)
        {
            _cashLcRepository = cashLcRepository;
            _compId = PortalContext.CurrentUser.CompId;
        }

        public int DeleteCashLc(int cashLcId)
        {
            return _cashLcRepository.Delete(x => x.CashLcId == cashLcId);
        }

        public int EditCashLc(CommCashLc model)
        {
            CommCashLc commCashLc = _cashLcRepository.FindOne(x => x.CashLcId == model.CashLcId);

            commCashLc.CashLcId = model.CashLcId;
            commCashLc.CashLcNo = model.CashLcNo;
            commCashLc.LcDate = model.LcDate;
            commCashLc.Item = model.Item;
            commCashLc.BillOfEntry = model.BillOfEntry;
            commCashLc.WayOfEntry = model.WayOfEntry;
            commCashLc.QuantitySetMultiple = model.QuantitySetMultiple;
            commCashLc.SupplierName = model.SupplierName;
            commCashLc.PortOfDelivery = model.PortOfDelivery;
            commCashLc.DateOfBL = model.DateOfBL;
            commCashLc.CountryOfOrigin = model.CountryOfOrigin;
            commCashLc.PaymentTerms = model.PaymentTerms;
            commCashLc.BillOfImportCode = model.BillOfImportCode;
            commCashLc.DateOfBLMultiple = model.DateOfBLMultiple;
            commCashLc.DateOfBill = commCashLc.DateOfBill;
            commCashLc.LcValue = model.LcValue;
            commCashLc.BankRef = model.BankRef;
            commCashLc.PaymentDate = model.PaymentDate;
            commCashLc.EditedDate = model.EditedDate;
            commCashLc.EditedBy = model.EditedBy;
            commCashLc.Remarks = model.Remarks;
            return _cashLcRepository.Edit(commCashLc);
        }

        public List<CommCashLc> GetAllCashLcs()
        {
            var commCashLcList = _cashLcRepository.All();
            return commCashLcList.ToList();
        }

        public List<CommCashLc> GetAllCashLcsByPaging(CommCashLc model, out int totalRecords, string searchString)
        {
            var index = model.PageIndex;
            var pageSize = AppConfig.PageSize;

            var cashLc = _cashLcRepository.Filter(p => (p.SupplierName.Contains(model.SupplierName) || string.IsNullOrEmpty(model.SupplierName))

                                                          && (p.CashLcNo.Contains(model.CashLcNo) || string.IsNullOrEmpty(model.CashLcNo))
                                                          && (p.LcDate <= model.FromDate || model.FromDate == null)
                                                          && (p.LcDate >= model.ToDate || model.ToDate == null)
                                                          && (p.WayOfEntry.Contains(model.WayOfEntry) || string.IsNullOrEmpty(model.WayOfEntry))
                                                          && (p.Item.Contains(model.Item) || string.IsNullOrEmpty(model.Item))
                                                          && (p.PortOfDelivery.Contains(model.PortOfDelivery) || string.IsNullOrEmpty(model.PortOfDelivery))
                                                          );

            totalRecords = cashLc.Count();

            switch (model.sort)
            {
                case "CashLcId":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            cashLc = cashLc
                                 .OrderByDescending(r => r.CashLcId)
                                .Skip(index * pageSize)
                                .Take(pageSize);

                            break;
                        default:
                            cashLc = cashLc
                                 .OrderBy(r => r.CashLcId)
                                .Skip(index * pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;

                case "CashLcNo":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            cashLc = cashLc
                                 .OrderByDescending(r => r.CashLcNo)
                                .Skip(index * pageSize)
                                .Take(pageSize);

                            break;
                        default:
                            cashLc = cashLc
                                 .OrderBy(r => r.CashLcNo)
                                .Skip(index * pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;




                default:
                    cashLc = cashLc
                        .OrderByDescending(r => r.CashLcId)
                        .Skip(index * pageSize)
                        .Take(pageSize);
                    break;
            }

            return cashLc.ToList();
        }

        public CommCashLc GetCashLcById(int cashLcId)
        {
            var itemList = _cashLcRepository.Filter(x => x.CashLcId == cashLcId).FirstOrDefault(x => x.CashLcId == cashLcId);
            return itemList;
        }

        public string GetNewCashLcRefId(string prifix)
        {
            throw new NotImplementedException();
        }

        public bool IsCashLcExist(CommCashLc model)
        {
            return _cashLcRepository.Exists(x => x.CashLcId == model.CashLcId && x.CashLcNo == model.CashLcNo && x.LcDate == model.LcDate && x.Item == model.Item && x.Quantity == model.Quantity && x.SupplierName == model.SupplierName && x.PortOfDelivery == model.PortOfDelivery && x.DateOfBL == model.DateOfBL && x.CountryOfOrigin == model.CountryOfOrigin && x.PaymentTerms == model.PaymentTerms && x.BillOfImport == model.BillOfImport && x.DateOfBill == model.DateOfBill && x.LcValue == model.LcValue && x.BankRef == model.BankRef && x.PaymentDate == model.PaymentDate && x.Remarks == model.Remarks);
        }

        public int SaveCashLc(CommCashLc model)
        {
            return _cashLcRepository.Save(model);
        }
    }
}
