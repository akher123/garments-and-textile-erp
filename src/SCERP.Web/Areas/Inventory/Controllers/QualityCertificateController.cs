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
using SCERP.Web.Controllers;
using SCERP.Web.Helpers;


namespace SCERP.Web.Areas.Inventory.Controllers
{
    public class QualityCertificateController : BaseInventoryController
    {
        [AjaxAuthorize(Roles = "qualitycertificate-1,qualitycertificate-2,qualitycertificate-3")]
        public ActionResult Index(QualityCertificateViewModel model)
        {
            try
            {
                var totalRecords = 0;
                ModelState.Clear();
                var courrencies = CurrencyManagerCommon.GetAllCourrency();
                model.Companies = courrencies;
                var supplierCompanies = SupplierCompanyManager.GetAllSupplierCompany();
                model.Companies = CompanyManager.GetAllPermittedCompanies();
                model.Suppliers = supplierCompanies;
                if (!model.IsSearch)
                {
                    model.IsSearch = true;
                    return View(model);
                }
                var qualityCertificates = QualityCertificateManager.GetQualityCertificateByPaging(out totalRecords, model);
                model.QualityCertificates = qualityCertificates;
                model.TotalRecords = totalRecords;
            }
            catch (Exception exception)
            {

                Errorlog.WriteLog(exception);
            }

            return View(model);
        }

        [AjaxAuthorize(Roles = "qualitycertificate-2,qualitycertificate-3")]
        public ActionResult Edit(QualityCertificateViewModel model)
        {
            ModelState.Clear();
            var qcReferenceNo = QualityCertificateManager.GetNewQCReferenceNo();
            model.QCReferenceNo = qcReferenceNo;
            var isQcPassed = ItemStoreManager.CheckQcPassed(model.ItemStoreId);
            model.IsActive = true;
            if (isQcPassed)
            {
                return ErrorResult("QC all ready Passed");
            }
            return View(model);
        }

        [AjaxAuthorize(Roles = "qualitycertificate-2,qualitycertificate-3")]
        public ActionResult UpdateQualityCertificate(QualityCertificateViewModel model)
        {
            ModelState.Clear();
            var qualityCertificate =
                QualityCertificateManager.GetQualityCertificateById(model.QualityCertificateId);
            model.ItemStoreId = qualityCertificate.ItemStoreId;
            model.QCReferenceNo = qualityCertificate.QCReferenceNo;
            model.SendingDate = qualityCertificate.SendingDate;
            model.IsSearch = true;
            model.VQualityCertificateDetails = QualityCertificateManager.GetQualityCertificateDetailIds(model.ItemStoreId, model.QualityCertificateId);
            return View(model);
        }

        [AjaxAuthorize(Roles = "qualitycertificate-2,qualitycertificate-3")]
        public ActionResult Save(QualityCertificateViewModel model)
        {
            var effectedRows = 0;
            var qcCertificate = new Inventory_QualityCertificate
            {
                QCReferenceNo = model.QCReferenceNo,
                ItemStoreId = model.ItemStoreId,
                IsActive = true,
                IsGrnConverted = false,
                SendingDate = model.SendingDate
            };
            if (model.VQualityCertificateDetails.Any() && model.QualityCertificateId > 0)
            {
                qcCertificate.QualityCertificateId = model.QualityCertificateId;
                qcCertificate.Inventory_QualityCertificateDetail =
                    model.VQualityCertificateDetails.Select(x => new Inventory_QualityCertificateDetail()
                    {
                        QualityCertificateId = x.QualityCertificateId,
                        QualityCertificateDetailId = x.QualityCertificateDetailId,
                        RejectedQuantity = x.RejectedQuantity,
                        CorrectQuantity = x.CorrectQuantity,
                        Remarks = x.Remarks
                    }).ToList();
                effectedRows = QualityCertificateManager.EditQualityCertificate(qcCertificate);
            }
            else
            {
                qcCertificate.CreatedBy = PortalContext.CurrentUser.UserId;
                qcCertificate.CreatedDate = DateTime.Now;
                effectedRows = QualityCertificateManager.SaveQualityCertificate(qcCertificate);
            }
            return effectedRows > 0 ? Reload() : ErrorResult("Fail to save");

        }

        [AjaxAuthorize(Roles = "qualitycertificate-1,qualitycertificate-2,qualitycertificate-3")]
        public ActionResult Report(int qualityCertificateId)
        {
            string reportName = "QualityCertificate";
            var reportParams = new List<ReportParameter> {
             new ReportParameter("QualityCertificateId", qualityCertificateId.ToString()),
             new ReportParameter("CompId", PortalContext.CurrentUser.CompId),
             new ReportParameter("HostingServerAddress", AppConfig.HostingServerAddress)};
            return ReportExtension.ToSsrsFile(ReportType.PDF, reportName, reportParams);
        }

        [AjaxAuthorize(Roles = "qualitycertificate-3")]
        public JsonResult Delete(int? qualityCertificateId)
        {
            var deleteIndex = 0;
            try
            {
                deleteIndex = QualityCertificateManager.DeleteQualityCertificate(qualityCertificateId);
            }
            catch (Exception exception)
            {

                Errorlog.WriteLog(exception);
            }
            return deleteIndex > 0 ? Reload() : ErrorResult("Fail To delte Item store");
        }


        public ActionResult GetAllBranchesByCompanyId(int companyId)
        {
            var branches = BranchManager.GetAllPermittedBranchesByCompanyId(companyId);
            return Json(new { Success = true, Branches = branches }, JsonRequestBehavior.AllowGet);
        }

    }
}