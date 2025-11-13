using System;
using System.Web.Mvc;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.Common;
using SCERP.Model;
using SCERP.Web.Areas.Merchandising.Models.ViewModel;
using SCERP.Web.Controllers;

namespace SCERP.Web.Areas.Merchandising.Controllers
{
    public class PaymentTermController : BaseController
    {
        private readonly IPaymentTermManager paymentTermManager;
        public PaymentTermController(IPaymentTermManager paymentTermManager)
        {
            this.paymentTermManager = paymentTermManager;
        }
        [AjaxAuthorize(Roles = "paymentterm-1,paymentterm-2,paymentterm-3")]
        public ActionResult Index(PaymentTermViewModel model)
        {
            ModelState.Clear();
            var totalRecords = 0;
            model.OmPaymentTerms = paymentTermManager.GetPaymentTermByPaging(model, out totalRecords);
            model.TotalRecords = totalRecords;
            return View(model);
        }
       [HttpGet]
       [AjaxAuthorize(Roles = "paymentterm-2,paymentterm-3")]
        public ActionResult Edit(PaymentTermViewModel model)
        {
            ModelState.Clear();
            if (model.PayentTermId > 0)
            {
                var paymentTerm = paymentTermManager.GetPaymentTermById(model.PayentTermId);
                model.PayentTermId = paymentTerm.PayentTermId;
                model.CompId = paymentTerm.CompId;
                model.PayTermRefId = paymentTerm.PayTermRefId;
                model.PayTerm = paymentTerm.PayTerm;
                model.ECGCPerc = paymentTerm.ECGCPerc;
                model.InsurancePerc = paymentTerm.InsurancePerc;
                model.CreditDays = paymentTerm.CreditDays;
                model.PayType = paymentTerm.PayType;
           
            }
            else
            {
                model.PayTermRefId = paymentTermManager.GetPayTermRef();
            }

            return View(model);
        }
        [HttpPost]
        [AjaxAuthorize(Roles = "paymentterm-2,paymentterm-3")]
        public ActionResult Save(OM_PaymentTerm model)
        {
            var index = 0;
            var errorMessage = "";
            try
            {
                index = model.PayentTermId > 0 ? paymentTermManager.EditPaymentTerm(model) : paymentTermManager.SavePaymentTerm(model);
            }
            catch (Exception exception)
            {
                errorMessage = exception.Message;
                Errorlog.WriteLog(exception);
            }
            return index > 0 ? Reload() : ErrorResult("Failed to save Payment ! " + errorMessage);

        }
        [AjaxAuthorize(Roles = "paymentterm-3")]
        public ActionResult Delete(OM_PaymentTerm model)
        {

            var deleteIndex = paymentTermManager.DeletePaymentTerm(model.PayTermRefId);
            if (deleteIndex == -1)
            {
                return ErrorResult("Could not possible to delete Payment Term because of it's all ready used in buyer Order");
            }

            return deleteIndex > 0 ? Reload() : ErrorResult("Delete Fail");
        }
        [HttpPost]
        public JsonResult CheckExistingPaymentTerm(OM_PaymentTerm model)
        {
            ModelState.Clear();
            var isExist = !paymentTermManager.CheckExistingPaymentTerm(model);
            return Json(isExist, JsonRequestBehavior.AllowGet);
        }
	}
}