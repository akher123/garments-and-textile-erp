using SCERP.BLL.IManager.IMarketingManager;
using SCERP.Common;
using SCERP.Web.Areas.Marketing.Models;
using SCERP.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace SCERP.Web.Areas.Marketing.Controllers
{
    public class MarketingInquiryController : BaseController
    {
        private readonly IMarketingInquiryManager _marketingInquiryManager;
        private readonly IMarketingInstituteManager _marketingInstituteManager;
        public MarketingInquiryController(IMarketingInquiryManager marketingInquiryManager, IMarketingInstituteManager marketingInstituteManager)
        {
            _marketingInquiryManager = marketingInquiryManager;
            _marketingInstituteManager = marketingInstituteManager;
        }

         [AjaxAuthorize(Roles = "marketinginquiry-1,marketinginquiry-2,marketinginquiry-3")]
        public ActionResult Index(MarketingInquiryViewModel model)
        {

            ModelState.Clear();
            int totalRecords = 0;
           // model.MarketingInstitute= _marketingInstituteManager.GetAllMarketingtInstitute();
            model.MarketingInquiries = _marketingInquiryManager.GetMarketingInquiry(model.PageIndex, model.sort, model.sortdir, model.SearchString,model.MarketingInquiriy.MarketingPersonId, out totalRecords);
        
            model.MarketingPersons = _marketingInquiryManager.GetMarketingPersons();
            //model.MarketingInquiries = _marketingInquiryManager.GetAllMarketingInquiry();
            model.TotalRecords = totalRecords;
            return View(model);
        }
        [AjaxAuthorize(Roles = "marketinginquiry-2,marketinginquiry-3")]
        public ActionResult Edit(MarketingInquiryViewModel model)
        {
            ModelState.Clear();
            if (model.MarketingInquiriy.InquiryId > 0)
            {
                model.Status = _marketingInquiryManager.GetAllMarketingStatus();
             //   model.MarketingInstitute = _marketingInstituteManager.GetAllMarketingtInstitute();
                model.MarketingInquiriy = _marketingInquiryManager.GetMarketingInquiryById(model.MarketingInquiriy.InquiryId);
            }
            else
            {
                 //model.MarketingInstitute = _marketingInstituteManager.GetNewRefId();
               
                model.Status = _marketingInquiryManager.GetAllMarketingStatus();
                model.MarketingInquiriy.IsActive = true;
            }
            model.MarketingInstitute = _marketingInstituteManager.GetAllMarketingtInstitute();
            model.MarketingPersons = _marketingInquiryManager.GetMarketingPersons();
            return View(model);
        }
            [AjaxAuthorize(Roles = "marketinginquiry-2,marketinginquiry-3")]
        public ActionResult Save(MarketingInquiryViewModel model)
        {
            try
            {
                var saved = model.MarketingInquiriy.InquiryId > 0 ? _marketingInquiryManager.EditMarketingInquiry(model.MarketingInquiriy) : _marketingInquiryManager.SaveMarketingInquiry(model.MarketingInquiriy);
                return saved > 0 ? Reload() : ErrorResult("House Keeping Good Information not save successfully!!");
            }
            catch (Exception e)
            {
                Errorlog.WriteLog(e);
                return ErrorResult(e.Message);
            }

        }

            [AjaxAuthorize(Roles = "marketinginquiry-3")]
        public ActionResult Delete(int inquiryId)
        {
            var deleted = 0;
            var mi = _marketingInquiryManager.GetMarketingInquiryById(inquiryId);
            deleted = _marketingInquiryManager.DeleteMarketingInquiry(mi);
            return (deleted > 0) ? Reload() : ErrorResult("Failed to delete data");
        }
    }
}
