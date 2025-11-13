using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.Reporting.WebForms;
using SCERP.Common;
using SCERP.Model;
using SCERP.Web.Areas.Inventory.Models.ViewModels;
using SCERP.Web.Helpers;

namespace SCERP.Web.Areas.Inventory.Controllers
{
    public class ItemStoreController : BaseInventoryController
    {
        [AjaxAuthorize(Roles = "materialreceive-1,materialreceive-2,materialreceive-3")]
        public ActionResult Index(ItemStoreViewModel model)
        {
            ModelState.Clear();
            try
            {
                ModelState.Clear();
                var totalRecords = 0;
                model.Currencies = CurrencyManagerCommon.GetAllCourrency();
                var authorizedPersons = InventoryAuthorizedPersonManager.GetAuthorizedPersonsByProcessTypeId((int)InventoryProcessType.StorePurchaseRequisition, (int)StorePurchaseRequisition.Purchases);
                var supplierCompanies = SupplierCompanyManager.GetAllSupplierCompany();
                model.AuthorizedPersons = authorizedPersons;
                model.Suppliers = supplierCompanies;
                model.InventoryItemStores = ItemStoreManager.GetInventoryItemStoreBypaging(out totalRecords, model);
                model.TotalRecords = totalRecords;
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return View(model);
        }

        [AjaxAuthorize(Roles = "materialreceive-2,materialreceive-3")]
        public ActionResult Edit(ItemStoreViewModel model)
        {
            ModelState.Clear();
            model.Currencies = CurrencyManagerCommon.GetAllCourrency();
            var authorizedPersons = InventoryAuthorizedPersonManager.GetAuthorizedPersonsByProcessTypeId((int)InventoryProcessType.StorePurchaseRequisition, (int)StorePurchaseRequisition.Purchases);
            var supplierCompanies = SupplierCompanyManager.GetAllSupplierCompany();
            model.AuthorizedPersons = authorizedPersons;
            model.Suppliers = supplierCompanies;
            model.InventoryBrands = BrandManager.GetBrands();
            model.InventoryOrigins = CountryManager.GetAllCountries();
            model.InventorySizes = SizeManager.GetSizeList();
                if (model.ItemStoreId > 0)
                {
                    var itemStore =
                    ItemStoreManager.GetInventoryItemStoreByItemStoreId(model.ItemStoreId);
                    model.InventoryItemStoreDetails = ItemStoreManager.GetVItemReciveDetails(model.ItemStoreId).ToDictionary(x => Convert.ToString(x.ItemStoreDetailId), x => x);
                    model.ReceivedDate = itemStore.ReceivedDate;
                    model.ReceivedRegisterNo = itemStore.ReceivedRegisterNo;
                    model.RequisitionNo = itemStore.RequisitionNo;
                    model.Requisitor = itemStore.Requisitor;
                    if (model.Requisitor.HasValue)
                    {
                        model.RequisitorName = EmployeeManager.GetEmployeeById(model.Requisitor.Value).Name;
                    }
                    model.PurchaseReferencePerson = itemStore.PurchaseReferencePerson;
                    model.LCNo = itemStore.LCNo;
                    model.SupplierId = itemStore.SupplierId;
                    model.InvoiceNo = itemStore.InvoiceNo;
                    model.InvoiceDate = itemStore.InvoiceDate;
                    model.GateEntry = itemStore.GateEntry;
                    model.GateEntryDate = itemStore.GateEntryDate;
                   
                    model.IsProcessRequsition = itemStore.IsProcessRequsition;
                    model.ReceiveType = itemStore.ReceiveType;
                    model.IsSearch = true;

                    model.SuppliedStatus = itemStore.SuppliedStatus;
                }
                else
                {
                    model.IsProcessRequsition = true;
                    model.InvoiceDate = DateTime.Now;
                    model.ReceivedDate = DateTime.Now;
                    model.GateEntryDate = DateTime.Now;
                }
            return View(model);
        }

        public ActionResult ItemReceive(ItemStoreViewModel model)
        {
            ModelState.Clear();
            model.Currencies = CurrencyManagerCommon.GetAllCourrency();
            var authorizedPersons = InventoryAuthorizedPersonManager.GetAuthorizedPersonsByProcessTypeId((int)InventoryProcessType.StorePurchaseRequisition, (int)StorePurchaseRequisition.Purchases);
            model.AuthorizedPersons = authorizedPersons;
            var supplierCompanies = SupplierCompanyManager.GetAllSupplierCompany();
            model.Suppliers = supplierCompanies;
            if (model.StorePurchaseRequisitionId > 0)
            {
                var vStorePurchaseRequisition = StorePurchaseRequisitionManager.GetVStorePurchaseRequisitionById(model.StorePurchaseRequisitionId);
                var sprdetails = StorePurchaseRequisitionManager.GetVStorePurchaseRequisitionDetails(model.StorePurchaseRequisitionId);
                var materialReq =
                    MaterialRequisitionManager.GetVMaterialRequisitionById(
                        vStorePurchaseRequisition.MaterialRequisitionId);
                model.RequisitionNo = vStorePurchaseRequisition.RequisitionNo;
                model.Requisitor = materialReq.PreparedBy;
                model.RequisitorName = materialReq.PreparedByEmployeeName;
                model.InventoryItemStoreDetails = sprdetails;
                model.InvoiceDate = DateTime.Now;
                model.ReceivedDate = DateTime.Now;
                model.GateEntryDate = DateTime.Now;
                model.InventoryBrands = BrandManager.GetBrands();
                model.InventoryOrigins = CountryManager.GetAllCountries();
                model.InventorySizes = SizeManager.GetSizeList();
                model.ReceiveType = 1;
            }
            model.IsProcessRequsition = true;
            
            return View("~/Areas/Inventory/Views/ItemStore/_ItemReceive.cshtml", model);
        }

        [AjaxAuthorize(Roles = "materialreceive-2,materialreceive-3")]
        public ActionResult AddRow(ItemStoreViewModel model)
        {
            model.InventoryBrands = BrandManager.GetBrands();
            model.InventoryOrigins = CountryManager.GetAllCountries();
            model.InventorySizes = SizeManager.GetSizeList();
            model.Currencies = CurrencyManagerCommon.GetAllCourrency();

            model.InventoryItemStoreDetails.Add(model.Key, new VItemReceiveDetail());
            return PartialView(model);
        }

        [AjaxAuthorize(Roles = "materialreceive-2,materialreceive-3")]
        public ActionResult Save(Inventory_ItemStore itemStore, ItemStoreViewModel model)
        {
            var saveIndex = 0;
            try
            {
                var inventoryItemStoreDetailIds =
                    model.InventoryItemStoreDetails.Select(x => x.Value.StorePurchaseRequisitionDetailId).Where(x => x > 0).ToList();
                itemStore.Inventory_ItemStoreDetail = ConvetItemStoreDetai(model.InventoryItemStoreDetails.Select(x => x.Value));

                saveIndex = model.ItemStoreId > 0 ? ItemStoreManager.EditItemStor(itemStore) : ItemStoreManager.SaveItemStor(itemStore, inventoryItemStoreDetailIds);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return saveIndex > 0 ? Reload() : ErrorResult("Fail to save data");

        }

        [AjaxAuthorize(Roles = "materialreceive-3")]
        public JsonResult Delete(Inventory_ItemStore itemStore)
        {
            var deleteIndex = 0;
            try
            {
                deleteIndex = ItemStoreManager.DeleteItemStore(itemStore.ItemStoreId);
            }
            catch (Exception exception)
            {

                Errorlog.WriteLog(exception);
            }
            return deleteIndex > 0 ? Reload() : ErrorResult("Fail To delte Item store");
        }

        public JsonResult AutocompliteRquisitionNumber(string searchString )
        {

          var sprList=  StorePurchaseRequisitionManager.GetStorePurchaseRequisitions(searchString);

            return Json(sprList, JsonRequestBehavior.AllowGet);
        }

   
        public ActionResult Report(int itemStoreId)
        {
            string reportName = "MaterialReceived";
            var reportParams = new List<ReportParameter> { new ReportParameter("ItemStoreId", itemStoreId.ToString())
                , new ReportParameter("CompId", PortalContext.CurrentUser.CompId)
                , new ReportParameter("HostingServerAddress", AppConfig.HostingServerAddress)};
            return ReportExtension.ToSsrsFile(ReportType.PDF, reportName, reportParams);
        }
        public ActionResult AutocompliteItemByBranch(string itemName)
        {
            var itemList = InventoryItemManager.AutocompliteItemByBranch(itemName);
            return Json(itemList, JsonRequestBehavior.AllowGet);
        }
        private List<Inventory_ItemStoreDetail> ConvetItemStoreDetai(IEnumerable<VItemReceiveDetail> itemReceives)
        {
            var temStoreDetails = itemReceives.Select(x => new Inventory_ItemStoreDetail
                {
                    ItemStoreDetailId = x.ItemStoreDetailId,
                    ItemStoreId = x.ItemStoreId,
                    ItemId = x.ItemId,
                    SizeId = x.SizeId,
                    BrandId = x.BrandId,
                    OriginId = x.OriginId,
                    CurrencyId = x.CurrencyId,
                    UnitPrice = x.UnitPrice,
                    Specification = x.Specification,
                    ReceivedQuantity = x.ReceivedQuantity,
                    SuppliedQuantity = x.SuppliedQuantity,
                    ManufacturingDate = x.ManufacturingDate,
                    ExpirationDate = x.ExpirationDate,
                    CreatedBy = PortalContext.CurrentUser.UserId,
                    CreatedDate = DateTime.Now,
                    IsActive = true

                }).ToList();
            return temStoreDetails;
        }

    }
}