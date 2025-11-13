using SCERP.BLL.IManager.ICommercialManager;
using SCERP.Common;
using SCERP.DAL.IRepository.ICommercialRepository;
using SCERP.Model.CommercialModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.BLL.Manager.CommercialManager
{
    public class ImportDetailsManager : IImportDetailsManager
    {
        private readonly IImportDetailsRepository _importDetailsRepository;
        private readonly string _compId;
        public ImportDetailsManager(IImportDetailsRepository importDetailsRepository)
        {
            _importDetailsRepository = importDetailsRepository;
            _compId = PortalContext.CurrentUser.CompId;
        }

        public bool CheckExistingImportDetails(CommImportDetails model)
        {
            return _importDetailsRepository.Exists(x => x.ImportId == model.ImportId
            
            && x.DocsQty == model.DocsQty
            && x.Item == model.Item
            && x.InvoiceNo == model.InvoiceNo
            && x.SupplierName == model.SupplierName
            && x.IncomingPort == model.IncomingPort
            && x.LCQty == model.LCQty
            && x.LcValue == model.LcValue
            && x.SupplierCountry == model.SupplierCountry
            && x.GoodsInHouseDate == model.GoodsInHouseDate
            && x.SupplierName == model.SupplierName
            //&& x.DisplayMember == model.DisplayMember
            && x.CreatedBy == model.CreatedBy
            && x.CreatedDate == model.CreatedDate
            && x.LCDate == model.LCDate
            && x.InvoiceDate == model.InvoiceDate
            && x.Remarks == model.Remarks
            );
        }

        public int DeleteImportDetails(CommImportDetails model)
        {
            return _importDetailsRepository.Delete(x => x.ImportDetailId == model.ImportDetailId);
        }

        public int EditImportDetails(CommImportDetails model)
        {
            CommImportDetails commImportDetail = _importDetailsRepository.FindOne(x => x.ImportDetailId == model.ImportDetailId);
            commImportDetail.IncomingPort = model.IncomingPort;
            commImportDetail.ImportId = model.ImportId;
            commImportDetail.InvoiceDate = model.InvoiceDate;
            commImportDetail.InvoiceNo = model.InvoiceNo;
            commImportDetail.Item = model.Item;
            commImportDetail.LCDate = model.LCDate;
            commImportDetail.LCQty = model.LCQty;
            commImportDetail.LcValue = model.LcValue;
            commImportDetail.SupplierCountry = model.SupplierCountry;
            commImportDetail.SupplierName = model.SupplierName;
            commImportDetail.GoodsInHouseDate = model.GoodsInHouseDate;
            commImportDetail.DocsValue = model.DocsValue;
            commImportDetail.DocsReceiverPerson = model.DocsReceiverPerson;
            commImportDetail.DocsReceiveDate = model.DocsReceiveDate;
            commImportDetail.DocsReceiverAgent = model.DocsReceiverAgent;
            commImportDetail.DocsQty = model.DocsQty;
            commImportDetail.LCNo = model.LCNo;
            commImportDetail.IncomingPort = model.IncomingPort;
            commImportDetail.CompId = _compId;
            commImportDetail.EditedDate = model.EditedDate;
            commImportDetail.EditedBy = model.EditedBy;
            commImportDetail.Remarks = model.Remarks;
            return _importDetailsRepository.Edit(commImportDetail);


        }

        public List<CommImportDetails> GetAllImportDetails()
        {
            return _importDetailsRepository.All().ToList();
        }

        public List<CommImportDetails> GetAllImportsDetailsByPaging(out int totalRecords, CommImportDetails model)
        {
            var index = model.PageIndex;
            var pageSize = AppConfig.PageSize;
            var aitList = _importDetailsRepository.Filter(x => ((x.LCNo.Trim().Contains(model.SearchString.Trim()) || String.IsNullOrEmpty(model.SearchString)))
                           || ((x.SupplierName.Trim().Contains(model.SearchString.Trim()) || String.IsNullOrEmpty(model.SearchString)))
                           || ((x.InvoiceNo.Trim().Contains(model.SearchString.Trim()) || String.IsNullOrEmpty(model.SearchString)))
                            || ((x.Item.Trim().Contains(model.SearchString.Trim()) || String.IsNullOrEmpty(model.SearchString))));
           
            totalRecords = aitList.Count();
            switch (model.sort)
            {
                case "CashLcId":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            aitList = aitList
                                 .OrderByDescending(r => r.ImportId)
                                .Skip(index * pageSize)
                                .Take(pageSize);

                            break;
                        default:
                            aitList = aitList
                                 .OrderByDescending(r => r.ImportId)
                                .Skip(index * pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;

                case "BBLCNo":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            aitList = aitList
                                 .OrderByDescending(r => r.LCNo)
                                .Skip(index * pageSize)
                                .Take(pageSize);

                            break;
                        default:
                            aitList = aitList
                                 .OrderByDescending(r => r.ImportId)
                                .Skip(index * pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;




                default:
                    aitList = aitList
                        .OrderByDescending(r => r.ImportId)
                        .Skip(index * pageSize)
                        .Take(pageSize);
                    break;
            }

            return aitList.ToList();
        }

        

        public CommImportDetails GetImportDetailsById(int? id)
        {
            var itemList = _importDetailsRepository.Filter(x => x.ImportDetailId == id).FirstOrDefault(x => x.ImportDetailId == id);
            return itemList;
        }

        public List<CommImportDetails> GetImportDetailsByLcId(int? id)
        {
            return _importDetailsRepository.Filter(x=>x.ImportId==id).ToList();
        }

        public int SaveImportDetails(CommImportDetails model)
        {
            return _importDetailsRepository.Save(model);
        }
    }
}
