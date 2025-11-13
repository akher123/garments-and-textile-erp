using SCERP.BLL.IManager.ICommercialManager;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.Common;
using SCERP.Model.CommercialModel;
using SCERP.Web.Areas.Commercial.Models.ViewModel;
using SCERP.Web.Controllers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SCERP.Web.Areas.Commercial.Controllers
{
    public class CashBbLcController : BaseController
    {
        private readonly int _pageSize = AppConfig.PageSize;
        private readonly ICashBbLcManager _cashBbLcManager;
        private readonly ILcManager _lcManager;
        private readonly ISupplierCompanyManager _supplierCompanyManager;
        private readonly IPaymentTermManager _paymentTermManager;

        public CashBbLcController(ICashBbLcManager cashBbLcManager, ILcManager lcManager, ISupplierCompanyManager supplierCompanyManager, IPaymentTermManager paymentTermManager)
        {
            _cashBbLcManager = cashBbLcManager;
            _lcManager = lcManager;
            _supplierCompanyManager = supplierCompanyManager;
            _paymentTermManager = paymentTermManager;
        }

        // GET: Commercial/CashBbLc
        [AjaxAuthorize(Roles = "cashlc-1,cashlc-2,cashlc-3")]
        public ActionResult Index(CashBbLcViewModel model)
        {
            try
            {
                ModelState.Clear();

                IEnumerable lcTypeList = from LcType lcType in Enum.GetValues(typeof(LcType))
                                         select new { Id = (int)lcType, Name = lcType.ToString() };

                model.LcTypes = lcTypeList;
                model.Lcs = _lcManager.GetAllLcInfos();
                model.Suppliers = _supplierCompanyManager.GetAllSupplierCompany();

                CommCashBbLcInfo cashBbLcInfo = model;
                cashBbLcInfo.CompId = model.CompId;
                cashBbLcInfo.LcRefId = model.LcRefId;
                cashBbLcInfo.BbLcType = model.BbLcType;
                cashBbLcInfo.SupplierCompanyRefId = model.SupplierCompanyRefId;
                cashBbLcInfo.FromDate = model.FromDate;
                cashBbLcInfo.ToDate = model.ToDate;

                int totalRecords = 0;
                model.CashBbLcInfos = _cashBbLcManager.GetAllCashBbLcInfosByPaging(out totalRecords, cashBbLcInfo) ?? new List<CommCashBbLcInfo>();
                //model.CashBbLcInfos = _cashBbLcManager.GetAllCashBbLcInfos();
                model.TotalRecords = totalRecords;
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return View(model);
        }
        [AjaxAuthorize(Roles = "cashlc-2,cashlc-3")]
        public ActionResult Edit(int id)
        {
            ModelState.Clear();
            CashBbLcViewModel model = new CashBbLcViewModel();

            try
            {
                IEnumerable lcTypeList = from LcType lcType in Enum.GetValues(typeof(LcType)) select new { Id = (int)lcType, Name = lcType.ToString() };
                IEnumerable partialShipment = from PartialShipment shipment in Enum.GetValues(typeof(PartialShipment)) select new { Id = (int)shipment, Name = shipment.ToString() };
                model.Suppliers = _supplierCompanyManager.GetAllSupplierCompany();
               
                //model.LCTypeName = LcTypeList;
                model.LcTypes = lcTypeList;
                model.Lcs = _lcManager.GetAllLcInfos();
                model.OmPaymentTerms= _paymentTermManager.GetPaymentTerms();
                model.Banks = _lcManager.GetBankInfo("Receiving");
                model.PartialShip = partialShipment;
                model.PartialShipment = 1;
                

                if (id > 0)
                {
                    CommCashBbLcInfo cashBbLc = _cashBbLcManager.GetCashBbLcInfoById(id);
                    model.CommCashBbLcDetailsDct =
                    cashBbLc.CommCashBbLcDetail.ToDictionary(
                        x => Convert.ToString(x.CashBbLcDetailsId), x => x);
                    model.CashBbLcInfo.BbLcId = cashBbLc.BbLcId;
                    model.CashBbLcInfo.BbLcNo = cashBbLc.BbLcNo;
                    model.CashBbLcInfo.BbLcDate = cashBbLc.BbLcDate;
                    model.CashBbLcInfo.SupplierCompanyRefId = cashBbLc.SupplierCompanyRefId;
                    model.CashBbLcInfo.PI = cashBbLc.PI;
                    model.CashBbLcInfo.PaymentMode= cashBbLc.PaymentMode;
                    model.CashBbLcInfo.LcRefId = cashBbLc.LcRefId;
                    model.CashBbLcInfo.BbLcAmount = cashBbLc.BbLcAmount;
                    model.CashBbLcInfo.BbLcQuantity = cashBbLc.BbLcQuantity;
                    model.CashBbLcInfo.MatureDate = cashBbLc.MatureDate;
                    model.CashBbLcInfo.ExpiryDate = cashBbLc.ExpiryDate;
                    model.CashBbLcInfo.ExtensionDate = cashBbLc.ExtensionDate;
                    model.CashBbLcInfo.BbLcIssuingBank = cashBbLc.BbLcIssuingBank;
                    model.CashBbLcInfo.IssuingBankId = cashBbLc.IssuingBankId;
                    model.CashBbLcInfo.BbLcIssuingBankAddress = cashBbLc.BbLcIssuingBankAddress;
                    model.CashBbLcInfo.ReceivingBank = cashBbLc.ReceivingBank;
                    model.CashBbLcInfo.ReceivingBankAddress = cashBbLc.ReceivingBankAddress;
                    model.CashBbLcInfo.BbLcType = cashBbLc.BbLcType;
                    model.CashBbLcInfo.Beneficiary = cashBbLc.Beneficiary;
                    model.CashBbLcInfo.PartialShipment = cashBbLc.PartialShipment;
                    model.CashBbLcInfo.Description = cashBbLc.Description;
                    model.CashBbLcInfo.LCType = cashBbLc.LCType;
                    model.CashBbLcInfo.IfdbcNo = cashBbLc.IfdbcNo;
                    model.CashBbLcInfo.IfdbcDate = cashBbLc.IfdbcDate;
                    model.CashBbLcInfo.IfdbcValue = cashBbLc.IfdbcValue;
                    model.CashBbLcInfo.PcsSanctionAmount = cashBbLc.PcsSanctionAmount;

                    model.CashBbLcInfo.PaymentDate = cashBbLc.PaymentDate;
                    model.CashBbLcInfo.BtmeaNo = cashBbLc.BtmeaNo;
                    model.CashBbLcInfo.BtmeaDate = cashBbLc.BtmeaDate;
                    model.CashBbLcInfo.BeNo = cashBbLc.BeNo;
                    model.CashBbLcInfo.BeDate = cashBbLc.BeDate;
                    model.CashBbLcInfo.Vat = cashBbLc.Vat;
                    model.PaymentModeRefId ="0"+cashBbLc.PaymentMode.ToString();
                    ViewBag.Title = "Edit LC";
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

        public ActionResult AddNewRow([Bind(Include = "CashBbLcDetail")]CashBbLcViewModel model)
        {
            ModelState.Clear();
            model.Key = DateTime.Now.ToString("yyyyMMddHHmmssffff");
            model.CommCashBbLcDetailsDct.Add(model.Key, model.CashBbLcDetail);
            return PartialView("~/Areas/Commercial/Views/CashBbLc/_CashBbLcDetails.cshtml", model);
        }

        [AjaxAuthorize(Roles = "cashlc-2,cashlc-3")]
        public ActionResult Save(CashBbLcViewModel model)
        {
            int saveIndex = 0;

            try
            {
                CommCashBbLcInfo cashBbLcInfo = _cashBbLcManager.GetCashBbLcInfoById(model.CashBbLcInfo.BbLcId) ?? new CommCashBbLcInfo();
                cashBbLcInfo.BbLcNo = model.CashBbLcInfo.BbLcNo;
                cashBbLcInfo.BbLcDate = model.CashBbLcInfo.BbLcDate;
                cashBbLcInfo.SupplierCompanyRefId = model.CashBbLcInfo.SupplierCompanyRefId;
                cashBbLcInfo.LcRefId = model.CashBbLcInfo.LcRefId;
                cashBbLcInfo.PI = model.CashBbLcInfo.PI;
                cashBbLcInfo.PaymentMode = int.Parse(model.PaymentModeRefId);
                cashBbLcInfo.BbLcAmount = model.CashBbLcInfo.BbLcAmount;
                cashBbLcInfo.BbLcQuantity = model.CashBbLcInfo.BbLcQuantity;
                cashBbLcInfo.MatureDate = model.CashBbLcInfo.MatureDate;
                cashBbLcInfo.ExpiryDate = model.CashBbLcInfo.ExpiryDate;
                cashBbLcInfo.ExtensionDate = model.CashBbLcInfo.ExtensionDate;
                cashBbLcInfo.IssuingBankId = model.CashBbLcInfo.IssuingBankId;
                cashBbLcInfo.BbLcIssuingBankAddress = model.CashBbLcInfo.BbLcIssuingBankAddress;
                cashBbLcInfo.ReceivingBank = model.CashBbLcInfo.ReceivingBank;
                cashBbLcInfo.ReceivingBankAddress = model.CashBbLcInfo.ReceivingBankAddress;
                cashBbLcInfo.BbLcType = model.CashBbLcInfo.BbLcType;
                cashBbLcInfo.Beneficiary = model.CashBbLcInfo.Beneficiary;
                cashBbLcInfo.PartialShipment = model.CashBbLcInfo.PartialShipment;
                cashBbLcInfo.Description = model.CashBbLcInfo.Description;
                cashBbLcInfo.IfdbcNo = model.CashBbLcInfo.IfdbcNo;
                cashBbLcInfo.IfdbcDate = model.CashBbLcInfo.IfdbcDate;
                cashBbLcInfo.IfdbcValue = model.CashBbLcInfo.IfdbcValue;
                cashBbLcInfo.PcsSanctionAmount = model.CashBbLcInfo.PcsSanctionAmount;
                cashBbLcInfo.PaymentDate = model.CashBbLcInfo.PaymentDate;
                cashBbLcInfo.BtmeaNo = model.CashBbLcInfo.BtmeaNo;
                cashBbLcInfo.BtmeaDate = model.CashBbLcInfo.BtmeaDate;
                cashBbLcInfo.BeNo = model.CashBbLcInfo.BeNo;
                cashBbLcInfo.BeDate = model.CashBbLcInfo.BeDate;
                cashBbLcInfo.Vat = model.CashBbLcInfo.Vat;
                cashBbLcInfo.LCType = model.CashBbLcInfo.LCType;
                cashBbLcInfo.IsActive = true;
                cashBbLcInfo.CompId = PortalContext.CurrentUser.CompId;

                cashBbLcInfo.CommCashBbLcDetail=model.CommCashBbLcDetailsDct.Select(x => x.Value).ToList();
                if (_cashBbLcManager.CheckExistingCashBbLcInfo(cashBbLcInfo))
                    return ErrorResult("Duplicate BBLC No exists !");
                //model.CashBbLcInfo.CommCashBbLcDetail= model.CommCashBbLcDetailsDct.Select(x => x.Value).ToList();
                saveIndex = (model.CashBbLcInfo.BbLcId > 0) ? _cashBbLcManager.EditCashBbLcInfo(cashBbLcInfo) : _cashBbLcManager.SaveCashBbLcInfo(cashBbLcInfo);
            }
            catch (Exception exception)
            {
                //Errorlog.WriteLog(exception);
                return (saveIndex > 0) ? Reload() : ErrorResult(exception.ToString());
            }
            
            if (saveIndex > 0)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return ErrorResult("Save Failed");
            }
        }
        [AjaxAuthorize(Roles = "cashlc-2,cashlc-3")]
        public ActionResult Delete(int id)
        {
            int deleted = 0;

            try
            {
                CommCashBbLcInfo lcInfo = _cashBbLcManager.GetCashBbLcInfoById(id) ?? new CommCashBbLcInfo();
                deleted = _cashBbLcManager.DeleteCashBbLcInfo(lcInfo);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return (deleted > 0) ? Reload() : ErrorResult("Failed to delete data");
        }


    }
}