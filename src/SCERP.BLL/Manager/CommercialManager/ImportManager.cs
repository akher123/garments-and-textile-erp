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
    public class ImportManager : IImportManager
    {
        private readonly IImportRepository _importRepository;
        private readonly string _compId;
        public ImportManager(IImportRepository importRepository)
        {
            _importRepository = importRepository;
            _compId = PortalContext.CurrentUser.CompId;
        }

        public bool CheckExistingImport(CommImport model)
        {
            return _importRepository.Exists(x => x.ImportId == model.ImportId
            
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
            && x.DisplayMember == model.DisplayMember
            && x.CreatedBy == model.CreatedBy
            && x.CreatedDate == model.CreatedDate
            && x.LCDate == model.LCDate
            && x.InvoiceDate == model.InvoiceDate
            && x.Remarks == model.Remarks
            );
        }

        public int DeleteImport(CommImport model)
        {
            return _importRepository.Delete(x => x.ImportId == model.ImportId);
        }

        public int EditImport(CommImport model)
        {
            CommImport commImport = _importRepository.FindOne(x => x.ImportId == model.ImportId);
            commImport.IncomingPort = model.IncomingPort;
            commImport.InvoiceDate = model.InvoiceDate;
            commImport.InvoiceNo = model.InvoiceNo;
            commImport.Item = model.Item;
            commImport.LCDate = model.LCDate;
            commImport.LCQty = model.LCQty;
            commImport.LcValue = model.LcValue;
            commImport.SupplierCountry = model.SupplierCountry;
            commImport.SupplierName = model.SupplierName;
            commImport.GoodsInHouseDate = model.GoodsInHouseDate;
            commImport.DocsValue = model.DocsValue;
            commImport.DocsReceiverPerson = model.DocsReceiverPerson;
            commImport.DocsReceiveDate = model.DocsReceiveDate;
            commImport.DocsReceiverAgent = model.DocsReceiverAgent;
            commImport.DocsQty = model.DocsQty;
            commImport.LCNo = model.LCNo;
            commImport.IncomingPort = model.IncomingPort;
            commImport.CompId = _compId;
            commImport.EditedDate = model.EditedDate;
            commImport.EditedBy = model.EditedBy;
            commImport.Remarks = model.Remarks;
            return _importRepository.Edit(commImport);


        }

        public List<CommImport> GetAllImports()
        {
            return _importRepository.All().ToList();
        }

        public List<CommImport> GetAllImportsByPaging(out int totalRecords, CommImport model)
        {
            var index = model.PageIndex;
            var pageSize = AppConfig.PageSize;
            var aitList = _importRepository.Filter(x => ((x.LCNo.Trim().Contains(model.SearchString.Trim()) || String.IsNullOrEmpty(model.SearchString)))
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

        public CommImport GetExportByLcNo(string lcNo)
        {
            throw new NotImplementedException();
        }

        public CommImport GetImportById(int? id)
        {
            var itemList = _importRepository.Filter(x => x.ImportId == id).FirstOrDefault(x => x.ImportId == id);
            return itemList;
        }

        public List<CommImport> GetImportByLcId(int? id)
        {
            throw new NotImplementedException();
        }

        public int SaveImport(CommImport model)
        {
            return _importRepository.Save(model);
        }
    }
}
