using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using SCERP.BLL.IManager.ICommercialManager;
using SCERP.BLL.IManager.IPlanningManager;
using SCERP.Model.CommercialModel;
using SCERP.Web.Areas.Commercial.Models.ViewModel;
using SCERP.Web.Controllers;
using SCERP.Common;
using System.Collections;
using SCERP.Model;

namespace SCERP.Web.Areas.Commercial.Controllers
{
  
    public class LcController : BaseController
    {
        private readonly int _pageSize = AppConfig.PageSize;
        private readonly ILcManager _lcManager;
        private readonly ITNAManager _tnaManager;
        private readonly IReportManager _reportManager;

        public LcController(ILcManager lcManager, ITNAManager tnaManager, IReportManager reportManager)
        {
            this._lcManager = lcManager;
            this._tnaManager = tnaManager;
            this._reportManager = reportManager;
        }
        [AjaxAuthorize(Roles = "lc-1,lc-2,lc-3")]
        public ActionResult Index(LcViewModel model)
        {
            try
            {
                ModelState.Clear();
                model.RStatus = model.RStatus ?? "O";
                model.Buyers = _tnaManager.GetAllBuyers();
                model.Banks = _lcManager.GetBankInfo("Receiving");
       
                int totalRecords = 0;
                model.VwCommLcInfos = _lcManager.GetLcInfos(model.PageIndex, out totalRecords, model.RStatus,
                    model.ReceivingBankId,model.BuyerId,model.SearchString);
                model.TotalRecords = totalRecords;
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return View(model);
        }
        [AjaxAuthorize(Roles = "lc-2,lc-3")]
        public ActionResult Edit(int id)
        {
            ModelState.Clear();
            LcViewModel model = new LcViewModel();

            try
            {
                Guid? userId = PortalContext.CurrentUser.UserId;
                Acc_CompanySector companyName = _reportManager.GetActiveCompanySectory(userId);
                model.GroupLcs = _lcManager.GetAllGroupLcs(PortalContext.CurrentUser.CompId);
                IEnumerable lcTypeList = from LcType lcType in Enum.GetValues(typeof (LcType)) select new {Id = (int) lcType, Name = lcType.ToString()};
                IEnumerable partialShipment = from PartialShipment shipment in Enum.GetValues(typeof (PartialShipment)) select new {Id = (int) shipment, Name = shipment.ToString()};
                model.Buyers = _tnaManager.GetAllBuyers();
                model.Banks = _lcManager.GetBankInfo("Receiving");
                model.LcTypes = lcTypeList;
                model.PartialShip = partialShipment;
                model.PartialShipment = 1;
                model.Beneficary = companyName.SectorName;

                if (id > 0)
                {
                    COMMLcInfo lc = _lcManager.GetLcInfoById(id);
                    model.LcId = lc.LcId;
                    model.LcNo = lc.LcNo;
                    model.LcDate = lc.LcDate;
                    model.BuyerId = lc.BuyerId;
                    model.LcAmount = lc.LcAmount;
                    model.LcQuantity = lc.LcQuantity;
                    model.MatureDate = lc.MatureDate;
                    model.ExpiryDate = lc.ExpiryDate;
                    model.ExtensionDate = lc.ExtensionDate;
                    model.ShipmentDate = lc.ShipmentDate;
                    model.LcIssuingBank = lc.LcIssuingBank;
                    model.LcIssuingBankAddress = lc.LcIssuingBankAddress;
                    model.ReceivingBank = lc.ReceivingBank;
                    model.ReceivingBankId = lc.ReceivingBankId;
                    model.ReceivingBankAddress = lc.ReceivingBankAddress;
                    model.SalesContactNo = lc.SalesContactNo;
                    model.LcType = lc.LcType;
                    model.GroupLcId = lc.GroupLcId;
                    model.Beneficary = lc.Beneficary;
                    model.PartialShipment = lc.PartialShipment;
                    model.Beneficary = lc.Beneficary;
                    model.Description = lc.Description;
                    model.RStatus = lc.RStatus;
                    model.CommissionPrc = lc.CommissionPrc;
                    model.CommissionsAmount = lc.CommissionsAmount;
                    model.FreightAmount = lc.FreightAmount;
                    model.UdEoNo = lc.UdEoNo;
                    model.FileNo = lc.FileNo;
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
        [AjaxAuthorize(Roles = "lc-2,lc-3")]
        public ActionResult Save(LcViewModel model)
        {
            int saveIndex = 0;

            try
            {
                COMMLcInfo lcInfo = _lcManager.GetLcInfoById(model.LcId) ?? new COMMLcInfo();
                lcInfo.LcNo = model.LcNo;
                lcInfo.LcDate = model.LcDate;
                lcInfo.BuyerId = model.BuyerId;
                lcInfo.LcAmount = model.LcAmount;
                lcInfo.LcQuantity = model.LcQuantity;
                lcInfo.MatureDate = model.MatureDate;
                lcInfo.ExpiryDate = model.ExpiryDate;
                lcInfo.ExtensionDate = model.ExtensionDate;
                lcInfo.ShipmentDate = model.ShipmentDate;
                lcInfo.LcIssuingBank = model.LcIssuingBank;
                lcInfo.LcIssuingBankAddress = model.LcIssuingBankAddress;
                lcInfo.ReceivingBank = model.ReceivingBank;
                lcInfo.ReceivingBankId = model.ReceivingBankId;
                lcInfo.ReceivingBankAddress = model.ReceivingBankAddress;
                lcInfo.SalesContactNo = model.SalesContactNo;
                lcInfo.LcType = model.LcType;
                lcInfo.GroupLcId = model.GroupLcId;
                lcInfo.PartialShipment = model.PartialShipment;
                lcInfo.Beneficary = model.Beneficary ?? " ";
                lcInfo.Description = model.Description;
                lcInfo.IsActive = true;
                lcInfo.RStatus = model.RStatus;
                lcInfo.CommissionPrc = model.CommissionPrc;
                lcInfo.CommissionsAmount = model.CommissionsAmount;
                lcInfo.FreightAmount = model.FreightAmount;
                lcInfo.UdEoNo = model.UdEoNo;
                lcInfo.FileNo = model.FileNo;
                if (_lcManager.CheckExistingLcInfo(lcInfo))
                    return ErrorResult("Duplicate LC No exists !");

                saveIndex = (model.LcId > 0) ? _lcManager.EditLcInfo(lcInfo) : _lcManager.SaveLcInfo(lcInfo);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return (saveIndex > 0) ? Reload() : ErrorResult("Failed to save data");
        }
        [AjaxAuthorize(Roles = "lc-3")]
        public ActionResult Delete(int id)
        {
            int deleted = 0;
            try
            {
                COMMLcInfo lcInfo = _lcManager.GetLcInfoById(id) ?? new COMMLcInfo();
                deleted = _lcManager.DeleteLcInfo(lcInfo);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return (deleted > 0) ? Reload() : ErrorResult("Failed to delete data");
        }

        public JsonResult SaveTest(List<string> values)
        {
            ModelState.Clear();
            var result = "";
            return Json(new {Success = true, Result = result});
        }

        public ActionResult TagLcSearch(string term)
        {
            List<string> tags = _lcManager.GetAllLcInfos().Select(p => p.LcNo).ToList();
            return this.Json(tags.Where(t => t.Substring(0, t.Length).Trim().ToLower().Contains(term.Trim().ToLower())), JsonRequestBehavior.AllowGet);
        }

        public ActionResult ImportExportPerformance(LcViewModel model)
        {
            model.Buyers = _tnaManager.GetAllBuyers();
            return View(model);
        }

        public ActionResult LcDetail(LcViewModel model)
        {
            return View(model);
        }

        public ActionResult LcOrderSummary(LcViewModel model)
        {
            return View(model);
        }

        public ActionResult OrderwithoutLC(LcViewModel model)
        {
            return View(model);
        }

        public ActionResult LcWithOrderDetail(LcViewModel model)
        {
            return View(model);
        }

        public ActionResult LcWithoutOrder(LcViewModel model)
        {
            return View(model);
        }

        public ActionResult LcWithOrderSummary(LcViewModel model)
        {
            return View(model);
        }

        public ActionResult GetBankAddressById(int? BankId)
        {
            var branches = BranchManager.GetAllPermittedBranchesByCompanyId(BankId.Value);
            return Json(new { Success = true, Branches = branches }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult LcStatus(string rStatus = "O", int receivingBankId = 0)
        {
            LcViewModel model = new LcViewModel();
            List<VwCommLcInfo> lcInfos = _reportManager.GetLcStatus(rStatus, receivingBankId);
            model.Banks = _lcManager.GetBankInfo("Receiving");
            model.VwCommLcInfos = lcInfos;
            return View(model);
        }
    }
}