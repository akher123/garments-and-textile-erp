using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Remoting.Contexts;
using System.Transactions;
using SCERP.BLL.IManager.IInventoryManager;
using SCERP.Common;
using SCERP.DAL;
using SCERP.DAL.IRepository.IInventoryRepository;
using SCERP.DAL.Repository.InventoryRepository;
using SCERP.Model;
using SCERP.Model.Custom;

namespace SCERP.BLL.Manager.InventoryManager
{
    public class ItemStoreManager : IItemStoreManager
    {
        private readonly IItemStoreRepository _itemStoreRepository = null;
        private readonly IItemStoreDetailRepository _temItemStoreDetailRepository = null;
        private readonly IStorePurchaseRequisitionDetailRepository _storePurchaseRequisitionDetail;

        public ItemStoreManager(SCERPDBContext context)
        {
            _temItemStoreDetailRepository = new ItemStoreDetailRepository(context);
            _itemStoreRepository = new ItemStoreRepository(context);
            _storePurchaseRequisitionDetail = new StorePurchaseRequisitionDetailRepository(context);
        }



        public List<VInventoryItemStore> GetInventoryItemStoreBypaging(out int totalRecords, Inventory_ItemStore model)
        {

            var index = model.PageIndex;
            var pageSize = AppConfig.PageSize;
            Expression<Func<VInventoryItemStore, bool>> predicate =
                x => (x.QCStatus == model.QCStatus || model.QCStatus == 2) && (x.ReceiveType == model.ReceiveType || model.ReceiveType == 0)
                     &&
                     ((x.ReceivedRegisterNo.Trim().Contains(model.SearchString) || String.IsNullOrEmpty(model.SearchString.Trim().ToLower()))
                     || (x.RequisitionNo.Trim().Contains(model.SearchString) || String.IsNullOrEmpty(model.SearchString.Trim().ToLower()))
                     || (x.InvoiceNo.Trim().Contains(model.SearchString) || String.IsNullOrEmpty(model.SearchString.Trim().ToLower()))
                     || (x.SupplierCompanyName.Trim().Contains(model.SearchString) || String.IsNullOrEmpty(model.SearchString.Trim().ToLower())))
                      && ((x.ReceivedDate >= model.FromDate || model.FromDate == null) && (x.ReceivedDate <= model.ToDate || model.ToDate == null));
            IQueryable<VInventoryItemStore> vInventoryItemStores = _itemStoreRepository.GetInventoryItemStore(predicate);
            totalRecords = vInventoryItemStores.Count();
            switch (model.sort)
            {

                case "PurchaseReferencePersonName":

                    switch (model.sortdir)
                    {
                        case "DESC":
                            vInventoryItemStores = vInventoryItemStores
                              .OrderByDescending(r => r.PurchaseReferencePersonName).ThenByDescending(x => x.ReceivedDate)
                              .Skip(index * pageSize)
                              .Take(pageSize);

                            break;
                        default:
                            vInventoryItemStores = vInventoryItemStores
                            .OrderBy(r => r.PurchaseReferencePersonName).ThenByDescending(x => x.ReceivedDate)
                            .Skip(index * pageSize)
                            .Take(pageSize);


                            break;
                    }

                    break;

                case "SupplierCompanyName":

                    switch (model.sortdir)
                    {
                        case "DESC":
                            vInventoryItemStores = vInventoryItemStores
                             .OrderByDescending(r => r.SupplierCompanyName)
                             .Skip(index * pageSize)
                             .Take(pageSize);

                            break;
                        default:
                            vInventoryItemStores = vInventoryItemStores
                            .OrderBy(r => r.SupplierCompanyName).ThenByDescending(x => x.ReceivedDate)
                            .Skip(index * pageSize)
                            .Take(pageSize);
                            break;
                    }
                    break;
                case "RequisitionNo":

                    switch (model.sortdir)
                    {
                        case "DESC":
                            vInventoryItemStores = vInventoryItemStores
                                .OrderByDescending(r => r.RequisitionNo).ThenByDescending(x => x.ReceivedDate)

                                .Skip(index * pageSize)
                                .Take(pageSize);
                            break;
                        default:
                            vInventoryItemStores = vInventoryItemStores
                                 .OrderBy(r => r.RequisitionNo).ThenByDescending(x => x.ReceivedDate)
                                .Skip(index * pageSize)
                                .Take(pageSize);

                            break;
                    }

                    break;
                case "ReceivedRegisterNo":

                    switch (model.sortdir)
                    {
                        case "DESC":
                            vInventoryItemStores = vInventoryItemStores
                                .OrderByDescending(r => r.ReceivedRegisterNo).ThenByDescending(x => x.ReceivedDate)

                                .Skip(index * pageSize)
                                .Take(pageSize);

                            break;
                        default:
                            vInventoryItemStores = vInventoryItemStores
                                 .OrderByDescending(r => r.ReceivedRegisterNo).ThenByDescending(x => x.ReceivedDate)
                                .Skip(index * pageSize)
                                .Take(pageSize);

                            break;
                    }

                    break;
                case "LCNo":

                    switch (model.sortdir)
                    {
                        case "DESC":
                            vInventoryItemStores = vInventoryItemStores
                                .OrderByDescending(r => r.LCNo).ThenByDescending(x => x.ReceivedDate)

                                .Skip(index * pageSize)
                                .Take(pageSize);

                            break;
                        default:
                            vInventoryItemStores = vInventoryItemStores
                                 .OrderByDescending(r => r.LCNo).ThenByDescending(x => x.ReceivedDate)
                                .Skip(index * pageSize)
                                .Take(pageSize);

                            break;
                    }

                    break;
                case "InvoiceNo":

                    switch (model.sortdir)
                    {
                        case "DESC":
                            vInventoryItemStores = vInventoryItemStores
                                .OrderByDescending(r => r.InvoiceNo).ThenByDescending(x => x.ReceivedDate)

                                .Skip(index * pageSize)
                                .Take(pageSize);

                            break;
                        default:
                            vInventoryItemStores = vInventoryItemStores
                                 .OrderByDescending(x => x.ReceivedDate)
                                .Skip(index * pageSize)
                                .Take(pageSize);

                            break;
                    }

                    break;
                default:
                    vInventoryItemStores = vInventoryItemStores
                                   .OrderByDescending(x => x.ReceivedDate)
                                   .Skip(index * pageSize)
                                   .Take(pageSize);
                    break;

            }
            return vInventoryItemStores.ToList();
        }

