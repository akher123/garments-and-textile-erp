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
    public class CashLcController : BaseController
    {
        private readonly ICashLcManager _cashLcManager;
        private readonly ILcManager _lcManager;
        private readonly ISupplierCompanyManager _supplierCompanyManager;

        public CashLcController(ICashLcManager cashLcManager, ILcManager lcManager, ISupplierCompanyManager supplierCompanyManager)
        {
            _cashLcManager = cashLcManager;
            this._supplierCompanyManager = supplierCompanyManager;
            _lcManager = lcManager;
        }

        [AjaxAuthorize(Roles = "cashlcmachinary-1,cashlmachinary-2,cashlcmachinary-3")]
        public ActionResult Index(CashLcViewModel model)
        {        
            ModelState.Clear();

            IEnumerable lcTypeList = from LcType lcType in Enum.GetValues(typeof(LcType))
                                     select new { Id = (int)lcType, Name = lcType.ToString() };

            var printFormat = from PrintFormatType formatType in Enum.GetValues(typeof(PrintFormatType))
                              select new { Id = (int)formatType, Name = formatType.ToString() };

            model.Suppliers = _supplierCompanyManager.GetAllSupplierCompany();
            model.LcTypes = lcTypeList;
            model.Lcs = _lcManager.GetAllLcInfos();                                    
            model.Banks = _lcManager.GetBankInfo("Receiving");
            model.PrintFormatStatuses = printFormat;

            var totalRecords = 0;
            model.CommCashLcs = _cashLcManager.GetAllCashLcsByPaging(model, out totalRecords, model.CommCashLc.SearchString);
            model.TotalRecords = totalRecords;
            return View(model);
        }


        [AjaxAuthorize(Roles = "cashlcmachinary-2,cashlcmachinary-3")]
        public ActionResult Edit(CashLcViewModel model)
        {
            ModelState.Clear();

            try
            {
                model.Banks = _lcManager.GetBankInfo("Receiving");

                if (model.CashLcId > 0)
                {
                    CommCashLc commCashLc = _cashLcManager.GetCashLcById(model.CashLcId);
                    model.CommCashLc = commCashLc;
                }
                else
                {
                }
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult("" + exception);
            }
            return View(model);
        }

        [AjaxAuthorize(Roles = "cashlcmachinary-2,cashlcmachinary-3")]
        public ActionResult Save(CashLcViewModel model)
        {
            var index = 0;

            try
            {
                bool exist = _cashLcManager.IsCashLcExist(model.CommCashLc);

                if (!exist)
                {
                    if (model.CommCashLc.CashLcId > 0)
                    {
                        model.CommCashLc.EditedDate = DateTime.Now;
                        model.CommCashLc.EditedBy = PortalContext.CurrentUser.UserId;
                        index = _cashLcManager.EditCashLc(model.CommCashLc);
                    }
                    else
                    {
                        model.CommCashLc.CreatedDate = DateTime.Now;
                        model.CommCashLc.CreatedBy = PortalContext.CurrentUser.UserId;
                        index = _cashLcManager.SaveCashLc(model.CommCashLc);
                    }
                }
                else
                {
                    return ErrorResult("Same Information Already Exist ! Please Entry Another One.");
                }

            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult("" + exception);
            }
            return index > 0 ? Reload() : ErrorResult("Fail To Save Task");
        }

        [AjaxAuthorize(Roles = "cashlcmachinary-3")]
        public ActionResult Delete(int cashLcId)
        {
            var index = 0;
            try
            {
                index = _cashLcManager.DeleteCashLc(cashLcId);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult("" + exception);
            }
            return index > 0 ? Reload() : ErrorResult("Fail to Delete Subject");
        }
    }
}