using System;

using System.Web.Mvc;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.Common;
using SCERP.Model.MerchandisingModel;
using SCERP.Web.Areas.Merchandising.Models.ViewModel;
using SCERP.Web.Controllers;

namespace SCERP.Web.Areas.Merchandising.Controllers
{
    public class InquiryController : BaseController
    {
        private readonly IInquiryManager _inquiryManager;

        public InquiryController(IInquiryManager inquiryManager)
        {
            _inquiryManager = inquiryManager;
        }
        [AjaxAuthorize(Roles = "inquiry-1,inquiry-2,inquiry-3")]
        public ActionResult Index(InquiryViewModel model)
        {
            ModelState.Clear();
            var totalRecords = 0;
            model.Inquiries = _inquiryManager.GetInquiriesByPaging(model.PageIndex, model.sort, model.sortdir, model.SearchString, PortalContext.CurrentUser.CompId, out totalRecords);
            model.TotalRecords = totalRecords;
            return View(model);
        }
        [AjaxAuthorize(Roles = "inquiry-2,inquiry-3")]
        public ActionResult Edit(InquiryViewModel model)
        {
            if (model.Inquiry.InquiryId > 0)
            {
                model.Inquiry = _inquiryManager.GetInquiryById(model.Inquiry.InquiryId);

            }
            else
            {
                model.Inquiry.InquiryRef = _inquiryManager.GetInqueryRefId(PortalContext.CurrentUser.CompId);
            }

            return View(model);
        }
        [AjaxAuthorize(Roles = "inquiry-2,inquiry-3")]
        public ActionResult Save(InquiryViewModel model)
        {
            try
            {
                OM_Inquiry nquiry = _inquiryManager.GetInquiryById(model.Inquiry.InquiryId) ?? new OM_Inquiry();
                nquiry.BuyerName = model.Inquiry.BuyerName;
                nquiry.Photo = model.Inquiry.Photo;
                nquiry.Merchandiser = model.Inquiry.Merchandiser;
                nquiry.InquiryRef = model.Inquiry.InquiryRef;
                nquiry.Season = model.Inquiry.Season;
                nquiry.DesignRef = model.Inquiry.DesignRef;
                nquiry.Designer = model.Inquiry.Designer;
                nquiry.Description = model.Inquiry.Description;
                nquiry.FabricationGsm = model.Inquiry.FabricationGsm;
                nquiry.Colour = model.Inquiry.Colour;
                nquiry.SampleType = model.Inquiry.SampleType;
                nquiry.SampleSize = model.Inquiry.SampleSize;
                nquiry.RecvedDate = model.Inquiry.RecvedDate;
                nquiry.SubmissionDate = model.Inquiry.SubmissionDate;
                nquiry.PriceUSD = model.Inquiry.PriceUSD;
                nquiry.Remarks = model.Inquiry.Remarks;
                nquiry.IsActive = true;

                if (nquiry.InquiryId > 0)
                {
                    nquiry.EditedDate = DateTime.Now;
                    nquiry.EditedBy = PortalContext.CurrentUser.UserId;
                    nquiry.CompId = PortalContext.CurrentUser.CompId;
                }
                else
                {
                    nquiry.CreatedDate = DateTime.Now;
                    nquiry.CreatedBy = PortalContext.CurrentUser.UserId;
                    nquiry.CompId = PortalContext.CurrentUser.CompId;
                    model.Inquiry.InquiryRef = _inquiryManager.GetInqueryRefId(PortalContext.CurrentUser.CompId);
                }
                int saveIndex = _inquiryManager.SaveInquiry(nquiry);

                if (saveIndex > 0)
                {
                    return Reload();
                }
                else
                {
                    return ErrorMessageResult();
                }

            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult(exception.Message);
            }
        }
        [AjaxAuthorize(Roles = "inquiry-3")]
        public ActionResult Delete(int inquiryId)
        {
           
            try
            {
                OM_Inquiry inquiry = _inquiryManager.GetInquiryById(inquiryId);
                inquiry.IsActive = false;
                int saveIndex = _inquiryManager.SaveInquiry(inquiry);
                return saveIndex > 0 ? Reload() : ErrorResult("Delete Failed");

            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult(exception.Message);
            }

        }
    }
}