        public int SaveItemStor(Inventory_ItemStore model, List<int> inventoryItemStoreDetailIds)
        {

            var effectedRows = 0;

            using (var transaction = new TransactionScope())
            {
                if (inventoryItemStoreDetailIds.Any())
                {

                    foreach (var sprd in inventoryItemStoreDetailIds.Select(sprdId => _storePurchaseRequisitionDetail.FindOne(
                        x => x.StorePurchaseRequisitionDetailId == sprdId)))
                    {
                        sprd.IsReceived = true;

                        effectedRows += _storePurchaseRequisitionDetail.Edit(sprd);
                    }



                }
                model.CreatedDate = DateTime.Now;
                model.CreatedBy = PortalContext.CurrentUser.UserId;
                model.IsActive = true;
                effectedRows = _itemStoreRepository.Save(model);
                transaction.Complete();
            }

            return effectedRows;
        }

        public Inventory_ItemStore GetInventoryItemStoreByItemStoreId(long itemStoreId)
        {

            var InventoryItemStore = _itemStoreRepository.GetInventoryItemStoreByItemStoreId(itemStoreId);
            bool isExist =
                _itemStoreRepository.Exists(
                    x => x.IsProcessRequsition && x.RequisitionNo == InventoryItemStore.RequisitionNo);
            if (!isExist)
            {
                InventoryItemStore.IsProcessRequsition = false;
            }
            return InventoryItemStore;
        }

