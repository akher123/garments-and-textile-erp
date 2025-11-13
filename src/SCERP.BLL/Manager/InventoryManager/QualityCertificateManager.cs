using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using SCERP.BLL.IManager.IInventoryManager;
using SCERP.Common;
using SCERP.DAL;
using SCERP.DAL.IRepository.IInventoryRepository;
using SCERP.DAL.Repository.InventoryRepository;
using SCERP.Model;


namespace SCERP.BLL.Manager.InventoryManager
{
    public class QualityCertificateManager : IQualityCertificateManager
    {
        private readonly IQualityCertificateRepository _qualityCertificateRepository = null;
        private readonly IItemStoreRepository _itemStoreRepository;
        private readonly ItemStoreDetailRepository _itemStoreDetailRepository;
        private readonly IQualityCertificateDetailRepository _qualityCertificateDetailRepository;
        public QualityCertificateManager(SCERPDBContext context)
        {
            _itemStoreRepository = new ItemStoreRepository(context);
            _qualityCertificateRepository = new QualityCertificateRepository(context);
            _itemStoreDetailRepository = new ItemStoreDetailRepository(context);
            _qualityCertificateDetailRepository=new QualityCertificateDetailRepository(context);
        }

        public string GetNewQCReferenceNo()
        {
            return _qualityCertificateRepository.GetNewQCReferenceNo();
        }

        public int SaveQualityCertificate(Inventory_QualityCertificate qcCertificate)
        {
            int saveIndex = 0;
            using (var transaction = new TransactionScope())
            {
                Inventory_ItemStore inventoryItemStore = _itemStoreRepository.FindOne(x => x.ItemStoreId == qcCertificate.ItemStoreId);
                inventoryItemStore.QCStatus = Convert.ToByte(QCPassStatus.Passed);
                saveIndex += _itemStoreRepository.Edit(inventoryItemStore);
                IQueryable<Inventory_ItemStoreDetail> inventoryItemStoreDetails = _itemStoreDetailRepository.Filter(x => x.ItemStoreId == qcCertificate.ItemStoreId && x.IsActive);
                foreach (Inventory_ItemStoreDetail itemStoreDetail in inventoryItemStoreDetails)
                {
                    qcCertificate.Inventory_QualityCertificateDetail.Add(new Inventory_QualityCertificateDetail() 
                    { QualityCertificateId = qcCertificate.QualityCertificateId, 
                        ItemId = itemStoreDetail.ItemId,
                      CreatedBy = PortalContext.CurrentUser.UserId,
                      CreatedDate = DateTime.Now,
                      IsActive = true
                    });
                }
                saveIndex += _qualityCertificateRepository.Save(qcCertificate);
                transaction.Complete();
            }
            return saveIndex;
        }

        public List<VQualityCertificate> GetQualityCertificateByPaging(out int totalRecords, VQualityCertificate model)
        {
            return _qualityCertificateRepository.GetQualityCertificateByPaging(out totalRecords, model);
        }

        public List<VQualityCertificateDetail> GetQualityCertificateDetailIds(long itemStoreId, int qualityCertificateId)
        {
            var qcList=new List<VQualityCertificateDetail>();
            var qalityCertificates= _qualityCertificateRepository.GetQualityCertificateDetailIds(itemStoreId, qualityCertificateId);

            foreach (var qc in qalityCertificates)
            {
                if (qc.RejectedQuantity==0.0m)
                {
                    qc.CorrectQuantity = qc.ReceivedQuantity;
                }
                qcList.Add(qc);
            }
            return qcList;
        }

        public int EditQualityCertificate(Inventory_QualityCertificate qualityCertificate)
        {
            var effectedRows = 0;
            using (var transaction = new TransactionScope())
            {
                var qualityCertificateObj =
                    _qualityCertificateRepository.FindOne(
                        x => x.QualityCertificateId == qualityCertificate.QualityCertificateId);
                qualityCertificateObj.EditedBy = PortalContext.CurrentUser.UserId;
                qualityCertificateObj.EditedDate = DateTime.Now;
                qualityCertificateObj.SendingDate = qualityCertificate.SendingDate;
                qualityCertificateObj.QCReferenceNo = qualityCertificate.QCReferenceNo;
                effectedRows = _qualityCertificateRepository.Edit(qualityCertificateObj);
                foreach (var  qualityCertificatedetail in qualityCertificate.Inventory_QualityCertificateDetail)
                {
                    var qualityCertificateDetail = _qualityCertificateDetailRepository.FindOne( x => x.QualityCertificateDetailId == qualityCertificatedetail.QualityCertificateDetailId && x.QualityCertificateId == qualityCertificatedetail.QualityCertificateId);
                    qualityCertificateDetail.CreatedBy = PortalContext.CurrentUser.UserId;
                    qualityCertificateDetail.CreatedDate = DateTime.Now;
                    qualityCertificateDetail.RejectedQuantity = qualityCertificatedetail.RejectedQuantity;
                    qualityCertificateDetail.CorrectQuantity = qualityCertificatedetail.CorrectQuantity;
                    qualityCertificateDetail.Remarks = qualityCertificatedetail.Remarks;
                    effectedRows = _qualityCertificateDetailRepository.Edit(qualityCertificateDetail);
                }
                transaction.Complete();
            }
            return effectedRows;
        }

        public Inventory_QualityCertificate GetQualityCertificateById(int qualityCertificateId)
        {
            return _qualityCertificateRepository.FindOne(x => x.QualityCertificateId == qualityCertificateId);
        }

        public int DeleteQualityCertificate(int? qualityCertificateId)
        {
            var qualityCertificate =
                _qualityCertificateRepository.FindOne(x => x.QualityCertificateId == qualityCertificateId);
            qualityCertificate.IsActive = false;
            return _qualityCertificateRepository.Edit(qualityCertificate);
        }
    }
}
