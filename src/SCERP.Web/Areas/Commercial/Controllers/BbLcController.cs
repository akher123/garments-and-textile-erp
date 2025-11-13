using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using iTextSharp.text.pdf.qrcode;
using SCERP.BLL.IManager.ICommercialManager;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.Model.CommercialModel;
using SCERP.Web.Areas.Commercial.Models.ViewModel;
using SCERP.Web.Controllers;
using SCERP.Common;
using System.Collections;
using SCERP.BLL.IManager.ICommonManager;
using SCERP.BLL.IManager.IInventoryManager;

namespace SCERP.Web.Areas.Commercial.Controllers
{
    public class BbLcController : BaseController
    {
        private readonly int _pageSize = AppConfig.PageSize;
        private readonly IBbLcManager _bbLcManager;
        private readonly ILcManager _lcManager;
        private readonly ISupplierCompanyManager _supplierCompanyManager;
        private readonly IInventoryGroupManager _inventoryManager;
        private ISalesContactManager _salesContactManager;
        public BbLcController(IBbLcManager bbLcManager, ILcManager lcManager, ISupplierCompanyManager supplierCompanyManager, IInventoryGroupManager inventoryManager, ISalesContactManager salesContactManager)
        {
            this._bbLcManager = bbLcManager;
            this._lcManager = lcManager;
            this._supplierCompanyManager = supplierCompanyManager;
            _inventoryManager = inventoryManager;
            _salesContactManager = salesContactManager;
        }