        public int EditItemStor(Inventory_ItemStore model)
        {
            var edit = 0;
            using (var transaction = new TransactionScope())
            {
                var itemStore = _itemStoreRepository.FindOne(x => x.IsActive && x.ItemStoreId == model.ItemStoreId);
                itemStore.ReceivedDate = model.ReceivedDate;
                itemStore.ReceivedRegisterNo = model.ReceivedRegisterNo;
                itemStore.RequisitionNo = model.RequisitionNo;
                itemStore.PurchaseReferencePerson = model.PurchaseReferencePerson;
                itemStore.LCNo = model.LCNo;
                itemStore.Requisitor = model.Requisitor;
                itemStore.SupplierId = model.SupplierId;
                itemStore.InvoiceNo = model.InvoiceNo;
                itemStore.InvoiceDate = model.InvoiceDate;
                itemStore.ReceivedDate = model.ReceivedDate;
                itemStore.GateEntry = model.GateEntry;
                itemStore.GateEntryDate = model.GateEntryDate;
                itemStore.IsProcessRequsition = model.IsProcessRequsition;
                itemStore.EditedDate = DateTime.Now;
                itemStore.EditedBy = PortalContext.CurrentUser.UserId;
                itemStore.ReceiveType = model.ReceiveType;
                itemStore.IsActive = true;
                itemStore.SuppliedStatus = model.SuppliedStatus;
                edit = _itemStoreRepository.Edit(itemStore);
                _temItemStoreDetailRepository.Delete(x => x.ItemStoreId == model.ItemStoreId);
                foreach (var itemdetailNew in model.Inventory_ItemStoreDetail)
                {
                    itemdetailNew.EditedDate = DateTime.Now;
                    itemdetailNew.EditedBy = PortalContext.CurrentUser.UserId;
                    itemdetailNew.IsActive = true;
                    itemdetailNew.ItemStoreId = itemStore.ItemStoreId;
                    edit += _temItemStoreDetailRepository.Save(itemdetailNew);
                }
                transaction.Complete();
            }
            return edit;
        }

        public int DeleteItemStore(long itemStoreId)
        {

            var itemStore = _itemStoreRepository.FindOne(x => x.IsActive && x.ItemStoreId == itemStoreId);
            itemStore.EditedDate = DateTime.Now;
            itemStore.EditedBy = PortalContext.CurrentUser.UserId;
            itemStore.IsActive = false;
            return _itemStoreRepository.Edit(itemStore);
        }
        public bool CheckQcPassed(long itemStoreId)
        {
            byte qcStatus = Convert.ToByte(QCPassStatus.Passed);
            return _itemStoreRepository.Exists(x => x.QCStatus == qcStatus && x.ItemStoreId == itemStoreId);
        }
        public List<VItemReceiveDetail> GetVItemReciveDetails(long itemStoreId)
        {
            return _temItemStoreDetailRepository.GetVItemReciveDetails(itemStoreId);
        }

        public object AutocompliteReceiveLoanInfo(string searchString)
        {
            return
                _itemStoreRepository.GetInventoryItemStore(
                    x => x.ReceiveType == 3
                        && ((x.RequisitionNo.Trim().ToLower().Contains(searchString.Trim().ToLower()) || String.IsNullOrEmpty(searchString.Trim().ToLower()))
                          || ((x.InvoiceNo.Trim().ToLower().Contains(searchString.Trim().ToLower()) || String.IsNullOrEmpty(searchString.Trim().ToLower()))
                                                  || ((x.GateEntry.Trim().ToLower().Contains(searchString.Trim().ToLower()) || String.IsNullOrEmpty(searchString.Trim().ToLower()))
                        ))));
        }
    }
}
