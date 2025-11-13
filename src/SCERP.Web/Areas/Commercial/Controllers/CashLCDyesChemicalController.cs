using SCERP.BLL.IManager.ICommercialManager;
using SCERP.Common;
using SCERP.Model.CommercialModel;
using SCERP.Web.Areas.Commercial.Models.ViewModel;
using SCERP.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SCERP.Web.Areas.Commercial.Controllers
{
    public class CashLCDyesChemicalController : BaseController
    {
        private readonly ICashLCDyesChemicalManager _cashLCDyesChemicalManager;
        private readonly ILcManager _lcManager;

        public CashLCDyesChemicalController(ICashLCDyesChemicalManager cashLCDyesChemicalManager, ILcManager lcManager)
        {
            _cashLCDyesChemicalManager = cashLCDyesChemicalManager;
            _lcManager = lcManager;
        }
        // GET: Commercial/CashLCDyesChemical
        [AjaxAuthorize(Roles = "cashlcdyeschemical-1,cashlcdyeschemical-2,cashlcdyeschemical-3")]
        public ActionResult Index(CashLCDyesChemicalViewModel model)
        {
            ModelState.Clear();
            int totalRecords;
            
            model.Banks = _lcManager.GetBankInfo("Receiving");
            model.CommCashLcs = _cashLCDyesChemicalManager.GetAllCashLcsByPaging(model, out totalRecords,model.CommCashLc.SearchString);
            model.TotalRecords = totalRecords;
            return View(model);
        }
        [AjaxAuthorize(Roles = "cashlcdyeschemical-2,cashlcdyeschemical-3")]
        public ActionResult Edit(CashLCDyesChemicalViewModel model)
        {
            ModelState.Clear();
            try
            {
                model.Banks = _lcManager.GetBankInfo("Receiving");

                if (model.CashLcId > 0)
                {
                    CommCashLCDyesChemical commCashLc = _cashLCDyesChemicalManager.GetCashLcById(model.CashLcId);
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

        [AjaxAuthorize(Roles = "cashlcdyeschemical-2,cashlcdyeschemical-3")]
        public ActionResult Save(CashLCDyesChemicalViewModel model)
        {
            var index = 0;

            try
            {
                bool exist = _cashLCDyesChemicalManager.IsCashLcExist(model.CommCashLc);
                if (!exist)
                {
                    if (model.CommCashLc.CashLcId > 0)
                    {
                        model.CommCashLc.EditedDate = DateTime.Now;
                        model.CommCashLc.EditedBy = PortalContext.CurrentUser.UserId;
                        index = _cashLCDyesChemicalManager.EditCashLc(model.CommCashLc);
                    }
                    else
                    {
                        model.CommCashLc.CreatedDate = DateTime.Now;
                        model.CommCashLc.CreatedBy = PortalContext.CurrentUser.UserId;
                        index = _cashLCDyesChemicalManager.SaveCashLc(model.CommCashLc);
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

        [AjaxAuthorize(Roles = "cashlcdyeschemical-3")]
        public ActionResult Delete(int cashLcId)
        {
            var index = 0;
            try
            {
                index = _cashLCDyesChemicalManager.DeleteCashLc(cashLcId);
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