using System;
using System.Web.Mvc;
using SCERP.BLL.IManager.ICommercialManager;
using SCERP.BLL.IManager.ICommonManager;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.Common;
using SCERP.Model.CommercialModel;
using SCERP.Web.Areas.Commercial.Models.ViewModel;
using SCERP.Web.Controllers;

namespace SCERP.Web.Areas.Commercial.Controllers
{
    public class SalesContactController : BaseController
    {
        private readonly ISalesContactManager _salesContactManager;
        private readonly IOmBuyerManager _omBuyerManager;
        private readonly ILcManager _lcManager;
        public SalesContactController(ISalesContactManager salesContactManager, IOmBuyerManager omBuyerManager, ILcManager lcManager)
        {
            _salesContactManager = salesContactManager;
            _omBuyerManager = omBuyerManager;
            _lcManager = lcManager;
        }
        public ActionResult Index(int lcId)
        {
            SalesContactViewModel model=new SalesContactViewModel();
            model.SalseContact.LcId = lcId;
            model.SalseContact.LcDate = DateTime.Now;
            model.SalseContacts = _salesContactManager.GetSalesContacts(lcId);
            model.Buyers = _omBuyerManager.GetAllBuyers();
            model.Banks = _lcManager.GetBankInfo("Receiving");
            return View(model);
        }

        public ActionResult Save(SalesContactViewModel model)
        {
            
            int saved = 0;
            try
            {
                if (model.SalseContact.SalseContactId > 0)
                {

                    model.SalseContact.EditedBy = PortalContext.CurrentUser.UserId;
                    model.SalseContact.EditedDate = DateTime.Now;
                    saved = _salesContactManager.EditSalseContact(model.SalseContact);
                }
                else
                {
                    model.SalseContact.IsActive = true;
                    model.SalseContact.LcType = 1;
                    model.SalseContact.CreatedDate = DateTime.Now;
                    model.SalseContact.CreatedBy = PortalContext.CurrentUser.UserId.GetValueOrDefault();
                    saved = _salesContactManager.SaveSalseContact(model.SalseContact);
                }
            }
            catch (Exception e)
            {
               Errorlog.WriteLog(e);
                return ErrorResult(e.Message);
            }
         
            if (saved>0)
            {

                model.SalseContacts = _salesContactManager.GetSalesContacts(model.SalseContact.LcId);
                return PartialView("~/Areas/Commercial/Views/SalesContact/_List.cshtml", model);
            }
            else
            {
                return ErrorResult("Save Failed!Plse contact with vendor");
            }
          
        }

        public ActionResult Edit(int salseContactId)
        {
            SalesContactViewModel model = new SalesContactViewModel
            {
                Buyers = _omBuyerManager.GetAllBuyers(),
                Banks = _lcManager.GetBankInfo("Receiving"),
                SalseContact = _salesContactManager.GetSalseContactById(salseContactId) ?? new CommSalseContact()
                {
                    LcDate = DateTime.Now
                }
            };
            return PartialView("~/Areas/Commercial/Views/SalesContact/_Edit.cshtml", model);
        }

        public ActionResult Refresh(int lcId)
        {
            SalesContactViewModel model = new SalesContactViewModel
            {
                Buyers = _omBuyerManager.GetAllBuyers(),
                Banks = _lcManager.GetBankInfo("Receiving"),
                SalseContact =
                {
                    LcDate = DateTime.Now,
                    LcId = lcId
                }
            };
            return PartialView("~/Areas/Commercial/Views/SalesContact/_Edit.cshtml", model);
        }

        public ActionResult Delete(int salseContactId,int lcId)
        {
            SalesContactViewModel model=new SalesContactViewModel();
            int delete = _salesContactManager.DeleteSalesContact(salseContactId);
            if (delete > 0)
            {

                model.SalseContacts = _salesContactManager.GetSalesContacts(lcId);
                return PartialView("~/Areas/Commercial/Views/SalesContact/_List.cshtml", model);
            }
            else
            {
                return ErrorResult("Delete Failed!Please contact with vendor");
            }
            
        }
    }
}

