using SCERP.BLL.IManager.ICommercialManager;
using SCERP.Common;
using SCERP.DAL.IRepository.ICommercialRepository;
using SCERP.Model.CommercialModel;
using SCERP.Model.Production;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.BLL.Manager.CommercialManager
{
    public class CashLCDyesChemicalManager : ICashLCDyesChemicalManager
    {
        private readonly ICashLCDyesChemicalRepository _cashLCDyesChemicalRepository;
        private readonly string _compId;
        public CashLCDyesChemicalManager(ICashLCDyesChemicalRepository cashLCDyesChemicalRepository)
        {
            _cashLCDyesChemicalRepository = cashLCDyesChemicalRepository;
            _compId = PortalContext.CurrentUser.CompId;
        }

        public int DeleteCashLc(int cashLcId)
        {
            return _cashLCDyesChemicalRepository.Delete(x => x.CashLcId == cashLcId);
        }

        public int EditCashLc(CommCashLCDyesChemical model)
        {
            CommCashLCDyesChemical commCashLc = _cashLCDyesChemicalRepository.FindOne(x => x.CashLcId == model.CashLcId);

            commCashLc.CashLcId = model.CashLcId;
            commCashLc.CashLcNo = model.CashLcNo;
            commCashLc.LcDate = model.LcDate;
            commCashLc.Item = model.Item;
            commCashLc.Quantity = model.Quantity;
            commCashLc.BillOfImportCode = model.BillOfImportCode;
            commCashLc.ShipmentMode = model.ShipmentMode;
            commCashLc.BillOfEntry = model.BillOfEntry;
            commCashLc.SupplierName = model.SupplierName;
            commCashLc.PortOfDelivery = model.PortOfDelivery;
            commCashLc.DateOfBL = model.DateOfBL;
            commCashLc.CountryOfOrigin = model.CountryOfOrigin;
            commCashLc.PaymentTerms = model.PaymentTerms;
            commCashLc.BillOfImport = model.BillOfImport;
            commCashLc.DateOfBill = model.DateOfBill;
            commCashLc.LcValue = model.LcValue;
            commCashLc.BankRef = model.BankRef;
            commCashLc.PaymentDate = model.PaymentDate;
            commCashLc.EditedDate = model.EditedDate;
            commCashLc.EditedBy = model.EditedBy;
            commCashLc.Remarks = model.Remarks;
            return _cashLCDyesChemicalRepository.Edit(commCashLc);
        }

        public List<CommCashLCDyesChemical> GetAllCashLcs()
        {
            var commCashLcList = _cashLCDyesChemicalRepository.All();
            return commCashLcList.ToList();
        }

        public List<CommCashLCDyesChemical> GetAllCashLcsByPaging(CommCashLCDyesChemical model, out int totalRecords, string searchString)
        {
            var index = model.PageIndex;
            string compId = PortalContext.CurrentUser.CompId;
            var pageSize = AppConfig.PageSize;
            
            var aitList = _cashLCDyesChemicalRepository.Filter(x => ((x.CashLcNo.Trim().Contains(searchString.Trim()) || String.IsNullOrEmpty(searchString)))
                           || ((x.SupplierName.Trim().Contains(searchString.Trim()) || String.IsNullOrEmpty(searchString)))
                           || ((x.PaymentTerms.Trim().Contains(searchString.Trim()) || String.IsNullOrEmpty(searchString)))
                            || ((x.Item.Trim().Contains(searchString.Trim()) || String.IsNullOrEmpty(searchString))));
                          // && ((x.LcDate >= model.FromDate || model.FromDate == null) && (x.LcDate <= model.ToDate || model.ToDate == null)));
            totalRecords = aitList.Count();
            switch (model.sort)
            {
                case "CashLcId":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            aitList = aitList
                                 .OrderByDescending(r => r.CashLcId)
                                .Skip(index * pageSize)
                                .Take(pageSize);

                            break;
                        default:
                            aitList = aitList
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
                            aitList = aitList
                                 .OrderByDescending(r => r.CashLcNo)
                                .Skip(index * pageSize)
                                .Take(pageSize);

                            break;
                        default:
                            aitList = aitList
                                 .OrderBy(r => r.CashLcNo)
                                .Skip(index * pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;




                default:
                    aitList = aitList
                        .OrderByDescending(r => r.CashLcId)
                        .Skip(index * pageSize)
                        .Take(pageSize);
                    break;
            }

            return aitList.ToList();
        }

        public CommCashLCDyesChemical GetCashLcById(int cashLcId)
        {
            var itemList = _cashLCDyesChemicalRepository.Filter(x => x.CashLcId == cashLcId).FirstOrDefault(x => x.CashLcId == cashLcId);
            return itemList;
        }

        public string GetNewCashLcRefId(string prifix)
        {
            throw new NotImplementedException();
        }

        public bool IsCashLcExist(CommCashLCDyesChemical model)
        {
            return _cashLCDyesChemicalRepository.Exists(x => x.CashLcId == model.CashLcId && x.CashLcNo == model.CashLcNo && x.LcDate == model.LcDate && x.Item == model.Item && x.Quantity == model.Quantity && x.SupplierName == model.SupplierName && x.PortOfDelivery == model.PortOfDelivery && x.DateOfBL == model.DateOfBL && x.CountryOfOrigin == model.CountryOfOrigin && x.PaymentTerms == model.PaymentTerms && x.BillOfImport == model.BillOfImport && x.DateOfBill == model.DateOfBill && x.LcValue == model.LcValue && x.BankRef == model.BankRef && x.PaymentDate == model.PaymentDate && x.Remarks == model.Remarks);
        }

        public int SaveCashLc(CommCashLCDyesChemical model)
        {
            return _cashLCDyesChemicalRepository.Save(model);
        }
    }
}