        public ActionResult Index(BbLcViewModel model)
        {
            try
            {
                ModelState.Clear();

                //IEnumerable lcTypeList = from LcType lcType in Enum.GetValues(typeof (LcType))
                //    select new {Id = (int) lcType, Name = lcType.ToString()};

                var printFormat = from PrintFormatType formatType in Enum.GetValues(typeof(PrintFormatType))
                                  select new { Id = (int)formatType, Name = formatType.ToString() };

                //model.LcTypes = lcTypeList;
                model.Lcs = _lcManager.GetAllLcInfos();
                model.Suppliers = _supplierCompanyManager.GetAllSupplierCompany();
                model.PrintFormatStatuses = printFormat;
                model.Banks = _lcManager.GetBankInfo("Receiving");

                CommBbLcInfo bbLcInfo = model;
                bbLcInfo.CompId = model.CompId;
                bbLcInfo.LcRefId = model.LcRefId;
                bbLcInfo.BbLcType = model.BbLcType;
                bbLcInfo.SupplierCompanyRefId = model.SupplierCompanyRefId;
                bbLcInfo.FromDate = model.FromDate;
                bbLcInfo.ToDate = model.ToDate;

                if (string.IsNullOrEmpty(model.BbLcNo))
                    bbLcInfo.BbLcNo = "";
                else
                    bbLcInfo.BbLcNo = model.BbLcNo.Trim();

                bbLcInfo.MaturityDateFrom = model.MaturityDateFrom;
                bbLcInfo.MaturityDateTo = model.MaturityDateTo;
                bbLcInfo.ExpiryDateFrom = model.ExpiryDateFrom;
                bbLcInfo.ExpiryDateTo = model.ExpiryDateTo;
                bbLcInfo.DonothaveMaturityDate = model.DonothaveMaturityDate ?? false;
                bbLcInfo.DonothaveExpiryDate = model.DonothaveExpiryDate ?? false;
                bbLcInfo.IssuingBankId = model.IssuingBankId;

                int totalRecords = 0;
                model.BbLcInfos = _bbLcManager.GetAllBbLcInfosByPaging(out totalRecords, bbLcInfo) ?? new List<CommBbLcInfo>();
                model.TotalRecords = totalRecords;
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return View(model);
        }

        public ActionResult Edit(int id)
        {
            ModelState.Clear();
            BbLcViewModel model = new BbLcViewModel();

            try
            {
                IEnumerable lcTypeList = from LcType lcType in Enum.GetValues(typeof (LcType)) select new {Id = (int) lcType, Name = lcType.ToString()};
                IEnumerable partialShipment = from PartialShipment shipment in Enum.GetValues(typeof (PartialShipment)) select new {Id = (int) shipment, Name = shipment.ToString()};
                model.Suppliers = _supplierCompanyManager.GetAllSupplierCompany();
                model.LcTypes = lcTypeList;
                model.Lcs = _lcManager.GetAllLcInfos();
                model.InventoryGroupNames = _inventoryManager.GetGroups();
                model.Banks = _lcManager.GetBankInfo("Receiving");
                model.PartialShip = partialShipment;
                model.PartialShipment = 1;

                if (id > 0)
                {
                    CommBbLcInfo lc = _bbLcManager.GetBbLcInfoById((id));
                    model.BbLcId = lc.BbLcId;
                    model.CommBbLcItemDetailsDct =
                    lc.CommBbLcItemDetails.ToDictionary(
                        x => Convert.ToString(x.BbLcItemDetailsId), x => x);
                    model.BbLcNo = lc.BbLcNo;
                    model.BbLcDate = lc.BbLcDate;
                    model.SupplierCompanyRefId = lc.SupplierCompanyRefId;
                    model.LcRefId = lc.LcRefId;
                    model.BbLcAmount = lc.BbLcAmount;
                    model.BbLcQuantity = lc.BbLcQuantity;
                    model.MatureDate = lc.MatureDate;
                    model.ExpiryDate = lc.ExpiryDate;
                    model.ExtensionDate = lc.ExtensionDate;
                    model.ItemType = lc.ItemType;
                    model.IssuingBankId = lc.IssuingBankId;
                    model.BbLcIssuingBankAddress = lc.BbLcIssuingBankAddress;
                    model.ReceivingBank = lc.ReceivingBank;
                    model.ReceivingBankAddress = lc.ReceivingBankAddress;
                    model.BbLcType = lc.BbLcType;
                    model.Beneficiary = lc.Beneficiary;
                    model.PartialShipment = lc.PartialShipment;
                    model.Description = lc.Description;

                    model.IfdbcNo = lc.IfdbcNo;
                    model.IfdbcDate = lc.IfdbcDate;
                    model.IfdbcValue = lc.IfdbcValue;
                    model.PcsSanctionAmount = lc.PcsSanctionAmount;

                    model.PaymentDate = lc.PaymentDate;
                    model.BtmeaNo = lc.BtmeaNo;
                    model.BtmeaDate = lc.BtmeaDate;
                    model.BeNo = lc.BeNo;
                    model.BeDate = lc.BeDate;
                    model.Vat = lc.Vat;

                    ViewBag.Title = "Edit LC";
                    model.SalseContactId = lc.SalseContactId;
                    model.CommSalesContacts = _salesContactManager.GetSalesContacts(model.LcRefId);
                }
                else
                {
                    ViewBag.Title = "Add LC";
                }
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return View(model);
        }

        public ActionResult AddNewRow([Bind(Include = "CommBbLcItemDetails")]BbLcViewModel model)
        {
            ModelState.Clear();
            model.Key = DateTime.Now.ToString("yyyyMMddHHmmssffff");
            model.CommBbLcItemDetailsDct.Add(model.Key, model.CommBbLcItemDetails);
            return PartialView("~/Areas/Commercial/Views/BbLc/_BbLcItemDetails.cshtml", model);
        }

        public ActionResult Save(BbLcViewModel model)
        {
            int saveIndex = 0;

            try
            {
                CommBbLcInfo bbLcInfo = _bbLcManager.GetBbLcInfoById(model.BbLcId) ?? new CommBbLcInfo();

                bbLcInfo.BbLcNo = model.BbLcNo;
                bbLcInfo.BbLcDate = model.BbLcDate;
                bbLcInfo.SupplierCompanyRefId = model.SupplierCompanyRefId;
                bbLcInfo.LcRefId = model.LcRefId;
                bbLcInfo.BbLcAmount = model.BbLcAmount;
                bbLcInfo.BbLcQuantity = model.BbLcQuantity;
                bbLcInfo.MatureDate = model.MatureDate;
                bbLcInfo.ExpiryDate = model.ExpiryDate;
                bbLcInfo.ExtensionDate = model.ExtensionDate;
                bbLcInfo.IssuingBankId = model.IssuingBankId;
                bbLcInfo.BbLcIssuingBankAddress = model.BbLcIssuingBankAddress;
                bbLcInfo.ReceivingBank = model.ReceivingBank;
                bbLcInfo.ReceivingBankAddress = model.ReceivingBankAddress;
                bbLcInfo.BbLcType = model.BbLcType;
                bbLcInfo.Beneficiary = model.Beneficiary;
                bbLcInfo.PartialShipment = model.PartialShipment;
                bbLcInfo.Description = model.Description;
                bbLcInfo.IfdbcNo = model.IfdbcNo;
                bbLcInfo.IfdbcDate = model.IfdbcDate;
                bbLcInfo.IfdbcValue = model.IfdbcValue;
                bbLcInfo.PcsSanctionAmount = model.PcsSanctionAmount;
                bbLcInfo.PaymentDate = model.PaymentDate;
                bbLcInfo.BtmeaNo = model.BtmeaNo;
                bbLcInfo.BtmeaDate = model.BtmeaDate;
                bbLcInfo.BeNo = model.BeNo;
                bbLcInfo.BeDate = model.BeDate;
                bbLcInfo.Vat = model.Vat;
                bbLcInfo.ItemType = model.ItemType;
                bbLcInfo.IsActive = true;
                bbLcInfo.CompId = PortalContext.CurrentUser.CompId;
                bbLcInfo.SalseContactId = model.SalseContactId;
                bbLcInfo.CommBbLcItemDetails = model.CommBbLcItemDetailsDct.Select(x => x.Value).ToList();

                if (_bbLcManager.CheckExistingBbLcInfo(bbLcInfo))
                    return ErrorResult("Duplicate BBLC No exists !");

                saveIndex = (model.BbLcId > 0) ? _bbLcManager.EditBbLcInfo(bbLcInfo) : _bbLcManager.SaveBbLcInfo(bbLcInfo);
               
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
              
            }
            return (saveIndex > 0) ? Reload() : ErrorResult("Failed to save data");
        }

        public ActionResult Delete(int id)
        {
            int deleted = 0;

            try
            {
                CommBbLcInfo lcInfo = _bbLcManager.GetBbLcInfoById(id) ?? new CommBbLcInfo();
                deleted = _bbLcManager.DeleteBbLcInfo(lcInfo);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return (deleted > 0) ? Reload() : ErrorResult("Failed to delete data");
        }

    }
}